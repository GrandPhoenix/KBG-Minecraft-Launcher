using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
//using PhoenixRTB;
using System.Xml;
using System.Diagnostics;

namespace KBG_Minecraft_Launcher_NewsWriter
{
    public partial class FormPackInfoWriter : Form
    {
        string _openFile = "";
        bool _loadingStuff = false;

        public FormPackInfoWriter()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                string resourceName = new AssemblyName(args.Name).Name + ".dll";
                string resource = Array.Find(this.GetType().Assembly.GetManifestResourceNames(), element => element.EndsWith(resourceName));

                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource))
                {
                    Byte[] assemblyData = new Byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    return Assembly.Load(assemblyData);
                }
            };
            InitializeComponent();
        }
                
        private void buttonOpen_Click(object sender, EventArgs e)
        {
            //bool InvalidXML = false;
            OpenFileDialog oFileDialog = new OpenFileDialog();

            oFileDialog.Filter = "Xml files (*.xml)|*.xml|All files (*.*)|*.*";
            oFileDialog.Multiselect = false;
            oFileDialog.DefaultExt = "xml";

            if (_openFile != "")
                oFileDialog.InitialDirectory = _openFile;
            else
                oFileDialog.InitialDirectory = Application.StartupPath;

            if (oFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                XmlDocument xDoc = new XmlDocument();
                try
                {
                    xDoc.Load(oFileDialog.FileName);
                    string tmpNews = "";
                    string tmpCredits = "";
                    int tmpInt = 0;
                    bool tmpBool = false;
                    XmlNodeList Excludes;
                    XmlNode tmpNode;

                    _loadingStuff = true;

                    _openFile = oFileDialog.FileName;
                    labelCurrentFile.Text = oFileDialog.FileName;
                    

                    //Version - Major
                    tmpNode = xDoc.SelectSingleNode("KBGVersionInfo/VersionMajor");
                    if (tmpNode != null)
                    {
                        int.TryParse(tmpNode.InnerText, out tmpInt);
                        numericUpDownMajor.Value = tmpInt;
                    }
                    else
                        numericUpDownMajor.Value = 0;

                    //Version - Minor
                    tmpNode = xDoc.SelectSingleNode("KBGVersionInfo/VersionMinor");
                    if (tmpNode != null)
                    {
                        int.TryParse(tmpNode.InnerText, out tmpInt);
                        numericUpDownMinor.Value = tmpInt;
                    }
                    else
                        numericUpDownMinor.Value = 0;

                    //Version - Revision
                    tmpNode = xDoc.SelectSingleNode("KBGVersionInfo/VersionRevision");
                    if (tmpNode != null)
                    {
                        int.TryParse(tmpNode.InnerText, out tmpInt);
                        numericUpDownRevision.Value = tmpInt;
                    }
                    else
                        numericUpDownRevision.Value = 0;

                    //Version - Revision
                    tmpNode = xDoc.SelectSingleNode("KBGVersionInfo/VersionPack");
                    if (tmpNode != null)
                    {
                        int.TryParse(tmpNode.InnerText, out tmpInt);
                        numericUpDownPack.Value = tmpInt;
                    }
                    else
                        numericUpDownPack.Value = 0;

                    //News
                    tmpNode = xDoc.SelectSingleNode("KBGVersionInfo/News");

                    if (tmpNode != null)
                        tmpNews = tmpNode.InnerText;


                    //News
                    tmpNode = xDoc.SelectSingleNode("KBGVersionInfo/Credits");

                    if (tmpNode != null)
                        tmpCredits = tmpNode.InnerText;


                    //PreventPackDownload
                    tmpNode = xDoc.SelectSingleNode("KBGVersionInfo/PreventPackDownload");

                    if (tmpNode != null)
                    {
                        bool.TryParse(tmpNode.InnerText, out tmpBool);
                        checkBoxPreventPackDownload.Checked = tmpBool;
                    }
                    else
                        checkBoxPreventPackDownload.Checked = false;


                    try
                    {
                        phoenixRichTextBoxNews.Rtf = tmpNews;
                        phoenixRichTextBoxCredits.Rtf = tmpCredits;
                    }
                    catch (FormatException)            
                    {
                        phoenixRichTextBoxNews.Text = tmpNews;
                        phoenixRichTextBoxCredits.Text = tmpCredits;
                    }
                    catch (ArgumentException)
                    {
                        phoenixRichTextBoxNews.Text = tmpNews;
                        phoenixRichTextBoxCredits.Text = tmpCredits;
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                    //Excludes
                    Excludes = xDoc.SelectNodes("KBGVersionInfo/ExcludeFromUpdate");
                    listBoxExcludes.Items.Clear();
                    if (Excludes != null)
                    {
                        foreach (XmlNode exclude in Excludes)
                            listBoxExcludes.Items.Add(exclude.InnerText);
                    }
                    _loadingStuff = false;
                }
                catch (XmlException ex)
                {
                    MessageBox.Show("This does not appear to be a valid xml file", "Invalid Xml file");
                    //InvalidXML = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error at buttonOpen_Click()" + Environment.NewLine + "Error: " + ex.Message, "Error");
                }
                buttonSave.Enabled = false;
            }
        }


        private void listBoxExcludes_SelectedIndexChanged(object sender, EventArgs e)
        {            
            if (listBoxExcludes.SelectedItems.Count == 0)
                buttonRemoveExclude.Enabled = false;
            else
                buttonRemoveExclude.Enabled = true;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (_openFile != "")            
                SaveXml(_openFile);            
        }


        private void SaveXml(string path)
        {
            //XmlDocument xmlDoc = new XmlDocument();
            //XmlNode RootNode;
            
            //XmlNode NodeVersionMajor;
            //XmlNode NodeVersionMinor = null;
            //XmlNode nodeVersionRevision;
            //XmlNode NodeNews;
            //XmlNode NodeExclude;


            try 
            {
                XmlDocument xmlDoc = new XmlDocument();
                string textContent = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + Environment.NewLine;
                textContent += @"<KBGVersionInfo>" + Environment.NewLine;
                textContent += string.Format("<{0}>{1}</{0}>{2}", "VersionMajor", numericUpDownMajor.Value.ToString(), Environment.NewLine);
                textContent += string.Format("<{0}>{1}</{0}>{2}", "VersionMinor", numericUpDownMinor.Value.ToString(), Environment.NewLine);
                textContent += string.Format("<{0}>{1}</{0}>{2}", "VersionRevision", numericUpDownRevision.Value.ToString(), Environment.NewLine);
                textContent += string.Format("<{0}>{1}</{0}>{2}", "VersionPack", numericUpDownPack.Value.ToString(), Environment.NewLine);
                textContent += string.Format("<{0}>{1}</{0}>{2}", "PreventPackDownload", checkBoxPreventPackDownload.Checked.ToString(), Environment.NewLine);
                textContent += string.Format("<{0}>{1}</{0}>{2}", "News", System.Security.SecurityElement.Escape(phoenixRichTextBoxNews.Rtf), Environment.NewLine);
                textContent += string.Format("<{0}>{1}</{0}>{2}", "Credits", System.Security.SecurityElement.Escape(phoenixRichTextBoxCredits.Rtf), Environment.NewLine);
                foreach(string item in listBoxExcludes.Items)
                    textContent += string.Format("<{0}>{1}</{0}>{2}", "ExcludeFromUpdate", item, Environment.NewLine);
                
                textContent += @"</KBGVersionInfo>" + Environment.NewLine;

                xmlDoc.LoadXml(textContent);

                using (FileStream fsxml = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    xmlDoc.Save(fsxml);
                }
                buttonSave.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error at SaveXml()" + Environment.NewLine + "Error: " + ex.Message, "Error");
            }
        }

        private void buttonSaveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog sFileDialog = new SaveFileDialog();

            sFileDialog.Filter = "Xml files (*.xml)|*.xml|All files (*.*)|*.*";           
            sFileDialog.DefaultExt = "xml";

            if (_openFile != "")
                sFileDialog.InitialDirectory = _openFile;
            else
                sFileDialog.InitialDirectory = Application.StartupPath;

            if (sFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {                
                SaveXml(sFileDialog.FileName);
                _openFile = sFileDialog.FileName;
                labelCurrentFile.Text = sFileDialog.FileName;
                buttonSave.Enabled = false;
            }


        }

        private void control_SomethingChanged(object sender, EventArgs e)
        {
            if (!_loadingStuff && _openFile != "")
                buttonSave.Enabled = true;
        }

        

        private void buttonRemoveExclude_Click(object sender, EventArgs e)
        {

            List<string> tmlList = new List<string>();
            foreach (string item in listBoxExcludes.SelectedItems)
                tmlList.Add(item);

            foreach (string item in tmlList)
                listBoxExcludes.Items.Remove(item);
            if(_openFile != "")
                buttonSave.Enabled = true;
        }

        private void buttonAddExclude_Click(object sender, EventArgs e)
        {
            OpenFileDialog oFileDialog = new OpenFileDialog();

            oFileDialog.Filter = "All files (*.*)|*.*";
            oFileDialog.Multiselect = true;

            if (_openFile != "")
                oFileDialog.InitialDirectory = _openFile;
            else
                oFileDialog.InitialDirectory = Application.StartupPath;

            if (oFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string item in oFileDialog.FileNames)
                {
                    listBoxExcludes.Items.Add(item.Substring(item.LastIndexOf(".Minecraft") + 11, item.Length - (item.LastIndexOf(".Minecraft") + 11)));
                }
                if(_openFile != "")
                    buttonSave.Enabled = true;
            }
        }

        private void textBoxExclude_TextChanged_1(object sender, EventArgs e)
        {
            buttonExcludeAddSingle.Enabled = (textBoxExclude.Text != "");
        }

        private void buttonExcludeAddSingle_Click(object sender, EventArgs e)
        {
            listBoxExcludes.Items.Add(textBoxExclude.Text);
            if(_openFile != "")
                buttonSave.Enabled = true;
        }

        private void FormPackInfoWriter_Load(object sender, EventArgs e)
        {
            Assembly ass = Assembly.GetExecutingAssembly();
            if (ass != null)
            {
                FileVersionInfo FVI = FileVersionInfo.GetVersionInfo(ass.Location);
                this.Text = string.Format("KBG PackInfo Writer v.{0}.{1}", FVI.FileMajorPart, FVI.FileMinorPart);
            }
        }

          
    }

    public class MyTabControl : TabControl
    {
        public override System.Drawing.Rectangle DisplayRectangle
        {
            get
            {
                Rectangle r = base.DisplayRectangle;
                //r.Inflate(4, 3);
                r.Inflate(1, 1);
                r.Offset(-1, 0);
                return r;
            }
        }
    }
}
