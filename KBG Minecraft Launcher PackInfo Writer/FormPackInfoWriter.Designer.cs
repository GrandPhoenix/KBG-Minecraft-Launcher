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
            this.groupBoxNews = new System.Windows.Forms.GroupBox();
            this.phoenixRichTextBoxNews = new PhoenixRTB.PhoenixRichTextBox();
            this.buttonSaveAs = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.labelCurrentFile = new System.Windows.Forms.Label();
            this.listBoxExcludes = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxExclude = new System.Windows.Forms.TextBox();
            this.buttonExcludeAddSingle = new System.Windows.Forms.Button();
            this.buttonAddExclude = new System.Windows.Forms.Button();
            this.buttonRemoveExclude = new System.Windows.Forms.Button();
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
            this.groupBoxNews.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBoxVersion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRevision)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMajor)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxNews
            // 
            this.groupBoxNews.Controls.Add(this.phoenixRichTextBoxNews);
            this.groupBoxNews.Location = new System.Drawing.Point(12, 134);
            this.groupBoxNews.Name = "groupBoxNews";
            this.groupBoxNews.Size = new System.Drawing.Size(506, 269);
            this.groupBoxNews.TabIndex = 0;
            this.groupBoxNews.TabStop = false;
            this.groupBoxNews.Text = "News";
            // 
            // phoenixRichTextBoxNews
            // 
            this.phoenixRichTextBoxNews.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.phoenixRichTextBoxNews.Dock = System.Windows.Forms.DockStyle.Fill;
            this.phoenixRichTextBoxNews.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.phoenixRichTextBoxNews.Location = new System.Drawing.Point(3, 16);
            this.phoenixRichTextBoxNews.MinimumSize = new System.Drawing.Size(342, 50);
            this.phoenixRichTextBoxNews.Name = "phoenixRichTextBoxNews";
            this.phoenixRichTextBoxNews.Rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang1033{\\fonttbl{\\f0\\fnil\\fcharset0 Microsoft S" +
                "ans Serif;}}\r\n\\viewkind4\\uc1\\pard\\f0\\fs17\\par\r\n}\r\n";
            this.phoenixRichTextBoxNews.Size = new System.Drawing.Size(500, 250);
            this.phoenixRichTextBoxNews.TabIndex = 0;
            this.phoenixRichTextBoxNews.ToolStripRenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.phoenixRichTextBoxNews.RichTextChanged += new PhoenixRTB.PhoenixRichTextBox.EventHandler(this.control_SomethingChanged);
            // 
            // buttonSaveAs
            // 
            this.buttonSaveAs.Location = new System.Drawing.Point(297, 19);
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
            this.buttonSave.Location = new System.Drawing.Point(378, 19);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(58, 23);
            this.buttonSave.TabIndex = 4;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonOpen
            // 
            this.buttonOpen.Location = new System.Drawing.Point(442, 19);
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
            this.labelCurrentFile.Size = new System.Drawing.Size(279, 16);
            this.labelCurrentFile.TabIndex = 6;
            this.labelCurrentFile.Text = "None";
            this.labelCurrentFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // listBoxExcludes
            // 
            this.listBoxExcludes.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listBoxExcludes.FormattingEnabled = true;
            this.listBoxExcludes.Location = new System.Drawing.Point(3, 83);
            this.listBoxExcludes.Name = "listBoxExcludes";
            this.listBoxExcludes.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxExcludes.Size = new System.Drawing.Size(301, 251);
            this.listBoxExcludes.Sorted = true;
            this.listBoxExcludes.TabIndex = 7;
            this.listBoxExcludes.TabStop = false;
            this.listBoxExcludes.SelectedIndexChanged += new System.EventHandler(this.listBoxExcludes_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBoxExclude);
            this.groupBox2.Controls.Add(this.buttonExcludeAddSingle);
            this.groupBox2.Controls.Add(this.buttonAddExclude);
            this.groupBox2.Controls.Add(this.buttonRemoveExclude);
            this.groupBox2.Controls.Add(this.listBoxExcludes);
            this.groupBox2.Location = new System.Drawing.Point(524, 66);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(307, 337);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Excludes (what not to overwrite when updating) (Packs only)";
            // 
            // textBoxExclude
            // 
            this.textBoxExclude.Location = new System.Drawing.Point(6, 53);
            this.textBoxExclude.Name = "textBoxExclude";
            this.textBoxExclude.Size = new System.Drawing.Size(214, 20);
            this.textBoxExclude.TabIndex = 11;
            this.textBoxExclude.TextChanged += new System.EventHandler(this.textBoxExclude_TextChanged_1);
            // 
            // buttonExcludeAddSingle
            // 
            this.buttonExcludeAddSingle.Location = new System.Drawing.Point(226, 51);
            this.buttonExcludeAddSingle.Name = "buttonExcludeAddSingle";
            this.buttonExcludeAddSingle.Size = new System.Drawing.Size(75, 23);
            this.buttonExcludeAddSingle.TabIndex = 10;
            this.buttonExcludeAddSingle.Text = "Add";
            this.buttonExcludeAddSingle.UseVisualStyleBackColor = true;
            this.buttonExcludeAddSingle.Click += new System.EventHandler(this.buttonExcludeAddSingle_Click);
            // 
            // buttonAddExclude
            // 
            this.buttonAddExclude.Location = new System.Drawing.Point(6, 22);
            this.buttonAddExclude.Name = "buttonAddExclude";
            this.buttonAddExclude.Size = new System.Drawing.Size(144, 23);
            this.buttonAddExclude.TabIndex = 9;
            this.buttonAddExclude.Text = "Add Item(s)..";
            this.buttonAddExclude.UseVisualStyleBackColor = true;
            this.buttonAddExclude.Click += new System.EventHandler(this.buttonAddExclude_Click);
            // 
            // buttonRemoveExclude
            // 
            this.buttonRemoveExclude.Enabled = false;
            this.buttonRemoveExclude.Location = new System.Drawing.Point(158, 22);
            this.buttonRemoveExclude.Name = "buttonRemoveExclude";
            this.buttonRemoveExclude.Size = new System.Drawing.Size(143, 23);
            this.buttonRemoveExclude.TabIndex = 8;
            this.buttonRemoveExclude.Text = "Remove Item(s)";
            this.buttonRemoveExclude.UseVisualStyleBackColor = true;
            this.buttonRemoveExclude.Click += new System.EventHandler(this.buttonRemoveExclude_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.buttonOpen);
            this.groupBox3.Controls.Add(this.buttonSave);
            this.groupBox3.Controls.Add(this.buttonSaveAs);
            this.groupBox3.Controls.Add(this.labelCurrentFile);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(506, 48);
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
            this.groupBox1.Location = new System.Drawing.Point(524, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(307, 48);
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
            // FormPackInfoWriter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(842, 414);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBoxVersion);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBoxNews);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormPackInfoWriter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "KBG PackInfo Writer";
            this.Load += new System.EventHandler(this.FormPackInfoWriter_Load);
            this.groupBoxNews.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBoxVersion.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRevision)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMajor)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
    }
}

