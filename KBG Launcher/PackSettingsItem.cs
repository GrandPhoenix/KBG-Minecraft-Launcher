using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KBG_Launcher
{
    public partial class PackSettingsItem : UserControl
    {
        private PackSettings _parent;
        private bool _constuctorAddingStuff = false;

        public string PackName
        {
            get { return groupBoxPack.Text; }
        }

        public string PackDownloadUrl
        {
            get { return textBoxPackUrl.Text; }
        }

        public string PackVersionUrl
        {
            get { return textBoxVersionUrl.Text; }
        }

        public PackSettingsItem()
        {
            InitializeComponent();
        }

        public PackSettingsItem(string name, string downloadUrl, string versionUrl, PackSettings parent)
        {
            InitializeComponent();
            _constuctorAddingStuff = true;
            groupBoxPack.Text = name;
            textBoxPackUrl.Text = downloadUrl;
            textBoxVersionUrl.Text = versionUrl;
            _parent = parent;
            _constuctorAddingStuff = false;

        }       

        private void PackSettingsItem_MouseEnter(object sender, EventArgs e)
        {
            if (this.ParentForm.ContainsFocus)
            {
                buttonDelete.Visible = true;
                Console.WriteLine("PackSettingsItem_MouseEnter");
            }
        }

        private void PackSettingsItem_MouseLeave(object sender, EventArgs e)
        {
            //if (this.ParentForm.ContainsFocus)
            if (groupBoxPack.GetChildAtPoint(groupBoxPack.PointToClient(MousePosition)) == null)
            {
                buttonDelete.Visible = false;
                Console.WriteLine("PackSettingsItem_MouseLeave");
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the " + groupBoxPack.Text + " pack from the list?" + Environment.NewLine + "This action cannot be undone", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                (ParentForm as FormOptions).DeletePack(groupBoxPack.Text);
                _parent.RemovePack(this);
            }
        }

        private void SaveInfo()
        {
            // //(Parent as PackSettings).SavePack(new PackClass(groupBoxPack.Text,-1,textBoxPackUrl.Text,textBoxVersionUrl.Text));
            //PackClass existingPack = (ParentForm as FormOptions).GetPackInfoFromPackName(groupBoxPack.Text);

            // //merges the version with the new information            
            //(ParentForm as FormOptions).Settings.SavePackInfo(new PackClass(groupBoxPack.Text, existingPack.Version, textBoxPackUrl.Text, textBoxVersionUrl.Text));  
        }

        private void textBox_Leave(object sender, EventArgs e)
        {
            //Console.WriteLine("textBoxPackUrl_Leave");
            SaveInfo();
            labelChanges1.Visible = false;
            labelChanges2.Visible = false;
        }

        private void GroupBox_MouseMove(object sender, MouseEventArgs e)
        {
            buttonDelete.Visible = (e.X <= this.Width && e.X >= 0 && e.Y <= this.Height && e.Y >= 0);
            //{
            //    buttonDelete.Visible = true;
            //}
            
        }

        private void textBoxPackUrl_TextChanged(object sender, EventArgs e)
        {
            if (!_constuctorAddingStuff)
                labelChanges1.Visible = true;
        }

        private void textBoxVersionUrl_TextChanged(object sender, EventArgs e)
        {
            if (!_constuctorAddingStuff) 
                labelChanges2.Visible = true;
        }

        
    }    
}
