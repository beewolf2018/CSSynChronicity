namespace CSSynChronicity.Interface
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.SynchronizationMethodBox = new System.Windows.Forms.GroupBox();
            this.StrictMirrorOption = new System.Windows.Forms.CheckBox();
            this.MethodLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.TwoWaysIncrementalMethodOption = new System.Windows.Forms.RadioButton();
            this.LRIncrementalMethodOption = new System.Windows.Forms.RadioButton();
            this.LRMirrorMethodOption = new System.Windows.Forms.RadioButton();
            this.IncludeExcludeBox = new System.Windows.Forms.GroupBox();
            this.CopyAllFilesCheckBox = new System.Windows.Forms.CheckBox();
            this.IncludeExcludeLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.IncludedTypesTextBox = new System.Windows.Forms.TextBox();
            this.ExcludedTypesTextBox = new System.Windows.Forms.TextBox();
            this.IncludeFilesOption = new System.Windows.Forms.RadioButton();
            this.ExcludeFilesOption = new System.Windows.Forms.RadioButton();
            this.ReplicateEmptyDirectoriesOption = new System.Windows.Forms.CheckBox();
            this.AdvancedBox = new System.Windows.Forms.GroupBox();
            this.TableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.GroupNameBox = new System.Windows.Forms.ComboBox();
            this.GroupLabel = new System.Windows.Forms.Label();
            this.StrictDateComparisonOption = new System.Windows.Forms.CheckBox();
            this.TimeOffsetLabel = new System.Windows.Forms.Label();
            this.MoreLabel = new System.Windows.Forms.LinkLabel();
            this.TimeOffset = new System.Windows.Forms.NumericUpDown();
            this.TimeOffsetHoursLabel = new System.Windows.Forms.Label();
            this.ActionsPanel = new System.Windows.Forms.TableLayoutPanel();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.ViewsBox = new System.Windows.Forms.GroupBox();
            this.ViewsLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ReloadButton = new System.Windows.Forms.Button();
            this.LeftViewLabel = new System.Windows.Forms.Label();
            this.RightViewLabel = new System.Windows.Forms.Label();
            this.Loading = new System.Windows.Forms.Label();
            this.HelpLink = new System.Windows.Forms.Label();
            this.LeftViewPanel = new System.Windows.Forms.Panel();
            this.LeftReloadButton = new System.Windows.Forms.Button();
            this.LeftView = new System.Windows.Forms.TreeView();
            this.RightViewPanel = new System.Windows.Forms.Panel();
            this.RightReloadButton = new System.Windows.Forms.Button();
            this.RightView = new System.Windows.Forms.TreeView();
            this.DirectoriesBox = new System.Windows.Forms.GroupBox();
            this.SwapButton = new System.Windows.Forms.Button();
            this.BrowseRButton = new System.Windows.Forms.Button();
            this.BrowseLButton = new System.Windows.Forms.Button();
            this.ToTextBox = new System.Windows.Forms.TextBox();
            this.FromTextBox = new System.Windows.Forms.TextBox();
            this.ToLabel = new System.Windows.Forms.Label();
            this.FromLabel = new System.Windows.Forms.Label();
            this.FolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.TreeViewMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SynchronizeFolderAndSubfoldersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SynchronizeFilesOnlyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SynchronizeSubFoldersOnlyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DontSynchronizeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToggleMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExpertMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CreateDestOption = new System.Windows.Forms.ToolStripMenuItem();
            this.CheckFileSizeOption = new System.Windows.Forms.ToolStripMenuItem();
            this.ChecksumOption = new System.Windows.Forms.ToolStripMenuItem();
            this.PropagateUpdatesOption = new System.Windows.Forms.ToolStripMenuItem();
            this.SynchronizationMethodBox.SuspendLayout();
            this.MethodLayoutPanel.SuspendLayout();
            this.IncludeExcludeBox.SuspendLayout();
            this.IncludeExcludeLayoutPanel.SuspendLayout();
            this.AdvancedBox.SuspendLayout();
            this.TableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TimeOffset)).BeginInit();
            this.ActionsPanel.SuspendLayout();
            this.ViewsBox.SuspendLayout();
            this.ViewsLayoutPanel.SuspendLayout();
            this.LeftViewPanel.SuspendLayout();
            this.RightViewPanel.SuspendLayout();
            this.DirectoriesBox.SuspendLayout();
            this.TreeViewMenuStrip.SuspendLayout();
            this.ExpertMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // SynchronizationMethodBox
            // 
            this.SynchronizationMethodBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SynchronizationMethodBox.Controls.Add(this.StrictMirrorOption);
            this.SynchronizationMethodBox.Controls.Add(this.MethodLayoutPanel);
            this.SynchronizationMethodBox.Location = new System.Drawing.Point(13, 363);
            this.SynchronizationMethodBox.Name = "SynchronizationMethodBox";
            this.SynchronizationMethodBox.Size = new System.Drawing.Size(909, 75);
            this.SynchronizationMethodBox.TabIndex = 3;
            this.SynchronizationMethodBox.TabStop = false;
            this.SynchronizationMethodBox.Text = "\\SYNC_METHOD";
            // 
            // StrictMirrorOption
            // 
            this.StrictMirrorOption.AutoSize = true;
            this.StrictMirrorOption.Location = new System.Drawing.Point(6, 46);
            this.StrictMirrorOption.Name = "StrictMirrorOption";
            this.StrictMirrorOption.Size = new System.Drawing.Size(138, 16);
            this.StrictMirrorOption.TabIndex = 2;
            this.StrictMirrorOption.Text = "\\STRICT_MIRROR_DESC";
            this.StrictMirrorOption.UseVisualStyleBackColor = true;
            // 
            // MethodLayoutPanel
            // 
            this.MethodLayoutPanel.ColumnCount = 3;
            this.MethodLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.MethodLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.MethodLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.MethodLayoutPanel.Controls.Add(this.TwoWaysIncrementalMethodOption, 2, 0);
            this.MethodLayoutPanel.Controls.Add(this.LRIncrementalMethodOption, 1, 0);
            this.MethodLayoutPanel.Controls.Add(this.LRMirrorMethodOption, 0, 0);
            this.MethodLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.MethodLayoutPanel.Location = new System.Drawing.Point(3, 17);
            this.MethodLayoutPanel.Name = "MethodLayoutPanel";
            this.MethodLayoutPanel.RowCount = 1;
            this.MethodLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.MethodLayoutPanel.Size = new System.Drawing.Size(903, 23);
            this.MethodLayoutPanel.TabIndex = 0;
            // 
            // TwoWaysIncrementalMethodOption
            // 
            this.TwoWaysIncrementalMethodOption.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.TwoWaysIncrementalMethodOption.AutoSize = true;
            this.TwoWaysIncrementalMethodOption.Location = new System.Drawing.Point(680, 3);
            this.TwoWaysIncrementalMethodOption.Name = "TwoWaysIncrementalMethodOption";
            this.TwoWaysIncrementalMethodOption.Size = new System.Drawing.Size(143, 16);
            this.TwoWaysIncrementalMethodOption.TabIndex = 2;
            this.TwoWaysIncrementalMethodOption.Tag = "\\TWOWAYS_INCREMENTAL_TAG";
            this.TwoWaysIncrementalMethodOption.Text = "\\TWOWAYS_INCREMENTAL";
            this.TwoWaysIncrementalMethodOption.UseVisualStyleBackColor = true;
            // 
            // LRIncrementalMethodOption
            // 
            this.LRIncrementalMethodOption.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LRIncrementalMethodOption.AutoSize = true;
            this.LRIncrementalMethodOption.Location = new System.Drawing.Point(394, 3);
            this.LRIncrementalMethodOption.Name = "LRIncrementalMethodOption";
            this.LRIncrementalMethodOption.Size = new System.Drawing.Size(113, 16);
            this.LRIncrementalMethodOption.TabIndex = 1;
            this.LRIncrementalMethodOption.Tag = "\\LR_INCREMENTAL_TAG";
            this.LRIncrementalMethodOption.Text = "\\LR_INCREMENTAL";
            this.LRIncrementalMethodOption.UseVisualStyleBackColor = true;
            // 
            // LRMirrorMethodOption
            // 
            this.LRMirrorMethodOption.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LRMirrorMethodOption.AutoSize = true;
            this.LRMirrorMethodOption.Checked = true;
            this.LRMirrorMethodOption.Location = new System.Drawing.Point(108, 3);
            this.LRMirrorMethodOption.Name = "LRMirrorMethodOption";
            this.LRMirrorMethodOption.Size = new System.Drawing.Size(83, 16);
            this.LRMirrorMethodOption.TabIndex = 0;
            this.LRMirrorMethodOption.TabStop = true;
            this.LRMirrorMethodOption.Tag = "\\LR_MIRROR_TAG";
            this.LRMirrorMethodOption.Text = "\\LR_MIRROR";
            this.LRMirrorMethodOption.UseVisualStyleBackColor = true;
            // 
            // IncludeExcludeBox
            // 
            this.IncludeExcludeBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.IncludeExcludeBox.Controls.Add(this.CopyAllFilesCheckBox);
            this.IncludeExcludeBox.Controls.Add(this.IncludeExcludeLayoutPanel);
            this.IncludeExcludeBox.Controls.Add(this.ReplicateEmptyDirectoriesOption);
            this.IncludeExcludeBox.Location = new System.Drawing.Point(12, 444);
            this.IncludeExcludeBox.Name = "IncludeExcludeBox";
            this.IncludeExcludeBox.Size = new System.Drawing.Size(910, 94);
            this.IncludeExcludeBox.TabIndex = 4;
            this.IncludeExcludeBox.TabStop = false;
            this.IncludeExcludeBox.Text = "\\INCLUDE_EXCLUDE";
            // 
            // CopyAllFilesCheckBox
            // 
            this.CopyAllFilesCheckBox.AutoSize = true;
            this.CopyAllFilesCheckBox.Checked = true;
            this.CopyAllFilesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CopyAllFilesCheckBox.Location = new System.Drawing.Point(6, 20);
            this.CopyAllFilesCheckBox.Name = "CopyAllFilesCheckBox";
            this.CopyAllFilesCheckBox.Size = new System.Drawing.Size(84, 16);
            this.CopyAllFilesCheckBox.TabIndex = 0;
            this.CopyAllFilesCheckBox.Text = "\\ALL_FILES";
            this.CopyAllFilesCheckBox.UseVisualStyleBackColor = true;
            // 
            // IncludeExcludeLayoutPanel
            // 
            this.IncludeExcludeLayoutPanel.ColumnCount = 2;
            this.IncludeExcludeLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.IncludeExcludeLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.IncludeExcludeLayoutPanel.Controls.Add(this.IncludedTypesTextBox, 0, 1);
            this.IncludeExcludeLayoutPanel.Controls.Add(this.ExcludedTypesTextBox, 0, 1);
            this.IncludeExcludeLayoutPanel.Controls.Add(this.IncludeFilesOption, 0, 0);
            this.IncludeExcludeLayoutPanel.Controls.Add(this.ExcludeFilesOption, 1, 0);
            this.IncludeExcludeLayoutPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.IncludeExcludeLayoutPanel.Enabled = false;
            this.IncludeExcludeLayoutPanel.Location = new System.Drawing.Point(3, 40);
            this.IncludeExcludeLayoutPanel.Name = "IncludeExcludeLayoutPanel";
            this.IncludeExcludeLayoutPanel.RowCount = 3;
            this.IncludeExcludeLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.IncludeExcludeLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.IncludeExcludeLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.IncludeExcludeLayoutPanel.Size = new System.Drawing.Size(904, 51);
            this.IncludeExcludeLayoutPanel.TabIndex = 2;
            // 
            // IncludedTypesTextBox
            // 
            this.IncludedTypesTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.IncludedTypesTextBox.Location = new System.Drawing.Point(3, 25);
            this.IncludedTypesTextBox.Name = "IncludedTypesTextBox";
            this.IncludedTypesTextBox.Size = new System.Drawing.Size(446, 21);
            this.IncludedTypesTextBox.TabIndex = 1;
            this.IncludedTypesTextBox.Tag = "\\FILEEXT_TIPS";
            // 
            // ExcludedTypesTextBox
            // 
            this.ExcludedTypesTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExcludedTypesTextBox.Location = new System.Drawing.Point(455, 25);
            this.ExcludedTypesTextBox.Name = "ExcludedTypesTextBox";
            this.ExcludedTypesTextBox.Size = new System.Drawing.Size(446, 21);
            this.ExcludedTypesTextBox.TabIndex = 3;
            this.ExcludedTypesTextBox.Tag = "\\FILEEXT_TIPS";
            // 
            // IncludeFilesOption
            // 
            this.IncludeFilesOption.AutoSize = true;
            this.IncludeFilesOption.Location = new System.Drawing.Point(3, 3);
            this.IncludeFilesOption.Name = "IncludeFilesOption";
            this.IncludeFilesOption.Size = new System.Drawing.Size(89, 16);
            this.IncludeFilesOption.TabIndex = 0;
            this.IncludeFilesOption.TabStop = true;
            this.IncludeFilesOption.Text = "\\THESE_ONLY";
            this.IncludeFilesOption.UseVisualStyleBackColor = true;
            // 
            // ExcludeFilesOption
            // 
            this.ExcludeFilesOption.AutoSize = true;
            this.ExcludeFilesOption.Location = new System.Drawing.Point(455, 3);
            this.ExcludeFilesOption.Name = "ExcludeFilesOption";
            this.ExcludeFilesOption.Size = new System.Drawing.Size(107, 16);
            this.ExcludeFilesOption.TabIndex = 2;
            this.ExcludeFilesOption.TabStop = true;
            this.ExcludeFilesOption.Text = "\\EXCLUDE_THESE";
            this.ExcludeFilesOption.UseVisualStyleBackColor = true;
            // 
            // ReplicateEmptyDirectoriesOption
            // 
            this.ReplicateEmptyDirectoriesOption.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ReplicateEmptyDirectoriesOption.AutoSize = true;
            this.ReplicateEmptyDirectoriesOption.Checked = true;
            this.ReplicateEmptyDirectoriesOption.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ReplicateEmptyDirectoriesOption.Location = new System.Drawing.Point(787, 20);
            this.ReplicateEmptyDirectoriesOption.Name = "ReplicateEmptyDirectoriesOption";
            this.ReplicateEmptyDirectoriesOption.Size = new System.Drawing.Size(120, 16);
            this.ReplicateEmptyDirectoriesOption.TabIndex = 1;
            this.ReplicateEmptyDirectoriesOption.Text = "\\REPLICATE_EMPTY";
            this.ReplicateEmptyDirectoriesOption.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ReplicateEmptyDirectoriesOption.UseVisualStyleBackColor = true;
            // 
            // AdvancedBox
            // 
            this.AdvancedBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AdvancedBox.Controls.Add(this.TableLayoutPanel1);
            this.AdvancedBox.Controls.Add(this.TimeOffsetLabel);
            this.AdvancedBox.Controls.Add(this.MoreLabel);
            this.AdvancedBox.Controls.Add(this.TimeOffset);
            this.AdvancedBox.Controls.Add(this.TimeOffsetHoursLabel);
            this.AdvancedBox.Location = new System.Drawing.Point(16, 544);
            this.AdvancedBox.Name = "AdvancedBox";
            this.AdvancedBox.Size = new System.Drawing.Size(906, 88);
            this.AdvancedBox.TabIndex = 5;
            this.AdvancedBox.TabStop = false;
            this.AdvancedBox.Text = "\\ADVANCED_OPTS";
            // 
            // TableLayoutPanel1
            // 
            this.TableLayoutPanel1.ColumnCount = 4;
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TableLayoutPanel1.Controls.Add(this.GroupNameBox, 1, 0);
            this.TableLayoutPanel1.Controls.Add(this.GroupLabel, 0, 0);
            this.TableLayoutPanel1.Controls.Add(this.StrictDateComparisonOption, 3, 0);
            this.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.TableLayoutPanel1.Location = new System.Drawing.Point(3, 17);
            this.TableLayoutPanel1.Name = "TableLayoutPanel1";
            this.TableLayoutPanel1.RowCount = 1;
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel1.Size = new System.Drawing.Size(900, 26);
            this.TableLayoutPanel1.TabIndex = 10;
            // 
            // GroupNameBox
            // 
            this.GroupNameBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupNameBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.GroupNameBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.GroupNameBox.FormattingEnabled = true;
            this.GroupNameBox.Location = new System.Drawing.Point(50, 3);
            this.GroupNameBox.Name = "GroupNameBox";
            this.GroupNameBox.Size = new System.Drawing.Size(566, 20);
            this.GroupNameBox.TabIndex = 9;
            // 
            // GroupLabel
            // 
            this.GroupLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.GroupLabel.AutoSize = true;
            this.GroupLabel.Location = new System.Drawing.Point(3, 0);
            this.GroupLabel.Name = "GroupLabel";
            this.GroupLabel.Size = new System.Drawing.Size(41, 26);
            this.GroupLabel.TabIndex = 8;
            this.GroupLabel.Text = "\\GROUP";
            this.GroupLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // StrictDateComparisonOption
            // 
            this.StrictDateComparisonOption.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.StrictDateComparisonOption.AutoSize = true;
            this.StrictDateComparisonOption.Checked = true;
            this.StrictDateComparisonOption.CheckState = System.Windows.Forms.CheckState.Checked;
            this.StrictDateComparisonOption.Location = new System.Drawing.Point(765, 3);
            this.StrictDateComparisonOption.Name = "StrictDateComparisonOption";
            this.StrictDateComparisonOption.Size = new System.Drawing.Size(132, 20);
            this.StrictDateComparisonOption.TabIndex = 2;
            this.StrictDateComparisonOption.Tag = "\\STRICTCOMPARISON_TAG";
            this.StrictDateComparisonOption.Text = "\\STRICT_COMPARISON";
            this.StrictDateComparisonOption.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.StrictDateComparisonOption.UseVisualStyleBackColor = true;
            // 
            // TimeOffsetLabel
            // 
            this.TimeOffsetLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TimeOffsetLabel.Location = new System.Drawing.Point(660, 48);
            this.TimeOffsetLabel.Name = "TimeOffsetLabel";
            this.TimeOffsetLabel.Size = new System.Drawing.Size(143, 13);
            this.TimeOffsetLabel.TabIndex = 3;
            this.TimeOffsetLabel.Text = "\\TIME_OFFSET";
            this.TimeOffsetLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MoreLabel
            // 
            this.MoreLabel.AutoSize = true;
            this.MoreLabel.Location = new System.Drawing.Point(6, 48);
            this.MoreLabel.Name = "MoreLabel";
            this.MoreLabel.Size = new System.Drawing.Size(35, 12);
            this.MoreLabel.TabIndex = 7;
            this.MoreLabel.TabStop = true;
            this.MoreLabel.Text = "\\MORE";
            // 
            // TimeOffset
            // 
            this.TimeOffset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TimeOffset.Location = new System.Drawing.Point(806, 46);
            this.TimeOffset.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.TimeOffset.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            -2147483648});
            this.TimeOffset.Name = "TimeOffset";
            this.TimeOffset.Size = new System.Drawing.Size(35, 21);
            this.TimeOffset.TabIndex = 4;
            // 
            // TimeOffsetHoursLabel
            // 
            this.TimeOffsetHoursLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TimeOffsetHoursLabel.AutoSize = true;
            this.TimeOffsetHoursLabel.Location = new System.Drawing.Point(847, 48);
            this.TimeOffsetHoursLabel.Name = "TimeOffsetHoursLabel";
            this.TimeOffsetHoursLabel.Size = new System.Drawing.Size(41, 12);
            this.TimeOffsetHoursLabel.TabIndex = 5;
            this.TimeOffsetHoursLabel.Text = "\\HOURS";
            // 
            // ActionsPanel
            // 
            this.ActionsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ActionsPanel.ColumnCount = 2;
            this.ActionsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ActionsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ActionsPanel.Controls.Add(this.CancelBtn, 1, 0);
            this.ActionsPanel.Controls.Add(this.SaveButton, 0, 0);
            this.ActionsPanel.Location = new System.Drawing.Point(722, 652);
            this.ActionsPanel.Name = "ActionsPanel";
            this.ActionsPanel.RowCount = 1;
            this.ActionsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ActionsPanel.Size = new System.Drawing.Size(200, 31);
            this.ActionsPanel.TabIndex = 7;
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(103, 3);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(94, 25);
            this.CancelBtn.TabIndex = 1;
            this.CancelBtn.Text = "\\CANCEL";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveButton.Location = new System.Drawing.Point(3, 3);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(94, 25);
            this.SaveButton.TabIndex = 0;
            this.SaveButton.Text = "\\SAVE";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // ViewsBox
            // 
            this.ViewsBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ViewsBox.Controls.Add(this.ViewsLayoutPanel);
            this.ViewsBox.Location = new System.Drawing.Point(12, 119);
            this.ViewsBox.Name = "ViewsBox";
            this.ViewsBox.Size = new System.Drawing.Size(910, 238);
            this.ViewsBox.TabIndex = 8;
            this.ViewsBox.TabStop = false;
            this.ViewsBox.Text = "\\SUBDIRECTORIES";
            // 
            // ViewsLayoutPanel
            // 
            this.ViewsLayoutPanel.ColumnCount = 3;
            this.ViewsLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ViewsLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.ViewsLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ViewsLayoutPanel.Controls.Add(this.ReloadButton, 1, 1);
            this.ViewsLayoutPanel.Controls.Add(this.LeftViewLabel, 0, 0);
            this.ViewsLayoutPanel.Controls.Add(this.RightViewLabel, 2, 0);
            this.ViewsLayoutPanel.Controls.Add(this.Loading, 1, 2);
            this.ViewsLayoutPanel.Controls.Add(this.HelpLink, 1, 3);
            this.ViewsLayoutPanel.Controls.Add(this.LeftViewPanel, 0, 1);
            this.ViewsLayoutPanel.Controls.Add(this.RightViewPanel, 2, 1);
            this.ViewsLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ViewsLayoutPanel.Location = new System.Drawing.Point(3, 17);
            this.ViewsLayoutPanel.Name = "ViewsLayoutPanel";
            this.ViewsLayoutPanel.RowCount = 4;
            this.ViewsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.ViewsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.ViewsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.ViewsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ViewsLayoutPanel.Size = new System.Drawing.Size(904, 218);
            this.ViewsLayoutPanel.TabIndex = 0;
            // 
            // ReloadButton
            // 
            this.ReloadButton.Image = ((System.Drawing.Image)(resources.GetObject("ReloadButton.Image")));
            this.ReloadButton.Location = new System.Drawing.Point(437, 15);
            this.ReloadButton.Name = "ReloadButton";
            this.ReloadButton.Size = new System.Drawing.Size(29, 29);
            this.ReloadButton.TabIndex = 4;
            this.ReloadButton.UseVisualStyleBackColor = false;
            // 
            // LeftViewLabel
            // 
            this.LeftViewLabel.AutoSize = true;
            this.LeftViewLabel.Location = new System.Drawing.Point(3, 0);
            this.LeftViewLabel.Name = "LeftViewLabel";
            this.LeftViewLabel.Size = new System.Drawing.Size(65, 12);
            this.LeftViewLabel.TabIndex = 0;
            this.LeftViewLabel.Text = "\\LEFT_SIDE";
            // 
            // RightViewLabel
            // 
            this.RightViewLabel.AutoSize = true;
            this.RightViewLabel.Location = new System.Drawing.Point(472, 0);
            this.RightViewLabel.Name = "RightViewLabel";
            this.RightViewLabel.Size = new System.Drawing.Size(71, 12);
            this.RightViewLabel.TabIndex = 2;
            this.RightViewLabel.Text = "\\RIGHT_SIDE";
            // 
            // Loading
            // 
            this.Loading.Image = ((System.Drawing.Image)(resources.GetObject("Loading.Image")));
            this.Loading.Location = new System.Drawing.Point(437, 47);
            this.Loading.Name = "Loading";
            this.Loading.Size = new System.Drawing.Size(29, 29);
            this.Loading.TabIndex = 5;
            this.Loading.Visible = false;
            // 
            // HelpLink
            // 
            this.HelpLink.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.HelpLink.Image = ((System.Drawing.Image)(resources.GetObject("HelpLink.Image")));
            this.HelpLink.Location = new System.Drawing.Point(437, 190);
            this.HelpLink.Name = "HelpLink";
            this.HelpLink.Size = new System.Drawing.Size(28, 28);
            this.HelpLink.TabIndex = 6;
            // 
            // LeftViewPanel
            // 
            this.LeftViewPanel.Controls.Add(this.LeftReloadButton);
            this.LeftViewPanel.Controls.Add(this.LeftView);
            this.LeftViewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LeftViewPanel.Location = new System.Drawing.Point(3, 15);
            this.LeftViewPanel.Name = "LeftViewPanel";
            this.ViewsLayoutPanel.SetRowSpan(this.LeftViewPanel, 3);
            this.LeftViewPanel.Size = new System.Drawing.Size(428, 200);
            this.LeftViewPanel.TabIndex = 7;
            // 
            // LeftReloadButton
            // 
            this.LeftReloadButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LeftReloadButton.Location = new System.Drawing.Point(165, 64);
            this.LeftReloadButton.Name = "LeftReloadButton";
            this.LeftReloadButton.Size = new System.Drawing.Size(100, 66);
            this.LeftReloadButton.TabIndex = 7;
            this.LeftReloadButton.Text = "\\RELOAD_TREES";
            this.LeftReloadButton.UseVisualStyleBackColor = true;
            this.LeftReloadButton.Click += new System.EventHandler(this.LeftReloadButton_Click);
            // 
            // LeftView
            // 
            this.LeftView.BackColor = System.Drawing.Color.LightGray;
            this.LeftView.CheckBoxes = true;
            this.LeftView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LeftView.Enabled = false;
            this.LeftView.Location = new System.Drawing.Point(0, 0);
            this.LeftView.Name = "LeftView";
            this.LeftView.Size = new System.Drawing.Size(428, 200);
            this.LeftView.TabIndex = 1;
            this.LeftView.Tag = "\\TREEVIEW_TIPS";
            // 
            // RightViewPanel
            // 
            this.RightViewPanel.Controls.Add(this.RightReloadButton);
            this.RightViewPanel.Controls.Add(this.RightView);
            this.RightViewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RightViewPanel.Location = new System.Drawing.Point(472, 15);
            this.RightViewPanel.Name = "RightViewPanel";
            this.ViewsLayoutPanel.SetRowSpan(this.RightViewPanel, 3);
            this.RightViewPanel.Size = new System.Drawing.Size(429, 200);
            this.RightViewPanel.TabIndex = 8;
            // 
            // RightReloadButton
            // 
            this.RightReloadButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.RightReloadButton.Location = new System.Drawing.Point(163, 64);
            this.RightReloadButton.Name = "RightReloadButton";
            this.RightReloadButton.Size = new System.Drawing.Size(100, 66);
            this.RightReloadButton.TabIndex = 8;
            this.RightReloadButton.Text = "\\RELOAD_TREES";
            this.RightReloadButton.UseVisualStyleBackColor = true;
            // 
            // RightView
            // 
            this.RightView.BackColor = System.Drawing.Color.LightGray;
            this.RightView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RightView.Enabled = false;
            this.RightView.Location = new System.Drawing.Point(0, 0);
            this.RightView.Name = "RightView";
            this.RightView.Size = new System.Drawing.Size(429, 200);
            this.RightView.TabIndex = 3;
            this.RightView.Tag = "\\TREEVIEW_TIPS";
            // 
            // DirectoriesBox
            // 
            this.DirectoriesBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DirectoriesBox.Controls.Add(this.SwapButton);
            this.DirectoriesBox.Controls.Add(this.BrowseRButton);
            this.DirectoriesBox.Controls.Add(this.BrowseLButton);
            this.DirectoriesBox.Controls.Add(this.ToTextBox);
            this.DirectoriesBox.Controls.Add(this.FromTextBox);
            this.DirectoriesBox.Controls.Add(this.ToLabel);
            this.DirectoriesBox.Controls.Add(this.FromLabel);
            this.DirectoriesBox.Location = new System.Drawing.Point(13, 12);
            this.DirectoriesBox.Name = "DirectoriesBox";
            this.DirectoriesBox.Size = new System.Drawing.Size(906, 101);
            this.DirectoriesBox.TabIndex = 1;
            this.DirectoriesBox.TabStop = false;
            this.DirectoriesBox.Text = "\\DIRECTORIES";
            // 
            // SwapButton
            // 
            this.SwapButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SwapButton.Location = new System.Drawing.Point(844, 19);
            this.SwapButton.Name = "SwapButton";
            this.SwapButton.Size = new System.Drawing.Size(56, 49);
            this.SwapButton.TabIndex = 6;
            this.SwapButton.Text = "\\SWAP";
            this.SwapButton.UseVisualStyleBackColor = true;
            this.SwapButton.Click += new System.EventHandler(this.SwapButton_Click);
            // 
            // BrowseRButton
            // 
            this.BrowseRButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseRButton.Location = new System.Drawing.Point(809, 44);
            this.BrowseRButton.Name = "BrowseRButton";
            this.BrowseRButton.Size = new System.Drawing.Size(29, 24);
            this.BrowseRButton.TabIndex = 5;
            this.BrowseRButton.Text = "...";
            this.BrowseRButton.UseVisualStyleBackColor = true;
            this.BrowseRButton.Click += new System.EventHandler(this.BrowseRButton_Click);
            // 
            // BrowseLButton
            // 
            this.BrowseLButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseLButton.Location = new System.Drawing.Point(809, 18);
            this.BrowseLButton.Name = "BrowseLButton";
            this.BrowseLButton.Size = new System.Drawing.Size(29, 24);
            this.BrowseLButton.TabIndex = 2;
            this.BrowseLButton.Text = "...";
            this.BrowseLButton.UseVisualStyleBackColor = true;
            this.BrowseLButton.Click += new System.EventHandler(this.BrowseLButton_Click);
            // 
            // ToTextBox
            // 
            this.ToTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ToTextBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.ToTextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.ToTextBox.Location = new System.Drawing.Point(62, 45);
            this.ToTextBox.Name = "ToTextBox";
            this.ToTextBox.Size = new System.Drawing.Size(741, 21);
            this.ToTextBox.TabIndex = 4;
            this.ToTextBox.Tag = "\\PATH_TIPS";
            // 
            // FromTextBox
            // 
            this.FromTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FromTextBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.FromTextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.FromTextBox.Location = new System.Drawing.Point(62, 19);
            this.FromTextBox.Name = "FromTextBox";
            this.FromTextBox.Size = new System.Drawing.Size(741, 21);
            this.FromTextBox.TabIndex = 1;
            this.FromTextBox.Tag = "\\PATH_TIPS";
            // 
            // ToLabel
            // 
            this.ToLabel.Location = new System.Drawing.Point(6, 44);
            this.ToLabel.Name = "ToLabel";
            this.ToLabel.Size = new System.Drawing.Size(59, 21);
            this.ToLabel.TabIndex = 3;
            this.ToLabel.Text = "\\TO";
            this.ToLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FromLabel
            // 
            this.FromLabel.Location = new System.Drawing.Point(3, 18);
            this.FromLabel.Name = "FromLabel";
            this.FromLabel.Size = new System.Drawing.Size(62, 21);
            this.FromLabel.TabIndex = 0;
            this.FromLabel.Text = "\\FROM";
            this.FromLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TreeViewMenuStrip
            // 
            this.TreeViewMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.TreeViewMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SynchronizeFolderAndSubfoldersMenuItem,
            this.SynchronizeFilesOnlyMenuItem,
            this.SynchronizeSubFoldersOnlyMenuItem,
            this.DontSynchronizeMenuItem,
            this.ToggleMenuItem});
            this.TreeViewMenuStrip.Name = "TreeViewMenuStrip";
            this.TreeViewMenuStrip.Size = new System.Drawing.Size(242, 114);
            // 
            // SynchronizeFolderAndSubfoldersMenuItem
            // 
            this.SynchronizeFolderAndSubfoldersMenuItem.Name = "SynchronizeFolderAndSubfoldersMenuItem";
            this.SynchronizeFolderAndSubfoldersMenuItem.Size = new System.Drawing.Size(241, 22);
            this.SynchronizeFolderAndSubfoldersMenuItem.Text = "\\FOLDER_AND_SUBFOLDERS";
            // 
            // SynchronizeFilesOnlyMenuItem
            // 
            this.SynchronizeFilesOnlyMenuItem.Name = "SynchronizeFilesOnlyMenuItem";
            this.SynchronizeFilesOnlyMenuItem.Size = new System.Drawing.Size(241, 22);
            this.SynchronizeFilesOnlyMenuItem.Text = "\\FILES_ONLY";
            // 
            // SynchronizeSubFoldersOnlyMenuItem
            // 
            this.SynchronizeSubFoldersOnlyMenuItem.Name = "SynchronizeSubFoldersOnlyMenuItem";
            this.SynchronizeSubFoldersOnlyMenuItem.Size = new System.Drawing.Size(241, 22);
            this.SynchronizeSubFoldersOnlyMenuItem.Text = "\\SUBFOLDERS_ONLY";
            // 
            // DontSynchronizeMenuItem
            // 
            this.DontSynchronizeMenuItem.Name = "DontSynchronizeMenuItem";
            this.DontSynchronizeMenuItem.Size = new System.Drawing.Size(241, 22);
            this.DontSynchronizeMenuItem.Text = "\\NO_SYNC";
            // 
            // ToggleMenuItem
            // 
            this.ToggleMenuItem.Name = "ToggleMenuItem";
            this.ToggleMenuItem.Size = new System.Drawing.Size(241, 22);
            this.ToggleMenuItem.Text = "\\TOGGLE";
            // 
            // ExpertMenu
            // 
            this.ExpertMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.ExpertMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CreateDestOption,
            this.CheckFileSizeOption,
            this.ChecksumOption,
            this.PropagateUpdatesOption});
            this.ExpertMenu.Name = "ExpertMenu";
            this.ExpertMenu.Size = new System.Drawing.Size(172, 92);
            // 
            // CreateDestOption
            // 
            this.CreateDestOption.Name = "CreateDestOption";
            this.CreateDestOption.Size = new System.Drawing.Size(171, 22);
            this.CreateDestOption.Text = "\\CREATE_DEST";
            // 
            // CheckFileSizeOption
            // 
            this.CheckFileSizeOption.Name = "CheckFileSizeOption";
            this.CheckFileSizeOption.Size = new System.Drawing.Size(171, 22);
            this.CheckFileSizeOption.Text = "\\COMPARE_SIZE";
            // 
            // ChecksumOption
            // 
            this.ChecksumOption.Name = "ChecksumOption";
            this.ChecksumOption.Size = new System.Drawing.Size(171, 22);
            this.ChecksumOption.Text = "\\MD5";
            // 
            // PropagateUpdatesOption
            // 
            this.PropagateUpdatesOption.Name = "PropagateUpdatesOption";
            this.PropagateUpdatesOption.Size = new System.Drawing.Size(171, 22);
            this.PropagateUpdatesOption.Text = "\\PROPAGATE";
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(939, 721);
            this.Controls.Add(this.DirectoriesBox);
            this.Controls.Add(this.ViewsBox);
            this.Controls.Add(this.ActionsPanel);
            this.Controls.Add(this.AdvancedBox);
            this.Controls.Add(this.IncludeExcludeBox);
            this.Controls.Add(this.SynchronizationMethodBox);
            this.Name = "SettingsForm";
            this.Text = "SettingsForm";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.SynchronizationMethodBox.ResumeLayout(false);
            this.SynchronizationMethodBox.PerformLayout();
            this.MethodLayoutPanel.ResumeLayout(false);
            this.MethodLayoutPanel.PerformLayout();
            this.IncludeExcludeBox.ResumeLayout(false);
            this.IncludeExcludeBox.PerformLayout();
            this.IncludeExcludeLayoutPanel.ResumeLayout(false);
            this.IncludeExcludeLayoutPanel.PerformLayout();
            this.AdvancedBox.ResumeLayout(false);
            this.AdvancedBox.PerformLayout();
            this.TableLayoutPanel1.ResumeLayout(false);
            this.TableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TimeOffset)).EndInit();
            this.ActionsPanel.ResumeLayout(false);
            this.ViewsBox.ResumeLayout(false);
            this.ViewsLayoutPanel.ResumeLayout(false);
            this.ViewsLayoutPanel.PerformLayout();
            this.LeftViewPanel.ResumeLayout(false);
            this.RightViewPanel.ResumeLayout(false);
            this.DirectoriesBox.ResumeLayout(false);
            this.DirectoriesBox.PerformLayout();
            this.TreeViewMenuStrip.ResumeLayout(false);
            this.ExpertMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        internal System.Windows.Forms.GroupBox SynchronizationMethodBox;
        internal System.Windows.Forms.CheckBox StrictMirrorOption;
        internal System.Windows.Forms.TableLayoutPanel MethodLayoutPanel;
        internal System.Windows.Forms.RadioButton TwoWaysIncrementalMethodOption;
        internal System.Windows.Forms.RadioButton LRIncrementalMethodOption;
        internal System.Windows.Forms.RadioButton LRMirrorMethodOption;
        internal System.Windows.Forms.GroupBox IncludeExcludeBox;
        internal System.Windows.Forms.CheckBox CopyAllFilesCheckBox;
        internal System.Windows.Forms.TableLayoutPanel IncludeExcludeLayoutPanel;
        internal System.Windows.Forms.TextBox IncludedTypesTextBox;
        internal System.Windows.Forms.TextBox ExcludedTypesTextBox;
        internal System.Windows.Forms.RadioButton IncludeFilesOption;
        internal System.Windows.Forms.RadioButton ExcludeFilesOption;
        internal System.Windows.Forms.CheckBox ReplicateEmptyDirectoriesOption;
        internal System.Windows.Forms.GroupBox AdvancedBox;
        internal System.Windows.Forms.TableLayoutPanel TableLayoutPanel1;
        internal System.Windows.Forms.ComboBox GroupNameBox;
        internal System.Windows.Forms.Label GroupLabel;
        internal System.Windows.Forms.CheckBox StrictDateComparisonOption;
        internal System.Windows.Forms.Label TimeOffsetLabel;
        internal System.Windows.Forms.LinkLabel MoreLabel;
        internal System.Windows.Forms.NumericUpDown TimeOffset;
        internal System.Windows.Forms.Label TimeOffsetHoursLabel;
        internal System.Windows.Forms.TableLayoutPanel ActionsPanel;
        internal System.Windows.Forms.Button CancelBtn;
        internal System.Windows.Forms.Button SaveButton;
        internal System.Windows.Forms.GroupBox ViewsBox;
        internal System.Windows.Forms.TableLayoutPanel ViewsLayoutPanel;
        internal System.Windows.Forms.Button ReloadButton;
        internal System.Windows.Forms.Label LeftViewLabel;
        internal System.Windows.Forms.Label RightViewLabel;
        internal System.Windows.Forms.Label Loading;
        internal System.Windows.Forms.Label HelpLink;
        internal System.Windows.Forms.Panel LeftViewPanel;
        internal System.Windows.Forms.Button LeftReloadButton;
        internal System.Windows.Forms.TreeView LeftView;
        internal System.Windows.Forms.Panel RightViewPanel;
        internal System.Windows.Forms.Button RightReloadButton;
        internal System.Windows.Forms.TreeView RightView;
        internal System.Windows.Forms.GroupBox DirectoriesBox;
        internal System.Windows.Forms.Button SwapButton;
        internal System.Windows.Forms.Button BrowseRButton;
        internal System.Windows.Forms.Button BrowseLButton;
        internal System.Windows.Forms.TextBox ToTextBox;
        internal System.Windows.Forms.TextBox FromTextBox;
        internal System.Windows.Forms.Label ToLabel;
        internal System.Windows.Forms.Label FromLabel;
        private System.Windows.Forms.ContextMenuStrip TreeViewMenuStrip;
        private System.Windows.Forms.ContextMenuStrip ExpertMenu;
        private System.Windows.Forms.ToolStripMenuItem CreateDestOption;
        private System.Windows.Forms.ToolStripMenuItem CheckFileSizeOption;
        private System.Windows.Forms.ToolStripMenuItem ChecksumOption;
        private System.Windows.Forms.ToolStripMenuItem PropagateUpdatesOption;
        private System.Windows.Forms.ToolStripMenuItem SynchronizeFolderAndSubfoldersMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SynchronizeFilesOnlyMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SynchronizeSubFoldersOnlyMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DontSynchronizeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToggleMenuItem;
        public System.Windows.Forms.FolderBrowserDialog FolderBrowser;
    }
}