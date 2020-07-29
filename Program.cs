using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BeaterScript
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
                        ScriptLexer l = new ScriptLexer(p.Scripts, p.Functions, p.Movements, args[3], args[2]);
                        break;
                    case "-g":
                        Util.GenerateCommandASM(args[1]);
                        break;
                    case "-h":
                        Console.WriteLine(usage);
                        break;
                    case "-m":
                        string devkitPath;

                        // If this check fails, the user does not have devkitARM properly setup.
                        try
                        {
                            devkitPath = Path.Combine(Environment.GetEnvironmentVariable("DEVKITARM"), "bin");
                        }
                        catch (ArgumentNullException)
                        {
                            Console.WriteLine("Cannot detect a proper devkitARM setup. Exiting.");
                            return;
                        }

                        var filePath = Path.GetFileNameWithoutExtension(Path.GetFullPath(args[1]));

                        Process proc = Process.Start(Path.Combine(devkitPath, "arm-none-eabi-as"), $"-mthumb -c {args[1]} -o {filePath}.o");
                        proc.WaitForExit();
                        proc = Process.Start(Path.Combine(devkitPath, "arm-none-eabi-objcopy"), $"-O binary {filePath}.o {args[2]}");
                        proc.WaitForExit();
                        break;
                }
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine(usage);
            }
        }

        private static string usage = $"BeaterScript --- Usage{Environment.NewLine}" +
            $"To decompile: BeaterScript -d <script location> <game> <output>{Environment.NewLine}" +
            $"To compile: BeaterScript -m <script location> <output>{Environment.NewLine}" +
            $"To generate a commands list for use with the program: BeaterScript -g <game>";
    }
}
