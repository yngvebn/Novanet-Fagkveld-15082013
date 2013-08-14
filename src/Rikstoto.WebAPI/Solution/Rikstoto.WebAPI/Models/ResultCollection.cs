using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Rikstoto.WebAPI.Models
{
    [DataContract(Namespace = "")]
    public class ResultCollection<T> 
    {
        public ResultCollection()
        {
            Elements = new Collection<T>();
            Links = new Collection<Link>();
        }

        [DataMember]
        public int Limit { get; set; }
        [DataMember]
        public int Offset { get; set; }
        [DataMember]
        public Collection<Link> Links { get; set; }
        [DataMember]
        public Collection<T> Elements { get; set; }
        [DataMember]
        public int TotalCount { get; set; }
    }
}