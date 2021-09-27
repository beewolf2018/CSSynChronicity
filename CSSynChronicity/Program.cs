using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using CSSynChronicity.Interface;
using System.Windows.Forms;
using log4net;

namespace CSSynChronicity
{
    static class Program
    {
        static ILog log;
        //internal static LanguageHandler Translation;
        //internal static ConfigHandler ProgramConfig;
        //internal static Dictionary<string, ProfileHandler> Profiles;
        //internal static bool ReloadNeeded;
        //internal static MainForm MainFormInstance;

        //internal static MessageLoop MsgLoop;
        //internal delegate void Action(); // LATER: replace with .Net 4.0 standards.
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {

            log4net.Config.XmlConfigurator.Configure();
            log = LogManager.GetLogger(typeof(Program));
            log.Info(new String('*', 40));
            log.Info("主程序初始化开始!");


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            try
            {

                Application.Run(new MainForm());

                //MsgLoop = new MessageLoop();
                //if (!MsgLoop.ExitNeeded)
                //    Application.Run(MsgLoop);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
    }
}
