using System;
using System.Net;
using System.Runtime.Serialization;

namespace Rikstoto.WebAPI.Models
{
    /// <summary>
    /// Generic class to be used as the content in the http message in the case of errors.
    /// </summary>
    [DataContract(Namespace = "")]
    public class Error
    {
        public Error(string errorMessage, string errorTag, string errorCode)
        {
            ErrorMessage = errorMessage;
            ErrorTag = errorTag;
            ErrorCode = errorCode;
            HelpUrl = @"http://developer.rikstoto.no/help?id=" + errorCode;
        }

        /// <summary>
        /// Gets or sets the http status code (the number part only ie. 401).
        /// When used with a error response message this is set automatically.
        /// </summary>
        public HttpStatusCode HttpStatusCode { get; set; }

        [DataMember(Name = "HttpStatusCode")]
        public int HttpStatus
        {
            get { return (int) HttpStatusCode; }
            set { HttpStatusCode = (HttpStatusCode) value; }
        }

        /// <summary>
        /// Gets the http status name corresponding to the status code ie. NotFound, Created etc.
        /// When used with a error response message this is set automatically.
        /// We need the setter because of serialization/deserialization.
        /// </summary>
        [DataMember]
        public string HttpStatusName
        {
            get { return HttpStatusCode.ToString(); }
            set
            {
                HttpStatusCode = (HttpStatusCode) Enum.Parse(typeof (HttpStatusCode), value, true);
            }
        }

        /// <summary>
        /// Gets or sets a detailed message for the developer consuming the API.
        /// No stacktrace should be included here.
        /// </summary>
        [DataMember]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets an unique error tag which can be traced back in the server log (ie. like the ITag).
        /// </summary>
        [DataMember]
        public string ErrorTag { get; set; }

        /// <summary>
        /// Gets or sets a error code detailing the http status code. 
        /// This error code is specific to the API.
        /// </summary>
        [DataMember]
        public string ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets a url pointing to a page where more help can be found.
        /// </summary>
        [DataMember]
        public string HelpUrl { get; set; }

        [DataMember]
        public string Time { get; set; }
    }
}