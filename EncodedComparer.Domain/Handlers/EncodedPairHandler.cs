using EncodedComparer.Domain.Commands;
using EncodedComparer.Domain.Commands.Results;
using EncodedComparer.Domain.Entities;
using EncodedComparer.Domain.Repository;
using EncodedComparer.Shared.Commands;
using EncodedComparer.Shared.Notifications;
using System.Linq;
using System.Threading.Tasks;
using static System.String;

namespace EncodedComparer.Domain.Handlers
{
    public class EncodedPairHandler :
        Notifiable,
        ICommandHandler<SetLeftCommand>,
        ICommandHandler<SetRightCommand>,
        ICommandHandler<FindDifferencesCommand>,
        ICommandHandler<DeleteByIdCommand>
    {
        private const string GenericValidationErrorMessa = "Some validation errors occurred. See the notifications list.";
        private IEncodedPairRepository _repository;

        public EncodedPairHandler(IEncodedPairRepository repository)
        {
            _repository = repository;
        }

        public async Task<ICommandResult> Handle(SetLeftCommand command)
        {
            var alreadyExists = _repository.LeftExists(command.Id);

            var encodedData = new Base64Data(command.Id, command.Base64EncodedData);
            AddNotifications(encodedData.Notifications);

            if (await alreadyExists)
                AddNotification(nameof(command.Id), "There is already a Left data associated to this ID");

            if (!IsValid)
                return new SimpleResult(false, GenericValidationErrorMessa);

            await _repository.CreateLeft(encodedData);
            return new SimpleResult(true, "Left data was successfully added.");
        }

        public async Task<ICommandResult> Handle(SetRightCommand command)
        {
            var alreadyExists = _repository.RightExists(command.Id);

            var encodedData = new Base64Data(command.Id, command.Base64EncodedData);
            AddNotifications(encodedData.Notifications);

            if (await alreadyExists)
                AddNotification(nameof(command.Id), "There is already a Right data associated to this ID");

            if (!IsValid)
                return new SimpleResult(false, GenericValidationErrorMessa);

            await _repository.CreateRight(encodedData);
            return new SimpleResult(true, "Right data was successfully added.");
        }

        public async Task<ICommandResult> Handle(FindDifferencesCommand command)
        {
            var leftRightPair = await _repository.GetLeftRightById(command.Id);

            if (leftRightPair == null || IsNullOrEmpty(leftRightPair.Left) || IsNullOrEmpty(leftRightPair.Right))
                AddNotification(nameof(command.Id), "Missing Left or Right data associated to this ID");

            if (!IsValid)
                return new FindDifferencesResult(false, "Some validation errors occurred. See the notifications list.");

            var encodedPair = new EncodedPair(new Base64Data(leftRightPair.Id, leftRightPair.Left), new Base64Data(leftRightPair.Id, leftRightPair.Right));

            if (!encodedPair.AreSameSize)
                return new FindDifferencesResult(true, "Left and Right are not same size.");

            if (encodedPair.AreExactlyEquals)
                return new FindDifferencesResult(true, "Left and Right are exactly the same.");

            var differences = encodedPair.FindDifferences();
            return new FindDifferencesResult(true, "Same size but have differences. See the differences list.", differences);
        }

        public async Task<ICommandResult> Handle(DeleteByIdCommand command)
        {
            await _repository.DeleteById(command.Id);
            return new SimpleResult(true, $"Left and Right of Id {command.Id} were successfuly deleted.");
        }
    }
}
