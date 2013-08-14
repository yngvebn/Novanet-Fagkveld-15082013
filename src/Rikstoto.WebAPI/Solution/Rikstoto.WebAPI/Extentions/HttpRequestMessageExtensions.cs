using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Http;
using Rikstoto.WebAPI.Helpers;
using Rikstoto.WebAPI.Models;

namespace Rikstoto.WebAPI.Extentions
{
    public static class HttpRequestMessageExtensions
    {
        public static string GetAllHeadersFormatted(this HttpRequestMessage request)
        {
            var headers = request.Headers.ToString();

            if (request.Content != null)
                headers += request.Content.Headers.ToString();

            return headers;
        }

        public static string GetClientIpAddress(this HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;

            if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty prop;
                prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
                return prop.Address;
            }
            
            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="errorMessage">Message</param>
        /// <param name="errorTag">Unique tag to identify the error. If left as defualt null or string.Empty one will be generated</param>
        /// <param name="errorCode">A code for this type of error. If left default null or string.Empty the value (int)HttpStatusCode.InternalServerError will be used</param>
        /// <returns></returns>
        public static HttpResponseMessage CreateInternalServerErrorResponseMessage(this HttpRequestMessage request, string errorMessage, string errorTag = null, string errorCode = null)
        {
            var error = GetErrorMessage(request, errorMessage, HttpStatusCode.InternalServerError, errorTag, errorCode);
            return request.CreateResponse(error.HttpStatusCode, error);
        }

        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="errorMessage">Message</param>
        /// <param name="errorCode">A code for this type of error. If left default null or string.Empty the value (int)HttpStatusCode.BadRequest will be used</param>
        /// <returns></returns>
        public static HttpResponseMessage CreateInternalServerErrorResponseMessage<T>(this HttpRequestMessage request, string errorMessage, T errorCode)
        {
            return CreateInternalServerErrorResponseMessage(request, errorMessage, null, errorCode == null ? null : errorCode.ToString());
        }

        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="errorMessage">Message</param>
        /// <param name="errorTag">Unique tag to identify the error. If left as defualt null or string.Empty one will be generated</param>
        /// <param name="errorCode">A code for this type of error. If left default null or string.Empty the value (int)HttpStatusCode.BadRequest will be used</param>
        /// <returns></returns>
        public static HttpResponseMessage CreateBadRequestErrorResponseMessage(this HttpRequestMessage request, string errorMessage, string errorTag = null, string errorCode = null)
        {
            var error = GetErrorMessage(request, errorMessage, HttpStatusCode.BadRequest, errorTag, errorCode);
            return request.CreateResponse(error.HttpStatusCode, error);
        }

        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="errorMessage">Message</param>
        /// <param name="errorCode">A code for this type of error. If left default null or string.Empty the value (int)HttpStatusCode.BadRequest will be used</param>
        /// <returns></returns>
        public static HttpResponseMessage CreateBadRequestErrorResponseMessage<T>(this HttpRequestMessage request, string errorMessage, T errorCode) 
        {
            return CreateBadRequestErrorResponseMessage(request, errorMessage, null, errorCode == null ? null : errorCode.ToString());
        }

        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="errorMessage">Message</param>
        /// <param name="errorTag">Unique tag to identify the error. If left as defualt null or string.Empty one will be generated</param>
        /// <param name="errorCode">A code for this type of error. If left default null or string.Empty the value (int)HttpStatusCode.Unauthorized will be used</param>
        /// <returns></returns>
        public static HttpResponseMessage CreateUnauthorizedErrorResponseMessage(this HttpRequestMessage request, string errorMessage, string errorTag = null, string errorCode = null)
        {
            var error = GetErrorMessage(request, errorMessage, HttpStatusCode.Unauthorized, errorTag, errorCode);
            return request.CreateResponse(error.HttpStatusCode, error);
        }


        

        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="errorMessage">Message</param>
        /// <param name="errorTag">Unique tag to identify the error. If left as defualt null or string.Empty one will be generated</param>
        /// <param name="errorCode">A code for this type of error. If left default null or string.Empty the value (int)HttpStatusCode.NotFound will be used</param>
        /// <returns></returns>
        public static HttpResponseMessage CreateNotFoundResponseMessage(this HttpRequestMessage request, string errorMessage, string errorTag = null, string errorCode = null)
        {
            var error = GetErrorMessage(request, errorMessage, HttpStatusCode.NotFound, errorTag, errorCode);
            return request.CreateResponse(error.HttpStatusCode, error);
        }


        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="errorMessage">Message</param>
        /// <param name="errorCode">A code for this type of error. If left default null or string.Empty the value (int)HttpStatusCode.BadRequest will be used</param>
        /// <returns></returns>
        public static HttpResponseMessage CreateNotFoundResponseMessage<T>(this HttpRequestMessage request, string errorMessage, T errorCode)
        {
            return CreateNotFoundResponseMessage(request, errorMessage, null, errorCode == null ? null : errorCode.ToString());
        }

        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="errorMessage">Message</param>
        /// <param name="errorTag">Unique tag to identify the error. If left as defualt null or string.Empty one will be generated</param>
        /// <param name="errorCode">A code for this type of error. If left default null or string.Empty the value (int)HttpStatusCode.NotAcceptable will be used</param>
        /// <returns></returns>
        public static HttpResponseMessage CreateNotAcceptableResponseMessage(this HttpRequestMessage request, string errorMessage, string errorTag = null, string errorCode = null)
        {
            var error = GetErrorMessage(request, errorMessage, HttpStatusCode.NotAcceptable, errorTag, errorCode);
            return request.CreateResponse(error.HttpStatusCode, error, GlobalConfiguration.Configuration);
        }
        
        public static HttpResponseMessage CreateOkResponseMessage<T>(this HttpRequestMessage request, T value) where T : class
        {
            return request.CreateResponse(HttpStatusCode.OK, value);
        }

        public static HttpResponseMessage CreateCreatedResponseMessage<T>(this HttpRequestMessage request, T value) where T : class
        {
            var response = request.CreateResponse(HttpStatusCode.Created, value);
            //TODO: Should be set to the url of the newly created object response.Headers.Location = ???
            return response;
        }

        public static HttpResponseMessage CreateNoContentResponseMessage(this HttpRequestMessage request)
        {
            return request.CreateResponse(HttpStatusCode.NoContent);
        }

        public static HttpResponseMessage CreateAcceptedResponseMessage<T>(this HttpRequestMessage request, T value) where T : class
        {
            return request.CreateResponse(HttpStatusCode.Accepted, value);
        }
        
        public static HttpResponseMessage CreateForbiddenResponseMessage<T>(this HttpRequestMessage request, T value) where T : class
        {
            return request.CreateResponse(HttpStatusCode.Forbidden, value);
        }

        public static string GetIncludeDetailsQueryParameter(this HttpRequestMessage request, string paramName)
        {
            NameValueCollection queryParameters = request.RequestUri.ParseQueryString();
            string details = string.Empty;
            if (queryParameters.HasKey(paramName))
            {
                details = queryParameters[paramName];
            }
            return details;
        }

        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="errorMessage">Message</param>
        /// <param name="httpStatusCode">The status code to return, ands to set at 'ErrorCode' on the returned <see cref="Error"/>-object</param>
        /// <param name="errorTag">Unique tag to identify the error. If left as defualt null or string.Empty one will be generated</param>
        /// <param name="errorCode">A code for this type of error. If left default null or string.Empty the value (int)<see cref="httpStatusCode"/> will be used</param>
        /// <returns></returns>
        private static Error GetErrorMessage(this HttpRequestMessage request, string errorMessage, HttpStatusCode httpStatusCode, string errorTag = null, string errorCode = null)
        {
            if(string.IsNullOrEmpty(errorTag))
            {
                errorTag = ErrorTagGenerator.NewErrorTag();
            }

            if(string.IsNullOrEmpty(errorCode))
            {
                errorCode = ((int) httpStatusCode).ToString();
            }

            var error = new Error(errorMessage, errorTag, errorCode) { HttpStatusCode = SetStatusToOkIfRequested(request, httpStatusCode), Time = DateTime.Now.ToString()};
            return error;
        }

        private static HttpStatusCode SetStatusToOkIfRequested(this HttpRequestMessage request, HttpStatusCode httpStatusCode)
        {
            var queryValues = request.RequestUri.ParseQueryString();
            if (queryValues.HasKey("enforceokstatus"))
            {
                var enforceOkStatus = queryValues["enforceokstatus"].ToLower() == "true";
                if (enforceOkStatus)
                {
                    return HttpStatusCode.OK;
                }
            }
            return httpStatusCode;
        }
    }
}