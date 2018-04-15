using EncodedComparer.Shared.Notifications;
using static System.String;
using static System.Text.RegularExpressions.Regex;

namespace EncodedComparer.Domain.Entities
{
    public class Base64Data : Notifiable
    {
        public int Id { get; private set; }
        public string Data { get; private set; }

        public Base64Data(int id, string data)
        {
            if (id < 1 || id > 9999)
                AddNotification(nameof(id), "Id must be between 1 and 9999");

            if (!IsBase64(data))
                AddNotification(nameof(data), "The informed data is not base64 encoded.");

            Id = id;
            Data = data;
        }

        private bool IsBase64(string stringToTest)
        {
            if (IsNullOrEmpty(stringToTest))
                return false;

            stringToTest = stringToTest.Trim();
            return (stringToTest.Length % 4 == 0) && IsMatch(stringToTest, @"^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{4}|[A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)$");
        }
    }
}
