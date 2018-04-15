using EncodedComparer.Domain.Commands;
using EncodedComparer.Domain.Entities;
using EncodedComparer.Domain.Handlers;
using EncodedComparer.Domain.Queries;
using EncodedComparer.Domain.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EncodedComparer.Tests.Unit.Entities
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
        private const int OneCharDiffId = 7;
        private const int TwoDiffsFourCharsAndOneCharId= 8;
        private const int FirstCharChangedId = 9;
        private const int LastCharChangedId = 10;
        private const int AllChangedId = 11;

        private const string SimpleBase64String = "ew0K";
        private const string DifferentSizeBase64String = "ew0yew0y";
        private const string SameSizeDiffContentBase64String = "ew0y";
        private const string OriginalString               = "ew0KIm5hbWUiOiJKb2huIiwNCiJhZ2UiOjMwLA0KImNhcnMiOlsgIkZvcmQiLCAiQk1XIiwgIkZpYXQiIF0NCn0=";
        private const string OneCharDiffString            = "ew0KIm5hbWUiOiJKb2huIiwNCiJhZ2UiOjMxLA0KImNhcnMiOlsgIkZvcmQiLCAiQk1XIiwgIkZpYXQiIF0NCn0=";
        private const string FourCharAndOneCharDiffString = "ew0KIm5hbWUiOiJNYXJ5IiwNCiJhZ2UiOjMwLA0KImNhcnMiOlsgIkZvcmQiLCAiQk1XIiwgIk5pYXQiIF0NCn0=";
        private const string FirstCharChangedString       = "aw0KIm5hbWUiOiJKb2huIiwNCiJhZ2UiOjMwLA0KImNhcnMiOlsgIkZvcmQiLCAiQk1XIiwgIkZpYXQiIF0NCn0=";
        private const string LastCharChangedString        = "aw0KIm5hbWUiOiJKb2huIiwNCiJhZ2UiOjMwLA0KImNhcnMiOlsgIkZvcmQiLCAiQk1XIiwgIkZpYXQiIF0NCn0=";
        private const string AllChangedString             = "9999999999999999999999999999999999999999999999999999999999999999999999999999999999999999";
        
        public EncodedPairHandlerTests()
        {
            var mockRepo = new Mock<IEncodedPairRepository>();
            mockRepo.Setup(repo => repo.LeftExists(AlreadyTakenId)).ReturnsAsync(true);
            mockRepo.Setup(repo => repo.LeftExists(NotTakenId)).ReturnsAsync(false);
            mockRepo.Setup(repo => repo.RightExists(AlreadyTakenId)).ReturnsAsync(true);
            mockRepo.Setup(repo => repo.RightExists(NotTakenId)).ReturnsAsync(false);

            mockRepo.Setup(repo => repo.GetLeftRightById(HasOnlyLeftId)).ReturnsAsync(new LeftRightSameIdQuery() { Left = SimpleBase64String });
            mockRepo.Setup(repo => repo.GetLeftRightById(HasOnlyRightId)).ReturnsAsync(new LeftRightSameIdQuery() { Id = HasOnlyRightId, Right = SimpleBase64String });
            mockRepo.Setup(repo => repo.GetLeftRightById(HasEqualsRightAndLeftId)).ReturnsAsync(new LeftRightSameIdQuery() { Id = HasOnlyRightId, Left = SimpleBase64String, Right = SimpleBase64String });
            mockRepo.Setup(repo => repo.GetLeftRightById(HasDiffSizeRightAndLeftId)).ReturnsAsync(new LeftRightSameIdQuery() { Id = HasOnlyRightId, Left = SimpleBase64String, Right = DifferentSizeBase64String });
            mockRepo.Setup(repo => repo.GetLeftRightById(OneCharDiffId)).ReturnsAsync(new LeftRightSameIdQuery() { Id = OneCharDiffId, Left = OriginalString, Right = OneCharDiffString });
            mockRepo.Setup(repo => repo.GetLeftRightById(TwoDiffsFourCharsAndOneCharId)).ReturnsAsync(new LeftRightSameIdQuery() { Id = TwoDiffsFourCharsAndOneCharId, Left = OriginalString, Right = FourCharAndOneCharDiffString });
            mockRepo.Setup(repo => repo.GetLeftRightById(FirstCharChangedId)).ReturnsAsync(new LeftRightSameIdQuery() { Id = FirstCharChangedId, Left = OriginalString, Right = FirstCharChangedString });
            mockRepo.Setup(repo => repo.GetLeftRightById(LastCharChangedId)).ReturnsAsync(new LeftRightSameIdQuery() { Id = LastCharChangedId, Left = OriginalString, Right = LastCharChangedString });
            mockRepo.Setup(repo => repo.GetLeftRightById(AllChangedId)).ReturnsAsync(new LeftRightSameIdQuery() { Id = AllChangedId, Left = AllChangedString, Right = OriginalString });

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
        public async Task ShouldReturnSuccessWithOneDiffWhenOnlyOneCharIsChanged()
        {
            var findDifferencesCommand = new FindDifferencesCommand() { Id = OneCharDiffId };

            var result = await _handler.Handle(findDifferencesCommand);
            var differences = ((IEnumerable<DifferenceInfo>)result.Data).ToList();

            Assert.IsTrue(result.Success);
            Assert.IsTrue(differences.Count == 1);
            Assert.IsTrue(differences.First().Length == 1);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(0, _handler.Notifications.Count);
        }

        [TestMethod]
        public async Task ShouldReturnSuccessWithOneDiffWhenOnlyTwoPartsAreChanged()
        {
            var findDifferencesCommand = new FindDifferencesCommand() { Id = TwoDiffsFourCharsAndOneCharId };

            var result = await _handler.Handle(findDifferencesCommand);
            var differences = ((IEnumerable<DifferenceInfo>)result.Data).ToList();

            Assert.IsTrue(result.Success);
            Assert.IsTrue(differences.Count == 2);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(0, _handler.Notifications.Count);
        }

        [TestMethod]
        public async Task ShouldReturnSuccessWithOneDiffWhenFirstCharIsChanged()
        {
            var findDifferencesCommand = new FindDifferencesCommand() { Id = FirstCharChangedId };

            var result = await _handler.Handle(findDifferencesCommand);
            var differences = ((IEnumerable<DifferenceInfo>)result.Data).ToList();

            Assert.IsTrue(result.Success);
            Assert.IsTrue(differences.Count == 1);
            Assert.IsTrue(differences.First().Length == 1);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(0, _handler.Notifications.Count);
        }

        [TestMethod]
        public async Task ShouldReturnSuccessWithOneDiffWhenLastCharIsChanged()
        {
            var findDifferencesCommand = new FindDifferencesCommand() { Id = LastCharChangedId };

            var result = await _handler.Handle(findDifferencesCommand);
            var differences = ((IEnumerable<DifferenceInfo>)result.Data).ToList();

            Assert.IsTrue(result.Success);
            Assert.IsTrue(differences.Count == 1);
            Assert.IsTrue(differences.First().Length == 1);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(0, _handler.Notifications.Count);
        }

        [TestMethod]
        public async Task ShouldReturnSuccessWithOneBigDiffWhenAllCharsAreChanged()
        {
            var findDifferencesCommand = new FindDifferencesCommand() { Id = AllChangedId };

            var result = await _handler.Handle(findDifferencesCommand);
            var differences = ((IEnumerable<DifferenceInfo>)result.Data).ToList();

            Assert.IsTrue(result.Success);
            Assert.IsTrue(differences.Count == 1);
            Assert.IsTrue(differences.First().Length == AllChangedString.Length);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(0, _handler.Notifications.Count);
        }
    }
}
