using System.Diagnostics;
using System.Globalization;
using System.IO.Compression;
using System.Text.RegularExpressions;

using RbxFFlagDumper.Lib;
using RbxFFlagDumper.Lib.Exceptions;

namespace RbxFFlagDumper.Test
{
    internal partial class Program
    {
        [GeneratedRegex("(version-[a-f0-9]{16}) at ([0-9\\/]+)")]
        private static partial Regex DeployHistoryRegex();

        static void Main(string[] args)
        {
            AsyncMain().Wait();
        }

        static async Task AsyncMain()
        {
            using var httpClient = new HttpClient();
            var done = new List<string>();

            string nextCanonDate = "";
            string nextVersionGuid = "";
            var nextDump = new List<string>();

            string deployHistory = await httpClient.GetStringAsync("https://setup.rbxcdn.com/DeployHistory.txt");

            Directory.CreateDirectory("bin");
            Directory.CreateDirectory("dmp");

            foreach (string line in deployHistory.Split('\n').Reverse())
            {
                if (!line.Contains("Studio64"))
                    continue;

                var match = DeployHistoryRegex().Match(line);

                string fullString = match.Groups[0].Value;
                string versionGuid = match.Groups[1].Value;
                string date = match.Groups[2].Value;

                string canonDate = DateOnly.Parse(date, new CultureInfo("en-US")).ToString("yyyy-MM-dd");

                if (done.Contains(versionGuid))
                    continue;

                Console.WriteLine(match);

                string exePath = Path.Combine("bin", $"{versionGuid}-RobloxStudioBeta.exe");
                string dmpPath = Path.Combine("dmp", $"{canonDate}-{versionGuid}.txt");

                if (!File.Exists(exePath))
                {
                    Console.WriteLine("\tDownloading...");
                    var zipStream = await httpClient.GetStreamAsync($"https://setup.rbxcdn.com/{versionGuid}-RobloxStudio.zip");

                    using var archive = new ZipArchive(zipStream);
                    archive.Entries.First(x => x.FullName == "RobloxStudioBeta.exe").ExtractToFile(exePath);
                }

                Console.WriteLine("\tDumping...");

                try
                {
                    var timer = new Stopwatch();
                    timer.Start();
                    var flags = StudioFFlagDumper.DumpCppFlags(exePath);
                    timer.Stop();

                    File.WriteAllText(dmpPath, String.Join('\n', flags));

                    if (!String.IsNullOrEmpty(nextVersionGuid))
                    {
                        var diff = new List<string>();
                        string diffPath = Path.Combine("dmp", $"{nextCanonDate}-{nextVersionGuid}.diff");

                        foreach (string flag in nextDump)
                        {
                            if (!flags.Contains(flag))
                                diff.Add($"+ {flag}");
                        }

                        foreach (string flag in flags)
                        {
                            if (!nextDump.Contains(flag))
                                diff.Add($"- {flag}");
                        }

                        diff.Sort((x, y) => x[2..].CompareTo(y[2..]));

                        File.WriteAllText(diffPath, String.Join("\n", diff));
                    }

                    Console.WriteLine($"\tDone (dumped {flags.Count} flags in {timer.Elapsed})");

                    nextCanonDate = canonDate;
                    nextDump = flags;
                    nextVersionGuid = versionGuid;
                }
                catch (CppDumpException ex)
                {
                    Console.WriteLine($"\tFAILED: {ex.Message}");
                    Console.ReadLine();
                    return;
                }

                Console.WriteLine("");

                done.Add(versionGuid);
            }
        }
    }
}
