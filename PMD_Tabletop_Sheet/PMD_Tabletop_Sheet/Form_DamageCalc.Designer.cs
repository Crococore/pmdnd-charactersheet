namespace PMD_Tabletop_Sheet
{
    partial class Form_DamageCalc
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbl_atkr_species = new System.Windows.Forms.Label();
            this.txt_atkr_species = new System.Windows.Forms.TextBox();
            this.lbl_atkr_lv = new System.Windows.Forms.Label();
            this.txt_atkr_lv = new System.Windows.Forms.TextBox();
            this.lbl_stat_atk_stage = new System.Windows.Forms.Label();
            this.ctr_stat_atk_stage = new System.Windows.Forms.NumericUpDown();
            this.lbl_stat_atk = new System.Windows.Forms.Label();
            this.txt_stat_atk_user = new System.Windows.Forms.TextBox();
            this.lbl_atkr_move_pow = new System.Windows.Forms.Label();
            this.txt_atkr_move_pow = new System.Windows.Forms.TextBox();
            this.lbl_atkr_move_name = new System.Windows.Forms.Label();
            this.txt_atkr_move_name = new System.Windows.Forms.TextBox();
            this.lbl_attacker = new System.Windows.Forms.Label();
            this.lbl_atkr_move_attr = new System.Windows.Forms.Label();
            this.cmb_atkr_move_type = new System.Windows.Forms.ComboBox();
            this.txt_stat_atk_max = new System.Windows.Forms.TextBox();
            this.txt_stat_atk_eff = new System.Windows.Forms.TextBox();
            this.lbl_stat_atk_max = new System.Windows.Forms.Label();
            this.lbl_stat_atk_eff = new System.Windows.Forms.Label();
            this.chk_atkr_crit = new System.Windows.Forms.CheckBox();
            this.chk_atkr_boosted = new System.Windows.Forms.CheckBox();
            this.lbl_damage = new System.Windows.Forms.Label();
            this.lbl_damage_title = new System.Windows.Forms.Label();
            this.txt_def_lv = new System.Windows.Forms.TextBox();
            this.lbl_def_lv = new System.Windows.Forms.Label();
            this.txt_def_species = new System.Windows.Forms.TextBox();
            this.lbl_def_species = new System.Windows.Forms.Label();
            this.txt_stat_def_user = new System.Windows.Forms.TextBox();
            this.lbl_stat_def = new System.Windows.Forms.Label();
            this.ctr_stat_def_stage = new System.Windows.Forms.NumericUpDown();
            this.def_stat_atk_stage = new System.Windows.Forms.Label();
            this.lbl_defender = new System.Windows.Forms.Label();
            this.lbl_stat_def_max = new System.Windows.Forms.Label();
            this.lbl_stat_def_eff = new System.Windows.Forms.Label();
            this.txt_stat_def_eff = new System.Windows.Forms.TextBox();
            this.txt_stat_def_max = new System.Windows.Forms.TextBox();
            this.chk_atkr_reduced = new System.Windows.Forms.CheckBox();
            this.chk_def_on_team = new System.Windows.Forms.CheckBox();
            this.cmb_atkr_move_attr = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lbl_damage_immune_warning = new System.Windows.Forms.Label();
            this.cmb_def_type1 = new System.Windows.Forms.ComboBox();
            this.cmb_def_type2 = new System.Windows.Forms.ComboBox();
            this.lbl_def_type1 = new System.Windows.Forms.Label();
            this.lbl_def_type2 = new System.Windows.Forms.Label();
            this.txt_type_effective = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ctr_stat_atk_stage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctr_stat_def_stage)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_atkr_species
            // 
            this.lbl_atkr_species.AutoSize = true;
            this.lbl_atkr_species.Location = new System.Drawing.Point(9, 87);
            this.lbl_atkr_species.Name = "lbl_atkr_species";
            this.lbl_atkr_species.Size = new System.Drawing.Size(45, 13);
            this.lbl_atkr_species.TabIndex = 100002;
            this.lbl_atkr_species.Text = "Species";
            // 
            // txt_atkr_species
            // 
            this.txt_atkr_species.Location = new System.Drawing.Point(12, 103);
            this.txt_atkr_species.Name = "txt_atkr_species";
            this.txt_atkr_species.Size = new System.Drawing.Size(121, 20);
            this.txt_atkr_species.TabIndex = 100000;
            this.txt_atkr_species.TextChanged += new System.EventHandler(this.txt_atk_species_TextChanged);
            // 
            // lbl_atkr_lv
            // 
            this.lbl_atkr_lv.AutoSize = true;
            this.lbl_atkr_lv.Location = new System.Drawing.Point(136, 87);
            this.lbl_atkr_lv.Name = "lbl_atkr_lv";
            this.lbl_atkr_lv.Size = new System.Drawing.Size(20, 13);
            this.lbl_atkr_lv.TabIndex = 100007;
            this.lbl_atkr_lv.Text = "LV";
            // 
            // txt_atkr_lv
            // 
            this.txt_atkr_lv.Location = new System.Drawing.Point(139, 103);
            this.txt_atkr_lv.MaxLength = 3;
            this.txt_atkr_lv.Name = "txt_atkr_lv";
            this.txt_atkr_lv.Size = new System.Drawing.Size(32, 20);
            this.txt_atkr_lv.TabIndex = 100001;
            this.txt_atkr_lv.TextChanged += new System.EventHandler(this.txt_lv_TextChanged);
            // 
            // lbl_stat_atk_stage
            // 
            this.lbl_stat_atk_stage.AutoSize = true;
            this.lbl_stat_atk_stage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_stat_atk_stage.Location = new System.Drawing.Point(56, 130);
            this.lbl_stat_atk_stage.Name = "lbl_stat_atk_stage";
            this.lbl_stat_atk_stage.Size = new System.Drawing.Size(35, 13);
            this.lbl_stat_atk_stage.TabIndex = 100010;
            this.lbl_stat_atk_stage.Text = "Stage";
            this.lbl_stat_atk_stage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ctr_stat_atk_stage
            // 
            this.ctr_stat_atk_stage.Location = new System.Drawing.Point(59, 145);
            this.ctr_stat_atk_stage.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.ctr_stat_atk_stage.Minimum = new decimal(new int[] {
            6,
            0,
            0,
            -2147483648});
            this.ctr_stat_atk_stage.Name = "ctr_stat_atk_stage";
            this.ctr_stat_atk_stage.Size = new System.Drawing.Size(40, 20);
            this.ctr_stat_atk_stage.TabIndex = 100009;
            this.ctr_stat_atk_stage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ctr_stat_atk_stage.ValueChanged += new System.EventHandler(this.txt_stat_user_TextChanged);
            // 
            // lbl_stat_atk
            // 
            this.lbl_stat_atk.AutoSize = true;
            this.lbl_stat_atk.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_stat_atk.Location = new System.Drawing.Point(9, 130);
            this.lbl_stat_atk.Name = "lbl_stat_atk";
            this.lbl_stat_atk.Size = new System.Drawing.Size(28, 13);
            this.lbl_stat_atk.TabIndex = 100011;
            this.lbl_stat_atk.Text = "ATK";
            // 
            // txt_stat_atk_user
            // 
            this.txt_stat_atk_user.Location = new System.Drawing.Point(12, 145);
            this.txt_stat_atk_user.MaxLength = 6;
            this.txt_stat_atk_user.Multiline = true;
            this.txt_stat_atk_user.Name = "txt_stat_atk_user";
            this.txt_stat_atk_user.Size = new System.Drawing.Size(42, 20);
            this.txt_stat_atk_user.TabIndex = 100008;
            this.txt_stat_atk_user.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_stat_atk_user.TextChanged += new System.EventHandler(this.txt_stat_user_TextChanged);
            // 
            // lbl_atkr_move_pow
            // 
            this.lbl_atkr_move_pow.AutoSize = true;
            this.lbl_atkr_move_pow.Location = new System.Drawing.Point(138, 168);
            this.lbl_atkr_move_pow.Name = "lbl_atkr_move_pow";
            this.lbl_atkr_move_pow.Size = new System.Drawing.Size(37, 13);
            this.lbl_atkr_move_pow.TabIndex = 100022;
            this.lbl_atkr_move_pow.Text = "Power";
            // 
            // txt_atkr_move_pow
            // 
            this.txt_atkr_move_pow.Location = new System.Drawing.Point(139, 184);
            this.txt_atkr_move_pow.Name = "txt_atkr_move_pow";
            this.txt_atkr_move_pow.ReadOnly = true;
            this.txt_atkr_move_pow.Size = new System.Drawing.Size(35, 20);
            this.txt_atkr_move_pow.TabIndex = 100023;
            this.txt_atkr_move_pow.TabStop = false;
            this.txt_atkr_move_pow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lbl_atkr_move_name
            // 
            this.lbl_atkr_move_name.AutoSize = true;
            this.lbl_atkr_move_name.Location = new System.Drawing.Point(9, 168);
            this.lbl_atkr_move_name.Name = "lbl_atkr_move_name";
            this.lbl_atkr_move_name.Size = new System.Drawing.Size(65, 13);
            this.lbl_atkr_move_name.TabIndex = 100024;
            this.lbl_atkr_move_name.Text = "Move Name";
            // 
            // txt_atkr_move_name
            // 
            this.txt_atkr_move_name.Location = new System.Drawing.Point(12, 184);
            this.txt_atkr_move_name.Name = "txt_atkr_move_name";
            this.txt_atkr_move_name.Size = new System.Drawing.Size(121, 20);
            this.txt_atkr_move_name.TabIndex = 100025;
            this.txt_atkr_move_name.TabStop = false;
            this.txt_atkr_move_name.TextChanged += new System.EventHandler(this.txt_atkr_move_name_TextChanged);
            // 
            // lbl_attacker
            // 
            this.lbl_attacker.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_attacker.Location = new System.Drawing.Point(8, 64);
            this.lbl_attacker.Name = "lbl_attacker";
            this.lbl_attacker.Size = new System.Drawing.Size(100, 23);
            this.lbl_attacker.TabIndex = 100026;
            this.lbl_attacker.Text = "Attacker";
            // 
            // lbl_atkr_move_attr
            // 
            this.lbl_atkr_move_attr.AutoSize = true;
            this.lbl_atkr_move_attr.Location = new System.Drawing.Point(9, 209);
            this.lbl_atkr_move_attr.Name = "lbl_atkr_move_attr";
            this.lbl_atkr_move_attr.Size = new System.Drawing.Size(56, 13);
            this.lbl_atkr_move_attr.TabIndex = 100027;
            this.lbl_atkr_move_attr.Text = "Move Attr.";
            // 
            // cmb_atkr_move_type
            // 
            this.cmb_atkr_move_type.Enabled = false;
            this.cmb_atkr_move_type.FormattingEnabled = true;
            this.cmb_atkr_move_type.Location = new System.Drawing.Point(12, 251);
            this.cmb_atkr_move_type.MaxDropDownItems = 19;
            this.cmb_atkr_move_type.Name = "cmb_atkr_move_type";
            this.cmb_atkr_move_type.Size = new System.Drawing.Size(63, 21);
            this.cmb_atkr_move_type.TabIndex = 100028;
            this.cmb_atkr_move_type.TabStop = false;
            this.cmb_atkr_move_type.Text = "Normal";
            // 
            // txt_stat_atk_max
            // 
            this.txt_stat_atk_max.Location = new System.Drawing.Point(103, 145);
            this.txt_stat_atk_max.Name = "txt_stat_atk_max";
            this.txt_stat_atk_max.ReadOnly = true;
            this.txt_stat_atk_max.Size = new System.Drawing.Size(27, 20);
            this.txt_stat_atk_max.TabIndex = 100029;
            this.txt_stat_atk_max.TabStop = false;
            this.txt_stat_atk_max.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txt_stat_atk_eff
            // 
            this.txt_stat_atk_eff.Location = new System.Drawing.Point(139, 145);
            this.txt_stat_atk_eff.Name = "txt_stat_atk_eff";
            this.txt_stat_atk_eff.ReadOnly = true;
            this.txt_stat_atk_eff.Size = new System.Drawing.Size(40, 20);
            this.txt_stat_atk_eff.TabIndex = 100030;
            this.txt_stat_atk_eff.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lbl_stat_atk_max
            // 
            this.lbl_stat_atk_max.AutoSize = true;
            this.lbl_stat_atk_max.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_stat_atk_max.Location = new System.Drawing.Point(102, 129);
            this.lbl_stat_atk_max.Name = "lbl_stat_atk_max";
            this.lbl_stat_atk_max.Size = new System.Drawing.Size(27, 13);
            this.lbl_stat_atk_max.TabIndex = 100031;
            this.lbl_stat_atk_max.Text = "Max";
            this.lbl_stat_atk_max.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbl_stat_atk_eff
            // 
            this.lbl_stat_atk_eff.AutoSize = true;
            this.lbl_stat_atk_eff.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_stat_atk_eff.Location = new System.Drawing.Point(136, 129);
            this.lbl_stat_atk_eff.Name = "lbl_stat_atk_eff";
            this.lbl_stat_atk_eff.Size = new System.Drawing.Size(31, 13);
            this.lbl_stat_atk_eff.TabIndex = 100031;
            this.lbl_stat_atk_eff.Text = "Total";
            this.lbl_stat_atk_eff.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // chk_atkr_crit
            // 
            this.chk_atkr_crit.AutoSize = true;
            this.chk_atkr_crit.Location = new System.Drawing.Point(81, 215);
            this.chk_atkr_crit.Name = "chk_atkr_crit";
            this.chk_atkr_crit.Size = new System.Drawing.Size(73, 17);
            this.chk_atkr_crit.TabIndex = 100032;
            this.chk_atkr_crit.Text = "Critical Hit";
            this.chk_atkr_crit.UseVisualStyleBackColor = true;
            this.chk_atkr_crit.CheckedChanged += new System.EventHandler(this.chk_atkr_crit_CheckedChanged);
            // 
            // chk_atkr_boosted
            // 
            this.chk_atkr_boosted.AutoSize = true;
            this.chk_atkr_boosted.Location = new System.Drawing.Point(81, 234);
            this.chk_atkr_boosted.Name = "chk_atkr_boosted";
            this.chk_atkr_boosted.Size = new System.Drawing.Size(98, 17);
            this.chk_atkr_boosted.TabIndex = 100032;
            this.chk_atkr_boosted.Text = "Boosted Power";
            this.chk_atkr_boosted.UseVisualStyleBackColor = true;
            this.chk_atkr_boosted.CheckedChanged += new System.EventHandler(this.chk_atkr_boosted_CheckedChanged);
            // 
            // lbl_damage
            // 
            this.lbl_damage.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_damage.Location = new System.Drawing.Point(185, 123);
            this.lbl_damage.Name = "lbl_damage";
            this.lbl_damage.Size = new System.Drawing.Size(142, 71);
            this.lbl_damage.TabIndex = 100033;
            this.lbl_damage.Text = "___";
            this.lbl_damage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_damage_title
            // 
            this.lbl_damage_title.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_damage_title.Location = new System.Drawing.Point(185, 103);
            this.lbl_damage_title.Name = "lbl_damage_title";
            this.lbl_damage_title.Size = new System.Drawing.Size(142, 20);
            this.lbl_damage_title.TabIndex = 100002;
            this.lbl_damage_title.Text = "Final Damage";
            this.lbl_damage_title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_def_lv
            // 
            this.txt_def_lv.Location = new System.Drawing.Point(460, 103);
            this.txt_def_lv.MaxLength = 3;
            this.txt_def_lv.Name = "txt_def_lv";
            this.txt_def_lv.Size = new System.Drawing.Size(32, 20);
            this.txt_def_lv.TabIndex = 100001;
            this.txt_def_lv.TextChanged += new System.EventHandler(this.txt_lv_TextChanged);
            // 
            // lbl_def_lv
            // 
            this.lbl_def_lv.AutoSize = true;
            this.lbl_def_lv.Location = new System.Drawing.Point(457, 87);
            this.lbl_def_lv.Name = "lbl_def_lv";
            this.lbl_def_lv.Size = new System.Drawing.Size(20, 13);
            this.lbl_def_lv.TabIndex = 100007;
            this.lbl_def_lv.Text = "LV";
            // 
            // txt_def_species
            // 
            this.txt_def_species.Location = new System.Drawing.Point(333, 103);
            this.txt_def_species.Name = "txt_def_species";
            this.txt_def_species.Size = new System.Drawing.Size(121, 20);
            this.txt_def_species.TabIndex = 100000;
            this.txt_def_species.TextChanged += new System.EventHandler(this.txt_def_species_TextChanged);
            // 
            // lbl_def_species
            // 
            this.lbl_def_species.AutoSize = true;
            this.lbl_def_species.Location = new System.Drawing.Point(330, 87);
            this.lbl_def_species.Name = "lbl_def_species";
            this.lbl_def_species.Size = new System.Drawing.Size(45, 13);
            this.lbl_def_species.TabIndex = 100002;
            this.lbl_def_species.Text = "Species";
            // 
            // txt_stat_def_user
            // 
            this.txt_stat_def_user.Location = new System.Drawing.Point(333, 187);
            this.txt_stat_def_user.MaxLength = 6;
            this.txt_stat_def_user.Multiline = true;
            this.txt_stat_def_user.Name = "txt_stat_def_user";
            this.txt_stat_def_user.Size = new System.Drawing.Size(42, 20);
            this.txt_stat_def_user.TabIndex = 100008;
            this.txt_stat_def_user.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_stat_def_user.TextChanged += new System.EventHandler(this.txt_stat_user_TextChanged);
            // 
            // lbl_stat_def
            // 
            this.lbl_stat_def.AutoSize = true;
            this.lbl_stat_def.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_stat_def.Location = new System.Drawing.Point(330, 172);
            this.lbl_stat_def.Name = "lbl_stat_def";
            this.lbl_stat_def.Size = new System.Drawing.Size(28, 13);
            this.lbl_stat_def.TabIndex = 100011;
            this.lbl_stat_def.Text = "DEF";
            // 
            // ctr_stat_def_stage
            // 
            this.ctr_stat_def_stage.Location = new System.Drawing.Point(380, 187);
            this.ctr_stat_def_stage.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.ctr_stat_def_stage.Minimum = new decimal(new int[] {
            6,
            0,
            0,
            -2147483648});
            this.ctr_stat_def_stage.Name = "ctr_stat_def_stage";
            this.ctr_stat_def_stage.Size = new System.Drawing.Size(40, 20);
            this.ctr_stat_def_stage.TabIndex = 100009;
            this.ctr_stat_def_stage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ctr_stat_def_stage.ValueChanged += new System.EventHandler(this.txt_stat_user_TextChanged);
            // 
            // def_stat_atk_stage
            // 
            this.def_stat_atk_stage.AutoSize = true;
            this.def_stat_atk_stage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.def_stat_atk_stage.Location = new System.Drawing.Point(377, 172);
            this.def_stat_atk_stage.Name = "def_stat_atk_stage";
            this.def_stat_atk_stage.Size = new System.Drawing.Size(35, 13);
            this.def_stat_atk_stage.TabIndex = 100010;
            this.def_stat_atk_stage.Text = "Stage";
            this.def_stat_atk_stage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbl_defender
            // 
            this.lbl_defender.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_defender.Location = new System.Drawing.Point(329, 64);
            this.lbl_defender.Name = "lbl_defender";
            this.lbl_defender.Size = new System.Drawing.Size(100, 23);
            this.lbl_defender.TabIndex = 100026;
            this.lbl_defender.Text = "Defender";
            // 
            // lbl_stat_def_max
            // 
            this.lbl_stat_def_max.AutoSize = true;
            this.lbl_stat_def_max.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_stat_def_max.Location = new System.Drawing.Point(423, 171);
            this.lbl_stat_def_max.Name = "lbl_stat_def_max";
            this.lbl_stat_def_max.Size = new System.Drawing.Size(27, 13);
            this.lbl_stat_def_max.TabIndex = 100031;
            this.lbl_stat_def_max.Text = "Max";
            this.lbl_stat_def_max.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbl_stat_def_eff
            // 
            this.lbl_stat_def_eff.AutoSize = true;
            this.lbl_stat_def_eff.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_stat_def_eff.Location = new System.Drawing.Point(457, 171);
            this.lbl_stat_def_eff.Name = "lbl_stat_def_eff";
            this.lbl_stat_def_eff.Size = new System.Drawing.Size(31, 13);
            this.lbl_stat_def_eff.TabIndex = 100031;
            this.lbl_stat_def_eff.Text = "Total";
            this.lbl_stat_def_eff.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txt_stat_def_eff
            // 
            this.txt_stat_def_eff.Location = new System.Drawing.Point(460, 187);
            this.txt_stat_def_eff.Name = "txt_stat_def_eff";
            this.txt_stat_def_eff.ReadOnly = true;
            this.txt_stat_def_eff.Size = new System.Drawing.Size(40, 20);
            this.txt_stat_def_eff.TabIndex = 100030;
            this.txt_stat_def_eff.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txt_stat_def_max
            // 
            this.txt_stat_def_max.Location = new System.Drawing.Point(424, 187);
            this.txt_stat_def_max.Name = "txt_stat_def_max";
            this.txt_stat_def_max.ReadOnly = true;
            this.txt_stat_def_max.Size = new System.Drawing.Size(27, 20);
            this.txt_stat_def_max.TabIndex = 100029;
            this.txt_stat_def_max.TabStop = false;
            this.txt_stat_def_max.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // chk_atkr_reduced
            // 
            this.chk_atkr_reduced.AutoSize = true;
            this.chk_atkr_reduced.Location = new System.Drawing.Point(81, 253);
            this.chk_atkr_reduced.Name = "chk_atkr_reduced";
            this.chk_atkr_reduced.Size = new System.Drawing.Size(118, 17);
            this.chk_atkr_reduced.TabIndex = 100032;
            this.chk_atkr_reduced.Text = "Damage Reduction";
            this.chk_atkr_reduced.UseVisualStyleBackColor = true;
            this.chk_atkr_reduced.CheckedChanged += new System.EventHandler(this.chk_atkr_reduced_CheckedChanged);
            // 
            // chk_def_on_team
            // 
            this.chk_def_on_team.AutoSize = true;
            this.chk_def_on_team.Location = new System.Drawing.Point(471, 138);
            this.chk_def_on_team.Name = "chk_def_on_team";
            this.chk_def_on_team.Size = new System.Drawing.Size(64, 30);
            this.chk_def_on_team.TabIndex = 100032;
            this.chk_def_on_team.Text = "Team\r\nMember";
            this.chk_def_on_team.UseVisualStyleBackColor = true;
            this.chk_def_on_team.CheckedChanged += new System.EventHandler(this.chk_def_on_team_CheckedChanged);
            // 
            // cmb_atkr_move_attr
            // 
            this.cmb_atkr_move_attr.Enabled = false;
            this.cmb_atkr_move_attr.FormattingEnabled = true;
            this.cmb_atkr_move_attr.Location = new System.Drawing.Point(12, 225);
            this.cmb_atkr_move_attr.MaxDropDownItems = 19;
            this.cmb_atkr_move_attr.Name = "cmb_atkr_move_attr";
            this.cmb_atkr_move_attr.Size = new System.Drawing.Size(63, 21);
            this.cmb_atkr_move_attr.TabIndex = 100028;
            this.cmb_atkr_move_attr.TabStop = false;
            this.cmb_atkr_move_attr.Text = "Physical";
            this.cmb_atkr_move_attr.SelectedIndexChanged += new System.EventHandler(this.cmb_atkr_move_attr_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(12, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(488, 55);
            this.label5.TabIndex = 100002;
            this.label5.Text = "Damage Calculator";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_damage_immune_warning
            // 
            this.lbl_damage_immune_warning.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_damage_immune_warning.Location = new System.Drawing.Point(185, 194);
            this.lbl_damage_immune_warning.Name = "lbl_damage_immune_warning";
            this.lbl_damage_immune_warning.Size = new System.Drawing.Size(142, 20);
            this.lbl_damage_immune_warning.TabIndex = 100002;
            this.lbl_damage_immune_warning.Text = "May be immune!";
            this.lbl_damage_immune_warning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_damage_immune_warning.Visible = false;
            // 
            // cmb_def_type1
            // 
            this.cmb_def_type1.Enabled = false;
            this.cmb_def_type1.FormattingEnabled = true;
            this.cmb_def_type1.Location = new System.Drawing.Point(333, 144);
            this.cmb_def_type1.MaxDropDownItems = 19;
            this.cmb_def_type1.Name = "cmb_def_type1";
            this.cmb_def_type1.Size = new System.Drawing.Size(63, 21);
            this.cmb_def_type1.TabIndex = 100034;
            this.cmb_def_type1.TabStop = false;
            this.cmb_def_type1.Text = "Normal";
            // 
            // cmb_def_type2
            // 
            this.cmb_def_type2.Enabled = false;
            this.cmb_def_type2.FormattingEnabled = true;
            this.cmb_def_type2.Location = new System.Drawing.Point(402, 144);
            this.cmb_def_type2.MaxDropDownItems = 19;
            this.cmb_def_type2.Name = "cmb_def_type2";
            this.cmb_def_type2.Size = new System.Drawing.Size(63, 21);
            this.cmb_def_type2.TabIndex = 100034;
            this.cmb_def_type2.TabStop = false;
            this.cmb_def_type2.Text = "Normal";
            // 
            // lbl_def_type1
            // 
            this.lbl_def_type1.AutoSize = true;
            this.lbl_def_type1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_def_type1.Location = new System.Drawing.Point(330, 128);
            this.lbl_def_type1.Name = "lbl_def_type1";
            this.lbl_def_type1.Size = new System.Drawing.Size(40, 13);
            this.lbl_def_type1.TabIndex = 100011;
            this.lbl_def_type1.Text = "Type 1";
            // 
            // lbl_def_type2
            // 
            this.lbl_def_type2.AutoSize = true;
            this.lbl_def_type2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_def_type2.Location = new System.Drawing.Point(399, 128);
            this.lbl_def_type2.Name = "lbl_def_type2";
            this.lbl_def_type2.Size = new System.Drawing.Size(40, 13);
            this.lbl_def_type2.TabIndex = 100011;
            this.lbl_def_type2.Text = "Type 2";
            // 
            // txt_type_effective
            // 
            this.txt_type_effective.AutoSize = true;
            this.txt_type_effective.Location = new System.Drawing.Point(333, 215);
            this.txt_type_effective.Name = "txt_type_effective";
            this.txt_type_effective.Size = new System.Drawing.Size(107, 13);
            this.txt_type_effective.TabIndex = 100035;
            this.txt_type_effective.Text = "Normal Effectiveness";
            // 
            // Form_DamageCalc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 293);
            this.Controls.Add(this.txt_type_effective);
            this.Controls.Add(this.cmb_def_type2);
            this.Controls.Add(this.cmb_def_type1);
            this.Controls.Add(this.lbl_damage);
            this.Controls.Add(this.chk_atkr_reduced);
            this.Controls.Add(this.chk_atkr_boosted);
            this.Controls.Add(this.chk_def_on_team);
            this.Controls.Add(this.chk_atkr_crit);
            this.Controls.Add(this.txt_stat_def_max);
            this.Controls.Add(this.txt_stat_atk_max);
            this.Controls.Add(this.txt_stat_def_eff);
            this.Controls.Add(this.txt_stat_atk_eff);
            this.Controls.Add(this.lbl_stat_def_eff);
            this.Controls.Add(this.lbl_stat_atk_eff);
            this.Controls.Add(this.lbl_stat_def_max);
            this.Controls.Add(this.lbl_stat_atk_max);
            this.Controls.Add(this.lbl_atkr_move_attr);
            this.Controls.Add(this.cmb_atkr_move_attr);
            this.Controls.Add(this.cmb_atkr_move_type);
            this.Controls.Add(this.lbl_defender);
            this.Controls.Add(this.lbl_attacker);
            this.Controls.Add(this.lbl_atkr_move_pow);
            this.Controls.Add(this.txt_atkr_move_pow);
            this.Controls.Add(this.lbl_atkr_move_name);
            this.Controls.Add(this.def_stat_atk_stage);
            this.Controls.Add(this.txt_atkr_move_name);
            this.Controls.Add(this.ctr_stat_def_stage);
            this.Controls.Add(this.lbl_stat_atk_stage);
            this.Controls.Add(this.lbl_def_type2);
            this.Controls.Add(this.lbl_def_type1);
            this.Controls.Add(this.lbl_stat_def);
            this.Controls.Add(this.ctr_stat_atk_stage);
            this.Controls.Add(this.txt_stat_def_user);
            this.Controls.Add(this.lbl_stat_atk);
            this.Controls.Add(this.txt_stat_atk_user);
            this.Controls.Add(this.lbl_def_species);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lbl_damage_immune_warning);
            this.Controls.Add(this.lbl_damage_title);
            this.Controls.Add(this.txt_def_species);
            this.Controls.Add(this.lbl_atkr_species);
            this.Controls.Add(this.lbl_def_lv);
            this.Controls.Add(this.txt_atkr_species);
            this.Controls.Add(this.txt_def_lv);
            this.Controls.Add(this.lbl_atkr_lv);
            this.Controls.Add(this.txt_atkr_lv);
            this.Name = "Form_DamageCalc";
            this.Text = "Damage Calculator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_DamageCalc_FormClosing);
            this.Load += new System.EventHandler(this.Form_DamageCalc_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ctr_stat_atk_stage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctr_stat_def_stage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label lbl_atkr_species;
        public System.Windows.Forms.TextBox txt_atkr_species;
        public System.Windows.Forms.Label lbl_atkr_lv;
        public System.Windows.Forms.TextBox txt_atkr_lv;
        public System.Windows.Forms.Label lbl_stat_atk_stage;
        public System.Windows.Forms.NumericUpDown ctr_stat_atk_stage;
        public System.Windows.Forms.Label lbl_stat_atk;
        public System.Windows.Forms.TextBox txt_stat_atk_user;
        public System.Windows.Forms.Label lbl_atkr_move_pow;
        public System.Windows.Forms.TextBox txt_atkr_move_pow;
        public System.Windows.Forms.Label lbl_atkr_move_name;
        public System.Windows.Forms.TextBox txt_atkr_move_name;
        public System.Windows.Forms.Label lbl_attacker;
        public System.Windows.Forms.Label lbl_atkr_move_attr;
        public System.Windows.Forms.ComboBox cmb_atkr_move_type;
        public System.Windows.Forms.TextBox txt_stat_atk_max;
        public System.Windows.Forms.TextBox txt_stat_atk_eff;
        public System.Windows.Forms.Label lbl_stat_atk_max;
        public System.Windows.Forms.Label lbl_stat_atk_eff;
        public System.Windows.Forms.CheckBox chk_atkr_crit;
        public System.Windows.Forms.CheckBox chk_atkr_boosted;
        public System.Windows.Forms.Label lbl_damage;
        public System.Windows.Forms.Label lbl_damage_title;
        public System.Windows.Forms.TextBox txt_def_lv;
        public System.Windows.Forms.Label lbl_def_lv;
        public System.Windows.Forms.TextBox txt_def_species;
        public System.Windows.Forms.Label lbl_def_species;
        public System.Windows.Forms.TextBox txt_stat_def_user;
        public System.Windows.Forms.Label lbl_stat_def;
        public System.Windows.Forms.NumericUpDown ctr_stat_def_stage;
        public System.Windows.Forms.Label def_stat_atk_stage;
        public System.Windows.Forms.Label lbl_defender;
        public System.Windows.Forms.Label lbl_stat_def_max;
        public System.Windows.Forms.Label lbl_stat_def_eff;
        public System.Windows.Forms.TextBox txt_stat_def_eff;
        public System.Windows.Forms.TextBox txt_stat_def_max;
        public System.Windows.Forms.CheckBox chk_atkr_reduced;
        public System.Windows.Forms.CheckBox chk_def_on_team;
        public System.Windows.Forms.ComboBox cmb_atkr_move_attr;
        public System.Windows.Forms.Label label5;
        public System.Windows.Forms.Label lbl_damage_immune_warning;
        public System.Windows.Forms.ComboBox cmb_def_type1;
        public System.Windows.Forms.ComboBox cmb_def_type2;
        public System.Windows.Forms.Label lbl_def_type1;
        public System.Windows.Forms.Label lbl_def_type2;
        private System.Windows.Forms.Label txt_type_effective;
    }
}