﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KBG_Launcher
{
    public partial class FormNews : Form
    {
        public FormNews()
        {
            InitializeComponent();
        }

        public FormNews(string news)
        {
            InitializeComponent();
            try
            {
                richTextBox1.Rtf = news;
            }
            catch (FormatException) //catch here if news is not a valid rtf text             
            {
                richTextBox1.Text = news;
            }
            catch (ArgumentException)
            {
                richTextBox1.Text = news;
            }
            catch (Exception )
            {
                throw;
            }
        }

        private void FormNews_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape || e.KeyChar == (char)Keys.Enter)
                this.Close();
        }

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape || e.KeyChar == (char)Keys.Enter)
                this.Close();
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
    }
}
