namespace KBG_Minecraft_Launcher
{
    partial class FormMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.buttonLogin = new System.Windows.Forms.Button();
            this.groupBoxLogin = new System.Windows.Forms.GroupBox();
            this.checkBoxRememberLoginInfo = new System.Windows.Forms.CheckBox();
            this.comboBoxPackSelect = new System.Windows.Forms.ComboBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.labelUsername = new System.Windows.Forms.Label();
            this.textBoxUsername = new System.Windows.Forms.TextBox();
            this.buttonOptions = new System.Windows.Forms.Button();
            this.groupBoxTwitter = new System.Windows.Forms.GroupBox();
            this.progressBarTwitter = new System.Windows.Forms.ProgressBar();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.labelMinecraftLoginServers = new System.Windows.Forms.Label();
            this.labelMinecraftdotnet = new System.Windows.Forms.Label();
            this.labelKBGEvent = new System.Windows.Forms.Label();
            this.labelTFCR = new System.Windows.Forms.Label();
            this.labelIR = new System.Windows.Forms.Label();
            this.labelER = new System.Windows.Forms.Label();
            this.groupBoxServerStatus = new System.Windows.Forms.GroupBox();
            this.buttonRefreshTwitterFeeds = new System.Windows.Forms.Button();
            this.progressBarMinecraftLoginServers = new System.Windows.Forms.ProgressBar();
            this.progressBarMinecraftdotnet = new System.Windows.Forms.ProgressBar();
            this.progressBarTFCR = new System.Windows.Forms.ProgressBar();
            this.progressBarER = new System.Windows.Forms.ProgressBar();
            this.progressBarKBGEvent = new System.Windows.Forms.ProgressBar();
            this.progressBarIR = new System.Windows.Forms.ProgressBar();
            this.labelMinecraftLoginServersResult = new System.Windows.Forms.Label();
            this.labelMinecraftdotnetResult = new System.Windows.Forms.Label();
            this.labelTFCRResult = new System.Windows.Forms.Label();
            this.labelERResult = new System.Windows.Forms.Label();
            this.labelKBGEventResult = new System.Windows.Forms.Label();
            this.labelIRResult = new System.Windows.Forms.Label();
            this.buttonRefreshServerStatus = new System.Windows.Forms.Button();
            this.buttonDebug = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panelDownload = new System.Windows.Forms.Panel();
            this.labelDownloadProgress = new System.Windows.Forms.Label();
            this.labelDownloadSpeed = new System.Windows.Forms.Label();
            this.buttonDownloadCancel = new System.Windows.Forms.Button();
            this.labelDownload = new System.Windows.Forms.Label();
            this.progressBarDownload = new System.Windows.Forms.ProgressBar();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.textBoxDebug = new System.Windows.Forms.TextBox();
            this.linkLabelKB_Gaming = new System.Windows.Forms.LinkLabel();
            this.linkLabelCredits = new System.Windows.Forms.LinkLabel();
            this.groupBoxLogin.SuspendLayout();
            this.groupBoxTwitter.SuspendLayout();
            this.groupBoxServerStatus.SuspendLayout();
            this.panelDownload.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonLogin
            // 
            this.buttonLogin.Location = new System.Drawing.Point(196, 73);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(86, 23);
            this.buttonLogin.TabIndex = 0;
            this.buttonLogin.Text = "Login";
            this.buttonLogin.UseVisualStyleBackColor = true;
            this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
            // 
            // groupBoxLogin
            // 
            this.groupBoxLogin.Controls.Add(this.linkLabelCredits);
            this.groupBoxLogin.Controls.Add(this.checkBoxRememberLoginInfo);
            this.groupBoxLogin.Controls.Add(this.comboBoxPackSelect);
            this.groupBoxLogin.Controls.Add(this.labelPassword);
            this.groupBoxLogin.Controls.Add(this.textBoxPassword);
            this.groupBoxLogin.Controls.Add(this.labelUsername);
            this.groupBoxLogin.Controls.Add(this.textBoxUsername);
            this.groupBoxLogin.Controls.Add(this.buttonOptions);
            this.groupBoxLogin.Controls.Add(this.buttonLogin);
            this.groupBoxLogin.Location = new System.Drawing.Point(328, 198);
            this.groupBoxLogin.Name = "groupBoxLogin";
            this.groupBoxLogin.Size = new System.Drawing.Size(289, 121);
            this.groupBoxLogin.TabIndex = 1;
            this.groupBoxLogin.TabStop = false;
            this.groupBoxLogin.Text = "Login";
            // 
            // checkBoxRememberLoginInfo
            // 
            this.checkBoxRememberLoginInfo.AutoSize = true;
            this.checkBoxRememberLoginInfo.Location = new System.Drawing.Point(9, 98);
            this.checkBoxRememberLoginInfo.Name = "checkBoxRememberLoginInfo";
            this.checkBoxRememberLoginInfo.Size = new System.Drawing.Size(106, 17);
            this.checkBoxRememberLoginInfo.TabIndex = 6;
            this.checkBoxRememberLoginInfo.Text = "Remember Login";
            this.checkBoxRememberLoginInfo.UseVisualStyleBackColor = true;
            this.checkBoxRememberLoginInfo.CheckedChanged += new System.EventHandler(this.checkBoxRememberLoginInfo_CheckedChanged);
            // 
            // comboBoxPackSelect
            // 
            this.comboBoxPackSelect.FormattingEnabled = true;
            this.comboBoxPackSelect.Location = new System.Drawing.Point(9, 19);
            this.comboBoxPackSelect.Name = "comboBoxPackSelect";
            this.comboBoxPackSelect.Size = new System.Drawing.Size(273, 21);
            this.comboBoxPackSelect.TabIndex = 2;
            this.comboBoxPackSelect.SelectedIndexChanged += new System.EventHandler(this.comboBoxPackSelect_SelectedIndexChanged);
            // 
            // labelPassword
            // 
            this.labelPassword.Location = new System.Drawing.Point(6, 79);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(58, 15);
            this.labelPassword.TabIndex = 6;
            this.labelPassword.Text = "Password";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(70, 76);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(120, 20);
            this.textBoxPassword.TabIndex = 5;
            this.textBoxPassword.UseSystemPasswordChar = true;
            // 
            // labelUsername
            // 
            this.labelUsername.Location = new System.Drawing.Point(6, 49);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(58, 15);
            this.labelUsername.TabIndex = 4;
            this.labelUsername.Text = "Username";
            // 
            // textBoxUsername
            // 
            this.textBoxUsername.Location = new System.Drawing.Point(70, 46);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.Size = new System.Drawing.Size(120, 20);
            this.textBoxUsername.TabIndex = 3;
            // 
            // buttonOptions
            // 
            this.buttonOptions.Location = new System.Drawing.Point(196, 44);
            this.buttonOptions.Name = "buttonOptions";
            this.buttonOptions.Size = new System.Drawing.Size(86, 23);
            this.buttonOptions.TabIndex = 4;
            this.buttonOptions.Text = "Options";
            this.buttonOptions.UseVisualStyleBackColor = true;
            this.buttonOptions.Click += new System.EventHandler(this.buttonOptions_Click);
            // 
            // groupBoxTwitter
            // 
            this.groupBoxTwitter.Controls.Add(this.linkLabelKB_Gaming);
            this.groupBoxTwitter.Controls.Add(this.progressBarTwitter);
            this.groupBoxTwitter.Controls.Add(this.richTextBox1);
            this.groupBoxTwitter.Location = new System.Drawing.Point(12, 12);
            this.groupBoxTwitter.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.groupBoxTwitter.Name = "groupBoxTwitter";
            this.groupBoxTwitter.Padding = new System.Windows.Forms.Padding(4, 6, 4, 4);
            this.groupBoxTwitter.Size = new System.Drawing.Size(306, 307);
            this.groupBoxTwitter.TabIndex = 2;
            this.groupBoxTwitter.TabStop = false;
            this.groupBoxTwitter.Text = "Twitter - KB_Gaming";
            // 
            // progressBarTwitter
            // 
            this.progressBarTwitter.Location = new System.Drawing.Point(6, 149);
            this.progressBarTwitter.MarqueeAnimationSpeed = 50;
            this.progressBarTwitter.Name = "progressBarTwitter";
            this.progressBarTwitter.Size = new System.Drawing.Size(294, 23);
            this.progressBarTwitter.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBarTwitter.TabIndex = 1;
            this.progressBarTwitter.Visible = false;
            // 
            // richTextBox1
            // 
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(4, 19);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBox1.Size = new System.Drawing.Size(298, 284);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.TabStop = false;
            this.richTextBox1.Text = "";
            this.richTextBox1.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBox1_LinkClicked);
            // 
            // labelMinecraftLoginServers
            // 
            this.labelMinecraftLoginServers.AutoSize = true;
            this.labelMinecraftLoginServers.Location = new System.Drawing.Point(21, 128);
            this.labelMinecraftLoginServers.Name = "labelMinecraftLoginServers";
            this.labelMinecraftLoginServers.Size = new System.Drawing.Size(122, 13);
            this.labelMinecraftLoginServers.TabIndex = 17;
            this.labelMinecraftLoginServers.Text = "Minecraft Login Servers:";
            // 
            // labelMinecraftdotnet
            // 
            this.labelMinecraftdotnet.AutoSize = true;
            this.labelMinecraftdotnet.Location = new System.Drawing.Point(71, 105);
            this.labelMinecraftdotnet.Name = "labelMinecraftdotnet";
            this.labelMinecraftdotnet.Size = new System.Drawing.Size(72, 13);
            this.labelMinecraftdotnet.TabIndex = 16;
            this.labelMinecraftdotnet.Text = "Minecraft.net:";
            // 
            // labelKBGEvent
            // 
            this.labelKBGEvent.AutoSize = true;
            this.labelKBGEvent.Location = new System.Drawing.Point(80, 39);
            this.labelKBGEvent.Name = "labelKBGEvent";
            this.labelKBGEvent.Size = new System.Drawing.Size(63, 13);
            this.labelKBGEvent.TabIndex = 13;
            this.labelKBGEvent.Text = "KBG Event:";
            // 
            // labelTFCR
            // 
            this.labelTFCR.AutoSize = true;
            this.labelTFCR.Location = new System.Drawing.Point(49, 83);
            this.labelTFCR.Name = "labelTFCR";
            this.labelTFCR.Size = new System.Drawing.Size(93, 13);
            this.labelTFCR.TabIndex = 15;
            this.labelTFCR.Text = "TerraFerma Rage:";
            // 
            // labelIR
            // 
            this.labelIR.AutoSize = true;
            this.labelIR.Location = new System.Drawing.Point(62, 16);
            this.labelIR.Name = "labelIR";
            this.labelIR.Size = new System.Drawing.Size(81, 13);
            this.labelIR.TabIndex = 12;
            this.labelIR.Text = "Industrial Rage:";
            // 
            // labelER
            // 
            this.labelER.AutoSize = true;
            this.labelER.Location = new System.Drawing.Point(67, 61);
            this.labelER.Name = "labelER";
            this.labelER.Size = new System.Drawing.Size(76, 13);
            this.labelER.TabIndex = 14;
            this.labelER.Text = "Endless Rage:";
            // 
            // groupBoxServerStatus
            // 
            this.groupBoxServerStatus.Controls.Add(this.buttonRefreshTwitterFeeds);
            this.groupBoxServerStatus.Controls.Add(this.progressBarMinecraftLoginServers);
            this.groupBoxServerStatus.Controls.Add(this.progressBarMinecraftdotnet);
            this.groupBoxServerStatus.Controls.Add(this.progressBarTFCR);
            this.groupBoxServerStatus.Controls.Add(this.progressBarER);
            this.groupBoxServerStatus.Controls.Add(this.progressBarKBGEvent);
            this.groupBoxServerStatus.Controls.Add(this.progressBarIR);
            this.groupBoxServerStatus.Controls.Add(this.labelMinecraftLoginServersResult);
            this.groupBoxServerStatus.Controls.Add(this.labelMinecraftdotnetResult);
            this.groupBoxServerStatus.Controls.Add(this.labelTFCRResult);
            this.groupBoxServerStatus.Controls.Add(this.labelERResult);
            this.groupBoxServerStatus.Controls.Add(this.labelKBGEventResult);
            this.groupBoxServerStatus.Controls.Add(this.labelIRResult);
            this.groupBoxServerStatus.Controls.Add(this.labelMinecraftLoginServers);
            this.groupBoxServerStatus.Controls.Add(this.buttonRefreshServerStatus);
            this.groupBoxServerStatus.Controls.Add(this.labelMinecraftdotnet);
            this.groupBoxServerStatus.Controls.Add(this.labelIR);
            this.groupBoxServerStatus.Controls.Add(this.labelKBGEvent);
            this.groupBoxServerStatus.Controls.Add(this.labelER);
            this.groupBoxServerStatus.Controls.Add(this.labelTFCR);
            this.groupBoxServerStatus.Location = new System.Drawing.Point(328, 12);
            this.groupBoxServerStatus.Name = "groupBoxServerStatus";
            this.groupBoxServerStatus.Size = new System.Drawing.Size(289, 180);
            this.groupBoxServerStatus.TabIndex = 3;
            this.groupBoxServerStatus.TabStop = false;
            this.groupBoxServerStatus.Text = "Server status";
            // 
            // buttonRefreshTwitterFeeds
            // 
            this.buttonRefreshTwitterFeeds.Location = new System.Drawing.Point(6, 149);
            this.buttonRefreshTwitterFeeds.Name = "buttonRefreshTwitterFeeds";
            this.buttonRefreshTwitterFeeds.Size = new System.Drawing.Size(132, 23);
            this.buttonRefreshTwitterFeeds.TabIndex = 31;
            this.buttonRefreshTwitterFeeds.Text = "Refresh Twitter feeds";
            this.buttonRefreshTwitterFeeds.UseVisualStyleBackColor = true;
            this.buttonRefreshTwitterFeeds.Click += new System.EventHandler(this.buttonRefreshTwitterFeeds_Click);
            // 
            // progressBarMinecraftLoginServers
            // 
            this.progressBarMinecraftLoginServers.Location = new System.Drawing.Point(152, 128);
            this.progressBarMinecraftLoginServers.Name = "progressBarMinecraftLoginServers";
            this.progressBarMinecraftLoginServers.Size = new System.Drawing.Size(63, 13);
            this.progressBarMinecraftLoginServers.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBarMinecraftLoginServers.TabIndex = 30;
            this.progressBarMinecraftLoginServers.Visible = false;
            // 
            // progressBarMinecraftdotnet
            // 
            this.progressBarMinecraftdotnet.Location = new System.Drawing.Point(152, 105);
            this.progressBarMinecraftdotnet.Name = "progressBarMinecraftdotnet";
            this.progressBarMinecraftdotnet.Size = new System.Drawing.Size(63, 13);
            this.progressBarMinecraftdotnet.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBarMinecraftdotnet.TabIndex = 29;
            this.progressBarMinecraftdotnet.Visible = false;
            // 
            // progressBarTFCR
            // 
            this.progressBarTFCR.Location = new System.Drawing.Point(152, 83);
            this.progressBarTFCR.Name = "progressBarTFCR";
            this.progressBarTFCR.Size = new System.Drawing.Size(63, 13);
            this.progressBarTFCR.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBarTFCR.TabIndex = 28;
            this.progressBarTFCR.Visible = false;
            // 
            // progressBarER
            // 
            this.progressBarER.Location = new System.Drawing.Point(152, 61);
            this.progressBarER.Name = "progressBarER";
            this.progressBarER.Size = new System.Drawing.Size(63, 13);
            this.progressBarER.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBarER.TabIndex = 27;
            this.progressBarER.Visible = false;
            // 
            // progressBarKBGEvent
            // 
            this.progressBarKBGEvent.Location = new System.Drawing.Point(152, 39);
            this.progressBarKBGEvent.Name = "progressBarKBGEvent";
            this.progressBarKBGEvent.Size = new System.Drawing.Size(63, 13);
            this.progressBarKBGEvent.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBarKBGEvent.TabIndex = 26;
            this.progressBarKBGEvent.Visible = false;
            // 
            // progressBarIR
            // 
            this.progressBarIR.Location = new System.Drawing.Point(152, 16);
            this.progressBarIR.MarqueeAnimationSpeed = 50;
            this.progressBarIR.Name = "progressBarIR";
            this.progressBarIR.Size = new System.Drawing.Size(63, 13);
            this.progressBarIR.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBarIR.TabIndex = 24;
            this.progressBarIR.Visible = false;
            // 
            // labelMinecraftLoginServersResult
            // 
            this.labelMinecraftLoginServersResult.AutoSize = true;
            this.labelMinecraftLoginServersResult.Location = new System.Drawing.Point(149, 128);
            this.labelMinecraftLoginServersResult.Name = "labelMinecraftLoginServersResult";
            this.labelMinecraftLoginServersResult.Size = new System.Drawing.Size(35, 13);
            this.labelMinecraftLoginServersResult.TabIndex = 23;
            this.labelMinecraftLoginServersResult.Text = "label6";
            this.labelMinecraftLoginServersResult.Visible = false;
            // 
            // labelMinecraftdotnetResult
            // 
            this.labelMinecraftdotnetResult.AutoSize = true;
            this.labelMinecraftdotnetResult.Location = new System.Drawing.Point(149, 105);
            this.labelMinecraftdotnetResult.Name = "labelMinecraftdotnetResult";
            this.labelMinecraftdotnetResult.Size = new System.Drawing.Size(35, 13);
            this.labelMinecraftdotnetResult.TabIndex = 22;
            this.labelMinecraftdotnetResult.Text = "label5";
            this.labelMinecraftdotnetResult.Visible = false;
            // 
            // labelTFCRResult
            // 
            this.labelTFCRResult.AutoSize = true;
            this.labelTFCRResult.Location = new System.Drawing.Point(149, 83);
            this.labelTFCRResult.Name = "labelTFCRResult";
            this.labelTFCRResult.Size = new System.Drawing.Size(35, 13);
            this.labelTFCRResult.TabIndex = 21;
            this.labelTFCRResult.Text = "label4";
            this.labelTFCRResult.Visible = false;
            // 
            // labelERResult
            // 
            this.labelERResult.AutoSize = true;
            this.labelERResult.Location = new System.Drawing.Point(149, 61);
            this.labelERResult.Name = "labelERResult";
            this.labelERResult.Size = new System.Drawing.Size(35, 13);
            this.labelERResult.TabIndex = 20;
            this.labelERResult.Text = "label3";
            this.labelERResult.Visible = false;
            // 
            // labelKBGEventResult
            // 
            this.labelKBGEventResult.AutoSize = true;
            this.labelKBGEventResult.Location = new System.Drawing.Point(149, 39);
            this.labelKBGEventResult.Name = "labelKBGEventResult";
            this.labelKBGEventResult.Size = new System.Drawing.Size(35, 13);
            this.labelKBGEventResult.TabIndex = 19;
            this.labelKBGEventResult.Text = "label2";
            this.labelKBGEventResult.Visible = false;
            // 
            // labelIRResult
            // 
            this.labelIRResult.AutoSize = true;
            this.labelIRResult.Location = new System.Drawing.Point(149, 16);
            this.labelIRResult.Name = "labelIRResult";
            this.labelIRResult.Size = new System.Drawing.Size(40, 13);
            this.labelIRResult.TabIndex = 18;
            this.labelIRResult.Text = "labelIR";
            this.labelIRResult.Visible = false;
            // 
            // buttonRefreshServerStatus
            // 
            this.buttonRefreshServerStatus.Location = new System.Drawing.Point(150, 149);
            this.buttonRefreshServerStatus.Name = "buttonRefreshServerStatus";
            this.buttonRefreshServerStatus.Size = new System.Drawing.Size(132, 23);
            this.buttonRefreshServerStatus.TabIndex = 1;
            this.buttonRefreshServerStatus.Text = "Refresh server status";
            this.buttonRefreshServerStatus.UseVisualStyleBackColor = true;
            this.buttonRefreshServerStatus.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // buttonDebug
            // 
            this.buttonDebug.Location = new System.Drawing.Point(117, 304);
            this.buttonDebug.Name = "buttonDebug";
            this.buttonDebug.Size = new System.Drawing.Size(75, 23);
            this.buttonDebug.TabIndex = 25;
            this.buttonDebug.Text = "debug";
            this.buttonDebug.UseVisualStyleBackColor = true;
            this.buttonDebug.Visible = false;
            this.buttonDebug.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // timer1
            // 
            this.timer1.Interval = 5000;
            // 
            // panelDownload
            // 
            this.panelDownload.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelDownload.Controls.Add(this.labelDownloadProgress);
            this.panelDownload.Controls.Add(this.labelDownloadSpeed);
            this.panelDownload.Controls.Add(this.buttonDownloadCancel);
            this.panelDownload.Controls.Add(this.labelDownload);
            this.panelDownload.Controls.Add(this.progressBarDownload);
            this.panelDownload.Location = new System.Drawing.Point(222, 121);
            this.panelDownload.Name = "panelDownload";
            this.panelDownload.Size = new System.Drawing.Size(200, 100);
            this.panelDownload.TabIndex = 26;
            this.panelDownload.Visible = false;
            // 
            // labelDownloadProgress
            // 
            this.labelDownloadProgress.Location = new System.Drawing.Point(101, 54);
            this.labelDownloadProgress.Name = "labelDownloadProgress";
            this.labelDownloadProgress.Size = new System.Drawing.Size(92, 13);
            this.labelDownloadProgress.TabIndex = 4;
            this.labelDownloadProgress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelDownloadSpeed
            // 
            this.labelDownloadSpeed.Location = new System.Drawing.Point(8, 54);
            this.labelDownloadSpeed.Name = "labelDownloadSpeed";
            this.labelDownloadSpeed.Size = new System.Drawing.Size(87, 13);
            this.labelDownloadSpeed.TabIndex = 3;
            this.labelDownloadSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonDownloadCancel
            // 
            this.buttonDownloadCancel.ForeColor = System.Drawing.Color.Red;
            this.buttonDownloadCancel.Location = new System.Drawing.Point(175, 3);
            this.buttonDownloadCancel.Name = "buttonDownloadCancel";
            this.buttonDownloadCancel.Size = new System.Drawing.Size(19, 21);
            this.buttonDownloadCancel.TabIndex = 2;
            this.buttonDownloadCancel.Text = "X";
            this.toolTip1.SetToolTip(this.buttonDownloadCancel, "Cancel action");
            this.buttonDownloadCancel.UseVisualStyleBackColor = true;
            this.buttonDownloadCancel.Visible = false;
            this.buttonDownloadCancel.Click += new System.EventHandler(this.buttonDownloadCancel_Click);
            // 
            // labelDownload
            // 
            this.labelDownload.Location = new System.Drawing.Point(2, 8);
            this.labelDownload.Name = "labelDownload";
            this.labelDownload.Size = new System.Drawing.Size(193, 42);
            this.labelDownload.TabIndex = 1;
            this.labelDownload.Text = "label1";
            this.labelDownload.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progressBarDownload
            // 
            this.progressBarDownload.Location = new System.Drawing.Point(5, 69);
            this.progressBarDownload.Name = "progressBarDownload";
            this.progressBarDownload.Size = new System.Drawing.Size(188, 23);
            this.progressBarDownload.TabIndex = 0;
            // 
            // textBoxDebug
            // 
            this.textBoxDebug.Location = new System.Drawing.Point(11, 306);
            this.textBoxDebug.Name = "textBoxDebug";
            this.textBoxDebug.Size = new System.Drawing.Size(100, 20);
            this.textBoxDebug.TabIndex = 27;
            this.textBoxDebug.Visible = false;
            // 
            // linkLabelKB_Gaming
            // 
            this.linkLabelKB_Gaming.AutoSize = true;
            this.linkLabelKB_Gaming.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabelKB_Gaming.Location = new System.Drawing.Point(9, 1);
            this.linkLabelKB_Gaming.Name = "linkLabelKB_Gaming";
            this.linkLabelKB_Gaming.Size = new System.Drawing.Size(131, 13);
            this.linkLabelKB_Gaming.TabIndex = 28;
            this.linkLabelKB_Gaming.TabStop = true;
            this.linkLabelKB_Gaming.Text = "Visit KB_Gaming on twitter";
            this.linkLabelKB_Gaming.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelKB_Gaming_LinkClicked);
            // 
            // linkLabelCredits
            // 
            this.linkLabelCredits.AutoSize = true;
            this.linkLabelCredits.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabelCredits.Location = new System.Drawing.Point(206, 99);
            this.linkLabelCredits.Name = "linkLabelCredits";
            this.linkLabelCredits.Size = new System.Drawing.Size(67, 13);
            this.linkLabelCredits.TabIndex = 29;
            this.linkLabelCredits.TabStop = true;
            this.linkLabelCredits.Text = "Pack Credits";
            this.linkLabelCredits.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelCredits_LinkClicked);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 331);
            this.Controls.Add(this.textBoxDebug);
            this.Controls.Add(this.panelDownload);
            this.Controls.Add(this.buttonDebug);
            this.Controls.Add(this.groupBoxTwitter);
            this.Controls.Add(this.groupBoxLogin);
            this.Controls.Add(this.groupBoxServerStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "KBG Minecraft Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.groupBoxLogin.ResumeLayout(false);
            this.groupBoxLogin.PerformLayout();
            this.groupBoxTwitter.ResumeLayout(false);
            this.groupBoxTwitter.PerformLayout();
            this.groupBoxServerStatus.ResumeLayout(false);
            this.groupBoxServerStatus.PerformLayout();
            this.panelDownload.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonLogin;
        private System.Windows.Forms.GroupBox groupBoxLogin;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.TextBox textBoxUsername;
        private System.Windows.Forms.Button buttonOptions;
        private System.Windows.Forms.GroupBox groupBoxTwitter;
        private System.Windows.Forms.GroupBox groupBoxServerStatus;
        private System.Windows.Forms.ComboBox comboBoxPackSelect;
        internal System.Windows.Forms.Label labelMinecraftLoginServers;
        internal System.Windows.Forms.Label labelMinecraftdotnet;
        internal System.Windows.Forms.Label labelTFCR;
        internal System.Windows.Forms.Label labelER;
        internal System.Windows.Forms.Label labelKBGEvent;
        internal System.Windows.Forms.Label labelIR;
        private System.Windows.Forms.Button buttonRefreshServerStatus;
        private System.Windows.Forms.Label labelMinecraftLoginServersResult;
        private System.Windows.Forms.Label labelMinecraftdotnetResult;
        private System.Windows.Forms.Label labelTFCRResult;
        private System.Windows.Forms.Label labelERResult;
        private System.Windows.Forms.Label labelKBGEventResult;
        private System.Windows.Forms.Label labelIRResult;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ProgressBar progressBarIR;
        private System.Windows.Forms.Button buttonDebug;
        private System.Windows.Forms.ProgressBar progressBarTwitter;
        private System.Windows.Forms.ProgressBar progressBarMinecraftLoginServers;
        private System.Windows.Forms.ProgressBar progressBarMinecraftdotnet;
        private System.Windows.Forms.ProgressBar progressBarTFCR;
        private System.Windows.Forms.ProgressBar progressBarER;
        private System.Windows.Forms.ProgressBar progressBarKBGEvent;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox checkBoxRememberLoginInfo;
        private System.Windows.Forms.Panel panelDownload;
        private System.Windows.Forms.Label labelDownload;
        private System.Windows.Forms.ProgressBar progressBarDownload;
        private System.Windows.Forms.Button buttonDownloadCancel;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox textBoxDebug;
        private System.Windows.Forms.Label labelDownloadProgress;
        private System.Windows.Forms.Label labelDownloadSpeed;
        private System.Windows.Forms.Button buttonRefreshTwitterFeeds;
        private System.Windows.Forms.LinkLabel linkLabelKB_Gaming;
        private System.Windows.Forms.LinkLabel linkLabelCredits;
    }
}

