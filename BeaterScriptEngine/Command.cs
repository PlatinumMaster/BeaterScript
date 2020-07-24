using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BeaterScriptEngine
{
    public class Command
    {
        public string Name { get; }
        public Type[] Types { get; }
        public bool HasFunction { get; }
        public bool HasMovement { get; }
        public ushort ID { get; }
        public List<object> Parameters { get; set; }


        public Command(string name, ushort id, bool hasFunction, bool hasMovement, params Type[] types)
        {
            Name = name;
            Types = types;
            ID = id;
            Parameters = new List<object>();
            HasFunction = hasFunction;
            HasMovement = hasMovement;
        }

        public override string ToString()
        {
            string result = $"{this.Name}(";

            for (int i = 0; i < Parameters.Count; i++)
                result += Parameters[i] + (i != Parameters.Count - 1 ? ", " : "");

            return result.TrimEnd(' ') + ");";
        }

        public uint Size()
        {
            // All command sizes are greater than 2.
            uint size = 2;

            foreach (Type t in Types)
                switch (t.Name)
                {
                    case "Int32":
                        size += 4;
                        break;
                    case "UInt16":
                        size += 2;
                        break;
                    case "Byte":
                        size += 1;
                        break;
                    default:
                        break;
                }

            return size;
        }

        public byte[] ToBytes()
        {
            byte[] buf = new byte[this.Size()];
            
            Util.Deconstruct(out buf[0], out buf[1], BitConverter.GetBytes(ID));

            // Convert to byte array in little endian.
            int i = 2, k = 0;
            while (i < this.Size())
                switch (Types[k].Name)
                {
                    case "Int32":
                        Util.Deconstruct(out buf[i++], out buf[i++], out buf[i++], out buf[i++], BitConverter.GetBytes((int)Parameters[k++]));
                        break;
                    case "UInt16":
                        Util.Deconstruct(out buf[i++], out buf[i++], BitConverter.GetBytes((ushort)Parameters[k++]));
                        break;
                    case "Byte":
                        buf[i++] = (byte)this.Parameters[k++];
                        break;
                }

            return buf;
        }
    }
}
