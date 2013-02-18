namespace KBG_Launcher
{
    partial class PackSettings
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBoxPacks = new System.Windows.Forms.GroupBox();
            this.buttonAddNewPack = new System.Windows.Forms.Button();
            this.textBoxAddNewPack = new System.Windows.Forms.TextBox();
            this.groupBoxPacks.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.flowLayoutPanel1.MaximumSize = new System.Drawing.Size(600, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(538, 174);
            this.flowLayoutPanel1.TabIndex = 0;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // groupBoxPacks
            // 
            this.groupBoxPacks.Controls.Add(this.flowLayoutPanel1);
            this.groupBoxPacks.Location = new System.Drawing.Point(3, 32);
            this.groupBoxPacks.Name = "groupBoxPacks";
            this.groupBoxPacks.Size = new System.Drawing.Size(544, 193);
            this.groupBoxPacks.TabIndex = 1;
            this.groupBoxPacks.TabStop = false;
            this.groupBoxPacks.Text = "Minecraft Mod Packs";
            // 
            // buttonAddNewPack
            // 
            this.buttonAddNewPack.Location = new System.Drawing.Point(6, 3);
            this.buttonAddNewPack.Name = "buttonAddNewPack";
            this.buttonAddNewPack.Size = new System.Drawing.Size(117, 23);
            this.buttonAddNewPack.TabIndex = 2;
            this.buttonAddNewPack.Text = "Add new pack";
            this.buttonAddNewPack.UseVisualStyleBackColor = true;
            this.buttonAddNewPack.Click += new System.EventHandler(this.buttonAddNewPack_Click);
            // 
            // textBoxAddNewPack
            // 
            this.textBoxAddNewPack.Location = new System.Drawing.Point(129, 5);
            this.textBoxAddNewPack.Name = "textBoxAddNewPack";
            this.textBoxAddNewPack.Size = new System.Drawing.Size(415, 20);
            this.textBoxAddNewPack.TabIndex = 3;
            // 
            // PackSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxAddNewPack);
            this.Controls.Add(this.buttonAddNewPack);
            this.Controls.Add(this.groupBoxPacks);
            this.Name = "PackSettings";
            this.Size = new System.Drawing.Size(550, 228);
            this.MouseEnter += new System.EventHandler(this.PackSettings_MouseEnter);
            this.groupBoxPacks.ResumeLayout(false);
            this.groupBoxPacks.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBoxPacks;
        private System.Windows.Forms.Button buttonAddNewPack;
        private System.Windows.Forms.TextBox textBoxAddNewPack;

    }
}
