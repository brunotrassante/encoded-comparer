using EncodedComparer.Domain.Entities;
using EncodedComparer.Shared.Commands;
using EncodedComparer.Shared.Notifications;
using System.Collections.Generic;

namespace EncodedComparer.Domain.Commands.Results
{
    public class FindDifferencesResult : ICommandResult
    {
        public FindDifferencesResult(bool success, string message, IEnumerable<DifferenceInfo> data)
        {
            Success = success;
            Message = message;
            Data = data;
        }
        public FindDifferencesResult(bool success, string message)
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
