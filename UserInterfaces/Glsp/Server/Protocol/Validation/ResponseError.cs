using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Validation
{
    public class ResponseError
    {
        /// <summary>
         ///  Code identifying the error kind.
         /// </summary>
        public int Code { get; set; }

        /// <summary>
         ///  Error message.
         /// </summary>
        public string Message { get; set; }

        /// <summary>
         ///  Additional custom data, e.g., a serialized stacktrace.
         /// </summary>
        public object Data { get; set; }
    }
}
