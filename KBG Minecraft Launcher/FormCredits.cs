using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace KBG_Minecraft_Launcher
{
    public partial class FormCredits : Form
    {
        FormOptions _formOptions;
        Thread CreditsThread;
        string _selectedPack;

        public FormCredits()
        {
            InitializeComponent();
        }

        //public FormCredits(FormOptions formOptions)
        //{
        //    InitializeComponent();
        //    _formOptions = formOptions;
        //}

        //public void SetCredits(string credits)
        //{
        //    richTextBox1.Rtf = credits;
        //}

        public void GetAndShowCredits(string selectedItem, FormOptions formOptions)
        {
            try
            {
                _selectedPack = selectedItem;
                _formOptions = formOptions;
                CreditsThread = new Thread(new ThreadStart(this.DoWorkCreditInfo));

                CreditsThread.Start();
                while (!CreditsThread.IsAlive) ;
            }
            catch (Exception ex)
            {
                (ParentForm as FormMain).ErrorReporting(ex, false);
            }
            
        }

        private void DoWorkCreditInfo()
        {
            try
            {
                xmlVersionInfo versionInfo = _formOptions.GetVersionInfo(_selectedPack, true);

                if (versionInfo.Credits != "")
                    this.Invoke(new Action(delegate() { richTextBox1.Rtf = versionInfo.Credits; }));
                else
                    this.Invoke(new Action(delegate() { richTextBox1.Rtf = @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Microsoft Sans Serif;}}\viewkind4\uc1\pard\f0\fs17 Credits missing or not yet configured\par}"; }));
                this.Invoke(new Action(delegate() { this.panel1.Visible = false; })); 
            }
            catch (Exception ex)
            {
                (ParentForm as FormMain).ErrorReporting(ex, false);
            }
        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(e.LinkText);
            }
            catch (Exception ex)
            {
                (ParentForm as FormMain).ErrorReporting(ex, false);
            }
        }

    //    private void FormCredits_Resize(object sender, EventArgs e)
    //    {
    ////        panel1.Location = new Point(
    ////this.ClientSize.Width / 2 - panel1.Size.Width / 2,
    ////this.ClientSize.Height / 2 - panel1.Size.Height / 2);
    ////        panel1.Top = (this.Height - panel1.Height) / 2;
    ////        panel1.Left = (this.Width - panel1.Width) / 2;
    //    }
    }
}
