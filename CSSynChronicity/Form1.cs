using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSSynChronicity
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Interface.MainForm mainForm = new Interface.MainForm();
            //mainForm.Show();
            try
            {
                MessageLoop MsgLoop = new MessageLoop();
                if (!MsgLoop.ExitNeeded) Application.Run(MsgLoop);
            }
            catch(Exception ex)
            {

            }
        }
    }
}
