using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSSynChronicity.Interface
{
    public partial class SchedulingForm : Form
    {
        public int CurView;
        public List<string> ProfilesGroups = new List<string>();
        //View[] Views = new View() { View.Tile; View.Details; View.LargeIcon; };
        ConfigHandler ProgramConfig = ConfigHandler.GetSingleton();
        LanguageHandler Translation = LanguageHandler.GetSingleton();
        bool ReloadNeeded;
        ProfileHandler Handler;
        public SchedulingForm(string Name)
        {
            InitializeComponent();
            Handler = new ProfileHandler(Name);
        }

        private void Save_Click(object sender, EventArgs e)
        {
            try
            {
                ProgramConfig.RegisterBoot();

                if (!Enable.Checked)
                    Handler.Scheduler.Frequency = ScheduleInfo.Freq.Never;
                else
                {
                    Handler.Scheduler.Hour = Time.Value.Hour;
                    Handler.Scheduler.Minute = Time.Value.Minute;
                    Handler.Scheduler.WeekDay = WeekDay.SelectedIndex;
                    Handler.Scheduler.MonthDay = System.Convert.ToInt32(MonthDay.Value);
                    Handler.Scheduler.Frequency = DailyBtn.Checked ? ScheduleInfo.Freq.Daily : WeeklyBtn.Checked ? ScheduleInfo.Freq.Weekly : ScheduleInfo.Freq.Monthly;
                }

                Handler.SetSetting<bool>(ProfileSetting.CatchUpSync, Catchup.Checked);

                Handler.SaveScheduler();
                Handler.SaveConfigFile();
            }
            catch (Exception ex)
            {
                //Interaction.ShowMsg(Translation.Translate(@"\REG_ERROR"), Translation.Translate(@"\ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.Close();
        }

        private void SchedulingForm_Load(object sender, EventArgs e)
        {
            Translation.TranslateControl(this);
            Time.CustomFormat = "HH" + Translation.Translate(@"\H_M_SEP") + "mm";
            WeekDay.Items.AddRange(Translation.Translate(@"\WEEK_DAYS").Split(';'));
            if (WeekDay.Items.Count > 0)
                WeekDay.SelectedIndex = 0;

            LoadToForm();
        }
        public void LoadToForm()
        {
            Enable.Checked = true;
            switch (Handler.Scheduler.Frequency)
            {
                case ScheduleInfo.Freq.Never:
                    {
                        Enable.Checked = false;
                        break;
                    }

                default:
                    {
                        Time.Value = new DateTime(2021, 1, 1, Handler.Scheduler.Hour, Handler.Scheduler.Minute, 0);

                        switch (Handler.Scheduler.Frequency)
                        {
                            case ScheduleInfo.Freq.Daily:
                                {
                                    DailyBtn.Checked = true;
                                    break;
                                }

                            case ScheduleInfo.Freq.Weekly:
                                {
                                    WeeklyBtn.Checked = true;
                                    WeekDay.SelectedIndex = Handler.Scheduler.WeekDay;
                                    break;
                                }

                            case ScheduleInfo.Freq.Monthly:
                                {
                                    MonthlyBtn.Checked = true;
                                    MonthDay.Value = Handler.Scheduler.MonthDay;
                                    break;
                                }
                        }

                        break;
                    }
            }

            Handler.CopySetting(ProfileSetting.CatchUpSync, Catchup.Checked, true);
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Enable_CheckedChanged(object sender, EventArgs e)
        {
            //Panel.Enabled = Enable.Checked;
            Catchup.Enabled = Enable.Checked;
            TimeSelectionPanel.Enabled = Enable.Checked;
        }

        private void Enable_CheckedChanged_1(object sender, EventArgs e)
        {
            //Panel.Enabled = Enable.Checked;
            Catchup.Enabled = Enable.Checked;
            TimeSelectionPanel.Enabled = Enable.Checked;
        }
    }
}
