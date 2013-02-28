using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace KBG_Launcher
{
    public class xmlVersionInfo
    {
        private int _versionMajor = 0;
        private int _versionMinor = 0;
        private int _versionRevision = 0;
        private int _versionPack = 0;
        private List<string> _excludeFromUpdate = new List<string>();
        private bool _preventPackDownload = false;
        private string _updateNews = "";
        private string _credits = "";
        private string _installVersion = "";

        public List<string> ExcludeFromUpdate
        {
            get { return _excludeFromUpdate; }
        }

        public int VersionMajor
        {
            get { return _versionMajor; }
            set { _versionMajor = value; }
        }
        public int VersionMinor
        {
            get { return _versionMinor; }
            set { _versionMinor = value; }
        }
        public int VersionRevision
        {
            get { return _versionRevision; }
            set { _versionRevision = value; }
        }
        public int VersionPack
        {
            get { return _versionPack; }
            set { _versionPack = value; }
        }
        public string UpdateNews
        {
            get { return _updateNews; }
            set { _updateNews = value; }
        }
        public bool PreventPackDownload
        {
            get { return _preventPackDownload; }
            set { _preventPackDownload = value; }
        }
        public string Credits
        {
            get { return _credits; }
            set { _credits = value; }
        }
        public string InstallVersion
        {
            get { return _installVersion; }
            set { _installVersion = value; }
        }
        
        public xmlVersionInfo() { }

        public xmlVersionInfo(string url,bool WebAddress)
        {
            try
            {
                XmlNode versionMajor;
                XmlNode versionMinor;
                XmlNode versionRevision;
                XmlNode versionPack;
                XmlNode updateNews;
                XmlNode preventPackDownload;
                XmlNode credits;
                XmlNode installVersion;
                XmlNodeList Excludes;
                XmlDocument xmlDoc = new XmlDocument();
                                
                
                if (WebAddress)
                {
                    //webaddress ie. https://dl.dropbox.com/s/0fd399zvu72dkmb/KBGClientVersion.txt
                    using (System.Net.WebClient client = new System.Net.WebClient())
                    {
                        
                        xmlDoc.LoadXml(client.DownloadString(url));
                    }
                }
                else
                {
                    //Locas address ie. c:\games\something\KBGClientVersion.txt
                    string blankXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><KBGVersionInfo></KBGVersionInfo>";

                    if (File.Exists(url))
                        xmlDoc.Load(url);
                    else
                        xmlDoc.LoadXml(blankXml);
                }
                versionMajor = xmlDoc.SelectSingleNode("KBGVersionInfo/VersionMajor");
                versionMinor = xmlDoc.SelectSingleNode("KBGVersionInfo/VersionMinor");
                versionRevision = xmlDoc.SelectSingleNode("KBGVersionInfo/VersionRevision");
                versionPack = xmlDoc.SelectSingleNode("KBGVersionInfo/VersionPack");
                updateNews = xmlDoc.SelectSingleNode("KBGVersionInfo/News");
                credits = xmlDoc.SelectSingleNode("KBGVersionInfo/Credits");
                preventPackDownload = xmlDoc.SelectSingleNode("KBGVersionInfo/PreventPackDownload");
                Excludes = xmlDoc.SelectNodes("KBGVersionInfo/ExcludeFromUpdate");
                installVersion = xmlDoc.SelectSingleNode("KBGVersionInfo/InstallVersion");

                //if(version == null)
                //    throw new Exception("KBGVersionInfo/Version was not found in the update xml.");

                //if(!double.TryParse(version.InnerText,System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out _version))
                //    throw new Exception("KBGVersionInfo/Version was not a valid double value");
                if (versionMajor != null)
                    int.TryParse(versionMajor.InnerText, out _versionMajor);
                    

                if(versionMinor != null)
                    int.TryParse(versionMinor.InnerText, out _versionMinor);


                if (versionRevision != null)
                    int.TryParse(versionRevision.InnerText, out _versionRevision);

                if (versionPack != null)
                    int.TryParse(versionPack.InnerText, out _versionPack);
                    

                //if(updateNews == null)
                //    throw new Exception("KBGVersionInfo/News was not found in the update xml.");
                if(updateNews != null)                
                    _updateNews = updateNews.InnerText;

                if (preventPackDownload != null)
                    _preventPackDownload = bool.Parse(preventPackDownload.InnerText);

                if (credits != null)
                    _credits = credits.InnerText;

                if (installVersion != null)
                {
                    if (installVersion.InnerText != "")
                        _installVersion = installVersion.InnerText;
                    else
                        _installVersion = "1_4_7/minecraft.jar"; //defaulting to version 1.4.7
                }

                if (Excludes != null)
                {
                    foreach (XmlNode exclude in Excludes)
                        _excludeFromUpdate.Add(exclude.InnerText);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("xmlVersionInfo() failed.{0}Error: {1}", Environment.NewLine, ex.Message), ex.InnerException);
            }
        }


    }
}
