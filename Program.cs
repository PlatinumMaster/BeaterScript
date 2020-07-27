using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BeaterScriptEngine
{
    class Program
    {
        //  <summary>
        //  The main entry point for the application.
        //  </summary>

        static void Main(string[] args)
        {
            try 
            {
                switch(args[0].ToLower())
                {
                    case "-d":
                        ScriptParser p = new ScriptParser(args[1], args[2]);
                        using (StreamWriter o = new StreamWriter(args[3]))
                        {
                            o.WriteLine(".align 4");
                            o.WriteLine(".include \"B2W2.s\"");
                            o.WriteLine("");

                            o.WriteLine("Header:");
                            foreach (int Address in p.Addresses)
                                o.WriteLine($"  script Script{p.Scripts.Keys.ToList().IndexOf(Address)}");
                            o.WriteLine("EndHeader");
                            o.WriteLine("");

                            Console.WriteLine("Parsing functions...");
                            foreach (Script f in p.Functions.Values)
                            {
                                o.WriteLine($"Function{p.Functions.Values.ToList().IndexOf(f)}:");
                                foreach (Command c in f)
                                    o.WriteLine($"  {c}");
                                o.WriteLine("");
                            }

                            Console.WriteLine("Parsing scripts...");
                            foreach (Script s in p.Scripts.Values)
                            {
                                o.WriteLine($"Script{p.Scripts.Values.ToList().IndexOf(s)}:");
                                foreach (Command c in s)
                                    o.WriteLine($"  {c}");
                                o.WriteLine("");
                            }

                            Console.WriteLine("Parsing movements...");
                            foreach (List<Movement> moves in p.Movements.Values)
                            {
                                o.WriteLine($"MovementLabel Movement{p.Movements.Values.ToList().IndexOf(moves)}");
                                foreach (Movement m in moves)
                                    o.WriteLine($"  {m}");
                                o.WriteLine("");
                            }

                            Console.WriteLine("Done");
                        }
                        break;
                    case "-g":
                        CommandsListHandler cmd = new CommandsListHandler("B2W2");
                        using (StreamWriter o = new StreamWriter("B2W2.s"))
                        {
                            // Helper Macros
                            o.WriteLine($"@ Helper Macros");
                            o.WriteLine($".macro script, address{Environment.NewLine}" +
                                $".word  \\address - . - 4{Environment.NewLine}" +
                                $".endm" +
                                $"{Environment.NewLine}");

                            o.WriteLine($".macro EndHeader{Environment.NewLine}" +
                                $".hword 0xFD13{Environment.NewLine}" +
                                $".endm" +
                                $"{Environment.NewLine}");

                            o.WriteLine($".macro Movement x y{Environment.NewLine}" +
                                $".hword \\x{Environment.NewLine}" +
                                $".hword \\y{Environment.NewLine}" +
                                $".endm" +
                                $"{Environment.NewLine}");


                            o.WriteLine($".macro MovementLabel label{Environment.NewLine}" +
                                $".align 4{Environment.NewLine}" +
                                $"\\label:{Environment.NewLine}" +
                                $".endm" +
                                $"{Environment.NewLine}");

                            o.WriteLine($"@ -----------------");
                            o.WriteLine($"@ Script Commands");
                            foreach (ushort key in cmd.commands.Keys)
                            {
                                Command c = cmd.commands[key];
                                o.Write($".macro {c.Name} ");
                                for (int i = 0; i < c.Types.Count(); i++)
                                    o.Write($"p{i}{(i == c.Types.Count() - 1 ? "" : ",")} ");

                                o.WriteLine($"");
                                o.WriteLine($".hword {c.ID}");

                                int j = 0;
                                foreach (var t in c.Types)
                                    switch (t.Name)
                                    {
                                        case "Int32":
                                            if (c.HasFunction || c.HasMovement)
                                            { 
                                                o.WriteLine($".word (\\p{j++} - .) - 4");
                                                break;
                                            }
                                            o.WriteLine($".word \\p{j++}");
                                            break;
                                        case "UInt16":
                                            o.WriteLine($".hword \\p{j++}");
                                            break;
                                        case "Byte":
                                            o.WriteLine($".byte \\p{j++}");
                                            break;
                                    }
                                o.WriteLine($".endm");
                                o.WriteLine($"");
                            }
                        }
                        break;
                    case "-m":
                        var devkitPath = Path.Combine(Environment.GetEnvironmentVariable("DEVKITARM"), "bin");
                        var filePath = Environment.CurrentDirectory;
                        var fileName = Path.GetFileNameWithoutExtension(args[1]);

                        Process proc = Process.Start(Path.Combine(devkitPath, "arm-none-eabi-as"), $"-mthumb -c {args[1]} -o {Path.Combine(filePath, fileName)}.o");
                        proc.WaitForExit();
                        proc = Process.Start(Path.Combine(devkitPath, "arm-none-eabi-objcopy"), $"-O binary {Path.Combine(filePath, fileName)}.o {args[2]}");
                        proc.WaitForExit();
                        break;
                }
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Usage coming soon.");
            }
        }
    }
}
