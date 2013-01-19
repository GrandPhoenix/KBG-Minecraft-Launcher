using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace KBG_Minecraft_Launcher
{
    public class xmlVersionInfo
    {
        private int _versionMajor = 0;
        private int _versionMinor = 0;
        private int _versionRevision = 0;
        private int _versionPack = 0;
        private List<string> _excludeFromUpdate = new List<string>();

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

        private string _updateNews;

        public string UpdateNews
        {
            get { return _updateNews; }
            set { _updateNews = value; }
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
                    xmlDoc.Load(url);
                }
                versionMajor = xmlDoc.SelectSingleNode("KBGVersionInfo/VersionMajor");
                versionMinor = xmlDoc.SelectSingleNode("KBGVersionInfo/VersionMinor");
                versionRevision = xmlDoc.SelectSingleNode("KBGVersionInfo/VersionRevision");
                versionPack = xmlDoc.SelectSingleNode("KBGVersionInfo/VersionPack");
                updateNews = xmlDoc.SelectSingleNode("KBGVersionInfo/News");
                Excludes = xmlDoc.SelectNodes("KBGVersionInfo/ExcludeFromUpdate");

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

                if (Excludes != null)
                {
                    foreach (XmlNode exclude in Excludes)
                        _excludeFromUpdate.Add(exclude.InnerText);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("xmlVersionInfo() failed.{0}Url: {1}{0}Error: {2}", Environment.NewLine, url, ex.Message), ex.InnerException);
            }
        }


    }
}
