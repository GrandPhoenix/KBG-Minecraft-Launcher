namespace KBG_Launcher
{
    partial class FormOptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOptions));
            this.buttonOk = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.tabControl1 = new KBG_Launcher.MyTabControl();
            this.tabPageMisc = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBoxManualJavaPath = new System.Windows.Forms.CheckBox();
            this.buttonManualJavaPath = new System.Windows.Forms.Button();
            this.textBoxManualJavaPath = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxAutoLoginAddress = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.checkBoxAutoLogin = new System.Windows.Forms.CheckBox();
            this.comboBoxAutoLoginPack = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.radioButtonRatioDefault = new System.Windows.Forms.RadioButton();
            this.radioButtonRatioLinked = new System.Windows.Forms.RadioButton();
            this.labelMemmoryMin = new System.Windows.Forms.Label();
            this.labelMemmoryMax = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelMemmoryText = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.trackBarMemmory = new System.Windows.Forms.TrackBar();
            this.tabPageAbout = new System.Windows.Forms.TabPage();
            this.labelAbout = new System.Windows.Forms.Label();
            this.groupBoxLinks = new System.Windows.Forms.GroupBox();
            this.richTextBoxLinks = new System.Windows.Forms.RichTextBox();
            this.tabControl1.SuspendLayout();
            this.tabPageMisc.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMemmory)).BeginInit();
            this.tabPageAbout.SuspendLayout();
            this.groupBoxLinks.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(501, 281);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 2;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(12, 280);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(330, 15);
            this.label4.TabIndex = 14;
            this.label4.Text = "KBG Minecraft Launcher was made by Phoenix (GrandPhoenix82)";
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Location = new System.Drawing.Point(12, 295);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(60, 13);
            this.labelVersion.TabIndex = 15;
            this.labelVersion.Text = "Version 0.1";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageMisc);
            this.tabControl1.Controls.Add(this.tabPageAbout);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(564, 263);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPageMisc
            // 
            this.tabPageMisc.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageMisc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabPageMisc.Controls.Add(this.groupBox3);
            this.tabPageMisc.Controls.Add(this.groupBox1);
            this.tabPageMisc.Controls.Add(this.groupBox2);
            this.tabPageMisc.Location = new System.Drawing.Point(2, 21);
            this.tabPageMisc.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageMisc.Name = "tabPageMisc";
            this.tabPageMisc.Size = new System.Drawing.Size(558, 239);
            this.tabPageMisc.TabIndex = 0;
            this.tabPageMisc.Text = "Misc.";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBoxManualJavaPath);
            this.groupBox3.Controls.Add(this.buttonManualJavaPath);
            this.groupBox3.Controls.Add(this.textBoxManualJavaPath);
            this.groupBox3.Location = new System.Drawing.Point(3, 188);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(550, 46);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Manual Java Path";
            // 
            // checkBoxManualJavaPath
            // 
            this.checkBoxManualJavaPath.AutoSize = true;
            this.checkBoxManualJavaPath.Location = new System.Drawing.Point(6, 18);
            this.checkBoxManualJavaPath.Name = "checkBoxManualJavaPath";
            this.checkBoxManualJavaPath.Size = new System.Drawing.Size(51, 17);
            this.checkBoxManualJavaPath.TabIndex = 5;
            this.checkBoxManualJavaPath.Text = "Use  ";
            this.checkBoxManualJavaPath.UseVisualStyleBackColor = true;
            // 
            // buttonManualJavaPath
            // 
            this.buttonManualJavaPath.Location = new System.Drawing.Point(482, 14);
            this.buttonManualJavaPath.Name = "buttonManualJavaPath";
            this.buttonManualJavaPath.Size = new System.Drawing.Size(61, 23);
            this.buttonManualJavaPath.TabIndex = 4;
            this.buttonManualJavaPath.Text = "Find";
            this.buttonManualJavaPath.UseVisualStyleBackColor = true;
            this.buttonManualJavaPath.Click += new System.EventHandler(this.buttonManualJavaPath_Click);
            // 
            // textBoxManualJavaPath
            // 
            this.textBoxManualJavaPath.Location = new System.Drawing.Point(63, 16);
            this.textBoxManualJavaPath.Name = "textBoxManualJavaPath";
            this.textBoxManualJavaPath.Size = new System.Drawing.Size(413, 20);
            this.textBoxManualJavaPath.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.textBoxAutoLoginAddress);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.checkBoxAutoLogin);
            this.groupBox1.Controls.Add(this.comboBoxAutoLoginPack);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(3, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(550, 65);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Auto login";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(237, 42);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(307, 13);
            this.label10.TabIndex = 11;
            this.label10.Text = "Leave the address blank to disable auto-login for specific packs";
            // 
            // textBoxAutoLoginAddress
            // 
            this.textBoxAutoLoginAddress.Location = new System.Drawing.Point(423, 19);
            this.textBoxAutoLoginAddress.Name = "textBoxAutoLoginAddress";
            this.textBoxAutoLoginAddress.Size = new System.Drawing.Size(121, 20);
            this.textBoxAutoLoginAddress.TabIndex = 10;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(335, 22);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(82, 13);
            this.label11.TabIndex = 9;
            this.label11.Text = "auto-connect to";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(138, 22);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(64, 13);
            this.label12.TabIndex = 8;
            this.label12.Text = "When using";
            // 
            // checkBoxAutoLogin
            // 
            this.checkBoxAutoLogin.AutoSize = true;
            this.checkBoxAutoLogin.Location = new System.Drawing.Point(6, 21);
            this.checkBoxAutoLogin.Name = "checkBoxAutoLogin";
            this.checkBoxAutoLogin.Size = new System.Drawing.Size(98, 17);
            this.checkBoxAutoLogin.TabIndex = 7;
            this.checkBoxAutoLogin.Text = "Use Auto-login:";
            this.checkBoxAutoLogin.UseVisualStyleBackColor = true;
            // 
            // comboBoxAutoLoginPack
            // 
            this.comboBoxAutoLoginPack.FormattingEnabled = true;
            this.comboBoxAutoLoginPack.Location = new System.Drawing.Point(208, 19);
            this.comboBoxAutoLoginPack.Name = "comboBoxAutoLoginPack";
            this.comboBoxAutoLoginPack.Size = new System.Drawing.Size(121, 21);
            this.comboBoxAutoLoginPack.TabIndex = 6;
            this.comboBoxAutoLoginPack.Text = "Industrial Rage";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox6);
            this.groupBox2.Controls.Add(this.labelMemmoryMin);
            this.groupBox2.Controls.Add(this.labelMemmoryMax);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.labelMemmoryText);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.trackBarMemmory);
            this.groupBox2.Location = new System.Drawing.Point(3, 77);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(550, 105);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Memmory allocation";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.radioButtonRatioDefault);
            this.groupBox6.Controls.Add(this.radioButtonRatioLinked);
            this.groupBox6.Location = new System.Drawing.Point(438, 19);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(106, 80);
            this.groupBox6.TabIndex = 24;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Min : Max ratio";
            // 
            // radioButtonRatioDefault
            // 
            this.radioButtonRatioDefault.AutoSize = true;
            this.radioButtonRatioDefault.Checked = true;
            this.radioButtonRatioDefault.Location = new System.Drawing.Point(13, 27);
            this.radioButtonRatioDefault.Name = "radioButtonRatioDefault";
            this.radioButtonRatioDefault.Size = new System.Drawing.Size(81, 17);
            this.radioButtonRatioDefault.TabIndex = 22;
            this.radioButtonRatioDefault.TabStop = true;
            this.radioButtonRatioDefault.Text = "1:2 (default)";
            this.radioButtonRatioDefault.UseVisualStyleBackColor = true;
            this.radioButtonRatioDefault.CheckedChanged += new System.EventHandler(this.radioButtonRatioDefault_CheckedChanged);
            // 
            // radioButtonRatioLinked
            // 
            this.radioButtonRatioLinked.AutoSize = true;
            this.radioButtonRatioLinked.Location = new System.Drawing.Point(13, 50);
            this.radioButtonRatioLinked.Name = "radioButtonRatioLinked";
            this.radioButtonRatioLinked.Size = new System.Drawing.Size(71, 17);
            this.radioButtonRatioLinked.TabIndex = 23;
            this.radioButtonRatioLinked.Text = "1:1 linked";
            this.radioButtonRatioLinked.UseVisualStyleBackColor = true;
            this.radioButtonRatioLinked.CheckedChanged += new System.EventHandler(this.radioButtonRatioLinked_CheckedChanged);
            // 
            // labelMemmoryMin
            // 
            this.labelMemmoryMin.Location = new System.Drawing.Point(352, 16);
            this.labelMemmoryMin.Name = "labelMemmoryMin";
            this.labelMemmoryMin.Size = new System.Drawing.Size(80, 20);
            this.labelMemmoryMin.TabIndex = 20;
            this.labelMemmoryMin.Text = "Min. (Xms)";
            this.labelMemmoryMin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelMemmoryMax
            // 
            this.labelMemmoryMax.Location = new System.Drawing.Point(352, 36);
            this.labelMemmoryMax.Name = "labelMemmoryMax";
            this.labelMemmoryMax.Size = new System.Drawing.Size(80, 20);
            this.labelMemmoryMax.TabIndex = 21;
            this.labelMemmoryMax.Text = "Max (Xmx)";
            this.labelMemmoryMax.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(286, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Min. (Xms):";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelMemmoryText
            // 
            this.labelMemmoryText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelMemmoryText.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelMemmoryText.Location = new System.Drawing.Point(8, 65);
            this.labelMemmoryText.Name = "labelMemmoryText";
            this.labelMemmoryText.Size = new System.Drawing.Size(424, 32);
            this.labelMemmoryText.TabIndex = 19;
            this.labelMemmoryText.Text = "label5 long label text";
            this.labelMemmoryText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(286, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Max (Xmx):";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trackBarMemmory
            // 
            this.trackBarMemmory.LargeChange = 100;
            this.trackBarMemmory.Location = new System.Drawing.Point(12, 19);
            this.trackBarMemmory.Maximum = 512;
            this.trackBarMemmory.Minimum = 512;
            this.trackBarMemmory.Name = "trackBarMemmory";
            this.trackBarMemmory.Size = new System.Drawing.Size(268, 45);
            this.trackBarMemmory.TabIndex = 17;
            this.trackBarMemmory.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trackBarMemmory.Value = 512;
            this.trackBarMemmory.Scroll += new System.EventHandler(this.trackBarMemmory_Scroll);
            // 
            // tabPageAbout
            // 
            this.tabPageAbout.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageAbout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabPageAbout.Controls.Add(this.labelAbout);
            this.tabPageAbout.Controls.Add(this.groupBoxLinks);
            this.tabPageAbout.Location = new System.Drawing.Point(2, 21);
            this.tabPageAbout.Name = "tabPageAbout";
            this.tabPageAbout.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAbout.Size = new System.Drawing.Size(558, 239);
            this.tabPageAbout.TabIndex = 1;
            this.tabPageAbout.Text = "About";
            // 
            // labelAbout
            // 
            this.labelAbout.Location = new System.Drawing.Point(6, 3);
            this.labelAbout.Name = "labelAbout";
            this.labelAbout.Size = new System.Drawing.Size(541, 87);
            this.labelAbout.TabIndex = 13;
            this.labelAbout.Text = "\r\nThanks to Ancalgon for many of the ideas and information I used to make this pr" +
                "ogram.\r\nOh. and ofc. thanks to google :P";
            // 
            // groupBoxLinks
            // 
            this.groupBoxLinks.Controls.Add(this.richTextBoxLinks);
            this.groupBoxLinks.Location = new System.Drawing.Point(6, 117);
            this.groupBoxLinks.Name = "groupBoxLinks";
            this.groupBoxLinks.Size = new System.Drawing.Size(544, 111);
            this.groupBoxLinks.TabIndex = 12;
            this.groupBoxLinks.TabStop = false;
            this.groupBoxLinks.Text = "Links";
            // 
            // richTextBoxLinks
            // 
            this.richTextBoxLinks.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxLinks.Cursor = System.Windows.Forms.Cursors.Default;
            this.richTextBoxLinks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxLinks.Location = new System.Drawing.Point(3, 16);
            this.richTextBoxLinks.Name = "richTextBoxLinks";
            this.richTextBoxLinks.ReadOnly = true;
            this.richTextBoxLinks.Size = new System.Drawing.Size(538, 92);
            this.richTextBoxLinks.TabIndex = 13;
            this.richTextBoxLinks.Text = "";
            this.richTextBoxLinks.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBoxLinks_LinkClicked);
            // 
            // FormOptions
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(588, 312);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormOptions";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormOptions_FormClosing);
            this.Load += new System.EventHandler(this.FormOptions_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageMisc.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMemmory)).EndInit();
            this.tabPageAbout.ResumeLayout(false);
            this.groupBoxLinks.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonOk;
        private MyTabControl tabControl1;        
        private System.Windows.Forms.TabPage tabPageMisc;
        private System.Windows.Forms.TabPage tabPageAbout;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBoxLinks;
        private System.Windows.Forms.RichTextBox richTextBoxLinks;
        private System.Windows.Forms.Label labelAbout;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBoxAutoLoginAddress;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox checkBoxAutoLogin;
        private System.Windows.Forms.ComboBox comboBoxAutoLoginPack;
        private System.Windows.Forms.TrackBar trackBarMemmory;
        private System.Windows.Forms.Label labelMemmoryText;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.RadioButton radioButtonRatioDefault;
        private System.Windows.Forms.RadioButton radioButtonRatioLinked;
        private System.Windows.Forms.Label labelMemmoryMin;
        private System.Windows.Forms.Label labelMemmoryMax;
        private System.Windows.Forms.CheckBox checkBoxManualJavaPath;
        private System.Windows.Forms.Button buttonManualJavaPath;
        private System.Windows.Forms.TextBox textBoxManualJavaPath;
    }
}