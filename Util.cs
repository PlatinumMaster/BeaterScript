using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeaterScript
{
    public class Util
    {
        public static void GenerateCommandASM(string game)
        {
            CommandsListHandler cmd = new CommandsListHandler(game);
            using (System.IO.StreamWriter o = new System.IO.StreamWriter($"{game}.s"))
            {
                // Helper Macros
                o.WriteLine($"@ Helper Macros");

                // Script: For the purpose of declaring each script in the header.
                o.WriteLine($".macro script, address{Environment.NewLine}" +
                    $".word  \\address - . - 4{Environment.NewLine}" +
                    $".endm{Environment.NewLine}");

                // EndHeader: Declares the end of the header section.
                o.WriteLine($".macro EndHeader{Environment.NewLine}" +
                    $".hword 0xFD13{Environment.NewLine}" +
                    $".endm{Environment.NewLine}");

                // Movement: Declares a new movement instruction.
                o.WriteLine($".macro Movement x y{Environment.NewLine}" +
                    $".hword \\x{Environment.NewLine}" +
                    $".hword \\y{Environment.NewLine}" +
                    $".endm{Environment.NewLine}");

                // MovementLabel: Declares a a new movement label. Needed for padding.
                o.WriteLine($".macro MovementLabel label{Environment.NewLine}" +
                    $".align 4{Environment.NewLine}" +
                    $"\\label:{Environment.NewLine}" +
                    $".endm{Environment.NewLine}");

                // Write all of the commands from the YAML.
                o.WriteLine($"@ -----------------");
                o.WriteLine($"@ Script Commands");
                foreach (ushort key in cmd.commands.Keys)
                {
                    Command c = cmd.commands[key];
                    o.Write($".macro {c.Name} ");
                    for (int i = 0; i < c.Types.Count(); i++)
                        o.Write($"p{i}{(i == c.Types.Count() - 1 ? "" : ",")} ");

                    o.WriteLine($"{Environment.NewLine}" +
                        $".hword {c.ID}");

                    int j = 0;
                    for (int i = 0; i < c.Types.Count(); i++)
                        switch (c.Types[i].Name)
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
                    o.WriteLine($".endm" +
                        $"{Environment.NewLine}");
                }
            }
        }
    }
}
