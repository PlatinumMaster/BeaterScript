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

        public Command(string name, bool hasFunction, bool hasMovement, params Type[] types)
        {
            this.name = name;
            this.types = types;
            this.hasFunction = hasFunction;
            this.hasMovement = hasMovement;
        }

        public Command(string name, bool hasFunction, bool hasMovement, object[] parameters, params Type[] types)
        {
            this.name = name;
            this.types = types;
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
                switch (t.ToString())
                {
                    case "int":
                        size += 4;
                        break;
                    case "ushort":
                        size += 2;
                        break;
                    case "byte":
                        size += 1;
                        break;
                    default:
                        break;
                }

            return size;
        }

        public byte[] ToBytes()
        {
            CommandsListHandler c = new CommandsListHandler("B2W2");
            byte[] buf = new byte[this.Size()];
            buf[0] = Convert.ToByte(c.command_map[this.Name] >> 0x8 & 0xFF);
            buf[1] = Convert.ToByte(c.command_map[this.Name] & 0xFF);

            int i = 2;
            int k = 0;

            // Convert to byte array in little endian.
            while (i < this.Size())
            {
                if (this.Parameters[k].GetType() == typeof(string))
                    Parameters[k] = 0x3;
                switch (Types[k].ToString())
                {
                    case "int":
                        buf[i] = Convert.ToByte((int)this.Parameters[k] >> 24);
                        buf[i++] = Convert.ToByte((int)this.Parameters[k] >> 16);
                        buf[i++] = Convert.ToByte((int)this.Parameters[k] >> 8);
                        buf[i++] = Convert.ToByte((int)this.Parameters[k] & 0xFF);
                        k++;
                        break;
                    case "ushort":
                        buf[i] = Convert.ToByte((short)this.Parameters[k] >> 8);
                        buf[i++] = Convert.ToByte((short)this.Parameters[k] & 0xFF);
                        k++;
                        break;
                    case "byte":
                        buf[i] = Convert.ToByte((byte)this.Parameters[k] & 0xFF);
                        break;
                    default:
                        break;
                }
            }

            return buf;
        }

    }
}
