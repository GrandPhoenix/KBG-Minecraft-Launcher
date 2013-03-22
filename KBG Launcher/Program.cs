using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace KBG_Launcher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //AppDomain.CurrentDomain.UnhandledException += delegate(object sender, UnhandledExceptionEventArgs args)
            //{
            //    var exception = (Exception)args.ExceptionObject;
            //    FileStream fs = new FileStream("Debug1.txt", FileMode.Create);
            //    // First, save the standard output.
            //    TextWriter tmp = Console.Out;
            //    StreamWriter sw = new StreamWriter(fs);
            //    Console.SetOut(sw);
            //    Console.WriteLine("Unhandled exception: " + exception);
            //    sw.Close();
            //    Environment.Exit(1);
            //};
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormMain());
            }
            catch (Exception ex)
            {
                String ErrorMessage = "";
                string clientVersion = "";

                ErrorMessage += string.Format("Error: {0}", ex.Message);
                ErrorMessage += string.Format("{0}{0}source: {1}", Environment.NewLine, ex.Source);
                ErrorMessage += string.Format("{0}{0}Stack Trace: {1}", Environment.NewLine, ex.StackTrace);
                ErrorMessage += string.Format("{0}{0}InnerException: {1}{0}", Environment.NewLine, ex.InnerException);

                try
                {
                    Assembly ass = Assembly.GetExecutingAssembly();
                    if (ass != null)
                    {
                        FileVersionInfo FVI = FileVersionInfo.GetVersionInfo(ass.Location);
                        clientVersion = string.Format("{0}.{1}.{2}.{3}", FVI.FileMajorPart, FVI.FileMinorPart, FVI.FileBuildPart, FVI.FilePrivatePart);
                    }
                }
                catch (Exception exz) { }

                if (clientVersion == null)
                    ErrorMessage += string.Format("{0}Client Version: clientVersion was null",Environment.NewLine);
                else
                    ErrorMessage += string.Format("{0}Client Version: {1}", Environment.NewLine,clientVersion);

                if (System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE") != null)
                    ErrorMessage += string.Format("{0}PROCESSOR_ARCHITECTURE =  {1}",Environment.NewLine,System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE"));
                    
                else
                    ErrorMessage += string.Format("{0}PROCESSOR_ARCHITECTURE =  null", Environment.NewLine);




                using (FileStream fstream = new FileStream(Application.StartupPath + "\\StartupErrorLog.txt", FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    using (TextWriter writer = new StreamWriter(fstream))
                    {
                        writer.Write(ErrorMessage);
                    }
                }
            }
        }
    }
}
