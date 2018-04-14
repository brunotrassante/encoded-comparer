namespace EncodedComparer.Domain.Entities
{
    public class DifferenceInfo
    {
        public int StartingIndex { get; private set; }
        public int Length { get; private set; }

        public DifferenceInfo(int startingIndex, int length)
        {
            //TODO: Validations
            StartingIndex = startingIndex;
            Length = Length;
        }

        public override string ToString() => $"Difference found starting at index{StartingIndex} up to {StartingIndex + Length} ({Length} characters).";
    }
}
