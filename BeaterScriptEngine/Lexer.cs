using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Runtime.Remoting.Messaging;
using System.Runtime.CompilerServices;
using System.Collections;

namespace BeaterScriptEngine
{
    public class ScriptLexer
    {
        BinaryWriter b;
        List<int> Pointers { get; set; }
        CommandsListHandler cmds;
        string path;

        public ScriptLexer(string script, string game)
        {
            // Initialize the script we will write to.
            path = script;
            b = new BinaryWriter(File.Open(script, FileMode.Open));
            cmds = new CommandsListHandler(game);
            Pointers = new List<int>();
        }

        public void WriteScript(List<Script> scripts, List<Script> functions, List<List<Movement>> movements)
        {
            // Less messy. But... still messy.
            int location = Convert.ToInt32(scripts.Count * 0x4 + 2); // Size of header section.
            Dictionary<string, int> script_map = new Dictionary<string, int>();

            // Information gathering time.
            // For absolutely no reason, I will place all functions first. Problem?
            foreach (Script s in functions)
            {
                script_map.Add($"Function{functions.IndexOf(s)}", location);
                location += s.Size;
            }

            // Okay now we have scripts.
            foreach (Script s in scripts)
            {
                script_map.Add($"Script{scripts.IndexOf(s)}", location);
                Pointers.Add(location - 4 * scripts.IndexOf(s) - 4);
                location += s.Size;
            }

            // Now the funky weirdo cousin, movements.
            foreach (List<Movement> m in movements)
            {
                script_map.Add($"Movement{movements.IndexOf(m)}", location);
                location += 0x4;
            }

            // Time to write to the map.
            using (StreamWriter z = new StreamWriter(Path.Combine(Directory.GetParent(this.path).FullName, Path.GetFileNameWithoutExtension(this.path) + ".map")))
                foreach (KeyValuePair<string, int> d in script_map)
                    z.Write($"{d.Key} : {d.Value}\n");
            

            // Now we begin writing.
            foreach (int pointer in Pointers)
                b.Write(BitConverter.GetBytes(pointer));

            b.Write(Convert.ToUInt16(0xFD13));

            location = Convert.ToInt32(scripts.Count * 0x4 + 2); // Size of header section.

            foreach (Script s in functions)
                foreach (Command c in s)
                {
                    string original = "";
                    if (c.HasFunction || c.HasMovement)
                    {
                        original = (string)c.Parameters[c.Parameters.Count - 1];
                        c.Parameters[c.Parameters.Count - 1] = script_map[c.Parameters.Last().ToString()] - location - 4;
                    }
                    b.Write(c.ToBytes());

                    if (c.HasFunction || c.HasMovement)
                        c.Parameters[c.Parameters.Count - 1] = original;

                    location += c.Size();
                }

            foreach (Script s in scripts)
                foreach (Command c in s)
                {
                    string original = "";
                    if (c.HasFunction || c.HasMovement)
                    {
                        original = (string)c.Parameters[c.Parameters.Count - 1];
                        c.Parameters[c.Parameters.Count - 1] = script_map[c.Parameters.Last().ToString()] - location - 4;
                    }
                    b.Write(c.ToBytes());

                    if (c.HasFunction || c.HasMovement)
                        c.Parameters[c.Parameters.Count - 1] = original;

                    location += c.Size();
                }

            foreach (List<Movement> movement in movements)
                foreach (Movement m in movement)
                {
                    b.Write(m.ToBytes());
                    location += 0x4;
                }

            // Should be good. Let's hope...
            b.Close();
        }

    }
}
