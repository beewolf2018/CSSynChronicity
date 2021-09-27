namespace CSSynChronicity.Interface
{
    partial class MainForm
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
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("\\ACTIONS", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup4 = new System.Windows.Forms.ListViewGroup("\\PROFILES", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "\\NEW_PROFILE_LABEL",
            "\\NEW_PROFILE"}, 3);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.Actions = new System.Windows.Forms.ListView();
            this.NameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MethodsColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LastRunColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SyncIcons = new System.Windows.Forms.ImageList(this.components);
            this.InfoPanel = new System.Windows.Forms.Panel();
            this.Destination = new System.Windows.Forms.Label();
            this.DestinationLabel = new System.Windows.Forms.Label();
            this.Source = new System.Windows.Forms.Label();
            this.SourceLabel = new System.Windows.Forms.Label();
            this.TimeOffset = new System.Windows.Forms.Label();
            this.TimeOffsetLabel = new System.Windows.Forms.Label();
            this.Scheduling = new System.Windows.Forms.Label();
            this.SchedulingLabel = new System.Windows.Forms.Label();
            this.FileTypes = new System.Windows.Forms.Label();
            this.FileTypesLabel = new System.Windows.Forms.Label();
            this.Method = new System.Windows.Forms.Label();
            this.MethodLabel = new System.Windows.Forms.Label();
            this.LimitedCopy = new System.Windows.Forms.Label();
            this.LimitedCopyLabel = new System.Windows.Forms.Label();
            this.ProfileName = new System.Windows.Forms.Label();
            this.NameLabel = new System.Windows.Forms.Label();
            this.TipsLabel = new System.Windows.Forms.Label();
            this.ActionsMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.PreviewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SynchronizeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ChangeSettingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.DeleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RenameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewLogMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ScheduleMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ClearLogMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ApplicationTimer = new System.Windows.Forms.Timer(this.components);
            this.StatusIconMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripHeader = new System.Windows.Forms.ToolStripMenuItem();
            this.languageSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.csNotifyicon = new System.Windows.Forms.NotifyIcon(this.components);
            this.InfoPanel.SuspendLayout();
            this.ActionsMenu.SuspendLayout();
            this.StatusIconMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // Actions
            // 
            this.Actions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NameColumn,
            this.MethodsColumn,
            this.LastRunColumn});
            this.Actions.Dock = System.Windows.Forms.DockStyle.Fill;
            listViewGroup3.Header = "\\ACTIONS";
            listViewGroup3.Name = "Actions";
            listViewGroup4.Header = "\\PROFILES";
            listViewGroup4.Name = "Profiles";
            this.Actions.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup3,
            listViewGroup4});
            this.Actions.HideSelection = false;
            listViewItem2.Group = listViewGroup3;
            this.Actions.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem2});
            this.Actions.LargeImageList = this.SyncIcons;
            this.Actions.Location = new System.Drawing.Point(0, 0);
            this.Actions.MultiSelect = false;
            this.Actions.Name = "Actions";
            this.Actions.Size = new System.Drawing.Size(763, 632);
            this.Actions.TabIndex = 0;
            this.Actions.TileSize = new System.Drawing.Size(160, 40);
            this.Actions.UseCompatibleStateImageBehavior = false;
            this.Actions.View = System.Windows.Forms.View.Tile;
            this.Actions.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.Actions_AfterLabelEdit);
            this.Actions.SelectedIndexChanged += new System.EventHandler(this.Actions_SelectedIndexChanged);
            this.Actions.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Actions_MouseClick);
            // 
            // NameColumn
            // 
            this.NameColumn.Text = "\\NAME";
            // 
            // MethodsColumn
            // 
            this.MethodsColumn.Text = "\\METHOD";
            // 
            // LastRunColumn
            // 
            this.LastRunColumn.Text = "";
            // 
            // SyncIcons
            // 
            this.SyncIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("SyncIcons.ImageStream")));
            this.SyncIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.SyncIcons.Images.SetKeyName(0, "edit-redo.png");
            this.SyncIcons.Images.SetKeyName(1, "edit-redo-add.png");
            this.SyncIcons.Images.SetKeyName(2, "view-refresh-32.png");
            this.SyncIcons.Images.SetKeyName(3, "document-new.png");
            // 
            // InfoPanel
            // 
            this.InfoPanel.Controls.Add(this.Destination);
            this.InfoPanel.Controls.Add(this.DestinationLabel);
            this.InfoPanel.Controls.Add(this.Source);
            this.InfoPanel.Controls.Add(this.SourceLabel);
            this.InfoPanel.Controls.Add(this.TimeOffset);
            this.InfoPanel.Controls.Add(this.TimeOffsetLabel);
            this.InfoPanel.Controls.Add(this.Scheduling);
            this.InfoPanel.Controls.Add(this.SchedulingLabel);
            this.InfoPanel.Controls.Add(this.FileTypes);
            this.InfoPanel.Controls.Add(this.FileTypesLabel);
            this.InfoPanel.Controls.Add(this.Method);
            this.InfoPanel.Controls.Add(this.MethodLabel);
            this.InfoPanel.Controls.Add(this.LimitedCopy);
            this.InfoPanel.Controls.Add(this.LimitedCopyLabel);
            this.InfoPanel.Controls.Add(this.ProfileName);
            this.InfoPanel.Controls.Add(this.NameLabel);
            this.InfoPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.InfoPanel.Location = new System.Drawing.Point(0, 464);
            this.InfoPanel.Name = "InfoPanel";
            this.InfoPanel.Size = new System.Drawing.Size(763, 168);
            this.InfoPanel.TabIndex = 1;
            // 
            // Destination
            // 
            this.Destination.AutoSize = true;
            this.Destination.Location = new System.Drawing.Point(116, 125);
            this.Destination.Name = "Destination";
            this.Destination.Size = new System.Drawing.Size(0, 12);
            this.Destination.TabIndex = 15;
            // 
            // DestinationLabel
            // 
            this.DestinationLabel.AutoSize = true;
            this.DestinationLabel.Location = new System.Drawing.Point(40, 126);
            this.DestinationLabel.Name = "DestinationLabel";
            this.DestinationLabel.Size = new System.Drawing.Size(77, 12);
            this.DestinationLabel.TabIndex = 14;
            this.DestinationLabel.Text = "\\DESTINATION";
            // 
            // Source
            // 
            this.Source.AutoSize = true;
            this.Source.Location = new System.Drawing.Point(118, 97);
            this.Source.Name = "Source";
            this.Source.Size = new System.Drawing.Size(0, 12);
            this.Source.TabIndex = 13;
            // 
            // SourceLabel
            // 
            this.SourceLabel.AutoSize = true;
            this.SourceLabel.Location = new System.Drawing.Point(38, 98);
            this.SourceLabel.Name = "SourceLabel";
            this.SourceLabel.Size = new System.Drawing.Size(47, 12);
            this.SourceLabel.TabIndex = 12;
            this.SourceLabel.Text = "\\SOURCE";
            // 
            // TimeOffset
            // 
            this.TimeOffset.AutoSize = true;
            this.TimeOffset.Location = new System.Drawing.Point(334, 69);
            this.TimeOffset.Name = "TimeOffset";
            this.TimeOffset.Size = new System.Drawing.Size(0, 12);
            this.TimeOffset.TabIndex = 11;
            // 
            // TimeOffsetLabel
            // 
            this.TimeOffsetLabel.AutoSize = true;
            this.TimeOffsetLabel.Location = new System.Drawing.Point(228, 70);
            this.TimeOffsetLabel.Name = "TimeOffsetLabel";
            this.TimeOffsetLabel.Size = new System.Drawing.Size(77, 12);
            this.TimeOffsetLabel.TabIndex = 10;
            this.TimeOffsetLabel.Text = "\\TIME_OFFSET";
            // 
            // Scheduling
            // 
            this.Scheduling.AutoSize = true;
            this.Scheduling.Location = new System.Drawing.Point(116, 70);
            this.Scheduling.Name = "Scheduling";
            this.Scheduling.Size = new System.Drawing.Size(0, 12);
            this.Scheduling.TabIndex = 9;
            // 
            // SchedulingLabel
            // 
            this.SchedulingLabel.AutoSize = true;
            this.SchedulingLabel.Location = new System.Drawing.Point(38, 70);
            this.SchedulingLabel.Name = "SchedulingLabel";
            this.SchedulingLabel.Size = new System.Drawing.Size(65, 12);
            this.SchedulingLabel.TabIndex = 8;
            this.SchedulingLabel.Text = "\\SCH_LABEL";
            // 
            // FileTypes
            // 
            this.FileTypes.AutoSize = true;
            this.FileTypes.Location = new System.Drawing.Point(334, 41);
            this.FileTypes.Name = "FileTypes";
            this.FileTypes.Size = new System.Drawing.Size(0, 12);
            this.FileTypes.TabIndex = 7;
            // 
            // FileTypesLabel
            // 
            this.FileTypesLabel.AutoSize = true;
            this.FileTypesLabel.Location = new System.Drawing.Point(226, 41);
            this.FileTypesLabel.Name = "FileTypesLabel";
            this.FileTypesLabel.Size = new System.Drawing.Size(65, 12);
            this.FileTypesLabel.TabIndex = 6;
            this.FileTypesLabel.Text = "\\FILETYPES";
            // 
            // Method
            // 
            this.Method.AutoSize = true;
            this.Method.Location = new System.Drawing.Point(114, 41);
            this.Method.Name = "Method";
            this.Method.Size = new System.Drawing.Size(0, 12);
            this.Method.TabIndex = 5;
            // 
            // MethodLabel
            // 
            this.MethodLabel.AutoSize = true;
            this.MethodLabel.Location = new System.Drawing.Point(38, 41);
            this.MethodLabel.Name = "MethodLabel";
            this.MethodLabel.Size = new System.Drawing.Size(47, 12);
            this.MethodLabel.TabIndex = 4;
            this.MethodLabel.Text = "\\METHOD";
            // 
            // LimitedCopy
            // 
            this.LimitedCopy.AutoSize = true;
            this.LimitedCopy.Location = new System.Drawing.Point(332, 13);
            this.LimitedCopy.Name = "LimitedCopy";
            this.LimitedCopy.Size = new System.Drawing.Size(0, 12);
            this.LimitedCopy.TabIndex = 3;
            // 
            // LimitedCopyLabel
            // 
            this.LimitedCopyLabel.AutoSize = true;
            this.LimitedCopyLabel.Location = new System.Drawing.Point(224, 13);
            this.LimitedCopyLabel.Name = "LimitedCopyLabel";
            this.LimitedCopyLabel.Size = new System.Drawing.Size(83, 12);
            this.LimitedCopyLabel.TabIndex = 2;
            this.LimitedCopyLabel.Text = "\\LIMITED_COPY";
            // 
            // ProfileName
            // 
            this.ProfileName.AutoSize = true;
            this.ProfileName.Location = new System.Drawing.Point(112, 13);
            this.ProfileName.Name = "ProfileName";
            this.ProfileName.Size = new System.Drawing.Size(0, 12);
            this.ProfileName.TabIndex = 1;
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new System.Drawing.Point(36, 13);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(35, 12);
            this.NameLabel.TabIndex = 0;
            this.NameLabel.Text = "\\NAME";
            this.NameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TipsLabel
            // 
            this.TipsLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TipsLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.TipsLabel.Location = new System.Drawing.Point(0, 431);
            this.TipsLabel.Name = "TipsLabel";
            this.TipsLabel.Size = new System.Drawing.Size(763, 33);
            this.TipsLabel.TabIndex = 4;
            this.TipsLabel.Text = "\\NO_PROFILES";
            this.TipsLabel.Visible = false;
            // 
            // ActionsMenu
            // 
            this.ActionsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PreviewMenuItem,
            this.SynchronizeMenuItem,
            this.ChangeSettingsMenuItem,
            this.toolStripSeparator1,
            this.DeleteToolStripMenuItem,
            this.RenameMenuItem,
            this.ViewLogMenuItem,
            this.toolStripSeparator2,
            this.ScheduleMenuItem,
            this.ClearLogMenuItem});
            this.ActionsMenu.Name = "ActionsMenu";
            this.ActionsMenu.Size = new System.Drawing.Size(196, 192);
            // 
            // PreviewMenuItem
            // 
            this.PreviewMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("PreviewMenuItem.Image")));
            this.PreviewMenuItem.Name = "PreviewMenuItem";
            this.PreviewMenuItem.Size = new System.Drawing.Size(195, 22);
            this.PreviewMenuItem.Text = "\\PREVIEW";
            // 
            // SynchronizeMenuItem
            // 
            this.SynchronizeMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("SynchronizeMenuItem.Image")));
            this.SynchronizeMenuItem.Name = "SynchronizeMenuItem";
            this.SynchronizeMenuItem.Size = new System.Drawing.Size(195, 22);
            this.SynchronizeMenuItem.Text = "\\SYNC";
            this.SynchronizeMenuItem.Click += new System.EventHandler(this.SynchronizeMenuItem_Click);
            // 
            // ChangeSettingsMenuItem
            // 
            this.ChangeSettingsMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("ChangeSettingsMenuItem.Image")));
            this.ChangeSettingsMenuItem.Name = "ChangeSettingsMenuItem";
            this.ChangeSettingsMenuItem.Size = new System.Drawing.Size(195, 22);
            this.ChangeSettingsMenuItem.Text = "\\CHANGE_SETTINGS";
            this.ChangeSettingsMenuItem.Click += new System.EventHandler(this.ChangeSettingsMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(192, 6);
            // 
            // DeleteToolStripMenuItem
            // 
            this.DeleteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("DeleteToolStripMenuItem.Image")));
            this.DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem";
            this.DeleteToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.DeleteToolStripMenuItem.Text = "\\DELETE";
            this.DeleteToolStripMenuItem.Click += new System.EventHandler(this.DeleteToolStripMenuItem_Click);
            // 
            // RenameMenuItem
            // 
            this.RenameMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("RenameMenuItem.Image")));
            this.RenameMenuItem.Name = "RenameMenuItem";
            this.RenameMenuItem.Size = new System.Drawing.Size(195, 22);
            this.RenameMenuItem.Text = "\\RENAME";
            this.RenameMenuItem.Click += new System.EventHandler(this.RenameMenuItem_Click);
            // 
            // ViewLogMenuItem
            // 
            this.ViewLogMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("ViewLogMenuItem.Image")));
            this.ViewLogMenuItem.Name = "ViewLogMenuItem";
            this.ViewLogMenuItem.Size = new System.Drawing.Size(195, 22);
            this.ViewLogMenuItem.Text = "\\VIEW_LOG";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(192, 6);
            // 
            // ScheduleMenuItem
            // 
            this.ScheduleMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("ScheduleMenuItem.Image")));
            this.ScheduleMenuItem.Name = "ScheduleMenuItem";
            this.ScheduleMenuItem.Size = new System.Drawing.Size(195, 22);
            this.ScheduleMenuItem.Text = "\\SCHEDULING";
            this.ScheduleMenuItem.Click += new System.EventHandler(this.ScheduleMenuItem_Click);
            // 
            // ClearLogMenuItem
            // 
            this.ClearLogMenuItem.Name = "ClearLogMenuItem";
            this.ClearLogMenuItem.Size = new System.Drawing.Size(195, 22);
            this.ClearLogMenuItem.Text = " ";
            // 
            // ApplicationTimer
            // 
            this.ApplicationTimer.Interval = 60000;
            this.ApplicationTimer.Tick += new System.EventHandler(this.ApplicationTimer_Tick);
            // 
            // StatusIconMenu
            // 
            this.StatusIconMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripHeader,
            this.languageSelectionToolStripMenuItem,
            this.ExitToolStripMenuItem});
            this.StatusIconMenu.Name = "StatusIconMenu";
            this.StatusIconMenu.Size = new System.Drawing.Size(190, 70);
            // 
            // ToolStripHeader
            // 
            this.ToolStripHeader.Name = "ToolStripHeader";
            this.ToolStripHeader.Size = new System.Drawing.Size(189, 22);
            this.ToolStripHeader.Text = "File Synchronicity";
            this.ToolStripHeader.Click += new System.EventHandler(this.ToolStripHeader_Click);
            // 
            // languageSelectionToolStripMenuItem
            // 
            this.languageSelectionToolStripMenuItem.Name = "languageSelectionToolStripMenuItem";
            this.languageSelectionToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.languageSelectionToolStripMenuItem.Text = "Language Selection";
            this.languageSelectionToolStripMenuItem.Click += new System.EventHandler(this.languageSelectionToolStripMenuItem_Click);
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.ExitToolStripMenuItem.Text = "Exit";
            this.ExitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // csNotifyicon
            // 
            this.csNotifyicon.ContextMenuStrip = this.StatusIconMenu;
            this.csNotifyicon.Icon = ((System.Drawing.Icon)(resources.GetObject("csNotifyicon.Icon")));
            this.csNotifyicon.Text = "File Synchronicity";
            this.csNotifyicon.Visible = true;
            this.csNotifyicon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.csNotifyicon_MouseDoubleClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(763, 632);
            this.Controls.Add(this.TipsLabel);
            this.Controls.Add(this.InfoPanel);
            this.Controls.Add(this.Actions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.Text = "File Synchronicity";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.InfoPanel.ResumeLayout(false);
            this.InfoPanel.PerformLayout();
            this.ActionsMenu.ResumeLayout(false);
            this.StatusIconMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ListView Actions;
        public System.Windows.Forms.Panel InfoPanel;
        public System.Windows.Forms.Label Destination;
        public System.Windows.Forms.Label DestinationLabel;
        public System.Windows.Forms.Label Source;
        public System.Windows.Forms.Label SourceLabel;
        public System.Windows.Forms.Label TimeOffset;
        public System.Windows.Forms.Label TimeOffsetLabel;
        public System.Windows.Forms.Label Scheduling;
        public System.Windows.Forms.Label FileTypes;
        public System.Windows.Forms.Label FileTypesLabel;
        public System.Windows.Forms.Label Method;
        public System.Windows.Forms.Label MethodLabel;
        public System.Windows.Forms.Label LimitedCopy;
        public System.Windows.Forms.Label LimitedCopyLabel;
        public System.Windows.Forms.Label ProfileName;
        public System.Windows.Forms.Label NameLabel;
        public System.Windows.Forms.Label TipsLabel;
        public System.Windows.Forms.ContextMenuStrip ActionsMenu;
        public System.Windows.Forms.Timer ApplicationTimer;
        public System.Windows.Forms.ContextMenuStrip StatusIconMenu;
        public System.Windows.Forms.ToolStripMenuItem PreviewMenuItem;
        public System.Windows.Forms.ToolStripMenuItem SynchronizeMenuItem;
        public System.Windows.Forms.ToolStripMenuItem ChangeSettingsMenuItem;
        public System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        public System.Windows.Forms.ToolStripMenuItem DeleteToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem RenameMenuItem;
        public System.Windows.Forms.ToolStripMenuItem ViewLogMenuItem;
        public System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        public System.Windows.Forms.ToolStripMenuItem ScheduleMenuItem;
        public System.Windows.Forms.ColumnHeader NameColumn;
        public System.Windows.Forms.ColumnHeader MethodsColumn;
        public System.Windows.Forms.ColumnHeader LastRunColumn;
        public System.Windows.Forms.ImageList SyncIcons;
        public System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem ClearLogMenuItem;
        public System.Windows.Forms.ToolStripMenuItem ToolStripHeader;
        public System.Windows.Forms.Label SchedulingLabel;
        public System.Windows.Forms.ToolStripMenuItem languageSelectionToolStripMenuItem;
        public System.Windows.Forms.NotifyIcon csNotifyicon;
    }
}