using System;

namespace BeaterScriptEngine
{
    public class Movement
    {
        private string Name { get; set; }
        private ushort Duration { get; set; }
        private ushort ID { get; set; }
        private byte[] Bytes { get; set; }

        public Movement(string name, ushort id, ushort duration)
        {
            Name = name;
            ID = id;
            Duration = duration;
        }

        public override string ToString() => $"{Name}({Duration});";

        public byte[] ToBytes()
        {
            var id_bytes = BitConverter.GetBytes(ID);
            var duration_bytes = BitConverter.GetBytes(Duration);

            return new byte[] { id_bytes[0], id_bytes[1], duration_bytes[0], duration_bytes[1] };
        }

    }
}
