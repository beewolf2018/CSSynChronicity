namespace CSSynChronicity.Interface
{
    partial class LanguageForm
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
            this.LanguagesList = new System.Windows.Forms.ComboBox();
            this.OkBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LanguagesList
            // 
            this.LanguagesList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LanguagesList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LanguagesList.Location = new System.Drawing.Point(12, 57);
            this.LanguagesList.Name = "LanguagesList";
            this.LanguagesList.Size = new System.Drawing.Size(244, 20);
            this.LanguagesList.TabIndex = 5;
            // 
            // OkBtn
            // 
            this.OkBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OkBtn.Location = new System.Drawing.Point(367, 50);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(87, 27);
            this.OkBtn.TabIndex = 6;
            this.OkBtn.Text = "Ok";
            this.OkBtn.UseVisualStyleBackColor = true;
            this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // LanguageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 111);
            this.Controls.Add(this.OkBtn);
            this.Controls.Add(this.LanguagesList);
            this.Name = "LanguageForm";
            this.Text = "LanguageForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LanguageForm_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.ComboBox LanguagesList;
        internal System.Windows.Forms.Button OkBtn;
    }
}