using EncodedComparer.Domain.Commands;
using EncodedComparer.Domain.Entities;
using EncodedComparer.Domain.Handlers;
using EncodedComparer.Domain.Queries;
using EncodedComparer.Domain.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace EncodedComparer.Tests.Entities
{
    [TestClass]
    public class EncodedPairHandlerTests
    {
        private EncodedPairHandler _handler;
        private const int AlreadyTakenId = 1;
        private const int NotTakenId = 2;
        private const int HasOnlyLeftId = 3;
        private const int HasOnlyRightId = 4;
        private const int HasEqualsRightAndLeftId = 5;
        private const int HasDiffSizeRightAndLeftId = 6;
        private const int HasSameSizeDiffContentRightAndLeftId = 7;
        private const string SimpleBase64String = "ew0K";
        private const string DifferentSizeBase64String = "ew0yew0y";
        private const string SameSizeDiffContentBase64String = "ew0y";

        public EncodedPairHandlerTests()
        {
            var mockRepo = new Mock<IEncodedPairRepository>();
            mockRepo.Setup(repo => repo.LeftExists(AlreadyTakenId)).ReturnsAsync(true);
            mockRepo.Setup(repo => repo.LeftExists(NotTakenId)).ReturnsAsync(false);
            mockRepo.Setup(repo => repo.RightExists(AlreadyTakenId)).ReturnsAsync(true);
            mockRepo.Setup(repo => repo.RightExists(NotTakenId)).ReturnsAsync(false);

            mockRepo.Setup(repo => repo.GetLeftRightById(HasOnlyLeftId)).ReturnsAsync(new LeftRightSameIdQuery() { Id = HasOnlyLeftId, Left = SimpleBase64String });
            mockRepo.Setup(repo => repo.GetLeftRightById(HasOnlyRightId)).ReturnsAsync(new LeftRightSameIdQuery() { Id = HasOnlyRightId, Right = SimpleBase64String });
            mockRepo.Setup(repo => repo.GetLeftRightById(HasEqualsRightAndLeftId)).ReturnsAsync(new LeftRightSameIdQuery() { Id = HasOnlyRightId, Left = SimpleBase64String, Right = SimpleBase64String });
            mockRepo.Setup(repo => repo.GetLeftRightById(HasDiffSizeRightAndLeftId)).ReturnsAsync(new LeftRightSameIdQuery() { Id = HasOnlyRightId, Left = SimpleBase64String, Right = DifferentSizeBase64String });
            mockRepo.Setup(repo => repo.GetLeftRightById(HasSameSizeDiffContentRightAndLeftId)).ReturnsAsync(new LeftRightSameIdQuery() { Id = HasOnlyRightId, Left = SimpleBase64String, Right = SameSizeDiffContentBase64String });


            _handler = new EncodedPairHandler(mockRepo.Object);
        }

        [TestMethod]
        public async Task ShouldReturnNotificationWhenLeftAlreadyExists()
        {
            var setLeftCommand = new SetLeftCommand() { Id = AlreadyTakenId, Base64EncodedData = SimpleBase64String };

            var result = await _handler.Handle(setLeftCommand);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(1, _handler.Notifications.Count);
        }

        [TestMethod]
        public async Task ShouldReturnSuccessAddingLeftWhenInputsAreValid()
        {
            var setLeftCommand = new SetLeftCommand() { Id = NotTakenId, Base64EncodedData = SimpleBase64String };

            var result = await _handler.Handle(setLeftCommand);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(0, _handler.Notifications.Count);
        }

        [TestMethod]
        public async Task ShouldReturnNotificationWhenRightAlreadyExists()
        {
            var setRightCommand = new SetRightCommand() { Id = AlreadyTakenId, Base64EncodedData = SimpleBase64String };

            var result = await _handler.Handle(setRightCommand);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(1, _handler.Notifications.Count);
        }


        [TestMethod]
        public async Task ShouldReturnSuccessAddingRightWhenInputsAreValid()
        {
            var setLeftCommand = new SetRightCommand() { Id = NotTakenId, Base64EncodedData = SimpleBase64String };

            var result = await _handler.Handle(setLeftCommand);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(0, _handler.Notifications.Count);
        }

        [TestMethod]
        public async Task ShouldReturnNotificationFinddingDifferencesWhenOnlyRightExists()
        {
            var findDifferencesCommand = new FindDifferencesCommand() { Id = HasOnlyRightId };

            var result = await _handler.Handle(findDifferencesCommand);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(1, _handler.Notifications.Count);
        }

        [TestMethod]
        public async Task ShouldReturnNotificationFinddingDifferencesWhenOnlyLeftExists()
        {
            var findDifferencesCommand = new FindDifferencesCommand() { Id = HasOnlyLeftId };

            var result = await _handler.Handle(findDifferencesCommand);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(1, _handler.Notifications.Count);
        }

        [TestMethod]
        public async Task ShouldReturnSuccessWithNoDiffListWhenLeftRightAreTheSame()
        {
            var findDifferencesCommand = new FindDifferencesCommand() { Id = HasEqualsRightAndLeftId };

            var result = await _handler.Handle(findDifferencesCommand);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Data);
            Assert.AreEqual(0, _handler.Notifications.Count);
        }

        [TestMethod]
        public async Task ShouldReturnSuccessWithNoDiffListWhenLeftRightHaveDifferentSizes()
        {
            var findDifferencesCommand = new FindDifferencesCommand() { Id = HasDiffSizeRightAndLeftId };

            var result = await _handler.Handle(findDifferencesCommand);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Data);
            Assert.AreEqual(0, _handler.Notifications.Count);
        }

        [TestMethod]
        public async Task ShouldReturnSuccessWithDiffListPopulatedWhenLeftRightHaveSameSizeButDifferentContent()
        {
            var findDifferencesCommand = new FindDifferencesCommand() { Id = HasSameSizeDiffContentRightAndLeftId };

            var result = await _handler.Handle(findDifferencesCommand);

            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(0, _handler.Notifications.Count);
        }
    }
}
