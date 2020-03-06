using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;


namespace PMD_Tabletop_Sheet
{

    public partial class Form_Main : Form
    {
        public bool DynamaxEnabled; public bool MegaEnabled;
        public int PageSelected;
        public int ClkInfliction; public int Clkstun; public int Clkmez; public int Clkdoom; public int Clkmisc1; public int Clkmisc2; public int Clkmisc3;
        public string Savefilepath; public string dbpath; public bool fileio = false;
        private string[] typeList = { "---", "Normal", "Fire", "Water", "Electric", "Grass", "Ice", "Fighting", "Poison", "Ground", "Flying", "Psychic", "Bug", "Rock", "Ghost", "Dragon", "Dark", "Steel", "Fairy" };
        private int[] pkmn_Stats = { 0, 0, 0, 0, 0, 0 }; // HP, ATK, DEF, SATK, SDEF, SPD
        private string pkmn_EXP_Growth = "Slow";
        private double exp_to_lv;
        private string pkmn_gmax_move = "";
        private string pkmn_gmax_type = "";
        private double percent_hp_to_shadow = 0.10;
        public List<string> shadow_moves = new List<string>();
        public Random rng = new Random();

        public List<string> pkmn_battleset_list = new List<string>();
        public List<string> pkmn_battleset_types_list = new List<string>();
        public List<string> pkmn_battleset_atr_list = new List<string>();
        public List<string> pkmn_dynamax_list = new List<string>();
        public List<string> pkmn_moveset_list = new List<string>();
        public List<CheckBox> pkmn_moveset_chk_move_selected = new List<CheckBox>();
        public List<TextBox> pkmn_moveset_txt_move_lv = new List<TextBox>();
        public List<TextBox> pkmn_moveset_txt_move_name = new List<TextBox>();
        public List<ComboBox> pkmn_moveset_cmb_move_type = new List<ComboBox>();
        public List<TextBox> pkmn_moveset_txt_move_pp = new List<TextBox>();
        public List<PictureBox> pkmn_moveset_pbox_move_attr = new List<PictureBox>();
        public CheckBox chk_move_selected_template;
        public TextBox txt_move_lv_template;
        public TextBox txt_move_name_template;
        public ComboBox cmb_move_type_template;
        public TextBox txt_move_pp_template;
        public PictureBox pbox_move_attr_template;

        private System.Drawing.Bitmap rimgs_badge_expedition = Properties.Resources.badge_expedition;
        private System.Drawing.Bitmap rimgs_badge_shimmering_outline = Properties.Resources.badge_shimmering_outline;
        private System.Drawing.Bitmap rimgs_mega_stone_dormant = Properties.Resources.mega_stone_dormant;
        private System.Drawing.Bitmap rimgs_mega_stone_glow = Properties.Resources.mega_stone_glow;
        private System.Drawing.Bitmap rimgs_gmax_glow = Properties.Resources.gmax_glow;
        private System.Drawing.Bitmap rimgs_gmax_plain_dormant = Properties.Resources.gmax_plain_dormant;
        private System.Drawing.Bitmap rimgs_clock_notch_empty = Properties.Resources.clock_notch_empty;
        private System.Drawing.Bitmap rimgs_clock_notch_filled = Properties.Resources.clock_notch_filled;
        private System.Drawing.Bitmap rimgs_icon_Physical_Attack = Properties.Resources.icon_Physical_Attack;
        private System.Drawing.Bitmap rimgs_icon_Special_Attack = Properties.Resources.icon_Special_Attack;
        private System.Drawing.Bitmap rimgs_icon_Status_Attack = Properties.Resources.icon_Status_Attack;
        private System.Drawing.Bitmap rimgs_icon_Physical_Attack_Shadow = Properties.Resources.icon_Physical_Attack_Shadow;
        private System.Drawing.Bitmap rimgs_icon_Special_Attack_Shadow = Properties.Resources.icon_Special_Attack_Shadow;
        private System.Drawing.Bitmap rimgs_icon_Status_Attack_Shadow = Properties.Resources.icon_Status_Attack_Shadow;
        private System.Drawing.Bitmap rimgs_token = Properties.Resources.token;

        public static Control FindControlAtPoint(Control container, System.Drawing.Point pos)
        {
            Control child;
            foreach (Control c in container.Controls)
            {
                if (c.Visible && c.Bounds.Contains(pos))
                {
                    child = FindControlAtPoint(c, new System.Drawing.Point(pos.X - c.Left, pos.Y - c.Top));
                    if (child == null) return c;
                    else if (child.Visible) { return child; }
                }
            }
            return null;
        }

        public static Control FindControlAtCursor(Form form)
        {
            System.Drawing.Point pos = Cursor.Position;
            if (form.Bounds.Contains(pos))
                return FindControlAtPoint(form, form.PointToClient(pos));
            return null;
        }

        public int imageIsEqual(long size, long[] a, long[] b)
        {
            long start = size / 2;
            long i;
            for (i = start; i != size; i++) { if (a[i] != b[i]) return 0; }
            for (i = 0; i != start; i++) { if (a[i] != b[i]) return 0; }
            return 1;
        }

        public Form_Main()
        {
            InitializeComponent();
            dbpath = System.IO.Directory.GetCurrentDirectory();
            dbpath += "\\Stat_References.db";
        }

        private void Form_Main_Load(object sender, EventArgs e)
        {
            DynamaxEnabled = false;
            MegaEnabled = false;
            PageSelected = 1;
            toggleShadowMoveVisible(false);
            loadAllShadowMoves();
            loadBags();
            loadTraits();
            loadNatures();
            recentFilePathRead();

            foreach (string type_item in typeList)
            {
                cmb_sht_type1.Items.Add(type_item);
                cmb_sht_type2.Items.Add(type_item);
                cmb_combat_move_1_type.Items.Add(type_item);
                cmb_combat_move_2_type.Items.Add(type_item);
                cmb_combat_move_3_type.Items.Add(type_item);
                cmb_combat_move_4_type.Items.Add(type_item);
                cmb_moves_char_type1.Items.Add(type_item);
                cmb_moves_char_type2.Items.Add(type_item);
                cmb_combat_move_prev_type.Items.Add(type_item);
            }

            // Create moves template
            chk_move_selected_template = new CheckBox();
            txt_move_lv_template = new TextBox();
            txt_move_name_template = new TextBox();
            cmb_move_type_template = new ComboBox();
            txt_move_pp_template = new TextBox();
            pbox_move_attr_template = new PictureBox();

            chk_move_selected_template.Name = "chk_move_selected_";
            txt_move_lv_template.Name = "txt_move_lv_";
            txt_move_name_template.Name = "txt_move_name_";
            cmb_move_type_template.Name = "cmb_move_type_";
            txt_move_pp_template.Name = "txt_move_pp_";
            pbox_move_attr_template.Name = "pbox_move_attr_";

            chk_move_selected_template.Size = new System.Drawing.Size(15, 14);
            txt_move_lv_template.Size = new System.Drawing.Size(32, 20);
            txt_move_name_template.Size = new System.Drawing.Size(103, 20);
            cmb_move_type_template.Size = new System.Drawing.Size(63, 21);
            txt_move_pp_template.Size = new System.Drawing.Size(25, 20);
            pbox_move_attr_template.Size = new System.Drawing.Size(26, 26);

            chk_move_selected_template.Location = new System.Drawing.Point(48, 4);
            txt_move_lv_template.Location = new System.Drawing.Point(77, 1);
            txt_move_name_template.Location = new System.Drawing.Point(115, 1);
            cmb_move_type_template.Location = new System.Drawing.Point(224, 0);
            txt_move_pp_template.Location = new System.Drawing.Point(294, 1);
            pbox_move_attr_template.Location = new System.Drawing.Point(10, 0);

            txt_move_lv_template.ReadOnly = true;
            txt_move_name_template.ReadOnly = true;
            cmb_move_type_template.Enabled = false;
            txt_move_pp_template.ReadOnly = true;
            pbox_move_attr_template.SizeMode = PictureBoxSizeMode.Zoom;


            foreach (string type_item in typeList)
            {
                cmb_move_type_template.Items.Add(type_item);
            }
            cmb_move_type_template.SelectedIndex = 0;

        }

        private void toggleCampaign(string campaign)
        {
            if (campaign == "se")
            {
                // Starward Express
                ts_campaign_dnde_btn.Checked = false;
                ts_campaign_se_btn.Checked = true;
                pic_sht_badge.Image = rimgs_badge_expedition;
                if (PageSelected == 1)
                {
                    grp_clocks.Visible = false;
                    grp_inf.Visible = false;
                    lbl_stat_fort.Visible = false;
                    txt_stat_fort_user.Visible = false;
                    txt_stat_fort_eff.Visible = false;
                    lbl_stat_fort_max.Visible = false;
                    txt_stat_fort_max.Visible = false;
                    lbl_stat_fort_half.Visible = false;
                    txt_stat_fort_half.Visible = false;
                    lbl_stat_fort_quarter.Visible = false;
                    txt_stat_fort_quarter.Visible = false;
                    lbl_stat_fort_stage.Visible = false;
                    ctr_stat_fort_stage.Visible = false;
                    btn_dynamax.Visible = false;
                    toggleShadowMoveVisible(false);
                    this.Size = new System.Drawing.Size(546, 707);
                }
                else if (PageSelected == 3)
                {
                    if (ts_campaign_se_btn.Checked) { this.Size = new System.Drawing.Size(619, 500); }
                    else if (ts_campaign_dnde_btn.Checked) { this.Size = new System.Drawing.Size(619, 777); }
                }
            }
            else if (campaign == "dnde")
            {
                // Dungeons & Dragonites
                ts_campaign_se_btn.Checked = false;
                ts_campaign_dnde_btn.Checked = true;
                pic_sht_badge.Image = rimgs_badge_shimmering_outline;
                if (PageSelected == 1)
                {
                    grp_clocks.Visible = true;
                    grp_inf.Visible = true;
                    lbl_stat_fort.Visible = true;
                    txt_stat_fort_user.Visible = true;
                    txt_stat_fort_eff.Visible = true;
                    lbl_stat_fort_max.Visible = true;
                    txt_stat_fort_max.Visible = true;
                    lbl_stat_fort_half.Visible = true;
                    txt_stat_fort_half.Visible = true;
                    lbl_stat_fort_quarter.Visible = true;
                    txt_stat_fort_quarter.Visible = true;
                    lbl_stat_fort_stage.Visible = true;
                    ctr_stat_fort_stage.Visible = true;
                    btn_dynamax.Visible = true;
                    toggleShadowMoveVisible(true);
                    this.Size = new System.Drawing.Size(810, 777);
                }
                else if (PageSelected == 3)
                {
                    if (ts_campaign_se_btn.Checked) { this.Size = new System.Drawing.Size(619, 500); }
                    else if (ts_campaign_dnde_btn.Checked) { this.Size = new System.Drawing.Size(619, 777); }
                }
            }
        }

        private void ts_campaign_se_btn_Click(object sender, EventArgs e)
        {
            toggleCampaign("se");
        }

        private void ts_campaign_dnde_btn_Click(object sender, EventArgs e)
        {
            toggleCampaign("dnde");
        }

        private void toggleMega()
        {
            if (MegaEnabled)
            {
                MegaEnabled = false;
                btn_megaevolve.Image = rimgs_mega_stone_dormant;
                if (txt_sht_species.Text.Length > 5 && txt_sht_species.Text.Substring(0, 5) == "Mega ")
                {
                    txt_sht_species.Text = txt_sht_species.Text.Substring(5, txt_sht_species.Text.Length - 5);
                }
            }
            else
            {
                if (DynamaxEnabled)
                {
                    DynamaxEnabled = false;
                    btn_dynamax.Image = rimgs_gmax_plain_dormant;
                }
                if (String.IsNullOrWhiteSpace(txt_sht_ability_effect.Text))
                {
                    MessageBox.Show(txt_sht_species.Text.Substring(5, txt_sht_species.Text.Length - 5) + " has no mega evolution available. If this is an error, contact the developer!");
                    txt_sht_species.Text = txt_sht_species.Text.Substring(5, txt_sht_species.Text.Length - 5);
                }
                else
                {
                    MegaEnabled = true;
                    btn_megaevolve.Image = rimgs_mega_stone_glow;
                }
            }
        }

        private void toggleDynamax()
        {
            if (DynamaxEnabled)
            {
                DynamaxEnabled = false;
                btn_dynamax.Image = rimgs_gmax_plain_dormant;
                if (pkmn_battleset_list.Count > 0) { txt_combat_move_1_name.Text = pkmn_battleset_list[0]; }
                if (pkmn_battleset_list.Count > 1) { txt_combat_move_2_name.Text = pkmn_battleset_list[1]; }
                if (pkmn_battleset_list.Count > 2) { txt_combat_move_3_name.Text = pkmn_battleset_list[2]; }
                if (pkmn_battleset_list.Count > 3) { txt_combat_move_4_name.Text = pkmn_battleset_list[3]; }
                if (Int32.TryParse(txt_stat_hp_max.Text, out int j)) { txt_stat_hp_max.Text = (Int32.Parse(txt_stat_hp_max.Text) / 2).ToString(); }
                if (Int32.TryParse(txt_stat_hp_user.Text, out int k)) { txt_stat_hp_user.Text = (Int32.Parse(txt_stat_hp_user.Text) / 2).ToString(); }
            }
            else
            {
                if (MegaEnabled)
                {
                    toggleMega();
                }
                DynamaxEnabled = true;
                btn_dynamax.Image = rimgs_gmax_glow;
                if (pkmn_dynamax_list.Count > 0) { txt_combat_move_1_name.Text = pkmn_dynamax_list[0]; }
                if (pkmn_dynamax_list.Count > 1) { txt_combat_move_2_name.Text = pkmn_dynamax_list[1]; }
                if (pkmn_dynamax_list.Count > 2) { txt_combat_move_3_name.Text = pkmn_dynamax_list[2]; }
                if (pkmn_dynamax_list.Count > 3) { txt_combat_move_4_name.Text = pkmn_dynamax_list[3]; }
                if (Int32.TryParse(txt_stat_hp_max.Text, out int j)) { txt_stat_hp_max.Text = (Int32.Parse(txt_stat_hp_max.Text) * 2).ToString(); }
                if (Int32.TryParse(txt_stat_hp_user.Text, out int k)) { txt_stat_hp_user.Text = (Int32.Parse(txt_stat_hp_user.Text) * 2).ToString(); }
            }
        }

        private void btn_megaevolve_Click(object sender, EventArgs e)
        {
            if (txt_sht_species.Text.Length <= 5 || txt_sht_species.Text.Substring(0, 5) != "Mega ")
            {
                txt_sht_species.Text = "Mega " + txt_sht_species.Text;
            }
            toggleMega();
        }

        private void btn_dynamax_Click(object sender, EventArgs e)
        {
            toggleDynamax();
        }


        private void ts_page_stats_btn_Click(object sender, EventArgs e)
        {
            PageSelected = 1;
            ts_page_stats_btn.Checked = true;
            ts_page_moves_btn.Checked = false;
            ts_page_inv_btn.Checked = false;
            ts_page_journal_btn.Checked = false;
            pnl_pg_1_stats.Visible = true;
            pnl_pg_2_moves.Visible = false;
            pnl_pg_3_inv.Visible = false;
            pnl_pg_4_journal.Visible = false;
            if (ts_campaign_se_btn.Checked) {
                grp_clocks.Visible = false;
                grp_inf.Visible = false;
                lbl_stat_fort.Visible = false;
                txt_stat_fort_user.Visible = false;
                txt_stat_fort_eff.Visible = false;
                lbl_stat_fort_max.Visible = false;
                txt_stat_fort_max.Visible = false;
                lbl_stat_fort_half.Visible = false;
                txt_stat_fort_half.Visible = false;
                lbl_stat_fort_quarter.Visible = false;
                txt_stat_fort_quarter.Visible = false;
                lbl_stat_fort_stage.Visible = false;
                ctr_stat_fort_stage.Visible = false;
                btn_dynamax.Visible = false;
                toggleShadowMoveVisible(false);
                this.Size = new System.Drawing.Size(546, 707);
            }
            else if (ts_campaign_dnde_btn.Checked) {
                grp_clocks.Visible = true;
                grp_inf.Visible = true;
                lbl_stat_fort.Visible = true;
                txt_stat_fort_user.Visible = true;
                txt_stat_fort_eff.Visible = true;
                lbl_stat_fort_max.Visible = true;
                txt_stat_fort_max.Visible = true;
                lbl_stat_fort_half.Visible = true;
                txt_stat_fort_half.Visible = true;
                lbl_stat_fort_quarter.Visible = true;
                txt_stat_fort_quarter.Visible = true;
                lbl_stat_fort_stage.Visible = true;
                ctr_stat_fort_stage.Visible = true;
                btn_dynamax.Visible = true;
                toggleShadowMoveVisible(true);
                this.Size = new System.Drawing.Size(810, 777);
            }
        }

        private void ts_page_moves_btn_Click(object sender, EventArgs e)
        {
            PageSelected = 2;
            ts_page_stats_btn.Checked = false;
            ts_page_moves_btn.Checked = true;
            ts_page_inv_btn.Checked = false;
            ts_page_journal_btn.Checked = false;
            pnl_pg_1_stats.Visible = false;
            pnl_pg_2_moves.Visible = true;
            pnl_pg_3_inv.Visible = false;
            pnl_pg_4_journal.Visible = false;
            this.Size = new System.Drawing.Size(387, 667);
        }

        private void ts_page_inv_btn_Click(object sender, EventArgs e)
        {
            PageSelected = 3;
            ts_page_stats_btn.Checked = false;
            ts_page_moves_btn.Checked = false;
            ts_page_inv_btn.Checked = true;
            ts_page_journal_btn.Checked = false;
            pnl_pg_1_stats.Visible = false;
            pnl_pg_2_moves.Visible = false;
            pnl_pg_3_inv.Visible = true;
            pnl_pg_4_journal.Visible = false;
            if (ts_campaign_se_btn.Checked) { this.Size = new System.Drawing.Size(619, 500); }
            else if (ts_campaign_dnde_btn.Checked) { this.Size = new System.Drawing.Size(619, 777); }
        }

        private void ts_page_journal_btn_Click(object sender, EventArgs e)
        {
            PageSelected = 4;
            ts_page_stats_btn.Checked = false;
            ts_page_moves_btn.Checked = false;
            ts_page_inv_btn.Checked = false;
            ts_page_journal_btn.Checked = true;
            pnl_pg_1_stats.Visible = false;
            pnl_pg_2_moves.Visible = false;
            pnl_pg_3_inv.Visible = false;
            pnl_pg_4_journal.Visible = true;
            this.Size = new System.Drawing.Size(760, 700);
        }

        private void sanitizeNegativeText(TextBox obj)
        {
            if (!String.IsNullOrWhiteSpace(obj.Text) && obj.Text.Length > 1)
            {
                obj.Text = obj.Text[0] + obj.Text.Substring(1, obj.Text.Length - 1).Replace("-","");
            }
        }

        private void sanitizeSkills(object sender, EventArgs e)
        {
            TextBox[] skills = { txt_skills_gathering, txt_skills_tracking, txt_skills_medicine, txt_skills_nature, txt_skills_deception, txt_skills_persuasion, txt_skills_intimidation, txt_skills_perf, txt_skills_val_other1, txt_skills_val_other2, txt_skills_val_other3, txt_skills_val_other4 };
            foreach (TextBox skill in skills)
            {
                sanitizeNegativeText(skill);
            }
        }

        // Make sure specialized skills can only go negative, unless they are mastered
        private void txt_skills_crafting_TextChanged(object sender, EventArgs e)
        {
            sanitizeNegativeText(txt_skills_crafting);
            if (int.TryParse(txt_skills_crafting.Text, out int j))
            {
                if (!chk_skills_crafting.Checked && int.Parse(txt_skills_crafting.Text) > 0)
                {
                    txt_skills_crafting.Text = txt_skills_crafting.Text.Substring(0, txt_skills_crafting.Text.Length - 1);
                }
            }
            else
            {
                if (txt_skills_crafting.Text.Length > 0 && txt_skills_crafting.Text != "-") { txt_skills_crafting.Text = txt_skills_crafting.Text.Substring(0, txt_skills_crafting.Text.Length - 1); }
            }
        }

        private void txt_skills_app_TextChanged(object sender, EventArgs e)
        {
            sanitizeNegativeText(txt_skills_app);
            if (int.TryParse(txt_skills_app.Text, out int j))
            {
                if (!chk_skills_app.Checked && int.Parse(txt_skills_app.Text) > 0)
                {
                    txt_skills_app.Text = txt_skills_app.Text.Substring(0, txt_skills_app.Text.Length - 1);
                }
            }
            else
            {
                if (txt_skills_app.Text.Length > 0 && txt_skills_app.Text != "-") { txt_skills_app.Text = txt_skills_app.Text.Substring(0, txt_skills_app.Text.Length - 1); }
            }
        }

        private void txt_skills_hist_TextChanged(object sender, EventArgs e)
        {
            sanitizeNegativeText(txt_skills_hist);
            if (int.TryParse(txt_skills_hist.Text, out int j))
            {
                if (!chk_skills_hist.Checked && int.Parse(txt_skills_hist.Text) > 0)
                {
                    txt_skills_hist.Text = txt_skills_hist.Text.Substring(0, txt_skills_hist.Text.Length - 1);
                }
            }
            else
            {
                if (txt_skills_hist.Text.Length > 0 && txt_skills_hist.Text != "-") { txt_skills_hist.Text = txt_skills_hist.Text.Substring(0, txt_skills_hist.Text.Length - 1); }
            }
        }

        private void txt_skills_build_TextChanged(object sender, EventArgs e)
        {
            sanitizeNegativeText(txt_skills_build);
            if (int.TryParse(txt_skills_build.Text, out int j))
            {
                if (!chk_skills_build.Checked && int.Parse(txt_skills_build.Text) > 0)
                {
                    txt_skills_build.Text = txt_skills_build.Text.Substring(0, txt_skills_build.Text.Length - 1);
                }
            }
            else
            {
                if (txt_skills_build.Text.Length > 0 && txt_skills_crafting.Text != "-") { txt_skills_build.Text = txt_skills_build.Text.Substring(0, txt_skills_build.Text.Length - 1); }
            }
        }

        private void textbox_change_numbers_only(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textbox_change_numbers_only_allow_negative(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !(char.IsDigit(e.KeyChar) || e.KeyChar == '-'))
            {
                e.Handled = true;
            }
        }

        private void inv_qty_change()
        {
            ulong curr_inv_qty = 0;  bool valid_qty = true;
            string[] txt_inv_arr = {txt_inv_qty_1.Text, txt_inv_qty_2.Text, txt_inv_qty_3.Text, txt_inv_qty_4.Text, txt_inv_qty_5.Text, txt_inv_qty_6.Text
            , txt_inv_qty_7.Text, txt_inv_qty_8.Text, txt_inv_qty_9.Text, txt_inv_qty_10.Text, txt_inv_qty_11.Text, txt_inv_qty_12.Text, txt_inv_qty_13.Text
            , txt_inv_qty_14.Text, txt_inv_qty_15.Text, txt_inv_qty_16.Text, txt_inv_qty_17.Text, txt_inv_qty_18.Text, txt_inv_qty_19.Text, txt_inv_qty_20.Text
            , txt_inv_qty_21.Text, txt_inv_qty_22.Text, txt_inv_qty_23.Text, txt_inv_qty_24.Text, txt_inv_qty_25.Text, txt_inv_qty_26.Text};
            bool[] chk_inv_arr = {chk_inv_held_1.Checked, chk_inv_held_2.Checked, chk_inv_held_3.Checked, chk_inv_held_4.Checked, chk_inv_held_5.Checked, chk_inv_held_6.Checked
            , chk_inv_held_7.Checked, chk_inv_held_8.Checked, chk_inv_held_9.Checked, chk_inv_held_10.Checked, chk_inv_held_11.Checked, chk_inv_held_12.Checked, chk_inv_held_13.Checked
            , chk_inv_held_14.Checked, chk_inv_held_15.Checked, chk_inv_held_16.Checked, chk_inv_held_17.Checked, chk_inv_held_18.Checked, chk_inv_held_19.Checked, chk_inv_held_20.Checked
            , chk_inv_held_21.Checked, chk_inv_held_22.Checked, chk_inv_held_23.Checked, chk_inv_held_24.Checked, chk_inv_held_25.Checked, chk_inv_held_26.Checked};

            for (int i = 0; i < 26; i++) { 
                valid_qty = true;
                if (txt_inv_arr[i].Length <= 0) { valid_qty = false; }
                foreach (char c in txt_inv_arr[i])
                {
                    if (!char.IsDigit(c))
                    {
                        valid_qty = false;
                        break;
                    }
                }
                if (valid_qty && !chk_inv_arr[i]) { curr_inv_qty += ulong.Parse(txt_inv_arr[i]); }
            }

            txt_inv_curr_qty.Text = curr_inv_qty.ToString();
            if (ulong.TryParse(txt_inv_max_qty.Text, out ulong j) && ulong.Parse(txt_inv_max_qty.Text) < curr_inv_qty) { lbl_inv_curr_qty.ForeColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Crimson); }
            else { lbl_inv_curr_qty.ForeColor = System.Drawing.SystemColors.ControlText; }
        }

        private void clkChange()
        {
            if (ClkInfliction > 9) { btn_clocks_infliction_9.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_infliction_9.Image = rimgs_clock_notch_empty; }
            if (ClkInfliction > 8) { btn_clocks_infliction_8.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_infliction_8.Image = rimgs_clock_notch_empty; }
            if (ClkInfliction > 7) { btn_clocks_infliction_7.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_infliction_7.Image = rimgs_clock_notch_empty; }
            if (ClkInfliction > 6) { btn_clocks_infliction_6.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_infliction_6.Image = rimgs_clock_notch_empty; }
            if (ClkInfliction > 5) { btn_clocks_infliction_5.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_infliction_5.Image = rimgs_clock_notch_empty; }
            if (ClkInfliction > 4) { btn_clocks_infliction_4.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_infliction_4.Image = rimgs_clock_notch_empty; }
            if (ClkInfliction > 3) { btn_clocks_infliction_3.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_infliction_3.Image = rimgs_clock_notch_empty; }
            if (ClkInfliction > 2) { btn_clocks_infliction_2.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_infliction_2.Image = rimgs_clock_notch_empty; }
            if (ClkInfliction > 1) { btn_clocks_infliction_1.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_infliction_1.Image = rimgs_clock_notch_empty; }
            if (ClkInfliction > 0) { btn_clocks_infliction_0.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_infliction_0.Image = rimgs_clock_notch_empty; }

            if (Clkstun > 9) { btn_clocks_stun_9.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_stun_9.Image = rimgs_clock_notch_empty; }
            if (Clkstun > 8) { btn_clocks_stun_8.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_stun_8.Image = rimgs_clock_notch_empty; }
            if (Clkstun > 7) { btn_clocks_stun_7.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_stun_7.Image = rimgs_clock_notch_empty; }
            if (Clkstun > 6) { btn_clocks_stun_6.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_stun_6.Image = rimgs_clock_notch_empty; }
            if (Clkstun > 5) { btn_clocks_stun_5.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_stun_5.Image = rimgs_clock_notch_empty; }
            if (Clkstun > 4) { btn_clocks_stun_4.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_stun_4.Image = rimgs_clock_notch_empty; }
            if (Clkstun > 3) { btn_clocks_stun_3.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_stun_3.Image = rimgs_clock_notch_empty; }
            if (Clkstun > 2) { btn_clocks_stun_2.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_stun_2.Image = rimgs_clock_notch_empty; }
            if (Clkstun > 1) { btn_clocks_stun_1.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_stun_1.Image = rimgs_clock_notch_empty; }
            if (Clkstun > 0) { btn_clocks_stun_0.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_stun_0.Image = rimgs_clock_notch_empty; }

            if (Clkmez > 9) { btn_clocks_mez_9.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_mez_9.Image = rimgs_clock_notch_empty; }
            if (Clkmez > 8) { btn_clocks_mez_8.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_mez_8.Image = rimgs_clock_notch_empty; }
            if (Clkmez > 7) { btn_clocks_mez_7.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_mez_7.Image = rimgs_clock_notch_empty; }
            if (Clkmez > 6) { btn_clocks_mez_6.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_mez_6.Image = rimgs_clock_notch_empty; }
            if (Clkmez > 5) { btn_clocks_mez_5.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_mez_5.Image = rimgs_clock_notch_empty; }
            if (Clkmez > 4) { btn_clocks_mez_4.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_mez_4.Image = rimgs_clock_notch_empty; }
            if (Clkmez > 3) { btn_clocks_mez_3.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_mez_3.Image = rimgs_clock_notch_empty; }
            if (Clkmez > 2) { btn_clocks_mez_2.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_mez_2.Image = rimgs_clock_notch_empty; }
            if (Clkmez > 1) { btn_clocks_mez_1.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_mez_1.Image = rimgs_clock_notch_empty; }
            if (Clkmez > 0) { btn_clocks_mez_0.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_mez_0.Image = rimgs_clock_notch_empty; }

            if (Clkdoom > 9) { btn_clocks_doom_9.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_doom_9.Image = rimgs_clock_notch_empty; }
            if (Clkdoom > 8) { btn_clocks_doom_8.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_doom_8.Image = rimgs_clock_notch_empty; }
            if (Clkdoom > 7) { btn_clocks_doom_7.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_doom_7.Image = rimgs_clock_notch_empty; }
            if (Clkdoom > 6) { btn_clocks_doom_6.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_doom_6.Image = rimgs_clock_notch_empty; }
            if (Clkdoom > 5) { btn_clocks_doom_5.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_doom_5.Image = rimgs_clock_notch_empty; }
            if (Clkdoom > 4) { btn_clocks_doom_4.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_doom_4.Image = rimgs_clock_notch_empty; }
            if (Clkdoom > 3) { btn_clocks_doom_3.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_doom_3.Image = rimgs_clock_notch_empty; }
            if (Clkdoom > 2) { btn_clocks_doom_2.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_doom_2.Image = rimgs_clock_notch_empty; }
            if (Clkdoom > 1) { btn_clocks_doom_1.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_doom_1.Image = rimgs_clock_notch_empty; }
            if (Clkdoom > 0) { btn_clocks_doom_0.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_doom_0.Image = rimgs_clock_notch_empty; }

            if (Clkmisc1 > 9) { btn_clocks_misc1_9.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_misc1_9.Image = rimgs_clock_notch_empty; }
            if (Clkmisc1 > 8) { btn_clocks_misc1_8.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_misc1_8.Image = rimgs_clock_notch_empty; }
            if (Clkmisc1 > 7) { btn_clocks_misc1_7.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_misc1_7.Image = rimgs_clock_notch_empty; }
            if (Clkmisc1 > 6) { btn_clocks_misc1_6.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_misc1_6.Image = rimgs_clock_notch_empty; }
            if (Clkmisc1 > 5) { btn_clocks_misc1_5.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_misc1_5.Image = rimgs_clock_notch_empty; }
            if (Clkmisc1 > 4) { btn_clocks_misc1_4.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_misc1_4.Image = rimgs_clock_notch_empty; }
            if (Clkmisc1 > 3) { btn_clocks_misc1_3.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_misc1_3.Image = rimgs_clock_notch_empty; }
            if (Clkmisc1 > 2) { btn_clocks_misc1_2.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_misc1_2.Image = rimgs_clock_notch_empty; }
            if (Clkmisc1 > 1) { btn_clocks_misc1_1.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_misc1_1.Image = rimgs_clock_notch_empty; }
            if (Clkmisc1 > 0) { btn_clocks_misc1_0.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_misc1_0.Image = rimgs_clock_notch_empty; }

            if (Clkmisc2 > 9) { btn_clocks_misc2_9.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_misc2_9.Image = rimgs_clock_notch_empty; }
            if (Clkmisc2 > 8) { btn_clocks_misc2_8.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_misc2_8.Image = rimgs_clock_notch_empty; }
            if (Clkmisc2 > 7) { btn_clocks_misc2_7.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_misc2_7.Image = rimgs_clock_notch_empty; }
            if (Clkmisc2 > 6) { btn_clocks_misc2_6.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_misc2_6.Image = rimgs_clock_notch_empty; }
            if (Clkmisc2 > 5) { btn_clocks_misc2_5.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_misc2_5.Image = rimgs_clock_notch_empty; }
            if (Clkmisc2 > 4) { btn_clocks_misc2_4.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_misc2_4.Image = rimgs_clock_notch_empty; }
            if (Clkmisc2 > 3) { btn_clocks_misc2_3.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_misc2_3.Image = rimgs_clock_notch_empty; }
            if (Clkmisc2 > 2) { btn_clocks_misc2_2.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_misc2_2.Image = rimgs_clock_notch_empty; }
            if (Clkmisc2 > 1) { btn_clocks_misc2_1.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_misc2_1.Image = rimgs_clock_notch_empty; }
            if (Clkmisc2 > 0) { btn_clocks_misc2_0.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_misc2_0.Image = rimgs_clock_notch_empty; }

            if (Clkmisc3 > 9) { btn_clocks_misc3_9.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_misc3_9.Image = rimgs_clock_notch_empty; }
            if (Clkmisc3 > 8) { btn_clocks_misc3_8.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_misc3_8.Image = rimgs_clock_notch_empty; }
            if (Clkmisc3 > 7) { btn_clocks_misc3_7.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_misc3_7.Image = rimgs_clock_notch_empty; }
            if (Clkmisc3 > 6) { btn_clocks_misc3_6.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_misc3_6.Image = rimgs_clock_notch_empty; }
            if (Clkmisc3 > 5) { btn_clocks_misc3_5.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_misc3_5.Image = rimgs_clock_notch_empty; }
            if (Clkmisc3 > 4) { btn_clocks_misc3_4.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_misc3_4.Image = rimgs_clock_notch_empty; }
            if (Clkmisc3 > 3) { btn_clocks_misc3_3.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_misc3_3.Image = rimgs_clock_notch_empty; }
            if (Clkmisc3 > 2) { btn_clocks_misc3_2.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_misc3_2.Image = rimgs_clock_notch_empty; }
            if (Clkmisc3 > 1) { btn_clocks_misc3_1.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_misc3_1.Image = rimgs_clock_notch_empty; }
            if (Clkmisc3 > 0) { btn_clocks_misc3_0.Image = rimgs_clock_notch_filled; }
            else { btn_clocks_misc3_0.Image = rimgs_clock_notch_empty; }
        }

        private void btn_clocks_infliction_0_Click(object sender, EventArgs e)
        {
            if (ClkInfliction == 1) {ClkInfliction = 0; clkChange();}
            else {ClkInfliction = 1; clkChange();}
        }

        private void btn_clocks_infliction_1_Click(object sender, EventArgs e)
        {
            if (ClkInfliction == 2) { ClkInfliction = 1; clkChange(); }
            else { ClkInfliction = 2; clkChange(); }
        }

        private void btn_clocks_infliction_2_Click(object sender, EventArgs e)
        {
            if (ClkInfliction == 3) { ClkInfliction = 2; clkChange(); }
            else { ClkInfliction = 3; clkChange(); }
        }

        private void btn_clocks_infliction_3_Click(object sender, EventArgs e)
        {
            if (ClkInfliction == 4) { ClkInfliction = 3; clkChange(); }
            else { ClkInfliction = 4; clkChange(); }
        }

        private void btn_clocks_infliction_4_Click(object sender, EventArgs e)
        {
            if (ClkInfliction == 5) { ClkInfliction = 4; clkChange(); }
            else { ClkInfliction = 5; clkChange(); }
        }

        private void btn_clocks_infliction_5_Click(object sender, EventArgs e)
        {
            if (ClkInfliction == 6) { ClkInfliction = 5; clkChange(); }
            else { ClkInfliction = 6; clkChange(); }
        }

        private void btn_clocks_infliction_6_Click(object sender, EventArgs e)
        {
            if (ClkInfliction == 7) { ClkInfliction = 6; clkChange(); }
            else { ClkInfliction = 7; clkChange(); }
        }

        private void btn_clocks_infliction_7_Click(object sender, EventArgs e)
        {
            if (ClkInfliction == 8) { ClkInfliction = 7; clkChange(); }
            else { ClkInfliction = 8; clkChange(); }
        }

        private void btn_clocks_infliction_8_Click(object sender, EventArgs e)
        {
            if (ClkInfliction == 9) { ClkInfliction = 8; clkChange(); }
            else { ClkInfliction = 9; clkChange(); }
        }

        private void btn_clocks_infliction_9_Click(object sender, EventArgs e)
        {
            if (ClkInfliction == 10) { ClkInfliction = 9; clkChange(); }
            else { ClkInfliction = 10; clkChange(); }
        }

        private void btn_clocks_stun_0_Click(object sender, EventArgs e)
        {
            if (Clkstun == 1) { Clkstun = 0; clkChange(); }
            else { Clkstun = 1; clkChange(); }
        }

        private void btn_clocks_stun_1_Click(object sender, EventArgs e)
        {
            if (Clkstun == 2) { Clkstun = 1; clkChange(); }
            else { Clkstun = 2; clkChange(); }
        }

        private void btn_clocks_stun_2_Click(object sender, EventArgs e)
        {
            if (Clkstun == 3) { Clkstun = 2; clkChange(); }
            else { Clkstun = 3; clkChange(); }
        }

        private void btn_clocks_stun_3_Click(object sender, EventArgs e)
        {
            if (Clkstun == 4) { Clkstun = 3; clkChange(); }
            else { Clkstun = 4; clkChange(); }
        }

        private void btn_clocks_stun_4_Click(object sender, EventArgs e)
        {
            if (Clkstun == 5) { Clkstun = 4; clkChange(); }
            else { Clkstun = 5; clkChange(); }
        }

        private void btn_clocks_stun_5_Click(object sender, EventArgs e)
        {
            if (Clkstun == 6) { Clkstun = 5; clkChange(); }
            else { Clkstun = 6; clkChange(); }
        }

        private void btn_clocks_stun_6_Click(object sender, EventArgs e)
        {
            if (Clkstun == 7) { Clkstun = 6; clkChange(); }
            else { Clkstun = 7; clkChange(); }
        }

        private void btn_clocks_stun_7_Click(object sender, EventArgs e)
        {
            if (Clkstun == 8) { Clkstun = 7; clkChange(); }
            else { Clkstun = 8; clkChange(); }
        }

        private void btn_clocks_stun_8_Click(object sender, EventArgs e)
        {
            if (Clkstun == 9) { Clkstun = 8; clkChange(); }
            else { Clkstun = 9; clkChange(); }
        }

        private void btn_clocks_stun_9_Click(object sender, EventArgs e)
        {
            if (Clkstun == 10) { Clkstun = 9; clkChange(); }
            else { Clkstun = 10; clkChange(); }
        }

        private void btn_clocks_mez_0_Click(object sender, EventArgs e)
        {
            if (Clkmez == 1) { Clkmez = 0; clkChange(); }
            else { Clkmez = 1; clkChange(); }
        }

        private void btn_clocks_mez_1_Click(object sender, EventArgs e)
        {
            if (Clkmez == 2) { Clkmez = 1; clkChange(); }
            else { Clkmez = 2; clkChange(); }
        }

        private void btn_clocks_mez_2_Click(object sender, EventArgs e)
        {
            if (Clkmez == 3) { Clkmez = 2; clkChange(); }
            else { Clkmez = 3; clkChange(); }
        }

        private void btn_clocks_mez_3_Click(object sender, EventArgs e)
        {
            if (Clkmez == 4) { Clkmez = 3; clkChange(); }
            else { Clkmez = 4; clkChange(); }
        }

        private void btn_clocks_mez_4_Click(object sender, EventArgs e)
        {
            if (Clkmez == 5) { Clkmez = 4; clkChange(); }
            else { Clkmez = 5; clkChange(); }
        }

        private void btn_clocks_mez_5_Click(object sender, EventArgs e)
        {
            if (Clkmez == 6) { Clkmez = 5; clkChange(); }
            else { Clkmez = 6; clkChange(); }
        }

        private void btn_clocks_mez_6_Click(object sender, EventArgs e)
        {
            if (Clkmez == 7) { Clkmez = 6; clkChange(); }
            else { Clkmez = 7; clkChange(); }
        }

        private void btn_clocks_mez_7_Click(object sender, EventArgs e)
        {
            if (Clkmez == 8) { Clkmez = 7; clkChange(); }
            else { Clkmez = 8; clkChange(); }
        }

        private void btn_clocks_mez_8_Click(object sender, EventArgs e)
        {
            if (Clkmez == 9) { Clkmez = 8; clkChange(); }
            else { Clkmez = 9; clkChange(); }
        }

        private void btn_clocks_mez_9_Click(object sender, EventArgs e)
        {
            if (Clkmez == 10) { Clkmez = 9; clkChange(); }
            else { Clkmez = 10; clkChange(); }
        }

        private void btn_clocks_doom_0_Click(object sender, EventArgs e)
        {
            if (Clkdoom == 1) { Clkdoom = 0; clkChange(); }
            else { Clkdoom = 1; clkChange(); }
        }

        private void btn_clocks_doom_1_Click(object sender, EventArgs e)
        {
            if (Clkdoom == 2) { Clkdoom = 1; clkChange(); }
            else { Clkdoom = 2; clkChange(); }
        }

        private void btn_clocks_doom_2_Click(object sender, EventArgs e)
        {
            if (Clkdoom == 3) { Clkdoom = 2; clkChange(); }
            else { Clkdoom = 3; clkChange(); }
        }

        private void btn_clocks_doom_3_Click(object sender, EventArgs e)
        {
            if (Clkdoom == 4) { Clkdoom = 3; clkChange(); }
            else { Clkdoom = 4; clkChange(); }
        }

        private void btn_clocks_doom_4_Click(object sender, EventArgs e)
        {
            if (Clkdoom == 5) { Clkdoom = 4; clkChange(); }
            else { Clkdoom = 5; clkChange(); }
        }

        private void btn_clocks_doom_5_Click(object sender, EventArgs e)
        {
            if (Clkdoom == 6) { Clkdoom = 5; clkChange(); }
            else { Clkdoom = 6; clkChange(); }
        }

        private void btn_clocks_doom_6_Click(object sender, EventArgs e)
        {
            if (Clkdoom == 7) { Clkdoom = 6; clkChange(); }
            else { Clkdoom = 7; clkChange(); }
        }

        private void btn_clocks_doom_7_Click(object sender, EventArgs e)
        {
            if (Clkdoom == 8) { Clkdoom = 7; clkChange(); }
            else { Clkdoom = 8; clkChange(); }
        }

        private void btn_clocks_doom_8_Click(object sender, EventArgs e)
        {
            if (Clkdoom == 9) { Clkdoom = 8; clkChange(); }
            else { Clkdoom = 9; clkChange(); }
        }

        private void btn_clocks_doom_9_Click(object sender, EventArgs e)
        {
            if (Clkdoom == 10) { Clkdoom = 9; clkChange(); }
            else { Clkdoom = 10; clkChange(); }
        }

        private void btn_clocks_misc1_0_Click(object sender, EventArgs e)
        {
            if (Clkmisc1 == 1) { Clkmisc1 = 0; clkChange(); }
            else { Clkmisc1 = 1; clkChange(); }
        }

        private void btn_clocks_misc1_1_Click(object sender, EventArgs e)
        {
            if (Clkmisc1 == 2) { Clkmisc1 = 1; clkChange(); }
            else { Clkmisc1 = 2; clkChange(); }
        }

        private void btn_clocks_misc1_2_Click(object sender, EventArgs e)
        {
            if (Clkmisc1 == 3) { Clkmisc1 = 2; clkChange(); }
            else { Clkmisc1 = 3; clkChange(); }
        }

        private void btn_clocks_misc1_3_Click(object sender, EventArgs e)
        {
            if (Clkmisc1 == 4) { Clkmisc1 = 3; clkChange(); }
            else { Clkmisc1 = 4; clkChange(); }
        }

        private void btn_clocks_misc1_4_Click(object sender, EventArgs e)
        {
            if (Clkmisc1 == 5) { Clkmisc1 = 4; clkChange(); }
            else { Clkmisc1 = 5; clkChange(); }
        }

        private void btn_clocks_misc1_5_Click(object sender, EventArgs e)
        {
            if (Clkmisc1 == 6) { Clkmisc1 = 5; clkChange(); }
            else { Clkmisc1 = 6; clkChange(); }
        }

        private void btn_clocks_misc1_6_Click(object sender, EventArgs e)
        {
            if (Clkmisc1 == 7) { Clkmisc1 = 6; clkChange(); }
            else { Clkmisc1 = 7; clkChange(); }
        }

        private void btn_clocks_misc1_7_Click(object sender, EventArgs e)
        {
            if (Clkmisc1 == 8) { Clkmisc1 = 7; clkChange(); }
            else { Clkmisc1 = 8; clkChange(); }
        }

        private void btn_clocks_misc1_8_Click(object sender, EventArgs e)
        {
            if (Clkmisc1 == 9) { Clkmisc1 = 8; clkChange(); }
            else { Clkmisc1 = 9; clkChange(); }
        }

        private void btn_clocks_misc1_9_Click(object sender, EventArgs e)
        {
            if (Clkmisc1 == 10) { Clkmisc1 = 9; clkChange(); }
            else { Clkmisc1 = 10; clkChange(); }
        }

        private void btn_clocks_misc2_0_Click(object sender, EventArgs e)
        {
            if (Clkmisc2 == 1) { Clkmisc2 = 0; clkChange(); }
            else { Clkmisc2 = 1; clkChange(); }
        }

        private void btn_clocks_misc2_1_Click(object sender, EventArgs e)
        {
            if (Clkmisc2 == 2) { Clkmisc2 = 1; clkChange(); }
            else { Clkmisc2 = 2; clkChange(); }
        }

        private void btn_clocks_misc2_2_Click(object sender, EventArgs e)
        {
            if (Clkmisc2 == 3) { Clkmisc2 = 2; clkChange(); }
            else { Clkmisc2 = 3; clkChange(); }
        }

        private void btn_clocks_misc2_3_Click(object sender, EventArgs e)
        {
            if (Clkmisc2 == 4) { Clkmisc2 = 3; clkChange(); }
            else { Clkmisc2 = 4; clkChange(); }
        }

        private void btn_clocks_misc2_4_Click(object sender, EventArgs e)
        {
            if (Clkmisc2 == 5) { Clkmisc2 = 4; clkChange(); }
            else { Clkmisc2 = 5; clkChange(); }
        }

        private void btn_clocks_misc2_5_Click(object sender, EventArgs e)
        {
            if (Clkmisc2 == 6) { Clkmisc2 = 5; clkChange(); }
            else { Clkmisc2 = 6; clkChange(); }
        }

        private void btn_clocks_misc2_6_Click(object sender, EventArgs e)
        {
            if (Clkmisc2 == 7) { Clkmisc2 = 6; clkChange(); }
            else { Clkmisc2 = 7; clkChange(); }
        }

        private void btn_clocks_misc2_7_Click(object sender, EventArgs e)
        {
            if (Clkmisc2 == 8) { Clkmisc2 = 7; clkChange(); }
            else { Clkmisc2 = 8; clkChange(); }
        }

        private void btn_clocks_misc2_8_Click(object sender, EventArgs e)
        {
            if (Clkmisc2 == 9) { Clkmisc2 = 8; clkChange(); }
            else { Clkmisc2 = 9; clkChange(); }
        }

        private void btn_clocks_misc2_9_Click(object sender, EventArgs e)
        {
            if (Clkmisc2 == 10) { Clkmisc2 = 9; clkChange(); }
            else { Clkmisc2 = 10; clkChange(); }
        }

        private void btn_clocks_misc3_0_Click(object sender, EventArgs e)
        {
            if (Clkmisc3 == 1) { Clkmisc3 = 0; clkChange(); }
            else { Clkmisc3 = 1; clkChange(); }
        }

        private void btn_clocks_misc3_1_Click(object sender, EventArgs e)
        {
            if (Clkmisc3 == 2) { Clkmisc3 = 1; clkChange(); }
            else { Clkmisc3 = 2; clkChange(); }
        }

        private void btn_clocks_misc3_2_Click(object sender, EventArgs e)
        {
            if (Clkmisc3 == 3) { Clkmisc3 = 2; clkChange(); }
            else { Clkmisc3 = 3; clkChange(); }
        }

        private void btn_clocks_misc3_3_Click(object sender, EventArgs e)
        {
            if (Clkmisc3 == 4) { Clkmisc3 = 3; clkChange(); }
            else { Clkmisc3 = 4; clkChange(); }
        }

        private void btn_clocks_misc3_4_Click(object sender, EventArgs e)
        {
            if (Clkmisc3 == 5) { Clkmisc3 = 4; clkChange(); }
            else { Clkmisc3 = 5; clkChange(); }
        }

        private void btn_clocks_misc3_5_Click(object sender, EventArgs e)
        {
            if (Clkmisc3 == 6) { Clkmisc3 = 5; clkChange(); }
            else { Clkmisc3 = 6; clkChange(); }
        }

        private void btn_clocks_misc3_6_Click(object sender, EventArgs e)
        {
            if (Clkmisc3 == 7) { Clkmisc3 = 6; clkChange(); }
            else { Clkmisc3 = 7; clkChange(); }
        }

        private void btn_clocks_misc3_7_Click(object sender, EventArgs e)
        {
            if (Clkmisc3 == 8) { Clkmisc3 = 7; clkChange(); }
            else { Clkmisc3 = 8; clkChange(); }
        }

        private void btn_clocks_misc3_8_Click(object sender, EventArgs e)
        {
            if (Clkmisc3 == 9) { Clkmisc3 = 8; clkChange(); }
            else { Clkmisc3 = 9; clkChange(); }
        }

        private void btn_clocks_misc3_9_Click(object sender, EventArgs e)
        {
            if (Clkmisc3 == 10) { Clkmisc3 = 9; clkChange(); }
            else { Clkmisc3 = 10; clkChange(); }
        }

        private void txt_qty_KeyPress(object sender, KeyPressEventArgs e)
        {
            textbox_change_numbers_only(sender, e);
        }

        private void txt_qty_allow_negative_KeyPress(object sender, KeyPressEventArgs e)
        {
            textbox_change_numbers_only_allow_negative(sender, e);
        }

        private void txt_inv_qty_TextChanged(object sender, EventArgs e)
        {
            inv_qty_change();
        }

        private double calculateEXPtoLv(int inp_Lv)
        {
            double exp_calc = -1;
            if (pkmn_EXP_Growth == "Erratic")
            {
                //EXP = (n^3(100 - n)) / 50 where n<=50
                //EXP = (n^3(150 - n)) / 100 where 50<n<=68
                //EXP = (n^3((1911-10n)/3) / 500 where 68<n<=98
                //EXP = (n^3(160 - n)) / 100 where 98<n<=100
                if (inp_Lv <= 50)
                {
                    exp_calc = (Math.Pow(inp_Lv, 3) * (100 - inp_Lv)) / 50;
                }
                else if (inp_Lv <= 68)
                {
                    exp_calc = (Math.Pow(inp_Lv, 3) * (150 - inp_Lv)) / 100;
                }
                else if (inp_Lv <= 98)
                {
                    exp_calc = (Math.Pow(inp_Lv, 3) * ((1911 - (10 * inp_Lv)) / 3)) / 500;
                }
                else if (inp_Lv <= 100)
                {
                    exp_calc = (Math.Pow(inp_Lv, 3) * (160 - inp_Lv)) / 100;
                }
            }
            else if (pkmn_EXP_Growth == "Fast")
            {
                //EXP = (4n^3) / 5
                exp_calc = (4 * Math.Pow(inp_Lv, 3)) / 5;
            }
            else if (pkmn_EXP_Growth == "Medium Fast")
            {
                //EXP = n^3
                exp_calc = Math.Pow(inp_Lv, 3);
            }
            else if (pkmn_EXP_Growth == "Medium Slow")
            {
                //EXP = (6/5)n^3 - 15n^2 + 100n - 140
                exp_calc = ((6 / 5) * Math.Pow(inp_Lv, 3)) - (15 * Math.Pow(inp_Lv, 2)) + (100 * inp_Lv) - 140;
            }
            else if (pkmn_EXP_Growth == "Slow")
            {
                //EXP = (5n^3) / 4
                exp_calc = (5 * Math.Pow(inp_Lv, 3)) / 4;
            }
            else if (pkmn_EXP_Growth == "Fluctuating")
            {
                //EXP = n^3*(((((n+1)/3)+24) / 50) where n<=15
                //EXP = n^3*((n+14)/50) where 15<n<=36
                //EXP = n^3*(((n/2)+32)/50) where 36<n<=100
                if (inp_Lv <= 15)
                {
                    exp_calc = Math.Pow(inp_Lv, 3) * ((((inp_Lv + 1) / 3) + 24) / 50);
                }
                else if (inp_Lv <= 36)
                {
                    exp_calc = Math.Pow(inp_Lv, 3) * ((inp_Lv + 14) / 50);
                }
                else if (inp_Lv <= 100)
                {
                    exp_calc = Math.Pow(inp_Lv, 3) * (((inp_Lv / 2) + 32) / 50);
                }
            }
            return exp_calc;
        }

        private void handleEXP(bool lvChanged)
        {
            bool expIsNumeric = true; bool lvIsNumeric = true;
            if (String.IsNullOrWhiteSpace(txt_sht_exp.Text)) { expIsNumeric = false; }
            if (String.IsNullOrWhiteSpace(txt_sht_lv.Text)) { lvIsNumeric = false; }
            foreach (char c in txt_sht_exp.Text)
            {
                if (!char.IsDigit(c))
                {
                    expIsNumeric = false;
                    break;
                }
            }
            foreach (char c in txt_sht_lv.Text)
            {
                if (!char.IsDigit(c))
                {
                    lvIsNumeric = false;
                    break;
                }
            }
            if ((expIsNumeric && lvIsNumeric) || (lvChanged && lvIsNumeric))
            {
                exp_to_lv = calculateEXPtoLv(int.Parse(txt_sht_lv.Text)+1);
                tooltip_1.SetToolTip(txt_sht_lv, "EXP to Level: " + exp_to_lv.ToString());
                tooltip_1.SetToolTip(txt_sht_exp, "EXP to Level: " + exp_to_lv.ToString());
                tooltip_1.SetToolTip(lbl_sht_lv, "EXP to Level: " + exp_to_lv.ToString());
                tooltip_1.SetToolTip(lbl_sht_exp, "EXP to Level: " + exp_to_lv.ToString());
                if (lvChanged) {
                    if (!expIsNumeric || long.Parse(txt_sht_exp.Text) < calculateEXPtoLv(int.Parse(txt_sht_lv.Text)))
                    {
                        txt_sht_exp.Text = Math.Floor(calculateEXPtoLv(int.Parse(txt_sht_lv.Text))).ToString();
                    }
                }
                if (long.Parse(txt_sht_exp.Text) >= exp_to_lv) { txt_sht_exp.BackColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Yellow); }
                else if (long.Parse(txt_sht_exp.Text) < calculateEXPtoLv(int.Parse(txt_sht_lv.Text))) { txt_sht_exp.BackColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Crimson);}
                else { txt_sht_exp.BackColor = System.Drawing.SystemColors.Window; }
            }
            else { txt_sht_exp.BackColor = System.Drawing.SystemColors.Window; }
        }

        private int calculateEffStat(int statid, int currstat, decimal stage = 0)
        {
            //statid = {HP, ATK, DEF, SATK, SDEF, SPD, EVA, FORT}
            int effstat = 0; double effstat_calc = 0;
            //if (stage > 6 || stage < 6) { MessageBox.Show("Invalid stage entered. Defaulting to zero."); return currstat; }
            if (statid < 6)
            {
                if (stage == 0) { effstat_calc = currstat; }
                else if (stage < 0) { effstat_calc = currstat * (2 / (2 + Math.Abs(Decimal.ToDouble(stage)))); }
                else if (stage > 0) { effstat_calc = currstat * ((2 + Math.Abs(Decimal.ToDouble(stage))) / 2); }
            }
            else
            {
                if (stage == 0) { effstat_calc = currstat; }
                else if (stage < 0) { effstat_calc = currstat * (3 / (3 + Math.Abs(Decimal.ToDouble(stage)))); }
                else if (stage > 0) { effstat_calc = currstat * ((3 + Math.Abs(Decimal.ToDouble(stage))) / 3); }
            }
            effstat = Convert.ToInt32(Math.Floor(effstat_calc));
            return effstat;
        }

        private int calculateMaxStat(int statid, int basestat, int lv)
        {
            //statid = {HP, ATK, DEF, SATK, SDEF, SPD}
            int maxstat = 0; double maxstat_calc = 0;
            if (statid == 0)
            {
                //HP = (((2 * basestat) * lv) / 100) + lv + 10
                maxstat_calc = (((2 * basestat) * lv) / 100) + lv + 10;
            }
            else
            {
                // Stat = (((2 * basestat) * lv) / 100) + 5
                maxstat_calc = (((2 * basestat) * lv) / 100) + 5;
            }
            maxstat = Convert.ToInt32(Math.Floor(maxstat_calc));
            return maxstat;
        }

        private void calculateAllEffStats()
        {
            bool lvIsNumeric = true; bool userStatIsNumeric = true; string userStatText;
            TextBox[] userStatTextArr = { txt_stat_hp_user, txt_stat_atk_user, txt_stat_def_user, txt_stat_satk_user, txt_stat_sdef_user, txt_stat_spd_user, txt_stat_eva_user, txt_stat_fort_user };
            TextBox[] effStatTextArr = { txt_stat_hp_eff, txt_stat_atk_eff, txt_stat_def_eff, txt_stat_satk_eff, txt_stat_sdef_eff, txt_stat_spd_eff, txt_stat_eva_eff, txt_stat_fort_eff };
            TextBox[] halfStatTextArr = { txt_stat_hp_half, txt_stat_atk_half, txt_stat_def_half, txt_stat_satk_half, txt_stat_sdef_half, txt_stat_spd_half, txt_stat_eva_half, txt_stat_fort_half };
            TextBox[] quarterStatTextArr = { txt_stat_hp_quarter, txt_stat_atk_quarter, txt_stat_def_quarter, txt_stat_satk_quarter, txt_stat_sdef_quarter, txt_stat_spd_quarter, txt_stat_eva_quarter, txt_stat_fort_quarter };
            NumericUpDown[] userStageArr = { ctr_stat_hp_stage, ctr_stat_atk_stage, ctr_stat_def_stage, ctr_stat_satk_stage, ctr_stat_sdef_stage, ctr_stat_spd_stage, ctr_stat_eva_stage, ctr_stat_fort_stage };
            if (String.IsNullOrWhiteSpace(txt_sht_lv.Text)) { lvIsNumeric = false; }
            foreach (char c in txt_sht_lv.Text)
            {
                if (!char.IsDigit(c))
                {
                    lvIsNumeric = false;
                    break;
                }
            }
            for (int i = 0; i < userStatTextArr.Length; i++)
            {
                userStatText = userStatTextArr[i].Text;
                userStatIsNumeric = true;
                //effStatTextArr[i].Text = userStatTextArr[i].Text;
                if (String.IsNullOrWhiteSpace(userStatText)) { userStatIsNumeric = false; }
                foreach (char c in userStatText)
                {
                    if (!char.IsDigit(c))
                    {
                        userStatIsNumeric = false;
                        break;
                    }
                }
                if (lvIsNumeric && userStatIsNumeric)
                {
                    effStatTextArr[i].Text = calculateEffStat(i, int.Parse(userStatTextArr[i].Text), userStageArr[i].Value).ToString();
                    halfStatTextArr[i].Text = Math.Floor(calculateEffStat(i, int.Parse(userStatTextArr[i].Text), userStageArr[i].Value) / 2.0).ToString();
                    quarterStatTextArr[i].Text = Math.Floor(calculateEffStat(i, int.Parse(userStatTextArr[i].Text), userStageArr[i].Value) / 4.0).ToString();
                }
            }
        }

        private void calculateAllMaxStats()
        {
            bool lvIsNumeric = true;
            if (String.IsNullOrWhiteSpace(txt_sht_lv.Text)) { lvIsNumeric = false; }
            foreach (char c in txt_sht_lv.Text)
            {
                if (!char.IsDigit(c))
                {
                    lvIsNumeric = false;
                    break;
                }
            }
            if (lvIsNumeric)
            {
                txt_stat_hp_max.Text = calculateMaxStat(0, pkmn_Stats[0], int.Parse(txt_sht_lv.Text)).ToString();
                txt_stat_atk_max.Text = calculateMaxStat(1, pkmn_Stats[1], int.Parse(txt_sht_lv.Text)).ToString();
                txt_stat_def_max.Text = calculateMaxStat(2, pkmn_Stats[2], int.Parse(txt_sht_lv.Text)).ToString();
                txt_stat_satk_max.Text = calculateMaxStat(3, pkmn_Stats[3], int.Parse(txt_sht_lv.Text)).ToString();
                txt_stat_sdef_max.Text = calculateMaxStat(4, pkmn_Stats[4], int.Parse(txt_sht_lv.Text)).ToString();
                txt_stat_spd_max.Text = calculateMaxStat(5, pkmn_Stats[5], int.Parse(txt_sht_lv.Text)).ToString();
                txt_stat_eva_max.Text =
                    Math.Floor((0.25*int.Parse(txt_stat_def_max.Text)) + (0.25 * int.Parse(txt_stat_sdef_max.Text)) + (0.25 * int.Parse(txt_stat_spd_max.Text))).ToString();
                txt_stat_fort_max.Text =
                    Math.Floor((0.3333 * int.Parse(txt_stat_def_max.Text)) + (0.3333 * int.Parse(txt_stat_sdef_max.Text))).ToString();
                if (DynamaxEnabled && Int32.TryParse(txt_stat_hp_max.Text, out int j)) { txt_stat_hp_max.Text = (Int32.Parse(txt_stat_hp_max.Text) * 2).ToString(); }

                txt_stat_hp_user.Text = txt_stat_hp_max.Text;
                txt_stat_atk_user.Text = txt_stat_atk_max.Text;
                txt_stat_def_user.Text = txt_stat_def_max.Text;
                txt_stat_satk_user.Text = txt_stat_satk_max.Text;
                txt_stat_sdef_user.Text = txt_stat_sdef_max.Text;
                txt_stat_spd_user.Text = txt_stat_spd_max.Text;
                txt_stat_eva_user.Text = txt_stat_eva_max.Text;
                txt_stat_fort_user.Text = txt_stat_fort_max.Text;

                
            }
            calculateAllEffStats();
        }

        private void disposeLearnsetElements()
        {
            foreach (CheckBox chk_move_selected in pkmn_moveset_chk_move_selected)
            {
                chk_move_selected.Dispose();
            }
            foreach (TextBox txt_move_lv in pkmn_moveset_txt_move_lv)
            {
                txt_move_lv.Dispose();
            }
            foreach (TextBox txt_move_name in pkmn_moveset_txt_move_name)
            {
                txt_move_name.Dispose();
            }
            foreach (ComboBox cmb_move_type in pkmn_moveset_cmb_move_type)
            {
                cmb_move_type.Dispose();
            }
            foreach (TextBox txt_move_pp in pkmn_moveset_txt_move_pp)
            {
                txt_move_pp.Dispose();
            }
            foreach (PictureBox pbox_move_attr in pkmn_moveset_pbox_move_attr)
            {
                pbox_move_attr.Dispose();
            }
            pkmn_moveset_chk_move_selected.Clear();
            pkmn_moveset_txt_move_lv.Clear();
            pkmn_moveset_txt_move_name.Clear();
            pkmn_moveset_cmb_move_type.Clear();
            pkmn_moveset_txt_move_pp.Clear();
            pkmn_moveset_pbox_move_attr.Clear();
            txt_combat_move_1_name.Text = "";
            txt_combat_move_2_name.Text = "";
            txt_combat_move_3_name.Text = "";
            txt_combat_move_4_name.Text = "";
        }

        private void loadMoveParams(object sender, EventArgs e)
        {
            if (pnl_pg_2_moves.Visible)
            {
                Control ActiveControl = FindControlAtCursor(this);
                if (!String.IsNullOrWhiteSpace(ActiveControl.Text))
                {
                    using (SQLiteConnection cn = new SQLiteConnection("Data Source=" + dbpath + ";Version=3;New=True;Compress=True;"))
                    {
                        cn.Open();
                        SQLiteCommand sqlite_cmd;
                        SQLiteDataReader sqlite_datareader;
                        sqlite_cmd = cn.CreateCommand();
                        sqlite_cmd.CommandText = "SELECT * FROM Moves Where Name = '" + ActiveControl.Text + "'";
                        sqlite_datareader = sqlite_cmd.ExecuteReader();
                        while (sqlite_datareader.Read())
                        {
                            if (!sqlite_datareader.IsDBNull(1)) { txt_combat_move_prev_name.Text = sqlite_datareader.GetString(1); }
                            if (!sqlite_datareader.IsDBNull(5)) { txt_combat_move_prev_pp.Text = sqlite_datareader.GetInt32(5).ToString(); }
                            if (!sqlite_datareader.IsDBNull(7)) { txt_combat_move_prev_acc.Text = sqlite_datareader.GetString(7); }
                            if (!sqlite_datareader.IsDBNull(4)) { txt_combat_move_prev_pow.Text = sqlite_datareader.GetInt32(4).ToString(); }
                            if (!sqlite_datareader.IsDBNull(6) && !sqlite_datareader.IsDBNull(8)) { txt_combat_move_prev_effect.Text = sqlite_datareader.GetString(8) + "; " + sqlite_datareader.GetString(6); }
                            else if (sqlite_datareader.IsDBNull(6) && !sqlite_datareader.IsDBNull(8)) { txt_combat_move_prev_effect.Text = sqlite_datareader.GetString(8) + "; Deals damage."; }
                            else { txt_combat_move_prev_effect.Text = ""; }
                            if (!sqlite_datareader.IsDBNull(3)) { txt_combat_move_prev_atr.Text = sqlite_datareader.GetString(3); }
                        }
                        cn.Close();
                    }
                }
            }
        }

        private void movesetLvCompatible()
        {
            for (int i = 0; i < pkmn_moveset_chk_move_selected.Count; i++)
            {
                if (Int32.TryParse(txt_sht_lv.Text, out int j) && Int32.TryParse(pkmn_moveset_txt_move_lv[i].Text, out int k))
                {
                    if (Int32.Parse(txt_sht_lv.Text) >= Int32.Parse(pkmn_moveset_txt_move_lv[i].Text)) { pkmn_moveset_chk_move_selected[i].Enabled = true; }
                    else { pkmn_moveset_chk_move_selected[i].Enabled = false; }
                }
                else { pkmn_moveset_chk_move_selected[i].Enabled = true; }
            }
        }

        private void moveCheckBoxClick(object sender, EventArgs e)
        {
            int box_id = -1;
            TextBox[] combatMoveText = { txt_combat_move_1_name, txt_combat_move_2_name, txt_combat_move_3_name, txt_combat_move_4_name };
            CheckBox clicked_box = (sender as CheckBox);
            // Iterate through all checkboxes in learnset and get the id of this one
            for (int i = 0; i < pkmn_moveset_chk_move_selected.Count; i++)
            {
                if (pkmn_moveset_chk_move_selected[i].Name == clicked_box.Name)
                {
                    box_id = i;
                    break;
                }
            }
            // Make sure the checkbox actually belongs to the moveset checkboxes
            if (box_id > -1)
            {
                // If we are adding a move, make sure we only have four moves
                if (clicked_box.Checked && pkmn_battleset_list.Count < 4)
                {
                    // Before starting, search for the move's dynamax equilvalent
                    // If the move is a Status move, make it Max Guard
                    if (pkmn_moveset_pbox_move_attr[box_id].Image == rimgs_icon_Status_Attack) { pkmn_dynamax_list.Add("Max Guard"); }
                    else { 
                        if (!String.IsNullOrWhiteSpace(pkmn_moveset_txt_move_name[box_id].Text))
                        {
                            using (SQLiteConnection cn = new SQLiteConnection("Data Source=" + dbpath + ";Version=3;New=True;Compress=True;"))
                            {
                                cn.Open();
                                SQLiteCommand sqlite_cmd;
                                SQLiteDataReader sqlite_datareader;
                                sqlite_cmd = cn.CreateCommand();
                                
                                if (String.IsNullOrWhiteSpace(pkmn_gmax_move) || pkmn_gmax_type != pkmn_moveset_cmb_move_type[box_id].Text)
                                {
                                    sqlite_cmd.CommandText = "SELECT Dynamax.Name FROM Moves Join Dynamax on Dynamax.Type = Moves.Type Where Moves.Name = '" + pkmn_moveset_txt_move_name[box_id].Text + "'";
                                }
                                // If it has a G-Max move and the typing matches, use that instead
                                else
                                {
                                    sqlite_cmd.CommandText = "SELECT Gigantamax.Name FROM Gigantamax Where Gigantamax.Name = '" + pkmn_gmax_move + "'";
                                }
                                sqlite_datareader = sqlite_cmd.ExecuteReader();
                                // If somehow no dynamax equilvalent is found, add to list regardless in order to keep indices matching with pkmn_battleset_list
                                if (!sqlite_datareader.HasRows) { pkmn_dynamax_list.Add(""); }
                                while (sqlite_datareader.Read())
                                {
                                    if (sqlite_datareader.IsDBNull(0)) { pkmn_dynamax_list.Add(""); }
                                    else { pkmn_dynamax_list.Add(sqlite_datareader.GetString(0)); }
                                    break; // Fail safe in case there's somehow more than one record
                                }
                                cn.Close();
                            }
                        }
                        // If we somehow found nothing, add to list regardless in order to keep indices matching with pkmn_battleset_list
                        else { pkmn_dynamax_list.Add(""); }
                    }
                    // Add respective move to battleset list
                    pkmn_battleset_list.Add(pkmn_moveset_txt_move_name[box_id].Text);
                    pkmn_battleset_types_list.Add(pkmn_moveset_cmb_move_type[box_id].Text);
                    if (pkmn_moveset_pbox_move_attr[box_id].Image == rimgs_icon_Physical_Attack) { pkmn_battleset_atr_list.Add("Physical"); }
                    else if (pkmn_moveset_pbox_move_attr[box_id].Image == rimgs_icon_Special_Attack) { pkmn_battleset_atr_list.Add("Special"); }
                    else if (pkmn_moveset_pbox_move_attr[box_id].Image == rimgs_icon_Status_Attack) { pkmn_battleset_atr_list.Add("Status"); }
                    else { pkmn_battleset_atr_list.Add("");  }
                    // Are we currently dynamaxed?
                    if (DynamaxEnabled)
                    {
                        // If so, set the text of the combat textbox to the dynamax move we just added 
                        combatMoveText[pkmn_battleset_list.Count - 1].Text = pkmn_dynamax_list[pkmn_battleset_list.Count - 1];
                    }
                    else
                    {
                        // If not, set the text of the combat textbox to regular move we just added
                        combatMoveText[pkmn_battleset_list.Count - 1].Text = pkmn_battleset_list[pkmn_battleset_list.Count - 1];
                    }
                }
                // If we are removing a move, look for it in the battleset
                else if (!clicked_box.Checked)
                {
                    for (int i = 0; i < pkmn_battleset_list.Count; i++)
                    {
                        if (pkmn_moveset_txt_move_name[box_id].Text == pkmn_battleset_list[i])
                        {
                            pkmn_battleset_list.RemoveAt(i);
                            pkmn_battleset_types_list.RemoveAt(i);
                            pkmn_battleset_atr_list.RemoveAt(i);
                            pkmn_dynamax_list.RemoveAt(i);
                            combatMoveText[i].Text = "";
                            // Since we just removed an entry, we need to shift all entries down the line (unless it was the final entry)
                            for (int j = i; j < 3; j++)
                            {
                                combatMoveText[j].Text = combatMoveText[j+1].Text;
                            }
                            // Last entry will always be blank when we're unchecking a move
                            combatMoveText[3].Text = "";
                            break;
                        }
                    }
                }
            }
            else { MessageBox.Show("CheckBox ID for " + clicked_box.Name + " was not found in list. Contact a developer!"); }

            // If we have 4 or more moves in the set, blockout the checkboxes that aren't checked
            if (pkmn_battleset_list.Count >= 4)
            {
                foreach (CheckBox c in pkmn_moveset_chk_move_selected) { c.Enabled = c.Checked; }
            }
            // Otherwise, enable the checkboxes if the pokemon meets the level requirement
            else
            {
                movesetLvCompatible();
            }
        }

        private void loadLearnsetFromSpecies(string species)
        {
            pkmn_moveset_list.Clear();
            pkmn_battleset_list.Clear();
            pkmn_battleset_types_list.Clear();
            pkmn_battleset_atr_list.Clear();
            pkmn_dynamax_list.Clear();
            disposeLearnsetElements();
            CheckBox chk_move_selected;
            TextBox txt_move_lv;
            TextBox txt_move_name;
            ComboBox cmb_move_type;
            TextBox txt_move_pp;
            PictureBox pbox_move_attr;
            int tab_counter = 40;
            using (SQLiteConnection cn = new SQLiteConnection("Data Source=" + dbpath + ";Version=3;New=True;Compress=True;"))
            {
                cn.Open();
                SQLiteCommand sqlite_cmd;
                SQLiteDataReader sqlite_datareader;
                sqlite_cmd = cn.CreateCommand();
                sqlite_cmd.CommandText = "Select Learnset.Move, Learnset.Obtained_By, Learnset.LV, Moves.Type, Moves.PP, Moves.Attribute from Learnset join Moves on Moves.Name = Learnset.Move Where Learnset.Species = '" + txt_sht_species.Text + "' order by Learnset.Obtained_By, Learnset.LV, Learnset.Move";
                sqlite_datareader = sqlite_cmd.ExecuteReader();
                while (sqlite_datareader.Read())
                {
                    // Read in each move from learnset
                    if (!sqlite_datareader.IsDBNull(0)) {
                        pkmn_moveset_list.Add(sqlite_datareader.GetString(0));

                        chk_move_selected = new CheckBox();
                        pkmn_moveset_chk_move_selected.Add(chk_move_selected);
                        chk_move_selected.Name = chk_move_selected_template.Name + pkmn_moveset_chk_move_selected.Count();
                        chk_move_selected.Size = chk_move_selected_template.Size;
                        chk_move_selected.Location = new System.Drawing.Point(chk_move_selected_template.Location.X, chk_move_selected_template.Location.Y + (24 * (pkmn_moveset_chk_move_selected.Count() - 1)));
                        chk_move_selected.CheckedChanged += new EventHandler(moveCheckBoxClick);
                        chk_move_selected.TabIndex += tab_counter;
                        tab_counter++;
                        pnl_move_list.Controls.Add(chk_move_selected);

                        txt_move_name = new TextBox();
                        pkmn_moveset_txt_move_name.Add(txt_move_name);
                        txt_move_name.Name = txt_move_name_template.Name + pkmn_moveset_txt_move_name.Count();
                        txt_move_name.Size = txt_move_name_template.Size;
                        txt_move_name.Location = new System.Drawing.Point(txt_move_name_template.Location.X, txt_move_name_template.Location.Y + (24 * (pkmn_moveset_txt_move_name.Count() - 1)));
                        txt_move_name.ReadOnly = txt_move_name_template.ReadOnly;
                        txt_move_name.Text = sqlite_datareader.GetString(0);
                        txt_move_name.MouseHover += new EventHandler(loadMoveParams);
                        pnl_move_list.Controls.Add(txt_move_name);

                        txt_move_lv = new TextBox();
                        pkmn_moveset_txt_move_lv.Add(txt_move_lv);
                        txt_move_lv.Name = txt_move_lv_template.Name + pkmn_moveset_txt_move_lv.Count();
                        txt_move_lv.Size = txt_move_lv_template.Size;
                        txt_move_lv.Location = new System.Drawing.Point(txt_move_lv_template.Location.X, txt_move_lv_template.Location.Y + (24 * (pkmn_moveset_txt_move_lv.Count() - 1)));
                        txt_move_lv.ReadOnly = txt_move_lv_template.ReadOnly;
                        if (!sqlite_datareader.IsDBNull(2)) { txt_move_lv.Text = sqlite_datareader.GetInt32(2).ToString(); }
                        else if (!sqlite_datareader.IsDBNull(1)) { txt_move_lv.Text = sqlite_datareader.GetString(1); }
                        pnl_move_list.Controls.Add(txt_move_lv);

                        txt_move_pp = new TextBox();
                        pkmn_moveset_txt_move_pp.Add(txt_move_pp);
                        txt_move_pp.Name = txt_move_pp_template.Name + pkmn_moveset_txt_move_pp.Count();
                        txt_move_pp.Size = txt_move_pp_template.Size;
                        txt_move_pp.Location = new System.Drawing.Point(txt_move_pp_template.Location.X, txt_move_pp_template.Location.Y + (24 * (pkmn_moveset_txt_move_pp.Count() - 1)));
                        txt_move_pp.ReadOnly = txt_move_pp_template.ReadOnly;
                        if (!sqlite_datareader.IsDBNull(4)) { txt_move_pp.Text = sqlite_datareader.GetInt32(4).ToString(); }
                        pnl_move_list.Controls.Add(txt_move_pp);

                        cmb_move_type = new ComboBox();
                        pkmn_moveset_cmb_move_type.Add(cmb_move_type);
                        cmb_move_type.Name = cmb_move_type_template.Name + pkmn_moveset_cmb_move_type.Count();
                        cmb_move_type.Size = cmb_move_type_template.Size;
                        cmb_move_type.Location = new System.Drawing.Point(cmb_move_type_template.Location.X, cmb_move_type_template.Location.Y + (24 * (pkmn_moveset_cmb_move_type.Count() - 1)));
                        cmb_move_type.Enabled = cmb_move_type_template.Enabled;
                        foreach (string type_item in cmb_move_type_template.Items)
                        {
                            cmb_move_type.Items.Add(type_item);
                        }
                        cmb_move_type_template.SelectedIndex = 0;
                        if (!sqlite_datareader.IsDBNull(3)) { cmb_move_type.SelectedIndex = cmb_move_type.FindStringExact(sqlite_datareader.GetString(3));}
                        pnl_move_list.Controls.Add(cmb_move_type);

                        pbox_move_attr = new PictureBox();
                        pkmn_moveset_pbox_move_attr.Add(pbox_move_attr);
                        pbox_move_attr.Name = pbox_move_attr_template.Name + pkmn_moveset_pbox_move_attr.Count();
                        pbox_move_attr.Size = pbox_move_attr_template.Size;
                        pbox_move_attr.Location = new System.Drawing.Point(pbox_move_attr_template.Location.X, pbox_move_attr_template.Location.Y + (24 * (pkmn_moveset_pbox_move_attr.Count() - 1)));
                        pbox_move_attr.SizeMode = pbox_move_attr_template.SizeMode;
                        if (!sqlite_datareader.IsDBNull(5)) { pbox_move_attr.Image = determineMoveAttributeImage(sqlite_datareader.GetString(5)); }
                        else { pbox_move_attr.Image = pbox_move_attr_template.Image;  }
                        pnl_move_list.Controls.Add(pbox_move_attr);
                    }
                }
                cn.Close();
            }
            movesetLvCompatible();
        }

        private void getGmaxMoveType()
        {
            if (!String.IsNullOrWhiteSpace(pkmn_gmax_move))
            {
                using (SQLiteConnection cn = new SQLiteConnection("Data Source=" + dbpath + ";Version=3;New=True;Compress=True;"))
                {
                    cn.Open();
                    SQLiteCommand sqlite_cmd;
                    SQLiteDataReader sqlite_datareader;
                    sqlite_cmd = cn.CreateCommand();
                    sqlite_cmd.CommandText = "SELECT Type FROM Gigantamax Where Name = '" + pkmn_gmax_move + "'";
                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    if (!sqlite_datareader.HasRows) { pkmn_gmax_type = ""; }
                    while (sqlite_datareader.Read())
                    {
                        if (sqlite_datareader.IsDBNull(0)) { pkmn_gmax_type = ""; }
                        else { pkmn_gmax_type = sqlite_datareader.GetString(0); }
                    }
                    cn.Close();
                }
            }
            else { pkmn_gmax_type = ""; }
        }

        private void txt_sht_species_TextChanged(object sender, EventArgs e)
        {
            pkmn_gmax_move = ""; pkmn_gmax_type = "";
            txt_moves_char_species.Text = txt_sht_species.Text;
            cmb_sht_ability.Text = "";
            cmb_sht_ability.Items.Clear();
            using (SQLiteConnection cn = new SQLiteConnection("Data Source=" + dbpath + ";Version=3;New=True;Compress=True;"))
            {
                cn.Open();
                SQLiteCommand sqlite_cmd;
                SQLiteDataReader sqlite_datareader;
                // create a new SQL command:
                sqlite_cmd = cn.CreateCommand();
                // First lets build a SQL-Query again:
                sqlite_cmd.CommandText = "SELECT * FROM Species Where Name = '" + txt_sht_species.Text + "'";
                // Now the SQLiteCommand object can give us a DataReader-Object:
                sqlite_datareader = sqlite_cmd.ExecuteReader();
                // The SQLiteDataReader allows us to run through the result lines:
                while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
                {
                    // Interpret each typing
                    string read_type1; string read_type2;
                    if (sqlite_datareader.IsDBNull(2)) { read_type1 = "---"; }
                    else { read_type1 = sqlite_datareader.GetString(2); }
                    if (sqlite_datareader.IsDBNull(3)) { read_type2 = "---"; }
                    else { read_type2 = sqlite_datareader.GetString(3); }
                    cmb_sht_type1.SelectedIndex = cmb_sht_type1.FindStringExact(read_type1);
                    cmb_sht_type2.SelectedIndex = cmb_sht_type2.FindStringExact(read_type2);
                    cmb_moves_char_type1.SelectedIndex = cmb_sht_type1.SelectedIndex;
                    cmb_moves_char_type2.SelectedIndex = cmb_sht_type2.SelectedIndex;
                    // Interpret abilites
                    string read_ability1; string read_ability2; string read_ability3;
                    if (sqlite_datareader.IsDBNull(4)) { read_ability1 = ""; }
                    else { read_ability1 = sqlite_datareader.GetString(4); cmb_sht_ability.Items.Add(read_ability1); cmb_sht_ability.SelectedIndex = 0; }
                    if (sqlite_datareader.IsDBNull(5)) { read_ability2 = ""; }
                    else { read_ability2 = sqlite_datareader.GetString(5); cmb_sht_ability.Items.Add(read_ability2); }
                    if (sqlite_datareader.IsDBNull(6)) { read_ability3 = ""; }
                    else { read_ability3 = sqlite_datareader.GetString(6); cmb_sht_ability.Items.Add(read_ability3); }
                    // Interpret exp growth
                    if (sqlite_datareader.IsDBNull(7)) { pkmn_EXP_Growth = ""; }
                    else { pkmn_EXP_Growth = sqlite_datareader.GetString(7); handleEXP(false); }
                    // Interpret base stats
                    if (sqlite_datareader.IsDBNull(8)) { pkmn_Stats[0] = 0; }
                    else { pkmn_Stats[0] = sqlite_datareader.GetInt32(8); }
                    if (sqlite_datareader.IsDBNull(9)) { pkmn_Stats[1] = 0; }
                    else { pkmn_Stats[1] = sqlite_datareader.GetInt32(9); }
                    if (sqlite_datareader.IsDBNull(10)) { pkmn_Stats[2] = 0; }
                    else { pkmn_Stats[2] = sqlite_datareader.GetInt32(10); }
                    if (sqlite_datareader.IsDBNull(11)) { pkmn_Stats[3] = 0; }
                    else { pkmn_Stats[3] = sqlite_datareader.GetInt32(11); }
                    if (sqlite_datareader.IsDBNull(12)) { pkmn_Stats[4] = 0; }
                    else { pkmn_Stats[4] = sqlite_datareader.GetInt32(12); }
                    if (sqlite_datareader.IsDBNull(13)) { pkmn_Stats[5] = 0; }
                    else { pkmn_Stats[5] = sqlite_datareader.GetInt32(13); }
                    calculateAllMaxStats();
                    // Interpret G-Max exclusive move
                    if (sqlite_datareader.IsDBNull(14)) { pkmn_gmax_move = ""; }
                    else { pkmn_gmax_move = sqlite_datareader.GetString(14); }
                }
                // We are ready, now lets cleanup and close our connection:
                cn.Close();
            }

            // Get the G-Max move and store it for later
            getGmaxMoveType();

            // If this is not a mega, but mega is enabled, disable it
            if ((txt_sht_species.Text.Length <= 5 || txt_sht_species.Text.Substring(0, 5) != "Mega " || DynamaxEnabled) && MegaEnabled)
            {
                toggleMega();
            }
            // If this is not a mega, but mega is enabled and it has a mega name, change the name
            else if (MegaEnabled && String.IsNullOrWhiteSpace(txt_sht_ability_effect.Text))
            {
                MessageBox.Show(txt_sht_species.Text.Substring(5, txt_sht_species.Text.Length - 5) + " has no mega evolution available. If this is an error, contact the developer!");
                txt_sht_species.Text = txt_sht_species.Text.Substring(5, txt_sht_species.Text.Length - 5);
            }

            // Once the species is good to go, load up the moveset
            if (!String.IsNullOrWhiteSpace(txt_sht_ability_effect.Text))
            {
                // Potential opportunity: what if this runs 4 times because of mega evo ping-pong?
                loadLearnsetFromSpecies(txt_sht_species.Text);
            }
            else
            {
                pkmn_moveset_list.Clear();
                disposeLearnsetElements();
            }
        }

        private void cmb_sht_ability_TextChanged(object sender, EventArgs e)
        {
            txt_sht_ability_effect.Text = "";
            if (!String.IsNullOrWhiteSpace(cmb_sht_ability.Text)) {
                using (SQLiteConnection cn = new SQLiteConnection("Data Source=" + dbpath + ";Version=3;New=True;Compress=True;"))
                {
                    cn.Open();
                    SQLiteCommand sqlite_cmd;
                    SQLiteDataReader sqlite_datareader;
                    sqlite_cmd = cn.CreateCommand();
                    sqlite_cmd.CommandText = "SELECT * FROM Abilities Where Name = '" + cmb_sht_ability.Text + "'";
                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    while (sqlite_datareader.Read())
                    {
                        string read_desc;
                        if (sqlite_datareader.IsDBNull(2)) { read_desc = "---"; }
                        else { read_desc = sqlite_datareader.GetString(2); }
                        txt_sht_ability_effect.Text = read_desc;
                    }
                    cn.Close();
                }
            }
        }

        private void txt_sht_lv_TextChanged(object sender, EventArgs e)
        {
            handleEXP(true);
            calculateAllMaxStats();
            movesetLvCompatible();
        }

        private void txt_sht_exp_TextChanged(object sender, EventArgs e)
        {
            handleEXP(false);
            calculateAllMaxStats();
        }

        private void txt_stat_user_TextChanged(object sender, EventArgs e)
        {
            calculateAllEffStats();
        }

        private void txt_inf_ready_morale_TextChanged(object sender, EventArgs e)
        {
            if (Int32.TryParse(txt_inf_base_morale.Text, out int i) && Int32.TryParse(txt_inf_ready_morale.Text, out int j)
                && Int32.Parse(txt_inf_base_morale.Text) < Int32.Parse(txt_inf_ready_morale.Text))
            {
                txt_inf_ready_morale.BackColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Crimson);
            }
            else
            {
                txt_inf_ready_morale.BackColor = System.Drawing.SystemColors.Window;
            }
        }

        private void txt_inf_ready_ing_TextChanged(object sender, EventArgs e)
        {
            if (Int32.TryParse(txt_inf_base_ing.Text, out int i) && Int32.TryParse(txt_inf_ready_ing.Text, out int j)
                && Int32.Parse(txt_inf_base_ing.Text) < Int32.Parse(txt_inf_ready_ing.Text))
            {
                txt_inf_ready_ing.BackColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Crimson);
            }
            else
            {
                txt_inf_ready_ing.BackColor = System.Drawing.SystemColors.Window;
            }
        }

        private void txt_inf_ready_insp_TextChanged(object sender, EventArgs e)
        {
            if (Int32.TryParse(txt_inf_base_insp.Text, out int i) && Int32.TryParse(txt_inf_ready_insp.Text, out int j)
                && Int32.Parse(txt_inf_base_insp.Text) < Int32.Parse(txt_inf_ready_insp.Text))
            {
                txt_inf_ready_insp.BackColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Crimson);
            }
            else
            {
                txt_inf_ready_insp.BackColor = System.Drawing.SystemColors.Window;
            }
        }

        private void txt_inf_ready_luck_TextChanged(object sender, EventArgs e)
        {
            if (Int32.TryParse(txt_inf_base_luck.Text, out int i) && Int32.TryParse(txt_inf_ready_luck.Text, out int j)
                && Int32.Parse(txt_inf_base_luck.Text) < Int32.Parse(txt_inf_ready_luck.Text))
            {
                txt_inf_ready_luck.BackColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Crimson);
            }
            else
            {
                txt_inf_ready_luck.BackColor = System.Drawing.SystemColors.Window;
            }
        }

        private void txt_stat_belly_curr_TextChanged(object sender, EventArgs e)
        {
            if (Int32.TryParse(txt_stat_belly_max.Text, out int i) && Int32.TryParse(txt_stat_belly_curr.Text, out int j)
                && Int32.Parse(txt_stat_belly_max.Text) < Int32.Parse(txt_stat_belly_curr.Text))
            {
                txt_stat_belly_curr.BackColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Crimson);
            }
            else
            {
                txt_stat_belly_curr.BackColor = System.Drawing.SystemColors.Window;
            }
        }

        private System.Drawing.Bitmap determineMoveAttributeImage(string attribute)
        {
            System.Drawing.Bitmap attrimage = rimgs_token;
            if (attribute.ToLowerInvariant() == "physical") { attrimage = rimgs_icon_Physical_Attack; }
            else if (attribute.ToLowerInvariant() == "special") { attrimage = rimgs_icon_Special_Attack; }
            else if (attribute.ToLowerInvariant() == "status") { attrimage = rimgs_icon_Status_Attack; }
            else if (attribute.ToLowerInvariant() == "s-physical") { attrimage = rimgs_icon_Physical_Attack_Shadow; }
            else if (attribute.ToLowerInvariant() == "s-special") { attrimage = rimgs_icon_Special_Attack_Shadow; }
            else if (attribute.ToLowerInvariant() == "s-status") { attrimage = rimgs_icon_Status_Attack_Shadow; }
            return attrimage;
        }

        private void txt_combat_move_prev_atr_TextChanged(object sender, EventArgs e)
        {
            pic_combat_move_prev_atr.Image = determineMoveAttributeImage(txt_combat_move_prev_atr.Text);
        }

        private void loadMoveCombatParams(int combat_move_slot)
        {
            TextBox[] txt_combat_move_name = { txt_combat_move_1_name, txt_combat_move_2_name, txt_combat_move_3_name, txt_combat_move_4_name };
            TextBox[] txt_combat_move_pp = { txt_combat_move_1_pp, txt_combat_move_2_pp, txt_combat_move_3_pp, txt_combat_move_4_pp };
            TextBox[] txt_combat_move_pp_max = { txt_combat_move_1_pp_max, txt_combat_move_2_pp_max, txt_combat_move_3_pp_max, txt_combat_move_4_pp_max };
            TextBox[] txt_combat_move_acc = { txt_combat_move_1_acc, txt_combat_move_2_acc, txt_combat_move_3_acc, txt_combat_move_4_acc };
            ComboBox[] cmb_combat_move_type = { cmb_combat_move_1_type, cmb_combat_move_2_type, cmb_combat_move_3_type, cmb_combat_move_4_type };
            TextBox[] txt_combat_move_pow = { txt_combat_move_1_pow, txt_combat_move_2_pow, txt_combat_move_3_pow, txt_combat_move_4_pow };
            TextBox[] txt_combat_move_atr = { txt_combat_move_1_atr, txt_combat_move_2_atr, txt_combat_move_3_atr, txt_combat_move_4_atr };
            TextBox[] txt_combat_move_effect = { txt_combat_move_1_effect, txt_combat_move_2_effect, txt_combat_move_3_effect, txt_combat_move_4_effect };
            PictureBox[] pic_combat_move_atr = { pic_combat_move_1_atr, pic_combat_move_2_atr, pic_combat_move_3_atr, pic_combat_move_4_atr };

            txt_combat_move_pp[combat_move_slot].Text = "";
            txt_combat_move_pp_max[combat_move_slot].Text = "";
            txt_combat_move_acc[combat_move_slot].Text = "";
            txt_combat_move_pow[combat_move_slot].Text = "";
            if (pkmn_battleset_atr_list.Count > combat_move_slot) { txt_combat_move_atr[combat_move_slot].Text = pkmn_battleset_atr_list[combat_move_slot]; }
            else { txt_combat_move_atr[combat_move_slot].Text = ""; }
            txt_combat_move_effect[combat_move_slot].Text = "";
            cmb_combat_move_type[combat_move_slot].SelectedIndex = 0;
            pic_combat_move_atr[combat_move_slot].Image = rimgs_token;

            if (!String.IsNullOrWhiteSpace(txt_combat_move_name[combat_move_slot].Text))
            {
                using (SQLiteConnection cn = new SQLiteConnection("Data Source=" + dbpath + ";Version=3;New=True;Compress=True;"))
                {
                    cn.Open();
                    SQLiteCommand sqlite_cmd;
                    SQLiteDataReader sqlite_datareader;
                    sqlite_cmd = cn.CreateCommand();
                    sqlite_cmd.CommandText = "Select * from (SELECT * FROM Moves union all SELECT * FROM Dynamax union all SELECT * FROM Gigantamax) Where Name = '" + txt_combat_move_name[combat_move_slot].Text + "'";
                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    while (sqlite_datareader.Read())
                    {
                        if (!sqlite_datareader.IsDBNull(5)) { txt_combat_move_pp_max[combat_move_slot].Text = sqlite_datareader.GetInt32(5).ToString(); }
                        if (!sqlite_datareader.IsDBNull(7)) { txt_combat_move_acc[combat_move_slot].Text = sqlite_datareader.GetString(7); }
                        if (!sqlite_datareader.IsDBNull(4)) { txt_combat_move_pow[combat_move_slot].Text = sqlite_datareader.GetInt32(4).ToString(); }
                        if (!sqlite_datareader.IsDBNull(6) && !sqlite_datareader.IsDBNull(8)) { txt_combat_move_effect[combat_move_slot].Text = sqlite_datareader.GetString(8) + "; " + sqlite_datareader.GetString(6); }
                        else if (sqlite_datareader.IsDBNull(6) && !sqlite_datareader.IsDBNull(8)) { txt_combat_move_effect[combat_move_slot].Text = sqlite_datareader.GetString(8) + "; Deals damage."; }
                        else { txt_combat_move_effect[combat_move_slot].Text = ""; }
                        if (txt_combat_move_atr[combat_move_slot].Text == "" && !sqlite_datareader.IsDBNull(3)) { txt_combat_move_atr[combat_move_slot].Text = sqlite_datareader.GetString(3); }
                        if (!sqlite_datareader.IsDBNull(2)) { cmb_combat_move_type[combat_move_slot].SelectedIndex = cmb_combat_move_type[combat_move_slot].FindStringExact(sqlite_datareader.GetString(2)); }
                    }
                    cn.Close();
                }
            }

            txt_combat_move_pp[combat_move_slot].Text = txt_combat_move_pp_max[combat_move_slot].Text;
            pic_combat_move_atr[combat_move_slot].Image = determineMoveAttributeImage(txt_combat_move_atr[combat_move_slot].Text);
        }

        private void txt_combat_move_1_name_TextChanged(object sender, EventArgs e)
        {
            loadMoveCombatParams(0);
        }

        private void txt_combat_move_2_name_TextChanged(object sender, EventArgs e)
        {
            loadMoveCombatParams(1);
        }

        private void txt_combat_move_3_name_TextChanged(object sender, EventArgs e)
        {
            loadMoveCombatParams(2);
        }

        private void txt_combat_move_4_name_TextChanged(object sender, EventArgs e)
        {
            loadMoveCombatParams(3);
        }

        private void txt_combat_move_1_pp_TextChanged(object sender, EventArgs e)
        {
            if (Int32.TryParse(txt_combat_move_1_pp.Text, out int i) && Int32.TryParse(txt_combat_move_1_pp_max.Text, out int j))
            {
                if (Int32.Parse(txt_combat_move_1_pp.Text) > Int32.Parse(txt_combat_move_1_pp_max.Text)) { txt_combat_move_1_pp.BackColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Crimson); }
                else if (Int32.Parse(txt_combat_move_1_pp.Text) == 0) { txt_combat_move_1_pp.BackColor = System.Drawing.SystemColors.Info; }
                else { txt_combat_move_1_pp.BackColor = System.Drawing.SystemColors.Window; }
            }
            else { txt_combat_move_1_pp.BackColor = System.Drawing.SystemColors.Window; }
        }

        private void txt_combat_move_2_pp_TextChanged(object sender, EventArgs e)
        {
            if (Int32.TryParse(txt_combat_move_2_pp.Text, out int i) && Int32.TryParse(txt_combat_move_2_pp_max.Text, out int j))
            {
                if (Int32.Parse(txt_combat_move_2_pp.Text) > Int32.Parse(txt_combat_move_2_pp_max.Text)) { txt_combat_move_2_pp.BackColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Crimson); }
                else if (Int32.Parse(txt_combat_move_2_pp.Text) == 0) { txt_combat_move_2_pp.BackColor = System.Drawing.SystemColors.Info; }
                else { txt_combat_move_2_pp.BackColor = System.Drawing.SystemColors.Window; }
            }
            else { txt_combat_move_2_pp.BackColor = System.Drawing.SystemColors.Window; }
        }

        private void txt_combat_move_3_pp_TextChanged(object sender, EventArgs e)
        {
            if (Int32.TryParse(txt_combat_move_3_pp.Text, out int i) && Int32.TryParse(txt_combat_move_3_pp_max.Text, out int j))
            {
                if (Int32.Parse(txt_combat_move_3_pp.Text) > Int32.Parse(txt_combat_move_3_pp_max.Text)) { txt_combat_move_3_pp.BackColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Crimson); }
                else if (Int32.Parse(txt_combat_move_3_pp.Text) == 0) { txt_combat_move_3_pp.BackColor = System.Drawing.SystemColors.Info; }
                else { txt_combat_move_3_pp.BackColor = System.Drawing.SystemColors.Window; }
            }
            else { txt_combat_move_3_pp.BackColor = System.Drawing.SystemColors.Window; }
        }

        private void txt_combat_move_4_pp_TextChanged(object sender, EventArgs e)
        {
            if (Int32.TryParse(txt_combat_move_4_pp.Text, out int i) && Int32.TryParse(txt_combat_move_4_pp_max.Text, out int j))
            {
                if (Int32.Parse(txt_combat_move_4_pp.Text) > Int32.Parse(txt_combat_move_4_pp_max.Text)) { txt_combat_move_4_pp.BackColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Crimson); }
                else if (Int32.Parse(txt_combat_move_4_pp.Text) == 0) { txt_combat_move_4_pp.BackColor = System.Drawing.SystemColors.Info; }
                else { txt_combat_move_4_pp.BackColor = System.Drawing.SystemColors.Window; }
            }
            else { txt_combat_move_4_pp.BackColor = System.Drawing.SystemColors.Window; }
        }

        private void txt_combat_move_5_pp_TextChanged(object sender, EventArgs e)
        {
            if (Int32.TryParse(txt_combat_move_5_pp.Text, out int i) && Int32.TryParse(txt_combat_move_5_pp_max.Text, out int j))
            {
                if (Int32.Parse(txt_combat_move_5_pp.Text) > Int32.Parse(txt_combat_move_5_pp_max.Text)) { txt_combat_move_5_pp.BackColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Crimson); }
                else if (Int32.Parse(txt_combat_move_5_pp.Text) == 0) { txt_combat_move_5_pp.BackColor = System.Drawing.SystemColors.ControlDarkDark; }
                else { txt_combat_move_5_pp.BackColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.DarkSlateBlue); }
            }
            else { txt_combat_move_5_pp.BackColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.DarkSlateBlue); }
        }

        private void disposeShadowMove()
        {
            txt_combat_move_5_name.Text = "";
            txt_combat_move_5_pp.Text = "";
            txt_combat_move_5_pp_max.Text = "";
            txt_combat_move_5_acc.Text = "";
            cmb_combat_move_5_type.Text = "???";
            txt_combat_move_5_pow.Text = "";
            txt_combat_move_5_atr.Text = "";
            txt_combat_move_5_effect.Text = "";
            pic_combat_move_5_atr.Image = rimgs_token;
        }

        private void toggleShadowMoveVisible(bool setvisible)
        {
            bool allowvisible = false;
            if (!lbl_combat_move_5_name.Visible && Int32.TryParse(txt_stat_hp_eff.Text, out int i) && Int32.TryParse(txt_stat_hp_max.Text, out int j)
                && Int32.Parse(txt_stat_hp_eff.Text) <= percent_hp_to_shadow*Int32.Parse(txt_stat_hp_max.Text) && Int32.Parse(txt_stat_hp_eff.Text) > 0)
            {
                allowvisible = true;
                loadShadowMove();
            }
            if ((allowvisible && setvisible) || !setvisible)
            {
                lbl_combat_move_5_name.Visible = setvisible;
                txt_combat_move_5_name.Visible = setvisible;
                txt_combat_move_5_pp.Visible = setvisible;
                lbl_combat_move_5_pp.Visible = setvisible;
                txt_combat_move_5_pp_max.Visible = setvisible;
                lbl_combat_move_5_pp_max.Visible = setvisible;
                txt_combat_move_5_acc.Visible = setvisible;
                lbl_combat_move_5_acc.Visible = setvisible;
                cmb_combat_move_5_type.Visible = setvisible;
                lbl_combat_move_5_type.Visible = setvisible;
                txt_combat_move_5_pow.Visible = setvisible;
                lbl_combat_move_5_pow.Visible = setvisible;
                txt_combat_move_5_atr.Visible = setvisible;
                txt_combat_move_5_effect.Visible = setvisible;
                lbl_combat_move_5_effect.Visible = setvisible;
                pic_combat_move_5_atr.Visible = setvisible;
            }
        }

        private void txt_stat_hp_eff_TextChanged(object sender, EventArgs e)
        {
            if (ts_campaign_dnde_btn.Checked && Int32.TryParse(txt_stat_hp_eff.Text, out int i) && Int32.TryParse(txt_stat_hp_max.Text, out int j))
            {
                if (Int32.Parse(txt_stat_hp_eff.Text) <= percent_hp_to_shadow * Int32.Parse(txt_stat_hp_max.Text) && Int32.Parse(txt_stat_hp_eff.Text) > 0) { toggleShadowMoveVisible(true);  }
                else { toggleShadowMoveVisible(false); }
            }
        }

        private void loadAllShadowMoves()
        {
            shadow_moves.Clear();
            using (SQLiteConnection cn = new SQLiteConnection("Data Source=" + dbpath + ";Version=3;New=True;Compress=True;"))
            {
                cn.Open();
                SQLiteCommand sqlite_cmd;
                SQLiteDataReader sqlite_datareader;
                sqlite_cmd = cn.CreateCommand();
                sqlite_cmd.CommandText = "Select Name from Moves WHERE Attribute like 'S-%' and Type = '---' ORDER BY ID";
                sqlite_datareader = sqlite_cmd.ExecuteReader();
                while (sqlite_datareader.Read())
                {
                    if (!sqlite_datareader.IsDBNull(0)) { shadow_moves.Add(sqlite_datareader.GetString(0)); }
                }
                cn.Close();
            }
            if (shadow_moves.Count > 0)
            {
                // Increase the frequency of Shadow Rush, Shadow Lance and Shadow Slash
                shadow_moves.Add(shadow_moves[0]); shadow_moves.Add(shadow_moves[0]); shadow_moves.Add(shadow_moves[0]); shadow_moves.Add(shadow_moves[0]); shadow_moves.Add(shadow_moves[0]); shadow_moves.Add(shadow_moves[0]);
                shadow_moves.Add(shadow_moves[1]); shadow_moves.Add(shadow_moves[1]); 
                shadow_moves.Add(shadow_moves[4]); shadow_moves.Add(shadow_moves[4]); 
                // Increase the frequency of all moves EXCEPT shadow punisher as well
                int original_count = shadow_moves.Count;
                for (int i = 0; i < original_count-1; i++) {
                    shadow_moves.Add(shadow_moves[i]);
                    shadow_moves.Add(shadow_moves[i]);
                }
                // Randomize the list order
                int n = shadow_moves.Count;
                while (n > 1)
                {
                    n--;
                    int k = rng.Next(n + 1);
                    string value = shadow_moves[k];
                    shadow_moves[k] = shadow_moves[n];
                    shadow_moves[n] = value;
                }
            }
        }

        
        private void loadShadowMove()
        {
            string loadmove = txt_combat_move_5_name.Text;
            disposeShadowMove();
            if (shadow_moves.Count > 0 && !fileio) {
                loadmove = shadow_moves[rng.Next(0, shadow_moves.Count-1)];
                txt_combat_move_5_name.Text = loadmove;
            }
            if (!String.IsNullOrWhiteSpace(loadmove))
            {
                using (SQLiteConnection cn = new SQLiteConnection("Data Source=" + dbpath + ";Version=3;New=True;Compress=True;"))
                {
                    cn.Open();
                    SQLiteCommand sqlite_cmd;
                    SQLiteDataReader sqlite_datareader;
                    sqlite_cmd = cn.CreateCommand();
                    sqlite_cmd.CommandText = "SELECT * FROM Moves Where Name = '" + loadmove + "'";
                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    while (sqlite_datareader.Read())
                    {
                        if (!sqlite_datareader.IsDBNull(5)) { txt_combat_move_5_pp_max.Text = sqlite_datareader.GetInt32(5).ToString(); }
                        if (!sqlite_datareader.IsDBNull(7)) { txt_combat_move_5_acc.Text = sqlite_datareader.GetString(7); }
                        if (!sqlite_datareader.IsDBNull(4)) { txt_combat_move_5_pow.Text = sqlite_datareader.GetInt32(4).ToString(); }
                        if (!sqlite_datareader.IsDBNull(6) && !sqlite_datareader.IsDBNull(8)) { txt_combat_move_5_effect.Text = sqlite_datareader.GetString(8) + "; " + sqlite_datareader.GetString(6); }
                        else if (sqlite_datareader.IsDBNull(6) && !sqlite_datareader.IsDBNull(8)) { txt_combat_move_5_effect.Text = sqlite_datareader.GetString(8) + "; Deals damage."; }
                        else { txt_combat_move_5_effect.Text = ""; }
                        if (!sqlite_datareader.IsDBNull(3)) { txt_combat_move_5_atr.Text = sqlite_datareader.GetString(3); }
                        if (!sqlite_datareader.IsDBNull(2)) { cmb_combat_move_5_type.SelectedIndex = cmb_combat_move_5_type.FindStringExact(sqlite_datareader.GetString(2)); }
                    }
                    cn.Close();
                }
            }
            txt_combat_move_5_pp.Text = txt_combat_move_5_pp_max.Text;
            pic_combat_move_5_atr.Image = determineMoveAttributeImage(txt_combat_move_5_atr.Text);
        }

        private void textbox_nouserinput(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void loadItemParams(object sender, EventArgs e)
        {
            if (pnl_pg_3_inv.Visible)
            {
                txt_inv_preview_title.Text = "Item Preview";
                txt_inv_preview_desc.Text = "Mouse over an item to view its description!";
                Control ActiveControl = FindControlAtCursor(this);
                if (!String.IsNullOrWhiteSpace(ActiveControl.Text))
                {
                    using (SQLiteConnection cn = new SQLiteConnection("Data Source=" + dbpath + ";Version=3;New=True;Compress=True;"))
                    {
                        cn.Open();
                        SQLiteCommand sqlite_cmd;
                        SQLiteDataReader sqlite_datareader;
                        sqlite_cmd = cn.CreateCommand();
                        sqlite_cmd.CommandText = "SELECT * FROM Items Where Name = '" + ActiveControl.Text + "'";
                        sqlite_datareader = sqlite_cmd.ExecuteReader();
                        while (sqlite_datareader.Read())
                        {
                            if (!sqlite_datareader.IsDBNull(1)) { txt_inv_preview_title.Text = sqlite_datareader.GetString(1); }
                            if (!sqlite_datareader.IsDBNull(2)) { txt_inv_preview_desc.Text = sqlite_datareader.GetString(2); }
                        }
                        cn.Close();
                    }
                }
            }
        }

        private void loadBags()
        {
            using (SQLiteConnection cn = new SQLiteConnection("Data Source=" + dbpath + ";Version=3;New=True;Compress=True;"))
            {
                cn.Open();
                SQLiteCommand sqlite_cmd;
                SQLiteDataReader sqlite_datareader;
                sqlite_cmd = cn.CreateCommand();
                sqlite_cmd.CommandText = "SELECT TRIM(SUBSTR(Name,7,LENGTH(Name)-6)) FROM Items Where Name like 'Bag - %'";
                sqlite_datareader = sqlite_cmd.ExecuteReader();
                while (sqlite_datareader.Read())
                {
                    if (!sqlite_datareader.IsDBNull(0)) { cmb_inv_bag.Items.Add(sqlite_datareader.GetString(0)); }
                }
                cn.Close();
            }
            cmb_inv_bag.SelectedIndex = 0;
        }

        private void loadBag()
        {
            if (!String.IsNullOrWhiteSpace(cmb_inv_bag.Text))
            {
                using (SQLiteConnection cn = new SQLiteConnection("Data Source=" + dbpath + ";Version=3;New=True;Compress=True;"))
                {
                    cn.Open();
                    SQLiteCommand sqlite_cmd;
                    SQLiteDataReader sqlite_datareader;
                    sqlite_cmd = cn.CreateCommand();
                    sqlite_cmd.CommandText = @"SELECT Description FROM Items Where Name = ""Bag - " + cmb_inv_bag.Text + @"""";
                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    while (sqlite_datareader.Read())
                    {
                        if (!sqlite_datareader.IsDBNull(0)) { txt_inv_max_qty.Text = sqlite_datareader.GetString(0); }
                    }
                    cn.Close();
                }
            }
        }

        private void cmb_inv_bag_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadBag();
            inv_qty_change();
        }

        private void loadTraitParams(object sender, EventArgs e)
        {
            txt_trait_preview_title.Text = "Trait Preview";
            txt_trait_preview_desc.Text = "Mouse over a trait to view its description!";
            Control ActiveControl = FindControlAtCursor(this);
            if (!String.IsNullOrWhiteSpace(ActiveControl.Text))
            {
                using (SQLiteConnection cn = new SQLiteConnection("Data Source=" + dbpath + ";Version=3;New=True;Compress=True;"))
                {
                    cn.Open();
                    SQLiteCommand sqlite_cmd;
                    SQLiteDataReader sqlite_datareader;
                    sqlite_cmd = cn.CreateCommand();
                    sqlite_cmd.CommandText = @"SELECT * FROM Traits Where Name = """ + ActiveControl.Text + @"""";
                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    while (sqlite_datareader.Read())
                    {
                        if (!sqlite_datareader.IsDBNull(1)) { txt_trait_preview_title.Text = sqlite_datareader.GetString(1); }
                        if (!sqlite_datareader.IsDBNull(3)) { txt_trait_preview_desc.Text = sqlite_datareader.GetString(3); }
                    }
                    cn.Close();
                }
            }
        }

        private void loadTraits()
        {
            ComboBox[] traitBoxes = { cmb_trait_1, cmb_trait_2, cmb_trait_3, cmb_trait_4, cmb_trait_5, cmb_trait_6,
                cmb_trait_7, cmb_trait_8, cmb_trait_9, cmb_trait_10, cmb_trait_11, cmb_trait_12 };
            foreach (ComboBox tbox in traitBoxes) { tbox.Items.Add("---"); }
            using (SQLiteConnection cn = new SQLiteConnection("Data Source=" + dbpath + ";Version=3;New=True;Compress=True;"))
            {
                cn.Open();
                SQLiteCommand sqlite_cmd;
                SQLiteDataReader sqlite_datareader;
                sqlite_cmd = cn.CreateCommand();
                sqlite_cmd.CommandText = "SELECT Name FROM Traits";
                sqlite_datareader = sqlite_cmd.ExecuteReader();
                while (sqlite_datareader.Read())
                {
                    
                    if (!sqlite_datareader.IsDBNull(0)) {
                        foreach (ComboBox tbox in traitBoxes) { tbox.Items.Add(sqlite_datareader.GetString(0)); }
                    }
                }
                cn.Close();
            }
            foreach (ComboBox tbox in traitBoxes) { tbox.SelectedIndex = 0;
                /*foreach (Object tboxitem in tbox.Items) {
                    tboxitem.GotMouseCapture += new EventHandler(loadTraitParams);
                }*/
            }
        }

        private void cmb_trait_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox[] traitBoxes = { cmb_trait_1, cmb_trait_2, cmb_trait_3, cmb_trait_4, cmb_trait_5, cmb_trait_6,
                cmb_trait_7, cmb_trait_8, cmb_trait_9, cmb_trait_10, cmb_trait_11, cmb_trait_12 };
            ComboBox changed_trait = (sender as ComboBox);
            foreach (ComboBox tbox in traitBoxes) {
                if (changed_trait.SelectedIndex == 0) { break; }
                else if (tbox.Name != changed_trait.Name && tbox.SelectedIndex == changed_trait.SelectedIndex)
                {
                    //MessageBox.Show("Please only choose one unique trait.");
                    changed_trait.SelectedIndex = 0;
                }
            }
        }

        private void loadNaturesParams(object sender, EventArgs e)
        {
            txt_nature_desc.Text = "";
            if (!String.IsNullOrWhiteSpace(cmb_nature_name.Text))
            {
                using (SQLiteConnection cn = new SQLiteConnection("Data Source=" + dbpath + ";Version=3;New=True;Compress=True;"))
                {
                    cn.Open();
                    SQLiteCommand sqlite_cmd;
                    SQLiteDataReader sqlite_datareader;
                    sqlite_cmd = cn.CreateCommand();
                    sqlite_cmd.CommandText = @"SELECT Description FROM Natures Where Name = """ + cmb_nature_name.Text + @"""";
                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    while (sqlite_datareader.Read())
                    {
                        if (!sqlite_datareader.IsDBNull(0)) { txt_nature_desc.Text = sqlite_datareader.GetString(0); }
                    }
                    cn.Close();
                }
            }
        }

        private void loadNatures()
        {
            using (SQLiteConnection cn = new SQLiteConnection("Data Source=" + dbpath + ";Version=3;New=True;Compress=True;"))
            {
                cn.Open();
                SQLiteCommand sqlite_cmd;
                SQLiteDataReader sqlite_datareader;
                sqlite_cmd = cn.CreateCommand();
                sqlite_cmd.CommandText = "SELECT Name FROM Natures";
                sqlite_datareader = sqlite_cmd.ExecuteReader();
                while (sqlite_datareader.Read())
                {

                    if (!sqlite_datareader.IsDBNull(0))
                    {
                        cmb_nature_name.Items.Add(sqlite_datareader.GetString(0));
                    }
                }
                cn.Close();
            }

            cmb_nature_name.SelectedIndex = 0;

        }

        private void pic_flavor_appearance_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlgOpenFileDialog = new OpenFileDialog();
            dlgOpenFileDialog.Multiselect = false;
            dlgOpenFileDialog.Filter = "Image files (*.bmp, *.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.bmp; *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            if (dlgOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pic_flavor_appearance.Image = System.Drawing.Image.FromFile(dlgOpenFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void disposeEntireSheet()
        {
            txt_sht_name.Text = "";
            txt_sht_age.Text = "";
            txt_sht_species.Text = "";
            txt_sht_lv.Text = "";
            txt_sht_exp.Text = "";

            txt_stat_belly_curr.Text = "100";
            txt_stat_belly_max.Text = "100";
            txt_stat_hp_user.Text = ""; txt_stat_hp_eff.Text = ""; txt_stat_hp_max.Text = ""; txt_stat_hp_half.Text = ""; txt_stat_hp_quarter.Text = "";
            txt_stat_atk_user.Text = ""; txt_stat_atk_eff.Text = ""; txt_stat_atk_max.Text = ""; txt_stat_atk_half.Text = ""; txt_stat_atk_quarter.Text = "";
            txt_stat_def_user.Text = ""; txt_stat_def_eff.Text = ""; txt_stat_def_max.Text = ""; txt_stat_def_half.Text = ""; txt_stat_def_quarter.Text = "";
            txt_stat_satk_user.Text = ""; txt_stat_satk_eff.Text = ""; txt_stat_satk_max.Text = ""; txt_stat_satk_half.Text = ""; txt_stat_satk_quarter.Text = "";
            txt_stat_sdef_user.Text = ""; txt_stat_sdef_eff.Text = ""; txt_stat_sdef_max.Text = ""; txt_stat_sdef_half.Text = ""; txt_stat_sdef_quarter.Text = "";
            txt_stat_spd_user.Text = ""; txt_stat_spd_eff.Text = ""; txt_stat_spd_max.Text = ""; txt_stat_spd_half.Text = ""; txt_stat_spd_quarter.Text = "";
            txt_stat_eva_user.Text = ""; txt_stat_eva_eff.Text = ""; txt_stat_eva_max.Text = ""; txt_stat_eva_half.Text = ""; txt_stat_eva_quarter.Text = "";
            txt_stat_fort_user.Text = ""; txt_stat_fort_eff.Text = ""; txt_stat_fort_max.Text = ""; txt_stat_fort_half.Text = ""; txt_stat_fort_quarter.Text = "";
            ctr_stat_hp_stage.Value = 0;
            ctr_stat_atk_stage.Value = 0;
            ctr_stat_def_stage.Value = 0;
            ctr_stat_satk_stage.Value = 0;
            ctr_stat_sdef_stage.Value = 0;
            ctr_stat_spd_stage.Value = 0;
            ctr_stat_eva_stage.Value = 0;
            ctr_stat_fort_stage.Value = 0;

            txt_inf_ready_morale.Text = "";
            txt_inf_ready_ing.Text = "";
            txt_inf_ready_insp.Text = "";
            txt_inf_ready_luck.Text = "";
            txt_inf_base_morale.Text = "";
            txt_inf_base_ing.Text = "";
            txt_inf_base_insp.Text = "";
            txt_inf_base_luck.Text = "";

            ClkInfliction = 0;
            Clkstun = 0;
            Clkmez = 0;
            Clkdoom = 0;
            Clkmisc1 = 0;
            Clkmisc2 = 0;
            Clkmisc3 = 0;
            clkChange();

            txt_clocks_misc1.Text = "Misc I";
            txt_clocks_misc2.Text = "Misc II";
            txt_clocks_misc3.Text = "Misc III";

            txt_combat_move_1_pp.Text = "";
            txt_combat_move_2_pp.Text = "";
            txt_combat_move_3_pp.Text = "";
            txt_combat_move_4_pp.Text = "";
            txt_combat_move_5_pp.Text = "";

            if (DynamaxEnabled) { toggleDynamax();  }

            txt_inv_qty_1.Text = "";
            chk_inv_held_2.Checked = false; txt_inv_qty_2.Text = ""; txt_inv_name_2.Text = "";
            chk_inv_held_3.Checked = false; txt_inv_qty_3.Text = ""; txt_inv_name_3.Text = "";
            chk_inv_held_4.Checked = false; txt_inv_qty_4.Text = ""; txt_inv_name_4.Text = "";
            chk_inv_held_5.Checked = false; txt_inv_qty_5.Text = ""; txt_inv_name_5.Text = "";
            chk_inv_held_6.Checked = false; txt_inv_qty_6.Text = ""; txt_inv_name_6.Text = "";
            chk_inv_held_7.Checked = false; txt_inv_qty_7.Text = ""; txt_inv_name_7.Text = "";
            chk_inv_held_8.Checked = false; txt_inv_qty_8.Text = ""; txt_inv_name_8.Text = "";
            chk_inv_held_9.Checked = false; txt_inv_qty_9.Text = ""; txt_inv_name_9.Text = "";
            chk_inv_held_10.Checked = false; txt_inv_qty_10.Text = ""; txt_inv_name_10.Text = "";
            chk_inv_held_11.Checked = false; txt_inv_qty_11.Text = ""; txt_inv_name_11.Text = "";
            chk_inv_held_12.Checked = false; txt_inv_qty_12.Text = ""; txt_inv_name_12.Text = "";
            chk_inv_held_13.Checked = false; txt_inv_qty_13.Text = ""; txt_inv_name_13.Text = "";
            chk_inv_held_14.Checked = false; txt_inv_qty_14.Text = ""; txt_inv_name_14.Text = "";
            chk_inv_held_15.Checked = false; txt_inv_qty_15.Text = ""; txt_inv_name_15.Text = "";
            chk_inv_held_16.Checked = false; txt_inv_qty_16.Text = ""; txt_inv_name_16.Text = "";
            chk_inv_held_17.Checked = false; txt_inv_qty_17.Text = ""; txt_inv_name_17.Text = "";
            chk_inv_held_18.Checked = false; txt_inv_qty_18.Text = ""; txt_inv_name_18.Text = "";
            chk_inv_held_19.Checked = false; txt_inv_qty_19.Text = ""; txt_inv_name_19.Text = "";
            chk_inv_held_20.Checked = false; txt_inv_qty_20.Text = ""; txt_inv_name_20.Text = "";
            chk_inv_held_21.Checked = false; txt_inv_qty_21.Text = ""; txt_inv_name_21.Text = "";
            chk_inv_held_22.Checked = false; txt_inv_qty_22.Text = ""; txt_inv_name_22.Text = "";
            chk_inv_held_23.Checked = false; txt_inv_qty_23.Text = ""; txt_inv_name_23.Text = "";
            chk_inv_held_24.Checked = false; txt_inv_qty_24.Text = ""; txt_inv_name_24.Text = "";
            chk_inv_held_25.Checked = false; txt_inv_qty_25.Text = ""; txt_inv_name_25.Text = "";
            chk_inv_held_26.Checked = false; txt_inv_qty_26.Text = ""; txt_inv_name_26.Text = "";
            cmb_inv_bag.SelectedIndex = 0;

            cmb_trait_1.SelectedIndex = 0; cmb_trait_2.SelectedIndex = 0; cmb_trait_3.SelectedIndex = 0;
            cmb_trait_4.SelectedIndex = 0; cmb_trait_5.SelectedIndex = 0; cmb_trait_6.SelectedIndex = 0;
            cmb_trait_7.SelectedIndex = 0; cmb_trait_8.SelectedIndex = 0; cmb_trait_9.SelectedIndex = 0;
            cmb_trait_10.SelectedIndex = 0; cmb_trait_11.SelectedIndex = 0; cmb_trait_12.SelectedIndex = 0;

            chk_skills_gathering.Checked = false; txt_skills_gathering.Text = "";
            chk_skills_tracking.Checked = false; txt_skills_tracking.Text = "";
            chk_skills_medicine.Checked = false; txt_skills_medicine.Text = "";
            chk_skills_nature.Checked = false; txt_skills_nature.Text = "";
            chk_skills_deception.Checked = false; txt_skills_deception.Text = "";
            chk_skills_persuasion.Checked = false; txt_skills_persuasion.Text = "";
            chk_skills_intimidation.Checked = false; txt_skills_intimidation.Text = "";
            chk_skills_perf.Checked = false; txt_skills_perf.Text = "";
            chk_skills_crafting.Checked = false; txt_skills_crafting.Text = "";
            chk_skills_app.Checked = false; txt_skills_app.Text = "";
            chk_skills_hist.Checked = false; txt_skills_hist.Text = "";
            chk_skills_build.Checked = false; txt_skills_build.Text = "";
            chk_skills_other1.Checked = false; txt_skills_nm_other1.Text = ""; txt_skills_val_other1.Text = "";
            chk_skills_other2.Checked = false; txt_skills_nm_other2.Text = ""; txt_skills_val_other2.Text = "";
            chk_skills_other3.Checked = false; txt_skills_nm_other3.Text = ""; txt_skills_val_other3.Text = "";
            chk_skills_other4.Checked = false; txt_skills_nm_other4.Text = ""; txt_skills_val_other4.Text = "";

            cmb_nature_name.SelectedIndex = 0;
            txt_flavor_personality.Text = "";
            txt_flavor_relationships.Text = "";
            txt_flavor_ideals.Text = "";
            txt_flavor_flaws.Text = "";
            txt_flavor_journal.Text = "";
            if (pic_flavor_appearance.Image != null) { try { pic_flavor_appearance.Image.Dispose(); } catch (Exception) { throw; } }
            pic_flavor_appearance.Image = rimgs_badge_shimmering_outline;

            pkmn_EXP_Growth = "Slow";
            pkmn_gmax_move = "";
            pkmn_gmax_type = "";
            exp_to_lv = 0;

            pkmn_battleset_list.Clear();
            pkmn_battleset_types_list.Clear();
            pkmn_battleset_atr_list.Clear();
            pkmn_dynamax_list.Clear();
            pkmn_moveset_list.Clear();

            PageSelected = 1;
            toggleShadowMoveVisible(false);
        }

        private void writeSheetToFile()
        {
            fileio = true;
            // Save the image to binary array first
            byte[] imgarr;
            using (MemoryStream ms = new MemoryStream())
            {
                pic_flavor_appearance.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                imgarr = ms.ToArray();
            }
            // Save the file
            using (BinaryWriter b = new BinaryWriter(File.Open(Savefilepath, FileMode.Create)))
            {
                b.Write(txt_sht_name.Text);
                b.Write(txt_sht_age.Text);
                b.Write(txt_sht_species.Text);
                b.Write(txt_sht_lv.Text);
                b.Write(txt_sht_exp.Text);
                b.Write(cmb_sht_ability.Text);

                b.Write(txt_stat_belly_curr.Text);
                b.Write(txt_stat_belly_max.Text);
                b.Write(txt_stat_hp_user.Text);
                b.Write(txt_stat_atk_user.Text);
                b.Write(txt_stat_def_user.Text);
                b.Write(txt_stat_satk_user.Text);
                b.Write(txt_stat_sdef_user.Text);
                b.Write(txt_stat_spd_user.Text);
                b.Write(txt_stat_eva_user.Text);
                b.Write(txt_stat_fort_user.Text);
                b.Write((int)ctr_stat_hp_stage.Value);
                b.Write((int)ctr_stat_atk_stage.Value);
                b.Write((int)ctr_stat_def_stage.Value);
                b.Write((int)ctr_stat_satk_stage.Value);
                b.Write((int)ctr_stat_sdef_stage.Value);
                b.Write((int)ctr_stat_spd_stage.Value);
                b.Write((int)ctr_stat_eva_stage.Value);
                b.Write((int)ctr_stat_fort_stage.Value);

                b.Write(txt_inf_ready_morale.Text);
                b.Write(txt_inf_ready_ing.Text);
                b.Write(txt_inf_ready_insp.Text);
                b.Write(txt_inf_ready_luck.Text);
                b.Write(txt_inf_base_morale.Text);
                b.Write(txt_inf_base_ing.Text);
                b.Write(txt_inf_base_insp.Text);
                b.Write(txt_inf_base_luck.Text);

                b.Write((int)ClkInfliction);
                b.Write((int)Clkstun);
                b.Write((int)Clkmez);
                b.Write((int)Clkdoom);
                b.Write((int)Clkmisc1);
                b.Write((int)Clkmisc2);
                b.Write((int)Clkmisc3);

                b.Write(txt_clocks_misc1.Text);
                b.Write(txt_clocks_misc2.Text);
                b.Write(txt_clocks_misc3.Text);

                // To-Do: make sure to save the ACTUAL moves and not the dynamax moves
                if (pkmn_battleset_list.Count > 0) { b.Write(pkmn_battleset_list[0]); } else { b.Write(txt_combat_move_1_name.Text); } //pkmn_battleset_list
                if (pkmn_battleset_list.Count > 1) { b.Write(pkmn_battleset_list[1]); } else { b.Write(txt_combat_move_2_name.Text); } //pkmn_battleset_list
                if (pkmn_battleset_list.Count > 2) { b.Write(pkmn_battleset_list[2]); } else { b.Write(txt_combat_move_3_name.Text); } //pkmn_battleset_list
                if (pkmn_battleset_list.Count > 3) { b.Write(pkmn_battleset_list[3]); } else { b.Write(txt_combat_move_4_name.Text); } //pkmn_battleset_list
                b.Write(txt_combat_move_5_name.Text);
                b.Write(txt_combat_move_1_pp.Text);
                b.Write(txt_combat_move_2_pp.Text);
                b.Write(txt_combat_move_3_pp.Text);
                b.Write(txt_combat_move_4_pp.Text);
                b.Write(txt_combat_move_5_pp.Text);

                b.Write(DynamaxEnabled);

                b.Write(txt_inv_qty_1.Text);
                b.Write(chk_inv_held_2.Checked);
                b.Write(txt_inv_qty_2.Text);
                b.Write(txt_inv_name_2.Text);
                b.Write(chk_inv_held_3.Checked);
                b.Write(txt_inv_qty_3.Text);
                b.Write(txt_inv_name_3.Text);
                b.Write(chk_inv_held_4.Checked);
                b.Write(txt_inv_qty_4.Text);
                b.Write(txt_inv_name_4.Text);
                b.Write(chk_inv_held_5.Checked);
                b.Write(txt_inv_qty_5.Text);
                b.Write(txt_inv_name_5.Text);
                b.Write(chk_inv_held_6.Checked);
                b.Write(txt_inv_qty_6.Text);
                b.Write(txt_inv_name_6.Text);
                b.Write(chk_inv_held_7.Checked);
                b.Write(txt_inv_qty_7.Text);
                b.Write(txt_inv_name_7.Text);
                b.Write(chk_inv_held_8.Checked);
                b.Write(txt_inv_qty_8.Text);
                b.Write(txt_inv_name_8.Text);
                b.Write(chk_inv_held_9.Checked);
                b.Write(txt_inv_qty_9.Text);
                b.Write(txt_inv_name_9.Text);
                b.Write(chk_inv_held_10.Checked);
                b.Write(txt_inv_qty_10.Text);
                b.Write(txt_inv_name_10.Text);
                b.Write(chk_inv_held_11.Checked);
                b.Write(txt_inv_qty_11.Text);
                b.Write(txt_inv_name_11.Text);
                b.Write(chk_inv_held_12.Checked);
                b.Write(txt_inv_qty_12.Text);
                b.Write(txt_inv_name_12.Text);
                b.Write(chk_inv_held_13.Checked);
                b.Write(txt_inv_qty_13.Text);
                b.Write(txt_inv_name_13.Text);
                b.Write(chk_inv_held_14.Checked);
                b.Write(txt_inv_qty_14.Text);
                b.Write(txt_inv_name_14.Text);
                b.Write(chk_inv_held_15.Checked);
                b.Write(txt_inv_qty_15.Text);
                b.Write(txt_inv_name_15.Text);
                b.Write(chk_inv_held_16.Checked);
                b.Write(txt_inv_qty_16.Text);
                b.Write(txt_inv_name_16.Text);
                b.Write(chk_inv_held_17.Checked);
                b.Write(txt_inv_qty_17.Text);
                b.Write(txt_inv_name_17.Text);
                b.Write(chk_inv_held_18.Checked);
                b.Write(txt_inv_qty_18.Text);
                b.Write(txt_inv_name_18.Text);
                b.Write(chk_inv_held_19.Checked);
                b.Write(txt_inv_qty_19.Text);
                b.Write(txt_inv_name_19.Text);
                b.Write(chk_inv_held_20.Checked);
                b.Write(txt_inv_qty_20.Text);
                b.Write(txt_inv_name_20.Text);
                b.Write(chk_inv_held_21.Checked);
                b.Write(txt_inv_qty_21.Text);
                b.Write(txt_inv_name_21.Text);
                b.Write(chk_inv_held_22.Checked);
                b.Write(txt_inv_qty_22.Text);
                b.Write(txt_inv_name_22.Text);
                b.Write(chk_inv_held_23.Checked);
                b.Write(txt_inv_qty_23.Text);
                b.Write(txt_inv_name_23.Text);
                b.Write(chk_inv_held_24.Checked);
                b.Write(txt_inv_qty_24.Text);
                b.Write(txt_inv_name_24.Text);
                b.Write(chk_inv_held_25.Checked);
                b.Write(txt_inv_qty_25.Text);
                b.Write(txt_inv_name_25.Text);
                b.Write(chk_inv_held_26.Checked);
                b.Write(txt_inv_qty_26.Text);
                b.Write(txt_inv_name_26.Text);
                b.Write(cmb_inv_bag.Text);
                b.Write(cmb_trait_1.Text);
                b.Write(cmb_trait_2.Text);
                b.Write(cmb_trait_3.Text);
                b.Write(cmb_trait_4.Text);
                b.Write(cmb_trait_5.Text);
                b.Write(cmb_trait_6.Text);
                b.Write(cmb_trait_7.Text);
                b.Write(cmb_trait_8.Text);
                b.Write(cmb_trait_9.Text);
                b.Write(cmb_trait_10.Text);
                b.Write(cmb_trait_11.Text);
                b.Write(cmb_trait_12.Text);
                b.Write(chk_skills_gathering.Checked);
                b.Write(txt_skills_gathering.Text);
                b.Write(chk_skills_tracking.Checked);
                b.Write(txt_skills_tracking.Text);
                b.Write(chk_skills_medicine.Checked);
                b.Write(txt_skills_medicine.Text);
                b.Write(chk_skills_nature.Checked);
                b.Write(txt_skills_nature.Text);
                b.Write(chk_skills_deception.Checked);
                b.Write(txt_skills_deception.Text);
                b.Write(chk_skills_persuasion.Checked);
                b.Write(txt_skills_persuasion.Text);
                b.Write(chk_skills_intimidation.Checked);
                b.Write(txt_skills_intimidation.Text);
                b.Write(chk_skills_perf.Checked);
                b.Write(txt_skills_perf.Text);
                b.Write(chk_skills_crafting.Checked);
                b.Write(txt_skills_crafting.Text);
                b.Write(chk_skills_app.Checked);
                b.Write(txt_skills_app.Text);
                b.Write(chk_skills_hist.Checked);
                b.Write(txt_skills_hist.Text);
                b.Write(chk_skills_build.Checked);
                b.Write(txt_skills_build.Text);
                b.Write(chk_skills_other1.Checked);
                b.Write(txt_skills_nm_other1.Text);
                b.Write(txt_skills_val_other1.Text);
                b.Write(chk_skills_other2.Checked);
                b.Write(txt_skills_nm_other2.Text);
                b.Write(txt_skills_val_other2.Text);
                b.Write(chk_skills_other3.Checked);
                b.Write(txt_skills_nm_other3.Text);
                b.Write(txt_skills_val_other3.Text);
                b.Write(chk_skills_other4.Checked);
                b.Write(txt_skills_nm_other4.Text);
                b.Write(txt_skills_val_other4.Text);

                b.Write(cmb_nature_name.Text);
                b.Write(txt_flavor_personality.Text);
                b.Write(txt_flavor_relationships.Text);
                b.Write(txt_flavor_ideals.Text);
                b.Write(txt_flavor_flaws.Text);
                b.Write(txt_flavor_journal.Text);

                if (ts_campaign_se_btn.Checked) { b.Write("se"); }
                else if (ts_campaign_dnde_btn.Checked) { b.Write("dnde"); }
                else { b.Write("dnde"); }

                b.Write(imgarr);

            }
            fileio = false;
        }

        private void loadSheetToFile()
        {
            fileio = true;
            List<byte> imgarr = new List<byte>();
            disposeEntireSheet();
            // Load the file
            using (BinaryReader b = new BinaryReader(File.Open(Savefilepath, FileMode.Open)))
            {
                txt_sht_name.Text = b.ReadString();
                txt_sht_age.Text = b.ReadString();
                txt_sht_species.Text = b.ReadString();
                txt_sht_lv.Text = b.ReadString();
                txt_sht_exp.Text = b.ReadString();
                cmb_sht_ability.SelectedIndex = cmb_sht_ability.FindStringExact(b.ReadString());

                txt_stat_belly_curr.Text = b.ReadString();
                txt_stat_belly_max.Text = b.ReadString();
                txt_stat_hp_user.Text = b.ReadString();
                txt_stat_atk_user.Text = b.ReadString();
                txt_stat_def_user.Text = b.ReadString();
                txt_stat_satk_user.Text = b.ReadString();
                txt_stat_sdef_user.Text = b.ReadString();
                txt_stat_spd_user.Text = b.ReadString();
                txt_stat_eva_user.Text = b.ReadString();
                txt_stat_fort_user.Text = b.ReadString();

                ctr_stat_hp_stage.Value = b.ReadInt32();
                ctr_stat_atk_stage.Value = b.ReadInt32();
                ctr_stat_def_stage.Value = b.ReadInt32();
                ctr_stat_satk_stage.Value = b.ReadInt32();
                ctr_stat_sdef_stage.Value = b.ReadInt32();
                ctr_stat_spd_stage.Value = b.ReadInt32();
                ctr_stat_eva_stage.Value = b.ReadInt32();
                ctr_stat_fort_stage.Value = b.ReadInt32();

                txt_inf_ready_morale.Text = b.ReadString();
                txt_inf_ready_ing.Text = b.ReadString();
                txt_inf_ready_insp.Text = b.ReadString();
                txt_inf_ready_luck.Text = b.ReadString();
                txt_inf_base_morale.Text = b.ReadString();
                txt_inf_base_ing.Text = b.ReadString();
                txt_inf_base_insp.Text = b.ReadString();
                txt_inf_base_luck.Text = b.ReadString();

                ClkInfliction = b.ReadInt32();
                Clkstun = b.ReadInt32();
                Clkmez = b.ReadInt32();
                Clkdoom = b.ReadInt32();
                Clkmisc1 = b.ReadInt32();
                Clkmisc2 = b.ReadInt32();
                Clkmisc3 = b.ReadInt32();
                clkChange();

                txt_clocks_misc1.Text = b.ReadString();
                txt_clocks_misc2.Text = b.ReadString();
                txt_clocks_misc3.Text = b.ReadString();

                string[] combatnames = {"","","",""};
                combatnames[0] = b.ReadString();
                combatnames[1] = b.ReadString();
                combatnames[2] = b.ReadString();
                combatnames[3] = b.ReadString();
                // Match combat name to learnset list and checkbox accordingly
                for (int carr_iter = 0; carr_iter < 4; carr_iter++)
                {
                    for (int move_iter = 0; move_iter < pkmn_moveset_txt_move_name.Count; move_iter++)
                    {
                        if (pkmn_moveset_txt_move_name[move_iter].Text == combatnames[carr_iter])
                        {
                            pkmn_moveset_chk_move_selected[move_iter].Checked = true;
                            break;
                        }
                    }
                }
                txt_combat_move_5_name.Text = b.ReadString();
                txt_combat_move_1_pp.Text = b.ReadString();
                txt_combat_move_2_pp.Text = b.ReadString();
                txt_combat_move_3_pp.Text = b.ReadString();
                txt_combat_move_4_pp.Text = b.ReadString();
                txt_combat_move_5_pp.Text = b.ReadString();

                bool dynamax_load;
                dynamax_load = b.ReadBoolean();
                if (dynamax_load != DynamaxEnabled) { toggleDynamax(); }

                txt_inv_qty_1.Text = b.ReadString();
                chk_inv_held_2.Checked = b.ReadBoolean();
                txt_inv_qty_2.Text = b.ReadString();
                txt_inv_name_2.Text = b.ReadString();
                chk_inv_held_3.Checked = b.ReadBoolean();
                txt_inv_qty_3.Text = b.ReadString();
                txt_inv_name_3.Text = b.ReadString();
                chk_inv_held_4.Checked = b.ReadBoolean();
                txt_inv_qty_4.Text = b.ReadString();
                txt_inv_name_4.Text = b.ReadString();
                chk_inv_held_5.Checked = b.ReadBoolean();
                txt_inv_qty_5.Text = b.ReadString();
                txt_inv_name_5.Text = b.ReadString();
                chk_inv_held_6.Checked = b.ReadBoolean();
                txt_inv_qty_6.Text = b.ReadString();
                txt_inv_name_6.Text = b.ReadString();
                chk_inv_held_7.Checked = b.ReadBoolean();
                txt_inv_qty_7.Text = b.ReadString();
                txt_inv_name_7.Text = b.ReadString();
                chk_inv_held_8.Checked = b.ReadBoolean();
                txt_inv_qty_8.Text = b.ReadString();
                txt_inv_name_8.Text = b.ReadString();
                chk_inv_held_9.Checked = b.ReadBoolean();
                txt_inv_qty_9.Text = b.ReadString();
                txt_inv_name_9.Text = b.ReadString();
                chk_inv_held_10.Checked = b.ReadBoolean();
                txt_inv_qty_10.Text = b.ReadString();
                txt_inv_name_10.Text = b.ReadString();
                chk_inv_held_11.Checked = b.ReadBoolean();
                txt_inv_qty_11.Text = b.ReadString();
                txt_inv_name_11.Text = b.ReadString();
                chk_inv_held_12.Checked = b.ReadBoolean();
                txt_inv_qty_12.Text = b.ReadString();
                txt_inv_name_12.Text = b.ReadString();
                chk_inv_held_13.Checked = b.ReadBoolean();
                txt_inv_qty_13.Text = b.ReadString();
                txt_inv_name_13.Text = b.ReadString();
                chk_inv_held_14.Checked = b.ReadBoolean();
                txt_inv_qty_14.Text = b.ReadString();
                txt_inv_name_14.Text = b.ReadString();
                chk_inv_held_15.Checked = b.ReadBoolean();
                txt_inv_qty_15.Text = b.ReadString();
                txt_inv_name_15.Text = b.ReadString();
                chk_inv_held_16.Checked = b.ReadBoolean();
                txt_inv_qty_16.Text = b.ReadString();
                txt_inv_name_16.Text = b.ReadString();
                chk_inv_held_17.Checked = b.ReadBoolean();
                txt_inv_qty_17.Text = b.ReadString();
                txt_inv_name_17.Text = b.ReadString();
                chk_inv_held_18.Checked = b.ReadBoolean();
                txt_inv_qty_18.Text = b.ReadString();
                txt_inv_name_18.Text = b.ReadString();
                chk_inv_held_19.Checked = b.ReadBoolean();
                txt_inv_qty_19.Text = b.ReadString();
                txt_inv_name_19.Text = b.ReadString();
                chk_inv_held_20.Checked = b.ReadBoolean();
                txt_inv_qty_20.Text = b.ReadString();
                txt_inv_name_20.Text = b.ReadString();
                chk_inv_held_21.Checked = b.ReadBoolean();
                txt_inv_qty_21.Text = b.ReadString();
                txt_inv_name_21.Text = b.ReadString();
                chk_inv_held_22.Checked = b.ReadBoolean();
                txt_inv_qty_22.Text = b.ReadString();
                txt_inv_name_22.Text = b.ReadString();
                chk_inv_held_23.Checked = b.ReadBoolean();
                txt_inv_qty_23.Text = b.ReadString();
                txt_inv_name_23.Text = b.ReadString();
                chk_inv_held_24.Checked = b.ReadBoolean();
                txt_inv_qty_24.Text = b.ReadString();
                txt_inv_name_24.Text = b.ReadString();
                chk_inv_held_25.Checked = b.ReadBoolean();
                txt_inv_qty_25.Text = b.ReadString();
                txt_inv_name_25.Text = b.ReadString();
                chk_inv_held_26.Checked = b.ReadBoolean();
                txt_inv_qty_26.Text = b.ReadString();
                txt_inv_name_26.Text = b.ReadString();
                cmb_inv_bag.SelectedIndex = cmb_inv_bag.FindStringExact(b.ReadString());
                cmb_trait_1.SelectedIndex = cmb_trait_1.FindStringExact(b.ReadString());
                cmb_trait_2.SelectedIndex = cmb_trait_2.FindStringExact(b.ReadString());
                cmb_trait_3.SelectedIndex = cmb_trait_3.FindStringExact(b.ReadString());
                cmb_trait_4.SelectedIndex = cmb_trait_4.FindStringExact(b.ReadString());
                cmb_trait_5.SelectedIndex = cmb_trait_5.FindStringExact(b.ReadString());
                cmb_trait_6.SelectedIndex = cmb_trait_6.FindStringExact(b.ReadString());
                cmb_trait_7.SelectedIndex = cmb_trait_7.FindStringExact(b.ReadString());
                cmb_trait_8.SelectedIndex = cmb_trait_8.FindStringExact(b.ReadString());
                cmb_trait_9.SelectedIndex = cmb_trait_9.FindStringExact(b.ReadString());
                cmb_trait_10.SelectedIndex = cmb_trait_10.FindStringExact(b.ReadString());
                cmb_trait_11.SelectedIndex = cmb_trait_11.FindStringExact(b.ReadString());
                cmb_trait_12.SelectedIndex = cmb_trait_12.FindStringExact(b.ReadString());
                chk_skills_gathering.Checked = b.ReadBoolean();
                txt_skills_gathering.Text = b.ReadString();
                chk_skills_tracking.Checked = b.ReadBoolean();
                txt_skills_tracking.Text = b.ReadString();
                chk_skills_medicine.Checked = b.ReadBoolean();
                txt_skills_medicine.Text = b.ReadString();
                chk_skills_nature.Checked = b.ReadBoolean();
                txt_skills_nature.Text = b.ReadString();
                chk_skills_deception.Checked = b.ReadBoolean();
                txt_skills_deception.Text = b.ReadString();
                chk_skills_persuasion.Checked = b.ReadBoolean();
                txt_skills_persuasion.Text = b.ReadString();
                chk_skills_intimidation.Checked = b.ReadBoolean();
                txt_skills_intimidation.Text = b.ReadString();
                chk_skills_perf.Checked = b.ReadBoolean();
                txt_skills_perf.Text = b.ReadString();
                chk_skills_crafting.Checked = b.ReadBoolean();
                txt_skills_crafting.Text = b.ReadString();
                chk_skills_app.Checked = b.ReadBoolean();
                txt_skills_app.Text = b.ReadString();
                chk_skills_hist.Checked = b.ReadBoolean();
                txt_skills_hist.Text = b.ReadString();
                chk_skills_build.Checked = b.ReadBoolean();
                txt_skills_build.Text = b.ReadString();
                chk_skills_other1.Checked = b.ReadBoolean();
                txt_skills_nm_other1.Text = b.ReadString();
                txt_skills_val_other1.Text = b.ReadString();
                chk_skills_other2.Checked = b.ReadBoolean();
                txt_skills_nm_other2.Text = b.ReadString();
                txt_skills_val_other2.Text = b.ReadString();
                chk_skills_other3.Checked = b.ReadBoolean();
                txt_skills_nm_other3.Text = b.ReadString();
                txt_skills_val_other3.Text = b.ReadString();
                chk_skills_other4.Checked = b.ReadBoolean();
                txt_skills_nm_other4.Text = b.ReadString();
                txt_skills_val_other4.Text = b.ReadString();

                cmb_nature_name.SelectedIndex = cmb_nature_name.FindStringExact(b.ReadString());
                txt_flavor_personality.Text = b.ReadString();
                txt_flavor_relationships.Text = b.ReadString();
                txt_flavor_ideals.Text = b.ReadString();
                txt_flavor_flaws.Text = b.ReadString();
                txt_flavor_journal.Text = b.ReadString();

                string campaign = b.ReadString();
                toggleCampaign(campaign);

                try
                {
                    while (b.BaseStream.Position < b.BaseStream.Length) { imgarr.Add(b.ReadByte()); }
                }
                catch (IOException e)
                {
                    throw;
                }
            }
            // Load image from byte array
            using (MemoryStream ms = new MemoryStream(imgarr.ToArray()))
            {
                pic_flavor_appearance.Image = System.Drawing.Image.FromStream(ms);
            }
            fileio = false;
        }
        private void ts_file_new_btn_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to start a new sheet? All unsaved data will be lost.", "Start New Sheet?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                disposeEntireSheet();
            }
        }

        private void recentFilePathRead()
        {
            ts_file_recentfiles_1_btn.Text = "1: " + Properties.Settings.Default.RecentFile1;
            ts_file_recentfiles_2_btn.Text = "2: " + Properties.Settings.Default.RecentFile2;
            ts_file_recentfiles_3_btn.Text = "3: " + Properties.Settings.Default.RecentFile3;
            ts_file_recentfiles_4_btn.Text = "4: " + Properties.Settings.Default.RecentFile4;
            if (String.IsNullOrWhiteSpace(Properties.Settings.Default.RecentFile1)) { ts_file_recentfiles_1_btn.Visible = false; } else { ts_file_recentfiles_1_btn.Visible = true; }
            if (String.IsNullOrWhiteSpace(Properties.Settings.Default.RecentFile2)) { ts_file_recentfiles_2_btn.Visible = false; } else { ts_file_recentfiles_2_btn.Visible = true; }
            if (String.IsNullOrWhiteSpace(Properties.Settings.Default.RecentFile3)) { ts_file_recentfiles_3_btn.Visible = false; } else { ts_file_recentfiles_3_btn.Visible = true; }
            if (String.IsNullOrWhiteSpace(Properties.Settings.Default.RecentFile4)) { ts_file_recentfiles_4_btn.Visible = false; } else { ts_file_recentfiles_4_btn.Visible = true; }
        }

        private void recentFilePathWrite()
        {
            string[] arr_prev = { Properties.Settings.Default.RecentFile1, Properties.Settings.Default.RecentFile2, Properties.Settings.Default.RecentFile3, Properties.Settings.Default.RecentFile4 };
            string[] arr_new = { "", "", "", "" };
            int arr_counter = 0; 
            for (int i = 0; i < 4; i++)
            {
                if (Savefilepath != arr_prev[i]) {
                    arr_new[arr_counter] = arr_prev[i];
                    arr_counter++;
                }
            }
            Properties.Settings.Default.RecentFile4 = arr_new[2];
            Properties.Settings.Default.RecentFile3 = arr_new[1];
            Properties.Settings.Default.RecentFile2 = arr_new[0];
            Properties.Settings.Default.RecentFile1 = Savefilepath;
            Properties.Settings.Default.Save();
            recentFilePathRead();
        }

        private void recentFileRemove(int index)
        {
            string[] arr_prev = { Properties.Settings.Default.RecentFile1, Properties.Settings.Default.RecentFile2, Properties.Settings.Default.RecentFile3, Properties.Settings.Default.RecentFile4 };
            string[] arr_new = { "", "", "", "" };
            int arr_counter = 0;
            for (int i = 0; i < 4; i++)
            {
                if (i != index)
                {
                    arr_new[arr_counter] = arr_prev[i];
                    arr_counter++;
                }
            }
            Properties.Settings.Default.RecentFile4 = arr_new[3];
            Properties.Settings.Default.RecentFile3 = arr_new[2];
            Properties.Settings.Default.RecentFile2 = arr_new[1];
            Properties.Settings.Default.RecentFile1 = arr_new[0];
            Properties.Settings.Default.Save();
            recentFilePathRead();
        }

        private void ts_save_file_dialog(bool force_show = false)
        {
            if (String.IsNullOrWhiteSpace(Savefilepath) || force_show)
            {
                SaveFileDialog dlgSaveFileDialog = new SaveFileDialog();
                dlgSaveFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
                dlgSaveFileDialog.RestoreDirectory = true;
                dlgSaveFileDialog.Title = "Save Character Sheet";
                dlgSaveFileDialog.Filter = "PMDnD Character Sheet files (*.pmdnd)|*.pmdnd|All files (*.*)|*.*";
                dlgSaveFileDialog.DefaultExt = "pmdnd";
                dlgSaveFileDialog.CheckPathExists = true;
                if (dlgSaveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Savefilepath = dlgSaveFileDialog.FileName;
                    recentFilePathWrite();
                    writeSheetToFile();
                    MessageBox.Show("Successfully saved to " + dlgSaveFileDialog.FileName);
                }
            }
            else
            {
                recentFilePathWrite();
                writeSheetToFile();
            }
        }

        private void ts_file_saveas_btn_Click(object sender, EventArgs e)
        {
            ts_save_file_dialog(true);
        }

        private void ts_file_save_btn_Click(object sender, EventArgs e)
        {
            ts_save_file_dialog();
        }

        private void Form_Main_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.Alt && e.KeyCode == Keys.S)
            {
                ts_save_file_dialog(true);
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                ts_save_file_dialog();
            }
            else if (e.Control && e.KeyCode == Keys.N)
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to start a new sheet? All unsaved data will be lost.", "Start New Sheet?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    disposeEntireSheet();
                }
            }
        }

        private void ts_open_file_dialog(bool forceload = false)
        {
            if (!forceload)
            {
                OpenFileDialog dlgOpenFileDialog = new OpenFileDialog();
                dlgOpenFileDialog.Multiselect = false;
                dlgOpenFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
                dlgOpenFileDialog.RestoreDirectory = true;
                dlgOpenFileDialog.Title = "Load Character Sheet";
                dlgOpenFileDialog.Filter = "PMDnD Character Sheet files (*.pmdnd)|*.pmdnd|All files (*.*)|*.*";
                dlgOpenFileDialog.DefaultExt = "pmdnd";
                dlgOpenFileDialog.CheckPathExists = true;
                if (dlgOpenFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Savefilepath = dlgOpenFileDialog.FileName;
                        recentFilePathWrite();
                        loadSheetToFile();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                try
                {
                    recentFilePathWrite();
                    loadSheetToFile();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void ts_file_open_btn_Click(object sender, EventArgs e)
        {
            ts_open_file_dialog();
        }

        private void ts_file_exit_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            string charname;
            if (String.IsNullOrWhiteSpace(txt_sht_name.Text)) { charname = "your character"; } else { charname = txt_sht_name.Text; }
            DialogResult dialogResult = MessageBox.Show("Do you want to save changes to " + charname + "?", "PMDnD Character Sheet", MessageBoxButtons.YesNoCancel);
            if (dialogResult == DialogResult.Yes)
            {
                Properties.Settings.Default.Save(); 
                ts_save_file_dialog();
            }
            else if (dialogResult == DialogResult.No)
            {
                //Pass
            }
            else if (dialogResult == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        private void ts_file_about_btn_Click(object sender, EventArgs e)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            MessageBox.Show(
            "Created by @Crococore\n\nVersion "+ fvi.FileVersion);
        }

        private void ts_file_recentfiles_1_btn_Click(object sender, EventArgs e)
        {
            FileInfo fileInfo = new FileInfo(ts_file_recentfiles_1_btn.Text.Substring(3, ts_file_recentfiles_1_btn.Text.Length - 3));
            if (fileInfo.Exists)
            {
                Savefilepath = ts_file_recentfiles_1_btn.Text.Substring(3, ts_file_recentfiles_1_btn.Text.Length - 3);
                ts_open_file_dialog(true);
            }
            else
            {
                recentFileRemove(0);
                MessageBox.Show("File not found.");
            }
        }

        private void ts_file_recentfiles_2_btn_Click(object sender, EventArgs e)
        {
            FileInfo fileInfo = new FileInfo(ts_file_recentfiles_2_btn.Text.Substring(3, ts_file_recentfiles_2_btn.Text.Length - 3));
            if (fileInfo.Exists)
            {
                Savefilepath = ts_file_recentfiles_2_btn.Text.Substring(3, ts_file_recentfiles_2_btn.Text.Length - 3);
                ts_open_file_dialog(true);
            }
            else
            {
                recentFileRemove(1);
                MessageBox.Show("File not found.");
            }
        }

        private void ts_file_recentfiles_3_btn_Click(object sender, EventArgs e)
        {
            FileInfo fileInfo = new FileInfo(ts_file_recentfiles_3_btn.Text.Substring(3, ts_file_recentfiles_3_btn.Text.Length - 3));
            if (fileInfo.Exists)
            {
                Savefilepath = ts_file_recentfiles_3_btn.Text.Substring(3, ts_file_recentfiles_3_btn.Text.Length - 3);
                ts_open_file_dialog(true);
            }
            else
            {
                recentFileRemove(2);
                MessageBox.Show("File not found.");
            }
        }

        private void ts_file_recentfiles_4_btn_Click(object sender, EventArgs e)
        {
            FileInfo fileInfo = new FileInfo(ts_file_recentfiles_4_btn.Text.Substring(3, ts_file_recentfiles_4_btn.Text.Length - 3));
            if (fileInfo.Exists)
            {
                Savefilepath = ts_file_recentfiles_4_btn.Text.Substring(3, ts_file_recentfiles_4_btn.Text.Length - 3);
                ts_open_file_dialog(true);
            }
            else
            {
                recentFileRemove(3);
                MessageBox.Show("File not found.");
            }
        }
    }
}
