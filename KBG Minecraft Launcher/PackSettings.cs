using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KBG_Minecraft_Launcher
{
    public partial class PackSettings : UserControl
    {
        private List<PackSettingsItem> _items = new List<PackSettingsItem>();
                
        public PackSettings()
        {
            InitializeComponent();
            flowLayoutPanel1.VerticalScroll.Visible = true;
        }

        /// <summary>
        /// Adds a pack to the internal list, and to the gui list. Does NOT save to file
        /// </summary>
        /// <param name="newPack"></param>
        public void AddPack(PackClass newPack)
        {
            PackSettingsItem newItem = new PackSettingsItem(newPack.Name, newPack.DownloadUrl, newPack.VersionUrl, this);
            _items.Add(newItem);
            flowLayoutPanel1.Controls.Add(newItem);            

        }

        ///// <summary>
        ///// Designed to only be called from the PackSettingsItem class.
        ///// </summary>
        ///// <param name="pack"></param>
        //public void SavePack(PackClass pack)
        //{
        //    //get existing pack info from _formoptions (need the version for merging)
        //    PackClass existingPack = (ParentForm as FormOptions).GetPackInfoFromPackName(pack.Name);
            
        //    //merges the version with the new information            
        //    (ParentForm as FormOptions).Settings.SavePackInfo(new PackClass(pack.Name, existingPack.Version, pack.DownloadUrl, pack.VersionUrl));            
        //}

        public void RemovePack(PackSettingsItem packItem)
        {
            //(this.Parent as FormOptions).deletePackFromXml(packItem.PackName);
            _items.Remove(packItem);
            flowLayoutPanel1.Controls.Remove(packItem);
        }

        public void ClearItems()
        {
            _items.Clear();
        }

        private void PackSettings_MouseEnter(object sender, EventArgs e)
        {
            //if (this.ParentForm.ContainsFocus)
            //    flowLayoutPanel1.Focus();
        }

        private void buttonAddNewPack_Click(object sender, EventArgs e)
        {
            if (textBoxAddNewPack.Text == "")
            {
                MessageBox.Show("You need to give the new pack a name. Type it into the text box besides the button", "Name needed");
            }
            else
            {
                AddPack(new PackClass(textBoxAddNewPack.Text));
                //(ParentForm as FormOptions).Settings.SavePackInfo(new PackClass(textBoxAddNewPack.Text));
                textBoxAddNewPack.Text = "";
            }
        }
    }
}
