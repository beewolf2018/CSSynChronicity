using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace CSSynChronicity.Interface
{
    public partial class MainForm : Form
    {
        ILog log;
        public int CurView;
        public List<string> ProfilesGroups = new List<string>();
        View[] Views = new View[] { View.Tile, View.Details, View.LargeIcon };
        ConfigHandler ProgramConfig = ConfigHandler.GetSingleton();
        LanguageHandler Translation = LanguageHandler.GetSingleton();
        MessageLoop Loop = MessageLoop.GetSingleton();
        public static Dictionary<string, ProfileHandler> Profiles;
        bool ReloadNeeded;
        public MainForm()
        {
            log4net.Config.XmlConfigurator.Configure();
            log = LogManager.GetLogger(typeof(MainForm));
            log.Info("MainForm程序初始化开始!");


            InitializeComponent();
            List<string> WindowSettings = new List<string>(ProgramConfig.GetProgramSetting(ProgramSetting.MainFormAttributes, "").Split(','));
            try
            {
                List<int> Values = new List<int>();
                WindowSettings.ForEach(Elem => Values.Add(System.Convert.ToInt32(Elem)));
                if (Values.Count == 4 && Values.TrueForAll(BetweenWithValue))
                {
                    this.Location = new System.Drawing.Point(Values[0], Values[1]);
                    this.Size = new System.Drawing.Size(Values[2], Values[3]);
                    this.StartPosition = FormStartPosition.Manual;
                }
            }
            catch (Exception ex) { }

            ReloadNeeded = false;

            //BuildIcons();

            this.Icon = ProgramConfig.Icon;
            this.ExitToolStripMenuItem.Image = this.DeleteToolStripMenuItem.Image;
            this.ExitToolStripMenuItem.Text = Translation.Translate("\\CANCEL_CLOSE", ";").Split(';')[1];

            Translation.TranslateControl(this);
            Translation.TranslateControl(this.ActionsMenu);
            Translation.TranslateControl(this.StatusIconMenu);

            //'Position the "About" label correctly
            //Dim PreviousWidth As Integer = AboutLinkLabel.Width
            //AboutLinkLabel.AutoSize = True
            //Dim NewWidth As Integer = AboutLinkLabel.Width + 16 + 4
            //AboutLinkLabel.AutoSize = False

            //AboutLinkLabel.Width = NewWidth
            //AboutLinkLabel.Location += New Drawing.Size(PreviousWidth - AboutLinkLabel.Width, 0)
        }

        private void BuildIcons()
        {
            for (int i = 0; i < this.SyncIcons.Images.Count - 2; i++)
            {
                Bitmap NewImg = new Bitmap(32, 32);
                Graphics Painter = Graphics.FromImage(NewImg);
                try
                {
                    if (i < 2)
                    {
                        Painter.DrawImage(ScheduleMenuItem.Image, 0, 0, 16, 16);
                    }
                    else
                    {
                        Painter.DrawImage(ScheduleMenuItem.Image, 9, 6, 16, 16);
                    }
                    Painter.DrawImageUnscaled(SyncIcons.Images[i], 0, 0);
                    SyncIcons.Images.Add(NewImg);
                }
                finally
                {
                    NewImg.Dispose();
                    Painter.Dispose();
                }
            }
        }

        private static bool BetweenWithValue(int value)
        {
            return value > 0 && value < 5000;
        }

        public void LoadDetails(string Name, bool Clear)
        {
            ProfileName.Text = Name;

            Method.Text = "";
            Source.Text = "";
            Destination.Text = "";
            LimitedCopy.Text = "";
            FileTypes.Text = "";
            Scheduling.Text = "";
            TimeOffset.Text = "";

            if (Clear)
                return;

            Method.Text = Profiles[Name].FormatMethod();
            Source.Text = Profiles[Name].GetSetting<string>(ProfileSetting.Source);
            Destination.Text = Profiles[Name].GetSetting<string>(ProfileSetting.Destination);

            Scheduling.Text = Translation.Translate(@"\" + Profiles[Name].Scheduler.Frequency.ToString().ToUpper(CultureInfo.InvariantCulture));

            switch (Profiles[Name].Scheduler.Frequency)
            {
                case ScheduleInfo.Freq.Weekly:
                    {
                        string Day = Translation.Translate(@"\WEEK_DAYS", ";;;;;;").Split(';')[Profiles[Name].Scheduler.WeekDay];
                        Scheduling.Text += Day;
                        break;
                    }

                case ScheduleInfo.Freq.Monthly:
                    {
                        Scheduling.Text += Profiles[Name].Scheduler.MonthDay;
                        break;
                    }
            }

            if (Profiles[Name].Scheduler.Frequency == ScheduleInfo.Freq.Never)
                Scheduling.Text = "";
            else
                Scheduling.Text += ", " + Profiles[Name].Scheduler.Hour.ToString("D2") + Translation.Translate(@"\H_M_SEP") + Profiles[Name].Scheduler.Minute.ToString("D2");

            TimeOffset.Text = Profiles[Name].GetSetting<int>(ProfileSetting.TimeOffset).ToString();

            switch (Profiles[Name].GetSetting<int>(ProfileSetting.Restrictions, 0))
            {
                case 0:
                    {
                        LimitedCopy.Text = Translation.Translate(@"\NO");
                        break;
                    }

                case 1:
                case 2:
                    {
                        LimitedCopy.Text = Translation.Translate(@"\YES");
                        break;
                    }
            }

            switch (Profiles[Name].GetSetting<int>(ProfileSetting.Restrictions, 0))
            {
                case 1:
                    {
                        FileTypes.Text = Profiles[Name].GetSetting<string>(ProfileSetting.IncludedTypes, "");
                        break;
                    }

                case 2:
                    {
                        FileTypes.Text = "-" + Profiles[Name].GetSetting<string>(ProfileSetting.ExcludedTypes, "");
                        break;
                    }
            }
        }


        private void Actions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Actions.SelectedIndices.Count == 0 || Actions.SelectedIndices[0] == 0)
            {
                if (Actions.SelectedIndices.Count == 0)
                    LoadDetails("", true);
                else if (Actions.SelectedIndices[0] == 0)
                    LoadDetails(Translation.Translate(@"\NEW_PROFILE"), true);

                ActionsMenu.Close();
                return;
            }

            LoadDetails(CurrentProfile(), false);
        }

        private void Actions_MouseClick(object sender, MouseEventArgs e)
        {
            if (Actions.SelectedItems.Count == 0) return;
            if (Actions.SelectedIndices[0] == 0)
            {
                Actions.LabelEdit = true;
                Actions.SelectedItems[0].BeginEdit();
            }
            else
            {
                ActionsMenu.Show(Actions, e.Location);
            }
        }

        private void Actions_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            Actions.LabelEdit = false;

            if (e.Label is null || e.Label.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                e.CancelEdit = true;
                return;
            }

            //if(e.Item == 0)
            //{
            //    e.CancelEdit = true;
            //}

            //SettingsForm settingsForm = new SettingsForm(e.Label, ProfilesGroups);
            //settingsForm.ShowDialog();

            if (e.Item == 0)
            {
                e.CancelEdit = true;
                SettingsForm SettingsForm = new SettingsForm(e.Label, ProfilesGroups);

                // Since this happens while the label is being edited, the normal blinking mecanisms that brings the modal dialog to front when the owner window is clicked doesn't work.
                // Explicitly specifying the owner forces the modal dialog to always be on top of the caller.
                SettingsForm.ShowDialog(this); // This is especially important if users minimize CS while editing the label - in this case, returning to CS shows a blocked main windows, and users must loop through all windows (Ctrl+Tab) to reach the settings dialog.
            }
            else if (!Profiles[Actions.Items[e.Item].Text].Rename(e.Label))
                e.CancelEdit = true;

            // Tracker #3357854: Reloading immediately deletes the item being edited, and the edit is committed on the first item of the list.
            this.BeginInvoke(new Action(ReloadProfilesList));


        }

        private void ToolStripHeader_Click(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Maximized;
            this.WindowState = FormWindowState.Normal;
            this.Activate();
            this.Show();
        }

        private void ApplicationTimer_Tick(object sender, EventArgs e)
        {
            //log.Info("ApplicationTimer_Tick.");
            //csNotifyicon.ShowBalloonTip(1000, "Scheduling_Tick", "Good good good girl.", ToolTipIcon.None);
            //ApplicationTimer.Stop();
            if (ProgramConfig.CanGoOn == false) { return; }// 'Don't start next sync yet.

            //Loop.ReloadScheduledProfiles();
            if (Loop.ScheduledProfiles.Count == 0)
            {
                //log.Info("Scheduler: No profiles left to run.");
                //Application.Exit();
                return;
            }

            else if (Loop.ScheduledProfiles.Count > 0)
            {

                //ApplicationTimer.Stop();
                SchedulerEntry NextInQueue = Loop.ScheduledProfiles[0];
                string Status = Translation.TranslateFormat("\\SCH_WAITING", NextInQueue.Name, (NextInQueue.NextRun == ScheduleInfo.DATE_CATCHUP ? "..." : NextInQueue.NextRun.ToString()));
                //Interaction.StatusIcon.Text = Status.Length >= 64 ? Status.Substring(0, 63) : Status;

                if (DateTime.Compare(NextInQueue.NextRun, DateTime.Now) <= 0)
                {
                    ApplicationTimer.Stop();
                    log.Info("ApplicationTimer.Stop()");
                    csNotifyicon.ShowBalloonTip(5000, "ScheduledProfiles", "开始同步.", ToolTipIcon.None);
                    log.Info("Scheduler: Launching " + NextInQueue.Name);
                    SynchronizeForm SyncForm = new SynchronizeForm(NextInQueue.Name, false, NextInQueue.CatchUp);
                    SyncForm.SyncFinished += ScheduledProfileCompleted;
                    Loop.ScheduledProfiles.RemoveAt(0);
                    SyncForm.StartSynchronization(false);
                    ApplicationTimer.Start();
                    log.Info("ApplicationTimer.Start()");
                }
            }
        }

        public void ScheduledProfileCompleted(string ProfileName, bool Completed)
        {
            if (Completed)
            {
                log.Info("Scheduler: " + ProfileName + " completed successfully.");
                if (Profiles.ContainsKey(ProfileName))
                    Loop.ScheduledProfiles.Add(new SchedulerEntry(ProfileName, Profiles[ProfileName].Scheduler.NextRun(), false, false));
            }
            else
            {
                log.Info("Scheduler: " + ProfileName + " reported an error, and will run again in 4 hours."); // If ProfileName has been removed, ReloadScheduledProfiles will unschedule it.
                Loop.ScheduledProfiles.Add(new SchedulerEntry(ProfileName, DateTime.Now.AddHours(4), true, true));
            }
            ApplicationTimer.Start();
            log.Info("ApplicationTimer.Start()");
        }


        private void ScheduleMenuItem_Click(object sender, EventArgs e)
        {
            SchedulingForm SchedForm = new SchedulingForm(CurrentProfile());
            SchedForm.ShowDialog();
            ReloadProfilesList();
            Loop.RedoSchedulerRegistration();
        }

        private string CurrentProfile()
        {
            return Actions.SelectedItems[0].Text;
        }

        private void ReloadProfilesList()
        {

            if (this.IsDisposed)
                return;
            ListViewItem CreateProfileItem = Actions.Items[0];

            Loop.ReloadProfiles();
            Profiles = MessageLoop.Profiles;
            Actions.Items.Clear();
            Actions.Items.Add(CreateProfileItem).Group = Actions.Groups[0];

            ProfilesGroups.Clear();
            foreach (ProfileHandler Profile in Profiles.Values)
            {
                ListViewItem NewItem = Actions.Items.Add(Profile.ProfileName);

                NewItem.Group = Actions.Groups[1];
                NewItem.ImageIndex = Profile.GetSetting<int>(ProfileSetting.Method, ProfileSetting.DefaultMethod) + Profile.Scheduler.Frequency == ScheduleInfo.Freq.Never ? 0 : 4;
                NewItem.SubItems.Add(Profile.FormatMethod()).ForeColor = System.Drawing.Color.DarkGray;
                if (Actions.View == View.Details)
                    NewItem.SubItems.Add(Profile.FormatLastRun("D3")); // Fix #3515300

                string GroupName = Profile.GetSetting<string>(ProfileSetting.Group, "");
                if (GroupName != "")
                {
                    if (!ProfilesGroups.Contains(GroupName))
                    {
                        ProfilesGroups.Add(GroupName);
                        Actions.Groups.Add(new ListViewGroup(GroupName, GroupName));
                    }

                    NewItem.Group = Actions.Groups[GroupName];
                }
            }

            TipsLabel.Visible = (Profiles.Count == 0 & TipsLabel.Text != "");
            TipsLabel.Text = string.Format(TipsLabel.Text, Translation.Translate(@"\NEW_PROFILE_LABEL"));

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ApplicationTimer.Start();
            log.Info("ApplicationTimer.Start() ");
            ReloadProfilesList();
            Loop.RedoSchedulerRegistration();
            SetView(ProgramConfig.GetProgramSetting<int>(ProgramSetting.MainView, 0));
            SetFont(ProgramConfig.GetProgramSetting<int>(ProgramSetting.FontSize, System.Convert.ToInt32(Actions.Font.Size)));
            Loop.ReloadScheduledProfiles();
            log.Info("Loop.ReloadScheduledProfiles over ");

        }
        public void SetView(int Offset)
        {
            CurView = (CurView + Offset) % Views.Length;
            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
            Actions.View = Views[CurView];

            ReloadProfilesList();
            Actions.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }
        public void SetFont(float Size)
        {
            Size = System.Convert.ToSingle(Math.Max(6.25, Math.Min(24.25, Size)));
            Actions.Font = new System.Drawing.Font(Actions.Font.Name, Size);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //the fellowing four col code from origin software.
            //string WindowAttributes = string.Format("{0},{1},{2},{3}", this.Location.X, this.Location.Y, this.Size.Width, this.Size.Height);
            //ProgramConfig.SetProgramSetting<string>(ProgramSetting.MainFormAttributes, WindowAttributes);
            //ProgramConfig.SetProgramSetting<int>(ProgramSetting.MainView, CurView);
            //ProgramConfig.SetProgramSetting<float>(ProgramSetting.FontSize, Actions.Font.Size);

            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Loop.ReloadScheduledProfiles();
                this.ApplicationTimer.Start();
                
                HideMainForm();
                return;
            }

        }

        private void SynchronizeMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckValidity())
                return;

            SynchronizeForm SyncForm = new SynchronizeForm(CurrentProfile(), false, false);
            SetVisible(false);
            SyncForm.StartSynchronization(true);
            //SyncForm.StartSynchronization(false);
            SyncForm.ShowDialog();
            SetVisible(true);
            SyncForm.Dispose();
        }
        public void SetVisible(bool Status)
        {
            if (this.IsDisposed)
                return;
            this.Visible = Status;
        }

        private bool CheckValidity()
        {
            if (!Profiles[CurrentProfile()].ValidateConfigFile(true, true))
            {
                //Interaction.ShowMsg(Translation.Translate(@"\INVALID_CONFIG"), Translation.Translate(@"\ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (true)
            //if (Interaction.ShowMsg(Translation.TranslateFormat(@"\DELETE_PROFILE", CurrentProfile()), Translation.Translate(@"\CONFIRM_DELETION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Profiles[CurrentProfile()].DeleteConfigFile();
                Profiles[CurrentProfile()] = null;
                Actions.Items.RemoveAt(Actions.SelectedIndices[0]);
                TipsLabel.Visible = (Actions.Items.Count == 1);
            }
        }

        private void RenameMenuItem_Click(object sender, EventArgs e)
        {
            Actions.LabelEdit = true;
            Actions.SelectedItems[0].BeginEdit();
        }

        private void csNotifyicon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowMainForm();
        }

        private void ShowMainForm()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        private void HideMainForm()
        {
            this.Hide();
        }

        private void languageSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            csNotifyicon.ShowBalloonTip(1000, "Config Language", "Good boy.", ToolTipIcon.None);
            LanguageForm Lng = new LanguageForm();
            Lng.ShowDialog();
            Translation = LanguageHandler.GetSingleton(true);
            csNotifyicon.ShowBalloonTip(1000, "Config Language", "Good girl.", ToolTipIcon.None);
        }

        private void ChangeSettingsMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm SettingsForm = new SettingsForm(CurrentProfile(), ProfilesGroups);
            SettingsForm.ShowDialog();
            ReloadProfilesList();
        }
    }
}
