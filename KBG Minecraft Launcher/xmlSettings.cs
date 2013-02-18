using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography;

namespace KBG_Minecraft_Launcher
{
    public class PackClass
    {
        private string _packName = "";
        public string Name
        {
            get { return _packName; }
            set { _packName = value; }
        }

        //private double _packVersion = 0;
        //public double Version
        //{
        //    get { return _packVersion; }
        //    set { _packVersion = value; }
        //}

        private string _packDownloadUrl = "";
        public string DownloadUrl
        {
            get { return _packDownloadUrl; }
            set { _packDownloadUrl = value; }
        }

        private string _packVersionUrl = "";
        public string VersionUrl
        {
            get { return _packVersionUrl; }
            set { _packVersionUrl = value; }
        }

        private string _packVersionFileName = "";
        public string VersionFileName
        {
            get { return _packVersionFileName; }
            set { _packVersionFileName = value; }
        }

        public PackClass() { }

        public PackClass(string packName)
        {
            _packName = packName;
            //_packVersion = 0;
            _packDownloadUrl = "";
            _packVersionUrl = "";            
            _packVersionFileName = "";
        }

        public PackClass(string packName, double packVersion, string packDownloadUrl, string packVersionUrl)
        {

            _packName = packName;
            //_packVersion = packVersion;
            _packDownloadUrl = packDownloadUrl;
            _packVersionUrl = packVersionUrl;
            if(packVersionUrl.Contains("/"))
                _packVersionFileName = packVersionUrl.Substring(packVersionUrl.LastIndexOf("/")+1, packVersionUrl.Length - packVersionUrl.LastIndexOf("/") - 1);            
        }

        public PackClass(string packName, double packVersion, string packDownloadUrl, string packVersionUrl, string packVersionFileName)
        {

            _packName = packName;
            //_packVersion = packVersion;
            _packDownloadUrl = packDownloadUrl;
            _packVersionUrl = packVersionUrl;
            _packVersionFileName = packVersionFileName;
        }
    }


    public class xmlSettings
    {
        //settings
        //username
        //passsword
        //remember me
        //memmory Min
        //memmory Max
        //chosen java version
        //auto login bool
        //auto login server
        //previous server

        //packs
        //  Name
        //  version
        //  packUrl
        //  versionUrl
        //  pack name
        //private string _settingsFileName = "Settings.xml";
        private string _settingsFile = "";

        private const string SettingNameUsername = "UserName";
        private const string SettingNamePassword = "Password";
        private const string SettingNameRememberLogin = "RememberLogin";
        private const string SettingNameMemmoryMin = "MemmoryMin";
        private const string SettingNameMemmoryMax = "MemmoryMax";
        private const string SettingNameJavaVersion = "JavaVersion";
        private const string SettingNameDoAutoLogin = "DoAutoLogin";
        private const string SettingNameAutoLoginServer = "AutoLoginServer";
        private const string SettingNameLastPlayedServer = "LastPlayedServer";


        //private string _username;
        //private string _password;
        //private bool _rememberLogin;
        //private int _memmoryMin;
        //private int _memmoryMax;
        //private string _javaVersion;
        //private bool _doAutoLogin;
        //private string _autoLoginServer;
        //private string _lastPlayedServer;

        //List<PackClass> _packList = new List<PackClass>();


        public string Username
        {
            get{ return LoadSettingByName(SettingNameUsername);}
            set { SaveSettingByName(SettingNameUsername, value); }
        }

        public string Password
        {
            //get { return LoadSettingByName(SettingNamePassword); }
            get 
            {
                string returnString = LoadSettingByName(SettingNamePassword);
                if(returnString != "")
                    return Decrypt(returnString, "SuperHaxMaxSecurePassword");
                return returnString;
            }
            //set { SaveSettingByName(SettingNamePassword, value); }
            set 
            {
                if(value == "")
                    SaveSettingByName(SettingNamePassword, ""); 
                else
                    SaveSettingByName(SettingNamePassword, Encrypt(value, "SuperHaxMaxSecurePassword")); 
            } 
        }

        public bool RememberLogin
        {
            get 
            {
                bool b = false;
                bool.TryParse(LoadSettingByName(SettingNameRememberLogin),out b); 
                return b;
            }
            set { SaveSettingByName(SettingNameRememberLogin, value.ToString()); }
        }

        public int MemmoryMin
        {
            get 
            {
                int i = 512;
                int.TryParse(LoadSettingByName(SettingNameMemmoryMin), out i);
                return i;
            }
            set
            {
                int i = 512;
                int.TryParse(value.ToString(), out i);                
                SaveSettingByName(SettingNameMemmoryMin, i.ToString()); 
            }
        }

        public int MemmoryMax
        {
            get 
            {
                int i = 512;
                int.TryParse(LoadSettingByName(SettingNameMemmoryMax), out i);
                return i; 
            }
            set 
            {
                int i = 512;
                int.TryParse(value.ToString(), out i);
                SaveSettingByName(SettingNameMemmoryMax, i.ToString()); 
            }
        }

        public string JavaVersion
        {
            get { return LoadSettingByName(SettingNameJavaVersion); }
            set { SaveSettingByName(SettingNameJavaVersion, value); }
        }

        public bool DoAutoLogin
        {
            get 
            {
                bool b = false;
                bool.TryParse(LoadSettingByName(SettingNameDoAutoLogin), out b);
                return b; 
            }
            set 
            {
                bool b = false;
                bool.TryParse(value.ToString(), out b);
                SaveSettingByName(SettingNameDoAutoLogin, b.ToString()); 
            }
        }

        public string AutoLoginServer
        {
            get { return LoadSettingByName(SettingNameAutoLoginServer); }
            set { SaveSettingByName(SettingNameAutoLoginServer, value); }
        }

        public string LastPlayedServer
        {
            get { return LoadSettingByName(SettingNameLastPlayedServer); }
            set { SaveSettingByName(SettingNameLastPlayedServer, value); }
        }

        //public List<PackClass> PackList
        //{
        //    get { return _packList; }
        //}

        public xmlSettings()
        {
            _settingsFile = Environment.CurrentDirectory + @"\Settings.xml";
            if (!File.Exists(_settingsFile))
                CreateNewSettingsFile();
            LoadPacks();
        }

        ///// <summary>
        ///// Saves information about a Minecraft Pack. also used to create new.
        ///// </summary>
        ///// <param name="newPack"></param>
        //public void SavePackInfo(PackClass newPack)
        //{
        //    XmlDocument xmlDoc = new XmlDocument();
        //    XmlNode PackNodeList;
        //    XmlNode PackNode = null;
        //    XmlNode nodeName;
        //    XmlNode nodeVersion;
        //    XmlNode nodePackUrl;
        //    XmlNode nodeVersionUrl;
                        
        //    //packs
        //    //  Name
        //    //  version
        //    //  packUrl
        //    //  versionUrl
            
        //    try
        //    {
        //        if (!XmlFileContainsPack(newPack.Name))
        //        {
        //            //add new pack

        //            EnsureXMLIntegrity();
        //            xmlDoc.Load(_settingsFile);

        //            PackNodeList = xmlDoc.SelectSingleNode("KBGSettings/Packs");

        //            foreach (XmlNode pack in PackNodeList.ChildNodes)
        //            {
        //                if (pack.SelectSingleNode("Name").InnerText == newPack.Name)
        //                {
        //                    PackNode = pack;
        //                    break;
        //                }
        //            }
        //            if (PackNode != null)
        //                throw new Exception("The pack info your trying to add was not in the loaded Packlist, but in the xml file. This inconsistancy should not happen unless the xml have been manually edited" + Environment.NewLine + "@add new pack");

        //            PackNode = xmlDoc.CreateElement("Pack");

        //            nodeName = xmlDoc.CreateElement("Name");
        //            nodeName.InnerText = newPack.Name;
        //            PackNode.AppendChild(nodeName);

        //            nodeVersion = xmlDoc.CreateElement("Version");
        //            nodeVersion.InnerText = newPack.Version.ToString();
        //            PackNode.AppendChild(nodeVersion);

        //            nodePackUrl = xmlDoc.CreateElement("PackUrl");
        //            nodePackUrl.InnerText = newPack.DownloadUrl;
        //            PackNode.AppendChild(nodePackUrl);

        //            nodeVersionUrl = xmlDoc.CreateElement("VersionUrl");
        //            nodeVersionUrl.InnerText = newPack.VersionUrl;
        //            PackNode.AppendChild(nodeVersionUrl);

        //            PackNodeList.AppendChild(PackNode);
        //            using (FileStream fsxml = new FileStream(_settingsFile, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite))
        //            {
        //                xmlDoc.Save(fsxml);
        //            }

        //            //_packList.Add(newPack);
        //        }
        //        else
        //        {
        //            //override existing pack                    
                    
        //            EnsureXMLIntegrity();
        //            xmlDoc.Load(_settingsFile);

        //            PackNodeList = xmlDoc.SelectSingleNode("KBGSettings/Packs");

        //            foreach (XmlNode pack in PackNodeList.ChildNodes)
        //            {
        //                if (pack.SelectSingleNode("Name").InnerText == newPack.Name)
        //                {
        //                    PackNode = pack;
        //                    break;
        //                }
        //            }
        //            if (PackNode == null)
        //                throw new Exception("The pack info your trying to add was in the loaded Packlist, but not in the xml file. This inconsistancy should not happen unless the xml have been manually edited" + Environment.NewLine + "@override existing pack");

        //            nodeName = PackNode.SelectSingleNode("Name");
        //            nodeName.InnerText = newPack.Name;
        //            PackNode.ReplaceChild(nodeName, nodeName);

        //            nodeVersion = PackNode.SelectSingleNode("Version");
        //            nodeVersion.InnerText = newPack.Version.ToString();
        //            PackNode.ReplaceChild(nodeVersion, nodeVersion);

        //            nodePackUrl = PackNode.SelectSingleNode("PackUrl");
        //            nodePackUrl.InnerText = newPack.DownloadUrl;
        //            PackNode.ReplaceChild(nodePackUrl, nodePackUrl);

        //            nodeVersionUrl = PackNode.SelectSingleNode("VersionUrl");
        //            nodeVersionUrl.InnerText = newPack.VersionUrl;
        //            PackNode.ReplaceChild(nodeVersionUrl, nodeVersionUrl);


        //            PackNodeList.ReplaceChild(PackNode, PackNode); //(newNode, newNode) sounds wrong, but does it work?
        //            //xmlDoc.SelectSingleNode("KBGSettings/Packs").ReplaceChild(PackNode, PackNode); //(newNode, newNode) sounds wrong, but does it work?

        //            using (FileStream fsxml = new FileStream(_settingsFile, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite))
        //            {
        //                xmlDoc.Save(fsxml);
        //            }

        //            //foreach (PackClass pack in _packList)
        //            //{
        //            //    if (pack.Name == newPack.Name)
        //            //    {
        //            //        _packList.Remove(pack);
        //            //        _packList.Add(newPack);
        //            //        break;
        //            //    }
        //            //}
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message + Environment.NewLine + "@xmlSettings.SavePackInfo()");
        //    }
        //    finally
        //    {
        //        System.GC.Collect();
        //    }
        //}

        public void DeletePackInfo(PackClass pack)
        {
            DeletePackInfo(pack.Name);
        }

        public void DeletePackInfo(string packName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode PackNodeList;
            XmlNode PackNode = null;
            try
            {
                EnsureXMLIntegrity();
                xmlDoc.Load(_settingsFile);

                PackNodeList = xmlDoc.SelectSingleNode("KBGSettings/Packs");

                foreach (XmlNode pack in PackNodeList.ChildNodes)
                {
                    if (pack.SelectSingleNode("Name").InnerText == packName)
                    {
                        PackNode = pack;
                        break;
                    }
                }
                if (PackNode == null)
                    throw new Exception("The pack info your trying to delete was in the loaded Packlist, but not in the xml file. This inconsistancy should not happen unless the xml have been manually edited" + Environment.NewLine + "@Delete Pack");

                PackNodeList.RemoveChild(PackNode);

                using (FileStream fsxml = new FileStream(_settingsFile, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite))
                {
                    xmlDoc.Save(fsxml);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + Environment.NewLine + "@xmlSettings.DeletePackInfo()");
            }
        }

        public List<PackClass> LoadPacks()
        {
            
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode PackNodeList;
            List<PackClass> PackList = new List<PackClass>();
            //XmlNode PackNode = null;

            try
            {
                //_packList.Clear();             
                EnsureXMLIntegrity();
                xmlDoc.Load(_settingsFile);

                PackNodeList = xmlDoc.SelectSingleNode("KBGSettings/Packs");

                foreach (XmlNode pack in PackNodeList.ChildNodes)
                {
                    double tmpDouble = 0;
                    double.TryParse(pack.SelectSingleNode("Version").InnerText,System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out tmpDouble);

                    PackList.Add(new PackClass(
                        pack.SelectSingleNode("Name").InnerText,
                        tmpDouble,
                        pack.SelectSingleNode("PackUrl").InnerText,
                        pack.SelectSingleNode("VersionUrl").InnerText));
                }
                return PackList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + Environment.NewLine + "@xmlSettings.LoadPacks()");
            }

        }

        //private void loadSettings()
        //{
        //}

        private bool XmlFileContainsPack(string packName)
        {
            bool returnValue = false;
            List<PackClass> PackList = LoadPacks();

            foreach (PackClass pack in PackList)
                if (pack.Name == packName)
                {
                    returnValue = true;
                    break;
                }
            return returnValue;
        }

        private void CreateNewSettingsFile()
        {
            try
            {
                //<!-- Students grades are updated bi-monthly -->
                XmlDocument xmlDoc = new XmlDocument();
                string textContent = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + Environment.NewLine;
                textContent += @"<!-- Mess with this file at your own risk!!! -->" + Environment.NewLine;
                textContent += @"<KBGSettings>" + Environment.NewLine;
                textContent += @"<Settings>" + Environment.NewLine;
                textContent += string.Format("<{0}>{1}</{0}>{2}", SettingNameUsername, "",Environment.NewLine); //The "" here is for default values
                textContent += string.Format("<{0}>{1}</{0}>{2}", SettingNamePassword, "", Environment.NewLine);
                textContent += string.Format("<{0}>{1}</{0}>{2}", SettingNameRememberLogin, false.ToString(), Environment.NewLine);
                textContent += string.Format("<{0}>{1}</{0}>{2}", SettingNameMemmoryMin, "512", Environment.NewLine);
                textContent += string.Format("<{0}>{1}</{0}>{2}", SettingNameMemmoryMax, "512", Environment.NewLine);
                textContent += string.Format("<{0}>{1}</{0}>{2}", SettingNameJavaVersion, "", Environment.NewLine);
                textContent += string.Format("<{0}>{1}</{0}>{2}", SettingNameDoAutoLogin, "false", Environment.NewLine);
                textContent += string.Format("<{0}>{1}</{0}>{2}", SettingNameAutoLoginServer, "", Environment.NewLine);
                textContent += string.Format("<{0}>{1}</{0}>{2}", SettingNameLastPlayedServer, "", Environment.NewLine); 
                textContent += @"</Settings>"+ Environment.NewLine;
                textContent += @"<Packs></Packs>";
                textContent += @"</KBGSettings>" + Environment.NewLine;

                xmlDoc.LoadXml(textContent);

                using (FileStream fsxml = new FileStream(_settingsFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    xmlDoc.Save(fsxml);
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                System.GC.Collect();
            }
        }

        private void EnsureXMLIntegrity()
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                if(!File.Exists(_settingsFile))
                    CreateNewSettingsFile();

                xmlDoc.Load(_settingsFile);
                if (xmlDoc.SelectSingleNode("KBGSettings") == null)
                {
                    File.Delete(_settingsFile);
                    CreateNewSettingsFile();
                }

                if (xmlDoc.SelectSingleNode("KBGSettings/Settings") == null)
                    xmlDoc.SelectSingleNode("KBGSettings").AppendChild(xmlDoc.CreateElement("Settings"));

                if (xmlDoc.SelectSingleNode("KBGSettings/Packs") == null)
                    xmlDoc.SelectSingleNode("KBGSettings").AppendChild(xmlDoc.CreateElement("Packs"));
            }
            catch (System.Xml.XmlException ex)
            {
                File.Delete(_settingsFile);
                CreateNewSettingsFile();
                throw new Exception("Please try not to mess up the XML when you manually edit it. Thanks!" + Environment.NewLine + ex.Message + Environment.NewLine + "@xmlSettings.EnsureXMLIntegrity()");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + Environment.NewLine + "@xmlSettings.EnsureXMLIntegrity()");
            }
        }

        private string LoadSettingByName(string settingName)
        {
            string returnString = "";
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                EnsureXMLIntegrity();
                xmlDoc.Load(_settingsFile);
                if (xmlDoc.SelectSingleNode("KBGSettings/Settings/" + settingName) != null)
                    returnString = xmlDoc.SelectSingleNode("KBGSettings/Settings/" + settingName).InnerText;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + Environment.NewLine + "@xmlSettings.LoadValueFromName()");
            }
            return returnString;
        }

        private void SaveSettingByName(string settingsName, string value)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode newNode;

            try
            {
                EnsureXMLIntegrity();
                xmlDoc.Load(_settingsFile);

                if(xmlDoc.SelectSingleNode("KBGSettings/Settings/" + settingsName) == null)
                    xmlDoc.SelectSingleNode("KBGSettings/Settings").AppendChild(xmlDoc.CreateElement(settingsName));
            
                newNode = xmlDoc.SelectSingleNode("KBGSettings/Settings/" + settingsName);
                
                //newNode.InnerText = System.Security.SecurityElement.Escape(value); //use this if someone gets problems with characters " < > ' or &
                newNode.InnerText = value;
                xmlDoc.SelectSingleNode("KBGSettings/Settings").ReplaceChild(newNode, newNode); //(newNode, newNode) sounds wrong, but seems to work!

                using (FileStream fsxml = new FileStream(_settingsFile, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite))
                {
                    xmlDoc.Save(fsxml);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + Environment.NewLine + "@xmlSettings.SaveSettingByName()");
            }
            finally
            {
                System.GC.Collect();
            }
        }

        private static byte[] Encrypt(byte[] clearText, byte[] Key, byte[] IV)
        {
            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();
            alg.Key = Key;
            alg.IV = IV;
            CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(clearText, 0, clearText.Length);
            cs.Close();
            byte[] encryptedData = ms.ToArray();
            return encryptedData;
        }

        public string Encrypt(string clearText, string Password)
        {
            byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(clearText);
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            byte[] encryptedData = Encrypt(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16));
            return Convert.ToBase64String(encryptedData);
        }

        private static byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV)
        {
            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();
            alg.Key = Key;
            alg.IV = IV;
            CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(cipherData, 0, cipherData.Length);
            cs.Close();
            byte[] decryptedData = ms.ToArray();
            return decryptedData;
        }

        public string Decrypt(string cipherText, string Password)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            byte[] decryptedData = Decrypt(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));
            return System.Text.Encoding.Unicode.GetString(decryptedData);
        }


        //static byte[] EncryptString(string plainText, byte[] Key, byte[] IV)
        //{
        //    // Check arguments. 
        //    if (plainText == null || plainText.Length <= 0)
        //        throw new ArgumentNullException("plainText");
        //    if (Key == null || Key.Length <= 0)
        //        throw new ArgumentNullException("Key");
        //    if (IV == null || IV.Length <= 0)
        //        throw new ArgumentNullException("Key");
        //    byte[] encrypted;
        //    // Create an RijndaelManaged object 
        //    // with the specified key and IV. 
        //    using (RijndaelManaged rijAlg = new RijndaelManaged())
        //    {
        //        rijAlg.Key = Key;
        //        rijAlg.IV = IV;

        //        // Create a decrytor to perform the stream transform.
        //        ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);
                                
        //        // Create the streams used for encryption. 
        //        using (MemoryStream msEncrypt = new MemoryStream())
        //        {
        //            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
        //            {
        //                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
        //                {

        //                    //Write all data to the stream.
        //                    swEncrypt.Write(plainText);
        //                }
        //                encrypted = msEncrypt.ToArray();                        
        //            }
        //            var sw = new StreamWriter(msEncrypt);
        //            //return sw.
        //        }
        //    }


        //    // Return the encrypted bytes from the memory stream. 
        //    return encrypted;

        //}

        //static string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
        //{
        //    // Check arguments. 
        //    if (cipherText == null || cipherText.Length <= 0)
        //        throw new ArgumentNullException("cipherText");
        //    if (Key == null || Key.Length <= 0)
        //        throw new ArgumentNullException("Key");
        //    if (IV == null || IV.Length <= 0)
        //        throw new ArgumentNullException("Key");

        //    // Declare the string used to hold 
        //    // the decrypted text.
        //    string plaintext = null;

        //    // Create an RijndaelManaged object 
        //    // with the specified key and IV. 
        //    using (RijndaelManaged rijAlg = new RijndaelManaged())
        //    {
        //        rijAlg.Key = Key;
        //        rijAlg.IV = IV;

        //        // Create a decrytor to perform the stream transform.
        //        ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

        //        // Create the streams used for decryption. 
        //        using (MemoryStream msDecrypt = new MemoryStream(cipherText))
        //        {
        //            using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
        //            {
        //                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
        //                {

        //                    // Read the decrypted bytes from the decrypting stream 
        //                    // and place them in a string.
        //                    plaintext = srDecrypt.ReadToEnd();
        //                }
        //            }
        //        }

        //    }

        //    return plaintext;

        //}


        //public static void Encrypt(XmlDocument Doc, string ElementName, SymmetricAlgorithm Key)
        //{
        //    // Check the arguments.   
        //    if (Doc == null)
        //        throw new ArgumentNullException("Doc");
        //    if (ElementName == null)
        //        throw new ArgumentNullException("ElementToEncrypt");
        //    if (Key == null)
        //        throw new ArgumentNullException("Alg");

        //    //////////////////////////////////////////////// 
        //    // Find the specified element in the XmlDocument 
        //    // object and create a new XmlElemnt object. 
        //    ////////////////////////////////////////////////
        //    XmlElement elementToEncrypt = Doc.GetElementsByTagName(ElementName)[0] as XmlElement;
        //    // Throw an XmlException if the element was not found. 
        //    if (elementToEncrypt == null)
        //    {
        //        throw new XmlException("The specified element was not found");

        //    }

        //    ////////////////////////////////////////////////// 
        //    // Create a new instance of the EncryptedXml class  
        //    // and use it to encrypt the XmlElement with the  
        //    // symmetric key. 
        //    //////////////////////////////////////////////////

        //    EncryptedXml eXml = new EncryptedXml();

        //    byte[] encryptedElement = eXml.EncryptData(elementToEncrypt, Key, false);
        //    //////////////////////////////////////////////// 
        //    // Construct an EncryptedData object and populate 
        //    // it with the desired encryption information. 
        //    ////////////////////////////////////////////////

        //    EncryptedData edElement = new EncryptedData();
        //    edElement.Type = EncryptedXml.XmlEncElementUrl;

        //    // Create an EncryptionMethod element so that the  
        //    // receiver knows which algorithm to use for decryption. 
        //    // Determine what kind of algorithm is being used and 
        //    // supply the appropriate URL to the EncryptionMethod element. 

        //    string encryptionMethod = null;

        //    if (Key is TripleDES)
        //    {
        //        encryptionMethod = EncryptedXml.XmlEncTripleDESUrl;
        //    }
        //    else if (Key is DES)
        //    {
        //        encryptionMethod = EncryptedXml.XmlEncDESUrl;
        //    }
        //    if (Key is Rijndael)
        //    {
        //        switch (Key.KeySize)
        //        {
        //            case 128:
        //                encryptionMethod = EncryptedXml.XmlEncAES128Url;
        //                break;
        //            case 192:
        //                encryptionMethod = EncryptedXml.XmlEncAES192Url;
        //                break;
        //            case 256:
        //                encryptionMethod = EncryptedXml.XmlEncAES256Url;
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        // Throw an exception if the transform is not in the previous categories 
        //        throw new CryptographicException("The specified algorithm is not supported for XML Encryption.");
        //    }

        //    edElement.EncryptionMethod = new EncryptionMethod(encryptionMethod);

        //    // Add the encrypted element data to the  
        //    // EncryptedData object.
        //    edElement.CipherData.CipherValue = encryptedElement;

        //    //////////////////////////////////////////////////// 
        //    // Replace the element from the original XmlDocument 
        //    // object with the EncryptedData element. 
        //    ////////////////////////////////////////////////////
        //    EncryptedXml.ReplaceElement(elementToEncrypt, edElement, false);
        //}

        //public static void Decrypt(XmlDocument Doc, SymmetricAlgorithm Alg)
        //{
        //    // Check the arguments.   
        //    if (Doc == null)
        //        throw new ArgumentNullException("Doc");
        //    if (Alg == null)
        //        throw new ArgumentNullException("Alg");

        //    // Find the EncryptedData element in the XmlDocument.
        //    XmlElement encryptedElement = Doc.GetElementsByTagName("EncryptedData")[0] as XmlElement;

        //    // If the EncryptedData element was not found, throw an exception. 
        //    if (encryptedElement == null)
        //    {
        //        throw new XmlException("The EncryptedData element was not found.");
        //    }


        //    // Create an EncryptedData object and populate it.
        //    EncryptedData edElement = new EncryptedData();
        //    edElement.LoadXml(encryptedElement);

        //    // Create a new EncryptedXml object.
        //    EncryptedXml exml = new EncryptedXml();


        //    // Decrypt the element using the symmetric key. 
        //    byte[] rgbOutput = exml.DecryptData(edElement, Alg);

        //    // Replace the encryptedData element with the plaintext XML element.
        //    exml.ReplaceData(encryptedElement, rgbOutput);

        //}
    }
}
