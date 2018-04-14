using EncodedComparer.Shared.Commands;

namespace EncodedComparer.Domain.Commands
{
    public class SetRightCommand : ICommand
    {
        public int Id { get; set; }
        public string Base64EncodedData { get; set; }
    }
}
