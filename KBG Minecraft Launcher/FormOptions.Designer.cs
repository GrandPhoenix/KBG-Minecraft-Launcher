﻿namespace KBG_Minecraft_Launcher
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
            this.tabControl1 = new KBG_Minecraft_Launcher.MyTabControl();
            this.tabPageMisc = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.labelJavaVersion = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownRamMax = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDownRamMin = new System.Windows.Forms.NumericUpDown();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.buttonSetRam = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPageAbout = new System.Windows.Forms.TabPage();
            this.labelAbout = new System.Windows.Forms.Label();
            this.groupBoxLinks = new System.Windows.Forms.GroupBox();
            this.richTextBoxLinks = new System.Windows.Forms.RichTextBox();
            this.tabControl1.SuspendLayout();
            this.tabPageMisc.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRamMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRamMin)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.tabPageAbout.SuspendLayout();
            this.groupBoxLinks.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(501, 277);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 2;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(12, 277);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(330, 15);
            this.label4.TabIndex = 14;
            this.label4.Text = "KBG Minecraft Launcher is made by Phoenix (GrandPhoenix82)";
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Location = new System.Drawing.Point(12, 292);
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
            this.tabControl1.Size = new System.Drawing.Size(564, 260);
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
            this.tabPageMisc.Size = new System.Drawing.Size(558, 236);
            this.tabPageMisc.TabIndex = 0;
            this.tabPageMisc.Text = "Misc.";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.labelJavaVersion);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.comboBox2);
            this.groupBox3.Enabled = false;
            this.groupBox3.Location = new System.Drawing.Point(3, 184);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(550, 47);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Java";
            // 
            // labelJavaVersion
            // 
            this.labelJavaVersion.AutoSize = true;
            this.labelJavaVersion.Location = new System.Drawing.Point(329, 19);
            this.labelJavaVersion.Name = "labelJavaVersion";
            this.labelJavaVersion.Size = new System.Drawing.Size(0, 13);
            this.labelJavaVersion.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(228, 19);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(95, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Your Java version:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 19);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Use java ";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "Java x32 (32 bit)",
            "Java x64 (64 bit)"});
            this.comboBox2.Location = new System.Drawing.Point(83, 16);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 21);
            this.comboBox2.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(3, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(550, 53);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Auto login";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(231, 19);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(310, 21);
            this.comboBox1.TabIndex = 1;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(6, 21);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(117, 17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Auto login to server";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox5);
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Location = new System.Drawing.Point(3, 65);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(550, 105);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Memmory allocation";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.numericUpDownRamMax);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.numericUpDownRamMin);
            this.groupBox5.Location = new System.Drawing.Point(6, 19);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(237, 80);
            this.groupBox5.TabIndex = 16;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Values";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Min. (Xms)";
            // 
            // numericUpDownRamMax
            // 
            this.numericUpDownRamMax.Location = new System.Drawing.Point(111, 45);
            this.numericUpDownRamMax.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDownRamMax.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownRamMax.Name = "numericUpDownRamMax";
            this.numericUpDownRamMax.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownRamMax.TabIndex = 4;
            this.numericUpDownRamMax.Value = new decimal(new int[] {
            511,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Max (Xmx)";
            // 
            // numericUpDownRamMin
            // 
            this.numericUpDownRamMin.Location = new System.Drawing.Point(111, 19);
            this.numericUpDownRamMin.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDownRamMin.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownRamMin.Name = "numericUpDownRamMin";
            this.numericUpDownRamMin.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownRamMin.TabIndex = 3;
            this.numericUpDownRamMin.Value = new decimal(new int[] {
            511,
            0,
            0,
            0});
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.buttonSetRam);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Location = new System.Drawing.Point(249, 17);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(286, 82);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Recommended";
            // 
            // buttonSetRam
            // 
            this.buttonSetRam.Location = new System.Drawing.Point(6, 53);
            this.buttonSetRam.Name = "buttonSetRam";
            this.buttonSetRam.Size = new System.Drawing.Size(79, 23);
            this.buttonSetRam.TabIndex = 2;
            this.buttonSetRam.Text = "Set";
            this.buttonSetRam.UseVisualStyleBackColor = true;
            this.buttonSetRam.Click += new System.EventHandler(this.buttonSetRam_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(274, 34);
            this.label3.TabIndex = 5;
            this.label3.Text = "It is recommended that you set the values to about 33%\r\nof your total memory. Pre" +
                "ss \'Set\' to set this automaticly\r\n";
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
            this.tabPageAbout.Size = new System.Drawing.Size(558, 236);
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
            this.groupBoxLinks.Location = new System.Drawing.Point(6, 147);
            this.groupBoxLinks.Name = "groupBoxLinks";
            this.groupBoxLinks.Size = new System.Drawing.Size(544, 81);
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
            this.richTextBoxLinks.Size = new System.Drawing.Size(538, 62);
            this.richTextBoxLinks.TabIndex = 13;
            this.richTextBoxLinks.Text = "";
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
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRamMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRamMin)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.tabPageAbout.ResumeLayout(false);
            this.groupBoxLinks.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonSetRam;
        private System.Windows.Forms.NumericUpDown numericUpDownRamMax;
        private System.Windows.Forms.NumericUpDown numericUpDownRamMin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button buttonOk;
        private MyTabControl tabControl1;        
        private System.Windows.Forms.TabPage tabPageMisc;
        private System.Windows.Forms.TabPage tabPageAbout;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label labelJavaVersion;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.GroupBox groupBoxLinks;
        private System.Windows.Forms.RichTextBox richTextBoxLinks;
        private System.Windows.Forms.Label labelAbout;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox4;
    }
}