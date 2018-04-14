using EncodedComparer.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EncodedComparer.Tests.Entities
{
    [TestClass]
    public class Base64DataTests
    {
        private const string NotMultipleOfFourInput = "ew0KICAgICJnbG9zc2FyeSI6IHsNCiAgICAgICAgInRpdGxlIjogImV4YW1wbGUgZ2xvc3NhcnkiDQogICAgICn0=";
        private const string InvalidCharInput = "&w0KICAgICJnbG9zc2FyeSI6IHsNCiAgICAgICAgInRpdGxlIjogImV4YW1wbGUgZ2xvc3NhcnkiDQogICAgIH0NCn0=";
        private const string ValidBase64Input = "ew0KICAgICJnbG9zc2FyeSI6IHsNCiAgICAgICAgInRpdGxlIjogImV4YW1wbGUgZ2xvc3NhcnkiDQogICAgIH0NCn0=";

        [TestMethod]
        public void ShouldReturnNotificationWhenIdIsInvalid()
        {
            var tooLowIdEncodedPair = new Base64Data(0, ValidBase64Input);
            Assert.IsFalse(tooLowIdEncodedPair.IsValid);
            Assert.AreEqual(1, tooLowIdEncodedPair.Notifications.Count);

            var tooHighIdEncodedPair = new Base64Data(10000, ValidBase64Input);
            Assert.IsFalse(tooHighIdEncodedPair.IsValid);
            Assert.AreEqual(1, tooHighIdEncodedPair.Notifications.Count);
        }

        [TestMethod]
        public void ShouldBeValidWhenIdIsValid()
        {
            var tooLowIdEncodedPair = new Base64Data(1, ValidBase64Input);
            Assert.IsTrue(tooLowIdEncodedPair.IsValid);
            Assert.AreEqual(0, tooLowIdEncodedPair.Notifications.Count);

            var tooHighIdEncodedPair = new Base64Data(9999, ValidBase64Input);
            Assert.IsTrue(tooLowIdEncodedPair.IsValid);
            Assert.AreEqual(0, tooLowIdEncodedPair.Notifications.Count);
        }

        [TestMethod]
        public void ShouldReturnNotificationWhenInputIsNotValidBase64()
        {
            var notMultipleOfFourData = new Base64Data(1, NotMultipleOfFourInput);
            Assert.IsFalse(notMultipleOfFourData.IsValid);
            Assert.AreEqual(1, notMultipleOfFourData.Notifications.Count);

            var invalidCharData = new Base64Data(1, InvalidCharInput);
            Assert.IsFalse(invalidCharData.IsValid);
            Assert.AreEqual(1, invalidCharData.Notifications.Count);

            var nullData = new Base64Data(1, null);
            Assert.IsFalse(invalidCharData.IsValid);
            Assert.AreEqual(1, invalidCharData.Notifications.Count);
        }

        [TestMethod]
        public void ShouldBeValidWhenInputIsValidBase64()
        {
            var validData = new Base64Data(1, ValidBase64Input);
            Assert.IsTrue(validData.IsValid);
        }
    }
}
