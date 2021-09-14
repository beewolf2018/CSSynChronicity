using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CSSynChronicity
{
    public class ProgramSetting
    {
        public const string Language = "Language";
        public const string DefaultLanguage = "english";
        public const string AutoUpdates = "Auto updates";
        public const string MaxLogEntries = "Archived log entries";
        public const string MainView = "Main view";
        public const string FontSize = "Font size";
        public const string MainFormAttributes = "Window size and position";
        public const string ExpertMode = "Expert mode";
        public const string DiffProgram = "Diff program";
        public const string DiffArguments = "Diff arguments";
        public const string TextLogs = "Text logs";
        public const string Autocomplete = "Autocomplete";
        public const string Forecast = "Forecast";
        public const string Pause = "Pause";
        public const string AutoStartupRegistration = "Auto startup registration";

        //'Program files
        public const string ConfigFolderName = "config";
        public const string LogFolderName = "log";
        public const string SettingsFileName = "mainconfig.ini";
        public const string AppLogName = "app.log";
        public const string DllName = "compress-decompress.dll";
        //'Public CompressionThreshold As Integer = 0 'Better not filter at all

        public const string ExcludedFolderPrefix = "folder";// 'Used to parse excluded file types. For example, `folder"Documents"` means that folders named documents should be excluded.
        public const char GroupPrefix = ':';
        public const char EnqueuingSeparator = '|';

        public const char DirSep = '\\';

        public const bool Debug = false;
        public const int ForecastDelay = 60;


        public const int AppLogThreshold = 1 << 23; //'8 MB

        public const string RegistryBootVal = "Create Synchronicity - Scheduler";
        public const string RegistryBootKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Run";
        public const string RegistryRootedBootKey = "HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Run";
    }

    public class ConfigHandler
    {
        private static ConfigHandler Singleton;

        public string LogRootDir;
        public string ConfigRootDir;
        public string LanguageRootDir;

        public string CompressionDll;
        public string LocalNamesFile;
        public string MainConfigFile;
        public string AppLogFile;
        public string StatsFile;

        public bool CanGoOn = true;// 'To check whether a synchronization is already running (in scheduler mode only, queuing uses callbacks).

        public Icon Icon;
        public bool SettingsLoaded = false;
        public Dictionary<String, String> Settings = new Dictionary<string, string>();

        public ConfigHandler()
        {
            LogRootDir = GetUserFilesRootDir() + ProgramSetting.LogFolderName;
            ConfigRootDir = GetUserFilesRootDir() + ProgramSetting.ConfigFolderName;
            LanguageRootDir = Application.StartupPath + ProgramSetting.DirSep + "languages";

            StatsFile = ConfigRootDir + ProgramSetting.DirSep + "syncs-count.txt";
            LocalNamesFile = LanguageRootDir + ProgramSetting.DirSep + "local-names.txt";
            MainConfigFile = ConfigRootDir + ProgramSetting.DirSep + ProgramSetting.SettingsFileName;
            CompressionDll = Application.StartupPath + ProgramSetting.DirSep + ProgramSetting.DllName;
            AppLogFile = GetUserFilesRootDir() + ProgramSetting.AppLogName;

            try
            {

                Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            }
            catch (ArgumentException ex)
            {
                Icon = Icon.FromHandle(new Bitmap(32, 32).GetHicon());
            }

            //TrimAppLog() //Prevents app log from getting too large.

        }

        private string GetUserFilesRootDir()
        {
            string UserFilesRootDir = "";
            if (UserFilesRootDir != "") return UserFilesRootDir;

            string UserPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + ProgramSetting.DirSep + Branding.Brand + ProgramSetting.DirSep + Branding.Name + ProgramSetting.DirSep;

            // To change folder attributes: http://support.microsoft.com/default.aspx?scid=kb;EN-US;326549
            List<string> WriteNeededFolders = new List<string>() {
                Application.StartupPath,
                Application.StartupPath + ProgramSetting.DirSep + ProgramSetting.LogFolderName,
                Application.StartupPath +ProgramSetting.DirSep + ProgramSetting.ConfigFolderName
                };
            bool ProgramPathExists = Directory.Exists(Application.StartupPath + ProgramSetting.DirSep + ProgramSetting.ConfigFolderName);
            List<string> ToDelete = new List<string>();
            try
            {
                foreach (var Folder in WriteNeededFolders)
                {
                    if (!Directory.Exists(Folder)) continue;

                    string TestPath = Folder + ProgramSetting.DirSep + "write-permissions." + Path.GetRandomFileName();
                    System.IO.File.Create(TestPath).Close();
                    ToDelete.Add(TestPath);

                    if (Folder == Application.StartupPath) continue;
                    foreach (var file in Directory.GetFiles(Folder))
                    {
                        if ((System.IO.File.GetAttributes(file) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly) throw new System.IO.IOException(file);
                    }
                }

                foreach (var TestFile in ToDelete)
                {
                    try
                    {
                        System.IO.File.Delete(TestFile);
                    }
                    catch (IOException ex)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(System.UnauthorizedAccessException))
                {

                    if (ProgramPathExists) { }
                    //If ProgramPathExists Then ProgramPathExists.ShowMsg("Create Synchronicity cannot write to your installation directory, although it contains configuration files. Your Application Data folder will therefore be used instead." & Environment.NewLine & Ex.Message, "Information", , MessageBoxIcon.Information)
                    return UserPath;
                }
            }
            // When a user folder exists, and no config folder exists in the install dir, use the user's folder.
            if (ProgramPathExists || !System.IO.Directory.Exists(UserPath))
            {
                return Application.StartupPath + ProgramSetting.DirSep;
            }
            else
            {
                return UserPath;
            }
        }

        public static ConfigHandler GetSingleton()
        {
            if (Singleton == null)
            {
                Singleton = new ConfigHandler();
            }
            return Singleton;
        }

        public string GetConfigPath(string Name)
        {
            return ConfigRootDir + ProgramSetting.DirSep + Name + ".sync";
        }


        public string GetLogPath(string Name)
        {
            return LogRootDir + ProgramSetting.DirSep + Name + ".log";
        }


        public string GetErrorsLogPath(string Name)
        {
            return LogRootDir + ProgramSetting.DirSep + Name + ".errors.log";
        }

        public T GetProgramSetting<T>(string Key, T DefaultVal)
        {
            string Val = "";
            Settings.TryGetValue(Key, out Val);
            if (!string.IsNullOrEmpty(Val))
            {
                try
                {
                    //Object retObj = Val as Object;
                    //T ret = (T)retObj;
                    return (T)((Object) Val); 
                }
                catch (Exception ex)
                {
                    SetProgramSetting<T>(Key, DefaultVal);
                }
            }
            return DefaultVal;
        }
        public void SetProgramSetting<T>(string Key, T Value)
        {
            Settings[Key] = Value.ToString();
        }



        public void LoadProgramSettings()
        {
            if (SettingsLoaded) return;
            Directory.CreateDirectory(ConfigRootDir);
            if (!File.Exists(MainConfigFile))
            {
                File.Create(MainConfigFile).Close();
                return;
            }
            string ConfigString;
            try
            {
                ConfigString = File.ReadAllText(MainConfigFile);
            }
            catch (IOException ex)
            {
                System.Threading.Thread.Sleep(200);
                ConfigString = File.ReadAllText(MainConfigFile);

            }
            foreach (string Setting in ConfigString.Split(';'))
            {
                string[] Pair = Setting.Split(":".ToCharArray(), 2);
                if (Pair.Length < 2) continue;
                if (Settings.ContainsKey(Pair[0])) Settings.Remove(Pair[0]);
                Settings.Add(Pair[0].Trim(), Pair[1].Trim());
            }
            SettingsLoaded = true;
        }

        public void SaveProgramSettings()
        {
            StringBuilder ConfigStrB = new StringBuilder();
            foreach (var Setting in Settings)
            {
                ConfigStrB.AppendFormat("{0}:{1};", Setting.Key, Setting.Value);
            }
            try
            {
                File.WriteAllText(MainConfigFile, ConfigStrB.ToString());
            }
            catch (Exception ex)
            {

            }
        }

        public bool ProgramSettingsSet(string Setting)
        {
            return Settings.ContainsKey(Setting);
        }

        public void LogDebugEvent(string EventData)
        {
            LogAppEvent(EventData);
        }


        private void TrimAppLog()
        {
            if(File.Exists(AppLogFile) && Utilities.GetSize(AppLogFile) > ProgramSetting.AppLogThreshold)
            {
                string AppLogBackup = AppLogFile + ".old";
                if(File.Exists(AppLogBackup))
                {
                    File.Delete(AppLogBackup);
                }
                File.Move(AppLogFile, AppLogBackup);
                LogAppEvent("Moved " + AppLogFile + " to " + AppLogBackup);
            }
        }


        public void LogAppEvent(string EventData)
        {
            if (ProgramSetting.Debug || CommandLine.Silent || CommandLine.Log)
            {
                string UniqueID = Guid.NewGuid().ToString();
                try
                {
                    using (StreamWriter AppLog = new StreamWriter(AppLogFile, true))
                    {
                        AppLog.WriteLine(String.Format("[{0}][{1}] {2}", UniqueID, DateTime.Now.ToString(), EventData.Replace(Environment.NewLine, " // ")));

                    };

                }
                catch(IOException ex)
                {
                }
            }

        }

        public  void RegisterBoot()
        {
            if(GetProgramSetting(ProgramSetting.AutoStartupRegistration, "True")== "True")
            {
                if(Microsoft.Win32.Registry.GetValue(ProgramSetting.RegistryRootedBootKey, ProgramSetting.RegistryBootVal, null)==null)
                {
                    LogAppEvent("Registering program in startup list");
                    Microsoft.Win32.Registry.SetValue(ProgramSetting.RegistryRootedBootKey, ProgramSetting.RegistryBootVal, String.Format(" {0} /scheduler", Application.ExecutablePath));
                }
            }
        }

        public void IncrementSyncsCount()
        {
            try
            {
                int Count;
                if(File.Exists(StatsFile) && int.TryParse(File.ReadAllText(StatsFile),out Count))
                {
                    File.WriteAllText(StatsFile, (Count + 1).ToString());
                }
            }
            catch(Exception ex){ }
        }

    }

    public struct CommandLine
    {
        public enum RunMode
        {
            Normal,
            Scheduler,
            Queue,
            Scanner,
        }


        public static bool Help;  // False
        public static bool Quiet;//'= False
        public static string TasksToRun;//=""
        public static bool RunAll; //'= False
        public static bool ShowPreview; //'= False
        public static RunMode RunAs; //'= RunMode.Normal
        public static bool Silent; //'= False
        public static bool Log; //'= False
        public static bool NoUpdates;// '= False
        public static bool NoStop;// '= False

        public static string ScanPath;// = ""


        public static void ReadArgs(List<string> ArgsList)
        {
            ConfigHandler ProgramConfig = new ConfigHandler();
            ProgramConfig.LogDebugEvent("Parsing command line settings");

            foreach (string Param in ArgsList)
            {
                ProgramConfig.LogDebugEvent("  Got: " + Param);
            }
            ProgramConfig.LogDebugEvent("Done.");
            if (ArgsList.Count > 1)
            {
                CommandLine.Help = ArgsList.Contains("/help");
                CommandLine.Quiet = ArgsList.Contains("/quiet");
                CommandLine.ShowPreview = ArgsList.Contains("/preview");
                CommandLine.Silent = ArgsList.Contains("/silent");
                CommandLine.Log = ArgsList.Contains("/log");
                CommandLine.NoUpdates = ArgsList.Contains("/noupdates");
                CommandLine.NoStop = ArgsList.Contains("/nostop");
                CommandLine.RunAll = ArgsList.Contains("/all");
                int RunArgIndex = ArgsList.IndexOf("/run");
                if(!CommandLine.RunAll && RunArgIndex != -1 && RunArgIndex +1 < ArgsList.Count)
                {                  
                    CommandLine.TasksToRun = ArgsList[RunArgIndex + 1];
                }
            }
        }

    }
}
