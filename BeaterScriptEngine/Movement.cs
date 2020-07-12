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
        private short duration { get; set; }

        public Movement(string name, short duration)
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

        public short Duration
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

    }
}
