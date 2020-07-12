using System;
using System.Collections.Generic;
using System.Linq;
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

    }
}
