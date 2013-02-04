using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Xml;
using Microsoft.Win32;

namespace KBG_Minecraft_Launcher
{
    public partial class FormOptions : Form
    {
        public enum SupportetAutoUpdatePack { IR=0, ER, TFCR, Vanilla };        

        FormMain _formMain;
        private xmlSettings _settings;
        //private List<PackClass> _packsInfo = new List<PackClass>();
        private bool _LoadingSettings = false;

#if (DEBUG) //debug
        private string _KBGClientVersionUrl = "https://dl.dropbox.com/s/iu0qn58zdqxlqty/KBGClientVersion.xml";
        private string _KBGClientUpdateUrl = "https://dl.dropbox.com/s/clwfuihcnhjtchd/KBG%20Minecraft%20Launcher.exe";

        private const string _packIRName = "Industrial Rage";
        private const string _packIRUpdateUrl = "https://dl.dropbox.com/s/bzo0ab0ggpg40t3/IRpack.zip";
        private const string _packIRUpdateVersionUrl = "https://dl.dropbox.com/s/v6vnuqirlveqdd4/IRVersion.xml";

        private const string _packERName = "Endless Rage";
        private const string _packERUpdateUrl = "https://dl.dropbox.com/s/m504beqo6h315b6/ERPack.zip";
        private const string _packERUpdateVersionUrl = "https://dl.dropbox.com/s/4ztq99vmg16otcb/ERVersion.xml";

        private const string _packTFCRName = "Terrafirma Rage";
        private const string _packTFCRUpdateUrl = "https://dl.dropbox.com/s/cczexatyb64pm4f/TFRpack.zip"; 
        private const string _packTFCRUpdateVersionUrl = "https://dl.dropbox.com/s/j7ltqdaycxm0wru/TFRVersion.xml";

        private const string _packVanillaName = "Vanilla";
        private const string _packVanillaUpdateUrl = "https://dl.dropbox.com/s/p17f8pcz9k9tit2/VanillaPack.zip";
        private const string _packVanillaUpdateVersionUrl = "https://dl.dropbox.com/s/ph4s9n125smtj44/VanillaVersion.xml";
        

                
#endif
#if (!DEBUG) //release

        private string _KBGClientVersionUrl = "https://dl.dropbox.com/s/y1kjdxqwnl935fk/KBGClientVersion.xml";
        private string _KBGClientUpdateUrl = "https://dl.dropbox.com/s/fdfrns4ww5wvoxo/KBG%20Minecraft%20Launcher.exe";
        
        private const string _packIRName = "Industrial Rage";
        private const string _packIRUpdateUrl = "https://dl.dropbox.com/u/32095369/IR.zip";
        private const string _packIRUpdateVersionUrl = "https://dl.dropbox.com/u/32095369/IR.xml";

        private const string _packERName = "Endless Rage";
        private const string _packERUpdateUrl = "https://dl.dropbox.com/u/32095369/ER.zip";
        private const string _packERUpdateVersionUrl = "https://dl.dropbox.com/u/32095369/ER.xml";

        private const string _packTFCRName = "Terrafirma Rage";
        private const string _packTFCRUpdateUrl = "https://dl.dropbox.com/u/32095369/TFR.zip";
        private const string _packTFCRUpdateVersionUrl = "https://dl.dropbox.com/u/32095369/TFR.xml"; 

        private const string _packVanillaName = "Vanilla";
        private const string _packVanillaUpdateUrl = "https://dl.dropbox.com/s/jp4qrfq3xnbwmdu/VanillaPack.zip";
        private const string _packVanillaUpdateVersionUrl = "https://dl.dropbox.com/s/nog81tee51yemj6/VanillaVersion.xml";

#endif





        //public double KBGClientVersion
        //{
        //    get 
        //    {
        //        //return ClientVersionToDouble(Application.ProductVersion); 
        //        double returnValue = 0;
        //        try
        //        {
        //            Assembly ass = Assembly.GetExecutingAssembly();

        //            if (ass != null)
        //            {
        //                FileVersionInfo FVI = FileVersionInfo.GetVersionInfo(ass.Location);
        //                returnValue = FVI.FileMajorPart;
        //                returnValue += (double)FVI.FileMinorPart / 1000;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw;
        //        }
        //        return returnValue;
        //    }
        //}

        //public double KBGClientVersionFromUrl
        //{
        //    get { return ClientVersionFromUrl(); }
        //}

        public string KBGClientUpdateUrl
        {
            get { return _KBGClientUpdateUrl; }
        }

        //public string KBGClientVersionUrl
        //{
        //    get { return _KBGClientVersionUrl; }
        //}


        public string PackIRName
        {
            get { return _packIRName; }
        }
        public string PackERName
        {
            get { return _packERName; }
        }
        public string PackTFCRName
        {
            get { return _packTFCRName; }
        }
        public string PackVanillaName
        {
            get { return _packVanillaName; }
        }

        //public List<PackClass> Packs
        //{
        //    get { return _packsInfo; }            
        //}

        //public PackSettings MCPackSettings
        //{
        //    get { return packSettings1; }
        //    set { packSettings1 = value; }
        //}

        public xmlSettings Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }
        
        //public string DownloadLinkVanillaMC
        //{
        //    get { return ""; }
        //}              

        //public string DownloadLinkTFCR
        //{
        //    //old links
        //    //http://205.196.121.190/wml3sgkfr7xg/2jg54a9fcdk1d5p/TFRpack12-16-2012.zip

        //    get { return ""; }
        //}

        //public string DownloadLinkIR
        //{
        //    get { return ""; }
        //}

        //public string DownloadLinkER
        //{
        //    get { return ""; }
        //}

        //public double GetIrPackVersion
        //{
        //    get { return GetPackVersion("https://www.dropbox.com/s/s7dh3a0pxawsyxh/IRVersion.txt"); }
        //}

        //public double GetTFRPackVersion
        //{
        //    get { return GetPackVersion("https://www.dropbox.com/s/ku39s71paof1ydd/TFRVersion.txt"); }
        //} 

        

        public FormOptions()
        {
            InitializeComponent();
        }

        public FormOptions(FormMain parent)
        {
            InitializeComponent();
            _formMain = parent;

            try
            {
                string LinksRTF = @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Segoe UI;}{\f1\fnil\fcharset0 Microsoft Sans Serif;}}";
                LinksRTF += @"\viewkind4\uc1\pard\f0\fs18 ";//                
                LinksRTF += @"KillerBeesGaming.com \tab -  http://www.killerbeesgaming.com/content.php\par ";
                LinksRTF += @"KillerBees twitter page \tab -  https://twitter.com/intent/user?screen_name=KB_Gaming\par ";
                LinksRTF += @"Industrial Rage \tab\tab -  http://www.killerbeesgaming.com/games.php?do=mc#industrial\par ";
                LinksRTF += @"Endless Rage \tab\tab -  http://www.killerbeesgaming.com/games.php?do=mc#endless\par ";
                LinksRTF += @"Terrafirma Rage \tab\tab -  http://www.killerbeesgaming.com/games.php?do=mc#tfcraft\par ";
                LinksRTF += @"Vanilla Minecraft \tab\tab -  https://minecraft.net/download\f1\fs17\par}"; //
                richTextBoxLinks.Rtf = LinksRTF;

                if (_settings == null)
                    _settings = new xmlSettings(); //creates xml module, and loads all packs

                //_packsInfo = _settings.LoadPacks();

                //if (_packsInfo.Count == 0)
                //{
                //    _packsInfo.Add(new PackClass(_packIRName, 0, "https://www.dropbox.com/s/v26po41rzzun31i/IRpack.zip", "https://www.dropbox.com/s/s7dh3a0pxawsyxh/IRVersion.txt"));
                //    _packsInfo.Add(new PackClass(_packERName, 0, "https://www.dropbox.com/s/sppgwbtlm3hoeeo/ERPack.zip", "https://www.dropbox.com/s/a0vd96eiwp8frt1/ERVersion.txt"));
                //    _packsInfo.Add(new PackClass(_packTFCRName, 0, "https://www.dropbox.com/s/jws5yja076d7zco/TFRpack.zip", "https://www.dropbox.com/s/ku39s71paof1ydd/TFRVersion.txt"));
                //    _packsInfo.Add(new PackClass(_packVanillaName, 0, "https://www.dropbox.com/s/wno98avd34pg34o/VanillaPack.zip", "https://www.dropbox.com/s/0sy2id6ss6l76bt/Vanilla.txt"));

                //    foreach (PackClass pack in _packsInfo)
                //    {
                //        Settings.SavePackInfo(pack);
                //        //packSettings1.AddPack(pack);
                //    }
                //    //Settings.SavePackInfo();
                //    //Settings.SavePackInfo(
                //    //Settings.SavePackInfo(

                //    //MCPackSettings.AddPack(new PackClass(_packIRName, 0, "https://www.dropbox.com/s/v26po41rzzun31i/IRpack.zip", "https://www.dropbox.com/s/s7dh3a0pxawsyxh/IRVersion.txt"));
                //    //MCPackSettings.AddPack(new PackClass(_packERName, 0, "https://www.dropbox.com/s/sppgwbtlm3hoeeo/ERPack.zip", "https://www.dropbox.com/s/a0vd96eiwp8frt1/ERVersion.txt"));
                //    //MCPackSettings.AddPack(new PackClass(_packTFCRName, 0, "https://www.dropbox.com/s/jws5yja076d7zco/TFRpack.zip", "https://www.dropbox.com/s/ku39s71paof1ydd/TFRVersion.txt"));
                //    //MCPackSettings.AddPack(new PackClass(_packVanillaName, 0, "https://www.dropbox.com/s/wno98avd34pg34o/VanillaPack.zip", "https://www.dropbox.com/s/0sy2id6ss6l76bt/Vanilla.txt"));

                //}

                //foreach (PackClass pack in _packsInfo)
                //{
                //    //Settings.SavePackInfo(pack);
                //    packSettings1.AddPack(pack);
                //}

                //generate default folders
                string packDir = Application.StartupPath + "\\Minecraft Packs";


                if (!Directory.Exists(packDir + "\\" + _packIRName))
                    Directory.CreateDirectory(packDir + "\\" + _packIRName);

                if (!Directory.Exists(packDir + "\\" + _packERName))
                    Directory.CreateDirectory(packDir + "\\" + _packERName);

                if (!Directory.Exists(packDir + "\\" + _packTFCRName))
                    Directory.CreateDirectory(packDir + "\\" + _packTFCRName);

                if (!Directory.Exists(packDir + "\\" + _packVanillaName))
                    Directory.CreateDirectory(packDir + "\\" + _packVanillaName);
                

                //if (!Directory.Exists(packDir) || Directory.GetDirectories(packDir).Length == 0)
                //{
                //    Directory.CreateDirectory(packDir + "\\" + _packIRName);
                //    Directory.CreateDirectory(packDir + "\\" + _packERName);
                //    Directory.CreateDirectory(packDir + "\\" + _packTFCRName);
                //    Directory.CreateDirectory(packDir + "\\" + _packVanillaName);
                //}

                labelVersion.Text = string.Format("Version {0}.{1}.{2}.{3}",GetClientVersion().VersionMajor,GetClientVersion().VersionMinor,GetClientVersion().VersionRevision,GetClientVersion().VersionPack); //KBGClientVersion.ToString();
                labelJavaVersion.Text = GetJavaVersion();

                _LoadingSettings = true;

                numericUpDownRamMin.Value = _settings.MemmoryMin;
                numericUpDownRamMax.Value = _settings.MemmoryMax;

                _LoadingSettings = false;
            }
            catch (UnauthorizedAccessException ex)
            {
                ex.Data.Add("FormOptions() - ExecutablePath", Application.ExecutablePath);
                string message = string.Format("The program encountered a situation where it did not have enough permissions to do its work in its own folder and cannot continue to function.{0}Please make sure you put the {1} file in a folder with more permissions!{0}{0}I suggest a place like c:\\Games\\Minecraft", Environment.NewLine, new FileInfo(Application.ExecutablePath).Name);
                MessageBox.Show(message, "Insufficiant permissions", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                _formMain.ErrorReporting(ex, true);
            }
            catch (Exception ex)
            {
                _formMain.ErrorReporting(ex, true);
                throw;
            }
        }

        /// <summary>
        /// Made to gather information for Error Reporting
        /// </summary>
        /// <returns></returns>
        internal List<string> formOptionsInformation()
        {
            List<string> information = new List<string>();

            information.Add("_settings null? " + (_settings == null).ToString());
            information.Add("_loadingSettings = " + _LoadingSettings.ToString());

            //information.Add("_KBGClientVersionUrl = " + _KBGClientVersionUrl);
            //information.Add("_KBGClientUpdateUrl = " + _KBGClientUpdateUrl);

            //information.Add("_packIRName = " + _packIRName);
            //information.Add("_packIRUpdateUrl = " + _packIRUpdateUrl);
            //information.Add("_packIRUpdateVersionUrl = " + _packIRUpdateVersionUrl);

            //information.Add("_packERName = " + _packERName);
            //information.Add("_packERUpdateUrl = " + _packERUpdateUrl);
            //information.Add("_packERUpdateVersionUrl = " + _packERUpdateVersionUrl);

            //information.Add("_packTFCRName = " + _packTFCRName);
            //information.Add("_packTFCRUpdateUrl = " + _packTFCRUpdateUrl);
            //information.Add("_packTFCRUpdateVersionUrl = " + _packTFCRUpdateVersionUrl);

            //information.Add("_packVanillaName = " + _packVanillaName);
            //information.Add("_packVanillaUpdateUrl = " + _packVanillaUpdateUrl);
            //information.Add("_packVanillaUpdateVersionUrl = " + _packVanillaUpdateVersionUrl);


            information.Add(Environment.NewLine + "Pack Information" + Environment.NewLine + "{");
            if (Directory.Exists(Application.StartupPath + "\\Minecraft Packs"))            
                foreach (string path in Directory.GetDirectories(Application.StartupPath + "\\Minecraft Packs"))
                    foreach (string path2 in Directory.GetDirectories(path))
                        information.Add(path2.Replace(Application.StartupPath, ""));
            information.Add("}");
           


            return information;
        }

        public String GetJavaInstallationPath()
        {
            RegistryKey regkey = null;
            String currentVersion = "";
            try
            {
                //String javaKey = "SOFTWARE\\JavaSoft\\Java Runtime Environment";
                //regkey = Registry.LocalMachine.OpenSubKey(javaKey);

                ////using (var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(javaKey))
                ////using (var baseKey = Registry.LocalMachine.OpenSubKey(javaKey))

                ////using(var baseKey = regkey)
                ////{
                //currentVersion = regkey.GetValue("CurrentVersion").ToString();
                //using (var homeKey = regkey.OpenSubKey(currentVersion))
                //    return homeKey.GetValue("JavaHome").ToString();
                //}
                String javaKey = "SOFTWARE\\JavaSoft\\Java Runtime Environment";

                RegistryKey localMachineRegistry = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32);

                //MessageBox.Show(string.IsNullOrEmpty(javaKey) ? localMachineRegistry : localMachineRegistry.OpenSubKey(javaKey));



                regkey = localMachineRegistry.OpenSubKey(javaKey);

                //using (var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(javaKey))
                //using (var baseKey = Registry.LocalMachine.OpenSubKey(javaKey))

                //using(var baseKey = regkey)
                //{
                currentVersion = regkey.GetValue("CurrentVersion").ToString();
                using (var homeKey = regkey.OpenSubKey(currentVersion))
                    return homeKey.GetValue("JavaHome").ToString();

            }
            catch (Exception ex)
            {
                ex.Data.Add("GetJavaInstallationPath() - currentVersion", currentVersion);
                ex.Data.Add("GetJavaInstallationPath() - regkey null?", regkey == null);
                throw ex;
            }
        }


        private string GetJavaVersion()
        {
            RegistryKey regkey = null;
            string currentVersion = "";
            try
            {
                //String javaKey = "SOFTWARE\\JavaSoft\\Java Runtime Environment";
                ////using (var baseKey = Registry.LocalMachine.OpenSubKey(javaKey))
                //regkey = Registry.LocalMachine.OpenSubKey(javaKey);
                ////RegistryKey regKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                //if (regkey == null)
                //    return "unknown, wotking on a fix";
                //else
                //    //using (var baseKey = regkey.OpenSubKey(javaKey))
                //    //{
                //        //if(baseKey == null)
                //        //    return "unknown, wotking on a fix";
                //        //else
                //            return regkey.GetValue("CurrentVersion").ToString();
                //        //using (var homeKey = baseKey.OpenSubKey(currentVersion))
                //        //    return homeKey.GetValue("JavaHome").ToString();
                //    //}
                String javaKey = "SOFTWARE\\JavaSoft\\Java Runtime Environment";

                RegistryKey localMachineRegistry = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32);

                //MessageBox.Show(string.IsNullOrEmpty(javaKey) ? localMachineRegistry : localMachineRegistry.OpenSubKey(javaKey));



                regkey = localMachineRegistry.OpenSubKey(javaKey);

                //using (var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(javaKey))
                //using (var baseKey = Registry.LocalMachine.OpenSubKey(javaKey))

                //using(var baseKey = regkey)
                //{
                return regkey.GetValue("CurrentVersion").ToString();
                //using (var homeKey = regkey.OpenSubKey(currentVersion))
                //    MessageBox.Show(homeKey.GetValue("JavaHome").ToString());
            }
            catch (Exception ex)
            {
                ex.Data.Add("GetJavaInstallationPath() - regkey null?", regkey == null);
                throw ex;
            }
        }


        public string PackUpdateUrl(string packName)
        {
            string returnString = "";
            switch (packName)
            {
                case _packIRName:
                    returnString = _packIRUpdateUrl;
                    break;
                case _packERName:
                    returnString = _packERUpdateUrl;
                    break;
                case _packTFCRName:
                    returnString = _packTFCRUpdateUrl;
                    break;
                case _packVanillaName:
                    returnString = _packVanillaUpdateUrl;
                    break;
            }
            return returnString;
        }

        /// <summary>
        /// Gets an xmlVersionInfo object with information about the version and news extracted from the xml file
        /// </summary>
        /// <param name="pack">SupportetAutoUpdatePack type of pack to retrieve information from</param>
        /// <param name="remoteFile">true: getting info from a web link, false: get the info froma  local file on the harddrive</param>
        /// <returns></returns>
        public xmlVersionInfo GetVersionInfo(string packName, bool remoteFile)
        {
            xmlVersionInfo returnInfo = new xmlVersionInfo();
            try
            {                
                switch (packName)
                {                    
                    case _packIRName:
                        if (remoteFile)
                            returnInfo = new xmlVersionInfo(_packIRUpdateVersionUrl, true);
                        else
                            returnInfo = new xmlVersionInfo(Application.StartupPath + "\\Minecraft Packs\\" + _packIRName + "\\PackInfo.xml", false);
                        break;
                    case _packERName:
                        if (remoteFile)
                            returnInfo = new xmlVersionInfo(_packERUpdateVersionUrl, true);
                        else
                            returnInfo = new xmlVersionInfo(Application.StartupPath + "\\Minecraft Packs\\" + _packERName + "\\PackInfo.xml", false);
                        break;
                    case _packTFCRName:
                        if (remoteFile)
                            returnInfo = new xmlVersionInfo(_packTFCRUpdateVersionUrl, true);
                        else
                            returnInfo = new xmlVersionInfo(Application.StartupPath + "\\Minecraft Packs\\" + _packTFCRName + "\\PackInfo.xml", false);
                        break;
                    case _packVanillaName:
                        if (remoteFile)
                            returnInfo = new xmlVersionInfo(_packVanillaUpdateVersionUrl, true);
                        else
                            returnInfo = new xmlVersionInfo(Application.StartupPath + "\\Minecraft Packs\\" + _packVanillaName + "\\PackInfo.xml", false);
                        break;
                }
                
            }
            catch (Exception ex)
            {
                throw ex;           
            }
            return returnInfo;
        }

        public xmlVersionInfo GetClientVersion()
        {
            xmlVersionInfo returnValue = new xmlVersionInfo();
            try
            {
                Assembly ass = Assembly.GetExecutingAssembly();


                if (ass != null)
                {
                    FileVersionInfo FVI = FileVersionInfo.GetVersionInfo(ass.Location);
                    returnValue.VersionMajor = FVI.FileMajorPart;
                    returnValue.VersionMinor = FVI.FileMinorPart;
                    returnValue.VersionRevision = FVI.FileBuildPart;
                    returnValue.VersionPack = FVI.FilePrivatePart;
                    //returnValue += (double)FVI.FileMinorPart / 1000;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public xmlVersionInfo GetClientUpdateInfo()
        {
            xmlVersionInfo returnInfo = new xmlVersionInfo();

            try
            {
                returnInfo = new xmlVersionInfo(_KBGClientVersionUrl, true);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return returnInfo;
        }


        //public PackClass GetPackInfoFromPackName(string packName)
        //{
        //    PackClass Returnpack = new PackClass();

        //    foreach (PackClass pack in _packsInfo)
        //    {
        //        if (pack.Name == packName)
        //        {
        //            Returnpack = pack;
        //            break;
        //        }
        //    }

        //    return Returnpack;
        //}

        

        public void DeletePack(string packName)
        {
            try
            {
                _settings.DeletePackInfo(packName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        


        static ulong GetTotalMemoryInBytes()
        {
            try
            {
                return new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void buttonSetRam_Click(object sender, EventArgs e)
        {
            try
            {
                numericUpDownRamMin.Value = decimal.Round(GetTotalMemoryInBytes() / 3000000);
                numericUpDownRamMax.Value = decimal.Round(GetTotalMemoryInBytes() / 3000000);
            }
            catch (Exception ex)
            {
                _formMain.ErrorReporting(ex, false);
            }
        }

        private void FormOptions_Load(object sender, EventArgs e)
        {
            try
            {
                numericUpDownRamMax.Maximum = decimal.Round(GetTotalMemoryInBytes() / 1048576);
                numericUpDownRamMin.Maximum = decimal.Round(GetTotalMemoryInBytes() / 1048576);
            }
            catch (Exception ex)
            {
                _formMain.ErrorReporting(ex, false);
            }
        }

        public int GetMemmoryMin()
        {
            return (int)numericUpDownRamMin.Value;
        }

        public int GetMemmoryMax()
        {
            return (int)numericUpDownRamMax.Value;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            SaveOptions();
            this.Hide();
        }

        private void FormOptions_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveOptions();
        }

        private void SaveOptions()
        {
            try
            {
                _settings.MemmoryMin = (int)numericUpDownRamMin.Value;
                _settings.MemmoryMax = (int)numericUpDownRamMax.Value;
            }
            catch (Exception ex)
            {
                _formMain.ErrorReporting(ex, false);
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="version"></param>
        ///// <returns></returns>
        //private double ClientVersionToDouble(string version)
        //{
        //    double returnValue = 0;
        //    string[] split = version.Split('.');

        //    try
        //    {
        //        Assembly ass = Assembly.GetExecutingAssembly();
                
        //        if (ass != null)
        //        {
        //            FileVersionInfo FVI = FileVersionInfo.GetVersionInfo(ass.Location);
        //            returnValue = FVI.FileMajorPart;
        //            returnValue += (double)FVI.FileMinorPart / 1000;                    
        //        }

        //        //if (split.Length != 4)
        //        //{
        //        //    throw new Exception("ClientVersionToDouble() failed. String was not in the expected format");
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    return returnValue;
        //}

        public bool CheckNameForAutoUpdateSupport(string packName)
        {
            bool returnValue = false;
            switch (packName)
            {
                case _packIRName:
                case _packERName:
                case _packTFCRName:
                case _packVanillaName:
                    returnValue = true;
                    break;                    
            }
            return returnValue;
        }

        private void richTextBoxLinks_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(e.LinkText);
            }
            catch (Exception ex)
            {
                _formMain.ErrorReporting(ex, false);
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
