using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KBG_Minecraft_Launcher
{
    public partial class FormCredits : Form
    {
        public FormCredits()
        {
            InitializeComponent();
        }

        public void SetCredits(string credits)
        {
            richTextBox1.Rtf = credits;
        }
    }
}
