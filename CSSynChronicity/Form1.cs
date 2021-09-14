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

        //Catch Ex As Exception
        //    If MessageBox.Show("A critical error has occured. Can we upload the error log? " & Environment.NewLine & "Here's what we would send:" & Environment.NewLine & Environment.NewLine & Ex.ToString & Environment.NewLine & Environment.NewLine & "If not, you can copy this message using Ctrl+C and send it to createsoftware@users.sourceforge.net." & Environment.NewLine, "Critical error", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
        //        Dim ReportingClient As New Net.WebClient
        //        Try
        //            ReportingClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded")
        //            MessageBox.Show(ReportingClient.UploadString(Branding.Web & "code/bug.php", "POST", "version=" & Application.ProductVersion & "/" & Revision.Build & "&msg=" & Ex.ToString), "Bug report submitted!", MessageBoxButtons.OK, MessageBoxIcon.Information)
        //        Catch SubEx As Net.WebException
        //            MessageBox.Show("Unable to submit report. Plead send the following to createsoftware@users.sourceforge.net (Ctrl+C): " & Environment.NewLine & Ex.ToString, "Unable to submit report", MessageBoxButtons.OK, MessageBoxIcon.Error)
        //        Finally
        //            ReportingClient.Dispose()
        //        End Try
        //    End If
        //    Throw
        //End Try
        }
    }
}
