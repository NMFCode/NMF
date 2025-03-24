using LspTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.InlayClasses
{
    [DataContract]
    public class ExtendedServerCapabilities : ServerCapabilities
    {
        [DataMember(Name = "inlayHintProvider")]
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public InlayHintOptions InlayHintProvider { get; set; }
    }
}
