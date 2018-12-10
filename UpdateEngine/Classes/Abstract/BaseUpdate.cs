using UpdateEngine.Interfaces;
using GPL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// namespace for UpdateEngine.Classes.Abstract
/// </summary>
namespace UpdateEngine.Classes.Abstract
{
    internal abstract class BaseUpdate : IBaseUpdate
    {
        public event SendMessageEventHandler OnSendMessage;
        public event SendProgressEventHandler OnSendProgress;

        protected Enums.UpdateStatus _UpdateStatus = Enums.UpdateStatus.Unknow;
        protected Enums.UpdateEnvironment _UpdateEnvironment = Enums.UpdateEnvironment.Unknow;
        protected string CurrentExecutablePath = string.Empty;

        private string _MachineName;
        private string _UserName;

        
        /// <summary>
        /// Executes this instance.
        /// </summary>
        public virtual void Execute()
        {
            // fire the event
            SendMessage(string.Format(@"Starting Executing method:'{0}' on Class:'{1}'", Utility.GetCurrentMethodName(), this.GetType().BaseType.Name));

            if (UpdateEnvironment.Equals(Enums.UpdateEnvironment.Unknow))
                throw new Exception(@"ERROR Can not execute the update in a Unknow Environment.");

            if (IsTimeToRun()) // IsTimeToRun is abstract
            {
                GetConfigValues(); // GetConfigValues is virtual
                GetPreExecutionData(); // GetPreExecutionData is virtual

                if (IsRollBack)
                    ExecuteRollBack(); // ExecuteRollBack is abstract
                else
                {
                    ExecuteBackUp(); // ExecuteBackUp is abstract
                    ExecuteUpdate(); // ExecuteUpdate is abstract
                }
            }

            SendMessage(string.Format(@"Ending Executing method:'{0}' on Class:'{1}'", Utility.GetCurrentMethodName(), this.GetType().BaseType.Name));


        }

        /// <summary>
        /// Executes the back up.
        /// </summary>
        protected abstract void ExecuteBackUp();

        /// <summary>
        /// Executes the update.
        /// </summary>
        protected abstract void ExecuteUpdate();

        /// <summary>
        /// Executes the roll back.
        /// </summary>
        protected abstract void ExecuteRollBack();

        /// <summary>
        /// Gets the pre execution data.
        /// </summary>
        protected abstract void GetPreExecutionData();

        /// <summary>
        /// Get the Configuration Values.
        /// </summary>
        protected virtual void GetConfigValues()
        {
            // Get here values that does not depend of the Environtment

            SendMessage(string.Format(@"Starting Executing method:'{0}' on Class:'{1}'", Utility.GetCurrentMethodName(), this.GetType().BaseType.Name));


            _MachineName = Utility.GetMachineName();
            SendMessage(string.Format(@"MachineName='{0}'", _MachineName));

            _UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            SendMessage(string.Format(@"UserName='{0}'", _UserName));

#if DEBUG
            SendMessage(@"Object version: Debug.");
#else
            SendMessage(@"Object version: Release.");

#endif

            CurrentExecutablePath = Path.GetDirectoryName(Utility.GetCurrentExecutablePath()).Replace(@"\bin\Debug", "").Replace(@"\bin\Release", "");

            //#if DEBUG
            //            CurrentExecutablePath = CurrentExecutablePath.Replace(@"\bin\Debug","");
            //#endif

            SendMessage(string.Format("Current Executable Path:'{0}'", CurrentExecutablePath));

            SendMessage(string.Format(@"Ending Executing method:'{0}' on Class:'{1}'", Utility.GetCurrentMethodName(), this.GetType().BaseType.Name));

        }

        /// <summary>
        /// Determines whether is time to run this Update.
        /// </summary>
        /// <returns></returns>
        protected abstract Boolean IsTimeToRun();

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void SendMessage(string message)
        {
            SendMessageEventHandler handler = OnSendMessage;
            if (handler != null)
            {
                handler(message);
            }
        }

        /// <summary>
        /// Sends the progress.
        /// </summary>
        /// <param name="percent">The percent.</param>
        protected void SendProgress(int percent)
        {
            SendProgressEventHandler handler = OnSendProgress;
            if (handler != null)
            {
                handler(percent);
            }
        }

        public abstract string AR_ID
        {
            get;
        }

        public abstract string TFS_ID
        {
            get;
        }

        public abstract string UpdateDescription
        {
            get;
        }

        public bool IsRollBack
        {
            get;
            set;
        }

        public Enums.UpdateEnvironment UpdateEnvironment
        {
            get { return _UpdateEnvironment; }
            set
            {
                // Check that the new value exist in the EnvironmentsToApply

                if (EnvironmentsToApply.ContainsKey(value))
                {
                    _UpdateEnvironment = value;
                        RefreshUpdateStatus();
                }
                else
                {
                    throw new ArgumentOutOfRangeException(@"UpdateEnvironment", @"Invalid value, Valid values are: {0}".FormatString(EnvironmentsToApply.Keys.ToCSV()));
                }
            }
        }

        public abstract Dictionary<Enums.UpdateEnvironment,string> EnvironmentsToApply
        {
            get;
        }

        public Enums.UpdateStatus UpdateStatus
        {
            get { return _UpdateStatus; }
        }

        public string MachineName
        {
            get { return _MachineName; }
        }

        public string UserName
        {
            get { return _UserName; }
        }


        protected abstract void RefreshUpdateStatus();


    }
}
