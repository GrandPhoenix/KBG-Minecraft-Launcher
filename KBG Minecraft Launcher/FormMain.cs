using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.Net;
using System.Xml.Linq;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Ionic.Zip;
using System.Configuration;
using Microsoft.Win32;
using System.Security.Cryptography;
//using System.Security.Cryptography;
using System.Xml;
using System.Security.Cryptography.Xml;
using System.Collections;

namespace KBG_Minecraft_Launcher
{
    public partial class FormMain : Form
    {
        public enum SupportetAutoUpdatePack { IR = 0, ER, TFCR, Vanilla };        

        private List<TweetItem> _TweetList = new List<TweetItem>();
        private FormOptions _formOptions;
        private FormNews _formNews;
        private FormError _formError = new FormError();

        private bool _updateFinished = false;
        
        private string _packDir;
        private bool _abortDownload = false;
        private bool _loadingSettings = false;
        public bool CloseAllThreads = false;
        //public enum SourcePack {IR=0,ER,TFCR,Vanilla};

        Thread TweetThread;
        Thread pingIRThread;
        Thread pingERThread;
        Thread pingMiningThread;
        Thread pingTFCRThread;
        Thread pingMinecraftDotNetThread;
        Thread pingMinecraftLoginServersThread;
        Thread packThread;
        Thread updateThread;

        [DllImport("wininet", CharSet = CharSet.Auto)]
	    static extern bool InternetGetConnectedState(ref ConnectionStatusEnum flags, int dw);

        enum ConnectionStatusEnum : int
        {
            INTERNET_CONNECTION_MODEM = 0x1,
            INTERNET_CONNECTION_LAN = 0x2,
            INTERNET_CONNECTION_PROXY = 0x4,
            INTERNET_RAS_INSTALLED = 0x10,
            INTERNET_CONNECTION_OFFLINE = 0x20,
            INTERNET_CONNECTION_CONFIGURED = 0x40
        }

        //This is used to embed the Ionic.xip.dll into the project exe file, for easier deployment
        //see http://dotnetzip.codeplex.com/wikipage?title=Embed%20DotNetZip for more details
        static System.Reflection.Assembly ResolverDotNetZip(object sender, ResolveEventArgs args)
        {
            Assembly a1 = Assembly.GetExecutingAssembly();
            Stream s = a1.GetManifestResourceStream(typeof(FormMain), "Ionic.Zip.dll");
            byte[] block = new byte[s.Length];
            s.Read(block, 0, block.Length);
            Assembly a2 = Assembly.Load(block);
            return a2;
        }
        
        //constructor

        public FormMain()
        {
            string methodProgress = ""; //this is used to find out where in the method the error occurred

            //If startet as KBG minecraft launcher2.exe, then delete the first exe, copy second to first, and restart the first again
            try
            {                
                if (getCommandLineArgs().Contains("/UpdateRestart"))
                {
                    if (Application.ExecutablePath == Application.StartupPath + "\\KBG Minecraft Launcher2.exe")
                    {
                        //should be startet as KBG Minecraft Launcher2.exe
                        methodProgress = "UpdateRestart - start";
                        
                        try
                        {

                            // get old process and wait UP TO 5 secs then give up!
                            Process oldProcess = Process.GetProcessById(GetCommandLineArgsValue("/UpdateRestart"));
                            oldProcess.WaitForExit(10000);
                            
                        }
                        catch (Exception ex)
                        {
                            // the process did not exist - probably already closed!
                            //TODO: --> LOG
                            methodProgress = "UpdateRestart oldProcess exception: " + ex.Message;
                        }

                        if (File.Exists(Application.StartupPath + "\\KBG Minecraft Launcher2.exe"))
                        {
                            methodProgress = "UpdateRestart - preparing to copy";                            
                            
                            File.Copy(Application.StartupPath + "\\KBG Minecraft Launcher2.exe", Application.StartupPath + "\\KBG Minecraft Launcher.exe", true);

                            methodProgress = "UpdateRestart - Application.Exiting";
                            
                            Application.Exit();
                            methodProgress = "UpdateRestart - starting \\KBG Minecraft Launcher.exe..";
                            Process.Start(Application.StartupPath + "\\KBG Minecraft Launcher.exe", "/UpdateFinished " + Process.GetCurrentProcess().Id.ToString());
                            methodProgress = "UpdateRestart - Environment.Exit";
                            Environment.Exit(0);
                            methodProgress = "UpdateRestart - GC.Collect";
                            GC.Collect();
                        }
                        methodProgress = "UpdateRestart - end";
                        
                    }
                }

                if (getCommandLineArgs().Contains("/UpdateFinished"))
                {
                    methodProgress = "UpdateFinished - start";
                    
                    try
                    {
                        Process oldProcess = Process.GetProcessById(GetCommandLineArgsValue("/UpdateFinished"));
                        oldProcess.WaitForExit(10000);                        
                    }
                    catch (Exception ex)
                    {
                        methodProgress = "UpdateFinished oldProcess exception: " + ex.Message;
                        // the process did not exist - probably already closed!
                        //TODO: --> LOG
                    }
                    methodProgress = "UpdateFinished - Deleting KBG Minecraft Launcher2.exe";
                    if (File.Exists(Application.StartupPath + "\\KBG Minecraft Launcher2.exe"))
                        File.Delete(Application.StartupPath + "\\KBG Minecraft Launcher2.exe");
                    methodProgress = "UpdateFinished - Update finished";
                    _updateFinished = true;
                    methodProgress = "UpdateFinished - end";
                }
                
            }
            catch (Exception ex)
            {
                //List<string> information = new List<string>();
                //information.Add("CommandLineInformation = " + getCommandLineArgs());
                //information.Add("ExecutablePath = " + Application.ExecutablePath);
                //information.Add("KBG Minecraft Launcher.exe exists? " + File.Exists(Application.StartupPath + "\\KBG Minecraft Launcher.exe").ToString());
                //information.Add("KBG Minecraft Launcher2.exe exists? " + File.Exists(Application.StartupPath + "\\KBG Minecraft Launcher2.exe").ToString());
                //information.Add("Error occured at: " + methodProgress);
                //information.Add("Error: " + ex.Message);
                ex.Data.Add("CommandLineInformation", getCommandLineArgs());
                ex.Data.Add("ExecutablePath", Application.ExecutablePath);
                ex.Data.Add("KBG Minecraft Launcher.exe exists?", File.Exists(Application.StartupPath + "\\KBG Minecraft Launcher.exe").ToString());
                ex.Data.Add("KBG Minecraft Launcher2.exe exists?",File.Exists(Application.StartupPath + "\\KBG Minecraft Launcher2.exe").ToString());

                ErrorReporting(ex, true);
            }

            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(ResolverDotNetZip);            
            InitializeComponent();
        }


        //methods almost directly copy-paste from teh interwebs

        public DateTime ParseDateTime(string date)
        {
            try
            {
                string dayOfWeek = date.Substring(0, 3).Trim();
                string month = date.Substring(4, 3).Trim();
                string dayInMonth = date.Substring(8, 2).Trim();
                string time = date.Substring(11, 9).Trim();
                string offset = date.Substring(20, 5).Trim();
                string year = date.Substring(25, 5).Trim();
                string dateTime = string.Format("{0}-{1}-{2} {3}", dayInMonth, month, year, time);
                DateTime ret = DateTime.Parse(dateTime);
                return ret;
            }
            catch (Exception ex)
            {
                ex.Data.Add("ParseDateTime() info start", "");
                ex.Data.Add("date", date);
                throw ex;
            }
        }

        //Testing for an Internet connection. Thanks to http://www.dreamincode.net/forums/topic/71263-using-the-ping-class-in-c%23/
        private static bool HasConnection()
        {
            try
            {
                //instance of our ConnectionStatusEnum
                ConnectionStatusEnum state = 0;

                //call the API
                InternetGetConnectedState(ref state, 0);

                //check the status, if not offline and the returned state
                //isnt 0 then we have a connection
                if (((int)ConnectionStatusEnum.INTERNET_CONNECTION_OFFLINE & (int)state) != 0)
                {
                    //return true, we have a connection
                    return false;
                }
                //return false, no connection available
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //cole help thanks to http://social.msdn.microsoft.com/forums/en-US/netfxnetcom/thread/ff098248-551c-4da9-8ba5-358a9f8ccc57/
        public static bool SetAllowUnsafeHeaderParsing20()
        {
            try
            {
                //Get the assembly that contains the internal class
                Assembly aNetAssembly = Assembly.GetAssembly(typeof(System.Net.Configuration.SettingsSection));
                if (aNetAssembly != null)
                {
                    //Use the assembly in order to get the internal type for the internal class
                    Type aSettingsType = aNetAssembly.GetType("System.Net.Configuration.SettingsSectionInternal");
                    if (aSettingsType != null)
                    {
                        //Use the internal static property to get an instance of the internal settings class.
                        //If the static instance isn't created allready the property will create it for us.
                        object anInstance = aSettingsType.InvokeMember("Section",
                          BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.NonPublic, null, null, new object[] { });

                        if (anInstance != null)
                        {
                            //Locate the private bool field that tells the framework is unsafe header parsing should be allowed or not
                            FieldInfo aUseUnsafeHeaderParsing = aSettingsType.GetField("useUnsafeHeaderParsing", BindingFlags.NonPublic | BindingFlags.Instance);
                            if (aUseUnsafeHeaderParsing != null)
                            {
                                aUseUnsafeHeaderParsing.SetValue(anInstance, true);
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int GetCommandLineArgsValue(string argName)
        {
            try
            {
                string[] args = Environment.GetCommandLineArgs();
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] == argName)
                    {
                        if (i + 1 < args.Length)
                            return int.Parse(args[i + 1]);
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                ex.Data.Add("GetCommandLineArgsValue() info start","");
                ex.Data.Add("ArgName",argName);
                throw ex;
            }
        }

        static string getCommandLineArgs()
        {
            try
            {
                Queue<string> args = new Queue<string>(Environment.GetCommandLineArgs());

                args.Dequeue(); // args[0] is always exe path/filename
                return string.Join(" ", args.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private String GetJavaInstallationPath()
        {
            try
            {
                String javaKey = "SOFTWARE\\JavaSoft\\Java Runtime Environment";
                using (var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(javaKey))
                {
                    String currentVersion = baseKey.GetValue("CurrentVersion").ToString();
                    using (var homeKey = baseKey.OpenSubKey(currentVersion))
                        return homeKey.GetValue("JavaHome").ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            try
            {
                if (source.FullName.ToLower() == target.FullName.ToLower())
                {
                    return;
                }

                // Check if the target directory exists, if not, create it.
                if (Directory.Exists(target.FullName) == false)
                {
                    Directory.CreateDirectory(target.FullName);
                }

                // Copy each file into it's new directory.
                foreach (FileInfo fi in source.GetFiles())
                {
                    Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                    fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
                }

                // Copy each subdirectory using recursion.
                foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
                {
                    DirectoryInfo nextTargetSubDir =
                        target.CreateSubdirectory(diSourceSubDir.Name);
                    CopyAll(diSourceSubDir, nextTargetSubDir);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("CopyAll() info start", "");
                ex.Data.Add("source", source.FullName);
                ex.Data.Add("target", target.FullName);
                throw ex;
            }
        }

        /// <summary>
        /// Used to generate a session with login.minecraft.net for online use only
        /// </summary>
        /// <param name="username">The player's username</param>
        /// <param name="password">The player's password</param>
        /// <param name="clientVer">The client version (look here http://wiki.vg/Session)</param>
        /// <returns>Returns 5 values split by ":" Current Version : Download Ticket : Username : Session ID : UID</returns>
        public static string generateSession(string username, string password, int clientVer)
        {
            try
            {
                string netResponse = httpGET("https://login.minecraft.net?user=" + username + "&password=" + password + "&version=" + clientVer);
                return netResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string httpGET(string URI)
        {
            try
            {
                WebRequest req = WebRequest.Create(URI);
                WebResponse resp = req.GetResponse();
                StreamReader sr = new StreamReader(resp.GetResponseStream());
                return sr.ReadToEnd().Trim();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //general methods

        private void GenerateTweetList()
        {
            try
            {
                TweetThread = new Thread(new ThreadStart(this.getTwitterFeeds));
                progressBarTwitter.Visible = true;
                TweetThread.Start();
                while (!TweetThread.IsAlive) ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ShowTweets()
        {
            try
            {
                string rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang1033{\\fonttbl{\\f0\\fnil\\fcharset0 Microsoft Sans Serif;}}\r\n{\\colortbl;\\red25\\green133\\blue181;\\red0\\green0\\blue0;}\r\n\\viewkind4\\uc1\\pard";

                for (int i = 0; i < _TweetList.Count; i++)
                {
                    if (i != 0)
                    {
                        rtf += "\\fs17\\par\r\n\\cf2 --------------------------------------------------------------------------------------------- ";
                    }

                    //add tweet
                    rtf += "\\cf1\\f0\\fs18 " + _TweetList[i].Headline;
                    rtf += "\\cf0\\fs17  " + _TweetList[i].Text;
                    rtf += "\\par\r\n\\cf1\\fs14 " + _TweetList[i].Time;
                }

                rtf += "\\cf0\\fs17\\par\r\n}\r\n";

                progressBarTwitter.Visible = false;
                if (_TweetList.Count > 0)
                    richTextBox1.Rtf = rtf;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void StartCheckingServers()
        {
            try
            {
                if (HasConnection())
                {
                    ServerAccessClass pingIR = new ServerAccessClass("ir.industrial-craft.net", 25565, labelIRResult, progressBarIR, this);
                    ServerAccessClass pingER = new ServerAccessClass("209.105.230.53", 25565, labelERResult, progressBarER, this);
                    ServerAccessClass pingMining = new ServerAccessClass("mining.industrial-craft.net", 25565, labelMiningResult, progressBarMining, this);
                    ServerAccessClass pingTFCR = new ServerAccessClass("209.105.230.51", 25565, labelTFCRResult, progressBarTFCR, this);
                    ServerAccessClass pingMinecraftDotNet = new ServerAccessClass("Minecraft.Net", 80, labelMinecraftdotnetResult, progressBarMinecraftdotnet, this);
                    ServerAccessClass pingMinecraftLoginServers = new ServerAccessClass("Login.minecraft.net", 80, labelMinecraftLoginServersResult, progressBarMinecraftLoginServers, this);

                    pingIRThread = new Thread(new ThreadStart(pingIR.StartCheck));
                    pingIRThread.Start();
                    while (!pingIRThread.IsAlive) ;
                    pingERThread = new Thread(new ThreadStart(pingER.StartCheck));
                    pingERThread.Start();
                    while (!pingERThread.IsAlive) ;
                    pingMiningThread = new Thread(new ThreadStart(pingMining.StartCheck));
                    pingMiningThread.Start();
                    while (!pingMiningThread.IsAlive) ;
                    pingTFCRThread = new Thread(new ThreadStart(pingTFCR.StartCheck));
                    pingTFCRThread.Start();
                    while (!pingTFCRThread.IsAlive) ;
                    pingMinecraftDotNetThread = new Thread(new ThreadStart(pingMinecraftDotNet.StartCheck));
                    pingMinecraftDotNetThread.Start();
                    while (!pingMinecraftDotNetThread.IsAlive) ;
                    pingMinecraftLoginServersThread = new Thread(new ThreadStart(pingMinecraftLoginServers.StartCheck));
                    pingMinecraftLoginServersThread.Start();
                    while (!pingMinecraftLoginServersThread.IsAlive) ;
                }
                else
                {
                    MessageBox.Show("Could not find a valid internet connection." + Environment.NewLine + "Please check that you have access to the internet on this machine, and try again", "Server ping skipped: No internet found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void getTwitterFeeds()
        {
            //http://api.twitter.com/1/statuses/user_timeline.xml?screen_name=KB_Gaming
            //string xml = new WebClient().DownloadString("http://api.twitter.com/1/statuses/user_timeline.xml?screen_name=KB_Gaming");
            //var xml = XDocument.Load("http://www.dreamincode.net/forums/xml.php?showuser=1253");

            string url = "http://api.twitter.com/1/statuses/user_timeline.xml?screen_name=KB_Gaming&count=12";
            XDocument doc;
            DateTime Tweettime = new DateTime();
            string TweetText = "";
            string TweetTextAt = "";
            try
            {
                using (var webClient = new WebClient())
                {
                    doc = XDocument.Parse(webClient.DownloadString(url));
                    foreach (XElement element in doc.Descendants("status"))
                    {
                        TweetTextAt = "";

                        //Tweet text
                        TweetText = element.Element("text").Value;
                        if (TweetText.StartsWith("@"))
                        {
                            TweetTextAt = TweetText.Substring(0, TweetText.IndexOf(" "));
                            TweetText = TweetText.Remove(0, TweetText.IndexOf(" ")).Trim();                            
                        }

                        //tweet date
                        Tweettime = ParseDateTime(element.Element("created_at").Value);

                        TimeSpan TSpan = DateTime.Now - Tweettime;
                        
                        if (TSpan.TotalHours < 24)
                        {
                            _TweetList.Add(new TweetItem(TweetTextAt, TweetText,  Math.Floor(TSpan.TotalHours).ToString() + " hours ago"));
                        }
                        else if (TSpan.TotalHours < 48)
                        {
                            _TweetList.Add(new TweetItem(TweetTextAt, TweetText, "yesterday"));
                        }
                        else
                        {
                            _TweetList.Add(new TweetItem(TweetTextAt, TweetText, TSpan.Days.ToString() + " days ago"));
                        }
                    }
                }
                if (!CloseAllThreads) 
                    this.Invoke(new Action(delegate() { this.ShowTweets(); }));
                //ShowTweets();                
            }
            catch (Exception ex)
            {
                ex.Data.Add("getTwitterFeeds() info start", "");
                ex.Data.Add("url",url);
                ex.Data.Add("Tweettime", Tweettime.ToUniversalTime());
                ex.Data.Add("TweetText",TweetText);
                ex.Data.Add("TweetTextAt", TweetTextAt);

                throw ex;
                //Debug.WriteLine("GetTwitterFeeds() failed with the error: " + ex.Message);
            }
            Thread.CurrentThread.Abort();
        }


        /// <summary>
        /// returns true if info1 is larger then info2
        /// </summary>
        /// <param name="info1"></param>
        /// <param name="info2"></param>
        /// <returns></returns>
        private bool VersionInfo1LargerThenInfo2(xmlVersionInfo info1, xmlVersionInfo info2)
        {
            bool returnValue = false;
            if (info1.VersionMajor > info2.VersionMajor)
                returnValue = true;
            else if (info1.VersionMajor == info2.VersionMajor && info1.VersionMinor > info2.VersionMinor)
                returnValue = true;
            else if (info1.VersionMajor == info2.VersionMajor && info1.VersionMinor == info2.VersionMinor && info1.VersionRevision > info2.VersionRevision)
                returnValue = true;
            else if (info1.VersionMajor == info2.VersionMajor && info1.VersionMinor == info2.VersionMinor && info1.VersionRevision == info2.VersionRevision && info1.VersionPack > info2.VersionPack)
                returnValue = true;
            
            return returnValue;
        }

        private void CheckForClientUpdate()
        {
            Int64 iSize = 0;
            Int64 iRunningByteTotal = 0;

            try
            {
                SetDownloadLabelText("Checking for client updates");
                SetDownloadProgressbarMarqueueStyle(ProgressBarStyle.Marquee);
                //buttonDownloadCancel.Visible = true;
                SetDownloadPanelVisibility(true);

#if(DEBUG)
                xmlVersionInfo local = _formOptions.GetClientVersion();
                xmlVersionInfo remote = _formOptions.GetClientUpdateInfo();
                MessageBox.Show(string.Format("local: {0}.{1}.{2}.{3} - remote: {4}.{5}.{6}.{7}", local.VersionMajor, local.VersionMinor, local.VersionRevision, local.VersionPack, remote.VersionMajor, remote.VersionMinor, remote.VersionRevision, remote.VersionPack));
#endif

                if (VersionInfo1LargerThenInfo2(_formOptions.GetClientUpdateInfo(), _formOptions.GetClientVersion()))
                {
                    if (MessageBox.Show("A new version of the client is avalible. Update now?", "New update available", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                    {

                        SetDownloadLabelText("Updating client");
                        SetDownloadProgressbarMarqueueStyle(ProgressBarStyle.Blocks);

                        System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(_formOptions.KBGClientUpdateUrl);
                        SetAllowUnsafeHeaderParsing20();
                        System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
                        response.Close();

                        iSize = response.ContentLength;  // gets the size of the file in bytes                        
                        iRunningByteTotal = 0; // keeps track of the total bytes downloaded so we can update the progress bar

                        //download the update

                        using (System.Net.WebClient client = new System.Net.WebClient())
                        {
                            // open the file at the remote URL for reading
                            using (System.IO.Stream streamRemote = client.OpenRead(_formOptions.KBGClientUpdateUrl))
                            {
                                // using the FileStream object, we can write the downloaded bytes to the file system
                                using (Stream streamLocal = new FileStream(Application.StartupPath + "\\KBG Minecraft Launcher2.exe", FileMode.Create, FileAccess.Write, FileShare.None))
                                {
                                    // loop the stream and get the file into the byte buffer
                                    int iByteSize = 0;
                                    byte[] byteBuffer = new byte[iSize];
                                    while ((iByteSize = streamRemote.Read(byteBuffer, 0, byteBuffer.Length)) > 0)
                                    {
                                        // write the bytes to the file system at the file path specified
                                        streamLocal.Write(byteBuffer, 0, iByteSize);
                                        iRunningByteTotal += iByteSize;

                                        // calculate the progress out of a base "100"
                                        double dIndex = (double)(iRunningByteTotal);
                                        double dTotal = (double)byteBuffer.Length;
                                        double dProgressPercentage = (dIndex / dTotal);
                                        int iProgressPercentage = (int)(dProgressPercentage * 100);

                                        // update the progress bar
                                        //backgroundWorker1.ReportProgress(iProgressPercentage);
                                        SetDownloadProgressbarProgress(iProgressPercentage);
                                        Thread.SpinWait(1);
                                    }

                                    // clean up the file stream
                                    streamLocal.Close();
                                }

                                // close the connection to the remote server
                                streamRemote.Close();
                            }
                        }
                        SetDownloadPanelVisibility(false);
                        MessageBox.Show("The update was downloaded. The client will now restart!");
                        Process.Start(Application.StartupPath + "\\KBG Minecraft Launcher2.exe", "/UpdateRestart " + Process.GetCurrentProcess().Id.ToString());
                        Environment.Exit(0);
                        Application.Exit();
                        GC.Collect();
                    }
                }
                SetDownloadPanelVisibility(false);
            }
            catch (Exception ex)
            {
                //List<string> information = new List<string>();
                //information.Add("iSize = " + iSize.ToString());
                //information.Add("iRunningByteTotal = " + iRunningByteTotal.ToString());
                //information.Add("Error: " + ex.Message);
                ex.Data.Add("iSize", iSize.ToString());
                ex.Data.Add("iRunningByteTotal", iRunningByteTotal.ToString());

                ErrorReporting(ex, false);

                //MessageBox.Show("Something Failed while updating the client. Error: " + ex.Message);
            }
        }

        private void InstallPack(string packName, bool startGameAfterInstall)
        {
            try
            {

                if (HasConnection())
                {                    
                    if (startGameAfterInstall)
                    {                        
                        packThread = new Thread(new ParameterizedThreadStart(DownloadInstallPackAndStartGame));
                    }
                    else
                    {
                        packThread = new Thread(new ParameterizedThreadStart(DownloadAndInstallPack));
                    }

                    packThread.IsBackground = true;

                    packThread.Start(packName);
                    while (!packThread.IsAlive) ;
                }
                else
                {
                    MessageBox.Show("Could not find a valid internet connection." + Environment.NewLine + "Please check that you have access to the internet on this machine, and try again", "Pack Installation skipped: No internet found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //throw new Exception("No Internet connection could be found. Sorry :(");
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("InstallPack() info start", "");
                ex.Data.Add("packName", packName);
                ex.Data.Add("startGameAfterInstall", startGameAfterInstall.ToString());
                throw ex;
            }
        }

        public void DownloadInstallPackAndStartGame(object oPackName)
        {
            try
            {                        
                if (!CloseAllThreads)
                    if(DownloadAndInstallPackWorker((string)oPackName))
                        StartGame((string)oPackName);
            }
            catch (Exception ex)
            {
                ex.Data.Add("DownloadInstallPackAndStartGame() info start", "");
                ex.Data.Add("oPackName", oPackName.ToString());
                ErrorReporting(ex, false);
            }
        }

        public void DownloadAndInstallPack(object oPackName)
        {
            try
            {
                if (!CloseAllThreads)
                    DownloadAndInstallPackWorker((string)oPackName);
            }
            catch (Exception ex)
            {
                ex.Data.Add("DownloadAndInstallPack() info start", "");
                ex.Data.Add("oPackName", oPackName.ToString());
                ErrorReporting(ex, false);
            }
        }

        /// <summary>
        /// This method should NEVER be called by anything other then DownloadAndInstallPack() and DownloadInstallPackAndStartGame()
        /// Code help thanks to http://www.devtoolshed.com/content/c-download-file-progress-bar
        /// </summary>
        /// <param name="pack"></param>
        private bool DownloadAndInstallPackWorker(string packName)
        {
            //string packName = "";
            Uri url = null;
            string filename = "";
            bool returnValue = false;

            try
            {
                //packName = (string)oPackName;
                //FormOptions.SupportetAutoUpdatePack pack = (FormOptions.SupportetAutoUpdatePack)oPack;                
                url = new Uri(_formOptions.PackUpdateUrl(packName));
                filename = url.OriginalString.Substring(url.OriginalString.LastIndexOf("/") + 1, url.OriginalString.Length - url.OriginalString.LastIndexOf("/") - 1);

                //_formNews = new FormNews(_formOptions.GetVersionInfo(packName, true).UpdateNews);
                //_formNews.Show();
                //_formNews.Focus();
                SetAndShowNews(_formOptions.GetVersionInfo(packName, true).UpdateNews);

                //Download file
                returnValue = DownloadFile(url, filename);

                //extract file
                ExtractFile(packName, filename);

                //this.Invoke(new Action(delegate() { this.UpdatePackSelect(); }));
                //StartGame(packName);
                //MessageBox.Show("DEBUG - Game startet " + packName);

                //test if extract successful? (if minecraft.jar exists)
            }
            catch (Exception ex)
            {
                SetDownloadPanelVisibility(false);

                ex.Data.Add("DownloadAndInstallPack() info start", "");                
                ex.Data.Add("packName", packName);
                if (url != null)
                    ex.Data.Add("url", url.OriginalString);
                ex.Data.Add("filename", filename);
                ErrorReporting(ex, false);
            }
                        
            SetDownloadPanelVisibility(false);
            return returnValue;
        }

        private void ExtractFile(string packName, string filename)
        {
            string MethodProgress = "";
            try
            {

                if (File.Exists(_packDir + "\\" + filename))
                {
                    //Clean out in the pack before extraction, to prevent new versions of mods to screw up.
                    //Use a file named something like UpdateExcludeList.xml to find out what to delete and what not


                    SetDownloadLabelText("Preparing Update");
                    SetDownloadProgressbarProgress(0);
                    SetDownloadPanelVisibility(true);
                    xmlVersionInfo versionInfo;


                    if (Directory.Exists(_packDir + "\\" + packName + "\\.minecraft"))
                    {
                        versionInfo = _formOptions.GetVersionInfo(packName, true);
                        string fullPath = "";
                        string backupTmpPath = _packDir + "\\" + packName + "\\UpdateBackup";
                        FileAttributes attr;



                        MethodProgress = "Backup - starting";
                        //backup files before deleting everything in the pack folder
                        foreach (string excludeInfo in versionInfo.ExcludeFromUpdate)
                        {
                            if (CloseAllThreads)
                                break;

                            fullPath = _packDir + "\\" + packName + "\\.minecraft\\" + excludeInfo;

                            if (Directory.Exists(fullPath) || File.Exists(fullPath))
                            {

                                attr = File.GetAttributes(fullPath);
                                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                                {
                                    //directory
                                    string tmpStr = backupTmpPath + "\\" + excludeInfo;
                                    if (excludeInfo.Contains("\\"))
                                    {
                                        string tmpStr2 = tmpStr.Substring(0, tmpStr.LastIndexOf("\\"));
                                        if (!Directory.Exists(tmpStr2))
                                            Directory.CreateDirectory(tmpStr2);
                                        //tmpStr2 = tmpStr2;
                                    }
                                    MethodProgress = "Backup - moving - Directory";
                                    Directory.Move(fullPath, tmpStr);
                                }
                                else
                                {
                                    FileInfo fInfo = new FileInfo(backupTmpPath + "\\" + excludeInfo);
                                    if (!Directory.Exists(fInfo.DirectoryName))
                                        Directory.CreateDirectory(fInfo.DirectoryName);

                                    //file                
                                    MethodProgress = "Backup - moving - File";
                                    File.Move(fullPath, backupTmpPath + "\\" + excludeInfo);
                                }
                            }
                        }

                        //cleaning out
                        MethodProgress = "Backup - Deleting";
                        if (!CloseAllThreads)
                            Directory.Delete(_packDir + "\\" + packName + "\\.minecraft", true);
                    }

                    //extract the pack
                    if (!CloseAllThreads)
                    {
                        if (new FileInfo(filename).Extension.ToLower() == ".zip")
                        {
                            SetDownloadLabelText("Extracting " + filename);
                            SetDownloadProgressbarProgress(0);
                            SetDownloadPanelVisibility(true);

                            MethodProgress = "Extract - start";

                            //ExtractZip(_packDir + "\\" + filename, _packDir + "\\" + pack.ToString());
                            double dProgressPercentage;// = (dIndex / dTotal);
                            //int iProgressPercentage = (int)(dProgressPercentage * 100);

                            using (ZipFile zip1 = ZipFile.Read(_packDir + "\\" + filename))
                            {
                                int previousPercentage = 0;
                                //foreach (ZipEntry e in zip1)
                                MethodProgress = "Extract - extracting";
                                for (int i = 0; i < zip1.Count; i++)
                                {
                                    if (CloseAllThreads)
                                        break;
                                    dProgressPercentage = (i / (double)zip1.Count);
                                    //this.Invoke(new Action(delegate() { progressBarDownload.Value = (int)(dProgressPercentage * 100); }));
                                    if ((int)(dProgressPercentage * 100) != previousPercentage)
                                    {
                                        SetDownloadProgressbarProgress((int)(dProgressPercentage * 100));
                                        previousPercentage = (int)(dProgressPercentage * 100);
                                    }
                                    //progressBarDownload.Value = (int)(dProgressPercentage * 100);
                                    zip1[i].Extract(_packDir + "\\" + packName, ExtractExistingFileAction.OverwriteSilently);
                                }
                            }
                            //deletes the downloaded zip after extraction
#if(!DEBUG)
                            if (File.Exists(_packDir + "\\" + filename))
                                File.Delete(_packDir + "\\" + filename);
#endif
                        }
                        else
                        {
                            MessageBox.Show("The downloaded file is not a zip file. This program will now proceed to panic and abort the extraction", "File not a zip file");
                        }
                    }

                    //restores excluded files
                    if (!CloseAllThreads)
                    {
                        if (Directory.Exists(_packDir + "\\" + packName + "\\UpdateBackup"))
                        {
                            MethodProgress = "Restoring backup - Copying";
                            CopyAll(new DirectoryInfo(_packDir + "\\" + packName + "\\UpdateBackup"), new DirectoryInfo(_packDir + "\\" + packName + "\\.minecraft"));
                            MethodProgress = "Restoring backup - Deleting";
                            Directory.Delete(_packDir + "\\" + packName + "\\UpdateBackup", true);
                            //Directory.Move(_packDir + "\\" + packName + "\\UpdateBackup", _packDir + "\\" + packName + "\\.minecraft"); 
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("ExtractFile() info start", "");
                ex.Data.Add("packName", packName);
                ex.Data.Add("filename", filename);
                ex.Data.Add("MethodProgress", MethodProgress);
                throw ex;
            }

        }

        private void DownloadFile(Uri Url)
        {
            try
            {
                string filename = Url.OriginalString.Substring(Url.OriginalString.LastIndexOf("/") + 1, Url.OriginalString.Length - Url.OriginalString.LastIndexOf("/") - 1);
                DownloadFile(Url, filename);
            }
            catch (Exception ex)
            {
                ex.Data.Add("DownloadFile() info start", "");
                ex.Data.Add("Url", Url.OriginalString);
                throw ex;
            }
        }

        private bool DownloadFile(Uri url, string filename)
        {
            bool SkipDownload = false;
            Int64 iSize = 0;
            Int64 iRunningByteTotal = 0;
            DateTime previousTickTime;
            TimeSpan tickDuration;
            int previousPercentage = 0;
            Int64 TickRunningByte = 0;
            bool returnValue = true;

            try
            {
                SetDownloadLabelText("Preparing download");
                SetDownloadProgressbarProgress(0);
                SetDownloadPanelVisibility(true);

                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                SetAllowUnsafeHeaderParsing20();
                System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
                response.Close();

                iSize = response.ContentLength;  // gets the size of the file in bytes                        
                iRunningByteTotal = 0; // keeps track of the total bytes downloaded so we can update the progress bar



                //check if pack has already been downloaded


                if (File.Exists(_packDir + "\\" + filename))
                    if (new FileInfo(_packDir + "\\" + filename).Length == iSize) //file exists, but what about the size ? (to filter out incomplete downloads)
                        if (MessageBox.Show("A file with matching name and size was found on the disk." + Environment.NewLine + "Do you want to use that file instead of downloading it again?", "Existing file found", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                            SkipDownload = true;


                #region File Download
                if (!SkipDownload)
                {
                    //download the pack
                    SetDownloadPanelVisibility(true);
                    SetDownloadLabelText("Downloading " + filename);
                    SetDownloadProgressbarProgress(0);
                    SetDownloadCancelButtonVisibility(true);

                    using (System.Net.WebClient client = new System.Net.WebClient())
                    {
                        // open the file at the remote URL for reading
                        using (System.IO.Stream streamRemote = client.OpenRead(url))
                        {
                            // using the FileStream object, we can write the downloaded bytes to the file system
                            using (Stream streamLocal = new FileStream(_packDir + "\\" + filename, FileMode.Create, FileAccess.Write, FileShare.None))
                            {
                                // loop the stream and get the file into the byte buffer
                                int iByteSize = 0;
                                byte[] byteBuffer = new byte[iSize];
                                previousTickTime = DateTime.Now;

                                while ((iByteSize = streamRemote.Read(byteBuffer, 0, byteBuffer.Length)) > 0)
                                {
                                    //aborts the download if a cancel request have been made
                                    if (CloseAllThreads)
                                        break;
                                    if (_abortDownload)
                                    {
                                        MessageBox.Show("The download has been cancelled by the user.", "Download Cancelled!");
                                        returnValue = false;
                                        break;
                                    }

                                    // write the bytes to the file system at the file path specified
                                    streamLocal.Write(byteBuffer, 0, iByteSize);
                                    iRunningByteTotal += iByteSize;

                                    // calculate the progress out of a base "100"
                                    double dIndex = (double)(iRunningByteTotal);
                                    double dTotal = (double)byteBuffer.Length;
                                    double dProgressPercentage = (dIndex / dTotal);
                                    int iProgressPercentage = (int)(dProgressPercentage * 100);

                                    // update the progress bar
                                    //backgroundWorker1.ReportProgress(iProgressPercentage);

                                    if (previousPercentage < iProgressPercentage)
                                    {

                                        tickDuration = DateTime.Now - previousTickTime;
                                        double speed = (1 / tickDuration.TotalSeconds) * (iRunningByteTotal - TickRunningByte);
                                        previousTickTime = DateTime.Now;
                                        TickRunningByte = iRunningByteTotal;

                                        SetDownloadLabelSpeedAndProgressText(string.Format("{0} KB/s", Math.Floor(speed / 1024)), string.Format("{0} / {1} MB", iRunningByteTotal / 1048576, byteBuffer.LongLength / 1048576));
                                        SetDownloadProgressbarProgress(iProgressPercentage);
                                        previousPercentage = iProgressPercentage;
                                    }

                                }

                                // clean up the file stream
                                streamLocal.Close();
                            }

                            // close the connection to the remote server
                            streamRemote.Close();
                        }
                    }
                    if (!_abortDownload)
                    {
                        SetDownloadProgressbarProgress(100);
                        SetDownloadLabelText("Download Complete");
                    }
                }
                if (_abortDownload && !CloseAllThreads)
                {
                    File.Delete(_packDir + "\\" + filename);
                    _abortDownload = false;
                    SetDownloadPanelVisibility(false);
                }

                SetDownloadCancelButtonVisibility(false);
                #endregion File Download

            }
            catch (Exception ex)
            {

                ex.Data.Add("DownloadFile() info start", "");
                ex.Data.Add("Url", url.OriginalString);
                ex.Data.Add("filename", filename);
                ex.Data.Add("SkipDownload", SkipDownload.ToString());
                ex.Data.Add("iSize", iSize.ToString());
                ex.Data.Add("iRunningByteTotal", iRunningByteTotal.ToString());

                throw ex;
            }
            return returnValue;
        }

        private void StartGame(string selItem)
        {
            try
            {
                SetDownloadLabelText("Logging in");

                ProcessStartInfo procStartInfo = new ProcessStartInfo();

                string session = generateSession(textBoxUsername.Text, textBoxPassword.Text, 5000);
                string sessionID = "";
                string username = "";

                if (session.ToLower().Contains("bad login"))
                {
                    MessageBox.Show("Invalid Username and/or Password." + Environment.NewLine + "Please make sure you typed in the right information", "Invalid Username and/or Password",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);

                }
                else if (session == "Account migrated, use e-mail as username.")
                {
                    MessageBox.Show("Your account has been migrated to a Mojang acocunt. Use your email as a username to log in", "Account migrated");
                }
                else
                {
                    //assumes that a valid session was retrieved. Might need more work here
                    if (session.Contains(":"))
                        sessionID = session.Split(':')[3];

                    if (textBoxUsername.Text.Contains("@"))                    
                        username = session.Split(':')[2];
                    else
                        username = textBoxUsername.Text;


                    if (File.Exists(_packDir + "\\" + selItem + "\\.Minecraft\\bin\\minecraft.jar"))
                    {
                        //FINALLY i got it to work. Damm it was a pain
                        SetDownloadLabelText("Starting game");
                        procStartInfo.FileName = GetJavaInstallationPath() + @"\bin\javaw.exe";
                        Environment.SetEnvironmentVariable("APPDATA", _packDir + "\\" + selItem);
                        procStartInfo.Arguments = Environment.ExpandEnvironmentVariables(string.Format(@" -Xms{0}m -Xmx{1}m -cp ""%APPDATA%\.minecraft\bin\*"" -Djava.library.path=""%APPDATA%\.minecraft\bin\natives"" net.minecraft.client.Minecraft {2} {3}", /*0*/ _formOptions.GetMemmoryMin(), /*1*/ _formOptions.GetMemmoryMax(), /*2*/ username, /*3*/ sessionID));


#if(DEBUG)
                    MessageBox.Show("DEBUG - Game startet");
#else

                        Process.Start(procStartInfo);

                        this.Invoke(new Action(delegate() { this.Close(); }));
#endif
                    }
                    else
                    {
                        MessageBox.Show("The minecraft.jar file was not found. Make sure you installed the mod pack correctly", "Incorrectly installed minecraft mod pack", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //and this is just the remnants of my failed tries


            //procStartInfo.StartInfo.FileName =  GetJavaInstallationPath() + "\\bin\\java.exe";

            //procStartInfo.StartInfo.EnvironmentVariables.Keys["APPDATA"] = _packDir + "\\" + selItem;
            //procStartInfo.EnvironmentVariables.Remove("APPDATA");
            //procStartInfo.EnvironmentVariables.Add("APPDATA", _packDir + "\\" + selItem);

            //procStartInfo.UseShellExecute = false;
            //procStartInfo.StartInfo.CreateNoWindow = false;

            //procStartInfo.StartInfo.Arguments = string.Format("-Xms{0}M -Xmx{1}M -Djava.library.path={2}.minecraft\\bin\\natives -cp {2}.minecraft\\bin\\minecraft.jar;{2}.minecraft\\bin\\jinput.jar;{2}.minecraft\\bin\\lwjgl.jar;{2}.minecraft\\bin\\lwjgl_util.jar net.minecraft.client.Minecraft {3} {4}", _formOptions.GetMemmoryMin(), _formOptions.GetMemmoryMax(), _packDir + "\\" + selItem + "\\", textBoxUsername.Text, sessionID);
            //procStartInfo.StartInfo.Arguments = string.Format("-Xms{0}M -Xmx{1}M -Djava.library.path=%APPDATA%\\.minecraft\\bin\\natives -cp %APPDATA%\\.minecraft\\bin\\minecraft.jar;%APPDATA%\\.minecraft\\bin\\jinput.jar;%APPDATA%\\.minecraft\\bin\\lwjgl.jar;%APPDATA%\\.minecraft\\bin\\lwjgl_util.jar net.minecraft.client.Minecraft {2} {3}", _formOptions.GetMemmoryMin(), _formOptions.GetMemmoryMax(), textBoxUsername.Text, sessionID);

            //procStartInfo.StartInfo.Arguments = " -Xms" + _formOptions.GetMemmoryMin() + "m -Xmx" + _formOptions.GetMemmoryMax() + "m -cp \"%APPDATA%\\.minecraft\\bin\\*\" -Djava.library.path=\"%APPDATA%\\.minecraft\\bin\\natives\" net.minecraft.client.Minecraft" + textBoxUsername.Text + textBoxPassword.Text;
            //procStartInfo.StartInfo.Arguments = procStartInfo.StartInfo.Arguments.Replace("%APPDATA%", _packDir + "\\" + selItem);
            //procStartInfo.StartInfo.Arguments = @"-Xms512m -Xmx1024m -cp %APPDATA%\.minecraft\bin\* -Djava.library.path=%APPDATA%\.minecraft\bin\natives net.minecraft.client.Minecraft GrandPhoenix82 jabjab1";

            //procStartInfo.StartInfo.FileName = "java";

            //Console.WriteLine("session id " + sessionID);

            //Environment.SetEnvironmentVariable("APPDATA", _packDir + "\\" + selItem); //C:\Users\Phoenix\AppData\Roaming
            //string args = "-Xincgc -Xmx1024m -cp \"%APPDATA%\\.minecraft\\bin\\minecraft.jar;%APPDATA%\\.minecraft\\bin\\lwjgl.jar;%APPDATA%\\.minecraft\\bin\\lwjgl_util.jar;%APPDATA%\\.minecraft\\bin\\jinput.jar\" -Djava.library.path=\"%APPDATA%\\.minecraft\\bin\\natives\" net.minecraft.client.Minecraft \"NAME\"";
            //procStartInfo.Arguments = Environment.ExpandEnvironmentVariables(@" -Xms512m -Xmx1024m -cp ""%APPDATA%\.minecraft\bin\*"" -Djava.library.path=""%APPDATA%\.minecraft\bin\natives"" net.minecraft.client.Minecraft GrandPhoenix82 jabjab1");

            // - This is the one that works ---> //procStartInfo.Arguments = Environment.ExpandEnvironmentVariables(@" -Xms" + _formOptions.GetMemmoryMin().ToString() + "m -Xmx" + _formOptions.GetMemmoryMax().ToString() + @"m -cp ""%APPDATA%\.minecraft\bin\*"" -Djava.library.path=""%APPDATA%\.minecraft\bin\natives"" net.minecraft.client.Minecraft " + textBoxUsername.Text + " " + sessionID);



            //java -Xms512m -Xmx1024m -cp "%APPDATA%\.minecraft\bin\*" -Djava.library.path="%APPDATA%\.minecraft\bin\natives" net.minecraft.client.Minecraft GrandPhoenix82 jabjab1

            //procStartInfo.WindowStyle = ProcessWindowStyle.Normal;
            //procStartInfo.RedirectStandardError = true;
            //procStartInfo.RedirectStandardInput = true;
            //procStartInfo.RedirectStandardOutput = true;


            //this.Hide();
            //procStartInfo.WindowStyle = ProcessWindowStyle.Maximized;


            //FFS. Minecraft and Java hates me!!


            //"java -Xms256m -Xmx256m -cp \"%APPDATA%\\.minecraft\\bin\\minecraft.jar;%APPDATA%\\.minecraft\\bin\\jinput.jar;%APPDATA%\\.minecraft\\bin\\lwjgl.jar;%APPDATA%\\.minecraft\\bin\\lwjgl_util.jar\" -Djava.library.path=\"%APPDATA%\\.minecraft\\bin\\natives\" net.minecraft.client.Minecraft"
            //string orgJavaString = "java -Xms256m -Xmx256m -cp \"%APPDATA%\\.minecraft\\bin\\minecraft.jar;%APPDATA%\\.minecraft\\bin\\jinput.jar;%APPDATA%\\.minecraft\\bin\\lwjgl.jar;%APPDATA%\\.minecraft\\bin\\lwjgl_util.jar\" -Djava.library.path=\"%APPDATA%\\.minecraft\\bin\\natives\" net.minecraft.client.Minecraft";

            //string javaString = string.Format(" javaw -Xms{0}M -Xmx{1}M -cp \"{2}\\.minecraft\\bin\\minecraft.jar;{2}\\.minecraft\\bin\\jinput.jar;{2}\\.minecraft\\bin\\lwjgl.jar;{2}\\.minecraft\\bin\\lwjgl_util.jar\" -Djava.library.path=\"{2}\\.minecraft\\bin\\natives\" net.minecraft.client.Minecraft {3} {4}", _formOptions.GetMemmoryMin().ToString(), _formOptions.GetMemmoryMax().ToString(), _packDir + "\\" + selItem, textBoxUsername.Text, textBoxPassword.Text);
            //string javaString = string.Format(" -Xms{0}M -Xmx{1}M -cp \"{2}\\.minecraft\\bin\\minecraft.jar;{2}\\.minecraft\\bin\\jinput.jar;{2}\\.minecraft\\bin\\lwjgl.jar;{2}\\.minecraft\\bin\\lwjgl_util.jar\" -Djava.library.path=\"{2}\\.minecraft\\bin\\natives\" net.minecraft.LauncherFrame {3} {4}", _formOptions.GetMemmoryMin().ToString(), _formOptions.GetMemmoryMax().ToString(), _packDir + "\\" + selItem, textBoxUsername.Text, textBoxPassword.Text);
            //Process MCprocess = System.Diagnostics.Process.Start(javaString);
            //alternative    javaw -Xms512M -Xmx512M -cp Minecraft.exe net.minecraft.LauncherFrame



            //string appData = Environment.GetEnvironmentVariable("APPDATA");
            //object java = Wscript CreateObject("WScript.Shell")
            //java.Environment("PROCESS")("APPDATA") = LauncherFolder & "packs\" & CboMinecraftVersion.SelectedItem
            //java.Run("Minecraft.exe " + txtUsername.Text + " " + txtPassword.Text)


            //Process.Start(@"java", javaString);
            //  @"-Xms512m -Xmx1024m -cp """ + appData + @"\.minecraft\bin\*"" -Djava.library.path=""" + appData + @"\.minecraft\bin\natives"" net.minecraft.client.Minecraft");

            //var processInfo = new ProcessStartInfo("java.exe", javaString)
            //{
            //    CreateNoWindow = false,
            //    UseShellExecute = true
            //};
            //Process procStartInfo;

            //if ((procStartInfo = Process.Start(processInfo)) == null)
            //{
            //    //throw new InvalidOperationException("??");
            //}

            //procStartInfo.WaitForExit();
            //int exitCode = procStartInfo.ExitCode;
            //procStartInfo.Close();


            //java -Xms256m -Xmx256m -cp "%APPDATA%\.minecraft\bin\minecraft.jar;%APPDATA%\.minecraft\bin\jinput.jar;%APPDATA%\.minecraft\bin\lwjgl.jar;%APPDATA%\.minecraft\bin\lwjgl_util.jar" -Djava.library.path="%APPDATA%\.minecraft\bin\natives" net.minecraft.client.Minecraft
            //Starts the game
        }

        public void ShowErrorWindow(bool criticalError)
        {
            try
            {
                if (criticalError)
                    this.Invoke(new Action(delegate() { _formError.ShowDialog(); _formError.Focus(); }));
                else
                    this.Invoke(new Action(delegate() { _formError.Show(); _formError.Focus(); }));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gathers error information and shows the error window
        /// </summary>
        /// <param name="errorLocation">The location of where the erorr occurred</param>
        /// <param name="errorInfo">Information about the error</param>
        /// <param name="criticalError">If set to true, then the program closes after the error hhas been shown</param>        
        public void ErrorReporting(Exception ex, bool criticalError)
        {
            //error information
            try
            {
                _formError.AddInfoLine("Error: " + ex.Message + Environment.NewLine);

                _formError.AddInfoLine("Error Occured at: " + Environment.NewLine + ex.StackTrace);
                _formError.AddInfoLine(Environment.NewLine + Environment.NewLine + "Extra error information" + Environment.NewLine + "{");
                foreach (DictionaryEntry de in ex.Data)
                {
                    _formError.AddInfoLine(string.Format("      {0} = {1}", de.Key, de.Value));
                }
                _formError.AddInfoLine("}");


                //basic information
                _formError.AddInfoLine(Environment.NewLine + "FormMain information");
                _formError.AddInfoLine("_TweetList null?: " + (_TweetList == null).ToString());
                _formError.AddInfoLine("_formOptions null? " + (_formOptions == null).ToString());
                _formError.AddInfoLine("_formNews null? " + (_formNews == null).ToString());
                _formError.AddInfoLine("_updateFinished = " + _updateFinished.ToString());

                _formError.AddInfoLine("_packDir = " + _packDir);
                _formError.AddInfoLine("_abortDownload = " + _abortDownload.ToString());
                _formError.AddInfoLine("_loadingSettings = " + _loadingSettings.ToString());


                if (_formOptions != null)
                {
                    _formError.AddInfoLine(Environment.NewLine + "FormOptions information");
                    List<string> information = _formOptions.formOptionsInformation();

                    foreach (string info in information)
                    {
                        _formError.AddInfoLine("    " + info);
                    }
                }


                if (_formError == null)
                    _formError = new FormError();
                _formError.CriticalError = criticalError;
                ShowErrorWindow(criticalError);
            }
            catch (Exception ex2)
            {
                MessageBox.Show("Woah hold on a sec. En error occurred in the error handling?. Damm this is bad. Please report this error as soon as possible" + Environment.NewLine + "Error: " + ex2.Message + Environment.NewLine + ex.StackTrace, "An Unlikely error occurred :(", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //Events

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
#if(DEBUG)
                buttonDebug.Visible = true;
                textBoxDebug.Visible = true;
#endif

                _packDir = Application.StartupPath + "\\Minecraft Packs";

                GenerateTweetList();
                StartCheckingServers();

                _formOptions = new FormOptions(this);


                _loadingSettings = true;

                foreach (string folder in Directory.GetDirectories(_packDir))
                    comboBoxPackSelect.Items.Add(folder.Replace(_packDir + "\\", ""));
                if (comboBoxPackSelect.Items.Count > 0)
                    comboBoxPackSelect.SelectedIndex = 0;



                for (int i = 0; i < comboBoxPackSelect.Items.Count; i++)
                {
                    if (comboBoxPackSelect.Items[i].ToString() == _formOptions.Settings.LastPlayedServer)
                    {
                        comboBoxPackSelect.SelectedIndex = i;
                        break;
                    }
                }

                if (_formOptions.Settings.RememberLogin)
                {

                    textBoxPassword.Text = _formOptions.Settings.Password;
                    textBoxUsername.Text = _formOptions.Settings.Username;
                    checkBoxRememberLoginInfo.Checked = _formOptions.Settings.RememberLogin;

                }

                _loadingSettings = false;

                if (_updateFinished)
                {
                    //_formOptions.GetClientUpdateInfo().UpdateNews                    
                    if (HasConnection())
                    {
                        if (_formNews == null)
                            _formNews = new FormNews(_formOptions.GetClientUpdateInfo().UpdateNews);
                        _formNews.Show();
                        _formNews.Focus();
                    }
                    else
                    {
                        MessageBox.Show("Could not find a valid internet connection." + Environment.NewLine + "Please check that you have access to the internet on this machine, and try again", "ClientUpdate News skipped: No internet found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    if (HasConnection())
                    {
                        updateThread = new Thread(new ThreadStart(CheckForClientUpdate));
                        updateThread.Start();
                        while (!updateThread.IsAlive) ;
                    }
                    else
                    {
                        MessageBox.Show("Could not find a valid internet connection." + Environment.NewLine + "Please check that you have access to the internet on this machine, and try again", "ClientUpdate check skipped: No internet found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

            }
            catch (Exception ex)
            {
                //List<string> information = new List<string>();
                //information.Add("Error: " + ex.Message);
                //ErrorReporting("Form1_Load()", information, true);

                ErrorReporting(ex, true);
                //MessageBox.Show("Program initialization failed." + Environment.NewLine + "Error message: " + ex.Message, "An error occurred :(", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //check for new client version

            //if(File.Exists(Application.StartupPath + "\\KBG Minecraft Launcher2.exe"))
            //{
            //    //File.Delete(Application.StartupPath + "\\KBG Minecraft Launcher2.exe");
            //    File.Copy(Application.StartupPath + "\\KBG Minecraft Launcher2.exe", Application.StartupPath + "\\KBG Minecraft Launcher.exe", true);
            //    File.Delete(Application.StartupPath + "\\KBG Minecraft Launcher2.exe");
            //}



            //if (_formOptions.GetClientVersion() < _formOptions.GetClientUpdateInfo().Version)
            //{
            //    if (MessageBox.Show("A new version of the client is avalible. Update now?", "New update available", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
            //    {
            //        DownloadFile(new Uri(_formOptions.KBGClientUpdateUrl), Application.StartupPath + "\\KBG Minecraft Launcher2.exe");
            //        MessageBox.Show("The update was downloaded. The client will now restart!");
            //        Process.Start(Application.StartupPath + "\\KBG Minecraft Launcher2.exe");
            //        this.Close();
            //    }
            //}
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {

            if (_formNews != null)
            {
                _formNews.Show();
                _formNews.Focus();
            }




            //DownloadFile(new Uri(_formOptions.KBGClientUpdateUrl), Application.StartupPath + "\\KBG Minecraft Launcher2.exe");

        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseAllThreads = true;
            //if (TweetThread != null)
            //    TweetThread.Abort();
            //if (pingIRThread != null)
            //    pingIRThread.Abort();
            //if (pingERThread != null)
            //    pingERThread.Abort();
            //if (pingMiningThread != null)
            //    pingMiningThread.Abort();
            //if (pingTFCRThread != null)
            //    pingTFCRThread.Abort();
            //if (pingMinecraftDotNetThread != null)
            //    pingMinecraftDotNetThread.Abort();
            //if (pingMinecraftLoginServersThread != null)
            //    pingMinecraftLoginServersThread.Abort();
            //if (packThread != null)
            //{
                
            //    packThread.Abort();
            //}
            //if (updateThread != null)
            //    updateThread.Abort();


            //if (TweetThread != null)
            //    while (TweetThread.IsAlive) ;
            //if (pingIRThread != null)
            //    while (pingIRThread.IsAlive) ;
            //if (pingERThread != null)
            //    while (pingERThread.IsAlive) ;
            //if (pingMiningThread != null)
            //    while (pingMiningThread.IsAlive) ;
            //if (pingTFCRThread != null)
            //    while (pingTFCRThread.IsAlive) ;
            //if (pingMinecraftDotNetThread != null)
            //    while (pingMinecraftDotNetThread.IsAlive) ;
            //if (pingMinecraftLoginServersThread != null)
            //    while (pingMinecraftLoginServersThread.IsAlive) ;
            //if (packThread != null)
            //    while (packThread.IsAlive) ;
            //if (updateThread != null)
            //    while (updateThread.IsAlive) ;

        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                StartCheckingServers();
            }
            catch (Exception ex)
            {
                ErrorReporting(ex, false);
            }
        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(e.LinkText);
            }
            catch (Exception ex)
            {
                ErrorReporting(ex, false);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            #region OldTestCode
            //progressBarIR.Visible = true;
            //progressBarER.Visible = true;
            //progressBarMinecraftdotnet.Visible = true;
            //progressBarMinecraftLoginServers.Visible = true;
            //progressBarMining.Visible = true;
            //progressBarTFCR.Visible = true;

            //timer1.Enabled = true;

            //ServerAccessClass ping = new ServerAccessClass("",labelIRResult,progressBarIR,this);
            //Thread pingThread1 = new Thread(new ThreadStart(ping.StartCheck));
            //pingThread1.Start();
            //while (!pingThread1.IsAlive);
            //StartCheckingServers();
            //GenerateTweetList();
            //using (var client = new WebClient())
            //{
            //    client.SkipDownload(_formOptions.DownloadLinkVanillaMC, Application.StartupPath + "\\Minecraft Packs\\test");
           //}
            //comboBoxPackSelect.SelectedIndex = comboBoxPackSelect.Items.IndexOf("Terrafirma Craft");
            //DownloadFileStart(new BackgroundWorkerArgumentWrapper(_formOptions.DownloadLinkTFCR, comboBoxPackSelect.SelectedItem.ToString()));

            //panelDownload.Visible = true;
            //ExtractZip(@"C:\Users\Phoenix\Desktop\Programering\VS 2010\Minecraft Launcher\Minecraft Launcher\bin\Debug\Minecraft Packs\test.zip", @"C:\Users\Phoenix\Desktop\Programering\VS 2010\Minecraft Launcher\Minecraft Launcher\bin\Debug\Minecraft Packs\test");
            //PackConfigurationSection packConfigSection = ConfigurationManager.GetSection("PackSection") as PackConfigurationSection;
            //System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            
            
            //if (ConfigurationManager.GetSection("PackSection") as PackConfigurationSection == null)
            //if (config.GetSection("PackSection") as PackConfigurationSection == null)
            //{
            //    try
            //    {

            //        PackConfigurationSection customSection;

            //        // Get the current configuration file.
            //        //System.Configuration.Configuration config =
            //        //        ConfigurationManager.OpenExeConfiguration(
            //        //        ConfigurationUserLevel.None);

            //        // Create the section entry  
            //        // in the <configSections> and the 
            //        // related target section in <configuration>.
            //        if (config.Sections["PackSection"] == null)
            //        {
            //            customSection = new PackConfigurationSection();
            //            config.Sections.Add("PackSection", customSection);
            //            customSection.SectionInformation.ForceSave = true;
            //            config.Save(ConfigurationSaveMode.Full);
            //        }
            //    }
            //    catch (ConfigurationErrorsException err)
            //    {
            //        Console.WriteLine(err.ToString());
            //    }
            //}


            //PackConfigurationSection packConfigSection = /*config.GetSection("PackSection") as PackConfigurationSection; //*/PackConfigurationSection.GetConfig() as PackConfigurationSection;
            
            //packConfigSection.Packs.
            //packConfigSection.Packs.Add(new PackClass("Industrial Rage",1.3,"packUrl","versionUrl","filename"));

            //SHI.WebTeam.Common.ConfigurationManager.ShiConfiguration ConfigSettings =
            //      SHI.WebTeam.Common.ConfigurationManager.ShiConfiguration.GetConfig();

            //Only populate if the section exists.
            //if (packConfigSection != null)
            //{
            //    //Add the setting for the current machine's codebase to the 
            //    //settings collection. Current Codebases values are "Production"
            //    //and "Development". The validateCodebase method inforces this.
            //    PackConfigurationClass newpack = new PackConfigurationClass();
            //    newpack.Name = "name1";
            //    newpack.Version = 0.4;
            //    newpack.VersionFileName = "versionfilename1";
            //    newpack.VersionUrl = "versionurl";
            //    newpack.DownloadUrl = "url";
            //    packConfigSection.Packs.Add(newpack);
            //    //packConfigSection.sa
            //    //for (int i = 0; i < packConfigSection.Packs.Count; i++)
            //    //{
                    

            //    //    packConfigSection.Packs[0].Name = "name1";
            //    //    packConfigSection.Packs[0].Version = 0.4;
            //    //    packConfigSection.Packs[0].VersionFileName = "versionfilename1";
            //    //    packConfigSection.Packs[0].VersionUrl = "versionurl";
            //    //    packConfigSection.Packs[0].DownloadUrl = "url";
            //    //    packConfigSection.Packs.AddElementName = "something";
            //    //    //if (ConfigurationSettings.CodeBase == CodeBases.Production)
            //    //    //{
            //    //    //    base.Add(packConfigSection.shiSettings[i].Key,
            //    //    //        packConfigSection.shiSettings[i].Prod);
            //    //    //}
            //    //    //else if (ConfigurationSettings.CodeBase == CodeBases.Development)
            //    //    //{
            //    //    //    base.Add(packConfigSection.shiSettings[i].Key,
            //    //    //        packConfigSection.shiSettings[i].Dev);
            //    //    //}
            //    //    //else
            //    //    //{
            //    //    //    throw new Exception("The configured codebase value is " +
            //    //    //       "not currently implemented.");
            //    //    //}
            //    //}
            //}

            //packConfigSection.Save(ConfigurationSaveMode.Full);
            //config.Save();
            //PackClass packClass = packConfigSection.Packs[0];
            #endregion OldTestCode
            DateTime old = DateTime.Now;

            try
            {
                if (_formOptions.Settings == null)
                    _formOptions.Settings = new xmlSettings();
                //_settings.CreateNewSettingsFile();
                //_settings.Username = "HAHAHA";

                



                //string asdf = _formOptions.Settings.Username;
                TimeSpan span = DateTime.Now - old;
                //_formOptions.Settings.SavePackInfo(new PackClass("navn3", 0.3333, "packurl3333", "www.something.com/versionurl3333.rar"));
                //_formOptions.Settings.Username = "GrandPhoenix82";

                //MessageBox.Show(span.TotalMilliseconds.ToString() + " - " + _formOptions.KBGClientVersion.ToString() + " - " + _formOptions.KBGClientVersionFromUrl.ToString());
                //_formOptions.Settings.Username = "SDFG";
                //XmlDocument xdoc = new XmlDocument();
                //xdoc.Load(@"C:\Users\Phoenix\Desktop\Programering\VS 2010\KBG Minecraft Launcher\KBGClientVersion.xml");

                //xdoc.Save(@"C:\Users\Phoenix\Desktop\Programering\VS 2010\KBG Minecraft Launcher\KBGClientVersion.xml");
                //MessageBox.Show(xdoc.SelectSingleNode("KBGVersionInfo/News").InnerText);
                //MessageBox.Show(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
                //MessageBox.Show(getCommandLineArgs());
                //Application.Exit();
                //int asdf = int.Parse("SDFG");
                throw new Exception("DEMO critical exception");
                //MessageBox.Show(double.Parse(textBoxDebug.Text, System.Globalization.NumberFormatInfo.InvariantInfo).ToString());
                //File.Replace(Application.StartupPath + "\\KBG Minecraft Launcher2.exe", Application.StartupPath + "\\KBG Minecraft Launcher.exe", Application.StartupPath + "\\KBG Minecraft Launcher.backup", true);


            }
            catch (Exception ex)
            {
                //ex.Data.Add("button1_Click_1() start info", "");
                //ex.Data.Add("F U ALL :P", "value");
                ErrorReporting(ex, true);
            }

        }

        private void buttonOptions_Click(object sender, EventArgs e)
        {
            try
            {
                if (_formOptions == null || _formOptions.IsDisposed)
                    _formOptions = new FormOptions(this);
                else
                {
                    _formOptions.WindowState = FormWindowState.Normal;
                    _formOptions.Activate();
                }
                _formOptions.Show();
            }
            catch (Exception ex)
            {
                ErrorReporting(ex, false);

            }
        }

        private void comboBoxPackSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            //UpdatePackSelect();
            try
            {
                if (!_loadingSettings)
                {
                    _formOptions.Settings.LastPlayedServer = comboBoxPackSelect.SelectedItem.ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorReporting(ex, false);
            }
        }

        private void buttonDownloadCancel_Click(object sender, EventArgs e)
        {
            _abortDownload = true;
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string selItem = comboBoxPackSelect.SelectedItem.ToString();
                bool minecraftJarFound = File.Exists(_packDir + "\\" + comboBoxPackSelect.SelectedItem.ToString() + "\\.minecraft\\bin\\minecraft.jar");


                if (_formOptions.CheckNameForAutoUpdateSupport(selItem))
                {
                    if (minecraftJarFound)
                    {
                        //check for update
                        labelDownload.Text = "Checking for updates";
                        progressBarDownload.Style = ProgressBarStyle.Marquee;
                        panelDownload.Visible = true;

                        //check update
                        bool UpdateFound = VersionInfo1LargerThenInfo2(_formOptions.GetVersionInfo(selItem, false), _formOptions.GetVersionInfo(selItem, true));

                        if (UpdateFound)
                        {
                            if (MessageBox.Show("A new version of the " + selItem + " pack was found. Do you want to update now?", "New update found", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                            {
                                progressBarDownload.Style = ProgressBarStyle.Blocks;
                                InstallPack(selItem, true);
                            }
                            else
                            {
                                if (MessageBox.Show("You did not update your game pack. Do you want to launch the game anyway?", "Launch game anyway?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                                {
                                    progressBarDownload.Style = ProgressBarStyle.Blocks;
                                    StartGame(selItem);
                                }
                            }
                        }
                        else
                        {
                            progressBarDownload.Style = ProgressBarStyle.Blocks;
                            StartGame(selItem);
                        }

                        progressBarDownload.Style = ProgressBarStyle.Blocks;
                        panelDownload.Visible = false;

                    }
                    else
                    {
                        //download and install update  
                        InstallPack(selItem, true);
                    }

                    //eigher incorrectly installed pack (manually) or not installed automatic update pack

                    //check name for SupportetAutoUpdatePack
                    //download and install SupportetAutoUpdatePack
                }
                else
                {
                    if (minecraftJarFound)
                    {
                        StartGame(selItem);
                    }
                    else
                        MessageBox.Show("Incorrectly installed Minecraft pack. Use the format: " + Environment.NewLine + _packDir + "\\<PackName>\\.Minecraft\\bin\\minefraft.jar", "Incorrect minecraft installation detected", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    //throw new Exception("incorrectly installed Minecraft pack. Use the format " + _packDir + "\\<PackName>\\.Minecraft\\bin\\minefraft.jar");
                }
            }
            catch (Exception ex)
            {
                ErrorReporting(ex, false);
                //MessageBox.Show(string.Format("Login in failed. Error: " + ex.Message));
            }
            finally
            {
                progressBarDownload.Style = ProgressBarStyle.Blocks;
                panelDownload.Visible = false;
            }
        }

        private void checkBoxRememberLoginInfo_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!_loadingSettings)
                {
                    if (checkBoxRememberLoginInfo.Checked)
                    {
                        _formOptions.Settings.Username = textBoxUsername.Text;
                        _formOptions.Settings.Password = textBoxPassword.Text;
                        //_formOptions.Settings.RememberLogin = true;
                    }
                    else
                    {
                        _formOptions.Settings.Username = "";
                        _formOptions.Settings.Password = "";
                    }
                    _formOptions.Settings.RememberLogin = checkBoxRememberLoginInfo.Checked;
                }
            }
            catch (Exception ex)
            {
                ErrorReporting(ex, false);
            }
        }


        

        //Cross thread modifier methods

        /// <summary>
        /// Designed to be called from a seperate thread from the main thread (multi threading)
        /// </summary>
        /// <param name="value"></param>
        private void SetAndShowNews(string news)
        {
            try
            {
                if (!CloseAllThreads) 
                    this.Invoke(new Action(delegate()
                        {
                            _formNews = new FormNews(news);
                            _formNews.Show();
                            _formNews.Focus();
                        }));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Designed to be called from a seperate thread from the main thread (multi threading)
        /// </summary>
        /// <param name="value"></param>
        private void SetDownloadPanelVisibility(bool value)
        {
            try
            {
                if (!CloseAllThreads) 
                    this.Invoke(new Action(delegate()
                        {
                            this.panelDownload.Visible = value;
                            this.groupBoxLogin.Enabled = !value;
                            this.groupBoxServerStatus.Enabled = !value;
                            this.groupBoxTwitter.Enabled = !value;
                        }));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Designed to be called from a seperate thread from the main thread (multi threading)
        /// </summary>
        /// <param name="value"></param>
        private void SetDownloadCancelButtonVisibility(bool value)
        {
            try
            {
                if(!CloseAllThreads)                    
                    this.Invoke(new Action(delegate() { this.buttonDownloadCancel.Visible = value; }));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Designed to be called from a seperate thread from the main thread (multi threading)
        /// </summary>
        /// <param name="value"></param>
        /// 
        private void SetDownloadLabelText(string text)
        {
            SetDownloadLabelText(text, "", "");
        }
        
        private void SetDownloadLabelText(string text, string speed, string progress)
        {
            try
            {
                if (!CloseAllThreads) 
                    this.Invoke(new Action(delegate()
                                                {
                                                    this.labelDownload.Text = text;
                                                    this.labelDownloadProgress.Text = progress;
                                                    this.labelDownloadSpeed.Text = speed;
                                                }));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetDownloadLabelSpeedAndProgressText(string speed, string progress)
        {
            try
            {
                if (!CloseAllThreads) 
                    this.Invoke(new Action(delegate()
                                                {
                                                    this.labelDownloadProgress.Text = progress;
                                                    this.labelDownloadSpeed.Text = speed;
                                                }));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Designed to be called from a seperate thread from the main thread (multi threading)
        /// </summary>
        /// <param name="value"></param>
        private void SetDownloadProgressbarProgress(int value)
        {
            try
            {
                if (!CloseAllThreads) 
                    this.Invoke(new Action(delegate() { this.progressBarDownload.Value = value; }));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Designed to be called from a seperate thread from the main thread (multi threading)
        /// </summary>
        /// <param name="value"></param>
        private void SetDownloadProgressbarMarqueueStyle(ProgressBarStyle style)
        {
            try
            {
                if (!CloseAllThreads) 
                    this.Invoke(new Action(delegate() { this.progressBarDownload.Style = style; }));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        //private void DownloadFileFinished()
        //{
        //    panelDownload.Visible = false;
        //    //do more stuff
        //}

        //private void DownloadFileCancelled()
        //{
        //    panelDownload.Visible = false;
        //    MessageBox.Show("The download have been cancelled.", "Download cancelled");
        //}




        //code help thanks to http://www.devtoolshed.com/content/c-download-file-progress-bar
        //private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    Uri url = new Uri((e.Argument as BackgroundWorkerArgumentWrapper).Url);            
        //    string filename = url.OriginalString.Substring(url.OriginalString.LastIndexOf("/") + 1, url.OriginalString.Length - url.OriginalString.LastIndexOf("/") - 1);            

        //    // first, we need to get the exact size (in bytes) of the file we are downloading            
        //    System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
        //    SetAllowUnsafeHeaderParsing20();
        //    System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
        //    response.Close();

        //    // gets the size of the file in bytes
        //    Int64 iSize = response.ContentLength;

        //    // keeps track of the total bytes downloaded so we can update the progress bar
        //    Int64 iRunningByteTotal = 0;

        //    // use the webclient object to download the file
        //    using (System.Net.WebClient client = new System.Net.WebClient())            
        //    {
        //        // open the file at the remote URL for reading
        //        using (System.IO.Stream streamRemote = client.OpenRead(url))
        //        {
        //            // using the FileStream object, we can write the downloaded bytes to the file system
        //            using (Stream streamLocal = new FileStream(Application.StartupPath + "\\Minecraft Packs\\" + filename, FileMode.Create, FileAccess.Write, FileShare.None))                    
        //            {
        //                // loop the stream and get the file into the byte buffer
        //                int iByteSize = 0;
        //                byte[] byteBuffer = new byte[iSize];
        //                while ((iByteSize = streamRemote.Read(byteBuffer, 0, byteBuffer.Length)) > 0)
        //                {
        //                    //aborts the download if a cancel request have been made
        //                    if (backgroundWorker1.CancellationPending)
        //                    {
        //                        DownloadFileCancelled();
        //                        break;
        //                    }

        //                    // write the bytes to the file system at the file path specified
        //                    streamLocal.Write(byteBuffer, 0, iByteSize);
        //                    iRunningByteTotal += iByteSize;

        //                    // calculate the progress out of a base "100"
        //                    double dIndex = (double)(iRunningByteTotal);
        //                    double dTotal = (double)byteBuffer.Length;
        //                    double dProgressPercentage = (dIndex / dTotal);
        //                    int iProgressPercentage = (int)(dProgressPercentage * 100);

        //                    // update the progress bar
        //                    backgroundWorker1.ReportProgress(iProgressPercentage);
        //                }

        //                // clean up the file stream
        //                streamLocal.Close();
        //            }

        //            // close the connection to the remote server
        //            streamRemote.Close();
        //        }
        //    }

        //    //doing this stuff here dont seem to be the right place for it, but Its the best place i could think of

        //    try
        //    {
        //        if (new FileInfo(filename).Extension.ToLower() == ".zip")                
        //        {
        //            if ((e.Argument as BackgroundWorkerArgumentWrapper).SourcePack == "Terrafirma Craft")
        //            {
        //                this.Invoke(new Action(delegate() { labelDownload.Text = "Extracting file " + filename; }));
        //                //labelDownload.Text = "Extracting file " + filename;

        //                ExtractZip(Application.StartupPath + "\\Minecraft Packs\\" + filename, Application.StartupPath + "\\Minecraft Packs\\Terrafirma Craft");
        //            }
        //            else if ((e.Argument as BackgroundWorkerArgumentWrapper).SourcePack == "Terrafirma Craft")
        //            {
        //            }
        //            else if ((e.Argument as BackgroundWorkerArgumentWrapper).SourcePack == "Industrial Rage")
        //            {
        //            }
        //        }
        //        else if ((e.Argument as BackgroundWorkerArgumentWrapper).SourcePack == "Vanilla")
        //        {
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);                
        //    }

        //}

        //private void ExtractZip(string source, string destination)
        //{
        //    //double dIndex = (double)(iRunningByteTotal);
        //    //double dTotal = (double)byteBuffer.Length;
        //    double dProgressPercentage;// = (dIndex / dTotal);
        //    //int iProgressPercentage = (int)(dProgressPercentage * 100);
            
        //    using (ZipFile zip1 = ZipFile.Read(source))
        //    {
        //        //foreach (ZipEntry e in zip1)
        //        for(int i=0;i < zip1.Count;i++)
        //        {
        //            dProgressPercentage = (i / zip1.Count);
        //            this.Invoke(new Action(delegate() { progressBarDownload.Value = (int)(dProgressPercentage * 100); }));
        //            //progressBarDownload.Value = (int)(dProgressPercentage * 100);
        //            zip1[i].Extract(destination, ExtractExistingFileAction.OverwriteSilently);                    
        //        }
        //    }
        //}

        //private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    progressBarDownload.Value = e.ProgressPercentage;
        //    //could add 'remaining time' calculation here
        //}

        //private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    DownloadFileFinished();
        //}

        
        //private void UpdatePackSelect()
        //{
        //    if (!File.Exists(_packDir + "\\" + comboBoxPackSelect.SelectedItem.ToString() + "\\bin\\minecraft.jar"))
        //    {
        //        string selItem = comboBoxPackSelect.SelectedItem.ToString();
        //        if (selItem == _formOptions.PackIRName || selItem == _formOptions.PackERName || selItem == _formOptions.PackTFCRName || selItem == _formOptions.PackVanillaName)
        //        {
        //            buttonLogin.Enabled = true;
        //            buttonLogin.Text = "Install Pack";
        //        }
        //        else
        //        {
        //            buttonLogin.Enabled = false;
        //            buttonLogin.Text = "Login";
        //        }
        //    }
        //    else
        //    {
        //        buttonLogin.Text = "Login";
        //        buttonLogin.Enabled = true;
        //    }
        //}

        



        //public void Ping(string pingAddress, System.Windows.Forms.Label resultLabel, System.Windows.Forms.ProgressBar progressBar)
        //{
        //    //System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        //    //this.Invoke(new Action(delegate() { this.labelUpdatingInformation.Visible = true; }));
        //    this.Invoke(new Action(delegate() { resultLabel.Visible = false; }));
        //    this.Invoke(new Action(delegate() { progressBar.Visible = true; }));

        //    //start pinging
        //    Thread.Sleep(5000);

        //    this.Invoke(new Action(delegate() { resultLabel.ForeColor = Color.Red; }));
        //    this.Invoke(new Action(delegate() { resultLabel.Text = "Offline"; }));
        //    this.Invoke(new Action(delegate() { resultLabel.Visible = true; }));
        //    this.Invoke(new Action(delegate() { progressBar.Visible = false; }));
        //    Thread.CurrentThread.Abort();
        //}

        //private void StartCheck()
        //{
        //    progressBarIR.Visible = true;
        //    progressBarER.Visible = true;
        //    progressBarMinecraftdotnet.Visible = true;
        //    progressBarMinecraftLoginServers.Visible = true;
        //    progressBarMining.Visible = true;
        //    progressBarTFCR.Visible = true;

        //    //labelERResult.Text = "Offline";
        //    //labelERResult.ForeColor = Color.Red;
        //    //labelERResult.Visible = true;
        //    //labelIRResult.Text = "Offline";
        //    //labelIRResult.ForeColor = Color.Red;
        //    //labelIRResult.Visible = true;
        //    //labelMinecraftdotnetResult.Text = "Offline";
        //    //labelMinecraftdotnetResult.ForeColor = Color.Red;
        //    //labelMinecraftdotnetResult.Visible = true;
        //    //labelMinecraftLoginServersResult.Text = "Offline";
        //    //labelMinecraftLoginServersResult.ForeColor = Color.Red;
        //    //labelMinecraftLoginServersResult.Visible = true;
        //    //labelMiningResult.Text = "Offline";
        //    //labelMiningResult.ForeColor = Color.Red;
        //    //labelMiningResult.Visible = true;
        //    //labelTFCRResult.Text = "Offline";
        //    //labelTFCRResult.ForeColor = Color.Red;
        //    //labelTFCRResult.Visible = true;
        //    //timer1.Enabled = false;
        //}

        //private void timer1_Tick(object sender, EventArgs e)
        //{
            
        //}

        //private string GetUrlFromMCPack(SupportetAutoUpdatePack pack)
        //{
        //    if (pack == SupportetAutoUpdatePack.IR)
        //    {
        //        return _formOptions.DownloadLinkIR;
        //    }
        //    else if (pack == SupportetAutoUpdatePack.ER)
        //    {
        //        return _formOptions.DownloadLinkER;
        //    }
        //    else if (pack == SupportetAutoUpdatePack.TFCR)
        //    {
        //        return _formOptions.DownloadLinkTFCR;
        //    }
        //    else if (pack == SupportetAutoUpdatePack.Vanilla)
        //    {
        //        return _formOptions.DownloadLinkVanillaMC;
        //    }
        //    return "";
        //}



        ///// <summary>
        ///// This method controls the names of the default packs. The folders are named by these values
        ///// </summary>
        ///// <param name="pack"></param>
        ///// <returns></returns>
        //public string packEnumToString(SupportetAutoUpdatePack pack)
        //{
        //    switch (pack)
        //    {
        //        case SupportetAutoUpdatePack.IR:
        //            return "Industrial Rage";
        //        case SupportetAutoUpdatePack.ER:
        //            return "Endless Rage";
        //        case SupportetAutoUpdatePack.TFCR:
        //            return "Terrafirma Rage";
        //        case SupportetAutoUpdatePack.Vanilla:
        //            return "Vanilla";
        //        case SupportetAutoUpdatePack.Other: 
        //            return "";
        //    }
        //    return "";
        //}

    }
}

