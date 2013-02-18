namespace KBG_Launcher
{
    partial class PackSettingsItem
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBoxPack = new System.Windows.Forms.GroupBox();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.textBoxPackUrl = new System.Windows.Forms.TextBox();
            this.textBoxVersionUrl = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.labelPackUrl = new System.Windows.Forms.Label();
            this.labelChanges1 = new System.Windows.Forms.Label();
            this.labelChanges2 = new System.Windows.Forms.Label();
            this.groupBoxPack.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxPack
            // 
            this.groupBoxPack.Controls.Add(this.labelChanges2);
            this.groupBoxPack.Controls.Add(this.labelChanges1);
            this.groupBoxPack.Controls.Add(this.buttonDelete);
            this.groupBoxPack.Controls.Add(this.textBoxPackUrl);
            this.groupBoxPack.Controls.Add(this.textBoxVersionUrl);
            this.groupBoxPack.Controls.Add(this.label2);
            this.groupBoxPack.Controls.Add(this.labelPackUrl);
            this.groupBoxPack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxPack.Location = new System.Drawing.Point(1, 1);
            this.groupBoxPack.Name = "groupBoxPack";
            this.groupBoxPack.Size = new System.Drawing.Size(513, 71);
            this.groupBoxPack.TabIndex = 5;
            this.groupBoxPack.TabStop = false;
            this.groupBoxPack.Text = "groupBox1";
            this.groupBoxPack.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GroupBox_MouseMove);
            this.groupBoxPack.MouseLeave += new System.EventHandler(this.PackSettingsItem_MouseLeave);
            // 
            // buttonDelete
            // 
            this.buttonDelete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDelete.ForeColor = System.Drawing.Color.DarkRed;
            this.buttonDelete.Location = new System.Drawing.Point(463, 0);
            this.buttonDelete.Margin = new System.Windows.Forms.Padding(2);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(46, 21);
            this.buttonDelete.TabIndex = 9;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Visible = false;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            this.buttonDelete.MouseEnter += new System.EventHandler(this.PackSettingsItem_MouseEnter);
            this.buttonDelete.MouseLeave += new System.EventHandler(this.textBox_Leave);
            // 
            // textBoxPackUrl
            // 
            this.textBoxPackUrl.Location = new System.Drawing.Point(155, 20);
            this.textBoxPackUrl.Name = "textBoxPackUrl";
            this.textBoxPackUrl.Size = new System.Drawing.Size(354, 20);
            this.textBoxPackUrl.TabIndex = 8;
            this.textBoxPackUrl.TextChanged += new System.EventHandler(this.textBoxPackUrl_TextChanged);
            this.textBoxPackUrl.Leave += new System.EventHandler(this.textBox_Leave);
            this.textBoxPackUrl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GroupBox_MouseMove);
            // 
            // textBoxVersionUrl
            // 
            this.textBoxVersionUrl.Location = new System.Drawing.Point(155, 46);
            this.textBoxVersionUrl.Name = "textBoxVersionUrl";
            this.textBoxVersionUrl.Size = new System.Drawing.Size(354, 20);
            this.textBoxVersionUrl.TabIndex = 7;
            this.textBoxVersionUrl.TextChanged += new System.EventHandler(this.textBoxVersionUrl_TextChanged);
            this.textBoxVersionUrl.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(6, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(143, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "Link to Pack version txt";
            // 
            // labelPackUrl
            // 
            this.labelPackUrl.Location = new System.Drawing.Point(6, 23);
            this.labelPackUrl.Name = "labelPackUrl";
            this.labelPackUrl.Size = new System.Drawing.Size(143, 23);
            this.labelPackUrl.TabIndex = 5;
            this.labelPackUrl.Text = "Link to Pack zip";
            // 
            // labelChanges1
            // 
            this.labelChanges1.AutoSize = true;
            this.labelChanges1.Location = new System.Drawing.Point(142, 23);
            this.labelChanges1.Name = "labelChanges1";
            this.labelChanges1.Size = new System.Drawing.Size(11, 13);
            this.labelChanges1.TabIndex = 10;
            this.labelChanges1.Text = "*";
            this.labelChanges1.Visible = false;
            // 
            // labelChanges2
            // 
            this.labelChanges2.AutoSize = true;
            this.labelChanges2.Location = new System.Drawing.Point(142, 49);
            this.labelChanges2.Name = "labelChanges2";
            this.labelChanges2.Size = new System.Drawing.Size(11, 13);
            this.labelChanges2.TabIndex = 11;
            this.labelChanges2.Text = "*";
            this.labelChanges2.Visible = false;
            // 
            // PackSettingsItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxPack);
            this.Name = "PackSettingsItem";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.Size = new System.Drawing.Size(515, 73);
            this.MouseEnter += new System.EventHandler(this.PackSettingsItem_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.PackSettingsItem_MouseLeave);
            this.groupBoxPack.ResumeLayout(false);
            this.groupBoxPack.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxPack;
        private System.Windows.Forms.TextBox textBoxPackUrl;
        private System.Windows.Forms.TextBox textBoxVersionUrl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelPackUrl;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Label labelChanges2;
        private System.Windows.Forms.Label labelChanges1;

    }
}
