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
    public partial class LanguageForm : Form
    {
        ConfigHandler ProgramConfig = new ConfigHandler();
        public LanguageForm()
        {
            InitializeComponent();
            this.Icon = ProgramConfig.Icon;
            LanguageHandler.FillLanguagesComboBox(LanguagesList);
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LanguageForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(LanguagesList.SelectedIndex !=-1)
            {
                ProgramConfig.SetProgramSetting(ProgramSetting.Language, LanguagesList.SelectedItem.ToString().Split('-')[0].Trim());
                ProgramConfig.SaveProgramSettings();
            }
        }
    }
}
