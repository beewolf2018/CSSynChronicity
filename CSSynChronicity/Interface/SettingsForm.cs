using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace CSSynChronicity.Interface
{
    public partial class SettingsForm : Form
    {       
        ProfileHandler Handler;
        ConfigHandler ProgramConfig;
        LanguageHandler Translation;
        ProfileSetting profileSetting;

        bool ProcessingNodes;//'= False 'Some background activity is occuring, don't record events.
        bool InhibitAutocheck;// '= False 'Record events, but don't treat them as user input.
        bool ClickedRightTreeView; //'= False

        string PrevLeft = "-1"; //'Initiate to an invalid path value to force reloading.
        string PrevRight = "-1"; //'These values are used to check whether the folder tree should be reloaded.

        List<FileNamePattern> ExcludedFolderPatterns = new List<FileNamePattern>();
        readonly Color EXCLUDED_FORECOLOR = Color.LightGray;

        public SettingsForm(string Name, List<string> Groups)
        {
            ProgramConfig = ConfigHandler.GetSingleton();
            Handler = new ProfileHandler(Name);
            profileSetting = new ProfileSetting();
            Translation = new LanguageHandler();

            InitializeComponent();
            if (ProgramConfig.GetProgramSetting(ProgramSetting.Autocomplete, true))
            {
                FromTextBox.AutoCompleteMode = AutoCompleteMode.None;
                ToTextBox.AutoCompleteMode = AutoCompleteMode.None;
            }
            GroupNameBox.Items.AddRange(Groups.ToArray());

        }

        private void BrowseLButton_Click(object sender, EventArgs e)
        {
            BrowseTo(Translation.Translate("\\CHOOSE_SOURCE"), FromTextBox);
        }

        private void BrowseTo(string DialogMessage, TextBox TextboxField)
        {
            FolderBrowser.Description = DialogMessage;
            if( ! (TextboxField.Text == "") &&  Directory.Exists(ProfileHandler.TranslatePath(TextboxField.Text)) )
            {
                FolderBrowser.SelectedPath = ProfileHandler.TranslatePath(TextboxField.Text);
            }
            //    FolderBrowser.SelectedPath = ProfileHandler.TranslatePath(TextboxField.Text)
            //End If
            if(FolderBrowser.ShowDialog() == DialogResult.OK )
            {
                if (TextboxField.Text.StartsWith("\""))
                {
                    //TextboxField.Text = ProfileHandler.TranslatePath_Inverse(FolderBrowser.SelectedPath);
                    TextboxField.Text = FolderBrowser.SelectedPath;
                }
                else
                {
                    TextboxField.Text = FolderBrowser.SelectedPath;
                }
            }
           

        }

        private void BrowseRButton_Click(object sender, EventArgs e)
        {
            BrowseTo(Translation.Translate("\\CHOOSE_DEST"), ToTextBox);
        }

        private void SwapButton_Click(object sender, EventArgs e)
        {
            string temp = FromTextBox.Text;
            FromTextBox.Text = ToTextBox.Text;
            ToTextBox.Text = temp;
        }

        private void LeftReloadButton_Click(object sender, EventArgs e)
        {
            ReloadTrees(true);
        }

        private void ReloadTrees(bool AllowFullReload, bool ForceRight = false, bool AutoCheckRoot = true)
        {
            ReloadButton.Enabled = false;
            SaveButton.Enabled = false;
            Loading.Visible = true;

            //Only reload fully if the user didn't input any text. Adding an extra trailing "\" for example will disable FullReload.
            //Dim FullReload As Boolean = AllowFullReload And FromTextBox.Text = PrevLeft And ToTextBox.Text = PrevRight And LeftView.Enabled And RightView.Enabled
            bool FullReload = AllowFullReload && FromTextBox.Text == PrevLeft && ToTextBox.Text == PrevRight && LeftView.Enabled && RightView.Enabled;
            Cleanup_Paths();
            //'Unless FullReload is true, and no path has changed, only the trees where paths have changed are reloaded.
            if (FullReload || PrevLeft != FromTextBox.Text)
            {
                LoadTree(LeftView, FromTextBox.Text, Handler.LeftCheckedNodes, AutoCheckRoot);
            }

            if (FullReload || PrevRight != ToTextBox.Text || ForceRight)
            {
                LoadTree(RightView, ToTextBox.Text, Handler.RightCheckedNodes, AutoCheckRoot, CreateDestOption.Checked);
            }


            Loading.Visible = false;
            ReloadButton.Enabled = true;

            PrevLeft = LeftView.Enabled ? FromTextBox.Text : "-1";
            PrevRight = RightView.Enabled ? ToTextBox.Text : "-1";

            CheckSettings();
        }

        private void LoadTree(TreeView Tree, string OriginalPath, Dictionary<string, bool> CheckedNodes, bool AutoCheckRoot, bool DynamicDest = false)
        {
            Tree.Nodes.Clear();

            string Path = ProfileHandler.TranslatePath(OriginalPath) + ProgramSetting.DirSep;
            Tree.Enabled = OriginalPath != "" && (DynamicDest || Directory.Exists(Path));

            if (Tree.Enabled)
            {
                Tree.BackColor = Color.White;
                Tree.Nodes.Add("");
                SetRootPathDisplay(true); //'Needed for the FullPath method, see tracker #3006324
                if (!DynamicDest)
                {
                    try
                    {
                        foreach (string Dir in Directory.GetDirectories(Path))
                        {
                            Application.DoEvents();
                            Tree.Nodes[0].Nodes.Add(GetFileOrFolderName(Dir));

                        }

                    }
                    catch (Exception ex)
                    {
                        Tree.Nodes.Clear();
                        Tree.Enabled = false;

                    }
                }
            }
            if (!Tree.Enabled) { Tree.BackColor = Color.LightGray; }
        }

        private string GetFileOrFolderName(string Path)
        {
            return Path.Substring(Path.LastIndexOf(ProgramSetting.DirSep) + 1);// 'IO.Path.* -> Bad because of separate file/folder handling.
        }

        private void SetRootPathDisplay(bool Show)
        {
            if (Show)
            {
                if (!(FromTextBox.Text == "") && LeftView.Nodes.Count > 0) { LeftView.Nodes[0].Text = FromTextBox.Text; }
                if (!(ToTextBox.Text == "") && RightView.Nodes.Count > 0) { RightView.Nodes[0].Text = ToTextBox.Text; }
            }
            else
            {
                if (LeftView.Nodes.Count > 0) LeftView.Nodes[0].Text = "";
                if (RightView.Nodes.Count > 0) RightView.Nodes[0].Text = "";
            }

        }

        private void CheckSettings()
        {
            CheckPath(FromTextBox, false);
            CheckPath(ToTextBox, CreateDestOption.Checked);

            ToggleTree(PrevLeft, FromTextBox, LeftView, LeftReloadButton);
            ToggleTree(PrevRight, ToTextBox, RightView, RightReloadButton);

            SaveButton.Enabled = LeftView.Enabled && RightView.Enabled;
            ReloadButton.BackColor = SaveButton.Enabled ? System.Drawing.SystemColors.Control : System.Drawing.Color.Orange;
        }

        private static void ToggleTree(string PrevPath, TextBox Box, TreeView Tree, Button Btn)
        {
            Tree.Enabled = (Cleanup(Box.Text) == PrevPath);
            Btn.Visible = !Tree.Enabled;
        }

        private static void CheckPath(TextBox PathBox, bool Force)
        {
            if (PathBox.Text == "" || Force || Directory.Exists(ProfileHandler.TranslatePath(PathBox.Text)))
            {
                PathBox.BackColor = Color.White;
            }
            else
            {
                PathBox.BackColor = Color.LightPink;
            }
        }


        private static string Cleanup(string Path)
        {
            return Path.TrimEnd(new Char[] { ProgramSetting.DirSep, ' ' });
        }

        private void Cleanup_Paths()
        {
            FromTextBox.Text = Cleanup(FromTextBox.Text);
            ToTextBox.Text = Cleanup(ToTextBox.Text);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            UpdateSettings(false);
            if (Handler.ValidateConfigFile() && Handler.SaveConfigFile()) this.Close();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            Translation.TranslateControl(this);
            Translation.TranslateControl(ExpertMenu);
            LeftView.PathSeparator = ProgramSetting.DirSep.ToString();
            RightView.PathSeparator = ProgramSetting.DirSep.ToString();
            MoreLabel.Visible = ProgramConfig.GetProgramSetting(ProgramSetting.ExpertMode, false);

            if (!Handler.IsNewProfile) UpdateSettings(true);

            this.Text = Translation.TranslateFormat("\\PROFILE_SETTINGS", Handler.ProfileName);
        }

        private void UpdateSettings(bool LoadToForm)
        {
            Cleanup_Paths();

            //'Careful: Using .ToString here would break the ByRef passing of the second argument. Problem with Option strict.
            Handler.CopySetting(ProfileSetting.Source, FromTextBox.Text, LoadToForm);
            Handler.CopySetting(ProfileSetting.Destination, ToTextBox.Text, LoadToForm);
            Handler.CopySetting(ProfileSetting.IncludedTypes, IncludedTypesTextBox.Text, LoadToForm);
            Handler.CopySetting(ProfileSetting.ExcludedTypes, ExcludedTypesTextBox.Text, LoadToForm);
            Handler.CopySetting(ProfileSetting.ReplicateEmptyDirectories, ReplicateEmptyDirectoriesOption.Checked, LoadToForm);
            Handler.CopySetting(ProfileSetting.MayCreateDestination, CreateDestOption.Checked, LoadToForm);
            Handler.CopySetting(ProfileSetting.StrictDateComparison, StrictDateComparisonOption.Checked, LoadToForm);
            Handler.CopySetting(ProfileSetting.PropagateUpdates, PropagateUpdatesOption.Checked, LoadToForm);
            Handler.CopySetting(ProfileSetting.StrictMirror, StrictMirrorOption.Checked, LoadToForm);
            Handler.CopySetting(ProfileSetting.TimeOffset, TimeOffset.Value, LoadToForm);
            Handler.CopySetting(ProfileSetting.Checksum, ChecksumOption.Checked, LoadToForm);
            Handler.CopySetting(ProfileSetting.CheckFileSize, CheckFileSizeOption.Checked, LoadToForm);
            Handler.CopySetting(ProfileSetting.Group, GroupNameBox.Text, LoadToForm);
            //'Handler.CopySetting(ProfileSetting.ExcludedFolders, ExcludedFoldersBox.Text, LoadToForm)
            //'Hidden settings are not added here

            LoadExcludedFolderPatterns();// 'Load excluded folder patterns from UI (ExcludedTypesTextBox.Text) and settings dictionary (ExcludedFolders)

            //'Note: Behaves correctly when no radio button is checked, although CopyAllFiles is unchecked.
            int Restrictions = ((CopyAllFilesCheckBox.Checked ? 0 : 1) * ((IncludeFilesOption.Checked ? 1 : 0) + 2 * (ExcludeFilesOption.Checked ? 1 : 0)));

            ProfileSetting.SyncMethod Method;
            if (LRMirrorMethodOption.Checked) Method = ProfileSetting.SyncMethod.LRMirror;
            else if (TwoWaysIncrementalMethodOption.Checked) Method = ProfileSetting.SyncMethod.BiIncremental;
            else Method = ProfileSetting.SyncMethod.LRIncremental;

            if (LoadToForm)
            {
                switch (Handler.GetSetting<int>(ProfileSetting.Method, ProfileSetting.DefaultMethod))
                {
                    case (int)ProfileSetting.SyncMethod.LRMirror:
                        LRMirrorMethodOption.Checked = true;
                        break;
                    case (int)ProfileSetting.SyncMethod.BiIncremental:
                        TwoWaysIncrementalMethodOption.Checked = true;
                        break;
                    default:
                        LRIncrementalMethodOption.Checked = true;
                        break;

                }
                CopyAllFilesCheckBox.Checked = false;
                switch (Handler.GetSetting<int>(ProfileSetting.Restrictions))
                {
                    case (int)ProfileSetting.SyncMethod.LRMirror:
                        LRMirrorMethodOption.Checked = true;
                        break;
                    case (int)ProfileSetting.SyncMethod.BiIncremental:
                        TwoWaysIncrementalMethodOption.Checked = true;
                        break;
                    default:
                        LRIncrementalMethodOption.Checked = true;
                        break;

                }
            }
            else
            {
                Handler.SetSetting(ProfileSetting.LastModified, DateTime.UtcNow);// 'File.LastWriteTime is updated when saving last run.
                Handler.SetSetting(ProfileSetting.Method, (int)Method);// 'SetSetting(Of ProfileSetting.SyncMethod) would save a string, unparsable by GetSetting(Of ProfileSetting.SyncMethod)
                Handler.SetSetting(ProfileSetting.Restrictions, Restrictions);

                SetRootPathDisplay(false);
                if (LeftView.Enabled)
                {
                    Handler.LeftCheckedNodes.Clear();
                    BuildCheckedNodesList(Handler.LeftCheckedNodes, LeftView.Nodes[0]);
                    Handler.SetSetting(ProfileSetting.LeftSubFolders, GetString(Handler.LeftCheckedNodes));
                }

                if (RightView.Enabled)
                {
                    if (RightView.CheckBoxes || Handler.GetSetting<string>(ProfileSetting.RightSubFolders) == null)
                    {
                        Handler.RightCheckedNodes.Clear();
                        BuildCheckedNodesList(Handler.RightCheckedNodes, RightView.Nodes[0]);
                        Handler.SetSetting(ProfileSetting.RightSubFolders, GetString(Handler.RightCheckedNodes));
                    }
                }
                SetRootPathDisplay(true);

            }
        }

        private static string GetString(Dictionary<string, bool> Table)
        {
            System.Text.StringBuilder ListString = new System.Text.StringBuilder();
            foreach (string Node in Table.Keys)
            {
                ListString.Append(Node).Append(";");//'Must end with a '; ', since '' means that no directories were selected, while '; ' means that the root directory was selected, with no subfolders included. See LoadSubFoldersList()
            }
            return ListString.ToString();
        }

        private void BuildCheckedNodesList(Dictionary<string, bool> NodesList, TreeNode Node)
        {
            int OverAllNodeStatus = OverAllCheckStatus(Node);

            if (Node.Checked || Node.TreeView.CheckBoxes == false)
            {
                if (OverAllNodeStatus == 1)
                {
                    NodesList.Add(Node.FullPath + "*", true);
                    return;
                }
                else
                {
                    NodesList.Add(Node.FullPath, false);
                }
            }
            else
            {
                if (OverAllNodeStatus == 0) return;// 'No checked subnode
            }

            //'If node isn't checked
            for (int NodeId = 0; NodeId <= Node.Nodes.Count - 1; NodeId++)
            {
                if (OverAllNodeStatus == 1)
                {
                    NodesList.Add(Node.Nodes[NodeId].FullPath + "*", true);
                }
                else
                {
                    BuildCheckedNodesList(NodesList, Node.Nodes[NodeId]);
                }
            }
        }

        private void LoadExcludedFolderPatterns()
        {
            ExcludedFolderPatterns = new List<FileNamePattern>();
            FileNamePattern.LoadPatternsList(ExcludedFolderPatterns, ExcludedTypesTextBox.Text, true, ProgramSetting.ExcludedFolderPrefix);
            FileNamePattern.LoadPatternsList(ExcludedFolderPatterns, Handler.GetSetting(ProfileSetting.ExcludedFolders, ""), true, "");// 'Not shown in the UI.
        }




        private int OverAllCheckStatus(TreeNode Node)
        {
            if (!Node.TreeView.CheckBoxes) return 1;
            if (Node.Nodes.Count == 0) return Node.Checked ? 1 : 0;

            bool AllChecked = Node.Checked;
            bool AllClear = !Node.Checked;
            foreach (TreeNode SubNode in Node.Nodes)
            {
                int CurrentStatus = OverAllCheckStatus(SubNode);
                AllChecked = AllChecked && (CurrentStatus == 1);
                AllClear = AllClear && (CurrentStatus == 0);
            }

            if (AllChecked) return 1;
            if (AllClear) return 0;
            return -1;
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
    
