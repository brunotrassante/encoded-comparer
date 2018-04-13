using EncodedComparer.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EncodedComparer.Tests.Domain
{
    [TestClass]
    public class EncodedPairTests
    {
        private const string ValidBase64Input  = "ew0KICAgICJnbG9zc2FyeSI6IHsNCiAgICAgICAgInRpdGxlIjogImV4YW1wbGUgZ2xvc3NhcnkiDQogICAgIH0NCn0=";
        private const string ValidBase64Input2 = "1234ICAgICJnbG9zc2FyeSI6IHsNCiAgICAgICAgInRpdGxlIjogImV4YW1wbGUgZ2xvc3NhcnkiDQogICAgIH0NCn0=";
        private const string SmallerValidBase64Input = "ICAgICJnbG9zc2FyeSI6IHsNCiAgICAgICAgInRpdGxlIjogImV4YW1wbGUgZ2xvc3NhcnkiDQogICAgIH0NCn0=";

        [TestMethod]
        public void ShouldReturnNotificationWhenIdIsInvalid()
        {
            var tooLowIdEncodedPair = new EncodedPair(0);
            Assert.IsFalse(tooLowIdEncodedPair.IsValid);
            Assert.AreEqual(1, tooLowIdEncodedPair.Notifications.Count);

            var tooHighIdEncodedPair = new EncodedPair(10000);
            Assert.IsFalse(tooHighIdEncodedPair.IsValid);
            Assert.AreEqual(1, tooHighIdEncodedPair.Notifications.Count);
        }

        [TestMethod]
        public void ShouldBeValidWhenIdIsValid()
        {
            var tooLowIdEncodedPair = new EncodedPair(1);
            Assert.IsTrue(tooLowIdEncodedPair.IsValid);
            Assert.AreEqual(0, tooLowIdEncodedPair.Notifications.Count);

            var tooHighIdEncodedPair = new EncodedPair(9999);
            Assert.IsTrue(tooLowIdEncodedPair.IsValid);
            Assert.AreEqual(0, tooLowIdEncodedPair.Notifications.Count);
        }

        [TestMethod]
        public void ShouldReturnNotificationWhenAddingInvalidDataOnLeft()
        {
            var invalidBase64Data = new Base64Data(null);

            var encodedPair = new EncodedPair(1);
            encodedPair.AddLeft(invalidBase64Data);
            Assert.IsFalse(encodedPair.IsValid);
            Assert.AreEqual(1, encodedPair.Notifications.Count);
            Assert.IsNull(encodedPair.Left);

            encodedPair = new EncodedPair(1);
            encodedPair.AddLeft(null);
            Assert.IsFalse(encodedPair.IsValid);
            Assert.AreEqual(1, encodedPair.Notifications.Count);
            Assert.IsNull(encodedPair.Left);
        }

        [TestMethod]
        public void ShouldReturnNotificationWhenAddingInvalidDataOnRight()
        {
            var invalidBase64Data = new Base64Data(null);
            var encodedPair = new EncodedPair(1);

            encodedPair.AddRight(invalidBase64Data);
            Assert.IsFalse(encodedPair.IsValid);
            Assert.AreEqual(1, encodedPair.Notifications.Count);
            Assert.IsNull(encodedPair.Right);

            encodedPair = new EncodedPair(1);
            encodedPair.AddRight(null);
            Assert.IsFalse(encodedPair.IsValid);
            Assert.AreEqual(1, encodedPair.Notifications.Count);
            Assert.IsNull(encodedPair.Right);
        }

        [TestMethod]
        public void ShouldAddOnLeftWhenDataIsValid()
        {
            var validBase64Data = new Base64Data(ValidBase64Input);

            var encodedPair = new EncodedPair(1);
            encodedPair.AddLeft(validBase64Data);
            Assert.IsTrue(encodedPair.IsValid);
            Assert.IsNotNull(encodedPair.Left);
        }

        [TestMethod]
        public void ShouldAddOnRightWhenDataIsValid()
        {
            var validBase64Data = new Base64Data(ValidBase64Input);

            var encodedPair = new EncodedPair(1);
            encodedPair.AddRight(validBase64Data);
            Assert.IsTrue(encodedPair.IsValid);
            Assert.IsNotNull(encodedPair.Right);
        }

        [TestMethod]
        public void ShouldReturnFalseWhenLeftAndRigthAreNotSameSize()
        {
            var validBase64Data = new Base64Data(ValidBase64Input);
            var smallerValidBase64Data = new Base64Data(SmallerValidBase64Input);
            var encodedPair = new EncodedPair(1);

            encodedPair.AddRight(validBase64Data);
            Assert.IsFalse(encodedPair.AreSameSize());

            encodedPair.AddLeft(smallerValidBase64Data);
            Assert.IsFalse(encodedPair.AreSameSize());
        }

        [TestMethod]
        public void ShouldReturnTrueWhenLeftAndRigthAreSameSize()
        {
            var validBase64Data = new Base64Data(ValidBase64Input);
            var encodedPair = new EncodedPair(1);

            encodedPair.AddRight(validBase64Data);
            encodedPair.AddLeft(validBase64Data);
            Assert.IsTrue(encodedPair.AreSameSize());
        }

        [TestMethod]
        public void ShouldReturnFalseWhenLeftAndRigthAreNotTheSame()
        {
            var validBase64Data = new Base64Data(ValidBase64Input);
            var validBase64Data2 = new Base64Data(ValidBase64Input2);
            var encodedPair = new EncodedPair(1);

            encodedPair.AddRight(validBase64Data);
            encodedPair.AddLeft(validBase64Data2);
            Assert.IsFalse(encodedPair.AreExactlyEquals());
        }

        [TestMethod]
        public void ShouldReturnTrueWhenLeftAndRigthAreTheSame()
        {
            var validBase64Data = new Base64Data(ValidBase64Input);
            var encodedPair = new EncodedPair(1);

            encodedPair.AddRight(validBase64Data);
            encodedPair.AddLeft(validBase64Data);
            Assert.IsTrue(encodedPair.AreExactlyEquals());
        }

        [TestMethod]
        public void ShouldReturnNotificationWhenFindDiffIsCalledWithNoLeftAndRight()
        {
            var encodedPair = new EncodedPair(1);

            var differences = encodedPair.FindDifferences();

            Assert.IsFalse(encodedPair.IsValid);
            Assert.AreEqual(2, encodedPair.Notifications.Count);
            Assert.IsNull(differences);
        }

        [TestMethod]
        public void ShouldReturnNotificationWhenFindDiffIsCalledWithLeftAndRightNotSameSize()
        {
            var validBase64Data = new Base64Data(ValidBase64Input);
            var smallerValidBase64Data = new Base64Data(SmallerValidBase64Input);
            var encodedPair = new EncodedPair(1);

            encodedPair.AddLeft(validBase64Data);
            encodedPair.AddRight(smallerValidBase64Data);

            var differences = encodedPair.FindDifferences();

            Assert.IsFalse(encodedPair.IsValid);
            Assert.AreEqual(1, encodedPair.Notifications.Count);
            Assert.IsNull(differences);
        }
    }
}
