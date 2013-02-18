using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Tester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {           
            XmlDocument xmlDoc = new XmlDocument();
            List<string> list = new List<string>();
            List<string> listFinal = new List<string>();

            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                xmlDoc.LoadXml(client.DownloadString(@"https://s3.amazonaws.com/assets.minecraft.net/"));
            }

            foreach (XmlElement element in xmlDoc.LastChild.ChildNodes)
            {
                if (element.Name == "Contents")
                {
                    bool above2012 = int.Parse(element["LastModified"].InnerXml.Substring(0,4)) >= 2012;
                    if(above2012)
                        list.Add(element["Key"].InnerXml);
                }
            }
            list.RemoveAll(ContainsW);
            list.RemoveAll(IsNotVersionNumber);

            foreach (string str in list)
            {
                string[] split = str.Split('/');
                if (!listFinal.Contains(split[0]))
                    listFinal.Add(split[0]);
            }

            foreach (string str in listFinal)
            {
                int NewRowIndex = dataGridView1.Rows.Add();
                DataGridViewRow newRow = dataGridView1.Rows[NewRowIndex];

                newRow.Cells["Key"].Value = str;
            }

            dataGridView1.AutoResizeColumns();
        }

        private bool ContainsW(string s)
        {
            return s.ToLower().Contains("w");
        }

        private bool IsNotVersionNumber(string s)
        {
            int nothing = 0;
            return !int.TryParse(s.Substring(0, 1), out nothing);
        }
    }
}
