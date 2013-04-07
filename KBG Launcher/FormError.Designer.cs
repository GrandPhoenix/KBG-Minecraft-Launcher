namespace KBG_Launcher
{
    partial class FormError
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormError));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.labelPastebinLinkLabel = new System.Windows.Forms.Label();
            this.buttonOploadPastebin = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxPastebinLink = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(3, 3);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(672, 488);
            this.textBox1.TabIndex = 0;
            // 
            // labelPastebinLinkLabel
            // 
            this.labelPastebinLinkLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPastebinLinkLabel.Location = new System.Drawing.Point(3, 0);
            this.labelPastebinLinkLabel.Name = "labelPastebinLinkLabel";
            this.labelPastebinLinkLabel.Size = new System.Drawing.Size(74, 26);
            this.labelPastebinLinkLabel.TabIndex = 1;
            this.labelPastebinLinkLabel.Text = "Pastebin Link:";
            this.labelPastebinLinkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonOploadPastebin
            // 
            this.buttonOploadPastebin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOploadPastebin.Location = new System.Drawing.Point(3, 497);
            this.buttonOploadPastebin.Name = "buttonOploadPastebin";
            this.buttonOploadPastebin.Size = new System.Drawing.Size(672, 24);
            this.buttonOploadPastebin.TabIndex = 2;
            this.buttonOploadPastebin.Text = "Upload to Pastebin.com";
            this.buttonOploadPastebin.UseVisualStyleBackColor = true;
            this.buttonOploadPastebin.Click += new System.EventHandler(this.buttonOploadPastebin_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.textBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonOploadPastebin, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(678, 556);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.labelPastebinLinkLabel, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.textBoxPastebinLink, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Enabled = false;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 527);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(672, 26);
            this.tableLayoutPanel2.TabIndex = 6;
            // 
            // textBoxPastebinLink
            // 
            this.textBoxPastebinLink.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxPastebinLink.Location = new System.Drawing.Point(83, 3);
            this.textBoxPastebinLink.Name = "textBoxPastebinLink";
            this.textBoxPastebinLink.ReadOnly = true;
            this.textBoxPastebinLink.Size = new System.Drawing.Size(586, 20);
            this.textBoxPastebinLink.TabIndex = 2;
            this.textBoxPastebinLink.Text = "Not uploaded yet";
            this.textBoxPastebinLink.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FormError
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 562);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormError";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "An error occurred";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormError_FormClosing);
            this.Shown += new System.EventHandler(this.FormError_Shown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label labelPastebinLinkLabel;
        private System.Windows.Forms.Button buttonOploadPastebin;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox textBoxPastebinLink;
    }
}