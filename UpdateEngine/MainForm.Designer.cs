namespace UpdateEngine
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btnStart = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.gpbxUpdateInformation = new System.Windows.Forms.GroupBox();
            this.txtUpdateStatus = new System.Windows.Forms.TextBox();
            this.lblUpdateStatus = new System.Windows.Forms.Label();
            this.txtTFSID = new System.Windows.Forms.TextBox();
            this.LblTFSID = new System.Windows.Forms.Label();
            this.txtARID = new System.Windows.Forms.TextBox();
            this.lblARID = new System.Windows.Forms.Label();
            this.txtUpdateDescription = new System.Windows.Forms.TextBox();
            this.lblUpdateDescription = new System.Windows.Forms.Label();
            this.gpbxUpdateMessages = new System.Windows.Forms.GroupBox();
            this.txtUpdateMessages = new System.Windows.Forms.TextBox();
            this.gpbxUpdateProgress = new System.Windows.Forms.GroupBox();
            this.prgbUpdateProgress = new System.Windows.Forms.ProgressBar();
            this.gpbxUpdateAction = new System.Windows.Forms.GroupBox();
            this.rbtnRollBackUpdate = new System.Windows.Forms.RadioButton();
            this.rbtnApplyUpdate = new System.Windows.Forms.RadioButton();
            this.backgroundWorker001 = new System.ComponentModel.BackgroundWorker();
            this.gpbxEnvirontment = new System.Windows.Forms.GroupBox();
            this.gpbxUpdateInformation.SuspendLayout();
            this.gpbxUpdateMessages.SuspendLayout();
            this.gpbxUpdateProgress.SuspendLayout();
            this.gpbxUpdateAction.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(12, 603);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "&Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.Startbutton_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(579, 603);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "&Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // gpbxUpdateInformation
            // 
            this.gpbxUpdateInformation.Controls.Add(this.txtUpdateStatus);
            this.gpbxUpdateInformation.Controls.Add(this.lblUpdateStatus);
            this.gpbxUpdateInformation.Controls.Add(this.txtTFSID);
            this.gpbxUpdateInformation.Controls.Add(this.LblTFSID);
            this.gpbxUpdateInformation.Controls.Add(this.txtARID);
            this.gpbxUpdateInformation.Controls.Add(this.lblARID);
            this.gpbxUpdateInformation.Controls.Add(this.txtUpdateDescription);
            this.gpbxUpdateInformation.Controls.Add(this.lblUpdateDescription);
            this.gpbxUpdateInformation.Location = new System.Drawing.Point(12, 4);
            this.gpbxUpdateInformation.Name = "gpbxUpdateInformation";
            this.gpbxUpdateInformation.Size = new System.Drawing.Size(642, 63);
            this.gpbxUpdateInformation.TabIndex = 6;
            this.gpbxUpdateInformation.TabStop = false;
            this.gpbxUpdateInformation.Text = "Update information";
            // 
            // txtUpdateStatus
            // 
            this.txtUpdateStatus.Location = new System.Drawing.Point(297, 35);
            this.txtUpdateStatus.Name = "txtUpdateStatus";
            this.txtUpdateStatus.ReadOnly = true;
            this.txtUpdateStatus.Size = new System.Drawing.Size(322, 20);
            this.txtUpdateStatus.TabIndex = 10;
            this.txtUpdateStatus.TabStop = false;
            this.txtUpdateStatus.Text = "Write here the Update Status.";
            // 
            // lblUpdateStatus
            // 
            this.lblUpdateStatus.AutoSize = true;
            this.lblUpdateStatus.Location = new System.Drawing.Point(259, 39);
            this.lblUpdateStatus.Name = "lblUpdateStatus";
            this.lblUpdateStatus.Size = new System.Drawing.Size(40, 13);
            this.lblUpdateStatus.TabIndex = 9;
            this.lblUpdateStatus.Text = "Status:";
            // 
            // txtTFSID
            // 
            this.txtTFSID.Location = new System.Drawing.Point(188, 35);
            this.txtTFSID.Name = "txtTFSID";
            this.txtTFSID.ReadOnly = true;
            this.txtTFSID.Size = new System.Drawing.Size(65, 20);
            this.txtTFSID.TabIndex = 8;
            this.txtTFSID.TabStop = false;
            this.txtTFSID.Text = "########";
            // 
            // LblTFSID
            // 
            this.LblTFSID.AutoSize = true;
            this.LblTFSID.Location = new System.Drawing.Point(145, 39);
            this.LblTFSID.Name = "LblTFSID";
            this.LblTFSID.Size = new System.Drawing.Size(37, 13);
            this.LblTFSID.TabIndex = 7;
            this.LblTFSID.Text = "TFS#:";
            // 
            // txtARID
            // 
            this.txtARID.Location = new System.Drawing.Point(74, 35);
            this.txtARID.Name = "txtARID";
            this.txtARID.ReadOnly = true;
            this.txtARID.Size = new System.Drawing.Size(65, 20);
            this.txtARID.TabIndex = 6;
            this.txtARID.TabStop = false;
            this.txtARID.Text = "########";
            // 
            // lblARID
            // 
            this.lblARID.AutoSize = true;
            this.lblARID.Location = new System.Drawing.Point(36, 39);
            this.lblARID.Name = "lblARID";
            this.lblARID.Size = new System.Drawing.Size(32, 13);
            this.lblARID.TabIndex = 5;
            this.lblARID.Text = "AR#:";
            // 
            // txtUpdateDescription
            // 
            this.txtUpdateDescription.Location = new System.Drawing.Point(71, 12);
            this.txtUpdateDescription.Name = "txtUpdateDescription";
            this.txtUpdateDescription.ReadOnly = true;
            this.txtUpdateDescription.Size = new System.Drawing.Size(548, 20);
            this.txtUpdateDescription.TabIndex = 4;
            this.txtUpdateDescription.TabStop = false;
            this.txtUpdateDescription.Text = "Write here the Update Description.";
            // 
            // lblUpdateDescription
            // 
            this.lblUpdateDescription.AutoSize = true;
            this.lblUpdateDescription.Location = new System.Drawing.Point(6, 16);
            this.lblUpdateDescription.Name = "lblUpdateDescription";
            this.lblUpdateDescription.Size = new System.Drawing.Size(63, 13);
            this.lblUpdateDescription.TabIndex = 3;
            this.lblUpdateDescription.Text = "Description:";
            // 
            // gpbxUpdateMessages
            // 
            this.gpbxUpdateMessages.Controls.Add(this.txtUpdateMessages);
            this.gpbxUpdateMessages.Location = new System.Drawing.Point(12, 127);
            this.gpbxUpdateMessages.Name = "gpbxUpdateMessages";
            this.gpbxUpdateMessages.Size = new System.Drawing.Size(642, 418);
            this.gpbxUpdateMessages.TabIndex = 9;
            this.gpbxUpdateMessages.TabStop = false;
            this.gpbxUpdateMessages.Text = "Update Messages";
            // 
            // txtUpdateMessages
            // 
            this.txtUpdateMessages.BackColor = System.Drawing.Color.Black;
            this.txtUpdateMessages.Font = new System.Drawing.Font("Consolas", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUpdateMessages.ForeColor = System.Drawing.Color.LimeGreen;
            this.txtUpdateMessages.Location = new System.Drawing.Point(9, 19);
            this.txtUpdateMessages.Multiline = true;
            this.txtUpdateMessages.Name = "txtUpdateMessages";
            this.txtUpdateMessages.ReadOnly = true;
            this.txtUpdateMessages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtUpdateMessages.Size = new System.Drawing.Size(626, 392);
            this.txtUpdateMessages.TabIndex = 0;
            this.txtUpdateMessages.TabStop = false;
            this.txtUpdateMessages.Text = resources.GetString("txtUpdateMessages.Text");
            // 
            // gpbxUpdateProgress
            // 
            this.gpbxUpdateProgress.Controls.Add(this.prgbUpdateProgress);
            this.gpbxUpdateProgress.Location = new System.Drawing.Point(12, 551);
            this.gpbxUpdateProgress.Name = "gpbxUpdateProgress";
            this.gpbxUpdateProgress.Size = new System.Drawing.Size(642, 46);
            this.gpbxUpdateProgress.TabIndex = 10;
            this.gpbxUpdateProgress.TabStop = false;
            this.gpbxUpdateProgress.Text = "Progress";
            // 
            // prgbUpdateProgress
            // 
            this.prgbUpdateProgress.Location = new System.Drawing.Point(6, 14);
            this.prgbUpdateProgress.Name = "prgbUpdateProgress";
            this.prgbUpdateProgress.Size = new System.Drawing.Size(630, 26);
            this.prgbUpdateProgress.Step = 2;
            this.prgbUpdateProgress.TabIndex = 5;
            // 
            // gpbxUpdateAction
            // 
            this.gpbxUpdateAction.Controls.Add(this.rbtnRollBackUpdate);
            this.gpbxUpdateAction.Controls.Add(this.rbtnApplyUpdate);
            this.gpbxUpdateAction.Location = new System.Drawing.Point(423, 73);
            this.gpbxUpdateAction.Name = "gpbxUpdateAction";
            this.gpbxUpdateAction.Size = new System.Drawing.Size(231, 49);
            this.gpbxUpdateAction.TabIndex = 7;
            this.gpbxUpdateAction.TabStop = false;
            this.gpbxUpdateAction.Text = "Select Action";
            // 
            // rbtnRollBackUpdate
            // 
            this.rbtnRollBackUpdate.AutoSize = true;
            this.rbtnRollBackUpdate.Location = new System.Drawing.Point(110, 19);
            this.rbtnRollBackUpdate.Name = "rbtnRollBackUpdate";
            this.rbtnRollBackUpdate.Size = new System.Drawing.Size(106, 17);
            this.rbtnRollBackUpdate.TabIndex = 4;
            this.rbtnRollBackUpdate.TabStop = true;
            this.rbtnRollBackUpdate.Text = "RollBack Update";
            this.rbtnRollBackUpdate.UseVisualStyleBackColor = true;
            // 
            // rbtnApplyUpdate
            // 
            this.rbtnApplyUpdate.AutoSize = true;
            this.rbtnApplyUpdate.Location = new System.Drawing.Point(12, 19);
            this.rbtnApplyUpdate.Name = "rbtnApplyUpdate";
            this.rbtnApplyUpdate.Size = new System.Drawing.Size(89, 17);
            this.rbtnApplyUpdate.TabIndex = 3;
            this.rbtnApplyUpdate.TabStop = true;
            this.rbtnApplyUpdate.Text = "Apply Update";
            this.rbtnApplyUpdate.UseVisualStyleBackColor = true;
            // 
            // backgroundWorker001
            // 
            this.backgroundWorker001.WorkerReportsProgress = true;
            this.backgroundWorker001.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker001_DoWork);
            this.backgroundWorker001.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker001_ProgressChanged);
            this.backgroundWorker001.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker001_RunWorkerCompleted);
            // 
            // gpbxEnvirontment
            // 
            this.gpbxEnvirontment.Location = new System.Drawing.Point(21, 73);
            this.gpbxEnvirontment.Name = "gpbxEnvirontment";
            this.gpbxEnvirontment.Size = new System.Drawing.Size(396, 49);
            this.gpbxEnvirontment.TabIndex = 11;
            this.gpbxEnvirontment.TabStop = false;
            this.gpbxEnvirontment.Text = "Select Environtment";
            // 
            // MainForm
            // 
            this.AcceptButton = this.btnStart;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 630);
            this.Controls.Add(this.gpbxEnvirontment);
            this.Controls.Add(this.gpbxUpdateAction);
            this.Controls.Add(this.gpbxUpdateProgress);
            this.Controls.Add(this.gpbxUpdateMessages);
            this.Controls.Add(this.gpbxUpdateInformation);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnStart);
            this.Name = "MainForm";
            this.Text = "Update Engine 1.0";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.gpbxUpdateInformation.ResumeLayout(false);
            this.gpbxUpdateInformation.PerformLayout();
            this.gpbxUpdateMessages.ResumeLayout(false);
            this.gpbxUpdateMessages.PerformLayout();
            this.gpbxUpdateProgress.ResumeLayout(false);
            this.gpbxUpdateAction.ResumeLayout(false);
            this.gpbxUpdateAction.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.GroupBox gpbxUpdateInformation;
        private System.Windows.Forms.TextBox txtTFSID;
        private System.Windows.Forms.Label LblTFSID;
        private System.Windows.Forms.TextBox txtARID;
        private System.Windows.Forms.Label lblARID;
        private System.Windows.Forms.TextBox txtUpdateDescription;
        private System.Windows.Forms.Label lblUpdateDescription;
        private System.Windows.Forms.GroupBox gpbxUpdateMessages;
        private System.Windows.Forms.TextBox txtUpdateMessages;
        private System.Windows.Forms.GroupBox gpbxUpdateProgress;
        private System.Windows.Forms.ProgressBar prgbUpdateProgress;
        private System.Windows.Forms.TextBox txtUpdateStatus;
        private System.Windows.Forms.Label lblUpdateStatus;
        private System.Windows.Forms.GroupBox gpbxUpdateAction;
        private System.Windows.Forms.RadioButton rbtnRollBackUpdate;
        private System.Windows.Forms.RadioButton rbtnApplyUpdate;
        private System.ComponentModel.BackgroundWorker backgroundWorker001;
        private System.Windows.Forms.GroupBox gpbxEnvirontment;
    }
}

