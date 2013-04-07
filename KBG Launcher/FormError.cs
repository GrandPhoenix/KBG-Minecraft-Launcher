using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace KBG_Launcher
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
            try
            {
                textBox1.Text += RemovePersonalInfo(infoLine) + Environment.NewLine;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string RemovePersonalInfo(string info)
        {
            string username1 = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            string username2 = Environment.UserName;
            string returnstring = "";
            try
            {
                returnstring = info.Replace(username1, "xxxxx");
                returnstring = returnstring.Replace(username2, "xxxxx");

                return returnstring;
            }
            catch (Exception ex)
            {
                throw ex;
                //MessageBox.Show("An error occured while removing personal information from the error log." + Environment.NewLine + "Error: " + ex.Message, "An error occured");
            }
            //return "";
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

        private void buttonOploadPastebin_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxPastebinLink.Text = "Uploading... Please wait";
                this.Update();

                System.Collections.Specialized.NameValueCollection Data = new System.Collections.Specialized.NameValueCollection();

                Data["api_paste_name"] = "KBG Launcher Error Log";
                Data["api_paste_expire_date"] = "1M";
                Data["api_paste_code"] = textBox1.Text;
                Data["api_dev_key"] = "f36d451818d8bb354f1c344a508be4d8";
                Data["api_option"] = "paste";

                WebClient wb = new WebClient();
                byte[] bytes = wb.UploadValues("http://pastebin.com/api/api_post.php", Data);

                string response;
                using (MemoryStream ms = new MemoryStream(bytes))
                    using (StreamReader reader = new StreamReader(ms))
                        response = reader.ReadToEnd();

                if (response.StartsWith("Bad API request"))
                {
                    throw new Exception("The server returned a ''Bad API request''");
                }
                else
                {
                    buttonOploadPastebin.Enabled = false;
                    tableLayoutPanel2.Enabled = true;
                    Clipboard.SetText(response);
                    textBoxPastebinLink.Text = response;
                    MessageBox.Show("The log was succesfully uploaded to pastebin and the link was copied to the clipboard", "Upload successful");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured while uploading to Pastebin: " + Environment.NewLine + ex.Message, "Error");
            }
        }
    }
}
