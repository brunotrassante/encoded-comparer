using EncodedComparer.Shared.Commands;

namespace EncodedComparer.Domain.Commands.Results
{
    public class SimpleResult : ICommandResult
    {
        public SimpleResult(bool success, string message)
        {
            Success = success;
            Message = message;
            Data = null;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
