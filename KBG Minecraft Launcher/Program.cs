using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace KBG_Minecraft_Launcher
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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }
    }
}
