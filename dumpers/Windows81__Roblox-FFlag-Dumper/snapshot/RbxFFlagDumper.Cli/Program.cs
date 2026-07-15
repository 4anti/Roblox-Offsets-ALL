using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using RbxFFlagDumper.Lib;

namespace RbxFFlagDumper.Cli
{
    internal class Program
    {
        static readonly string[] dumpModes = new string[] { "cpp", "lua", "all" };

        static void Main(string[] args)
        {
            string studioPath;
            string dumpMode;

            if (Debugger.IsAttached)
            {
                Console.Write("Enter dump mode\n> ");
                dumpMode = Console.ReadLine();

                Console.Write("Enter path to Studio folder\n> ");
                studioPath = Console.ReadLine();
            }
            else
            {
                if (args.Length < 2)
                {
                    Console.WriteLine("Usage: RbxFFlagDumper.Cli.exe [cpp|lua|all] [path to studio folder]");
                    return;
                }

                dumpMode = args[0];
                studioPath = args[1];
            }

            if (!dumpModes.Contains(dumpMode))
            {
                Console.WriteLine("Invalid dump mode");
                return;
            }

            string exePath = Path.Combine(studioPath, "RobloxStudioBeta.exe");

            if (!File.Exists(exePath))
            {
                Console.WriteLine("Could not find RobloxStudioBeta.exe");
                return;
            }

            string extraContentPath = Path.Combine(studioPath, "ExtraContent");

            if (!Directory.Exists(extraContentPath))
            {
                Console.WriteLine("Could not find ExtraContent folder");
                return;
            }

            Console.WriteLine("Scanning, please wait...");

            List<string> output;

            try
            {
                if (dumpMode == "cpp")
                    output = StudioFFlagDumper.DumpCppFlags(exePath);
                else if (dumpMode == "lua")
                    output = StudioFFlagDumper.DumpLuaFlags(extraContentPath);
                else
                    output = StudioFFlagDumper.DumpAllFlags(studioPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                if (Debugger.IsAttached)
                    Console.ReadLine();

                return;
            }

            string dumpPath = Path.Combine(studioPath, $"fflags.{dumpMode}.txt");
            File.WriteAllText(dumpPath, String.Join("\n", output));
            Console.WriteLine($"Written to {dumpPath}");

            if (Debugger.IsAttached)
                Console.ReadLine();
        }
    }
}
