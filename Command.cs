using System;
using System.Collections.Generic;
using System.Text;

namespace BeaterScript
{
    public class Command
    {
        public string Name { get; }
        public IReadOnlyList<Type> Types { get; }
        public bool HasFunction { get; }
        public bool HasMovement { get; }
        public bool IsEnd { get;  }
        public ushort ID { get; }
        public List<object> Parameters { get; set; }
        
        public Command(string name, ushort id, bool hasFunction, bool hasMovement, bool isEnd, IReadOnlyList<Type> types)
        {
            Name = name;
            Types = types;
            ID = id;
            Parameters = new List<object>();
            HasFunction = hasFunction;
            HasMovement = hasMovement;
            IsEnd = isEnd;
        }

        public Command(string name, ushort id, bool hasFunction, bool hasMovement, bool isEnd)
            : this(name, id, hasFunction, hasMovement, isEnd, Array.Empty<Type>())
        {
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append($"{Name} ");

            for (int i = 0; i < Parameters.Count; i++)
            {
                result.Append(Parameters[i]);
                if (i != Parameters.Count - 1) result.Append(", ");
            }

            return result.ToString();
        }
    }
}
