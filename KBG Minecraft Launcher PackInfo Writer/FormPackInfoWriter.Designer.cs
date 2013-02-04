namespace KBG_Minecraft_Launcher_NewsWriter
{
    partial class FormPackInfoWriter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPackInfoWriter));
            this.buttonSaveAs = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.labelCurrentFile = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBoxVersion = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDownPack = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownRevision = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDownMinor = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownMajor = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxPreventPackDownload = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new KBG_Minecraft_Launcher_NewsWriter.MyTabControl();
            this.tabPageNews = new System.Windows.Forms.TabPage();
            this.groupBoxNews = new System.Windows.Forms.GroupBox();
            this.phoenixRichTextBoxNews = new PhoenixRTB.PhoenixRichTextBox();
            this.tabPageExcludes = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxExclude = new System.Windows.Forms.TextBox();
            this.buttonExcludeAddSingle = new System.Windows.Forms.Button();
            this.buttonAddExclude = new System.Windows.Forms.Button();
            this.buttonRemoveExclude = new System.Windows.Forms.Button();
            this.listBoxExcludes = new System.Windows.Forms.ListBox();
            this.tabPageCredits = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.phoenixRichTextBoxCredits = new PhoenixRTB.PhoenixRichTextBox();
            this.groupBox3.SuspendLayout();
            this.groupBoxVersion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRevision)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMajor)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageNews.SuspendLayout();
            this.groupBoxNews.SuspendLayout();
            this.tabPageExcludes.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPageCredits.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonSaveAs
            // 
            this.buttonSaveAs.Location = new System.Drawing.Point(610, 19);
            this.buttonSaveAs.Name = "buttonSaveAs";
            this.buttonSaveAs.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveAs.TabIndex = 3;
            this.buttonSaveAs.Text = "Save as..";
            this.buttonSaveAs.UseVisualStyleBackColor = true;
            this.buttonSaveAs.Click += new System.EventHandler(this.buttonSaveAs_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Enabled = false;
            this.buttonSave.Location = new System.Drawing.Point(691, 19);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(58, 23);
            this.buttonSave.TabIndex = 4;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonOpen
            // 
            this.buttonOpen.Location = new System.Drawing.Point(755, 19);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(58, 23);
            this.buttonOpen.TabIndex = 5;
            this.buttonOpen.Text = "Open..";
            this.buttonOpen.UseVisualStyleBackColor = true;
            this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
            // 
            // labelCurrentFile
            // 
            this.labelCurrentFile.AutoEllipsis = true;
            this.labelCurrentFile.Location = new System.Drawing.Point(6, 22);
            this.labelCurrentFile.Name = "labelCurrentFile";
            this.labelCurrentFile.Size = new System.Drawing.Size(598, 16);
            this.labelCurrentFile.TabIndex = 6;
            this.labelCurrentFile.Text = "None";
            this.labelCurrentFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.buttonOpen);
            this.groupBox3.Controls.Add(this.buttonSave);
            this.groupBox3.Controls.Add(this.buttonSaveAs);
            this.groupBox3.Controls.Add(this.labelCurrentFile);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(819, 48);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Current File";
            // 
            // groupBoxVersion
            // 
            this.groupBoxVersion.Controls.Add(this.label5);
            this.groupBoxVersion.Controls.Add(this.label4);
            this.groupBoxVersion.Controls.Add(this.numericUpDownPack);
            this.groupBoxVersion.Controls.Add(this.label3);
            this.groupBoxVersion.Controls.Add(this.numericUpDownRevision);
            this.groupBoxVersion.Controls.Add(this.label2);
            this.groupBoxVersion.Controls.Add(this.numericUpDownMinor);
            this.groupBoxVersion.Controls.Add(this.label1);
            this.groupBoxVersion.Controls.Add(this.numericUpDownMajor);
            this.groupBoxVersion.Location = new System.Drawing.Point(12, 66);
            this.groupBoxVersion.Name = "groupBoxVersion";
            this.groupBoxVersion.Size = new System.Drawing.Size(506, 62);
            this.groupBoxVersion.TabIndex = 10;
            this.groupBoxVersion.TabStop = false;
            this.groupBoxVersion.Text = "Version";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(6, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(494, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "* Syntax \"<Major>.<Minor>.<Revision>.<Pack>\".  (x.x.x.y) use x for Mc version. y " +
                "for pack version";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(376, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Pack:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numericUpDownPack
            // 
            this.numericUpDownPack.Location = new System.Drawing.Point(442, 17);
            this.numericUpDownPack.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownPack.Name = "numericUpDownPack";
            this.numericUpDownPack.Size = new System.Drawing.Size(45, 20);
            this.numericUpDownPack.TabIndex = 6;
            this.numericUpDownPack.ValueChanged += new System.EventHandler(this.control_SomethingChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(220, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Revision:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numericUpDownRevision
            // 
            this.numericUpDownRevision.Location = new System.Drawing.Point(286, 17);
            this.numericUpDownRevision.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownRevision.Name = "numericUpDownRevision";
            this.numericUpDownRevision.Size = new System.Drawing.Size(45, 20);
            this.numericUpDownRevision.TabIndex = 4;
            this.numericUpDownRevision.ValueChanged += new System.EventHandler(this.control_SomethingChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(113, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Minor:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numericUpDownMinor
            // 
            this.numericUpDownMinor.Location = new System.Drawing.Point(169, 17);
            this.numericUpDownMinor.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownMinor.Name = "numericUpDownMinor";
            this.numericUpDownMinor.Size = new System.Drawing.Size(45, 20);
            this.numericUpDownMinor.TabIndex = 2;
            this.numericUpDownMinor.ValueChanged += new System.EventHandler(this.control_SomethingChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Major:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numericUpDownMajor
            // 
            this.numericUpDownMajor.Location = new System.Drawing.Point(62, 17);
            this.numericUpDownMajor.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownMajor.Name = "numericUpDownMajor";
            this.numericUpDownMajor.Size = new System.Drawing.Size(45, 20);
            this.numericUpDownMajor.TabIndex = 0;
            this.numericUpDownMajor.ValueChanged += new System.EventHandler(this.control_SomethingChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxPreventPackDownload);
            this.groupBox1.Location = new System.Drawing.Point(524, 66);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(307, 62);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Misc.";
            // 
            // checkBoxPreventPackDownload
            // 
            this.checkBoxPreventPackDownload.AutoSize = true;
            this.checkBoxPreventPackDownload.Location = new System.Drawing.Point(15, 23);
            this.checkBoxPreventPackDownload.Name = "checkBoxPreventPackDownload";
            this.checkBoxPreventPackDownload.Size = new System.Drawing.Size(284, 17);
            this.checkBoxPreventPackDownload.TabIndex = 0;
            this.checkBoxPreventPackDownload.Text = "Prevent the launcher from updating/downloading pack";
            this.checkBoxPreventPackDownload.UseVisualStyleBackColor = true;
            this.checkBoxPreventPackDownload.CheckedChanged += new System.EventHandler(this.control_SomethingChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageNews);
            this.tabControl1.Controls.Add(this.tabPageExcludes);
            this.tabControl1.Controls.Add(this.tabPageCredits);
            this.tabControl1.Location = new System.Drawing.Point(12, 134);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(819, 352);
            this.tabControl1.TabIndex = 12;
            // 
            // tabPageNews
            // 
            this.tabPageNews.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageNews.Controls.Add(this.groupBoxNews);
            this.tabPageNews.Location = new System.Drawing.Point(2, 21);
            this.tabPageNews.Name = "tabPageNews";
            this.tabPageNews.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageNews.Size = new System.Drawing.Size(813, 328);
            this.tabPageNews.TabIndex = 0;
            this.tabPageNews.Text = "News";
            // 
            // groupBoxNews
            // 
            this.groupBoxNews.Controls.Add(this.phoenixRichTextBoxNews);
            this.groupBoxNews.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxNews.Location = new System.Drawing.Point(3, 3);
            this.groupBoxNews.Name = "groupBoxNews";
            this.groupBoxNews.Size = new System.Drawing.Size(807, 322);
            this.groupBoxNews.TabIndex = 0;
            this.groupBoxNews.TabStop = false;
            this.groupBoxNews.Text = "News";
            // 
            // phoenixRichTextBoxNews
            // 
            this.phoenixRichTextBoxNews.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.phoenixRichTextBoxNews.CompactMode = true;
            this.phoenixRichTextBoxNews.Dock = System.Windows.Forms.DockStyle.Fill;
            this.phoenixRichTextBoxNews.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.phoenixRichTextBoxNews.Location = new System.Drawing.Point(3, 16);
            this.phoenixRichTextBoxNews.MinimumSize = new System.Drawing.Size(342, 50);
            this.phoenixRichTextBoxNews.Name = "phoenixRichTextBoxNews";
            this.phoenixRichTextBoxNews.Rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang1033{\\fonttbl{\\f0\\fnil\\fcharset0 Microsoft S" +
                "ans Serif;}}\r\n\\viewkind4\\uc1\\pard\\f0\\fs17\\par\r\n}\r\n";
            this.phoenixRichTextBoxNews.Size = new System.Drawing.Size(801, 303);
            this.phoenixRichTextBoxNews.TabIndex = 0;
            this.phoenixRichTextBoxNews.ToolStripRenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.phoenixRichTextBoxNews.RichTextChanged += new PhoenixRTB.PhoenixRichTextBox.EventHandler(this.control_SomethingChanged);
            // 
            // tabPageExcludes
            // 
            this.tabPageExcludes.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageExcludes.Controls.Add(this.groupBox2);
            this.tabPageExcludes.Location = new System.Drawing.Point(2, 21);
            this.tabPageExcludes.Name = "tabPageExcludes";
            this.tabPageExcludes.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageExcludes.Size = new System.Drawing.Size(813, 328);
            this.tabPageExcludes.TabIndex = 1;
            this.tabPageExcludes.Text = "Excludes";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBoxExclude);
            this.groupBox2.Controls.Add(this.buttonExcludeAddSingle);
            this.groupBox2.Controls.Add(this.buttonAddExclude);
            this.groupBox2.Controls.Add(this.buttonRemoveExclude);
            this.groupBox2.Controls.Add(this.listBoxExcludes);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(807, 322);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Excludes (what not to overwrite when updating) (Packs only)";
            // 
            // textBoxExclude
            // 
            this.textBoxExclude.Location = new System.Drawing.Point(6, 19);
            this.textBoxExclude.Name = "textBoxExclude";
            this.textBoxExclude.Size = new System.Drawing.Size(795, 20);
            this.textBoxExclude.TabIndex = 11;
            this.textBoxExclude.TextChanged += new System.EventHandler(this.textBoxExclude_TextChanged_1);
            // 
            // buttonExcludeAddSingle
            // 
            this.buttonExcludeAddSingle.Location = new System.Drawing.Point(6, 45);
            this.buttonExcludeAddSingle.Name = "buttonExcludeAddSingle";
            this.buttonExcludeAddSingle.Size = new System.Drawing.Size(157, 23);
            this.buttonExcludeAddSingle.TabIndex = 10;
            this.buttonExcludeAddSingle.Text = "Add  (From textbox above)";
            this.buttonExcludeAddSingle.UseVisualStyleBackColor = true;
            this.buttonExcludeAddSingle.Click += new System.EventHandler(this.buttonExcludeAddSingle_Click);
            // 
            // buttonAddExclude
            // 
            this.buttonAddExclude.Location = new System.Drawing.Point(281, 45);
            this.buttonAddExclude.Name = "buttonAddExclude";
            this.buttonAddExclude.Size = new System.Drawing.Size(214, 23);
            this.buttonAddExclude.TabIndex = 9;
            this.buttonAddExclude.Text = "Add Item(s) (Opens dialog box)";
            this.buttonAddExclude.UseVisualStyleBackColor = true;
            this.buttonAddExclude.Click += new System.EventHandler(this.buttonAddExclude_Click);
            // 
            // buttonRemoveExclude
            // 
            this.buttonRemoveExclude.Enabled = false;
            this.buttonRemoveExclude.Location = new System.Drawing.Point(616, 45);
            this.buttonRemoveExclude.Name = "buttonRemoveExclude";
            this.buttonRemoveExclude.Size = new System.Drawing.Size(185, 23);
            this.buttonRemoveExclude.TabIndex = 8;
            this.buttonRemoveExclude.Text = "Remove Item(s) (From list below)";
            this.buttonRemoveExclude.UseVisualStyleBackColor = true;
            this.buttonRemoveExclude.Click += new System.EventHandler(this.buttonRemoveExclude_Click);
            // 
            // listBoxExcludes
            // 
            this.listBoxExcludes.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listBoxExcludes.FormattingEnabled = true;
            this.listBoxExcludes.Location = new System.Drawing.Point(3, 81);
            this.listBoxExcludes.Name = "listBoxExcludes";
            this.listBoxExcludes.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxExcludes.Size = new System.Drawing.Size(801, 238);
            this.listBoxExcludes.Sorted = true;
            this.listBoxExcludes.TabIndex = 7;
            this.listBoxExcludes.TabStop = false;
            this.listBoxExcludes.SelectedIndexChanged += new System.EventHandler(this.listBoxExcludes_SelectedIndexChanged);
            // 
            // tabPageCredits
            // 
            this.tabPageCredits.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageCredits.Controls.Add(this.groupBox4);
            this.tabPageCredits.Location = new System.Drawing.Point(2, 21);
            this.tabPageCredits.Name = "tabPageCredits";
            this.tabPageCredits.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCredits.Size = new System.Drawing.Size(813, 328);
            this.tabPageCredits.TabIndex = 2;
            this.tabPageCredits.Text = "Credits";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.phoenixRichTextBoxCredits);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(3, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(807, 322);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Credits";
            // 
            // phoenixRichTextBoxCredits
            // 
            this.phoenixRichTextBoxCredits.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.phoenixRichTextBoxCredits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.phoenixRichTextBoxCredits.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.phoenixRichTextBoxCredits.Location = new System.Drawing.Point(3, 16);
            this.phoenixRichTextBoxCredits.MinimumSize = new System.Drawing.Size(342, 50);
            this.phoenixRichTextBoxCredits.Name = "phoenixRichTextBoxCredits";
            this.phoenixRichTextBoxCredits.Rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang1033{\\fonttbl{\\f0\\fnil\\fcharset0 Microsoft S" +
                "ans Serif;}}\r\n\\viewkind4\\uc1\\pard\\f0\\fs17\\par\r\n}\r\n";
            this.phoenixRichTextBoxCredits.Size = new System.Drawing.Size(801, 303);
            this.phoenixRichTextBoxCredits.TabIndex = 0;
            this.phoenixRichTextBoxCredits.ToolStripRenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.phoenixRichTextBoxCredits.RichTextChanged += new PhoenixRTB.PhoenixRichTextBox.EventHandler(this.control_SomethingChanged);
            // 
            // FormPackInfoWriter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(843, 498);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBoxVersion);
            this.Controls.Add(this.groupBox3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormPackInfoWriter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "KBG PackInfo Writer";
            this.Load += new System.EventHandler(this.FormPackInfoWriter_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBoxVersion.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRevision)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMajor)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPageNews.ResumeLayout(false);
            this.groupBoxNews.ResumeLayout(false);
            this.tabPageExcludes.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPageCredits.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxNews;
        private System.Windows.Forms.Button buttonSaveAs;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.Label labelCurrentFile;
        private System.Windows.Forms.ListBox listBoxExcludes;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonAddExclude;
        private System.Windows.Forms.Button buttonRemoveExclude;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBoxVersion;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownRevision;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDownMinor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownMajor;
        private System.Windows.Forms.TextBox textBoxExclude;
        private System.Windows.Forms.Button buttonExcludeAddSingle;
        private PhoenixRTB.PhoenixRichTextBox phoenixRichTextBoxNews;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDownPack;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxPreventPackDownload;
        private MyTabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageNews;
        private System.Windows.Forms.TabPage tabPageExcludes;
        private System.Windows.Forms.TabPage tabPageCredits;
        private System.Windows.Forms.GroupBox groupBox4;
        private PhoenixRTB.PhoenixRichTextBox phoenixRichTextBoxCredits;
    }
}

