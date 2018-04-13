using EncodedComparer.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EncodedComparer.Tests.Domain
{
    [TestClass]
    public class Base64DataTests
    {
        private const string NotMultipleOfFourInput = "ew0KICAgICJnbG9zc2FyeSI6IHsNCiAgICAgICAgInRpdGxlIjogImV4YW1wbGUgZ2xvc3NhcnkiDQogICAgICn0=";
        private const string InvalidCharInput = "&w0KICAgICJnbG9zc2FyeSI6IHsNCiAgICAgICAgInRpdGxlIjogImV4YW1wbGUgZ2xvc3NhcnkiDQogICAgIH0NCn0=";
        private const string ValidBase64Input = "ew0KICAgICJnbG9zc2FyeSI6IHsNCiAgICAgICAgInRpdGxlIjogImV4YW1wbGUgZ2xvc3NhcnkiDQogICAgIH0NCn0=";

        [TestMethod]
        public void ShouldReturnNotificationWhenInputIsNotValidBase64()
        {
            var notMultipleOfFourData = new Base64Data(NotMultipleOfFourInput);
            Assert.IsFalse(notMultipleOfFourData.IsValid);
            Assert.AreEqual(1, notMultipleOfFourData.Notifications.Count);

            var invalidCharData = new Base64Data(InvalidCharInput);
            Assert.IsFalse(invalidCharData.IsValid);
            Assert.AreEqual(1, invalidCharData.Notifications.Count);

            var nullData = new Base64Data(null);
            Assert.IsFalse(invalidCharData.IsValid);
            Assert.AreEqual(1, invalidCharData.Notifications.Count);
        }

        [TestMethod]
        public void ShouldBeValidWhenInputIsValidBase64()
        {
            var validData = new Base64Data(ValidBase64Input);
            Assert.IsTrue(validData.IsValid);
        }
    }
}
