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
    public class ProfileSetting
    {
        public const string Source = "Source Directory";
        public const string Destination = "Destination Directory";
        public const string IncludedTypes = "Included Filetypes";
        public const string ExcludedTypes = "Excluded FileTypes";
        public const string ReplicateEmptyDirectories = "Replicate Empty Directories";
        public const string Method = "Synchronization Method";
        public const string Restrictions = "Files restrictions";
        public const string LeftSubFolders = "Source folders to be synchronized";
        public const string RightSubFolders = "Destination folders to be synchronized";
        public const string MayCreateDestination = "Create destination folder";
        public const string StrictDateComparison = "Strict date comparison";
        public const string PropagateUpdates = "Propagate Updates";
        public const string StrictMirror = "Strict mirror";
        public const string TimeOffset = "Time Offset";
        public const string LastRun = "Last run";
        public const string CatchUpSync = "Catch up if missed";
        public const string CompressionExt = "Compress";
        public const string Group = "Group";
        public const string CheckFileSize = "Check file size";
        public const string FuzzyDstCompensation = "Fuzzy DST compensation";
        public const string Checksum = "Checksum";

        //Next settings are hidden, not automatically appended to config files.
        public const string ExcludedFolders = "Excluded folder patterns";
        public const string WakeupAction = "Wakeup action";
        public const string PostSyncAction = "Post-sync action";
        public const string ExcludeHidden = "Exclude hidden entries";
        public const string DiscardAfter = "Discard after";
        public const string PreviewOnly = "Preview only";
        public const string SyncFolderAttributes = "Sync folder attributes";
        public const string ErrorsLog = "Track errors separately";
        public const string AutoIncludeNewFolders = "Auto-include new folders";//'TODO: Not ready for mass use yet.
        public const string LastModified = "Last modified";
        public const string Decompress = "Decompress";
        //'</>

        //'Disabled: would require keeping a list of modified files to work, since once a source file is deleted in the source, there's no way to tell when it had been last modified, and hence no way to calculate the appropriate deletion date.
        //'public const string Delay  = "Delay deletions"

        public const string Scheduling = "Scheduling";
        public const int SchedulingSettingsCount = 5; //Frequency;WeekDay;MonthDay;Hour;Minute

        public enum SyncMethod
        {
            LRMirror = 0,
            LRIncremental = 1,
            BiIncremental = 2,
        }

        public const int DefaultMethod = (int)SyncMethod.LRIncremental;
    }

    public class ProfileHandler
    {
        public string ProfileName;
        public bool IsNewProfile;
        public ScheduleInfo Scheduler = new ScheduleInfo();

        public string ConfigPath;
        public string LogPath;
        public string ErrorsLogPath;
        public ConfigHandler ProgramConfig = ConfigHandler.GetSingleton();
        public LanguageHandler Translation = LanguageHandler.GetSingleton();

        public Dictionary<String, String> Configuration = new Dictionary<string, string>();
        public Dictionary<String, bool> LeftCheckedNodes = new Dictionary<string, bool>();
        public Dictionary<String, bool> RightCheckedNodes = new Dictionary<string, bool>();

        //ProfileSettings ProfileSetting;
        //    'NOTE: Only vital settings should be checked for correctness, since the config will be rejected if a mismatch occurs.
        private readonly string[] RequiredSettings = { ProfileSetting.Source, ProfileSetting.Destination, ProfileSetting.ExcludedTypes, ProfileSetting.IncludedTypes, ProfileSetting.LeftSubFolders, ProfileSetting.RightSubFolders, ProfileSetting.Method, ProfileSetting.Restrictions, ProfileSetting.ReplicateEmptyDirectories };

        public ProfileHandler(string Name)
        {
            ProfileName = Name;

            ConfigPath = ProgramConfig.GetConfigPath(Name);
            LogPath = ProgramConfig.GetLogPath(Name);
            ErrorsLogPath = ProgramConfig.GetErrorsLogPath(Name);

            IsNewProfile = !LoadConfigFile();

            // 'Never use GetSetting(Of SyncMethod). It searches the config file for a string containing an int (eg "0"),
            //but when failing it calls SetSettings which saves a string containing an enum label (eg. "LRIncremental")
            if(GetSetting(ProfileSetting.Method, ProfileSetting.DefaultMethod) != (int)ProfileSetting.SyncMethod.LRMirror)
            {
                SetSetting(ProfileSetting.StrictMirror, false);
                SetSetting(ProfileSetting.DiscardAfter, 0);
            }
            if(GetSetting(ProfileSetting.PostSyncAction,"") != "")
            {
                SetSetting(ProfileSetting.ErrorsLog, true);
            }
            if(GetSetting(ProfileSetting.MayCreateDestination, false) && GetSetting(ProfileSetting.RightSubFolders, "") == "")
            {
                SetSetting(ProfileSetting.RightSubFolders, "*");
            }

        }

        private bool LoadConfigFile()
        {
            if (!File.Exists(ConfigPath)) return false;
            Configuration.Clear();
            using (StreamReader FileReader = new StreamReader(ConfigPath))
            {
                while (!FileReader.EndOfStream)
                {
                    string ConfigLine = "";
                    ConfigLine = FileReader.ReadLine();
                    string[] Param = ConfigLine.Split(":".ToCharArray(), 2);
                    if (Param.Length < 2)
                    {
                        //Interaction.ShowMsg(Translation.TranslateFormat("\INVALID_SETTING", ConfigLine))
                        ProgramConfig.LogAppEvent("Invalid setting for profile '" + ProfileName + "': " + ConfigLine);
                    }
                    else if (!Configuration.ContainsKey(Param[0]))
                    {
                        Configuration.Add(Param[0], Param[1]);
                    }
                }
            }

            LoadScheduler();
            LoadSubFoldersList(ProfileSetting.LeftSubFolders, LeftCheckedNodes);
            LoadSubFoldersList(ProfileSetting.RightSubFolders, RightCheckedNodes);
            return true;
        }

        private void LoadSubFoldersList(string ConfigLine, Dictionary<string, bool> Subfolders)
        {
            Subfolders.Clear();
            List<string> ConfigCheckedFoldersList = new List<string>(GetSetting(ConfigLine, "").Split(';'));
            ConfigCheckedFoldersList.RemoveAt(ConfigCheckedFoldersList.Count - 1);
            foreach(string Dir in ConfigCheckedFoldersList)
            {
                bool Recursive = false;
                string dir = Dir;
                if (Dir.EndsWith("*"))
                {
                    Recursive = true;
                    dir = Dir.Substring(0, Dir.Length - 1);
                }
                if (!Subfolders.ContainsKey(Dir)) Subfolders.Add(dir, Recursive);
            }

        }

        private void LoadScheduler()
        {
            string[] Opts = GetSetting(ProfileSetting.Scheduling, "").Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            if(Opts.GetLength(0) == ProfileSetting.SchedulingSettingsCount )
            {
                Scheduler = new ScheduleInfo(Opts[0], Opts[1], Opts[2], Opts[3], Opts[4]);
            }
            else
            {
                Scheduler = new ScheduleInfo() { Frequency = ScheduleInfo.Freq.Never };
            }
        }

        public bool SaveConfigFile()
        {
            try
            {
                using (StreamWriter FileWriter = new StreamWriter(ConfigPath))
                {
                    foreach (KeyValuePair<string, string> Setting in Configuration)
                    {
                        FileWriter.WriteLine(Setting.Key + ":" + Setting.Value);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                ProgramConfig.LogAppEvent("Unable to save config file for " + ProfileName + Environment.NewLine + ex.ToString());
                return false;
            }
        }


        //    ' `ReturnString` is used to pass locally generated error messages to caller.
        public bool ValidateConfigFile(bool WarnUnrootedPaths  = false, bool TryCreateDest = false, string FailureMsg ="")
        {
            bool IsValid = true;
            List<string> InvalidListing = new List<string>();
            string Dest = TranslatePath(GetSetting<string>(ProfileSetting.Destination));
            bool NeedsWakeup = true;// 'Static, but not shared.

            string Action = this.GetSetting(ProfileSetting.WakeupAction,"");
            if(NeedsWakeup && ProgramConfig.GetProgramSetting(ProgramSetting.ExpertMode, false) && Action !=  "")
            {
                try
                {
                    //'Call Wake-up script in a blocking way
                    System.Diagnostics.Process.Start(Action, Dest).WaitForExit();
                    NeedsWakeup = false;
                }
                catch(Exception ex)
                {
                    Interaction.ShowMsg(Translation.Translate("\\WAKEUP_FAILED"));
                    ProgramConfig.LogAppEvent(ex.ToString());
                    IsValid = false;
                }
            }

            if (!Directory.Exists(TranslatePath(GetSetting<string>(ProfileSetting.Source))))
            {
                InvalidListing.Add(Translation.Translate("\\INVALID_SOURCE"));
                IsValid = false;
            }
            //        'TryCreateDest <=> When this function returns, the folder should exist.
            //        'MayCreateDest <=> Creating the destination folder is allowed for this folder.
            bool MayCreateDest = GetSetting(ProfileSetting.MayCreateDestination, false);
            if(MayCreateDest && TryCreateDest)
            {
                try { Directory.CreateDirectory(Dest); }
                catch(Exception ex)
                {
                    InvalidListing.Add(Translation.TranslateFormat("\\FOLDER_FAILED", Dest, ex.Message));
                }
            }

            if(!Directory.Exists(Dest) && (TryCreateDest || (! MayCreateDest)))
            {
                InvalidListing.Add(Translation.Translate("\\INVALID_DEST"));
                IsValid = false;
            }

            foreach(string Key in RequiredSettings)
            {
                if(!Configuration.ContainsKey(Key))
                {
                    IsValid = false;
                    InvalidListing.Add(Translation.TranslateFormat("\\SETTING_UNSET", Key));
                }
            }
            //        If GetSetting(Of String)(ProfileSetting.CompressionExt, "") <> "" Then
            //            If Array.IndexOf({".gz", ".bz2"}, Configuration(ProfileSetting.CompressionExt)) < 0 Then
            //              IsValid = False
            //                InvalidListing.Add("Unknown compression extension, or missing ""."":" & Configuration(ProfileSetting.CompressionExt))
            //            End If

            //            If Not IO.File.Exists(ProgramConfig.CompressionDll) Then
            //                IsValid = False
            //                InvalidListing.Add(String.Format("{0} not found!", ProgramConfig.CompressionDll))
            //            End If
            //        End If
            if(!IsValid)
            {
                string ErrorsList = string.Join(Environment.NewLine, InvalidListing.ToArray());
                string ErrMsg = String.Format("{0} - {1}{2}{3}", ProfileName, Translation.Translate("\\INVALID_CONFIG"), Environment.NewLine, ErrorsList);
                //string ErrMsg = String.Format("{0} - {1}{2}{3}", ProfileName, "INVALID_CONFIG", Environment.NewLine, ErrorsList);
                if (! (FailureMsg == null)) { FailureMsg = ErrMsg; }            //    '(FailureMsg is passed ByRef)
                if (!CommandLine.Quiet) { Interaction.ShowMsg(ErrMsg, "INVALID_CONFIG", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
                return false;
            }
            else
            {
                if(WarnUnrootedPaths)
                {
                    if(!Path.IsPathRooted(TranslatePath(GetSetting<string>(ProfileSetting.Source))))
                    {
                        if (Interaction.ShowMsg(Translation.TranslateFormat("\\LEFT_UNROOTED", Path.GetFullPath(GetSetting<string>(ProfileSetting.Source))), null, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            return false;
                        }
                    }
                    if (!Path.IsPathRooted(TranslatePath(GetSetting<string>(ProfileSetting.Destination))))
                    {
                        if (Interaction.ShowMsg(Translation.TranslateFormat("\\RIGHT_UNROOTED", Path.GetFullPath(GetSetting<string>(ProfileSetting.Source))), null, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }

        }

        public bool Rename(string  NewName)
        {
            if((! string.Equals(ProfileName, NewName, StringComparison.OrdinalIgnoreCase)) && (File.Exists(ProgramConfig.GetLogPath(NewName)) || File.Exists(ProgramConfig.GetErrorsLogPath(NewName)) || File.Exists(ProgramConfig.GetConfigPath(NewName))))
            {
                return false;
            }
            try
            {
                if (File.Exists(ErrorsLogPath)) { File.Move(ErrorsLogPath, ProgramConfig.GetErrorsLogPath(NewName)); }
                if (File.Exists(LogPath)) { File.Move(ErrorsLogPath, ProgramConfig.GetLogPath(NewName)); }
                File.Move(ConfigPath, ProgramConfig.GetConfigPath(NewName));
                ProfileName = NewName;
            }
            catch(Exception ex)
            {
                return false;
            }

            return true;

        }
        public void DeleteConfigFile()
        {
            File.Delete(ConfigPath);
            DeleteLogFiles();
        }
        public void DeleteLogFiles()
        {
            File.Delete(LogPath);
            File.Delete(ErrorsLogPath);
        }


        public void SetSetting<T>(string SettingName, T Value)
        {
            Configuration[SettingName] = Value.ToString();
        }

        public void CopySetting<T>(string Key, T Value,bool Load)
        {
            if(Load)
            {
                Value = GetSetting(Key, Value);
            }
            else
            {
                Configuration[Key] = Value!= null? Value.ToString(): null;
            }
        }


        public T GetSetting<T>(string Key, T DefaultVal = default(T))
        {
            string Val = "";
            if(Configuration.TryGetValue(Key,out Val) && !string.IsNullOrEmpty(Val))
            {
                try
                {
                    return (T)(object)Val;
                }
                catch
                {
                    SetSetting(Key, DefaultVal);
                }
            }
            return DefaultVal;

        }
        public void SaveScheduler()
        {
            SetSetting<string>(ProfileSetting.Scheduling, string.Join(";", new string[] { Scheduler.Frequency.ToString(), Scheduler.WeekDay.ToString(), Scheduler.MonthDay.ToString(), Scheduler.Hour.ToString(), Scheduler.Minute.ToString() }));
        }

        public void LoadSubFoldersList(string ConfigLine, ref Dictionary<string, bool> Subfolders)
        {
            Subfolders.Clear();
            List<string> ConfigCheckedFoldersList = new List<string>(GetSetting<string>(ConfigLine, "").Split(';'));
            ConfigCheckedFoldersList.RemoveAt(ConfigCheckedFoldersList.Count - 1); // Removes the last, empty element
                                                                                   // Warning: The trailing comma can't be removed when generating the configuration string.
                                                                                   // Using StringSplitOptions.RemoveEmptyEntries would make no difference between ';' (root folder selected, no subfolders) and '' (nothing selected at all)

            foreach (string Dir in ConfigCheckedFoldersList)
            {
                bool Recursive = false;
                string tempDir = Dir;

                if (tempDir.EndsWith("*"))
                {
                    
                    Recursive = true;
                    tempDir = tempDir.Substring(0, tempDir.Length - 1);
                }

                if (!Subfolders.ContainsKey(tempDir))
                    Subfolders.Add(tempDir, Recursive);
            }
        }

        public static string TranslatePath(string  Path )
        {
            if (Path == "" || Path == null) return "";
            return TranslatePath_Unsafe(Path).TrimEnd(ProgramSetting.DirSep);// 'Careful with Linux root
                // 'Prevents a very annoying bug, where the presence of a slash at the end of the base directory would confuse the engine (#3052979)
        }

        public static string TranslatePath_Inverse(string Path)
        {

            if (System.Text.RegularExpressions.Regex.IsMatch(Path, @"^(?<driveletter>[A-Z]\:)(\\(?<relativepath>.*))?$"))
            {
                string Label = "";
                foreach (System.IO.DriveInfo Drive in System.IO.DriveInfo.GetDrives())
                {
                    if (Drive.Name[0] == Path[0])
                        Label = Drive.VolumeLabel;
                }
                if (Label != "")
                    return string.Format(@"""{0}""\{1}", Label, Path.Substring(2).Trim(ProgramSetting.DirSep)).TrimEnd(ProgramSetting.DirSep);
            }

            return Path;
        }



        public static string TranslatePath_Unsafe(string  Path )
        {
            string Translated_Path = Path;
            string Label;
            string RelativePath;
            if(Path.StartsWith(" ") || Path.StartsWith(":"))
            {
                int ClosingPos = Path.LastIndexOfAny(":".ToCharArray());
                if (ClosingPos == 0) return "";
                Label = Path.Substring(1, ClosingPos - 1);
                RelativePath = Path.Substring(ClosingPos + 1);
                if (Path.StartsWith( " ") &&! (Label ==""))
                {
                    foreach (DriveInfo Drive in DriveInfo.GetDrives())
                    {
                        if(!(Drive.Name[0] =='A')&& Drive.IsReady && (String.Compare(Drive.VolumeLabel, Label, true) == 0) )
                        {
                            Translated_Path = (Drive.Name + RelativePath.TrimStart(ProgramSetting.DirSep)).TrimEnd(ProgramSetting.DirSep);
                            break;
                        }
                    }
                }
            }
            //' Use a path-friendly version of the DATE constant.
            System.Environment.SetEnvironmentVariable("MMMYYYY", DateTime.Today.ToString("MMMyyyy").ToLower(Interaction.InvariantCulture));
            System.Environment.SetEnvironmentVariable("DATE", DateTime.Today.ToShortDateString().Replace('/', '-'));
            System.Environment.SetEnvironmentVariable("DAY", DateTime.Today.ToString("dd"));
            System.Environment.SetEnvironmentVariable("MONTH", DateTime.Today.ToString("MM"));
            System.Environment.SetEnvironmentVariable("YEAR", DateTime.Today.ToString("yyyy"));

            return System.Environment.ExpandEnvironmentVariables(Translated_Path);


        }



        public DateTime GetLastRun()
        {
            try
            {
                return GetSetting(ProfileSetting.LastRun, ScheduleInfo.DATE_NEVER);
            }
            catch(Exception ex)
            {
                return ScheduleInfo.DATE_NEVER;
            }


        }
        public void SetLastRun()
        {
            SetSetting(ProfileSetting.LastRun, DateTime.Now);
            SaveConfigFile();
        }

        public string FormatLastRun(string Format ="")
        {
            DateTime LastRun = GetLastRun();
            return (LastRun == ScheduleInfo.DATE_NEVER? "-": Translation.TranslateFormat("\\LAST_SYNC", (DateTime.Now - LastRun).Days.ToString(Format), (DateTime.Now - LastRun).Hours.ToString(Format), LastRun.ToString()));
        }

        public string FormatMethod()
        {
            switch(GetSetting(ProfileSetting.Method, ProfileSetting.DefaultMethod))
            {
                case (int)ProfileSetting.SyncMethod.LRMirror:
                    return Translation.Translate("\\LR_MIRROR");
                case (int)ProfileSetting.SyncMethod.BiIncremental:
                    return Translation.Translate("\\TWOWAYS_INCREMENTAL");
                default:
                        return Translation.Translate("\\LR_INCREMENTAL");
            }

        }


    }
    public struct SchedulerEntry
        {
            public string Name;
        public DateTime NextRun;
        public bool CatchUp;
        public bool HasFailed;

        public SchedulerEntry(string _Name, DateTime _NextRun, bool _Catchup, bool _HasFailed )
            {
                Name = _Name;
                NextRun = _NextRun;
                CatchUp = _Catchup;
                HasFailed = _HasFailed;
            }
        }

    public struct ScheduleInfo
        {
            public enum Freq
            {
                Never,
                Daily,
                Weekly,
                Monthly
            };

            public Freq Frequency;
            public int WeekDay, MonthDay, Hour, Minute;

            public static readonly DateTime DATE_NEVER = DateTime.MaxValue;
            public static readonly DateTime DATE_CATCHUP = DateTime.MinValue;

            public ScheduleInfo(string Frq , string _WeekDay, string _MonthDay, string _Hour , string _Minute)
            {
                Frequency = Freq.Never;
                Hour = 0;
                Minute = 0;
                WeekDay = 0;
                MonthDay = 0;
                try
                {
                    Hour = int.Parse(_Hour);
                    Minute = int.Parse(_Minute);
                    WeekDay = int.Parse(_WeekDay);
                    MonthDay = int.Parse(_MonthDay);
                    Frequency = Str2Freq(Frq);
                }
                catch (FormatException Ex)
                { }
                catch (OverflowException overEx)
                { }
            }


            private static Freq Str2Freq(string Str)
            {
                try
                {
                    return (Freq)Enum.Parse(typeof(Freq), Str);
                }
                catch (ArgumentException Ex)
                {
                    return Freq.Never;
                }

            }

            public TimeSpan GetInterval()
            {
                TimeSpan Interval=new TimeSpan(0);
                switch (Frequency)
                {
                    case Freq.Daily:
                        Interval = new TimeSpan(1, 0, 0, 0);
                        break;

                    case Freq.Weekly:
                        Interval = new TimeSpan(7, 0, 0, 0);
                        break;
                    case Freq.Monthly:
                        Interval = DateTime.Today.AddMonths(1) - DateTime.Today;
                        break;
                    case Freq.Never:
                        Interval = new TimeSpan(0);
                        break;
                    default:
                        break;
                }
                return Interval;
            }

            public DateTime NextRun()
            {
                DateTime Now = DateTime.Now;
                DateTime Today = DateTime.Today;
                DateTime RunAt;
                TimeSpan Interval = GetInterval();
                switch(Frequency)
                {
                    case Freq.Daily:
                        RunAt = Today.AddHours(Hour).AddMinutes(Minute);
                        break;
                    case Freq.Weekly:
                        RunAt = Today.AddDays(WeekDay -(int) Today.DayOfWeek).AddHours(Hour).AddMinutes(Minute);
                        break;
                    case Freq.Monthly:
                        RunAt = Today.AddDays(MonthDay - Today.Day).AddHours(Hour).AddMinutes(Minute);
                        break;
                    default:
                        return DATE_NEVER;
                        break;

                }
                return RunAt;
            }

        }
   
}


