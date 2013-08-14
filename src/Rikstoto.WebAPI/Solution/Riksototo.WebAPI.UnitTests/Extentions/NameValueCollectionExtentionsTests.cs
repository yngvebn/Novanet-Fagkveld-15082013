using System.Collections.Specialized;
using NUnit.Framework;
using Rikstoto.WebAPI.Extentions;

namespace Rikstoto.WebAPI.UnitTests.Extentions
{
    [TestFixture]
    public class NameValueCollectionExtentionsTests
    {
        [Test]
        public void HasKeyKeyExistsReturnsTrue()
        {
            var collection = new NameValueCollection {{"Exists", "whatever"}};
            Assert.IsTrue(NameValueCollectionExtentions.HasKey(collection, "Exists"));
        }

        [Test]
        public void HasKeyKeyDoesNotExistsReturnsFalse()
        {
            var collection = new NameValueCollection { { "exists", "whatever" } };
            Assert.IsFalse(NameValueCollectionExtentions.HasKey(collection, "DoesNotExist"));
        }
    }
}
