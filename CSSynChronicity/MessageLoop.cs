using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Drawing;
using CSSynChronicity.Interface;

namespace CSSynChronicity
{
    class MessageLoop : ApplicationContext
    {
        public bool ExitNeeded = false;
        public static LanguageHandler Translation;
        public static ConfigHandler ProgramConfig;
        public static Dictionary<string, ProfileHandler> Profiles;
        bool ReloadNeeded;
        Interface.MainForm MainFormInstance;

        Font SmallFont;
        Font LargeFont;



        private Mutex Blocker;// '= Nothing
        public List<SchedulerEntry> ScheduledProfiles = new List<SchedulerEntry>();// '= Nothing
        public MessageLoop()
        {
            //' Initialize ProgramConfig, Translation 
            InitializeSharedObjects();
           // ' Start logging
            ProgramConfig.LogAppEvent(new String('=', 20));
            ProgramConfig.LogAppEvent("Program started: " + Application.StartupPath);
            ProgramConfig.LogAppEvent(String.Format("Profiles folder: {0}.", ProgramConfig.ConfigRootDir));
            //Interaction.ShowDebug(Translation.Translate("\\DEBUG_WARNING"), Translation.Translate("\\DEBUG_MODE"));

            CommandLine.ReadArgs(new List<string>(Environment.GetCommandLineArgs()));

            //  ' Check if multiple instances are allowed.
            if(CommandLine.RunAs == CommandLine.RunMode.Scheduler && SchedulerAlreadyRunning())
            {
                ProgramConfig.LogAppEvent("Scheduler already running; exiting.");
                ExitNeeded = true;
            }
            else
            {
                this.ThreadExit += MessageLoop_ThreadExit;
            }



            //        ' Setup settings
            ReloadProfiles();
            ProgramConfig.LoadProgramSettings();
            if(!ProgramConfig.ProgramSettingsSet(ProgramSetting.AutoUpdates) || ! ProgramConfig.ProgramSettingsSet(ProgramSetting.Language) )
            {
                ProgramConfig.LogDebugEvent("Auto updates or language not set; launching first run dialog.");
                HandleFirstRun();
            }

            //Initialize Main, Updates
            InitializeForms();

            //        ' Look for updates
            //        If(Not CommandLine.NoUpdates) And ProgramConfig.GetProgramSetting(Of Boolean)(ProgramSetting.AutoUpdates, False) Then
            //           Dim UpdateThread As New Threading.Thread(AddressOf Updates.CheckForUpdates)
            //            UpdateThread.Start()
            //        End If
            if(CommandLine.Help)
            {
                //            Interaction.ShowMsg(String.Format("Create Synchronicity, version {1}.{0}{0}Profiles folder: ""{2}"".{0}{0}Available commands: see manual.{0}{0}License information: See ""Release notes.txt"".{0}{0}Full manual: See {3}.{0}{0}You can support this software! See {4}.{0}{0}Happy syncing!", Environment.NewLine, Application.ProductVersion, ProgramConfig.ConfigRootDir, Branding.Help, Branding.Contribute), "Help!")
                //#If DEBUG Then
                //            Dim FreeSpace As New Text.StringBuilder()
                //            For Each Drive As IO.DriveInfo In IO.DriveInfo.GetDrives()
                //                If Drive.IsReady Then
                //                    FreeSpace.AppendLine(String.Format("{0} -> {1:0,0} B free/{2:0,0} B", Drive.Name, Drive.TotalFreeSpace, Drive.TotalSize))
                //                End If
                //            Next
                //            Interaction.ShowMsg(FreeSpace.ToString)
            }
            else
            {
                ProgramConfig.LogDebugEvent(String.Format("Initialization complete. Running as '{0}'.", CommandLine.RunAs.ToString()));
                if(CommandLine.RunAs == CommandLine.RunMode.Queue || CommandLine.RunAs == CommandLine.RunMode.Scheduler)
                {
                    Interaction.ToggleStatusIcon(true);
                    if(CommandLine.RunAs == CommandLine.RunMode.Queue)
                    {
                        MainFormInstance.ApplicationTimer.Interval = 1000;
                        MainFormInstance.ApplicationTimer.Tick += StartQueue;
                    }
                    else if (CommandLine.RunAs == CommandLine.RunMode.Scheduler)
                    {
                        MainFormInstance.ApplicationTimer.Interval = 15000;
                        MainFormInstance.ApplicationTimer.Tick += Scheduling_Tick;
                    }

                }
                else
                {
                    MainFormInstance.FormClosed += ReloadMainForm;
                    MainFormInstance.Show();
                }
            }
        }

        private void ReloadMainForm(object sender, FormClosedEventArgs e)
        {
            if( ReloadNeeded )
            {
                MainFormInstance = new MainForm();
                MainFormInstance.FormClosed += this.ReloadMainForm;
                MainFormInstance.Show();
            }
            else
            {
                Application.Exit();
            }
        }

        private void ScheduledProfileCompleted(string ProfileName, bool Completed)
        {
            if (Completed)
            {
                ProgramConfig.LogAppEvent("Scheduler: " + ProfileName + " completed successfully.");
                if (Profiles.ContainsKey(ProfileName))
                    ScheduledProfiles.Add(new SchedulerEntry(ProfileName, Profiles[ProfileName].Scheduler.NextRun(), false, false));
            }
            else
            {
                ProgramConfig.LogAppEvent("Scheduler: " + ProfileName + " reported an error, and will run again in 4 hours."); // If ProfileName has been removed, ReloadScheduledProfiles will unschedule it.
                ScheduledProfiles.Add(new SchedulerEntry(ProfileName, DateTime.Now.AddHours(4), true, true));
            }
        }

        private void Scheduling_Tick(object sender, EventArgs e)
        {
            if (ProgramConfig.CanGoOn == false) { return; }// 'Don't start next sync yet.

            ReloadScheduledProfiles();
            if( ScheduledProfiles.Count == 0 )
            {
                ProgramConfig.LogAppEvent("Scheduler: No profiles left to run, exiting.");
                Application.Exit();
                return;
            }

            else
            {
                SchedulerEntry NextInQueue = ScheduledProfiles[0];
                string Status = Translation.TranslateFormat("\\SCH_WAITING", NextInQueue.Name, (NextInQueue.NextRun == ScheduleInfo.DATE_CATCHUP? "...": NextInQueue.NextRun.ToString()));
                Interaction.StatusIcon.Text = Status.Length >= 64 ? Status.Substring(0, 63) : Status;

                if (DateTime.Compare(NextInQueue.NextRun, DateTime.Now) <= 0)
                {
                    ProgramConfig.LogAppEvent("Scheduler: Launching " + NextInQueue.Name);
                    SynchronizeForm SyncForm = new SynchronizeForm(NextInQueue.Name, false, NextInQueue.CatchUp);
                    SyncForm.SyncFinished += ScheduledProfileCompleted;
                    ScheduledProfiles.RemoveAt(0);
                    SyncForm.StartSynchronization(false);
                }
            }

        }

        private string Needle;
        private bool EqualityPredicate(SchedulerEntry Item)
        {
            return Item.Name == Needle;
        }

        private void ReloadScheduledProfiles()
        {
            ReloadProfiles();// 'Needed! This allows to detect config changes.
            foreach(KeyValuePair<string , ProfileHandler> Profile in Profiles)
            {
                string Name = Profile.Key;
                ProfileHandler Handler = Profile.Value;
                TimeSpan OneDay = new TimeSpan(1, 0, 0, 0);

                if(Handler.Scheduler.Frequency != ScheduleInfo.Freq.Never)
                {
                    SchedulerEntry NewEntry = new SchedulerEntry(Name, Handler.Scheduler.NextRun(), false, false);
                    DateTime LastRun = Handler.GetLastRun();
                    if(Handler.GetSetting(ProfileSetting.CatchUpSync, false) && LastRun!= ScheduleInfo.DATE_NEVER && (NewEntry.NextRun -LastRun) > (Handler.Scheduler.GetInterval() + OneDay))
                    {
                        ProgramConfig.LogAppEvent("Scheduler: Profile " + Name + " was last executed on " + LastRun.ToString() + ", marked for catching up.");
                        NewEntry.NextRun = ScheduleInfo.DATE_CATCHUP;
                        NewEntry.CatchUp = true;
                    }
                    Needle = Name;
                    int ProfileIndex = ScheduledProfiles.FindIndex(new Predicate<SchedulerEntry>( EqualityPredicate));
                    if(ProfileIndex!=-1)
                    {
                        SchedulerEntry CurEntry = ScheduledProfiles[ProfileIndex];
                        if(NewEntry.NextRun != CurEntry.NextRun && CurEntry.NextRun >= DateTime.Now)
                        {
                            NewEntry.HasFailed = CurEntry.HasFailed;
                            if (CurEntry.HasFailed) NewEntry.NextRun = CurEntry.NextRun;
                            ScheduledProfiles.RemoveAt(ProfileIndex);
                            ScheduledProfiles.Add(NewEntry);
                            ProgramConfig.LogAppEvent("Scheduler: Re-registered profile for delayed run on " + NewEntry.NextRun.ToString() + ": " + Name);
                        }
                    }
                    else
                    {
                        ScheduledProfiles.Add(NewEntry);
                        ProgramConfig.LogAppEvent("Scheduler: Registered profile for delayed run on " +NewEntry.NextRun.ToString() + ": "+ Name);
                    }

                }





            }
            //'Remove deleted or disabled profiles
            for (int ProfileIndex = ScheduledProfiles.Count - 1; ProfileIndex>0; ProfileIndex--)
            {
                if(Profiles.ContainsKey(ScheduledProfiles[ProfileIndex].Name) || Profiles[ScheduledProfiles[ProfileIndex].Name].Scheduler.Frequency == ScheduleInfo.Freq.Never)
                {
                    ScheduledProfiles.RemoveAt(ProfileIndex);
                 }
            }


            ScheduledProfiles.Sort((SchedulerEntry First , SchedulerEntry Second ) => {return First.NextRun.CompareTo(Second.NextRun); });
        }

        private void StartQueue(object sender, EventArgs e)
        {
            MainFormInstance.ApplicationTimer.Interval = ProgramConfig.GetProgramSetting(ProgramSetting.Pause, 5000);// 'Wait 5s between profiles k and k+1, k > 0
            MainFormInstance.ApplicationTimer.Stop();
            ProcessProfilesQueue();
        }

        public static void RedoSchedulerRegistration()
        {
            bool NeedToRunAtBootTime = false;
            foreach (ProfileHandler Profile in Profiles.Values)
            {
                NeedToRunAtBootTime = NeedToRunAtBootTime | (Profile.Scheduler.Frequency != ScheduleInfo.Freq.Never);
                if (Profile.Scheduler.Frequency != ScheduleInfo.Freq.Never)
                    ProgramConfig.LogAppEvent(string.Format("Profile {0} requires the scheduler to run.", Profile.ProfileName));
            }

            try
            {
                if (NeedToRunAtBootTime && ProgramConfig.GetProgramSetting<bool>(ProgramSetting.AutoStartupRegistration, true))
                {
                    ProgramConfig.RegisterBoot();
                    if (CommandLine.RunAs == CommandLine.RunMode.Normal)
                    {
                        ProgramConfig.LogAppEvent("Starting scheduler");
                        System.Diagnostics.Process.Start(Application.ExecutablePath, "/scheduler /noupdates" + (CommandLine.Log ? " /log" : ""));
                    }
                }
                else if (Microsoft.Win32.Registry.GetValue(ProgramSetting.RegistryRootedBootKey, ProgramSetting.RegistryBootVal, null/* TODO Change to default(_) if this is not a reference type */) != null)
                {
                    ProgramConfig.LogAppEvent("Unregistering program from startup list");
                    Microsoft.Win32.Registry.CurrentUser.OpenSubKey(ProgramSetting.RegistryBootKey, true).DeleteValue(ProgramSetting.RegistryBootVal);
                }
            }
            catch (Exception Ex)
            {
                Interaction.ShowMsg(Translation.Translate(@"\UNREG_ERROR"), Translation.Translate(@"\ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ProcessProfilesQueue()
        {
             Queue<string> ProfilesQueue = null;
            if(ProfilesQueue is null)
            {
                ProfilesQueue = new Queue<string>();
                ProgramConfig.LogAppEvent("Profiles queue: Queue created.");
                List<string> RequestedProfiles = new List<string>();
                if(CommandLine.RunAll)
                {
                    RequestedProfiles.AddRange(Profiles.Keys);
                }
                else
                {
                    List<string> RequestedGroups = new List<string>();
                    foreach ( string Entry in CommandLine.TasksToRun.Split(ProgramSetting.EnqueuingSeparator))
                    {
                        if(Entry.StartsWith(ProgramSetting.GroupPrefix.ToString()))
                        {
                            RequestedGroups.Add(Entry.Substring(1));
                        }
                        else
                        {
                            RequestedProfiles.Add(Entry);
                        }
                    }
                    foreach(ProfileHandler Profile in Profiles.Values)
                    {
                        if(RequestedGroups.Contains(Profile.GetSetting(ProfileSetting.Group, "")))
                        {
                            RequestedProfiles.Add(Profile.ProfileName);
                        }
                    }
                }

                foreach (string Profile in RequestedProfiles)
                {
                    if (Profiles.ContainsKey(Profile))
                    {
                        if( Profiles[Profile].ValidateConfigFile())
                        {
                            ProgramConfig.LogAppEvent("Profiles queue: Registered profile " + Profile);
                            ProfilesQueue.Enqueue(Profile);
                        }
                    }
                }
            }

        }

        private void InitializeForms()
        {
            MainFormInstance = new MainForm();
            // 'Load status icon
            Interaction.LoadStatusIcon();
            MainFormInstance.ToolStripHeader.Image = Interaction.StatusIcon.Icon.ToBitmap();
            Interaction.StatusIcon.ContextMenuStrip = MainFormInstance.StatusIconMenu;
        }

        private void HandleFirstRun()
        {
            if(!ProgramConfig.ProgramSettingsSet(ProgramSetting.Language) )
            {
                LanguageForm Lng = new LanguageForm();
                Lng.ShowDialog();
                Translation = LanguageHandler.GetSingleton(true);
            }
            if(!ProgramConfig.ProgramSettingsSet(ProgramSetting.AutoUpdates))
            { 
                //Dim AutoUpdates As Boolean = If(Interaction.ShowMsg(Translation.Translate("\WELCOME_MSG"), Translation.Translate("\FIRST_RUN"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes, True, False)
                //此处简化
                bool AutoUpdates = false;
                ProgramConfig.SetProgramSetting(ProgramSetting.AutoUpdates, AutoUpdates);
            }

            ProgramConfig.SaveProgramSettings();
        }

        public static void ReloadProfiles()
        {
            Profiles.Clear();// 'Initialized in InitializeSharedObjects
            foreach(string ConfigFile in Directory.GetFiles(ProgramConfig.ConfigRootDir, "*.sync"))
            {
                string Name = Path.GetFileNameWithoutExtension(ConfigFile);
                Profiles.Add(Name, new ProfileHandler(Name));
            }
         }

        private void MessageLoop_ThreadExit(object sender, EventArgs e)
        {
            ExitNeeded = true;
            Interaction.ToggleStatusIcon(false);

            //' Save last window information. Don't overwrite config file if running in scheduler mode.
            if (!(CommandLine.RunAs == CommandLine.RunMode.Scheduler))
            {
                ProgramConfig.SaveProgramSettings();
            }

            //'Calling ReleaseMutex would be the same, since Blocker necessary holds the mutex at this point (otherwise the app would have closed already).
            if (CommandLine.RunAs == CommandLine.RunMode.Scheduler) { Blocker.Close(); }
            ProgramConfig.LogAppEvent("Program exited");
        }

        private bool SchedulerAlreadyRunning()
        {

            string MutexName = "[[Create Synchronicity scheduler]] " + Application.ExecutablePath.Replace(ProgramSetting.DirSep, '!').ToLower(Interaction.InvariantCulture);
            if (MutexName.Length > 260) { MutexName = MutexName.Substring(0, 260); }

            ProgramConfig.LogDebugEvent(String.Format("Registering mutex: {0}", MutexName));

            try
            {
                Blocker = new Mutex(false, MutexName);
            }
            catch(AbandonedMutexException ex)
            {
                ProgramConfig.LogDebugEvent("Abandoned mutex detected");
                return false;
            }
            catch(UnauthorizedAccessException ex)
            {
                ProgramConfig.LogDebugEvent("Acess to the Mutex forbidden");
                return true;
            }


            return (!Blocker.WaitOne(0, false));
        }

        private void InitializeSharedObjects()
        {
            //' Load program configuration
            ProgramConfig = ConfigHandler.GetSingleton();
            Translation = LanguageHandler.GetSingleton();
            Profiles = new Dictionary<string, ProfileHandler>();

            try
            {
                SmallFont = new Font("Verdana", 7.0f);
                LargeFont = new Font("Verdana", 8.25f);
            }
           catch(ArgumentException ex)
            {
                SmallFont = new Font(SystemFonts.MessageBoxFont.FontFamily.Name, 7.0f);
                LargeFont = SystemFonts.MessageBoxFont;
            }


            // ' Create required folders
            Directory.CreateDirectory(ProgramConfig.LogRootDir);
            Directory.CreateDirectory(ProgramConfig.ConfigRootDir);
            Directory.CreateDirectory(ProgramConfig.LanguageRootDir);
        }
    }
}
