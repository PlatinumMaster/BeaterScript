using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeaterScriptEngine
{
    public class Command
    {
        private string name { get; set; }
        private Type[] types { get; set; }
        private Object[] parameters { get; set; }
        private bool hasFunction { get; set; }
        private bool hasMovement { get; set; }

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
            set
            {
                this.name = value;
            }
        }

        public Type[] Types
        {
            get
            {
                return this.types;
            }
            set
            {
                this.types = value;
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
            set
            {
                this.hasFunction = value;
            }
        }

        public bool HasMovement
        {
            get
            {
                return hasMovement;
            }
            set
            {
                this.hasMovement = value;
            }
        }

        public override string ToString()
        {
            string result = this.Name + "(";
            for (int i = 0; i < Parameters.Length; i++)
                result += Parameters[i] + (i != Parameters.Length - 1 ? ", " : "");
            return result.TrimEnd(' ') + ");";
        }

    }
}
