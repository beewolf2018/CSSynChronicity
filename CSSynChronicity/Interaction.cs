using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;

namespace CSSynChronicity
{
    public static class Interaction
    {

        public static CultureInfo InvariantCulture = CultureInfo.InvariantCulture;
        public static ConfigHandler ProgramConfig = new ConfigHandler();
        public static NotifyIcon StatusIcon = new NotifyIcon() { BalloonTipTitle = "Create Synchronicity", BalloonTipIcon = ToolTipIcon.Info };

        public static int BalloonQueueSize;
        private static bool StatusIconVisible;

        private static string BalloonTipTarget = "";
        private static ToolTip SharedToolTip = new ToolTip() { UseFading = false, UseAnimation = false, ToolTipIcon = ToolTipIcon.Info };
    
         public static void LoadStatusIcon()
        {
            //Assembly assem = System.Reflection.Assembly.GetExecutingAssembly();
            //// Enumerate the assembly's manifest resources
            //foreach (string resourceName in assem.GetManifestResourceNames())
            //{
            //    MessageBox.Show(resourceName);
            //}

            StatusIcon.Icon = new Icon(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("CSSynChronicity.Icons.program_icons.icon-16x16.ico"));
        }

        public static void ToggleStatusIcon(bool Status )
        {
            StatusIcon.Visible = Status && (! CommandLine.Silent);
            StatusIconVisible = StatusIcon.Visible;
        }

        public static void ShowBalloonTip(string  Msg , string  File = "")
        {
            if(CommandLine.Silent)
            {
                ProgramConfig.LogAppEvent(String.Format("Interaction: Silent: Balloon tip discarded: [{0}].", Msg));
                return;
            }
            else
            {
                ProgramConfig.LogAppEvent(String.Format("Interaction: Balloon tip shown: [{0}].", Msg));
            }

            if (Msg == "") return;
            BalloonTipTarget = File;
            StatusIcon.BalloonTipText = Msg;
            BalloonQueueSize += 1;// 'Prevents StatusIcon_BalloonTipClosed from hiding the icon if a balloon is closed because another one is being shown.
            StatusIcon.Visible = true;
            StatusIcon.ShowBalloonTip(15000);
        }
        public static void ShowToolTip(Control Ctrl)
        {
            TreeView T = Ctrl as TreeView;// 'Exit if Ctrl is a TreeView without checkboxes.
            if (!(T is null) && !T.CheckBoxes) return;
            int Offset = (Ctrl is RadioButton) || (Ctrl is CheckBox) ? 12 : 1;
            string[] Pair = string.Format(Ctrl.Tag.ToString(), Ctrl.Text).Split(";".ToCharArray(), 2);
            try
            {
                Point Pos = new Point(0, Ctrl.Height + Offset);
                if(Pair.GetLength(0) ==1)
                {
                    SharedToolTip.ToolTipTitle = "";
                    SharedToolTip.Show(Pair[0], Ctrl, Pos);
                }
                else if(Pair.GetLength(0) > 1)
                {
                    SharedToolTip.ToolTipTitle = Pair[0];
                    SharedToolTip.Show(Pair[1], Ctrl, Pos);
                }
            }
            catch(InvalidOperationException ex)
            {
                //'See bug #3076129
            }

        }
        public static void HideToolTip(Control sender )
        {
            SharedToolTip.Hide(sender);

        }


        public static void ShowDebug(string Text ,string Caption = "")
        {
            ShowMsg(Text, Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        public static DialogResult ShowMsg(string Text, string Caption = "", MessageBoxButtons Buttons = MessageBoxButtons.OK, MessageBoxIcon Icon = MessageBoxIcon.None)
        {
            if(CommandLine.Silent)
            {
                ProgramConfig.LogAppEvent(String.Format("Interaction: Silent: Message Box discarded with default answer: [{0}] - [{1}].", Caption, Text));
                return DialogResult.OK;
            }
            DialogResult Result = MessageBox.Show(Text, Caption, Buttons, Icon);
            if (CommandLine.Log)
            {
                ProgramConfig.LogAppEvent(String.Format("Interaction: Message [{0}] - [{1}] received answer [{2}].", Caption, Text, Result.ToString()));
            }
            return Result;
        }

        private static void BallonClick(object sender, System.EventArgs e)
        {
            if (BalloonTipTarget != null)
                StartProcess(BalloonTipTarget);
        }

        private static void StatusIcon_BalloonTipClosed(object sender, System.EventArgs e)
        {
            BalloonQueueSize -= 1;
            if (BalloonQueueSize == 0)
                StatusIcon.Visible = StatusIconVisible;
        }

        public static void StartProcess(string Address, string Args = "")
        {
            try
            {
                System.Diagnostics.Process.Start(Address, Args);
            }
            catch
            {
            }
        }

        public static string FormatDate(DateTime Value)
        {
            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
            return "";
        }
      
    }
     class SyncingListSorter : IComparer<SyncingItem>
    {
        //    Implements Collections.Generic.IComparer(Of SyncingItem)

        public SortOrder Order;
        public int SortColumn;
        
        public SyncingListSorter(int ColumnId)
        {
            SortColumn = ColumnId;
            Order = SortOrder.Ascending;
        }
        
        public int Compare(SyncingItem xs, SyncingItem ys )
        {
            int Result;
            switch(SortColumn)
            {
                case 0:
                    Result = xs.Type.CompareTo(ys.Type);
                    break;
                case 1:
                    Result = (xs.Action == ys.Action)? xs.IsUpdate.CompareTo(ys.IsUpdate): xs.Action.CompareTo(ys.Action);
                    break;
                case 2:
                    Result = xs.Side.CompareTo(ys.Side);
                    break;
                case 3:
                    Result = string.Compare(xs.Path, ys.Path, true);
                    break;
                default:
                    Result = xs.RealId.CompareTo(ys.RealId);
                    break;
            }
            return (Order == SortOrder.Ascending ? 1 : -1) * Result;
        }

        public void RegisterClick(ColumnClickEventArgs e )
        {
            if(e.Column == SortColumn)
            {
                Order = Order == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
            }
            else
            {
                SortColumn = e.Column;
                Order = SortOrder.Ascending;
            }
        }

    }
}
