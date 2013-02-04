using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
                           //   "SOFTWARE\\JavaSoft\\Java Runtime Environment"
            String javaKey = "SOFTWARE\\JavaSoft\\Java Runtime Environment";

            RegistryKey localMachineRegistry = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32);

            //MessageBox.Show(string.IsNullOrEmpty(javaKey) ? localMachineRegistry : localMachineRegistry.OpenSubKey(javaKey));



            RegistryKey regkey = localMachineRegistry.OpenSubKey(javaKey);

            //using (var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(javaKey))
            //using (var baseKey = Registry.LocalMachine.OpenSubKey(javaKey))

            //using(var baseKey = regkey)
            //{
            string currentVersion = regkey.GetValue("CurrentVersion").ToString();
            using (var homeKey = regkey.OpenSubKey(currentVersion))
                MessageBox.Show(homeKey.GetValue("JavaHome").ToString());
        }
    }
}
