using UpdateEngine.Classes.Abstract;
using UpdateEngine.Classes.Sealed;
using GPL;
using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;
using System.Linq;

namespace UpdateEngine
{
    /// <summary>
    /// class MainForm
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class MainForm : Form
    {
        private BaseUpdate o;

        protected string CurrentExecutablePath = string.Empty;
        private string DataPath = string.Empty;
        private string ZIP_FILE_FULL_PATH = string.Empty;
        private string LOG_FILE_FULL_PATH = string.Empty;




        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control.

        /// <summary>
        /// WriteMessageCallback delegate
        /// </summary>
        /// <param name="text">The text.</param>
        delegate void WriteMessageCallback(string text);
        //delegate void WriteProgressCallback(int percent);

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            ControlBox = false;
            StartPosition = FormStartPosition.CenterScreen;

            prgbUpdateProgress.Maximum = 100;
            prgbUpdateProgress.Step = 1;
            prgbUpdateProgress.Value = 0;
        }

        /// <summary>
        /// Handles the Load event of the MainForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void MainForm_Load(object sender, EventArgs e)
        {


            txtUpdateMessages.Text = string.Empty;

            CurrentExecutablePath = Path.GetDirectoryName(Utility.GetCurrentExecutablePath()).Replace(@"\bin\Debug", "").Replace(@"\bin\Release", "");
            WriteMessage(string.Format("Current Executable path='{0}' on Class='{1}'", CurrentExecutablePath, this.GetType().Name));

            DataPath = Path.Combine(CurrentExecutablePath, @"App_Data");
            WriteMessage(string.Format("DataPath='{0}' on Class='{1}'", DataPath, this.GetType().Name));

            // Get ZipFileFullPath
            ZIP_FILE_FULL_PATH = Path.Combine(DataPath, @"LOG.ZIP");
            WriteMessage(string.Format("ZIP_FILE_FULL_PATH='{0}' on Class='{1}'", ZIP_FILE_FULL_PATH, this.GetType().Name));

            // Get ZipFileFullPath
            LOG_FILE_FULL_PATH = Path.Combine(DataPath, @"LOG.LOG");
            WriteMessage(string.Format("LOG_FILE_FULL_PATH='{0}' on Class='{1}'", LOG_FILE_FULL_PATH, this.GetType().Name));

            // Load the object type of BaseUpdate.
            o = new Update_TFS_5360();

            txtUpdateMessages.AppendText(@"Object created from: " + o.GetType() + @"." + Environment.NewLine);

            // subscribe to the WriteToLog event.
            txtUpdateMessages.AppendText(@"subscribing to the Object events." + Environment.NewLine);
            o.OnSendMessage += WriteMessage;

            // Create the radiobuttons inside of ngpbxEnvirontment
            var EnvironmentsToApply = o.EnvironmentsToApply;

            int PX = 10;
            int PY = 19;

            foreach (var item in EnvironmentsToApply.Keys)
            {


                if (item == Enums.UpdateEnvironment.Unknow)
                    continue;

                var btn = new RadioButton();
                btn.Name = @"rbtn" + item.ToString();
                btn.Text = item.ToString();
                btn.TabStop = true;
                btn.CheckedChanged += EnvironmentChanged;

                switch (item)
                {
                    case UpdateEngine.Enums.UpdateEnvironment.Development:
                        btn.Size = new System.Drawing.Size(88, 17);
                        break;
                    case UpdateEngine.Enums.UpdateEnvironment.Test:
                        btn.Size = new System.Drawing.Size(46, 17);
                        break;
                    case UpdateEngine.Enums.UpdateEnvironment.Stage:
                        btn.Size = new System.Drawing.Size(53, 17);
                        break;
                    case UpdateEngine.Enums.UpdateEnvironment.Production:
                        btn.Size = new System.Drawing.Size(76, 17);
                        break;
                    default:
                        break;
                }

                btn.Location = new System.Drawing.Point(PX, PY);

                gpbxEnvirontment.Controls.Add(btn);

                PX += btn.Size.Width + 4;
            }

            GetUpdateInformation();
        }

        /// <summary>
        /// Gets the update information.
        /// </summary>
        private void GetUpdateInformation()
        {
            // Get object properties to display textboxes.
            txtUpdateDescription.Text = o.UpdateDescription;
            txtARID.Text = o.AR_ID;
            txtTFSID.Text = o.TFS_ID;
            txtUpdateStatus.Text = o.UpdateStatus.ToString();
            WriteMessage(@"Update information updated.");
        }

        /// <summary>
        /// Writes the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void WriteMessage(string message)
        {
            // https://msdn.microsoft.com/en-us/library/ms171728.aspx
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txtUpdateMessages.InvokeRequired)
            {
                WriteMessageCallback d = new WriteMessageCallback(WriteMessage);
                this.Invoke(d, new object[] { message });
            }
            else
            {
                this.txtUpdateMessages.AppendText(message + Environment.NewLine);
            }
        }

        /// <summary>
        /// Writes the progress.
        /// </summary>
        /// <param name="percent">The percent.</param>
        public void WriteProgress(int percent)
        {
            backgroundWorker001.ReportProgress(percent);
        }

        /// <summary>
        /// Handles the Click event of the ExitButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ExitButton_Click(object sender, EventArgs e)
        {

            if (backgroundWorker001.IsBusy)
            {
                MessageBox.Show(@"Update is being {0} in this momment, please wait until it finish.".FormatString((rbtnApplyUpdate.Checked ? @"Applied" : @"roll backed")), @"Can not exit now.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (System.Windows.Forms.Application.MessageLoop)
            {
                // WinForms app
                System.Windows.Forms.Application.Exit();
            }
            else
            {
                // Console app
                System.Environment.Exit(1);
            }
        }

        /// <summary>
        /// Handles the Click event of the Startbutton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Startbutton_Click(object sender, EventArgs e)
        {
            if (UserSelectionIsValid())
            {

                if (rbtnApplyUpdate.Checked)
                    o.IsRollBack = false;

                if (rbtnRollBackUpdate.Checked)
                    o.IsRollBack = true;

                EnableControls(false);

                // Start the asynchronous operation.
                backgroundWorker001.RunWorkerAsync();
            }
        }

        /// <summary>
        /// Enable or Disable the controls.
        /// </summary>
        /// <param name="enableValue">if set to <c>true</c> [enable value].</param>
        private void EnableControls(bool enableValue)
        {
            // lock all controls to avoid error.
            gpbxEnvirontment.Enabled = enableValue;
            gpbxUpdateAction.Enabled = enableValue;
            btnStart.Enabled = enableValue;
        }

        /// <summary>
        /// Users the selection is valid.
        /// </summary>
        /// <returns></returns>
        private bool UserSelectionIsValid()
        {
            // Validate the selected Environment
            if (!gpbxEnvirontment.Controls.OfType<RadioButton>().Any(rb => rb.Checked))
            {
                gpbxEnvirontment.Focus();
                MessageBox.Show("Please Select an Environment", "Error in selection.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            //if (!rbtnDevelopment.Checked && !rbtnTest.Checked && !rbtnStage.Checked && !rbtnProduction.Checked)
            //{
            //    MessageBox.Show("Please Select an Environment", "Error in selection.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return false;
            //}

            // Validate the selected Update Action
            if (!rbtnApplyUpdate.Checked && !rbtnRollBackUpdate.Checked)
            {
                MessageBox.Show("Please Select a Update Action", "Error in selection.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Environments the changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void EnvironmentChanged(object sender, EventArgs e)
        {
            RadioButton s = sender as RadioButton;

            o.UpdateEnvironment = Enums.UpdateEnvironment.Unknow;

            if (s.Checked)
            {
                o.UpdateEnvironment = s.Text.ParseEnum<Enums.UpdateEnvironment>();

                txtUpdateMessages.AppendText(@"Environment Changed to:" + o.UpdateEnvironment + @"." + Environment.NewLine);

                GetUpdateInformation();
            }
        }

        /// <summary>
        /// Handles the DoWork event of the backgroundWorker001 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
        private void backgroundWorker001_DoWork(object sender, DoWorkEventArgs e)
        {
            var backgroundWorker = sender as BackgroundWorker;

            o.OnSendProgress += WriteProgress;

            o.Execute();
        }

        /// <summary>
        /// Handles the RunWorkerCompleted event of the backgroundWorker001 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RunWorkerCompletedEventArgs"/> instance containing the event data.</param>
        private void backgroundWorker001_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                WriteMessage(e.Error.Message);
                MessageBox.Show(e.Error.Message, "Program error.", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // unlock all controls.
                EnableControls(true);
            }
            else if (e.Cancelled)
            {
                // Next, handle the case where the user canceled 
                // the operation.
                // Note that due to a race condition in 
                // the DoWork event handler, the Cancelled
                // flag may not have been set, even though
                // CancelAsync was called.
                MessageBox.Show(@"The Update was CANCELLED...", "Program information.", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                // Finally, handle the case where the operation 
                // succeeded.

                GetUpdateInformation();
                var msg = @"The Update AR#:{0} TF#:{1} was {2} successful on {3} environment.";

                if (rbtnApplyUpdate.Checked)
                    msg = msg.FormatString(o.AR_ID, o.TFS_ID, @"applied", o.UpdateEnvironment.ToString().ToLower());
                if (rbtnRollBackUpdate.Checked)
                    msg = msg.FormatString(o.AR_ID, o.TFS_ID, @"roll backed", o.UpdateEnvironment.ToString().ToLower());

                WriteMessage(msg);

                SendEmailWithLog(msg);

                MessageBox.Show(msg, "Program ended.", MessageBoxButtons.OK, MessageBoxIcon.Information);

                btnExit.PerformClick();
            }
        }

        /// <summary>
        /// Handles the ProgressChanged event of the backgroundWorker001 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ProgressChangedEventArgs"/> instance containing the event data.</param>
        private void backgroundWorker001_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            prgbUpdateProgress.Value = e.ProgressPercentage;
        }

        private void SendEmailWithLog(string subject)
        {
            WriteMessage(@"Starting Executing method:'{0}' on Class:'{1}'".FormatString(Utility.GetCurrentMethodName(), this.GetType().Name));

            // Zip the log file to email it.
            ZipLogInformation();

            using (Email em = new Email())
            {
                em.From = Utility.GetMachineName() + @"@us.mednax.com";
                em.To = @"Marcos_Iturribeitia@us.mednax.com";
                em.Cc = @"Marcos_Iturribeitia@us.mednax.com;iturribeitia@gmail.com";
                em.Subject = subject;

                em.Body = subject + @", Please See the attached log file.";

                em.Attachments.Add(ZIP_FILE_FULL_PATH);

                em.SendEmail();
            }

            WriteMessage(@"Ending Executing method:'{0}' on Class:'{1}'".FormatString(Utility.GetCurrentMethodName(), this.GetType().Name));
        }

        private void ZipLogInformation()
        {
            WriteMessage(@"Starting Executing method:'{0}' on Class:'{1}'".FormatString(Utility.GetCurrentMethodName(), this.GetType().Name));

            // Write all the txtUpdateMessages.Text to a log file.
            if (File.Exists(LOG_FILE_FULL_PATH))
                File.Delete(LOG_FILE_FULL_PATH);

            if (File.Exists(ZIP_FILE_FULL_PATH))
                File.Delete(ZIP_FILE_FULL_PATH);

            Utility.WriteTextToFile(LOG_FILE_FULL_PATH, txtUpdateMessages.Text);

            using (ZipArchive zip = ZipFile.Open(ZIP_FILE_FULL_PATH, ZipArchiveMode.Create))
            {
                zip.CreateEntryFromFile(LOG_FILE_FULL_PATH, Path.GetFileName(LOG_FILE_FULL_PATH));
            }

            WriteMessage(@"Ending Executing method:'{0}' on Class:'{1}'".FormatString(Utility.GetCurrentMethodName(), this.GetType().Name));
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (Utility.DetectVirtualMachine())
                MessageBox.Show(@"This program is running in a Virtual machne.", @"Program information", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }


    }
}
