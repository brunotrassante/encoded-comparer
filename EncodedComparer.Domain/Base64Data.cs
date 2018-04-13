using EncodedComparer.Shared.Notifications;
using System.Text.RegularExpressions;

namespace EncodedComparer.Domain
{
    public class Base64Data : Notifiable
    {
        public string Data { get; private set; }

        public Base64Data(string data)
        {
            if (!IsBase64(data))
                AddNotification(nameof(data), "The informed data is not base64 encoded.");

            Data = data;
        }

        private bool IsBase64(string stringToTest)
        {
            if (string.IsNullOrEmpty(stringToTest))
                return false;

            stringToTest = stringToTest.Trim();
            return (stringToTest.Length % 4 == 0) && Regex.IsMatch(stringToTest, @"^[a-zA-Z0-9\+/]*={0,3}$");
        }
    }
}
