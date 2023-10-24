using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Notification
{
    /// <summary>
    /// The severity of a status or message.
    /// </summary>
    public static class SeverityLevels
    {
        public const string None = "NONE";
        public const string Info = "INFO";
        public const string Warning = "WARNING";
        public const string Error = "ERROR";
        public const string Fatal = "FATAL";
        public const string Ok = "OK";
    }
}
