using System;
using System.Net.Http;
using System.Runtime.Serialization;

namespace Rikstoto.WebAPI.Models
{
    [DataContract(Namespace = "")]
    public class Link
    {
        public Link()
        {
            Method = "Get";
        }

        [DataMember]
        public Uri Href { get; set; }

        [DataMember]
        public string Rel { get; set; }

        [DataMember]
        public string Method { get; set; }
    }
}