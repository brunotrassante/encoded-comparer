using EncodedComparer.Shared.Notifications;
using System;
using System.Collections.Generic;

namespace EncodedComparer.Domain.Entities
{
    public class EncodedPair : Notifiable
    {
        public Base64Data Left { get; private set; }
        public Base64Data Right { get; private set; }

        public EncodedPair(Base64Data left, Base64Data right)
        {
            if (left == null || right == null)
                AddNotification($"{nameof(left)},{nameof(right)}", $"Parameters {nameof(left)} and {nameof(right)} must be provided.");

            Left = left;
            Right = right;
        }

        public IEnumerable<DifferenceInfo> FindDifferences()
        {
            if (Left == null)
                AddNotification("Left", "Left cannot be null to compare");

            if (Right == null)
                AddNotification("Right", "Right cannot be null to compare");

            if (!AreSameSize)
                AddNotification("Left,Right", "Left and Right must be the same size to compare");

            if (!this.IsValid)
                return null;

            int index = 0;
            int maxIndexValue = Left.Data.Length;
            var differences = new List<DifferenceInfo>();

            while (index < maxIndexValue)
            {
                int startingIndex = FindNextDifferenceOrLastIndex(ref index, maxIndexValue);

                if (startingIndex == maxIndexValue)
                    break;

                int upToIndex = FindEndDifferenceIndex(ref index, maxIndexValue);

                differences.Add(new DifferenceInfo(startingIndex, upToIndex - startingIndex));                
            }

            return differences;
        }

        private int FindEndDifferenceIndex(ref int startingIndex, int maxIndexValue)
        {
            while (startingIndex < maxIndexValue && Left.Data[startingIndex] != Right.Data[startingIndex])
                startingIndex++;
            return startingIndex;
        }

        private int FindNextDifferenceOrLastIndex(ref int startingIndex, int maxIndexValue)
        {
            while (startingIndex < maxIndexValue && Left.Data[startingIndex] == Right.Data[startingIndex])
                startingIndex++;
            return startingIndex;
        }

        public bool AreSameSize => Left?.Data.Length == Right?.Data.Length;

        public bool AreExactlyEquals => Left?.Data == Right?.Data;

    }
}
