using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeaterScriptEngine
{
    public class Movement
    {
        private string name { get; set; }
        private ushort duration { get; set; }
        private ushort id{ get; set; }

        public Movement(string name, ushort id, ushort duration)
        {
            this.name = name;
            this.duration = duration;
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public ushort ID
        {
            get
            {
                return this.ID;
            }
            set
            {
                this.id = value;
            }
        }

        public ushort Duration
        {
            get
            {
                return this.duration;
            }
            set
            {
                this.duration = value;
            }
        }

        public override string ToString()
        {
            return $"{this.Name}({this.duration});";
        }

        public byte[] ToBytes()
        {
            var id_bytes = BitConverter.GetBytes((ushort)this.id);
            var duration_bytes = BitConverter.GetBytes((ushort)this.duration);

            return new byte[] { id_bytes[0], id_bytes[1], duration_bytes[0], duration_bytes[1] };
        }

    }
}
