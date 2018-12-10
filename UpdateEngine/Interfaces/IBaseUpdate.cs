using UpdateEngine.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

/// <summary>
/// name space for interfaces
/// </summary>
namespace UpdateEngine.Interfaces
{
    public delegate void SendMessageEventHandler(string message);
    public delegate void SendProgressEventHandler(int percent);

    /// <summary>
    /// Interface for the BaseUpdate class.
    /// </summary>
    internal interface IBaseUpdate
    {
        bool IsRollBack { get; set; }
        string AR_ID { get; }
        string TFS_ID { get; }
        Dictionary<Enums.UpdateEnvironment, string> EnvironmentsToApply { get; }
        UpdateEnvironment UpdateEnvironment { get; }
        UpdateStatus UpdateStatus { get; }
        string UpdateDescription { get; }

        void Execute();

        event SendMessageEventHandler OnSendMessage;
        event SendProgressEventHandler OnSendProgress;

    }
}
