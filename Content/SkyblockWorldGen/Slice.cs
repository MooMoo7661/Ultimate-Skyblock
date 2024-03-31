using static UltimateSkyblock.Content.SkyblockWorldGen.Slice;

namespace UltimateSkyblock.Content.SkyblockWorldGen
{

    public class Slice
    {
        public delegate void IslandGenerationEvent(Slice slice);
        public event IslandGenerationEvent IslandGeneration;

        private protected int _lengthMin;
        private protected int _lengthMax;
        private string _name;

        public string Name { get => _name; set => _name = value; }
        public int Index { get; set; }

        public int LengthMin { get => _lengthMin; }
        public int LengthMax { get => _lengthMax; }
        public int Length { get => LengthMax - LengthMin; }
        public int Center { get => Length / 2; }
        public int CenterInWorld { get => LengthMin + Length / 2; }

        public Slice(int lengthMin, int lengthMax)
        {
            _lengthMin = lengthMin;
            _lengthMax = lengthMax;
        }
        
        public void InvokeIslandGeneration() => IslandGeneration?.Invoke(this);

        public bool WithinRange(int pos) => pos >= _lengthMin && pos <= LengthMax;

        public static Slice GetIslandsFromCoordinate(int pos)
        {
            foreach (Slice slice in IslandHandler.Slices)
            {
                if (slice.WithinRange(pos))
                {
                    return slice;
                }
            }

            return null;
        }
    }

    /// <summary>
    /// Used to store misc data about a slice before adding to the slice.
    /// </summary>
    public class SliceGenerationInfo
    {
        private IslandGenerationEvent _evt;
        private string _name;

        public IslandGenerationEvent Event { get => _evt; }
        public string Name { get => _name; }

        public SliceGenerationInfo(string name, IslandGenerationEvent evt)
        {
            _name = name;
            _evt = evt;
        }
    }
}
