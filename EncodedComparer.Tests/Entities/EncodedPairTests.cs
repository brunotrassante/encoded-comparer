using EncodedComparer.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EncodedComparer.Tests.Entities
{
    [TestClass]
    public class EncodedPairTests
    {
        private const string ValidBase64 = "ew0KICAgICJnbG9zc2FyeSI6IHsNCiAgICAgICAgInRpdGxlIjogImV4YW1wbGUgZ2xvc3NhcnkiDQogICAgIH0NCn0=";
        private const string ValidSameSizeDifferentBase64 = "1234ICAgICJnbG9zc2FyeSI6IHsNCiAgICAgICAgInRpdGxlIjogImV4YW1wbGUgZ2xvc3NhcnkiDQogICAgIH0NCn0=";
        private const string SmallerValidBase64 = "ICAgICJnbG9zc2FyeSI6IHsNCiAgICAgICAgInRpdGxlIjogImV4YW1wbGUgZ2xvc3NhcnkiDQogICAgIH0NCn0=";
        private readonly Base64Data _validBase64Data = new Base64Data(1, ValidBase64);
        private readonly Base64Data _validSameSizeDifferentBase64Data = new Base64Data(1, ValidSameSizeDifferentBase64);
        private readonly Base64Data _smallerValidBase64Data = new Base64Data(1, SmallerValidBase64);


        [TestMethod]
        public void ShouldReturnFalseWhenLeftAndRigthAreNotSameSize()
        {
            var encodedPair = new EncodedPair(_validBase64Data, _smallerValidBase64Data);

            Assert.IsFalse(encodedPair.AreSameSize);
        }

        [TestMethod]
        public void ShouldReturnTrueWhenLeftAndRigthAreSameSize()
        {
            var encodedPair = new EncodedPair(_validBase64Data, _validBase64Data);

            Assert.IsTrue(encodedPair.AreSameSize);
        }

        [TestMethod]
        public void ShouldReturnFalseWhenLeftAndRigthAreNotTheSame()
        {
            var encodedPair = new EncodedPair(_validBase64Data, _validSameSizeDifferentBase64Data);

            Assert.IsFalse(encodedPair.AreExactlyEquals);
        }

        [TestMethod]
        public void ShouldReturnTrueWhenLeftAndRigthAreTheSame()
        {
            var encodedPair = new EncodedPair(_validBase64Data, _validBase64Data);

            Assert.IsTrue(encodedPair.AreExactlyEquals);
        }
    }
}
