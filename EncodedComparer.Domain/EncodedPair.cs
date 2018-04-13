using EncodedComparer.Shared.Notifications;
using System.Collections;
using System.Collections.Generic;

namespace EncodedComparer.Domain
{
    public class EncodedPair : Notifiable
    {
        public int Id { get; private set; }
        public Base64Data Left { get; private set; }
        public Base64Data Right { get; private set; }

        public EncodedPair(int id)
        {
            if (id < 1 || id > 9999)
                AddNotification(nameof(id), "Id must be between 1 and 9999");

            Id = id;
        }

        public void AddLeft(Base64Data data)
        {
            if (data == null || !data.IsValid)
                AddNotification(nameof(data), "Invalid data cannot be inserted");
            else
                Left = data;
        }

        public void AddRight(Base64Data data)
        {
            if (data == null || !data.IsValid)
                AddNotification(nameof(data), "Invalid data cannot be inserted");
            else
                Right = data;
        }

        public IEnumerable<string> FindDifferences()
        {
            if (Left == null)
                AddNotification("Left", "Left cannot be null to compare");

            if (Right == null)
                AddNotification("Right", "Right cannot be null to compare");

            if (!AreSameSize())
                AddNotification("Left,Right", "Left and Right must be the same size to compare");

            if (!this.IsValid)
                return null;

            //TODO: Create algoritm of comparison
            return new List<string>();
        }

        public bool AreSameSize()
        {
            return Left?.Data.Length == Right?.Data.Length;
        }

        public bool AreExactlyEquals()
        {
            return Left?.Data == Right?.Data;
        }
    }
}
