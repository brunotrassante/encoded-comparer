using EncodedComparer.Shared.Commands;

namespace EncodedComparer.Domain.Commands
{
    public class FindDifferencesCommand : ICommand
    {
        public int Id { get; set; }
    }
}
