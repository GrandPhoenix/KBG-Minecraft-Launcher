using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace KBG_Minecraft_Launcher
{
    public partial class FormError : Form
    {
        private bool _criticalError = false;

        public bool CriticalError
        {
            get { return _criticalError; }
            set 
            {
                _criticalError = value;                
            }
        }


        public FormError()
        {
            InitializeComponent();
        }



        public void AddInfoLine(string infoLine)
        {
            textBox1.Text += infoLine + Environment.NewLine;
        }

        private void FormError_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_criticalError)
                Application.Exit();
            textBox1.Text = "";
        }

        private void FormError_Shown(object sender, EventArgs e)
        {
            if (_criticalError)
                MessageBox.Show("This error was a critical one and/or happened at a critical place." + Environment.NewLine + "To avoid a cascade of further errors, the program will therefore close when you exit this error log window!", "Critical error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            try
            {
                using (FileStream fstream = new FileStream(Application.StartupPath + "\\ErrorLog.txt", FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    using (TextWriter writer = new StreamWriter(fstream))
                    {
                        writer.Write(textBox1.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                //just absorps the exception
            }
        }
    }
}
