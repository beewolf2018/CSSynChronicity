using CSSynChronicity.Interface;
using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace CSSynChronicity
{
    class MessageLoop : ApplicationContext
    {
        ILog log;


        public static MessageLoop Singleton;
        public bool ExitNeeded = false;
        public static LanguageHandler Translation;
        public static ConfigHandler ProgramConfig;
        public static Dictionary<string, ProfileHandler> Profiles;
        bool ReloadNeeded = false;
        //Interface.MainForm MainFormInstance;

        public Font SmallFont;
        public Font LargeFont;



        private Mutex Blocker;// '= Nothing
        public List<SchedulerEntry> ScheduledProfiles = new List<SchedulerEntry>();
        public MessageLoop()
        {

            log4net.Config.XmlConfigurator.Configure();
            log = LogManager.GetLogger(typeof(MessageLoop));
            log.Info(new String('=', 40));
            log.Info("MessageLoop started: " + Application.StartupPath);

            ProgramConfig = ConfigHandler.GetSingleton();
            Translation = LanguageHandler.GetSingleton();
            Profiles = new Dictionary<string, ProfileHandler>();

            try
            {
                SmallFont = new Font("Verdana", 7.0f);
                LargeFont = new Font("Verdana", 8.25f);
            }
            catch (ArgumentException ex)
            {
                SmallFont = new Font(SystemFonts.MessageBoxFont.FontFamily.Name, 7.0f);
                LargeFont = SystemFonts.MessageBoxFont;
                log.Error(ex.ToString());
            }

            Directory.CreateDirectory(ProgramConfig.LogRootDir);
            Directory.CreateDirectory(ProgramConfig.ConfigRootDir);
            Directory.CreateDirectory(ProgramConfig.LanguageRootDir);
            log.Info(String.Format("Log folder: {0}.", ProgramConfig.LogRootDir));
            log.Info(String.Format("Profiles folder: {0}.", ProgramConfig.ConfigRootDir));
            log.Info(String.Format("Language folder: {0}.", ProgramConfig.LanguageRootDir));


            CommandLine.ReadArgs(new List<string>(Environment.GetCommandLineArgs()));

            //  ' Check if multiple instances are allowed.
            if (CommandLine.RunAs == CommandLine.RunMode.Scheduler && SchedulerAlreadyRunning())
            {
                log.Info("Scheduler already running; exiting.");
                ExitNeeded = true;
            }
            else
            {
                //this.ThreadExit += MessageLoop_ThreadExit;
            }

            ReloadProfiles();
            ProgramConfig.LoadProgramSettings();
            if (!ProgramConfig.ProgramSettingsSet(ProgramSetting.Language))
            {
                log.Info("Auto updates or language not set; launching first run dialog.");
                HandleFirstRun();
            }

            //Initialize Main, Updates
            //MainFormInstance = new MainForm();


            System.Text.StringBuilder FreeSpace = new System.Text.StringBuilder();
            foreach (DriveInfo Drive in DriveInfo.GetDrives())
            {
                if (Drive.IsReady)
                {
                    log.Info(string.Format("{0} -> {1:0,0} B free/{2:0,0} B", Drive.Name, Drive.TotalFreeSpace, Drive.TotalSize));
                }



            }

            log.Info(String.Format("Initialization complete. Running as '{0}'.", CommandLine.RunAs.ToString()));
            // if (CommandLine.RunAs == CommandLine.RunMode.Queue || CommandLine.RunAs == CommandLine.RunMode.Scheduler)
            
            
            //Initialize Main, Updates
            //MainFormInstance = new MainForm();
            //MainFormInstance.ApplicationTimer.Interval = 15000;
            //MainFormInstance.ApplicationTimer.Tick += Scheduling_Tick;
            //MainFormInstance.ApplicationTimer.Start();
            //MainFormInstance.ShowDialog();

        }

        public static MessageLoop GetSingleton()
        {
            if (Singleton == null) { Singleton = new MessageLoop(); }
            return Singleton;
        }



        //private void ReloadMainForm(object sender, FormClosedEventArgs e)
        //{
        //    if (ReloadNeeded)
        //    {
        //        // MainFormInstance = new MainForm();
        //        //MainFormInstance.FormClosed += this.ReloadMainForm;
        //        // MainFormInstance.Show();
        //    }
        //    else
        //    {
        //        //Application.Exit();
        //    }
        //}

        //public void ScheduledProfileCompleted(string ProfileName, bool Completed)
        //{
        //    if (Completed)
        //    {
        //        log.Info("Scheduler: " + ProfileName + " completed successfully.");
        //        if (Profiles.ContainsKey(ProfileName))
        //            ScheduledProfiles.Add(new SchedulerEntry(ProfileName, Profiles[ProfileName].Scheduler.NextRun(), false, false));
        //    }
        //    else
        //    {
        //        log.Info("Scheduler: " + ProfileName + " reported an error, and will run again in 4 hours."); // If ProfileName has been removed, ReloadScheduledProfiles will unschedule it.
        //        ScheduledProfiles.Add(new SchedulerEntry(ProfileName, DateTime.Now.AddHours(4), true, true));
        //    }
        //    MainFormInstance.ApplicationTimer.Start();
        //}

        //public void Scheduling_Tick(object sender, EventArgs e)
        //{
        //    //log.Info("Scheduler: No profiles left to run, exiting.");
        //    //MainFormInstance.csNotifyicon.ShowBalloonTip(1000, "Scheduling_Tick", "Good good good girl.", ToolTipIcon.None);
        //    //MainFormInstance.ApplicationTimer.Stop();
        //    //if (ProgramConfig.CanGoOn == false) { return; }// 'Don't start next sync yet.

        //    //ReloadScheduledProfiles();
        //    //if (ScheduledProfiles.Count == 0)
        //    //{
        //    //    log.Info("Scheduler: No profiles left to run, exiting.");
        //    //    //Application.Exit();
        //    //    return;
        //    //}

        //    //else
        //    //{
        //    //    SchedulerEntry NextInQueue = ScheduledProfiles[0];
        //    //    string Status = Translation.TranslateFormat("\\SCH_WAITING", NextInQueue.Name, (NextInQueue.NextRun == ScheduleInfo.DATE_CATCHUP ? "..." : NextInQueue.NextRun.ToString()));
        //    //    //Interaction.StatusIcon.Text = Status.Length >= 64 ? Status.Substring(0, 63) : Status;

        //    //    if (DateTime.Compare(NextInQueue.NextRun, DateTime.Now) <= 0)
        //    //    {
        //    //        log.Info("Scheduler: Launching " + NextInQueue.Name);
        //    //        SynchronizeForm SyncForm = new SynchronizeForm(NextInQueue.Name, false, NextInQueue.CatchUp);
        //    //        SyncForm.SyncFinished += ScheduledProfileCompleted;
        //    //        ScheduledProfiles.RemoveAt(0);
        //    //        SyncForm.StartSynchronization(false);
        //    //    }
        //    //}
        //    ////MainFormInstance.ApplicationTimer.Start();

        //}

        private string Needle;
        private bool EqualityPredicate(SchedulerEntry Item)
        {
            return Item.Name == Needle;
        }



        //        'Logic of this function:                    Scheduling:Daily;0;1;14;23
        //' A new entry is created. The need for catching up is calculated regardless of the current state of the list.
        //' Then, a corresponding entry (same name) is searched for. If not found, then the new entry is simply added to the list.
        //' OOH, if a corresponding entry is found, then
        //'    If it's already late, or if changes would postpone it, then nothing happens.
        //'    But if it's not late, and the change will bring the sync forward, then the new entry superseedes the previous one.
        //'       Note: In the latter case, if current entry is marked as failed, then the next run time is loaded from it
        //'             (that's to avoid infinite loops when eg.the backup medium is unplugged)
        public void ReloadScheduledProfiles()
        {
            log.Info("ReloadScheduledProfiles");
            ReloadProfiles();// 'Needed! This allows to detect config changes.
            foreach (KeyValuePair<string, ProfileHandler> Profile in Profiles)
            {
                string Name = Profile.Key;
                ProfileHandler Handler = Profile.Value;
                TimeSpan OneDay = new TimeSpan(1, 0, 0, 0);

                if (Handler.Scheduler.Frequency != ScheduleInfo.Freq.Never)
                {
                    SchedulerEntry NewEntry = new SchedulerEntry(Name, Handler.Scheduler.NextRun(), false, false);
                    DateTime LastRun = Handler.GetLastRun();
                    if (Handler.GetSetting(ProfileSetting.CatchUpSync, false) && LastRun != ScheduleInfo.DATE_NEVER && (NewEntry.NextRun - LastRun) > (Handler.Scheduler.GetInterval() + OneDay))
                    {
                        log.Info("Scheduler: Profile " + Name + " was last executed on " + LastRun.ToString() + ", marked for catching up.");
                        NewEntry.NextRun = ScheduleInfo.DATE_CATCHUP;
                        NewEntry.CatchUp = true;
                    }
                    Needle = Name;
                    int ProfileIndex = ScheduledProfiles.FindIndex(new Predicate<SchedulerEntry>(EqualityPredicate));
                    if (ProfileIndex != -1)
                    {
                        SchedulerEntry CurEntry = ScheduledProfiles[ProfileIndex];
                        if (NewEntry.NextRun != CurEntry.NextRun && CurEntry.NextRun >= DateTime.Now)
                        {
                            NewEntry.HasFailed = CurEntry.HasFailed;
                            if (CurEntry.HasFailed) NewEntry.NextRun = CurEntry.NextRun;
                            ScheduledProfiles.RemoveAt(ProfileIndex);
                            ScheduledProfiles.Add(NewEntry);
                            log.Info("Scheduler: Re-registered profile for delayed run on " + NewEntry.NextRun.ToString() + ": " + Name);
                        }
                    }
                    else
                    {
                        ScheduledProfiles.Add(NewEntry);
                        log.Info("Scheduler: Registered profile for delayed run on " + NewEntry.NextRun.ToString() + ": " + Name);
                    }

                }





            }
            //'Remove deleted or disabled profiles
            for (int ProfileIndex = ScheduledProfiles.Count - 1; ProfileIndex > 0; ProfileIndex--)
            {
                if (Profiles.ContainsKey(ScheduledProfiles[ProfileIndex].Name) || Profiles[ScheduledProfiles[ProfileIndex].Name].Scheduler.Frequency == ScheduleInfo.Freq.Never)
                {
                    ScheduledProfiles.RemoveAt(ProfileIndex);
                }
            }


            ScheduledProfiles.Sort((SchedulerEntry First, SchedulerEntry Second) => { return First.NextRun.CompareTo(Second.NextRun); });
        }

        //public void StartQueue(object sender, EventArgs e)
        //{
        //    MainFormInstance.ApplicationTimer.Interval = ProgramConfig.GetProgramSetting(ProgramSetting.Pause, 5000);// 'Wait 5s between profiles k and k+1, k > 0
        //    MainFormInstance.ApplicationTimer.Stop();
        //    ProcessProfilesQueue();
        //}

        public void RedoSchedulerRegistration()
        {
            log.Info("RedoSchedulerRegistration");
            bool NeedToRunAtBootTime = false;
            foreach (ProfileHandler Profile in Profiles.Values)
            {
                NeedToRunAtBootTime = NeedToRunAtBootTime | (Profile.Scheduler.Frequency != ScheduleInfo.Freq.Never);
                log.Info(string.Format("Profile = {0}", Profile.ProfileName));
                if (Profile.Scheduler.Frequency != ScheduleInfo.Freq.Never)
                    log.Info(string.Format("Profile {0} requires the scheduler to run.", Profile.ProfileName));
            }

            try
            {
                if (NeedToRunAtBootTime && ProgramConfig.GetProgramSetting<bool>(ProgramSetting.AutoStartupRegistration, true))
                {
                    ProgramConfig.RegisterBoot();
                    if (CommandLine.RunAs == CommandLine.RunMode.Normal)
                    {
                        log.Info("Starting scheduler");
                        //System.Diagnostics.Process.Start(Application.ExecutablePath, "/scheduler /noupdates" + (CommandLine.Log ? " /log" : ""));
                    }
                }
                else if (Microsoft.Win32.Registry.GetValue(ProgramSetting.RegistryRootedBootKey, ProgramSetting.RegistryBootVal, null) != null)
                {
                    log.Info("Unregistering program from startup list");
                    Microsoft.Win32.Registry.CurrentUser.OpenSubKey(ProgramSetting.RegistryBootKey, true).DeleteValue(ProgramSetting.RegistryBootVal);
                }
            }
            catch (Exception Ex)
            {
                // Interaction.ShowMsg(Translation.Translate(@"\UNREG_ERROR"), Translation.Translate(@"\ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ProcessProfilesQueue()
        {
            Queue<string> ProfilesQueue = null;
            if (ProfilesQueue is null)
            {
                ProfilesQueue = new Queue<string>();
                log.Info("Profiles queue: Queue created.");
                List<string> RequestedProfiles = new List<string>();
                if (CommandLine.RunAll)
                {
                    RequestedProfiles.AddRange(Profiles.Keys);
                }
                else
                {
                    List<string> RequestedGroups = new List<string>();
                    foreach (string Entry in CommandLine.TasksToRun.Split(ProgramSetting.EnqueuingSeparator))
                    {
                        if (Entry.StartsWith(ProgramSetting.GroupPrefix.ToString()))
                        {
                            RequestedGroups.Add(Entry.Substring(1));
                        }
                        else
                        {
                            RequestedProfiles.Add(Entry);
                        }
                    }
                    foreach (ProfileHandler Profile in Profiles.Values)
                    {
                        if (RequestedGroups.Contains(Profile.GetSetting(ProfileSetting.Group, "")))
                        {
                            RequestedProfiles.Add(Profile.ProfileName);
                        }
                    }
                }

                foreach (string Profile in RequestedProfiles)
                {
                    if (Profiles.ContainsKey(Profile))
                    {
                        if (Profiles[Profile].ValidateConfigFile())
                        {
                            log.Info("Profiles queue: Registered profile " + Profile);
                            ProfilesQueue.Enqueue(Profile);
                        }
                    }
                }
            }

        }


        public void HandleFirstRun()
        {
            if (!ProgramConfig.ProgramSettingsSet(ProgramSetting.Language))
            {
                LanguageForm Lng = new LanguageForm();
                Lng.ShowDialog();
                Translation = LanguageHandler.GetSingleton(true);
            }
            //if (!ProgramConfig.ProgramSettingsSet(ProgramSetting.AutoUpdates))
            //{
            //    //Dim AutoUpdates As Boolean = If(Interaction.ShowMsg(Translation.Translate("\WELCOME_MSG"), Translation.Translate("\FIRST_RUN"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes, True, False)
            //    //此处简化
            //    bool AutoUpdates = false;
            //    ProgramConfig.SetProgramSetting(ProgramSetting.AutoUpdates, AutoUpdates);
            //}

            ProgramConfig.SaveProgramSettings();
        }

        public void ReloadProfiles()
        {
            Profiles.Clear();// 'Initialized in InitializeSharedObjects
            foreach (string ConfigFile in Directory.GetFiles(ProgramConfig.ConfigRootDir, "*.sync"))
            {
                string Name = Path.GetFileNameWithoutExtension(ConfigFile);
                Profiles.Add(Name, new ProfileHandler(Name));
            }
        }

        public void MessageLoop_ThreadExit(object sender, EventArgs e)
        {
            ExitNeeded = true;
            //Interaction.ToggleStatusIcon(false);

            //' Save last window information. Don't overwrite config file if running in scheduler mode.
            if (!(CommandLine.RunAs == CommandLine.RunMode.Scheduler))
            {
                ProgramConfig.SaveProgramSettings();
            }

            //'Calling ReleaseMutex would be the same, since Blocker necessary holds the mutex at this point (otherwise the app would have closed already).
            if (CommandLine.RunAs == CommandLine.RunMode.Scheduler) { Blocker.Close(); }
            log.Info("Program exited");
        }

        public bool SchedulerAlreadyRunning()
        {

            string MutexName = "[[CS Synchronicity scheduler]] " + Application.ExecutablePath.Replace(ProgramSetting.DirSep, '!').ToLower(System.Globalization.CultureInfo.InvariantCulture);
            if (MutexName.Length > 260) { MutexName = MutexName.Substring(0, 260); }

            log.Info(String.Format("Registering mutex: {0}", MutexName));

            try
            {
                Blocker = new Mutex(false, MutexName);
            }
            catch (AbandonedMutexException ex)
            {
                log.Info("Abandoned mutex detected");
                return false;
            }
            catch (UnauthorizedAccessException ex)
            {
                log.Info("Acess to the Mutex forbidden");
                return true;
            }


            return (!Blocker.WaitOne(0, false));
        }

      
    }
}
