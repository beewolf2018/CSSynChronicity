namespace CSSynChronicity.Interface
{
    partial class SchedulingForm
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
            this.WarningLabel = new System.Windows.Forms.Label();
            this.OptionsLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.Catchup = new System.Windows.Forms.CheckBox();
            this.Enable = new System.Windows.Forms.CheckBox();
            this.FrequencyLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.DailyBtn = new System.Windows.Forms.RadioButton();
            this.WeeklyBtn = new System.Windows.Forms.RadioButton();
            this.WeekDay = new System.Windows.Forms.ComboBox();
            this.MonthlyBtn = new System.Windows.Forms.RadioButton();
            this.MonthDay = new System.Windows.Forms.NumericUpDown();
            this.TimeSelectionPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.AtLabel = new System.Windows.Forms.Label();
            this.Time = new System.Windows.Forms.DateTimePicker();
            this.ActionsPanel = new System.Windows.Forms.TableLayoutPanel();
            this.Cancel = new System.Windows.Forms.Button();
            this.Save = new System.Windows.Forms.Button();
            this.OptionsLayoutPanel.SuspendLayout();
            this.FrequencyLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MonthDay)).BeginInit();
            this.TimeSelectionPanel.SuspendLayout();
            this.ActionsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // WarningLabel
            // 
            this.WarningLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.WarningLabel.BackColor = System.Drawing.Color.Orange;
            this.WarningLabel.Location = new System.Drawing.Point(12, 9);
            this.WarningLabel.Name = "WarningLabel";
            this.WarningLabel.Padding = new System.Windows.Forms.Padding(2);
            this.WarningLabel.Size = new System.Drawing.Size(745, 52);
            this.WarningLabel.TabIndex = 1;
            this.WarningLabel.Text = "\\SCHEDULE_WARNING";
            this.WarningLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // OptionsLayoutPanel
            // 
            this.OptionsLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OptionsLayoutPanel.ColumnCount = 2;
            this.OptionsLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.OptionsLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.OptionsLayoutPanel.Controls.Add(this.Catchup, 1, 0);
            this.OptionsLayoutPanel.Controls.Add(this.Enable, 0, 0);
            this.OptionsLayoutPanel.Location = new System.Drawing.Point(14, 64);
            this.OptionsLayoutPanel.Name = "OptionsLayoutPanel";
            this.OptionsLayoutPanel.RowCount = 1;
            this.OptionsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.OptionsLayoutPanel.Size = new System.Drawing.Size(740, 26);
            this.OptionsLayoutPanel.TabIndex = 6;
            // 
            // Catchup
            // 
            this.Catchup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Catchup.AutoSize = true;
            this.Catchup.Enabled = false;
            this.Catchup.Location = new System.Drawing.Point(575, 3);
            this.Catchup.Name = "Catchup";
            this.Catchup.Size = new System.Drawing.Size(162, 20);
            this.Catchup.TabIndex = 2;
            this.Catchup.Tag = "\\CATCHUP_MISSED_BACKUPS_TAG";
            this.Catchup.Text = "\\CATCHUP_MISSED_BACKUPS";
            this.Catchup.UseVisualStyleBackColor = true;
            // 
            // Enable
            // 
            this.Enable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Enable.AutoSize = true;
            this.Enable.Location = new System.Drawing.Point(3, 3);
            this.Enable.Name = "Enable";
            this.Enable.Size = new System.Drawing.Size(120, 20);
            this.Enable.TabIndex = 1;
            this.Enable.Text = "\\SCHEDULE_ENABLE";
            this.Enable.UseVisualStyleBackColor = true;
            // 
            // FrequencyLayoutPanel
            // 
            this.FrequencyLayoutPanel.Controls.Add(this.DailyBtn);
            this.FrequencyLayoutPanel.Controls.Add(this.WeeklyBtn);
            this.FrequencyLayoutPanel.Controls.Add(this.WeekDay);
            this.FrequencyLayoutPanel.Controls.Add(this.MonthlyBtn);
            this.FrequencyLayoutPanel.Controls.Add(this.MonthDay);
            this.FrequencyLayoutPanel.Location = new System.Drawing.Point(14, 95);
            this.FrequencyLayoutPanel.Name = "FrequencyLayoutPanel";
            this.FrequencyLayoutPanel.Size = new System.Drawing.Size(743, 77);
            this.FrequencyLayoutPanel.TabIndex = 7;
            // 
            // DailyBtn
            // 
            this.DailyBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.DailyBtn.AutoSize = true;
            this.DailyBtn.Checked = true;
            this.FrequencyLayoutPanel.SetFlowBreak(this.DailyBtn, true);
            this.DailyBtn.Location = new System.Drawing.Point(3, 3);
            this.DailyBtn.Name = "DailyBtn";
            this.DailyBtn.Size = new System.Drawing.Size(59, 16);
            this.DailyBtn.TabIndex = 2;
            this.DailyBtn.TabStop = true;
            this.DailyBtn.Text = "\\DAILY";
            this.DailyBtn.UseVisualStyleBackColor = true;
            // 
            // WeeklyBtn
            // 
            this.WeeklyBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.WeeklyBtn.AutoSize = true;
            this.WeeklyBtn.Location = new System.Drawing.Point(3, 25);
            this.WeeklyBtn.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.WeeklyBtn.Name = "WeeklyBtn";
            this.WeeklyBtn.Size = new System.Drawing.Size(65, 20);
            this.WeeklyBtn.TabIndex = 3;
            this.WeeklyBtn.TabStop = true;
            this.WeeklyBtn.Text = "\\WEEKLY";
            this.WeeklyBtn.UseVisualStyleBackColor = true;
            // 
            // WeekDay
            // 
            this.WeekDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FrequencyLayoutPanel.SetFlowBreak(this.WeekDay, true);
            this.WeekDay.FormattingEnabled = true;
            this.WeekDay.Location = new System.Drawing.Point(68, 25);
            this.WeekDay.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.WeekDay.Name = "WeekDay";
            this.WeekDay.Size = new System.Drawing.Size(121, 20);
            this.WeekDay.TabIndex = 1;
            // 
            // MonthlyBtn
            // 
            this.MonthlyBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.MonthlyBtn.AutoSize = true;
            this.MonthlyBtn.Location = new System.Drawing.Point(3, 51);
            this.MonthlyBtn.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.MonthlyBtn.Name = "MonthlyBtn";
            this.MonthlyBtn.Size = new System.Drawing.Size(71, 21);
            this.MonthlyBtn.TabIndex = 5;
            this.MonthlyBtn.TabStop = true;
            this.MonthlyBtn.Text = "\\MONTHLY";
            this.MonthlyBtn.UseVisualStyleBackColor = true;
            // 
            // MonthDay
            // 
            this.MonthDay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.MonthDay.AutoSize = true;
            this.MonthDay.Location = new System.Drawing.Point(74, 51);
            this.MonthDay.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.MonthDay.Maximum = new decimal(new int[] {
            28,
            0,
            0,
            0});
            this.MonthDay.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.MonthDay.Name = "MonthDay";
            this.MonthDay.Size = new System.Drawing.Size(33, 21);
            this.MonthDay.TabIndex = 4;
            this.MonthDay.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // TimeSelectionPanel
            // 
            this.TimeSelectionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TimeSelectionPanel.Controls.Add(this.AtLabel);
            this.TimeSelectionPanel.Controls.Add(this.Time);
            this.TimeSelectionPanel.Enabled = false;
            this.TimeSelectionPanel.Location = new System.Drawing.Point(14, 179);
            this.TimeSelectionPanel.Name = "TimeSelectionPanel";
            this.TimeSelectionPanel.Size = new System.Drawing.Size(344, 31);
            this.TimeSelectionPanel.TabIndex = 8;
            // 
            // AtLabel
            // 
            this.AtLabel.AutoSize = true;
            this.AtLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.AtLabel.Location = new System.Drawing.Point(3, 0);
            this.AtLabel.Name = "AtLabel";
            this.AtLabel.Size = new System.Drawing.Size(23, 27);
            this.AtLabel.TabIndex = 0;
            this.AtLabel.Text = "\\AT";
            this.AtLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Time
            // 
            this.Time.CustomFormat = "HH:mm";
            this.Time.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.Time.Location = new System.Drawing.Point(32, 3);
            this.Time.Name = "Time";
            this.Time.ShowUpDown = true;
            this.Time.Size = new System.Drawing.Size(63, 21);
            this.Time.TabIndex = 6;
            this.Time.Value = new System.DateTime(2011, 1, 1, 0, 0, 0, 0);
            // 
            // ActionsPanel
            // 
            this.ActionsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ActionsPanel.ColumnCount = 2;
            this.ActionsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ActionsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ActionsPanel.Controls.Add(this.Cancel, 1, 0);
            this.ActionsPanel.Controls.Add(this.Save, 0, 0);
            this.ActionsPanel.Location = new System.Drawing.Point(399, 179);
            this.ActionsPanel.Name = "ActionsPanel";
            this.ActionsPanel.RowCount = 1;
            this.ActionsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ActionsPanel.Size = new System.Drawing.Size(358, 31);
            this.ActionsPanel.TabIndex = 9;
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(182, 3);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(173, 25);
            this.Cancel.TabIndex = 1;
            this.Cancel.Text = "\\CANCEL";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // Save
            // 
            this.Save.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Save.Location = new System.Drawing.Point(3, 3);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(173, 25);
            this.Save.TabIndex = 0;
            this.Save.Text = "\\SAVE";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // SchedulingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(769, 302);
            this.Controls.Add(this.ActionsPanel);
            this.Controls.Add(this.TimeSelectionPanel);
            this.Controls.Add(this.FrequencyLayoutPanel);
            this.Controls.Add(this.OptionsLayoutPanel);
            this.Controls.Add(this.WarningLabel);
            this.Name = "SchedulingForm";
            this.Text = "SchedulingForm";
            this.Load += new System.EventHandler(this.SchedulingForm_Load);
            this.OptionsLayoutPanel.ResumeLayout(false);
            this.OptionsLayoutPanel.PerformLayout();
            this.FrequencyLayoutPanel.ResumeLayout(false);
            this.FrequencyLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MonthDay)).EndInit();
            this.TimeSelectionPanel.ResumeLayout(false);
            this.TimeSelectionPanel.PerformLayout();
            this.ActionsPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Label WarningLabel;
        internal System.Windows.Forms.TableLayoutPanel OptionsLayoutPanel;
        internal System.Windows.Forms.CheckBox Catchup;
        internal System.Windows.Forms.CheckBox Enable;
        internal System.Windows.Forms.FlowLayoutPanel FrequencyLayoutPanel;
        internal System.Windows.Forms.RadioButton DailyBtn;
        internal System.Windows.Forms.RadioButton WeeklyBtn;
        internal System.Windows.Forms.ComboBox WeekDay;
        internal System.Windows.Forms.RadioButton MonthlyBtn;
        internal System.Windows.Forms.NumericUpDown MonthDay;
        internal System.Windows.Forms.FlowLayoutPanel TimeSelectionPanel;
        internal System.Windows.Forms.Label AtLabel;
        internal System.Windows.Forms.DateTimePicker Time;
        internal System.Windows.Forms.TableLayoutPanel ActionsPanel;
        internal System.Windows.Forms.Button Cancel;
        internal System.Windows.Forms.Button Save;
    }
}