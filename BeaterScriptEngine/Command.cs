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
        private string name { get; }
        private Type[] types { get; }
        private Object[] parameters { get; set; }
        private bool hasFunction { get; }
        private bool hasMovement { get; }
        private ushort id { get;  }

        public Command(string name, ushort id, bool hasFunction, bool hasMovement, params Type[] types)
        {
            this.name = name;
            this.types = types;
            this.id = id;
            this.hasFunction = hasFunction;
            this.hasMovement = hasMovement;
        }

        public Command(string name, ushort id, bool hasFunction, bool hasMovement, object[] parameters, params Type[] types)
        {
            this.name = name;
            this.types = types;
            this.id = id;
            this.hasFunction = hasFunction;
            this.hasMovement = hasMovement;
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public ushort ID
        {
            get
            {
                return this.id;
            }
        }

        public Type[] Types
        {
            get
            {
                return this.types;
            }
        }

        public Object[] Parameters
        {
            get
            {
                return this.parameters;
            }
            set
            {
                this.parameters = value;
            }
        }

        public bool HasFunction
        {
            get
            {
                return hasFunction;
            }
        }

        public bool HasMovement
        {
            get
            {
                return hasMovement;
            }
        }

        public override string ToString()
        {
            string result = $"{this.Name}(";

            for (int i = 0; i < Parameters.Length; i++)
                result += Parameters[i] + (i != Parameters.Length - 1 ? ", " : "");

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
            byte[] id_conv = BitConverter.GetBytes(ID);
            buf[0] = id_conv[0];
            buf[1] = id_conv[1];

            int i = 2;
            int k = 0;

            // Convert to byte array in little endian.
            while (i < this.Size())
            {
                byte[] conv;
                switch (Types[k].Name)
                {
                    case "Int32":
                        conv = BitConverter.GetBytes(Convert.ToInt32(this.Parameters[k]));
                        buf[i++] = conv[0];
                        buf[i++] = conv[1];
                        buf[i++] = conv[2];
                        buf[i++] = conv[3];
                        break;
                    case "UInt16":
                        conv = BitConverter.GetBytes(Convert.ToUInt16(this.Parameters[k]));
                        buf[i++] = conv[0];
                        buf[i++] = conv[1];
                        break;
                    case "Byte":
                        buf[i++] = (byte)this.Parameters[k];
                        break;
                    default:
                        break;
                }
                k++;
            }

            return buf;
        }

    }
}
