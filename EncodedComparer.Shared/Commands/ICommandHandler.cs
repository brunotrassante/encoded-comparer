using System.Threading.Tasks;

namespace EncodedComparer.Shared.Commands
{
    public interface ICommandHandler<T>
        where T : ICommand
    {
        Task<ICommandResult> Handle(T command);
    }
}