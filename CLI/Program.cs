using System;
using System.Diagnostics;
using System.IO;
using BeaterScript;

namespace BeaterScriptCLI
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine(usage);
                return;
            }

            switch (args[0].ToLower())
            {
                case "-d":
                    var p = new ScriptParser(args[1], args[2]);
                    _ = new ScriptLexer(p.Scripts, p.Functions, p.Movements, args[3], args[2]);
                    Util.GenerateCommandASM(args[2]);
                    break;
                case "-g":
                    Util.GenerateCommandASM(args[1]);
                    break;
                case "-h":
                    Console.WriteLine(usage);
                    break;
                case "-m":

                    // If this check fails, the user does not have devkitARM properly setup.
                    var path = Environment.GetEnvironmentVariable("DEVKITARM");
                    if (path == null)
                    {
                        Console.WriteLine("Cannot detect a proper devkitARM setup. Exiting.");
                        return;
                    }

                    string devkitPath = Path.Combine(path, "bin");
                    var filePath = Path.GetFileNameWithoutExtension(Path.GetFullPath(args[1]));

                    var proc = Process.Start(Path.Combine(devkitPath, "arm-none-eabi-as"), $"-mthumb -c {args[1]} -o {filePath}.o");
                    proc?.WaitForExit();
                    proc = Process.Start(Path.Combine(devkitPath, "arm-none-eabi-objcopy"), $"-O binary {filePath}.o {args[2]}");
                    proc?.WaitForExit();
                    break;
            }
        }

        private const string usage = @"BeaterScript --- Usage:
To decompile: BeaterScript -d <script location> <game> <output>
To compile: BeaterScript -m <script location> <output>
To generate a commands list for use with the program: BeaterScript -g <game>";
    }
}
