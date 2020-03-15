using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace PMD_Tabletop_Sheet
{
    public partial class Form_DamageCalc : Form
    {
        public string dbpath;
        public int atkr_atk;
        public int atkr_satk;
        public int def_def;
        public int def_sdef;
        public float type_multiplier = 1.0f;
        public float boost_multiplier = 1.0f;
        public float reduction_multiplier = 1.0f;
        public float crit_multiplier = 1.0f;
        public float team_multiplier = 1.33f;
        public double final_damage = 0.0f;

        public Form_DamageCalc()
        {
            InitializeComponent();
        }

        private void Form_DamageCalc_Load(object sender, EventArgs e)
        {
            cmb_atkr_move_attr.Items.Add("Physical");
            cmb_atkr_move_attr.Items.Add("Special");
        }

        private void Form_DamageCalc_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
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

        private void calculateAllEffStats()
        {
            bool lvAtkrIsNumeric = int.TryParse(txt_atkr_lv.Text, out int i);
            bool userAtkIsNumeric = int.TryParse(txt_stat_atk_user.Text, out int j);
            bool lvDefIsNumeric = int.TryParse(txt_def_lv.Text, out int k);
            bool userDefIsNumeric = int.TryParse(txt_stat_def_user.Text, out int l);
            if (lvAtkrIsNumeric && userAtkIsNumeric && cmb_atkr_move_attr.Text == "Physical") { txt_stat_atk_eff.Text = calculateEffStat(1, int.Parse(txt_stat_atk_user.Text), ctr_stat_atk_stage.Value).ToString(); }
            else if (lvAtkrIsNumeric && userAtkIsNumeric && cmb_atkr_move_attr.Text == "Special") { txt_stat_atk_eff.Text = calculateEffStat(3, int.Parse(txt_stat_atk_user.Text), ctr_stat_atk_stage.Value).ToString(); }
            if (lvDefIsNumeric && userDefIsNumeric && cmb_atkr_move_attr.Text == "Physical") { txt_stat_def_eff.Text = calculateEffStat(2, int.Parse(txt_stat_def_user.Text), ctr_stat_def_stage.Value).ToString(); }
            else if (lvDefIsNumeric && userDefIsNumeric && cmb_atkr_move_attr.Text == "Special") { txt_stat_def_eff.Text = calculateEffStat(4, int.Parse(txt_stat_def_user.Text), ctr_stat_def_stage.Value).ToString(); }
        }

        private void calculateMaxStats()
        {
            bool lvAtkrIsNumeric = int.TryParse(txt_atkr_lv.Text, out int i);
            bool lvDefIsNumeric = int.TryParse(txt_def_lv.Text, out int j);

            if (lvAtkrIsNumeric && cmb_atkr_move_attr.Text == "Physical") { txt_stat_atk_max.Text = calculateMaxStat(1, atkr_atk, int.Parse(txt_atkr_lv.Text)).ToString(); }
            else if (lvAtkrIsNumeric && cmb_atkr_move_attr.Text == "Special") { txt_stat_atk_max.Text = calculateMaxStat(3, atkr_satk, int.Parse(txt_atkr_lv.Text)).ToString(); }
            if (lvDefIsNumeric && cmb_atkr_move_attr.Text == "Physical") { txt_stat_def_max.Text = calculateMaxStat(2, def_def, int.Parse(txt_def_lv.Text)).ToString(); }
            else if (lvDefIsNumeric && cmb_atkr_move_attr.Text == "Special") { txt_stat_def_max.Text = calculateMaxStat(4, def_sdef, int.Parse(txt_def_lv.Text)).ToString(); }

            txt_stat_atk_user.Text = txt_stat_atk_max.Text;
            txt_stat_def_user.Text = txt_stat_def_max.Text;

            calculateAllEffStats();
        }

        private void txt_atk_species_TextChanged(object sender, EventArgs e)
        {
            using (SQLiteConnection cn = new SQLiteConnection("Data Source=" + dbpath + ";Version=3;New=True;Compress=True;"))
            {
                cn.Open();
                SQLiteCommand sqlite_cmd;
                SQLiteDataReader sqlite_datareader;
                // create a new SQL command:
                sqlite_cmd = cn.CreateCommand();
                // First lets build a SQL-Query again:
                sqlite_cmd.CommandText = "SELECT * FROM Species Where Name = '" + txt_atkr_species.Text + "'";
                // Now the SQLiteCommand object can give us a DataReader-Object:
                sqlite_datareader = sqlite_cmd.ExecuteReader();
                // The SQLiteDataReader allows us to run through the result lines:
                while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
                {
                    // Interpret base stats
                    if (sqlite_datareader.IsDBNull(9)) { atkr_atk = 0; }
                    else { atkr_atk = sqlite_datareader.GetInt32(9); }
                    if (sqlite_datareader.IsDBNull(11)) { atkr_satk = 0; }
                    else { atkr_satk = sqlite_datareader.GetInt32(11); }
                    calculateMaxStats();
                }
                // We are ready, now lets cleanup and close our connection:
                cn.Close();
            }
        }

        private void txt_def_species_TextChanged(object sender, EventArgs e)
        {
            using (SQLiteConnection cn = new SQLiteConnection("Data Source=" + dbpath + ";Version=3;New=True;Compress=True;"))
            {
                cn.Open();
                SQLiteCommand sqlite_cmd;
                SQLiteDataReader sqlite_datareader;
                // create a new SQL command:
                sqlite_cmd = cn.CreateCommand();
                // First lets build a SQL-Query again:
                sqlite_cmd.CommandText = "SELECT * FROM Species Where Name = '" + txt_def_species.Text + "'";
                // Now the SQLiteCommand object can give us a DataReader-Object:
                sqlite_datareader = sqlite_cmd.ExecuteReader();
                // The SQLiteDataReader allows us to run through the result lines:
                while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
                {
                    // Interpret base stats
                    if (sqlite_datareader.IsDBNull(10)) { def_def = 0; }
                    else { def_def = sqlite_datareader.GetInt32(10); }
                    if (sqlite_datareader.IsDBNull(12)) { def_sdef = 0; }
                    else { def_sdef = sqlite_datareader.GetInt32(12); }
                    // Interpret each typing
                    string read_type1; string read_type2;
                    if (sqlite_datareader.IsDBNull(2)) { read_type1 = "---"; }
                    else { read_type1 = sqlite_datareader.GetString(2); }
                    if (sqlite_datareader.IsDBNull(3)) { read_type2 = "---"; }
                    else { read_type2 = sqlite_datareader.GetString(3); }
                    cmb_def_type1.SelectedIndex = cmb_def_type1.FindStringExact(read_type1);
                    cmb_def_type2.SelectedIndex = cmb_def_type2.FindStringExact(read_type2);
                    calculateMaxStats();
                    testStatEffectiveness();
                    calculateFinalDamage();
                }
                // We are ready, now lets cleanup and close our connection:
                cn.Close();
            }
        }

        private void txt_lv_TextChanged(object sender, EventArgs e)
        {
            calculateMaxStats();
            calculateFinalDamage();
        }

        private void txt_stat_user_TextChanged(object sender, EventArgs e)
        {
            calculateAllEffStats();
            calculateFinalDamage();
        }

        private void cmb_atkr_move_attr_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_atkr_move_attr.Text == "Physical")
            {
                lbl_stat_atk.Text = "ATK";
                lbl_stat_def.Text = "DEF";
            }
            else if (cmb_atkr_move_attr.Text == "Special")
            {
                lbl_stat_atk.Text = "S-ATK";
                lbl_stat_def.Text = "S-DEF";
            }
            calculateMaxStats();
        }

        private void loadMoveCombatParams()
        {
            if (!String.IsNullOrWhiteSpace(txt_atkr_move_name.Text))
            {
                using (SQLiteConnection cn = new SQLiteConnection("Data Source=" + dbpath + ";Version=3;New=True;Compress=True;"))
                {
                    cn.Open();
                    SQLiteCommand sqlite_cmd;
                    SQLiteDataReader sqlite_datareader;
                    sqlite_cmd = cn.CreateCommand();
                    sqlite_cmd.CommandText = "Select * from (SELECT * FROM Moves union all SELECT * FROM Dynamax union all SELECT * FROM Gigantamax) Where Name = '" + txt_atkr_move_name.Text + "'";
                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    while (sqlite_datareader.Read())
                    {
                        if (!sqlite_datareader.IsDBNull(4)) { txt_atkr_move_pow.Text = sqlite_datareader.GetInt32(4).ToString(); }
                        if (!sqlite_datareader.IsDBNull(3)) { cmb_atkr_move_attr.SelectedIndex = cmb_atkr_move_attr.FindStringExact(sqlite_datareader.GetString(3)); }
                        if (!sqlite_datareader.IsDBNull(2)) { cmb_atkr_move_type.SelectedIndex = cmb_atkr_move_type.FindStringExact(sqlite_datareader.GetString(2)); }
                    }
                    cn.Close();
                }
            }
        }

        private void txt_atkr_move_name_TextChanged(object sender, EventArgs e)
        {
            loadMoveCombatParams();
            testStatEffectiveness();
            calculateFinalDamage();
        }
        
        private void testStatEffectiveness()
        {
            int typesum = 0;
            if (!String.IsNullOrWhiteSpace(txt_atkr_move_name.Text))
            {
                using (SQLiteConnection cn = new SQLiteConnection("Data Source=" + dbpath + ";Version=3;New=True;Compress=True;"))
                {
                    cn.Open();
                    SQLiteCommand sqlite_cmd;
                    SQLiteDataReader sqlite_datareader;
                    sqlite_cmd = cn.CreateCommand();
                    sqlite_cmd.CommandText = "Select T1.Type as Atk_Type, T2.Type as Def_Type, Effective from Types_Weaknesses TW Join Types_Reference T1 on T1.ID = TW.Atk_Type_ID Join Types_Reference T2 on T2.ID = TW.Def_Type_ID " +
                        "where T1.Type = '" + cmb_atkr_move_type.Text + "' and T2.Type in ('" + cmb_def_type1.Text + "')";
                    if (!String.IsNullOrWhiteSpace(cmb_def_type2.Text)) { sqlite_cmd.CommandText = sqlite_cmd.CommandText.Replace("T2.Type in ('", "T2.Type in ('" + cmb_def_type2.Text + "','"); }
                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    while (sqlite_datareader.Read())
                    {
                        if (!sqlite_datareader.IsDBNull(2)) {
                            if (sqlite_datareader.GetInt32(2) !=0) { typesum += sqlite_datareader.GetInt32(2); }
                            else { typesum = -255; break; }
                        }
                    }
                    cn.Close();
                }
            }

            if (typesum == -255) { type_multiplier = 0.5f; txt_type_effective.Text = "Type Immune!"; lbl_damage_immune_warning.Visible = true; }
            else if (typesum <= -2) { type_multiplier = 0.5f; txt_type_effective.Text = "Little effect!!"; lbl_damage_immune_warning.Visible = false; }
            else if (typesum == -1) { type_multiplier = 0.7f; txt_type_effective.Text = "Not very effective!"; lbl_damage_immune_warning.Visible = false; }
            else if (typesum == 0) { type_multiplier = 1.0f; txt_type_effective.Text = "Normal Effectiveness"; lbl_damage_immune_warning.Visible = false; }
            else if (typesum == 1) { type_multiplier = 1.4f; txt_type_effective.Text = "Super effective!"; lbl_damage_immune_warning.Visible = false; }
            else if (typesum >= 2) { type_multiplier = 1.8f; txt_type_effective.Text = "Extremely effective!!"; lbl_damage_immune_warning.Visible = false; }

        }

        private void chk_atkr_boosted_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_atkr_boosted.Checked) { boost_multiplier = 1.5f; }
            else { boost_multiplier = 1.0f; }
            calculateFinalDamage();
        }

        private void chk_atkr_reduced_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_atkr_reduced.Checked) { reduction_multiplier = 0.5f; }
            else { reduction_multiplier = 1.0f; }
            calculateFinalDamage();
        }

        private void chk_atkr_crit_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_atkr_crit.Checked) { crit_multiplier = 1.5f; }
            else { crit_multiplier = 1.0f; }
            calculateFinalDamage();
        }

        private void chk_def_on_team_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_def_on_team.Checked) { team_multiplier = 1.00f; }
            else { team_multiplier = 1.33f; }
            calculateFinalDamage();
        }

        private void calculateFinalDamage()
        {
            bool allValidNum = false;
            double LV = 0; double ATK = 0; double POW = 0; double DEF = 0;
            if (double.TryParse(txt_atkr_lv.Text, out LV) && double.TryParse(txt_stat_atk_eff.Text, out ATK) && double.TryParse(txt_atkr_move_pow.Text, out POW) && double.TryParse(txt_stat_def_eff.Text, out DEF))
            {
                allValidNum = true;
            }
            if (allValidNum) {

                final_damage = Math.Floor(crit_multiplier * (type_multiplier * (reduction_multiplier * boost_multiplier * ((ATK + POW) * (39168f / 65536f) - (DEF / 2f) +  (50f * Math.Log(((ATK - DEF) / 8.0f + LV + 50f) * 10f)) - 311f) / team_multiplier)));
                if (final_damage < 1) { final_damage = 1; }
                else if (final_damage > 999) { final_damage = 999; }
            }
            lbl_damage.Text = final_damage.ToString();
        }
    }
}
