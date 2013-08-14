using System;
using System.Web.Http.ModelBinding;
using NUnit.Framework;
using Rikstoto.WebAPI.Extentions;

namespace Rikstoto.WebAPI.UnitTests.Extentions
{
    [TestFixture]
    public class ModelErrorExtentionsTests
    {
        [Test]
        public void GetErrorMessage_IsNull_ShouldReturnEmptyString()
        {
            ModelError error = null;
            Assert.AreEqual(string.Empty, error.GetErrorMessage());
        }

        [Test]
        public void GetErrorMessage_ErrorMessageSet_ShouldReturnErrorMessage()
        {
            ModelError error = new ModelError("errormessage");
            Assert.AreEqual("errormessage", error.GetErrorMessage());
        }

        [Test]
        public void GetErrorMessage_NullErrorMessageAndNoExceptionSet_ShouldReturnEmptyString()
        {
            ModelError error = new ModelError((string)null);
            Assert.AreEqual(string.Empty, error.GetErrorMessage());
        }

        [Test]
        public void GetErrorMessage_EmptyErrorMessageAndNoExceptionSet_ShouldReturnEmptyString()
        {
            ModelError error = new ModelError("    ");
            Assert.AreEqual(string.Empty, error.GetErrorMessage());
        }

        [Test]
        public void GetErrorMessage_NullOrEmptyAndExceptionSet_ShouldReturnExceptionMessage()
        {
            ModelError error = new ModelError(new Exception("exceptionmessage"));
            Assert.AreEqual("exceptionmessage", error.GetErrorMessage());
        }
    }
}
