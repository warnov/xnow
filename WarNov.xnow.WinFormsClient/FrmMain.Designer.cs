namespace WarNov.xnow.WinFormsClient
{
    partial class FrmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.TxtCommand = new System.Windows.Forms.TextBox();
            this.NtfMain = new System.Windows.Forms.NotifyIcon(this.components);
            this.SuspendLayout();
            // 
            // TxtCommand
            // 
            this.TxtCommand.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.TxtCommand.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.TxtCommand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.TxtCommand.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TxtCommand.Font = new System.Drawing.Font("Calibri", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtCommand.ForeColor = System.Drawing.Color.White;
            this.TxtCommand.HideSelection = false;
            this.TxtCommand.Location = new System.Drawing.Point(52, 26);
            this.TxtCommand.Name = "TxtCommand";
            this.TxtCommand.Size = new System.Drawing.Size(909, 59);
            this.TxtCommand.TabIndex = 0;
            this.TxtCommand.Text = "Command Here";
            this.TxtCommand.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TxtCommand.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TxtCommand_MouseClick);
            this.TxtCommand.Enter += new System.EventHandler(this.TxtCommand_Enter);
            // 
            // NtfMain
            // 
            this.NtfMain.Icon = ((System.Drawing.Icon)(resources.GetObject("NtfMain.Icon")));
            this.NtfMain.Text = "xnow";
            this.NtfMain.Visible = true;
            this.NtfMain.Click += new System.EventHandler(this.NtfMain_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1016, 115);
            this.ControlBox = false;
            this.Controls.Add(this.TxtCommand);
            this.Font = new System.Drawing.Font("Cascadia Code", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmMain";
            this.Opacity = 0.7D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Deactivate += new System.EventHandler(this.FrmMain_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TxtCommand;
        private System.Windows.Forms.NotifyIcon NtfMain;
    }
}

