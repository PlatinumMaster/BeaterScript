namespace BeaterScript
{
    public class Movement
    {
        private string Name { get; set; }
        private ushort Duration { get; set; }
        private ushort ID { get; set; }

        public Movement(string name, ushort id, ushort duration)
        {
            Name = name;
            ID = id;
            Duration = duration;
        }

        public override string ToString() => $"Movement {Name ?? ID.ToString()} {Duration}";
    }
}
