using EncodedComparer.Shared.Commands;

namespace EncodedComparer.Domain.Commands
{
    public class DeleteByIdCommand : ICommand
    {
        public int Id { get; set; }
    }
}
