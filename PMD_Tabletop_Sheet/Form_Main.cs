using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PMD_Tabletop_Sheet
{

    public partial class Form_Main : Form
    {
        public bool DynamaxEnabled; public bool MegaEnabled; public bool ShadowEnabled;
        public int PageSelected;
        public int ClkInfliction; public int Clkstun; public int Clkmez; public int Clkdoom; public int Clkmisc1; public int Clkmisc2; public int Clkmisc3;
        public string Savefilepath; public string dbpath;
        private string[] typeList = { "---", "Normal", "Fire", "Water", "Electric", "Grass", "Ice", "Fighting", "Poison", "Ground", "Flying", "Psychic", "Bug", "Rock", "Ghost", "Dragon", "Dark", "Steel", "Fairy" };
        private int[] pkmn_Stats = { 0, 0, 0, 0, 0, 0 }; // HP, ATK, DEF, SATK, SDEF, SPD
        private string pkmn_EXP_Growth = "Slow";
        private double exp_to_lv;
        private string pkmn_gmax_move = "";
        public List<string> pkmn_moveset_selected = new List<string>();
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
            ShadowEnabled = false;
            PageSelected = 1;

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

            //pnl_move_list.Controls.Add(chk_move_selected_template);
            //pnl_move_list.Controls.Add(txt_move_lv_template);
            //pnl_move_list.Controls.Add(txt_move_name_template);
            //pnl_move_list.Controls.Add(cmb_move_type_template);
            //pnl_move_list.Controls.Add(txt_move_pp_template);

        }

        private void ts_campaign_se_btn_Click(object sender, EventArgs e)
        {
            ts_campaign_dnde_btn.Checked = false;
            ts_campaign_se_btn.Checked = true;
            pic_sht_badge.Image = Properties.Resources.badge_expedition;
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
                this.Size = new System.Drawing.Size(546, 707);
            }
            else if (PageSelected == 3)
            {
                if (ts_campaign_se_btn.Checked) { this.Size = new System.Drawing.Size(619, 500); }
                else if (ts_campaign_dnde_btn.Checked) { this.Size = new System.Drawing.Size(619, 777); }
            }
        }

        private void ts_campaign_dnde_btn_Click(object sender, EventArgs e)
        {
            ts_campaign_se_btn.Checked = false;
            ts_campaign_dnde_btn.Checked = true;
            pic_sht_badge.Image = Properties.Resources.badge_shimmering_outline;
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
                this.Size = new System.Drawing.Size(810, 777);
            }
            else if (PageSelected == 3)
            {
                if (ts_campaign_se_btn.Checked) { this.Size = new System.Drawing.Size(619, 500); }
                else if (ts_campaign_dnde_btn.Checked) { this.Size = new System.Drawing.Size(619, 777); }
            }
        }

        private void toggleMega()
        {
            if (MegaEnabled)
            {
                MegaEnabled = false;
                btn_megaevolve.Image = Properties.Resources.mega_stone_dormant;
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
                    btn_dynamax.Image = Properties.Resources.gmax_plain_dormant;
                }
                if (String.IsNullOrWhiteSpace(txt_sht_ability_effect.Text))
                {
                    MessageBox.Show(txt_sht_species.Text.Substring(5, txt_sht_species.Text.Length - 5) + " has no mega evolution available. If this is an error, contact the developer!");
                    txt_sht_species.Text = txt_sht_species.Text.Substring(5, txt_sht_species.Text.Length - 5);
                }
                else
                {
                    MegaEnabled = true;
                    btn_megaevolve.Image = Properties.Resources.mega_stone_glow;
                }
            }
        }

        private void toggleDynamax()
        {
            if (DynamaxEnabled)
            {
                DynamaxEnabled = false;
                btn_dynamax.Image = Properties.Resources.gmax_plain_dormant;
            }
            else
            {
                if (MegaEnabled)
                {
                    toggleMega();
                }
                DynamaxEnabled = true;
                btn_dynamax.Image = Properties.Resources.gmax_glow;
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
            if (ulong.TryParse(txt_inv_max_qty.Text, out ulong j) && ulong.Parse(txt_inv_max_qty.Text) < curr_inv_qty) { txt_inv_max_qty.BackColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Crimson); }
            else { txt_inv_max_qty.BackColor = System.Drawing.SystemColors.Window; }
        }

        private void clkChange()
        {
            if (ClkInfliction > 9) { btn_clocks_infliction_9.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_infliction_9.Image = Properties.Resources.clock_notch_empty; }
            if (ClkInfliction > 8) { btn_clocks_infliction_8.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_infliction_8.Image = Properties.Resources.clock_notch_empty; }
            if (ClkInfliction > 7) { btn_clocks_infliction_7.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_infliction_7.Image = Properties.Resources.clock_notch_empty; }
            if (ClkInfliction > 6) { btn_clocks_infliction_6.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_infliction_6.Image = Properties.Resources.clock_notch_empty; }
            if (ClkInfliction > 5) { btn_clocks_infliction_5.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_infliction_5.Image = Properties.Resources.clock_notch_empty; }
            if (ClkInfliction > 4) { btn_clocks_infliction_4.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_infliction_4.Image = Properties.Resources.clock_notch_empty; }
            if (ClkInfliction > 3) { btn_clocks_infliction_3.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_infliction_3.Image = Properties.Resources.clock_notch_empty; }
            if (ClkInfliction > 2) { btn_clocks_infliction_2.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_infliction_2.Image = Properties.Resources.clock_notch_empty; }
            if (ClkInfliction > 1) { btn_clocks_infliction_1.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_infliction_1.Image = Properties.Resources.clock_notch_empty; }
            if (ClkInfliction > 0) { btn_clocks_infliction_0.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_infliction_0.Image = Properties.Resources.clock_notch_empty; }

            if (Clkstun > 9) { btn_clocks_stun_9.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_stun_9.Image = Properties.Resources.clock_notch_empty; }
            if (Clkstun > 8) { btn_clocks_stun_8.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_stun_8.Image = Properties.Resources.clock_notch_empty; }
            if (Clkstun > 7) { btn_clocks_stun_7.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_stun_7.Image = Properties.Resources.clock_notch_empty; }
            if (Clkstun > 6) { btn_clocks_stun_6.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_stun_6.Image = Properties.Resources.clock_notch_empty; }
            if (Clkstun > 5) { btn_clocks_stun_5.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_stun_5.Image = Properties.Resources.clock_notch_empty; }
            if (Clkstun > 4) { btn_clocks_stun_4.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_stun_4.Image = Properties.Resources.clock_notch_empty; }
            if (Clkstun > 3) { btn_clocks_stun_3.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_stun_3.Image = Properties.Resources.clock_notch_empty; }
            if (Clkstun > 2) { btn_clocks_stun_2.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_stun_2.Image = Properties.Resources.clock_notch_empty; }
            if (Clkstun > 1) { btn_clocks_stun_1.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_stun_1.Image = Properties.Resources.clock_notch_empty; }
            if (Clkstun > 0) { btn_clocks_stun_0.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_stun_0.Image = Properties.Resources.clock_notch_empty; }

            if (Clkmez > 9) { btn_clocks_mez_9.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_mez_9.Image = Properties.Resources.clock_notch_empty; }
            if (Clkmez > 8) { btn_clocks_mez_8.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_mez_8.Image = Properties.Resources.clock_notch_empty; }
            if (Clkmez > 7) { btn_clocks_mez_7.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_mez_7.Image = Properties.Resources.clock_notch_empty; }
            if (Clkmez > 6) { btn_clocks_mez_6.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_mez_6.Image = Properties.Resources.clock_notch_empty; }
            if (Clkmez > 5) { btn_clocks_mez_5.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_mez_5.Image = Properties.Resources.clock_notch_empty; }
            if (Clkmez > 4) { btn_clocks_mez_4.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_mez_4.Image = Properties.Resources.clock_notch_empty; }
            if (Clkmez > 3) { btn_clocks_mez_3.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_mez_3.Image = Properties.Resources.clock_notch_empty; }
            if (Clkmez > 2) { btn_clocks_mez_2.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_mez_2.Image = Properties.Resources.clock_notch_empty; }
            if (Clkmez > 1) { btn_clocks_mez_1.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_mez_1.Image = Properties.Resources.clock_notch_empty; }
            if (Clkmez > 0) { btn_clocks_mez_0.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_mez_0.Image = Properties.Resources.clock_notch_empty; }

            if (Clkdoom > 9) { btn_clocks_doom_9.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_doom_9.Image = Properties.Resources.clock_notch_empty; }
            if (Clkdoom > 8) { btn_clocks_doom_8.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_doom_8.Image = Properties.Resources.clock_notch_empty; }
            if (Clkdoom > 7) { btn_clocks_doom_7.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_doom_7.Image = Properties.Resources.clock_notch_empty; }
            if (Clkdoom > 6) { btn_clocks_doom_6.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_doom_6.Image = Properties.Resources.clock_notch_empty; }
            if (Clkdoom > 5) { btn_clocks_doom_5.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_doom_5.Image = Properties.Resources.clock_notch_empty; }
            if (Clkdoom > 4) { btn_clocks_doom_4.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_doom_4.Image = Properties.Resources.clock_notch_empty; }
            if (Clkdoom > 3) { btn_clocks_doom_3.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_doom_3.Image = Properties.Resources.clock_notch_empty; }
            if (Clkdoom > 2) { btn_clocks_doom_2.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_doom_2.Image = Properties.Resources.clock_notch_empty; }
            if (Clkdoom > 1) { btn_clocks_doom_1.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_doom_1.Image = Properties.Resources.clock_notch_empty; }
            if (Clkdoom > 0) { btn_clocks_doom_0.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_doom_0.Image = Properties.Resources.clock_notch_empty; }

            if (Clkmisc1 > 9) { btn_clocks_misc1_9.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_misc1_9.Image = Properties.Resources.clock_notch_empty; }
            if (Clkmisc1 > 8) { btn_clocks_misc1_8.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_misc1_8.Image = Properties.Resources.clock_notch_empty; }
            if (Clkmisc1 > 7) { btn_clocks_misc1_7.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_misc1_7.Image = Properties.Resources.clock_notch_empty; }
            if (Clkmisc1 > 6) { btn_clocks_misc1_6.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_misc1_6.Image = Properties.Resources.clock_notch_empty; }
            if (Clkmisc1 > 5) { btn_clocks_misc1_5.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_misc1_5.Image = Properties.Resources.clock_notch_empty; }
            if (Clkmisc1 > 4) { btn_clocks_misc1_4.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_misc1_4.Image = Properties.Resources.clock_notch_empty; }
            if (Clkmisc1 > 3) { btn_clocks_misc1_3.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_misc1_3.Image = Properties.Resources.clock_notch_empty; }
            if (Clkmisc1 > 2) { btn_clocks_misc1_2.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_misc1_2.Image = Properties.Resources.clock_notch_empty; }
            if (Clkmisc1 > 1) { btn_clocks_misc1_1.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_misc1_1.Image = Properties.Resources.clock_notch_empty; }
            if (Clkmisc1 > 0) { btn_clocks_misc1_0.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_misc1_0.Image = Properties.Resources.clock_notch_empty; }

            if (Clkmisc2 > 9) { btn_clocks_misc2_9.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_misc2_9.Image = Properties.Resources.clock_notch_empty; }
            if (Clkmisc2 > 8) { btn_clocks_misc2_8.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_misc2_8.Image = Properties.Resources.clock_notch_empty; }
            if (Clkmisc2 > 7) { btn_clocks_misc2_7.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_misc2_7.Image = Properties.Resources.clock_notch_empty; }
            if (Clkmisc2 > 6) { btn_clocks_misc2_6.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_misc2_6.Image = Properties.Resources.clock_notch_empty; }
            if (Clkmisc2 > 5) { btn_clocks_misc2_5.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_misc2_5.Image = Properties.Resources.clock_notch_empty; }
            if (Clkmisc2 > 4) { btn_clocks_misc2_4.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_misc2_4.Image = Properties.Resources.clock_notch_empty; }
            if (Clkmisc2 > 3) { btn_clocks_misc2_3.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_misc2_3.Image = Properties.Resources.clock_notch_empty; }
            if (Clkmisc2 > 2) { btn_clocks_misc2_2.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_misc2_2.Image = Properties.Resources.clock_notch_empty; }
            if (Clkmisc2 > 1) { btn_clocks_misc2_1.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_misc2_1.Image = Properties.Resources.clock_notch_empty; }
            if (Clkmisc2 > 0) { btn_clocks_misc2_0.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_misc2_0.Image = Properties.Resources.clock_notch_empty; }

            if (Clkmisc3 > 9) { btn_clocks_misc3_9.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_misc3_9.Image = Properties.Resources.clock_notch_empty; }
            if (Clkmisc3 > 8) { btn_clocks_misc3_8.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_misc3_8.Image = Properties.Resources.clock_notch_empty; }
            if (Clkmisc3 > 7) { btn_clocks_misc3_7.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_misc3_7.Image = Properties.Resources.clock_notch_empty; }
            if (Clkmisc3 > 6) { btn_clocks_misc3_6.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_misc3_6.Image = Properties.Resources.clock_notch_empty; }
            if (Clkmisc3 > 5) { btn_clocks_misc3_5.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_misc3_5.Image = Properties.Resources.clock_notch_empty; }
            if (Clkmisc3 > 4) { btn_clocks_misc3_4.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_misc3_4.Image = Properties.Resources.clock_notch_empty; }
            if (Clkmisc3 > 3) { btn_clocks_misc3_3.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_misc3_3.Image = Properties.Resources.clock_notch_empty; }
            if (Clkmisc3 > 2) { btn_clocks_misc3_2.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_misc3_2.Image = Properties.Resources.clock_notch_empty; }
            if (Clkmisc3 > 1) { btn_clocks_misc3_1.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_misc3_1.Image = Properties.Resources.clock_notch_empty; }
            if (Clkmisc3 > 0) { btn_clocks_misc3_0.Image = Properties.Resources.clock_notch_filled; }
            else { btn_clocks_misc3_0.Image = Properties.Resources.clock_notch_empty; }
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
        }

        private void ShowMessage(object sender, EventArgs e)
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
                        else if (sqlite_datareader.IsDBNull(6) && !sqlite_datareader.IsDBNull(8)) { txt_combat_move_prev_effect.Text = sqlite_datareader.GetString(8) + "; Deals damage.";  }
                        else { txt_combat_move_prev_effect.Text = ""; }
                        if (!sqlite_datareader.IsDBNull(3)) { txt_combat_move_prev_atr.Text = sqlite_datareader.GetString(3); }
                    }
                    cn.Close();
                }
            }
        }

        private void loadLearnsetFromSpecies(string species)
        {
            pkmn_moveset_selected.Clear();
            disposeLearnsetElements();
            CheckBox chk_move_selected;
            TextBox txt_move_lv;
            TextBox txt_move_name;
            ComboBox cmb_move_type;
            TextBox txt_move_pp;
            PictureBox pbox_move_attr;
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
                        pkmn_moveset_selected.Add(sqlite_datareader.GetString(0));

                        chk_move_selected = new CheckBox();
                        pkmn_moveset_chk_move_selected.Add(chk_move_selected);
                        chk_move_selected.Name = chk_move_selected_template.Name + pkmn_moveset_chk_move_selected.Count();
                        chk_move_selected.Size = chk_move_selected_template.Size;
                        chk_move_selected.Location = new System.Drawing.Point(chk_move_selected_template.Location.X, chk_move_selected_template.Location.Y + (24 * (pkmn_moveset_chk_move_selected.Count() - 1)));
                        pnl_move_list.Controls.Add(chk_move_selected);

                        txt_move_name = new TextBox();
                        pkmn_moveset_txt_move_name.Add(txt_move_name);
                        txt_move_name.Name = txt_move_name_template.Name + pkmn_moveset_txt_move_name.Count();
                        txt_move_name.Size = txt_move_name_template.Size;
                        txt_move_name.Location = new System.Drawing.Point(txt_move_name_template.Location.X, txt_move_name_template.Location.Y + (24 * (pkmn_moveset_txt_move_name.Count() - 1)));
                        txt_move_name.ReadOnly = txt_move_name_template.ReadOnly;
                        txt_move_name.Text = sqlite_datareader.GetString(0);
                        txt_move_name.MouseHover += new EventHandler(ShowMessage);
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
        }

        private void txt_sht_species_TextChanged(object sender, EventArgs e)
        {
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
                pkmn_moveset_selected.Clear();
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
            System.Drawing.Bitmap attrimage = Properties.Resources.token;
            if (attribute.ToLowerInvariant() == "physical") { attrimage = Properties.Resources.icon_Physical_Attack; }
            else if (attribute.ToLowerInvariant() == "special") { attrimage = Properties.Resources.icon_Special_Attack; }
            else if (attribute.ToLowerInvariant() == "status") { attrimage = Properties.Resources.icon_Status_Attack; }
            return attrimage;
        }

        private void txt_combat_move_prev_atr_TextChanged(object sender, EventArgs e)
        {
            pic_combat_move_prev_atr.Image = determineMoveAttributeImage(txt_combat_move_prev_atr.Text);
        }
    }
}
