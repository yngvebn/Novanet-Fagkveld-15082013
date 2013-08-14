using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using NUnit.Framework;
using Rikstoto.WebAPI.MessageHandlers;

namespace Rikstoto.WebAPI.UnitTests.MessageHandlers
{
    [TestFixture]
    public class NotAcceptableMessageHandlerTests : MessageHandlerTester
    {
        [Test]
        public void ShouldReturnNotAcceptableWhenMediaTypeIsNotSupported()
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://pleasework.now/foo/bar");
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("weird/type"));

            var notAcceptableMessageHandler = new NotAcceptableMessageHandler();
            var response = ExecuteRequest(notAcceptableMessageHandler, requestMessage);

            Assert.AreEqual(HttpStatusCode.NotAcceptable, response.StatusCode);
        }

        [Test]
        public void ShouldReturnOkWhenMediaTypeIsAccepted()
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://pleasework.now/foo/bar");
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));

            var notAcceptableMessageHandler = new NotAcceptableMessageHandler();
            var response = ExecuteRequest(notAcceptableMessageHandler, requestMessage);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public void ShouldReturnOkWhenTypeGroupIsAccepted()
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://pleasework.now/foo/bar");
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/*"));

            var notAcceptableMessageHandler = new NotAcceptableMessageHandler();
            var response = ExecuteRequest(notAcceptableMessageHandler, requestMessage);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public void ShouldReturnNotAcceptableWhenTypeGroupIsNotAccepted()
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://pleasework.now/foo/bar");
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("weird/*"));

            var notAcceptableMessageHandler = new NotAcceptableMessageHandler();
            var response = ExecuteRequest(notAcceptableMessageHandler, requestMessage);

            Assert.AreEqual(HttpStatusCode.NotAcceptable, response.StatusCode);
        }

        [Test]
        public void ShouldReturnOkWhenAllMediaTypesIsAccepted()
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://pleasework.now/foo/bar");
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));

            var notAcceptableMessageHandler = new NotAcceptableMessageHandler();
            var response = ExecuteRequest(notAcceptableMessageHandler, requestMessage);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public void ShouldReturnOkWhenOneOfTheMediaTypesIsAccepted()
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://pleasework.now/foo/bar");
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("weird/type"));
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));

            var notAcceptableMessageHandler = new NotAcceptableMessageHandler();
            var response = ExecuteRequest(notAcceptableMessageHandler, requestMessage);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public void ShouldReturnOkWhenOneOfTheMediaTypesIsAAcceptedTypeGroup()
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://pleasework.now/foo/bar");
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("weird/type"));
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/*"));

            var notAcceptableMessageHandler = new NotAcceptableMessageHandler();
            var response = ExecuteRequest(notAcceptableMessageHandler, requestMessage);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public void ShouldReturnOkWhenOneOfTheMediaTypesIsAllMediaTypes()
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://pleasework.now/foo/bar");
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("weird/type"));
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));

            var notAcceptableMessageHandler = new NotAcceptableMessageHandler();
            var response = ExecuteRequest(notAcceptableMessageHandler, requestMessage);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
