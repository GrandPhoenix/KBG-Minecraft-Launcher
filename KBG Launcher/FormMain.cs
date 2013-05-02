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
//using Ionic.Zip;
//using System.Configuration;
using Microsoft.Win32;
//using System.Security.Cryptography;
//using System.Security.Cryptography;
//using System.Xml;
//using System.Security.Cryptography.Xml;
using System.Collections;
using System.Web;
using System.Security.Cryptography;
using System.Xml;
using System.Windows.Shell;
using Hammock.Authentication.OAuth;
using Hammock;
using Ionic.Zip;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Globalization;



namespace KBG_Launcher
{
    public partial class FormMain : Form
    {
        public enum SupportetAutoUpdatePack { IR = 0, ER, TFCR, Vanilla };        

        private List<TweetItem> _TweetList = new List<TweetItem>();
        private FormOptions _formOptions;
        private FormNews _formNews;
        private FormError _formError = new FormError();
        private FormCredits _formCredits = new FormCredits();


        private bool _updateFinished = false;
        private bool _useTaskbarProgressBar = false;
        private bool _offlineMode = false;
        private bool _forceNoLoginConnection = false;
        
        
        private string _globalPackDir;
        private bool _abortDownload = false;
        private bool _loadingSettings = false;
        public bool CloseAllThreads = false;
        private DateTime LastDownloadTickTime;
        private int ByteDownloadedUpToPreviousTick = 0;
        private double LastDownloadPercentageTick = 0;
        private bool DownloadingFile = false;
        //public enum SourcePack {IR=0,ER,TFCR,Vanilla};

        Thread TweetThread;
        Thread pingIRThread;
        Thread pingERThread;
        Thread pingKBGEventThread;
        Thread pingTFCRThread;
        Thread pingMinecraftDotNetThread;
        Thread pingMinecraftLoginServersThread;
        Thread packThread;
        Thread updateThread;


        public bool OfflineMode
        {
            get { return _offlineMode; }
        }

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
            Stream s = a1.GetManifestResourceStream(typeof(FormMain), "Lib.Ionic.Zip.dll");
            byte[] block = new byte[s.Length];
            s.Read(block, 0, block.Length);
            Assembly a2 = Assembly.Load(block);
            return a2;
        }

        static System.Reflection.Assembly ResolverHammock(object sender, ResolveEventArgs args)
        {
            Assembly a1 = Assembly.GetExecutingAssembly();
            Stream s = a1.GetManifestResourceStream(typeof(FormMain), "Lib.Hammock.dll");
            byte[] block = new byte[s.Length];
            s.Read(block, 0, block.Length);
            Assembly a2 = Assembly.Load(block);
            return a2;
        }

        //constructor

        public FormMain()
        {
            //StreamWriter sw = new StreamWriter(new FileStream("Debug2.txt", FileMode.Create));
            
            try
            {
                AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(ResolverDotNetZip);
                AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(ResolverHammock);  

                string methodProgress = ""; //this is used to find out where in the method the error occurred

                _useTaskbarProgressBar = Environment.OSVersion.Version.Build >= 6000;

                _offlineMode = getCommandLineArgs().Contains("/Offline");
                _forceNoLoginConnection = getCommandLineArgs().Contains("/NoLoginConnection");
                //else
                //    _offlineMode = !IsNetworkAvailable();
                //_offlineMode = true; //debugging overwrite
                                
                //Console.SetOut(sw);
                ////Console.SetOut(new StreamWriter(new FileStream(Application.StartupPath + "\\debug.txt", FileMode.Create)));
                //Console.WriteLine("FormMain start");
                //Debug.WriteLine("Formmain start");
                //StreamWriter sw2 = new StreamWriter(new FileStream("Debug3.txt", FileMode.Create));
                //sw2.Close();

                //If startet as KBG minecraft launcher2.exe, then delete the first exe, copy second to first, and restart the first again
                if (getCommandLineArgs().Contains("/UpdateRestart"))
                {
                    //sw2 = new StreamWriter(new FileStream("DebugUpdateRestart.txt", FileMode.Create));
                    //sw2.Close();
                    //Console.WriteLine("/UpdateRestart start");

                    if (Application.ExecutablePath == Application.StartupPath + "\\KBG Minecraft Launcher2.exe")
                    {
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

                            File.Copy(Application.StartupPath + "\\KBG Minecraft Launcher2.exe", Application.StartupPath + "\\KBG Launcher.exe", true);

                            methodProgress = "UpdateRestart - Application.Exiting";

                            Application.Exit();
                            methodProgress = "UpdateRestart - starting \\KBG Launcher.exe..";
                            Process.Start(Application.StartupPath + "\\KBG Launcher.exe", "/UpdateFinished " + Process.GetCurrentProcess().Id.ToString());
                            methodProgress = "UpdateRestart - Environment.Exit";
                            Environment.Exit(0);
                            methodProgress = "UpdateRestart - GC.Collect";
                            GC.Collect();
                        }
                        methodProgress = "UpdateRestart - end";
                    }

                    if (Application.ExecutablePath == Application.StartupPath + "\\KBG Launcher2.exe")
                    {
                        //should be startet as KBG Launcher2.exe
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

                        if (File.Exists(Application.StartupPath + "\\KBG Launcher2.exe"))
                        {
                            methodProgress = "UpdateRestart - preparing to copy";

                            File.Copy(Application.StartupPath + "\\KBG Launcher2.exe", Application.StartupPath + "\\KBG Launcher.exe", true);

                            methodProgress = "UpdateRestart - Application.Exiting";

                            Application.Exit();
                            methodProgress = "UpdateRestart - starting \\KBG Launcher.exe..";
                            Process.Start(Application.StartupPath + "\\KBG Launcher.exe", "/UpdateFinished " + Process.GetCurrentProcess().Id.ToString());
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
                    //sw2 = new StreamWriter(new FileStream("DebugUpdateFinished.txt", FileMode.Create));
                    //sw2.Close();
                    Console.WriteLine("/UpdateFinished start");
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
                    //removing old file with old filename scheme
                    methodProgress = "UpdateFinished - Deleting KBG Minecraft Launcher2.exe";
                    if (File.Exists(Application.StartupPath + "\\KBG Minecraft Launcher2.exe"))
                        File.Delete(Application.StartupPath + "\\KBG Minecraft Launcher2.exe");

                    //cleaning up after namechange update
                    methodProgress = "UpdateFinished - Deleting KBG Minecraft Launcher.exe";
                    if (File.Exists(Application.StartupPath + "\\KBG Minecraft Launcher.exe"))
                        File.Delete(Application.StartupPath + "\\KBG Minecraft Launcher.exe");

                    //removing old file with new filename scheme
                    methodProgress = "UpdateFinished - Deleting KBG Launcher2.exe";
                    if (File.Exists(Application.StartupPath + "\\KBG Launcher2.exe"))
                        File.Delete(Application.StartupPath + "\\KBG Launcher2.exe");

                    methodProgress = "UpdateFinished - Update finished";
                    _updateFinished = true;
                    methodProgress = "UpdateFinished - end";
                }


                //sw2 = new StreamWriter(new FileStream("DebugAssemblyRecolver.txt", FileMode.Create));
                //Console.SetOut(sw2);
                //Console.WriteLine("AssemblyRecolver");

                //AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolver);    

                //static System.Reflection.Assembly ResolverDotNetZip(object sender, ResolveEventArgs args)
                //{
                //    Assembly a1 = Assembly.GetExecutingAssembly();
                //    Stream s = a1.GetManifestResourceStream(typeof(FormMain), "Ionic.Zip.dll");
                //    byte[] block = new byte[s.Length];
                //    s.Read(block, 0, block.Length);
                //    
                //    return Assembly.Load(block);
                //}




                     
                //Console.WriteLine("InitializeComponent");
                //sw2 = new StreamWriter(new FileStream("DebugInitializeComponent.txt", FileMode.Create));
                //sw2.Close();
                InitializeComponent();
                
            }
            catch (UnauthorizedAccessException ex)
            {
                //Console.WriteLine("exception catch: " + ex.Message);
                ex.Data.Add("FormMain() - CommandLineInformation", getCommandLineArgs());
                ex.Data.Add("FormMain() - ExecutablePath", Application.ExecutablePath);
                string message = string.Format("The program encountered a situation where it did not have enough permissions to do its work in its own folder and cannot continue to function.{0}Please make sure you put the {1} file in a folder with more permissions!{0}{0}I suggest a place like c:\\Games\\Minecraft", Environment.NewLine, new FileInfo(Application.ExecutablePath).Name);
                MessageBox.Show(message, "Insufficiant permissions", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ErrorReporting(ex, true);
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
                ex.Data.Add("FormMain() - CommandLineInformation", getCommandLineArgs());
                ex.Data.Add("FormMain() - ExecutablePath", Application.ExecutablePath);
                ex.Data.Add("FormMain() - KBG Minecraft Launcher.exe exists?", File.Exists(Application.StartupPath + "\\KBG Minecraft Launcher.exe").ToString());
                ex.Data.Add("FormMain() - KBG Minecraft Launcher2.exe exists?", File.Exists(Application.StartupPath + "\\KBG Minecraft Launcher2.exe").ToString());

                ErrorReporting(ex, true);
            }
            //finally
            //{
            //    sw.Close();
            //}
        }
        


        //static System.Reflection.Assembly AssemblyResolver(object sender, ResolveEventArgs args)
        //{
        //    try
        //    {
        //        StreamWriter sw2 = new StreamWriter(new FileStream("Just ignore this file.txt", FileMode.Create));
        //        //sw2.Close();

        //        string resourceName = new AssemblyName(args.Name).Name + ".dll";
        //        string resource = "";
        //        //sw2 = new StreamWriter(new FileStream("DebugLine2 - " + resourceName + ".txt", FileMode.Create));
        //        //sw2.Close();
        //        //string resource = Array.Find(this.GetType().Assembly.GetManifestResourceNames(), element => element.EndsWith(resourceName));
        //        //sw2 = new StreamWriter(new FileStream("DebugBeforeGetManifestResourceNames.txt", FileMode.Create)); 
        //        //Console.WriteLine("");

        //        string[] resourceNames = typeof(FormMain).Assembly.GetManifestResourceNames();
        //        //sw2 = new StreamWriter(new FileStream("DebugAfterGetManifestResourceNames.txt", FileMode.Create)); 
                
        //        foreach (string str in resourceNames)
        //        {
        //            if (str.EndsWith(resourceName))
        //            {
        //                resource = str;
        //                //sw2 = new StreamWriter(new FileStream("DebugResource - " + str + ".txt", FileMode.Create)); 
        //                break;
        //            }
        //        }
        //        //sw2 = new StreamWriter(new FileStream("DebugAfterForeach.txt", FileMode.Create)); 
        //        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource))
        //        {
        //            //sw2 = new StreamWriter(new FileStream("DebugLine3.txt", FileMode.Create));
        //            //sw2.Close();
        //            Byte[] assemblyData = new Byte[stream.Length];

        //            //sw2 = new StreamWriter(new FileStream("DebugLine4.txt", FileMode.Create));
        //            //sw2.Close();
        //            stream.Read(assemblyData, 0, assemblyData.Length);

        //            //sw2 = new StreamWriter(new FileStream("DebugLine5.txt", FileMode.Create));
        //            //sw2.Close();
        //            return Assembly.Load(assemblyData);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

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
                ex.Data.Add("ParseDateTime() - date", date);
                throw ex;
            }
        }

        //Testing for an Internet connection. Thanks to http://www.dreamincode.net/forums/topic/71263-using-the-ping-class-in-c%23/
        //private static bool HasConnection()
        //{
        //    try
        //    {
        //        //instance of our ConnectionStatusEnum
        //        ConnectionStatusEnum state = 0;

        //        //call the API
        //        InternetGetConnectedState(ref state, 0);

        //        //check the status, if not offline and the returned state
        //        //isnt 0 then we have a connection
        //        if (((int)ConnectionStatusEnum.INTERNET_CONNECTION_OFFLINE & (int)state) != 0)
        //        {
        //            //return true, we have a connection
        //            return false;
        //        }
        //        //return false, no connection available
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //thanks to http://stackoverflow.com/questions/520347/c-sharp-how-do-i-check-for-a-network-connection
        /// <summary>
        /// Indicates whether any network connection is available
        /// Filter connections below a specified speed, as well as virtual network cards.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if a network connection is available; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNetworkAvailable()
        {
            return IsNetworkAvailable(0);
        }

        /// <summary>
        /// Indicates whether any network connection is available.
        /// Filter connections below a specified speed, as well as virtual network cards.
        /// </summary>
        /// <param name="minimumSpeed">The minimum speed required. Passing 0 will not filter connection using speed.</param>
        /// <returns>
        ///     <c>true</c> if a network connection is available; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNetworkAvailable(long minimumSpeed)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
                return false;

            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                // discard because of standard reasons
                if ((ni.OperationalStatus != OperationalStatus.Up) ||
                    (ni.NetworkInterfaceType == NetworkInterfaceType.Loopback) ||
                    (ni.NetworkInterfaceType == NetworkInterfaceType.Tunnel))
                    continue;

                // this allow to filter modems, serial, etc.
                // I use 10000000 as a minimum speed for most cases
                if (ni.Speed < minimumSpeed)
                    continue;

                // discard virtual cards (virtual box, virtual pc, etc.)
                if ((ni.Description.IndexOf("virtual", StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (ni.Name.IndexOf("virtual", StringComparison.OrdinalIgnoreCase) >= 0))
                    continue;

                // discard "Microsoft Loopback Adapter", it will not show as NetworkInterfaceType.Loopback but as Ethernet Card.
                if (ni.Description.Equals("Microsoft Loopback Adapter", StringComparison.OrdinalIgnoreCase))
                    continue;

                return true;
            }
            return false;
        }
        
        //code help thanks to http://social.msdn.microsoft.com/forums/en-US/netfxnetcom/thread/ff098248-551c-4da9-8ba5-358a9f8ccc57/
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
                ex.Data.Add("GetCommandLineArgsValue() - ArgName", argName);
                throw ex;
            }
        }

        static List<string> getCommandLineArgs()
        {
            try
            {
                Queue<string> args = new Queue<string>(Environment.GetCommandLineArgs());

                args.Dequeue(); // args[0] is always exe path/filename
                //return string.Join(" ", args.ToArray());
                return new List<string>(args.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //private bool IsCommandArgOfflineMode()
        //{
        //    try
        //    {
        //        Queue<string> args = new Queue<string>(Environment.GetCommandLineArgs());

        //        args.Dequeue(); // args[0] is always exe path/filename
        //        List<string> argsList = new List<string>(args.ToArray());
        //        return argsList.Contains("/OfflineMode");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        



        

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
                ex.Data.Add("CopyAll() - source", source.FullName);
                ex.Data.Add("CopyAll() - target", target.FullName);
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

        /// <summary>
        /// The set of characters that are unreserved in RFC 2396 but are NOT unreserved in RFC 3986.
        /// code thanks to http://stackoverflow.com/questions/846487/how-to-get-uri-escapedatastring-to-comply-with-rfc-3986
        /// </summary>
        private static readonly string[] UriRfc3986CharsToEscape = new[] { "!", "*", "'", "(", ")" };

        /// <summary>
        /// Escapes a string according to the URI data string rules given in RFC 3986.
        /// </summary>
        /// <param name="value">The value to escape.</param>
        /// <returns>The escaped value.</returns>
        /// <remarks>
        /// The <see cref="Uri.EscapeDataString"/> method is <i>supposed</i> to take on
        /// RFC 3986 behavior if certain elements are present in a .config file.  Even if this
        /// actually worked (which in my experiments it <i>doesn't</i>), we can't rely on every
        /// host actually having this configuration element present.
        /// code thanks to http://stackoverflow.com/questions/846487/how-to-get-uri-escapedatastring-to-comply-with-rfc-3986
        /// </remarks>
        internal static string EscapeUriDataStringRfc3986(string value)
        {
            // Start with RFC 2396 escaping by calling the .NET method to do the work.
            // This MAY sometimes exhibit RFC 3986 behavior (according to the documentation).
            // If it does, the escaping we do that follows it will be a no-op since the
            // characters we search for to replace can't possibly exist in the string.
            StringBuilder escaped = new StringBuilder(Uri.EscapeDataString(value));

            // Upgrade the escaping to RFC 3986, if necessary.
            for (int i = 0; i < UriRfc3986CharsToEscape.Length; i++)
            {
                escaped.Replace(UriRfc3986CharsToEscape[i], Uri.HexEscape(UriRfc3986CharsToEscape[i][0]));
            }

            // Return the fully-RFC3986-escaped string.
            return escaped.ToString();
        }


        //general methods

        private void GenerateTweetList()
        {
            try
            {
                if (HasInternetConnection())
                {
                    labelNoTwitterConnection.Visible = false;
                    progressBarTwitter.Visible = true;
                    TweetThread = new Thread(new ThreadStart(this.getTwitterFeeds));
                    progressBarTwitter.Visible = true;
                    TweetThread.Start();
                    while (!TweetThread.IsAlive) ;
                }
                else
                {
                    labelNoTwitterConnection.Visible = true;
                    progressBarTwitter.Visible = false;
                }
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
                    rtf += "\\cf0\\fs17 " + _TweetList[i].Text;
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
                if (HasInternetConnection())
                {
                    //209.105.230.50:25568
                    ServerAccessClass pingIR = new ServerAccessClass("69.195.149.98", 25566, labelIRResult, progressBarIR, this);
                    ServerAccessClass pingER = new ServerAccessClass("69.195.149.98", 25567, labelERResult, progressBarER, this);
                    ServerAccessClass pingKBGEvent = new ServerAccessClass("69.195.149.98", 25569, labelKBGEventResult, progressBarKBGEvent, this);
                    ServerAccessClass pingTFCR = new ServerAccessClass("69.195.149.98", 25568, labelTFCRResult, progressBarTFCR, this);
                    ServerAccessClass pingMinecraftDotNet = new ServerAccessClass("Minecraft.Net", 80, labelMinecraftdotnetResult, progressBarMinecraftdotnet, this);
                    ServerAccessClass pingMinecraftLoginServers = new ServerAccessClass("Login.minecraft.net", 80, labelMinecraftLoginServersResult, progressBarMinecraftLoginServers, this);

                    pingIRThread = new Thread(new ThreadStart(pingIR.StartCheck));
                    pingIRThread.Start();
                    while (!pingIRThread.IsAlive) ;
                    pingERThread = new Thread(new ThreadStart(pingER.StartCheck));
                    pingERThread.Start();
                    while (!pingERThread.IsAlive) ;
                    pingKBGEventThread = new Thread(new ThreadStart(pingKBGEvent.StartCheck));
                    pingKBGEventThread.Start();
                    while (!pingKBGEventThread.IsAlive) ;
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
            XDocument doc = new XDocument();
            DateTime Tweettime = new DateTime();
            string TweetText = "";
            string TweetTextAt = "";
            bool webclientError = false;
            try
            {
                //using (var httpClient = new HttpClient())

                using (var webClient = new WebClient())
                {
                    try
                    {
                        //string responseString = "";
                        //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlString);
                        //request.Method = "GET";
                        //request.ServicePoint.Expect100Continue = false;
                        //request.ContentType = "application/x-www-form-urlencoded";

                        //using (WebResponse response = request.GetResponse())
                        //{
                        //    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        //    {
                        //        responseString = reader.ReadToEnd();
                        //    }
                        //}


                        webClient.Encoding = Encoding.UTF8;
                        webClient.Proxy = null;
                        doc = XDocument.Parse(webClient.DownloadString(url));
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message == "The remote server returned an error: (400) Bad Request.")
                            MessageBox.Show("It seems like Twitters Rate Limit has been reached for your network." + Environment.NewLine + "Twitter only allows for 150 requests pr. hour", "Twitter request limit reached", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show("The server returned an error while getting twitter feeds." + Environment.NewLine + "Error: " + ex.Message);
                            
                        webclientError = true;
                    }

                    if (!webclientError)
                    {
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

                            if (TSpan.TotalHours < 1)
                            {
                                _TweetList.Add(new TweetItem(TweetTextAt, TweetText, Math.Floor(TSpan.TotalMinutes).ToString() + " minutes ago"));
                            }
                            else if (TSpan.TotalHours < 24)
                            {
                                _TweetList.Add(new TweetItem(TweetTextAt, TweetText, Math.Floor(TSpan.TotalHours).ToString() + " hours ago"));
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
                }
                if (!CloseAllThreads)
                    this.Invoke(new Action(delegate() { this.ShowTweets(); }));
                
                //ShowTweets();                
            }
            catch (Exception ex)
            {
                ex.Data.Add("getTwitterFeeds() - urlString", url);
                ex.Data.Add("getTwitterFeeds() - Tweettime", Tweettime.ToUniversalTime());
                ex.Data.Add("getTwitterFeeds() - TweetText", TweetText);
                ex.Data.Add("getTwitterFeeds() - TweetTextAt", TweetTextAt);

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

        private bool HasInternetAndLoginConnection()
        {
            try
            {
                if (HasInternetConnection())
                {
                    if (_forceNoLoginConnection)
                        return false;
                    else
                    {
                        //can connect to mojang servers?
                        //ServerAccessClass pingMinecraftLoginServers = new ServerAccessClass("Login.minecraft.net", 80, labelMinecraftLoginServersResult, progressBarMinecraftLoginServers, this);
                        System.Net.Sockets.TcpClient client = new TcpClient();
                        try
                        {
                            client.Connect("Login.minecraft.net", 80);
                            //connected
                            client.Close();
                            return true;
                        }
                        catch (SocketException ex)
                        {
                            client.Close();
                            return false;
                        }
                    }
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool HasInternetConnection()
        {
            try
            {
                if (_offlineMode)
                    return false;
                else
                {
                    //bool returnValue = IsNetworkAvailable();
                    //_offlineMode = !returnValue;
                    //return returnValue;
                    return IsNetworkAvailable();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CheckForClientUpdate()
        {
            Int64 iSize = 0;
            Int64 iRunningByteTotal = 0;

            try
            {
                SetDownloadLabelTextMain("Checking for client updates");
                SetDownloadProgressbarMarqueueStyle(ProgressBarStyle.Marquee);
                //buttonDownloadCancel.Visible = true;
                SetDownloadPanelVisibility(true);
                if (HasInternetConnection())
                {
#if(DEBUG)
                    xmlVersionInfo local = _formOptions.GetClientVersion();
                    xmlVersionInfo remote = _formOptions.GetClientUpdateInfo();
                    MessageBox.Show(string.Format("local: {0}.{1}.{2}.{3} - remote: {4}.{5}.{6}.{7}", local.VersionMajor, local.VersionMinor, local.VersionRevision, local.VersionPack, remote.VersionMajor, remote.VersionMinor, remote.VersionRevision, remote.VersionPack));
#endif

                    if (VersionInfo1LargerThenInfo2(_formOptions.GetClientUpdateInfo(), _formOptions.GetClientVersion()))
                    {
                        if (MessageBox.Show("A new version of the client is avalible. Update now?", "New update available", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                        {

                            SetDownloadLabelTextMain("Updating client");
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
                                    using (Stream streamLocal = new FileStream(Application.StartupPath + "\\KBG Launcher2.exe", FileMode.Create, FileAccess.Write, FileShare.None))
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
                            Process.Start(Application.StartupPath + "\\KBG Launcher2.exe", "/UpdateRestart " + Process.GetCurrentProcess().Id.ToString());
                            Environment.Exit(0);
                            Application.Exit();
                            GC.Collect();
                        }
                    }
                }
                SetDownloadPanelVisibility(false);                
            }
            catch (Exception ex)
            {
                SetDownloadPanelVisibility(false);
                //List<string> information = new List<string>();
                //information.Add("RemoteFileSize = " + RemoteFileSize.ToString());
                //information.Add("iRunningByteTotal = " + iRunningByteTotal.ToString());
                //information.Add("Error: " + ex.Message);
                ex.Data.Add("CheckForClientUpdate() - RemoteFileSize", iSize.ToString());
                ex.Data.Add("CheckForClientUpdate() - iRunningByteTotal", iRunningByteTotal.ToString());

                ErrorReporting(ex, false);

                //MessageBox.Show("Something Failed while updating the client. Error: " + ex.Message);
            }
        }

        private void InstallPackStarter(string packName, bool startGameAfterInstall)
        {
            try
            {

                if (HasInternetConnection())
                {
                    ThreadParameterPack TPP = new ThreadParameterPack(packName, startGameAfterInstall);
                    //TPP.SelectedItem = packName;
                    //TPP.StartGameAfterInstall = startGameAfterInstall;

                    //if (startGameAfterInstall)
                    //{
                    //    packThread = new Thread(new ParameterizedThreadStart(DownloadInstallPackAndStartGame));
                    //}
                    //else
                    //{
                    //    packThread = new Thread(new ParameterizedThreadStart(DownloadAndInstallPack));
                    //}
                    packThread = new Thread(new ParameterizedThreadStart(InstallPack));
                    packThread.IsBackground = true;

                    packThread.Start(TPP);
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
                ex.Data.Add("InstallPackStarter() - packName", packName);
                ex.Data.Add("InstallPackStarter() - startGameAfterInstall", startGameAfterInstall.ToString());
                throw ex;
            }

        }


        private void InstallPack(object tpp)
        {
            ThreadParameterPack TPP = (ThreadParameterPack)tpp;
            string urlString = "";
            string urlInjectString = "";
            string filename = "";
            string filenameInject = "";
            string packDir = "";
            //bool returnValue = false;
            string MethodProgress = "";
            xmlVersionInfo versionInfo;
            bool processSucceeded = true;

            try
            {
                SetDownloadLabelTextMain("Preparing Install");
                SetDownloadProgressbarProgress(0);
                SetDownloadPanelVisibility(true);

                packDir = _globalPackDir + "\\" + TPP.SelectedItem;

                //DownloadAndInstallPackWorker(TPP.SelectedItem);
                versionInfo = _formOptions.GetVersionInfo(TPP.SelectedItem, true);

                //packName = (string)oPackName;
                //FormOptions.SupportetAutoUpdatePack pack = (FormOptions.SupportetAutoUpdatePack)oPack;     
                MethodProgress = "creating URI";
                urlString = _formOptions.PackUpdateUrl(TPP.SelectedItem);
                //urlString = new Uri("https://dl.dropbox.com/s/t6t1cjqq5isfzxh/IR.zip?token_hash=AAHWyaGx7Ql4t756Fmi1V8iKjF6imGlwr85DIE2aAKZ1Zw&dl=1"); //for debugging
                

                filename = urlString.Substring(urlString.LastIndexOf("/") + 1, urlString.Length - urlString.LastIndexOf("/") - 1);
                

                //_formNews = new FormNews(_formOptions.GetVersionInfo(packName, true).UpdateNews);
                //_formNews.Show();
                //_formNews.Focus();
                MethodProgress = "SetAndShowNews";                
                SetAndShowNews(versionInfo.UpdateNews);



                //-------------- Process Main pack file --------------
                if (TPP.SelectedItem != "Vanilla")
                {
                    urlInjectString = _formOptions.PackUpdateInjectUrl(TPP.SelectedItem);
                    //urlInjectString = new Uri("https://dl.dropbox.com/s/nx9bd3e9w03wyao/IRInject.zip?token_hash=AAF3bb0ajM7VFidjh6rY66X_cHuF9313Cy7whKSJIJNRQQ&dl=1");
                    filenameInject = urlInjectString.Substring(urlString.LastIndexOf("/") + 1, urlString.Length - urlString.LastIndexOf("/") - 1);

                    //Download file
                    MethodProgress = "Downloading file - Main pack";
                    processSucceeded = DownloadFile(urlString, _globalPackDir + "\\" + FileNameFromUri(urlString));

                    //backup files
                    if (processSucceeded)
                    {
                        MethodProgress = "Backing up Files";
                        processSucceeded = DoBackup(packDir + "\\.minecraft", packDir + "\\UpdateBackup", versionInfo);
                    }

                    //extract file
                    if (processSucceeded)
                    {
                        MethodProgress = "Extracting File";
                        processSucceeded = ExtractFile(_globalPackDir + "\\" + FileNameFromUri(urlString), packDir);
                    }

                    //restore backup files
                    if (processSucceeded)
                    {
                        MethodProgress = "Restoring backuped Files";
                        processSucceeded = DoRestoreBackup(packDir + "\\.minecraft", packDir + "\\UpdateBackup");
                    }
                }
                //-------------- Process Minecraft download/install --------------

                if (processSucceeded)
                {
                    Directory.CreateDirectory(packDir + "\\.Minecraft\\bin\\natives");
                    //download - https://s3.amazonaws.com/assets.minecraft.net/ + versionInstall from versionInfo (1_4_7/minecraft.jar)

                    if(versionInfo.InstallVersion != "")
                        DownloadFile("https://s3.amazonaws.com/assets.minecraft.net/" + versionInfo.InstallVersion, packDir + "\\.Minecraft\\bin\\minecraft.jar"); //for release
                    else
                        DownloadFile("minecraft.jar", packDir + "\\.Minecraft\\bin\\minecraft.jar"); //for release
                    //processSucceeded = DownloadFile(new Uri("https://s3.amazonaws.com/assets.minecraft.net/1_4_7/minecraft.jar"), packDir + "\\.Minecraft\\bin\\minecraft.jar"); //for debug
                }

                //download     lwjgl.jar +     jinput.jar +     lwjgl_util.jar 
                if (processSucceeded)
                    processSucceeded = DownloadFile("http://s3.amazonaws.com/MinecraftDownload/lwjgl.jar", packDir + "\\.Minecraft\\bin\\lwjgl.jar");
                
                if (processSucceeded)
                    processSucceeded = DownloadFile("http://s3.amazonaws.com/MinecraftDownload/jinput.jar", packDir + "\\.Minecraft\\bin\\jinput.jar");

                if (processSucceeded)
                    processSucceeded = DownloadFile("http://s3.amazonaws.com/MinecraftDownload/lwjgl_util.jar", packDir + "\\.Minecraft\\bin\\lwjgl_util.jar");

                //download - s3.amazonaws.com/MinecraftDownload/windows_natives.jar and extract
                if (processSucceeded)
                    processSucceeded = DownloadFile("http://s3.amazonaws.com/MinecraftDownload/windows_natives.jar", _globalPackDir + "\\windows_natives.jar");

                if (processSucceeded)
                    processSucceeded = ExtractFile(_globalPackDir + "\\windows_natives.jar", packDir + "\\.Minecraft\\bin\\natives");


                //-------------- Process .jar Injection --------------
                if (TPP.SelectedItem != "Vanilla")
                {
                    //Download file
                    if (processSucceeded)
                    {
                        MethodProgress = "Downloading file - Inject pack";
                        processSucceeded = DownloadFile(urlInjectString, _globalPackDir + "\\" + FileNameFromUri(urlInjectString));
                    }

                    //Inject files
                    if (processSucceeded)
                    {
                        MethodProgress = "Extracting File";
                        //ExtractFile(TPP.SelectedItem, filename);
                        processSucceeded = InjectZipToJar(_globalPackDir + "\\" + FileNameFromUri(urlInjectString), packDir + "\\.Minecraft\\bin\\minecraft.jar");
                    }
                }




                //--------
                if (processSucceeded)
                {
                    if (TPP.StartGameAfterInstall)
                        StartGame(TPP.SelectedItem);
                }
                else
                {
                    RemoveMinecraftFolderFromPack(TPP.SelectedItem);
                }


            }
            catch (Exception ex)
            {
                SetDownloadPanelVisibility(false);

                ex.Data.Add("InstallPack() - packName", TPP.SelectedItem);
                ex.Data.Add("InstallPack() - filename", filename);
                ex.Data.Add("InstallPack() - MethodProgress", MethodProgress);
                ErrorReporting(ex, false);
                //ex.Data.Add("InstallPack() - oPackName", TPP);
                //ErrorReporting(ex, false);
            }
            SetDownloadPanelVisibility(false);
        }

        private void RemoveMinecraftFolderFromPack(string selItem)
        {
            try
            {
                SetDownloadLabelTextMain("Cleaning up");
                if(Directory.Exists(_globalPackDir + "\\" + selItem + "\\.Minecraft\\bin"))
                    DeleteDirectory(_globalPackDir + "\\" + selItem + "\\.Minecraft\\bin");
                SetDownloadPanelVisibility(false);
            }
            catch (Exception ex)
            {
                SetDownloadPanelVisibility(false);

                ex.Data.Add("RemoveMinecraftFolderFromPack() - selItem", selItem);
                ErrorReporting(ex, false);
            }
        }

        public static void DeleteDirectory(string target_dir)
        {
            string[] files = new string[1];
            string[] dirs = new string[1];
            try
            {
                files = Directory.GetFiles(target_dir);
                dirs = Directory.GetDirectories(target_dir);

                foreach (string file in files)
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }

                foreach (string dir in dirs)
                {                    
                    DeleteDirectory(dir);
                }

                files = Directory.GetFiles(target_dir);
                dirs = Directory.GetDirectories(target_dir);

                Directory.Delete(target_dir, false);
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.WaitForPendingFinalizer(); 
            }
            catch (Exception ex)
            {
                ex.Data.Add("DeleteDirectory() - files count", files.Length.ToString());
                ex.Data.Add("DeleteDirectory() - dirs count", dirs.Length.ToString());
                ex.Data.Add("DeleteDirectory() - target_dir", target_dir);
                throw ex;
            }
        }


        //public void DownloadInstallPackAndStartGame(object oPackName)
        //{
        //    try
        //    {                        
        //        if (!CloseAllThreads)
        //            if(DownloadAndInstallPackWorker((string)oPackName))
        //                StartGame((string)oPackName);
        //    }
        //    catch (Exception ex)
        //    {                
        //        ex.Data.Add("DownloadInstallPackAndStartGame() - oPackName", oPackName.ToString());
        //        ErrorReporting(ex, false);
        //    }
        //}

        //public void DownloadAndInstallPack(object oPackName)
        //{
        //    try
        //    {
        //        if (!CloseAllThreads)
        //            DownloadAndInstallPackWorker((string)oPackName);
        //    }
        //    catch (Exception ex)
        //    {                
        //        ex.Data.Add("DownloadAndInstallPack() - oPackName", oPackName.ToString());
        //        ErrorReporting(ex, false);
        //    }
        //}

        ///// <summary>
        ///// This method should NEVER be called by anything other then DownloadAndInstallPack() and DownloadInstallPackAndStartGame()
        ///// Code help thanks to http://www.devtoolshed.com/content/c-download-file-progress-bar
        ///// </summary>
        ///// <param name="pack"></param>
        //private bool DownloadAndInstallPackWorker(string packName)
        //{
        //    //string packName = "";
        //    Uri urlString = null;
        //    string filename = "";
        //    bool returnValue = false;
        //    string MethodProgress = "";

        //    try
        //    {
        //        //packName = (string)oPackName;
        //        //FormOptions.SupportetAutoUpdatePack pack = (FormOptions.SupportetAutoUpdatePack)oPack;     
        //        MethodProgress = "creating URI";
        //        urlString = new Uri(_formOptions.PackUpdateUrl(packName));
        //        filename = urlString.OriginalString.Substring(urlString.OriginalString.LastIndexOf("/") + 1, urlString.OriginalString.Length - urlString.OriginalString.LastIndexOf("/") - 1);

        //        //_formNews = new FormNews(_formOptions.GetVersionInfo(packName, true).UpdateNews);
        //        //_formNews.Show();
        //        //_formNews.Focus();
        //        MethodProgress = "SetAndShowNews";
        //        SetAndShowNews(_formOptions.GetVersionInfo(packName, true).UpdateNews);

        //        //Download file
        //        MethodProgress = "Downloading file";
        //        returnValue = DownloadFile(urlString, filename);

        //        //extract file
        //        MethodProgress = "Extracting File";
        //        ExtractFile(packName, filename);

        //        //this.Invoke(new Action(delegate() { this.UpdatePackSelect(); }));
        //        //StartGame(packName);
        //        //MessageBox.Show("DEBUG - Game startet " + packName);

        //        //test if extract successful? (if minecraft.jar exists)
        //    }
        //    catch (Exception ex)
        //    {
        //        SetDownloadPanelVisibility(false);
                
        //        ex.Data.Add("DownloadAndInstallPack() - packName", packName);                  
        //        ex.Data.Add("DownloadAndInstallPack() - filename", filename);
        //        ex.Data.Add("DownloadAndInstallPack() - MethodProgress", MethodProgress);
        //        ErrorReporting(ex, false);
        //    }
                        
        //    SetDownloadPanelVisibility(false);
        //    return returnValue;
        //}

        private bool DoBackup(string targetFolder, string backupFolder, xmlVersionInfo versionInfo)
        {
            string MethodProgress = "";            
            try
            {                
                if (Directory.Exists(targetFolder)) //skipping backup if theres nothing to backup
                {                    
                    string fullPath = "";                    
                    FileAttributes attr;

                    SetDownloadLabelTextMain("Creating backup");
                    SetDownloadProgressbarMarqueueStyle(ProgressBarStyle.Marquee);
                    MethodProgress = "starting Backup";
                                        
                    foreach (string excludeInfo in versionInfo.ExcludeFromUpdate)
                    {
                        if (CloseAllThreads)
                            break;

                        fullPath = targetFolder + "\\" + excludeInfo;

                        if (Directory.Exists(fullPath) || File.Exists(fullPath))
                        {

                            attr = File.GetAttributes(fullPath);
                            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                            {
                                //directory
                                string tmpStr = backupFolder + "\\" + excludeInfo;
                                if (excludeInfo.Contains("\\"))
                                {
                                    string tmpStr2 = tmpStr.Substring(0, tmpStr.LastIndexOf("\\"));
                                    if (!Directory.Exists(tmpStr2))
                                        Directory.CreateDirectory(tmpStr2);
                                    //tmpStr2 = tmpStr2;
                                }
                                MethodProgress = "moving - Directory";
                                if (Directory.Exists(tmpStr))
                                {
                                    CopyAll(new DirectoryInfo(fullPath), new DirectoryInfo(tmpStr));
                                    DeleteDirectory(fullPath);
                                }
                                else
                                    Directory.Move(fullPath, tmpStr);
                            }
                            else
                            {
                                FileInfo fInfo = new FileInfo(backupFolder + "\\" + excludeInfo);
                                if (!Directory.Exists(fInfo.DirectoryName))
                                    Directory.CreateDirectory(fInfo.DirectoryName);

                                //file                
                                MethodProgress = "moving - File";
                                File.Move(fullPath, backupFolder + "\\" + excludeInfo);
                            }
                        }
                    }

                    //cleaning out
                    MethodProgress = "Cleanup";
                    if (!CloseAllThreads)
                    {
                        DeleteDirectory(targetFolder);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                ex.Data.Add("DoBackup() - targetFolder", targetFolder);
                ex.Data.Add("DoBackup() - backupFolder", backupFolder);
                ex.Data.Add("DoBackup() - MethodProgress", MethodProgress);
                throw ex;
            }
        }
        
        //private void ExtractFile(string packName, string filename)
        /// <summary>
        /// Extracts a specified file to the spedified destination
        /// </summary>
        /// <param name="fileName">The file to extract</param>
        /// <param name="destination">The destination to extract</param>
        private bool ExtractFile(string fileName, string destination)
        {
            string MethodProgress = "";
            bool returnValue = false;
            try
            {
                if (File.Exists(fileName))
                {
                    FileInfo fInfo = new FileInfo(fileName);

                    SetDownloadLabelTextMain("Extracting " + fInfo.Name);
                    SetDownloadProgressbarMarqueueStyle(ProgressBarStyle.Continuous);
                    
                    //SetDownloadProgressbarProgress(0);
                    //SetDownloadPanelVisibility(true);

                    //extract the pack
                    if (!CloseAllThreads)
                    {
                        
                        //if (fInfo.Extension.ToLower() == ".zip")
                        if(ZipFile.IsZipFile(fileName))
                        {
                            //SetDownloadLabelTextSub("Extracting " + fInfo.Name);
                            SetDownloadProgressbarProgress(0);
                            

                            MethodProgress = "Extract - start";

                            //ExtractZip(_globalPackDir + "\\" + filename, _globalPackDir + "\\" + pack.ToString());
                            double dProgressPercentage;// = (dIndex / dTotal);
                            //int iProgressPercentage = (int)(dProgressPercentage * 100);

                            //using (ZipFile zip = ZipFile.Read(_globalPackDir + "\\" + filename))
                            using (ZipFile zip1 = ZipFile.Read(fileName))
                            {
                                int previousPercentage = 0;
                                //foreach (ZipEntry e in zip)

                                //removing META-INF just to avoid potential problems, and make it look nicer
                                MethodProgress = "Extract - removing META-INF";
                                List<ZipEntry> selection = new List<ZipEntry>(zip1.SelectEntries("*", "META-INF"));
                                for (int x = selection.Count - 1; x >= 0; x--)
                                {
                                    ZipEntry entry = selection[x];
                                    zip1.RemoveEntry(entry.FileName);
                                }

                                //also. removing the bin folder to ensure backwords compatiblity
                                MethodProgress = "Extract - removing bin";
                                selection = new List<ZipEntry>();
                                selection.AddRange(zip1.SelectEntries("*", ".minecraft\\bin"));
                                selection.AddRange(zip1.SelectEntries("*", ".minecraft\\bin\\natives"));
                                for (int x = selection.Count - 1; x >= 0; x--)
                                {
                                    ZipEntry entry = selection[x];
                                    zip1.RemoveEntry(entry.FileName);
                                }


                                MethodProgress = "Extract - extracting";
                                for (int i = 0; i < zip1.Count; i++)
                                {
                                    SetDownloadLabelTextSub(zip1[i].FileName);
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
                                    //zip[i].Extract(_globalPackDir + "\\" + packName, ExtractExistingFileAction.OverwriteSilently);
                                    
                                    //zip1[i].Extract(destination, ExtractExistingFileAction.OverwriteSilently);
                                    try
                                    {
                                        zip1[i].Extract(destination, ExtractExistingFileAction.OverwriteSilently);
                                    }
                                    catch (IOException ex)
                                    {
                                        bool b = false;
                                        foreach (var postFix in new[] { ".tmp", ".PendingOverwrite" })
                                        {
                                            var errorPath = Path.Combine(destination, zip1[i].FileName) + postFix;
                                            if (File.Exists(errorPath))
                                            {
                                                File.Delete(errorPath);
                                                b = true;
                                            }
                                        }
                                        if (!b)
                                        {
                                            throw ex;
                                        }
                                        zip1[i].Extract(destination, ExtractExistingFileAction.OverwriteSilently);
                                    } 
                                }
                            }
                            //deletes the downloaded zip after extraction
#if(!DEBUG)
                            if (File.Exists(fileName))
                                File.Delete(fileName);
#endif
                            returnValue = true;
                        }
                        else
                        {
                            MessageBox.Show("The downloaded file is not a valid zip/jar file. This program will now proceed to panic and abort the extraction", "File not a zip file");                            
                        }
                    }
                }
                else
                    throw new Exception("Could not find the file to extract");
                return returnValue;
            }
            catch (Exception ex)
            {
                ex.Data.Add("ExtractFile() - fileName", fileName);
                ex.Data.Add("ExtractFile() - destination", destination);
                ex.Data.Add("ExtractFile() - MethodProgress", MethodProgress);
                throw ex;
            }

        }

        private bool InjectZipToJar(string zipFile, string jarFile)
        {
            string MethodProgress = "";
            bool returnValue = false;
            try
            {
                if (File.Exists(zipFile) && File.Exists(jarFile))
                {
                    FileInfo fInfoZip = new FileInfo(zipFile);
                    FileInfo fInfoJar = new FileInfo(jarFile);

                    SetDownloadLabelTextMain("Injecting Files");
                    SetDownloadProgressbarMarqueueStyle(ProgressBarStyle.Marquee);

                    //SetDownloadProgressbarProgress(0);
                    //SetDownloadPanelVisibility(true);

                    //extract the pack
                    if (!CloseAllThreads)
                    {

                        if(ZipFile.IsZipFile(zipFile) && ZipFile.IsZipFile(jarFile))
                        {
                            //SetDownloadLabelTextSub("Extracting " + fInfo.Name);
                            //SetDownloadProgressbarProgress(0);


                            MethodProgress = "Injecting - start";

                            //ExtractZip(_globalPackDir + "\\" + filename, _globalPackDir + "\\" + pack.ToString());
                            //double dProgressPercentage;// = (dIndex / dTotal);
                            //int iProgressPercentage = (int)(dProgressPercentage * 100);

                            //using (ZipFile zip = ZipFile.Read(_globalPackDir + "\\" + filename))
                            //using (ZipFile jarFile = ZipFile.Read(
                            using (ZipFile zip = ZipFile.Read(zipFile))
                            {
                                //int previousPercentage = 0;


                                using (ZipFile jar = ZipFile.Read(jarFile))
                                {
                                    //first. remove META-INF
                                    //byte[] fileBytes = new byte[]();
                                    MethodProgress = "Injecting - removing META-INF";
                                    List<ZipEntry> selection = new List<ZipEntry>(jar.SelectEntries("*", "META-INF"));
                                    for (int x = selection.Count - 1; x >= 0; x--)
                                    {
                                        ZipEntry entry = selection[x];
                                        jar.RemoveEntry(entry.FileName);
                                    }


                                    //then. add files from zip
                                    foreach (ZipEntry file in zip)
                                    {
                                        //using (var stream = file.OpenReader())
                                        //{
                                        //    var buffer = new byte[2048];
                                        //    int n;
                                        //    while ((n = stream.Read(buffer, 0, buffer.Length)) > 0)
                                        //    {
                                        //        // do something with the buffer here.
                                        //        fileBytes = buffer;
                                        //    }
                                        //    jar.AddEntry(file.FileName, fileBytes);
                                        //}
                                        //using (var ms = new MemoryStream())
                                        //{
                                        //    file.Extract(ms);

                                        //    //jar.AddEntry(file.FileName, ms.GetBuffer());
                                        //    ZipEntry entry = jar.UpdateEntry(file.FileName, ms.GetBuffer()); //this works, but cant CRC validate
                                        //    if (entry.Crc != file.Crc)
                                        //    {
                                        //        throw new Exception("sdfgsdfg");
                                        //    }                                            
                                        //}
                                        using (Ionic.Crc.CrcCalculatorStream s = file.OpenReader())
                                        {
                                            MethodProgress = "Injecting - starting injection";
                                            byte[] buffer = new byte[file.UncompressedSize];
                                            int n, totalBytesRead = 0;
                                            do
                                            {
                                                n = s.Read(buffer, 0, buffer.Length);
                                                totalBytesRead += n;
                                            } while (n > 0);
                                            if (s.Crc != file.Crc)
                                                throw new Exception(string.Format("The Zip Entry failed the CRC Check. (0x{0:X8}!=0x{1:X8})", s.Crc, file.Crc));
                                            if (totalBytesRead != file.UncompressedSize)
                                                throw new Exception(string.Format("We read an unexpected number of bytes. ({0}!={1})", totalBytesRead, file.UncompressedSize));

                                            MethodProgress = "Injecting - updating entry";
                                            jar.UpdateEntry(file.FileName, buffer);
                                        }                                        
                                    }
                                    jar.Save();
                                    returnValue = true;
                                }
                            }
                            //deletes the downloaded zip after extraction
#if(!DEBUG)
                            if (File.Exists(zipFile))
                                File.Delete(zipFile);
#endif
                        }
                        else
                        {
                            MessageBox.Show("The downloaded file is not a valid zip/jar file. This program will now proceed to panic and abort the extraction", "File not a zip file");                            
                        }
                    }
                    return returnValue;
                }
                else
                    return returnValue;
            }
            catch (Exception ex)
            {
                //ex.Data.Add("ExtractFile() - fileName", fileName);
                //ex.Data.Add("ExtractFile() - destination", destination);
                ex.Data.Add("ExtractFile() - MethodProgress", MethodProgress);
                throw ex;
            }

        }

        private string FileNameFromUri(string urlString)
        {
            try
            {
                //return uri.OriginalString.Substring(uri.OriginalString.LastIndexOf("/") + 1, uri.OriginalString.Length - uri.OriginalString.LastIndexOf("/") - 1);
                Uri url = null;
                if (Uri.TryCreate(urlString, UriKind.Absolute, out url))
                {
                    return url.Segments[url.Segments.Length - 1];
                }
                else
                    return urlString;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool DoRestoreBackup(string targetFolder, string backupFolder)
        {
            string MethodProgress = "";
            try
            {
                if (Directory.Exists(backupFolder))
                {
                    SetDownloadLabelTextMain("Restoring backup");
                    SetDownloadProgressbarMarqueueStyle(ProgressBarStyle.Marquee);
                    MethodProgress = "Copying";
                    CopyAll(new DirectoryInfo(backupFolder), new DirectoryInfo(targetFolder));
                    MethodProgress = "Deleting";
                    DeleteDirectory(backupFolder);
                    //Directory.Move(_globalPackDir + "\\" + packName + "\\UpdateBackup", _globalPackDir + "\\" + packName + "\\.minecraft"); 
                }
                return true;
            }
            catch (Exception ex)
            {
                ex.Data.Add("DoRestoreBackup() - targetFolder", targetFolder);
                ex.Data.Add("DoRestoreBackup() - backupFolder", backupFolder);
                ex.Data.Add("DoRestoreBackup() - MethodProgress", MethodProgress);
                throw ex;
            }
        }

        private void DownloadFile(Uri Url)
        {
            try
            {
                string destination = _globalPackDir + "\\" + FileNameFromUri(Url.OriginalString);
                DownloadFile(Url.OriginalString, destination);
            }
            catch (Exception ex)
            {                
                //ex.Data.Add("DownloadFile() - Url", Url.OriginalString);
                throw ex;
            }
        }

        /// <summary>
        /// Downloads a file from the specified Uri to a specified destination
        /// </summary>
        /// <param name="urlString">The Uri containing the web addres of the file to be downloaded.</param>
        /// <param name="destination">The destination of the file to be downloaded. Includes the filename.</param>
        /// <returns></returns>
        private bool DownloadFile(string url, string destination)
        {
            bool SkipDownload = false;
            //bool SkipCopy = false;
            int RemoteFileSize = 0;
            int iRunningByteTotal = 0;
            //DateTime previousTickTime;
            //TimeSpan tickDuration;
            //int previousPercentage = 0;
            //Int64 TickRunningByte = 0;
            bool returnValue = true;
            string MethodProgress = "";
            Uri validUrl = null;
            string tmpDestination = "";
            //double dProgressPercentage = 0;
            //int iProgressPercentage = 0;

            try
            {
                
                SkipDownload = !Uri.TryCreate(url, UriKind.Absolute, out validUrl);
                tmpDestination = _globalPackDir + "\\" + FileNameFromUri(url);

                SetDownloadLabelTextMain("Downloading");
                SetDownloadLabelTextSub("Preparing download");
                SetDownloadProgressbarProgress(0);
                SetDownloadProgressbarMarqueueStyle(ProgressBarStyle.Continuous);
                SetDownloadPanelVisibility(true);

                if (validUrl != null)
                {
                    System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                    SetAllowUnsafeHeaderParsing20();
                    System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
                    response.Close();

                    RemoteFileSize = (int)response.ContentLength;  // gets the size of the file in bytes                        
                    iRunningByteTotal = 0; // keeps track of the total bytes downloaded so we can update the progress bar

                    //check if pack has already been downloaded

                    if (File.Exists(tmpDestination))
                    {
                        if (new FileInfo(tmpDestination).Length == RemoteFileSize) //file exists, but what about the size ? (to filter out incomplete downloads)
                        {
                            if (MessageBox.Show("The file " + new FileInfo(tmpDestination).Name + " with matching size was found on the disk." + Environment.NewLine + "Do you want to use that file instead of downloading it again?", "Existing file found", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                            {
                                SkipDownload = true;
                            }
                        }
                    }

                }
                else
                {
                    if (!File.Exists(tmpDestination))                    
                        throw new Exception("The file " + new FileInfo(tmpDestination).Name + " could not be downloaded and was not found on the disk. If you downloaded it manually, then please make sure you placed it in the " + _globalPackDir + " folder");                    
                }


                if (!SkipDownload)
                {
                    //download the pack
                    SetDownloadPanelVisibility(true);
                    //SetDownloadLabelTextMain("Downloading");
                    //SetDownloadProgressbarProgress(0);
                    

                    using (System.Net.WebClient client = new System.Net.WebClient())
                    {
                        // open the file at the remote URL for reading
                        MethodProgress = "Using streamRemote";
                        client.DownloadProgressChanged += client_DownloadProgressChanged;
                        client.DownloadFileCompleted += client_DownloadFileCompleted;
                        
                        LastDownloadTickTime = DateTime.Now;
                        DownloadingFile = true;

                        client.DownloadFileAsync(validUrl, tmpDestination);

                        SetDownloadLabelTextSub(new FileInfo(tmpDestination).Name);
                        SetDownloadCancelButtonVisibility(true);

                        while (client.IsBusy)//(DownloadingFile)
                        {
                            Thread.Sleep(100);
                            if (CloseAllThreads || _abortDownload)
                                client.CancelAsync();
                        }
                        //if (CloseAllThreads)
                        //    break;
                        if (_abortDownload)
                        {
                            MessageBox.Show("The download has been cancelled by the user.", "Download Cancelled!");
                            returnValue = false;
                            //client.CancelAsync();
                            DownloadingFile = false;
                            //break;
                        }
                        //using (System.IO.Stream streamRemote = client.OpenRead(urlString))
                        //{
                        //    // using the FileStream object, we can write the downloaded bytes to the file system
                        //    MethodProgress = "Using streamLocal";
                        //    using (Stream streamLocal = new FileStream(destination, FileMode.Create, FileAccess.Write, FileShare.None))
                        //    {
                        //        // loop the stream and get the file into the byte buffer
                        //        int iByteSize = 0;
                        //        //byte[] byteBuffer = new byte[RemoteFileSize];
                        //        byte[] byteBuffer = new byte[4096];

                        //        previousTickTime = DateTime.Now;

                        //        SetDownloadLabelTextSub(new FileInfo(destination).Name);
                        //        SetDownloadCancelButtonVisibility(true);


                        //        MethodProgress = "While... streamRemote.read ";
                        //        while ((iByteSize = streamRemote.Read(byteBuffer, 0, byteBuffer.Length)) > 0) //...ngByteTotal, byteBuffer.Length))
                        //        {
                        //            //aborts the download if a cancel request have been made
                        //            if (CloseAllThreads)
                        //                break;
                        //            if (_abortDownload)
                        //            {
                        //                MessageBox.Show("The download has been cancelled by the user.", "Download Cancelled!");
                        //                returnValue = false;
                        //                break;
                        //            }

                        //            // write the bytes to the file system at the file path specified
                        //            MethodProgress = "streamLocal.Write";
                        //            streamLocal.Write(byteBuffer, 0, iByteSize);
                        //            iRunningByteTotal += iByteSize;

                        //            // calculate the progress out of a base "100"
                        //            //double dIndex = (double)(iRunningByteTotal);
                        //            //double dTotal = RemoteFileSize; //(double)byteBuffer.Length;
                        //            //dProgressPercentage = ((double)iRunningByteTotal / (double)RemoteFileSize);
                        //            iProgressPercentage = (int)(((double)iRunningByteTotal / (double)RemoteFileSize) * 100);

                        //            // update the progress bar
                        //            //backgroundWorker1.ReportProgress(iProgressPercentage);

                        //            if (previousPercentage < iProgressPercentage)
                        //            {

                        //                tickDuration = DateTime.Now - previousTickTime;
                        //                double speed = (1 / tickDuration.TotalSeconds) * (iRunningByteTotal - TickRunningByte);
                        //                previousTickTime = DateTime.Now;
                        //                TickRunningByte = iRunningByteTotal;

                        //                SetDownloadLabelSpeedAndProgressText(string.Format("{0} KB/s", Math.Floor(speed / 1024)), string.Format("{0} / {1} MB", iRunningByteTotal / 1048576, RemoteFileSize / 1048576));
                        //                SetDownloadProgressbarProgress(iProgressPercentage);
                        //                previousPercentage = iProgressPercentage;
                        //            }                                    
                        //        }

                        //        // clean up the file stream
                        //        streamLocal.Close();
                        //    }

                        //    // close the connection to the remote server
                        //    streamRemote.Close();
                        //}
                    }

                    if (!_abortDownload)
                    {
                        SetDownloadProgressbarProgress(100);
                        SetDownloadLabelTextMain("Download Complete");
                    }
                }

                if (_abortDownload && !CloseAllThreads)
                {
                    File.Delete(tmpDestination);
                    _abortDownload = false;
                    SetDownloadPanelVisibility(false);
                }

                if (String.Compare(Path.GetFullPath(tmpDestination).TrimEnd('\\'), Path.GetFullPath(destination).TrimEnd('\\'), StringComparison.InvariantCultureIgnoreCase) != 0)
                {
                    if (File.Exists(tmpDestination))
                    {
                        File.Copy(tmpDestination,destination,true);
                        File.Delete(tmpDestination);
                    }
                    else
                    {
                        throw new Exception("The tmpDestination (" + tmpDestination + ") did not exsist when trying to copy it to the correct destination");
                    }
                }

                SetDownloadCancelButtonVisibility(false);
            }
            catch (Exception ex)
            {
                //ex.Data.Add("DownloadFile() - Url", urlString.OriginalString);
                ex.Data.Add("DownloadFile() - methodProgress", MethodProgress);
                ex.Data.Add("DownloadFile() - destination", destination);
                ex.Data.Add("DownloadFile() - SkipDownload", SkipDownload.ToString());
                ex.Data.Add("DownloadFile() - RemoteFileSize", RemoteFileSize.ToString());
                ex.Data.Add("DownloadFile() - iRunningByteTotal", iRunningByteTotal.ToString());

                throw ex;
            }
            return returnValue;
        }

        private void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    // handle error scenario
                    if(!e.Cancelled)
                        throw e.Error;
                }
                if (e.Cancelled)
                {
                    // handle cancelled scenario                    
                }
                DownloadingFile = false;
            }
            catch (Exception ex)
            {
                SetDownloadPanelVisibility(false);                
                ErrorReporting(ex, false);
            }          
        }

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            TimeSpan TimeSinceLastDownloadTick;
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = (int)(bytesIn / totalBytes * 100);
            double speed =0;
            

            //if (percentage > LastDownloadPercentageTick)
            
            TimeSinceLastDownloadTick = DateTime.Now - LastDownloadTickTime;

            if(TimeSinceLastDownloadTick.TotalSeconds >= 1)
            {
                speed = (1 / TimeSinceLastDownloadTick.TotalSeconds) * (bytesIn - ByteDownloadedUpToPreviousTick);
                SetDownloadLabelSpeedAndProgressText(string.Format("{0} KB/s", ByteDownloadedUpToPreviousTick == 0 ? 0 : Math.Floor(speed / 1024)), string.Format("{0} / {1} MB", Math.Floor(bytesIn / 1048576), Math.Floor(totalBytes / 1048576)));
                ByteDownloadedUpToPreviousTick = (int)bytesIn;

                LastDownloadPercentageTick = percentage;
                LastDownloadTickTime = DateTime.Now;
                SetDownloadProgressbarProgress((int)percentage);
            }            
            
            //progressBar1.Value = int.Parse(Math.Truncate(percentage).ToString());
        }

        private void StartGame(string selItem)
        {
            string DebugMethodProgress = "";
            string tmpString = "";
            ProcessStartInfo procStartInfo = new ProcessStartInfo();

            try
            {
                if (HasInternetAndLoginConnection())
                {

                    DebugMethodProgress = "Line1";
                    SetDownloadLabelTextMain("Logging in");
                    this.Invoke(new Action(delegate() { this.Update(); }));
                    
                    Process proc = new Process();
                    DateTime gameStartTime = new DateTime();
                    DebugMethodProgress = "Line2";
                    string session = generateSession(textBoxUsername.Text, textBoxPassword.Text, 5000);
                    SaveSessionToFile(session);                    

                    string sessionID = "";
                    string username = "";

                    DebugMethodProgress = "Line3";
                    if (session.ToLower().Contains("bad login"))
                    {
                        MessageBox.Show("Invalid Username and/or Password." + Environment.NewLine + "Please make sure you typed in the right information", "Invalid Username and/or Password", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    else if (session == "Account migrated, use e-mail as username.")
                    {
                        MessageBox.Show("Your account has been migrated to a Mojang acocunt. Use your email as a username to log in", "Account migrated");
                    }
                    else
                    {
                        //assumes that a valid session was retrieved. Might need more work here
                        DebugMethodProgress = "Line4";
                        if (session.Contains(":"))
                            if (session.Split(':').Length < 4)
                                throw new Exception("Session was not in the expected format. Session was: " + session);
                            else
                                sessionID = session.Split(':')[3];

                        DebugMethodProgress = "Line5";
                        if (textBoxUsername.Text.Contains("@"))
                            if (session.Split(':').Length < 3)
                                throw new Exception("Session was not in the expected format Session was: " + session);
                            else
                                username = session.Split(':')[2];
                        else
                            username = textBoxUsername.Text;

                        DebugMethodProgress = "Line6";
                        if (File.Exists(_globalPackDir + "\\" + selItem + "\\.Minecraft\\bin\\minecraft.jar"))
                        {
                            //FINALLY i got it to work. Damm it was a pain
                            SetDownloadLabelTextMain("Starting game");
                            this.Invoke(new Action(delegate() { this.Update(); }));
                            DebugMethodProgress = "Line7";
                            procStartInfo.FileName = _formOptions.GetJavaInstallationPath() + @"\bin\javaw.exe";
                            DebugMethodProgress = "Line8";
                            Environment.SetEnvironmentVariable("APPDATA", _globalPackDir + "\\" + selItem);
                            DebugMethodProgress = "Line9";
                            /*no error*/
                            tmpString = string.Format(@" -Xms{0}m -Xmx{1}m -cp ""%APPDATA%\.minecraft\bin\*"" -Djava.library.path=""%APPDATA%\.minecraft\bin\natives"" net.minecraft.client.Minecraft {2} {3}", /*0*/ _formOptions.GetMemmoryMin(), /*1*/ _formOptions.GetMemmoryMax(), /*2*/ username, /*3*/ sessionID);
                            DebugMethodProgress = "Line10";
                            procStartInfo.Arguments = Environment.ExpandEnvironmentVariables(tmpString);
                            /*error*/
                            //procStartInfo.Arguments = Environment.ExpandEnvironmentVariables(string.Format(@" -Xms{0}m -Xmx{1}m -cp %APPDATA%\.minecraft\bin\*"" -Djava.library.path=""%APPDATA%\.minecraft\bin\natives"" net.minecraft.client.Minecraft {2} {3}", /*0*/ _formOptions.GetMemmoryMin(), /*1*/ _formOptions.GetMemmoryMax(), /*2*/ username, /*3*/ sessionID));

                            procStartInfo.RedirectStandardOutput = true;
                            procStartInfo.RedirectStandardError = true;
                            procStartInfo.UseShellExecute = false;


                            //#if(DEBUG)
                            //                    MessageBox.Show("DEBUG - Game startet");
                            //#else
                            gameStartTime = DateTime.Now;
                            proc.StartInfo = procStartInfo;
                            DebugMethodProgress = "Line11";
                            proc.Start();

                            //Process.Start(procStartInfo);

                            this.Invoke(new Action(delegate() { this.Hide(); }));

                            try
                            {
                                using (FileStream fstream = new FileStream(Application.StartupPath + "\\GameLog.txt", FileMode.Create, FileAccess.Write, FileShare.Read))
                                {
                                    using (TextWriter writer = new StreamWriter(fstream))
                                    {
                                        do
                                        {
                                            writer.Write(proc.StandardError.ReadLine() + Environment.NewLine);
                                        }
                                        while (!proc.StandardError.EndOfStream);
                                    }
                                }



                                //string standardError = proc.StandardError.ReadToEnd();
                                //if (standardError == "")
                                //{
                                //    if (File.Exists(Application.StartupPath + "\\GameLog1.txt"))
                                //        File.Delete(Application.StartupPath + "\\GameLog1.txt");
                                //}
                                //else
                                //{
                                //    using (FileStream fstream = new FileStream(Application.StartupPath + "\\GameLog1.txt", FileMode.Create, FileAccess.Write, FileShare.None))
                                //    {
                                //        using (TextWriter writer = new StreamWriter(fstream))
                                //        {
                                //            writer.Write(standardError);
                                //        }
                                //    }
                                //}

                                //string standardOutput = proc.StandardOutput.ReadToEnd();
                                //if (standardOutput == "")
                                //{
                                //    if (File.Exists(Application.StartupPath + "\\GameLog2.txt"))
                                //        File.Delete(Application.StartupPath + "\\GameLog2.txt");
                                //}
                                //else
                                //{
                                //    using (FileStream fstream = new FileStream(Application.StartupPath + "\\GameLog2.txt", FileMode.Create, FileAccess.Write, FileShare.None))
                                //    {
                                //        using (TextWriter writer = new StreamWriter(fstream))
                                //        {
                                //            writer.Write(standardOutput);
                                //        }
                                //    }
                                //}

                            }
                            catch (Exception ex)
                            {
                                //just absorps the exception
                            }
                            DebugMethodProgress = "Line12";
                            proc.WaitForExit();
                            //this.Invoke(new Action(delegate() { this.Close(); }));
                            this.Invoke(new Action(delegate() { this.Show(); }));
                            TimeSpan timespan = DateTime.Now - gameStartTime;
                            if (timespan.TotalSeconds < 1)
                            {
                                string message = "It looks like the game failed to start correctly. " + Environment.NewLine + "A GameLog file have been created with more information." + Environment.NewLine + "This file can be found in the same folder as the launcher";
                                MessageBox.Show(message, "An error occured (possibly and very likely)", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            //#endif
                        }
                        else
                        {
                            MessageBox.Show("The minecraft.jar file was not found. Make sure you installed the mod pack correctly", "Incorrectly installed minecraft mod pack", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                else
                {
                    if (MessageBox.Show("Could not connect to Mojangs Login server." + Environment.NewLine + "Do you want to start the game in offline mode?", "Connection problems. Play Offline?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Yes)
                    {
                        StartGameOffline(selItem);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("StartGame() - DebugMethodProgress", DebugMethodProgress);
                ex.Data.Add("StartGame() - procStartInfo.FileName", procStartInfo.FileName);
                throw ex;
            }

            //and this is just the remnants of my failed tries


            //procStartInfo.StartInfo.FileName =  GetJavaInstallationPath() + "\\bin\\java.exe";

            //procStartInfo.StartInfo.EnvironmentVariables.Keys["APPDATA"] = _globalPackDir + "\\" + selItem;
            //procStartInfo.EnvironmentVariables.Remove("APPDATA");
            //procStartInfo.EnvironmentVariables.Add("APPDATA", _globalPackDir + "\\" + selItem);

            //procStartInfo.UseShellExecute = false;
            //procStartInfo.StartInfo.CreateNoWindow = false;

            //procStartInfo.StartInfo.Arguments = string.Format("-Xms{0}M -Xmx{1}M -Djava.library.path={2}.minecraft\\bin\\natives -cp {2}.minecraft\\bin\\minecraft.jar;{2}.minecraft\\bin\\jinput.jar;{2}.minecraft\\bin\\lwjgl.jar;{2}.minecraft\\bin\\lwjgl_util.jar net.minecraft.client.Minecraft {3} {4}", _formOptions.GetMemmoryMin(), _formOptions.GetMemmoryMax(), _globalPackDir + "\\" + selItem + "\\", textBoxUsername.Text, sessionID);
            //procStartInfo.StartInfo.Arguments = string.Format("-Xms{0}M -Xmx{1}M -Djava.library.path=%APPDATA%\\.minecraft\\bin\\natives -cp %APPDATA%\\.minecraft\\bin\\minecraft.jar;%APPDATA%\\.minecraft\\bin\\jinput.jar;%APPDATA%\\.minecraft\\bin\\lwjgl.jar;%APPDATA%\\.minecraft\\bin\\lwjgl_util.jar net.minecraft.client.Minecraft {2} {3}", _formOptions.GetMemmoryMin(), _formOptions.GetMemmoryMax(), textBoxUsername.Text, sessionID);

            //procStartInfo.StartInfo.Arguments = " -Xms" + _formOptions.GetMemmoryMin() + "m -Xmx" + _formOptions.GetMemmoryMax() + "m -cp \"%APPDATA%\\.minecraft\\bin\\*\" -Djava.library.path=\"%APPDATA%\\.minecraft\\bin\\natives\" net.minecraft.client.Minecraft" + textBoxUsername.Text + textBoxPassword.Text;
            //procStartInfo.StartInfo.Arguments = procStartInfo.StartInfo.Arguments.Replace("%APPDATA%", _globalPackDir + "\\" + selItem);
            //procStartInfo.StartInfo.Arguments = @"-Xms512m -Xmx1024m -cp %APPDATA%\.minecraft\bin\* -Djava.library.path=%APPDATA%\.minecraft\bin\natives net.minecraft.client.Minecraft GrandPhoenix82 jabjab1";

            //procStartInfo.StartInfo.FileName = "java";

            //Console.WriteLine("session id " + sessionID);

            //Environment.SetEnvironmentVariable("APPDATA", _globalPackDir + "\\" + selItem); //C:\Users\Phoenix\AppData\Roaming
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

            //string javaString = string.Format(" javaw -Xms{0}M -Xmx{1}M -cp \"{2}\\.minecraft\\bin\\minecraft.jar;{2}\\.minecraft\\bin\\jinput.jar;{2}\\.minecraft\\bin\\lwjgl.jar;{2}\\.minecraft\\bin\\lwjgl_util.jar\" -Djava.library.path=\"{2}\\.minecraft\\bin\\natives\" net.minecraft.client.Minecraft {3} {4}", _formOptions.GetMemmoryMin().ToString(), _formOptions.GetMemmoryMax().ToString(), _globalPackDir + "\\" + selItem, textBoxUsername.Text, textBoxPassword.Text);
            //string javaString = string.Format(" -Xms{0}M -Xmx{1}M -cp \"{2}\\.minecraft\\bin\\minecraft.jar;{2}\\.minecraft\\bin\\jinput.jar;{2}\\.minecraft\\bin\\lwjgl.jar;{2}\\.minecraft\\bin\\lwjgl_util.jar\" -Djava.library.path=\"{2}\\.minecraft\\bin\\natives\" net.minecraft.LauncherFrame {3} {4}", _formOptions.GetMemmoryMin().ToString(), _formOptions.GetMemmoryMax().ToString(), _globalPackDir + "\\" + selItem, textBoxUsername.Text, textBoxPassword.Text);
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

        private void StartGameOffline(string selItem)
        {
            //byte[] LastLoginSalt = new byte[] { 0x0c, 0x9d, 0x4a, 0xe4, 0x1e, 0x83, 0x15, 0xfc };
            //const string LastLoginPassword = "passwordfile";
            string Username = "";            
            string DebugMethodProgress = "";
            
            try
            {
                DebugMethodProgress = "Line1";
                Username = LoadUsernamefromSessionFile();

                if (Username == "Bad/Invalid Session")
                {
                    MessageBox.Show("Could not find a valid session from your last login." + Environment.NewLine + "You need to have logged in with valid credentials atleast once to play offline", "Invalid Lastlogin");
                }
                else
                {                    
                    SetDownloadLabelTextMain("Starting Game");
                    this.Invoke(new Action(delegate() { this.Update(); }));
                    ProcessStartInfo procStartInfo = new ProcessStartInfo();
                    Process proc = new Process();
                    DateTime gameStartTime = new DateTime();
                                        
                    {                        
                        DebugMethodProgress = "Line2";
                        if (File.Exists(_globalPackDir + "\\" + selItem + "\\.Minecraft\\bin\\minecraft.jar"))
                        {
                            //FINALLY i got it to work. Damm it was a pain
                            //SetDownloadLabelTextMain("Starting game");
                            this.Invoke(new Action(delegate() { this.Update(); }));
                            DebugMethodProgress = "Line3";
                            procStartInfo.FileName = _formOptions.GetJavaInstallationPath() + @"\bin\javaw.exe";
                            DebugMethodProgress = "Line4";
                            Environment.SetEnvironmentVariable("APPDATA", _globalPackDir + "\\" + selItem);
                            DebugMethodProgress = "Line5";
                            /*no error*/
                            procStartInfo.Arguments = Environment.ExpandEnvironmentVariables(string.Format(@" -Xms{0}m -Xmx{1}m -cp ""%APPDATA%\.minecraft\bin\*"" -Djava.library.path=""%APPDATA%\.minecraft\bin\natives"" net.minecraft.client.Minecraft {2}", /*0*/ _formOptions.GetMemmoryMin(), /*1*/ _formOptions.GetMemmoryMax(), /*2*/ Username));
                            /*error*/
                            //procStartInfo.Arguments = Environment.ExpandEnvironmentVariables(string.Format(@" -Xms{0}m -Xmx{1}m -cp %APPDATA%\.minecraft\bin\*"" -Djava.library.path=""%APPDATA%\.minecraft\bin\natives"" net.minecraft.client.Minecraft {2} {3}", /*0*/ _formOptions.GetMemmoryMin(), /*1*/ _formOptions.GetMemmoryMax(), /*2*/ username, /*3*/ sessionID));

                            procStartInfo.RedirectStandardOutput = true;
                            procStartInfo.RedirectStandardError = true;
                            procStartInfo.UseShellExecute = false;


                            gameStartTime = DateTime.Now;
                            proc.StartInfo = procStartInfo;
                            DebugMethodProgress = "Line6";
                            proc.Start();

                            //Process.Start(procStartInfo);

                            this.Invoke(new Action(delegate() { this.Hide(); }));

                            try
                            {
                                using (FileStream fstream = new FileStream(Application.StartupPath + "\\GameLog.txt", FileMode.Create, FileAccess.Write, FileShare.Read))
                                {
                                    using (TextWriter writer = new StreamWriter(fstream))
                                    {
                                        do
                                        {
                                            writer.Write(proc.StandardError.ReadLine() + Environment.NewLine);
                                        }
                                        while (!proc.StandardError.EndOfStream);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                //just absorps the exception
                            }
                            DebugMethodProgress = "Line7";
                            proc.WaitForExit();
                            //this.Invoke(new Action(delegate() { this.Close(); }));
                            this.Invoke(new Action(delegate() { this.Show(); }));
                            TimeSpan timespan = DateTime.Now - gameStartTime;
                            if (timespan.TotalSeconds < 1)
                            {
                                string message = "It looks like the game failed to start correctly. " + Environment.NewLine + "A GameLog file have been created with more information." + Environment.NewLine + "This file can be found in the same folder as the launcher";
                                MessageBox.Show(message, "An error occured (possibly and very likely)", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            //#endif
                        }
                        else
                        {
                            MessageBox.Show("The minecraft.jar file was not found. Make sure you installed the mod pack correctly", "Incorrectly installed minecraft mod pack", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("StartGameOffline() - DebugMethodProgress", DebugMethodProgress);
                throw ex;
            }
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

        private void SaveSessionToFile(string session)
        {
            try
            {
                using (FileStream fstream = new FileStream(Application.StartupPath + "\\KBGLastLogin", FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    using (TextWriter writer = new StreamWriter(fstream))
                    {
                        writer.Write(_formOptions.Settings.Encrypt(session, "SuperHaxMaxSecurePassword"));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string LoadUsernamefromSessionFile()
        {
            string session = "";
            try
            {
                //if (File.Exists(_globalPackDir + "\\" + selItem + "\\.Minecraft\\KBGLastLogin"))
                if (File.Exists(Application.StartupPath + "\\KBGLastlogin"))
                {
                    using (FileStream fstream = new FileStream(Application.StartupPath + "\\KBGLastlogin", FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        using (TextReader  reader = new StreamReader(fstream))
                        {
                            session = _formOptions.Settings.Decrypt(reader.ReadToEnd(), "SuperHaxMaxSecurePassword");
                        }
                    }
                    //validate session info
                    if (session.Contains(":"))
                    {
                        string[] tmpStrArr = session.Split(':');
                        if (tmpStrArr.Length == 5)
                        {
                            if (tmpStrArr[2] != "" && tmpStrArr[3] != "")
                                return tmpStrArr[2];
                            else
                                return "Bad/Invalid Session";
                        }
                        else
                            return "Bad/Invalid Session"; 
                    }
                    else
                        return "Bad/Invalid Session";
                }
                else
                {
                    return "Bad/Invalid Session";
                }
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
            string clientVersion = null;
            ulong SystemMemory = 0;
            //error information
            try
            {
                if (_formError == null || _formError.IsDisposed)
                    _formError = new FormError();

                _formError.AddInfoLine("Error: " + ex.Message + Environment.NewLine);

                if(ex.InnerException != null)
                    _formError.AddInfoLine("InnerException: " + ex.InnerException.Message + Environment.NewLine);

                _formError.AddInfoLine("Error Occured at: " + Environment.NewLine + ex.StackTrace);

                _formError.AddInfoLine(Environment.NewLine + Environment.NewLine + "Extra error information.  {");
                foreach (DictionaryEntry de in ex.Data)
                {
                    _formError.AddInfoLine(string.Format("      {0} = {1}", de.Key, de.Value));
                }
                _formError.AddInfoLine("}");


                
                //system information
                _formError.AddInfoLine(Environment.NewLine + "System information.  {");                

                if (System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE") != null)
                    _formError.AddInfoLine("PROCESSOR_ARCHITECTURE = " + System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE"));
                else
                    _formError.AddInfoLine("PROCESSOR_ARCHITECTURE returned null");

                try
                {
                    //_formError.AddInfoLine("System memory amount deteted = " + Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory
                    SystemMemory = new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory;
                }
                catch(Exception exy){} //not interested in the error, so its just absorbed

                _formError.AddInfoLine("System memory ammount = " + SystemMemory.ToString());

                _formError.AddInfoLine("OS version: " + Environment.OSVersion);
                _formError.AddInfoLine("Environment version: " + Environment.Version.ToString());

                RegistryKey installed_versions = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP");
                string[] version_names = installed_versions.GetSubKeyNames();
                //version names start with 'v', eg, 'v3.5' which needs to be trimmed off before conversion
                double Framework = Convert.ToDouble(version_names[version_names.Length - 1].Remove(0, 1), CultureInfo.InvariantCulture);
                RegistryKey newestDotNetKey = installed_versions.OpenSubKey(version_names[version_names.Length - 1]);
                int SP = Convert.ToInt32(newestDotNetKey.GetValue("SP", 0));


                string tmpDotNETversionsString = "";
                foreach (string version in version_names)
                {
                    tmpDotNETversionsString += version + ", ";
                }
                tmpDotNETversionsString = tmpDotNETversionsString.Substring(0, tmpDotNETversionsString.Length - 2);

                _formError.AddInfoLine("Installed .NET versions: " + tmpDotNETversionsString);
                //_formError.AddInfoLine("Newest .NET version:  " + Framework + "  (SP: " + SP.ToString() + ")");

                string DotNetVersionLong = (string)newestDotNetKey.GetValue("Version", "");
                if (DotNetVersionLong == "")
                {
                    //assuming version 4 now
                    RegistryKey v4subkey = installed_versions.OpenSubKey("v4");
                    foreach (string subkey in v4subkey.GetSubKeyNames())
                    {
                        _formError.AddInfoLine(string.Format("Newest .NET version: {0}  ({1}) (SP: {2}", v4subkey.OpenSubKey(subkey).GetValue("Version", "x"), subkey, SP.ToString()));
                    }
                }
                else
                    _formError.AddInfoLine("Newest .NET version: " + Framework + "  (SP: " + SP.ToString() + ")");

                _formError.AddInfoLine("}");


                //basic information
                _formError.AddInfoLine(Environment.NewLine + "FormMain information.  {");
                //if(_formOptions != null)
                //    _formError.AddInfoLine("Client Version: " + _formOptions.GetClientVersion());
                //else
                //    _formError.AddInfoLine("Client Version: _formOptions is null");

                try 
                {
                    Assembly ass = Assembly.GetExecutingAssembly();
                    if (ass != null)
                    {
                        FileVersionInfo FVI = FileVersionInfo.GetVersionInfo(ass.Location);  
                        clientVersion = string.Format("{0}.{1}.{2}.{3}", FVI.FileMajorPart, FVI.FileMinorPart, FVI.FileBuildPart, FVI.FilePrivatePart);
                    }
                }
                catch (Exception exz){}

                if(clientVersion == null)
                    _formError.AddInfoLine("Client Version: clientVersion was null");
                else
                    _formError.AddInfoLine("Client Version: " + clientVersion);


                //System.Environment.GetEnvironmentVariable("ProgramFiles");
                

                _formError.AddInfoLine("_TweetList null?: " + (_TweetList == null).ToString());
                _formError.AddInfoLine("_formOptions null? " + (_formOptions == null).ToString());
                _formError.AddInfoLine("_formNews null? " + (_formNews == null).ToString());
                _formError.AddInfoLine("_updateFinished = " + _updateFinished.ToString());

                _formError.AddInfoLine("_globalPackDir = " + _globalPackDir);
                _formError.AddInfoLine("_abortDownload = " + _abortDownload.ToString());
                _formError.AddInfoLine("_loadingSettings = " + _loadingSettings.ToString());
                _formError.AddInfoLine("_offlineMode = " + _offlineMode.ToString());

                _formError.AddInfoLine("}");

                bool hasInternetConnection = true;
                try
                {
                    hasInternetConnection = HasInternetConnection();
                }
                catch(Exception)
                {
                    hasInternetConnection = false;
                }  

                if (!hasInternetConnection)
                {
                    _formError.AddInfoLine(Environment.NewLine + "Network Inferfaces" +  "{");
                    NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
                    foreach (NetworkInterface adapter in interfaces)
                    {
                        _formError.AddInfoLine(string.Format("Name: {0}, Description: {1}, Type: {2}, State: {3}", adapter.Name, adapter.Description, adapter.NetworkInterfaceType.ToString(), adapter.OperationalStatus.ToString()));
                    }
                    _formError.AddInfoLine("}");
                }


                if (_formOptions != null)
                {
                    _formError.AddInfoLine(Environment.NewLine + "FormOptions information.  {");
                    List<string> information = _formOptions.formOptionsInformation();

                    foreach (string info in information)
                    {
                        _formError.AddInfoLine(info);
                    }
                    _formError.AddInfoLine("}");
                }
                
                
                _formError.CriticalError = criticalError;
                ShowErrorWindow(criticalError);
            }
            catch (Exception ex2)
            {
                MessageBox.Show("Woah hold on a sec. En error occurred in the error handling?. Damm this is bad. Please report this error as soon as possible" + Environment.NewLine + "Error: " + ex2.Message + Environment.NewLine + ex.StackTrace, "An Unlikely error occurred :(", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //Events

        private void FormMain_Load(object sender, EventArgs e)
        {
            try
            {
#if(DEBUG)
                buttonDebug.Visible = true;
                textBoxDebug.Visible = true;
#endif

                _globalPackDir = Application.StartupPath + "\\Minecraft Packs";
                
                GenerateTweetList();                
                StartCheckingServers();

                _formOptions = new FormOptions(this);


                _loadingSettings = true;

                foreach (string folder in Directory.GetDirectories(_globalPackDir))
                    comboBoxPackSelect.Items.Add(folder.Replace(_globalPackDir + "\\", ""));
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
                    if (IsNetworkAvailable())
                    {
                        if (_formNews == null || _formNews.IsDisposed)
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
                    if (IsNetworkAvailable())
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
                //ErrorReporting("FormMain_Load()", information, true);

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
            //if (pingKBGEventThread != null)
            //    pingKBGEventThread.Abort();
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
            //if (pingKBGEventThread != null)
            //    while (pingKBGEventThread.IsAlive) ;
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
            #region OldOldTestCode
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
            //    newpack.DownloadUrl = "urlString";
            //    packConfigSection.Packs.Add(newpack);
            //    //packConfigSection.sa
            //    //for (int i = 0; i < packConfigSection.Packs.Count; i++)
            //    //{
                    

            //    //    packConfigSection.Packs[0].Name = "name1";
            //    //    packConfigSection.Packs[0].Version = 0.4;
            //    //    packConfigSection.Packs[0].VersionFileName = "versionfilename1";
            //    //    packConfigSection.Packs[0].VersionUrl = "versionurl";
            //    //    packConfigSection.Packs[0].DownloadUrl = "urlString";
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
            //PackClass packClass = packConfigSection.Packs[0]
            
            #endregion OldTestCode

            DateTime old = DateTime.Now;

            try
            {
                if (_formOptions.Settings == null)
                    _formOptions.Settings = new xmlSettings();
                //_settings.CreateNewSettingsFile();
                //_settings.Username = "HAHAHA";
                
                #region OldTestCode



                //string asdf = _formOptions.Settings.Username;
                //TimeSpan span = DateTime.Now - old;
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
                //throw new Exception("DEMO DEMO critical exception");
                //MessageBox.Show(double.Parse(textBoxDebug.Text, System.Globalization.NumberFormatInfo.InvariantInfo).ToString());
                //File.Replace(Application.StartupPath + "\\KBG Minecraft Launcher2.exe", Application.StartupPath + "\\KBG Minecraft Launcher.exe", Application.StartupPath + "\\KBG Minecraft Launcher.backup", true);


                //OAuthTokens tokens = new OAuthTokens();
                //tokens.ConsumerKey = "Consumer Key";
                //tokens.ConsumerSecret = "Consumer Secret";
                //tokens.AccessToken = "Access Key";
                //tokens.AccessTokenSecret = "Access Secret";

                //TwitterStatusCollection homeTimeline = TwitterStatus.GetHomeTimeline(tokens);

                //HttpUtility   .UrlEncodeUnicode  = "";
                //tnnArxj06cWHq44gCs1OSKk/jLY=
                //System.Net.WebUtility.HtmlEncode("tnnArxj06cWHq44gCs1OSKk/jLY=");
                //System.Web. 


                //OAuth oauth_consumer_key="xvz1evFS4wEEPTGEFPHBog", 
                //      oauth_nonce="kYjzVBB8Y0ZFabxSWbWovY3uYSQ2pTgmZeNu2VS4cg", 
                //      oauth_signature="tnnArxj06cWHq44gCs1OSKk%2FjLY%3D", 
                //      oauth_signature_method="HMAC-SHA1", 
                //      oauth_timestamp="1318622958", 
                //      oauth_token="370773112-GmHxMAgYyLbNEtIKZeRNFsMKPR9EyMZeS9weJAEb", 
                //      oauth_version="1.0"


                /*
                step 1. Gather information part 1
                step 2. Gather information part 2
                step 3. Collecting parameters
                 * Gather information part 3
                step 4. Creating the signature base string
                 * combine part 1, part 2, part 3 in a 'signature base string'
                step 5. Getting a signing key
                 * get Consumer secret (application secret) and OAuth token secret
                step 6. Calculating the signature

                //screen_name=KB_Gaming&count=12"

                */
                //GS - Get the oAuth params
                //string status = "your status";
                //string postBody = "status=" + Uri.EscapeDataString(status);

                /*
                WebRequest request = WebRequest.Create(get.AbsoluteUri + args);
                request.Method = "GET";
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        XmlTextReader reader = new XmlTextReader(stream);
                        ...
                    }
                }




                */
                /*
              string oauth_consumer_key = "yZn137jIlAEmy1e6Gcqztg";
              //string oauth_nonce = Convert.ToBase64String( new ASCIIEncoding().GetBytes( DateTime.Now.Ticks.ToString()));
              string oauth_nonce = "YSQ2pTgmZeNu2VS4cg" + Convert.ToBase64String( new ASCIIEncoding().GetBytes( DateTime.Now.Ticks.ToString()));

              string oauth_signature_method = "HMAC-SHA1";
              string oauth_token = "1121629825-olJ9TeIvv7tYhzojfZhMuv09449oGjyExxaz1tN";

              TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

              string oauth_timestamp = Convert.ToInt64(ts.TotalSeconds).ToString();

              string oauth_version = "1.0";

              //GS - When building the signature string the params
              //must be in alphabetical order. I can't be bothered
              //with that, get SortedDictionary to do it's thing
              SortedDictionary<string, string> sd = new SortedDictionary<string, string>();

              //sd.Add("status", status);
              sd.Add("oauth_version", oauth_version);
              sd.Add("oauth_consumer_key", oauth_consumer_key);
              sd.Add("oauth_nonce", oauth_nonce);
              sd.Add("oauth_signature_method", oauth_signature_method);
              sd.Add("oauth_timestamp", oauth_timestamp);
              sd.Add("oauth_token", oauth_token);

              //GS - Build the signature string
              string baseString = "GET" + "&" + EscapeUriDataStringRfc3986("http://api.twitter.com/1.1/statuses/user_timeline.json") + "&";

              foreach (KeyValuePair<string, string> entry in sd)
              {
                  baseString += EscapeUriDataStringRfc3986(entry.Key + "=" + entry.Value + "&");
              }
              baseString += EscapeUriDataStringRfc3986("screen_name=JonasBalling&");//KB_Gaming
              baseString += EscapeUriDataStringRfc3986("count=12");


              //GS - Remove the trailing ambersand char, remember 
              //it's been urlEncoded so you have to remove the 
              //last 3 chars - %26
              //baseString = baseString.Substring(0, baseString.Length - 3);

              //GS - Build the signing key
              string consumerSecret = rm.GetString("consumerSecret");

              string oauth_token_secret = rm.GetString("oauth_token_secret");

              string signingKey = EscapeUriDataStringRfc3986(consumerSecret) + "&" + EscapeUriDataStringRfc3986(oauth_token_secret);

              //GS - Sign the request
              HMACSHA1 hasher = new HMACSHA1(new ASCIIEncoding().GetBytes(signingKey));

              string signatureString = Convert.ToBase64String(hasher.ComputeHash(new ASCIIEncoding().GetBytes(baseString)));
                



              //GS - Tell Twitter we don't do the 100 continue thing
              ServicePointManager.Expect100Continue = false;

              //GS - Instantiate a web request and populate the 
              //authorization header
              //HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(@"https://api.twitter.com/1.1/statuses/user_timeline.json");

              string authorizationHeaderParams = String.Empty;
              authorizationHeaderParams += "OAuth ";

              authorizationHeaderParams += "oauth_consumer_key=" + "\"" + EscapeUriDataStringRfc3986(oauth_consumer_key) + "\",";

              authorizationHeaderParams += "oauth_nonce=" + "\"" + EscapeUriDataStringRfc3986(oauth_nonce) + "\",";

              authorizationHeaderParams += "oauth_signature=" + "\"" + EscapeUriDataStringRfc3986(signatureString) + "\",";

              authorizationHeaderParams += "oauth_signature_method=" + "\"" + EscapeUriDataStringRfc3986(oauth_signature_method) + "\",";

              authorizationHeaderParams += "oauth_timestamp=" + "\"" + EscapeUriDataStringRfc3986(oauth_timestamp) + "\",";

              authorizationHeaderParams += "oauth_token=" + "\"" + EscapeUriDataStringRfc3986(oauth_token) + "\",";

              authorizationHeaderParams += "oauth_version=" + "\"" + EscapeUriDataStringRfc3986(oauth_version) + "\"";

              //hwr.Headers.Add("Authorization", authorizationHeaderParams);

                
              //hwr.Method = "GET";
              //hwr.ContentType = "application/x-www-form-urlencoded";


                


              HttpWebRequest request = WebRequest.Create(@"http://api.twitter.com/1.1/statuses/user_timeline.json") as HttpWebRequest;
              request.KeepAlive = false;
              request.Method = "GET";
              request.ServicePoint.Expect100Continue = false;
              request.Headers.Add("Authorization", authorizationHeaderParams);
              //request.ContentType = "application/x-www-form-urlencoded";

              using (WebResponse response = request.GetResponse())
              {
                  using (Stream stream = response.GetResponseStream())
                  {
                      XmlTextReader reader = new XmlTextReader(stream);
                  }
              }
                

              //using (Stream stream = hwr.GetRequestStream())
              //{

              //    //Stream stream = hwr.GetRequestStream();
              //    byte[] bodyBytes = new ASCIIEncoding().GetBytes("");

              //    stream.Write(bodyBytes, 0, bodyBytes.Length);
              //}
              //stream.Flush();
              //stream.Close();

              //GS - Allow us a reasonable timeout in case
              //Twitter's busy
              //hwr.Timeout = 3 * 60 * 1000;

              try
              {
                  //HttpWebResponse rsp = hwr.GetResponse() as HttpWebResponse;
                  //GS - Do something with the return here...
              }
              catch (WebException ex)
              {
                  //GS - Do some clever error handling here...
              }*/
                //KBG_Launcher.Properties.Resources. .Properties.Resources

                System.Resources.ResourceManager rm = new System.Resources.ResourceManager("KBG_Launcher.Properties.Secrets", System.Reflection.Assembly.GetExecutingAssembly());
                rm.GetString("consumerSecret");
                #endregion

                #region oAuthTestCode
                /*
                string consumerKey = "ABCD7EM9qNGwQmRBxCcX";
                string consumerSecret = rm.GetString("consumerSecret");
                string callBackUrl = "http://localhost/mycallbackurl.aspx";

                // Use Hammock to set up our authentication credentials
                OAuthCredentials credentials = new OAuthCredentials()
                {
                    Type = OAuthType.RequestToken,
                    SignatureMethod = OAuthSignatureMethod.HmacSha1,
                    ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                    ConsumerKey = consumerKey,
                    ConsumerSecret = consumerSecret,
                    CallbackUrl = callBackUrl
                };

                // Use Hammock to create a rest client
                var client = new RestClient
                {
                    Authority = "http://twitter.com/oauth",
                    Credentials = credentials
                };

                // Use Hammock to create a request
                var request = new RestRequest
                {
                    Path = "request_token"
                };

                // Get the response from the request
                var response = client.Request(request);


                var urlString = SERVICE_SPECIFIC_AUTHORIZE_URL_STUB + oauth["token"];
                webBrowser1.Url = new Uri(urlString);
                */
#endregion



                /*
                1. normal backup proceadure (old code)
                2. delete .minecraft folder (old code)
                3. download needed minecraft jar files from Mojang (by selected version)
                4. Remove META-INF from Minecraft.jar
                5. Download jar-modifier zip.
                6. add jar-modifier zip to minecraft.jar
                7. download Mod-pack zip (same as old pack, just without the Bin folder)
                8. copy backup files back to pack folder.
                
                 
                 
                 
                 */
                //StartGameOffline(comboBoxPackSelect.SelectedItem.ToString());


                throw new Exception("TEST");
                //using (ZipFile zip1 = ZipFile.Read(@"C:\Users\Phoenix\Desktop\Programering\VS 2010\KBG Minecraft Launcher\KBG Launcher\bin\Debug\minecraft2.jar"))
                //{
                //    //zip["META-INF"] = null;
                //    //zip.RemoveEntry(zip.UpdateDirectory("META-INF"));
                //    List<ZipEntry> selection = new List<ZipEntry>(zip1.SelectEntries("*.*", "META-INF"));
                //    for (int x = selection.Count - 1; x >= 0; x--)
                //    {
                //        ZipEntry entry = selection[x];
                //        zip1.RemoveEntry(entry.FileName);
                //    }
                //    zip1.Save();
                //}
                DateTime start = DateTime.Now;
                //InjectZipToJar(@"C:\Users\Phoenix\Desktop\Programering\VS 2010\KBG Minecraft Launcher\KBG Launcher\bin\Debug\minecraft2.jar", @"C:\Users\Phoenix\Desktop\Programering\VS 2010\KBG Minecraft Launcher\KBG Launcher\bin\Debug\testTo.zip");
                //MessageBox.Show(InjectZipToJar(@"C:\Users\Phoenix\Desktop\Programering\VS 2010\KBG Minecraft Launcher\KBG Launcher\bin\Debug\minecraft2.jar", @"C:\Users\Phoenix\Desktop\Programering\VS 2010\KBG Minecraft Launcher\KBG Launcher\bin\Debug\testTo.zip").ToString());
                ExtractFile(_globalPackDir + "\\windows_natives.jar", _globalPackDir + "\\test\\natives");
                TimeSpan span = DateTime.Now - start;
                MessageBox.Show(span.TotalMilliseconds.ToString());
            }
            catch (Exception ex)
            {
                //ex.Data.Add("button1_Click_1() start info", "");
                //ex.Data.Add("F U ALL :P", "value");
                ErrorReporting(ex, false);
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

                    linkLabelCredits.Visible = _formOptions.CheckNameForAutoUpdateSupport(comboBoxPackSelect.SelectedItem.ToString());                         
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
                bool minecraftJarFound = File.Exists(_globalPackDir + "\\" + comboBoxPackSelect.SelectedItem.ToString() + "\\.minecraft\\bin\\minecraft.jar");


                if (_formOptions.CheckNameForAutoUpdateSupport(selItem))
                {
                    if (HasInternetConnection())
                    {
                        if (minecraftJarFound)
                        {
                            //check for update
                            labelDownload.Text = "Checking for updates";
                            progressBarDownload.Style = ProgressBarStyle.Marquee;
                            panelDownload.Visible = true;

                            this.Update();
                            //check update
                            xmlVersionInfo OnlineInfo = _formOptions.GetVersionInfo(selItem, true);
                            bool UpdateFound = VersionInfo1LargerThenInfo2(OnlineInfo, _formOptions.GetVersionInfo(selItem, false));

                            //check for PreventPackDownload
                            if (OnlineInfo.PreventPackDownload)
                            {
                                MessageBox.Show(string.Format("Downloading and updating of this pack have been disabled remotely. {0}This is most likely because xKillerBees is updating the pack.{0}Please try again later, or look for more information on his twitter", Environment.NewLine), "Download disabled remotely", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                if (UpdateFound)
                                {
                                    if (MessageBox.Show("A new version of the " + selItem + " pack was found. Do you want to update now?", "New update found", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                                    {
                                        progressBarDownload.Style = ProgressBarStyle.Blocks;
                                        InstallPackStarter(selItem, true);
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
                            }

                            progressBarDownload.Style = ProgressBarStyle.Blocks;
                            panelDownload.Visible = false;


                        }
                        else
                        {
                            //download and install update  
                            if (_formOptions.GetVersionInfo(selItem, true).PreventPackDownload)
                            {
                                MessageBox.Show(string.Format("Downloading and updating of this pack have been disabled remotely. {0}This is most likely because xKillerBees is updating the pack.{0}Please try again later, or look for more information on his twitter", Environment.NewLine), "Download disabled remotely", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                InstallPackStarter(selItem, true);
                            }
                        }
                    }
                    else
                    {
                        if (MessageBox.Show("No Internet connection could be found." + Environment.NewLine + "Do you want to start the game in offline mode?", "Connection problems. Play Offline?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Yes)
                        {
                            StartGameOffline(selItem);
                        }
                    }
                    //eigher incorrectly installed pack (manually) or not installed automatic update pack

                    //check name for SupportetAutoUpdatePack
                    //download and install SupportetAutoUpdatePack
                }
                else
                {
                    if (minecraftJarFound)
                    {
                        if (HasInternetConnection())
                        {
                            StartGame(selItem);
                        }
                        else
                        {
                            if (MessageBox.Show("No Internet connection could be found." + Environment.NewLine + "Do you want to start the game in offline mode?", "Connection problems. Play Offline?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Yes)
                            {
                                StartGameOffline(selItem);
                            }
                        }
                    }
                    else
                        MessageBox.Show("Incorrectly installed Minecraft pack. Use the format: " + Environment.NewLine + _globalPackDir + "\\<PackName>\\.Minecraft\\bin\\minefraft.jar", "Incorrect minecraft installation detected", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    //throw new Exception("incorrectly installed Minecraft pack. Use the format " + _globalPackDir + "\\<PackName>\\.Minecraft\\bin\\minefraft.jar");
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
        
        private void buttonRefreshTwitterFeeds_Click(object sender, EventArgs e)
        {
            _TweetList.Clear();
            richTextBox1.Text = "";
            GenerateTweetList();
        }

        private void linkLabelKB_Gaming_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://twitter.com/intent/user?screen_name=KB_Gaming");
            }
            catch (Exception ex)
            {
                ErrorReporting(ex, false);
            }
        }

        private void linkLabelCredits_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (_formCredits == null || _formCredits.IsDisposed)
                    _formCredits = new FormCredits();

                //string selitem = comboBoxPackSelect.SelectedItem.ToString();
                //_formOptions.GetVersionInfo(selitem, false);
                //xmlVersionInfo versionInfo = _formOptions.GetVersionInfo(comboBoxPackSelect.SelectedItem.ToString(), true);

                //if (versionInfo.Credits != "")
                //    _formCredits.SetCredits(versionInfo.Credits);
                //else
                //    _formCredits.SetCredits(@"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Microsoft Sans Serif;}}\viewkind4\uc1\pard\f0\fs17 Credits Missing\par}");
                _formCredits.GetAndShowCredits(comboBoxPackSelect.SelectedItem.ToString(), _formOptions);
                _formCredits.Show();
            }
            catch (Exception ex)
            {
                ErrorReporting(ex, false);
            }
        }


        private void buttonPlayOffline_Click(object sender, EventArgs e)
        {
            try
            {
                StartGameOffline(comboBoxPackSelect.SelectedItem.ToString());
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
                            //this.groupBoxServerStatus.Enabled = !value;
                            //this.groupBoxTwitter.Enabled = !value;
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
        private void SetDownloadLabelTextMain(string text)
        {
            SetDownloadLabelText(text, "", "", "");
        }

        /// <summary>
        /// Designed to be called from a seperate thread from the main thread (multi threading)
        /// </summary>
        /// <param name="value"></param>
        /// 
        private void SetDownloadLabelTextSub(string text)
        {
            try
            {
                if (!CloseAllThreads)
                    this.Invoke(new Action(delegate()
                    {                        
                        this.labelDownloadSub.Text = text;
                    }));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void SetDownloadLabelText(string text, string textSub, string speed, string progress)
        {
            try
            {
                if (!CloseAllThreads) 
                    this.Invoke(new Action(delegate()
                                                {
                                                    this.labelDownload.Text = text;
                                                    this.labelDownloadSub.Text = textSub;
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
                    if(value > 0)
                        this.Invoke(new Action(delegate() { this.progressBarDownload.Value = value; }));
                    else
                        this.Invoke(new Action(delegate() { this.progressBarDownload.Value = 0; }));

                if(_useTaskbarProgressBar)
                {
                    TaskbarItemInfo Tii = new TaskbarItemInfo();
                    Tii.ProgressState = TaskbarItemProgressState.Normal;
                    Tii.ProgressValue = value;
                    

                }
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

        //private void SetOfflineTwitterStatus(bool value)
        //{
        //    try
        //    {
        //        if (!CloseAllThreads)
        //            this.Invoke(new Action(delegate()
        //            {
        //                this.labelNoTwitterConnection.Visible = value;
        //                this.progressBarTwitter.Visible = !value;
        //            }));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}




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
        //    Uri urlString = new Uri((e.Argument as BackgroundWorkerArgumentWrapper).Url);            
        //    string filename = urlString.OriginalString.Substring(urlString.OriginalString.LastIndexOf("/") + 1, urlString.OriginalString.Length - urlString.OriginalString.LastIndexOf("/") - 1);            

        //    // first, we need to get the exact size (in bytes) of the file we are downloading            
        //    System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(urlString);
        //    SetAllowUnsafeHeaderParsing20();
        //    System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
        //    response.Close();

        //    // gets the size of the file in bytes
        //    Int64 RemoteFileSize = response.ContentLength;

        //    // keeps track of the total bytes downloaded so we can update the progress bar
        //    Int64 iRunningByteTotal = 0;

        //    // use the webclient object to download the file
        //    using (System.Net.WebClient client = new System.Net.WebClient())            
        //    {
        //        // open the file at the remote URL for reading
        //        using (System.IO.Stream streamRemote = client.OpenRead(urlString))
        //        {
        //            // using the FileStream object, we can write the downloaded bytes to the file system
        //            using (Stream streamLocal = new FileStream(Application.StartupPath + "\\Minecraft Packs\\" + filename, FileMode.Create, FileAccess.Write, FileShare.None))                    
        //            {
        //                // loop the stream and get the file into the byte buffer
        //                int iByteSize = 0;
        //                byte[] byteBuffer = new byte[RemoteFileSize];
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
            
        //    using (ZipFile zip = ZipFile.Read(source))
        //    {
        //        //foreach (ZipEntry e in zip)
        //        for(int i=0;i < zip.Count;i++)
        //        {
        //            dProgressPercentage = (i / zip.Count);
        //            this.Invoke(new Action(delegate() { progressBarDownload.Value = (int)(dProgressPercentage * 100); }));
        //            //progressBarDownload.Value = (int)(dProgressPercentage * 100);
        //            zip[i].Extract(destination, ExtractExistingFileAction.OverwriteSilently);                    
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
        //    if (!File.Exists(_globalPackDir + "\\" + comboBoxPackSelect.SelectedItem.ToString() + "\\bin\\minecraft.jar"))
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


    public class PKCSKeyGenerator
    {
        /// <summary>
        /// Key used in the encryption algorythm.
        /// </summary>
        private byte[] key = new byte[8];

        /// <summary>
        /// IV used in the encryption algorythm.
        /// </summary>
        private byte[] iv = new byte[8];

        /// <summary>
        /// DES Provider used in the encryption algorythm.
        /// </summary>
        private DESCryptoServiceProvider des = new DESCryptoServiceProvider();

        /// <summary>
        /// Initializes a new instance of the PKCSKeyGenerator class.
        /// </summary>
        public PKCSKeyGenerator()
        {
        }

        /// <summary>
        /// Initializes a new instance of the PKCSKeyGenerator class.
        /// </summary>
        /// <param name="keystring">This is the same as the "password" of the PBEWithMD5AndDES method.</param>
        /// <param name="salt">This is the salt used to provide extra security to the algorythim.</param>
        /// <param name="iterationsMd5">Fill out iterationsMd5 later.</param>
        /// <param name="segments">Fill out segments later.</param>
        public PKCSKeyGenerator(string keystring, byte[] salt, int iterationsMd5, int segments)
        {
            this.Generate(keystring, salt, iterationsMd5, segments);
        }

        /// <summary>
        /// Gets the asymetric Key used in the encryption algorythm.  Note that this is read only and is an empty byte array.
        /// </summary>
        public byte[] Key
        {
            get
            {
                return this.key;
            }
        }

        /// <summary>
        /// Gets the initialization vector used in in the encryption algorythm.  Note that this is read only and is an empty byte array.
        /// </summary>
        public byte[] IV
        {
            get
            {
                return this.iv;
            }
        }

        /// <summary>
        /// Gets an ICryptoTransform interface for encryption
        /// </summary>
        public ICryptoTransform Encryptor
        {
            get
            {
                return this.des.CreateEncryptor(this.key, this.iv);
            }
        }

        /// <summary>
        /// Gets an ICryptoTransform interface for decryption
        /// </summary>
        public ICryptoTransform Decryptor
        {
            get
            {
                return des.CreateDecryptor(key, iv);
            }
        }

        /// <summary>
        /// Returns the ICryptoTransform interface used to perform the encryption.
        /// </summary>
        /// <param name="keystring">This is the same as the "password" of the PBEWithMD5AndDES method.</param>
        /// <param name="salt">This is the salt used to provide extra security to the algorythim.</param>
        /// <param name="iterationsMd5">Fill out iterationsMd5 later.</param>
        /// <param name="segments">Fill out segments later.</param>
        /// <returns>ICryptoTransform interface used to perform the encryption.</returns>
        public ICryptoTransform Generate(string keystring, byte[] salt, int iterationsMd5, int segments)
        {
            // MD5 bytes
            int hashLength = 16;

            // to store contatenated Mi hashed results
            byte[] keyMaterial = new byte[hashLength * segments];

            // --- get secret password bytes ----
            byte[] passwordBytes;
            passwordBytes = Encoding.UTF8.GetBytes(keystring);

            // --- contatenate salt and pswd bytes into fixed data array ---
            byte[] data00 = new byte[passwordBytes.Length + salt.Length];

            // copy the pswd bytes
            Array.Copy(passwordBytes, data00, passwordBytes.Length);

            // concatenate the salt bytes
            Array.Copy(salt, 0, data00, passwordBytes.Length, salt.Length);

            // ---- do multi-hashing and contatenate results  D1, D2 ...  into keymaterial bytes ----
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = null;

            // fixed length initial hashtarget
            byte[] hashtarget = new byte[hashLength + data00.Length];

            for (int j = 0; j < segments; j++)
            {
                // ----  Now hash consecutively for iterationsMd5 times ------
                if (j == 0)
                {
                    // initialize
                    result = data00;
                }
                else
                {
                    Array.Copy(result, hashtarget, result.Length);
                    Array.Copy(data00, 0, hashtarget, result.Length, data00.Length);
                    result = hashtarget;
                }

                for (int i = 0; i < iterationsMd5; i++)
                {
                    result = md5.ComputeHash(result);
                }

                // contatenate to keymaterial
                Array.Copy(result, 0, keyMaterial, j * hashLength, result.Length);
            }

            Array.Copy(keyMaterial, 0, this.key, 0, 8);
            Array.Copy(keyMaterial, 8, this.iv, 0, 8);

            return this.Encryptor;
        }
    }

    public class LastLogin
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class ThreadParameterPack
    {
        public string SelectedItem { get; set; }
        public bool StartGameAfterInstall { get; set; }
        public ThreadParameterPack() { }
        public ThreadParameterPack( string selectedItem, bool startGameAfterInstall) 
        {
            SelectedItem = selectedItem;
            StartGameAfterInstall = startGameAfterInstall;
        }
    }
}

