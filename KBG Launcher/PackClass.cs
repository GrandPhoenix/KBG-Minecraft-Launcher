using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace KBG_Launcher
{
    //public class PackClass : ConfigurationElement
    //{
    //    //this["key"]
    //    private string _packName = "";
    //    //[ConfigurationProperty("Name", DefaultValue = "", IsRequired = true, IsKey = true)]
    //    public string Name
    //    {
    //        get { return _packName; }
    //        set { _packName = value; }
    //    }

    //    private double _packVersion = 0;
    //    //[ConfigurationProperty("Version", IsRequired = true)]
    //    public double Version
    //    {
    //        get { return _packVersion; }
    //        set { _packVersion = value; }
    //    }

    //    private string _packDownloadUrl = "";
    //    //[ConfigurationProperty("DownloadUrl", IsRequired = true)]
    //    public string DownloadUrl
    //    {
    //        get { return _packDownloadUrl; }
    //        set { _packDownloadUrl = value; }
    //    }

    //    private string _packVersionUrl = "";
    //    //[ConfigurationProperty("VersionUrl", IsRequired = true)]
    //    public string VersionUrl
    //    {
    //        get { return _packVersionUrl; }
    //        set { _packVersionUrl = value; }
    //    }

    //    private string _packVersionFileName = "";
    //    //[ConfigurationProperty("VersionFileName", IsRequired = true)]
    //    public string VersionFileName
    //    {
    //        get { return _packVersionFileName; }
    //        set { _packVersionFileName = value; }
    //    }

    //    public PackClass() { }

    //    public PackClass(string packName, double packVersion, string packDownloadUrl, string packVersionUrl, string packVersionFileName)
    //    {

    //        _packName = packName;
    //        _packVersion = packVersion;
    //        _packDownloadUrl = packDownloadUrl;
    //        _packVersionUrl = packVersionUrl;
    //        _packVersionFileName = packVersionFileName;
    //    }
    //}

    public class PackConfigurationClass : ConfigurationElement
    {
        //this["key"]
        // private string _packName = "";
        [ConfigurationProperty("name", DefaultValue = "", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return this["name"] as string; }
            set { this["name"] = value; }
        }

        //private double _packVersion = 0;
        [ConfigurationProperty("version", IsRequired = true)]
        public double Version
        {
            get { return double.Parse(this["version"] as string, System.Globalization.NumberFormatInfo.InvariantInfo); }
            set { this["version"] = value; }
        }

        //private string _packDownloadUrl = "";
        [ConfigurationProperty("downloadUrl", IsRequired = true)]
        public string DownloadUrl
        {
            get { return this["downloadUrl"] as string; }
            set { this["downloadUrl"] = value; }
        }

        //private string _packVersionUrl = "";
        [ConfigurationProperty("versionUrl", IsRequired = true)]
        public string VersionUrl
        {
            get { return this["versionUrl"] as string; }
            set { this["versionUrl"] = value; }
        }

        //private string _packVersionFileName = "";
        [ConfigurationProperty("versionFileName", IsRequired = true)]
        public string VersionFileName
        {
            get { return this["versionFileName"] as string; }
            set { this["versionFileName"] = value; }
        }

        //public PackClass() { }

        //public PackClass(string packName, double packVersion, string packDownloadUrl, string packVersionUrl, string packVersionFileName)
        //{

        //    _packName = packName;
        //    _packVersion = packVersion;
        //    _packDownloadUrl = packDownloadUrl;
        //    _packVersionUrl = packVersionUrl;
        //    _packVersionFileName = packVersionFileName;
        //}
    }

    public class PackConfigurationClassCollection : ConfigurationElementCollection
    {
        public PackConfigurationClassCollection()
        {
            Console.WriteLine("PackConfigurationClassCollection Constructor");
        }


       


        public PackConfigurationClass this[int index]
        {
            get { return (PackConfigurationClass)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public void Add(PackConfigurationClass serviceConfig)
        {
            BaseAdd(serviceConfig);
        }

        public void Clear()
        {
            BaseClear();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new PackConfigurationClass();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PackConfigurationClass)element).Name;
        }


        
        public new string AddElementName
        {
            get
            { return base.AddElementName; }

            set
            { base.AddElementName = value; }

        }


        public void Remove(PackConfigurationClass packClass)
        {
            BaseRemove(packClass.Name);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }
    }

    public class PackConfigurationSection : ConfigurationSection
    {
        //"PackSection"
        private static string sConfigurationSectionConst = "PackSection";
        private static System.Configuration.Configuration config;

        [ConfigurationProperty("Packs", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(PackConfigurationClassCollection))]
        public PackConfigurationClassCollection Packs
        {
            get
            {
                return (PackConfigurationClassCollection)base["Packs"] ?? new PackConfigurationClassCollection();
            }
            //Do i need the set ?
            //set { base["Packs"] = value; }  
        }

        public void Loadconfig()
        {

        }

        public static PackConfigurationSection GetConfig()
        {
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            return (PackConfigurationSection)ConfigurationManager.GetSection(PackConfigurationSection.sConfigurationSectionConst) ?? new PackConfigurationSection();
        }

        public void Save()
        {
            if (config != null)
                config.Save();
        }

    }

    //to use:
    //PackConfigurationSection packConfigSection = ConfigurationManager.GetSection("PackSection") as PackConfigurationSection;
    //PackClass packClass = packConfigSection.Packs[0];
}
