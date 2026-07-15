using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using PeNet;
using PeNet.Header.Pe;
using RbxFFlagDumper.Lib.Exceptions;

namespace RbxFFlagDumper.Lib
{
    public static class StudioFFlagDumper
    {
        /// <summary>
        /// Consolidated function for dumping all flags (both C++ and Lua).
        /// </summary>
        /// <param name="studioPath"></param>
        /// <returns>
        /// A list of flags prefixed with a [C++] or [Lua] indicator.
        /// </returns>
        /// <exception cref="CppDumpException">
        /// Thrown when an issue with C++ dumping occurs.
        /// </exception>
        public static List<string> DumpAllFlags(string studioPath)
        {
            var list = new List<string>();

            var cppFlags = DumpCppFlags(Path.Combine(studioPath, "RobloxStudioBeta.exe"));
            var luaFlags = DumpLuaFlags(Path.Combine(studioPath, "ExtraContent"));
            var commonFlags = cppFlags.Intersect(luaFlags);

            list.AddRange(cppFlags.Where(x => !commonFlags.Contains(x)).Select(x => "[C++] " + x));
            list.AddRange(luaFlags.Where(x => !commonFlags.Contains(x)).Select(x => "[Lua] " + x));
            list.AddRange(commonFlags.Select(x => "[Com] " + x));

            return list.OrderBy(x => x.Substring(6)).ToList();
        }

        /// <summary>
        /// Dumps all Lua defined flags found within the Studio CoreScripts
        /// </summary>
        /// <param name="extraContentPath"></param>
        /// <returns></returns>
        public static List<string> DumpLuaFlags(string extraContentPath)
        {
            var finalList = new List<string>();

            foreach (var file in Directory.GetFiles(extraContentPath, "*.lua", SearchOption.AllDirectories))
            {
                string contents = File.ReadAllText(file);

                var matches = Regex.Matches(contents, "game:(?:Get|Define)Fast(Flag|Int|String)\\(\\\"(\\w+)\\\"\\)").Cast<Match>();
                var userMatches = Regex.Matches(contents, "(?:IsUserFeatureEnabled|getUserFlag)\\(\\\"(\\w+)\\\"\\)").Cast<Match>();

                foreach (var match in matches)
                {
                    string flag = string.Format("F{0}{1}", match.Groups[1], match.Groups[2]);

                    if (!finalList.Contains(flag))
                        finalList.Add(flag);
                }

                foreach (var match in userMatches)
                {
                    string flag = string.Format("FFlag{0}", match.Groups[1]);

                    if (!finalList.Contains(flag) && flag != "FFlagUserDoStuff")
                        finalList.Add(flag);
                }
            }

            finalList.Sort();
            return finalList;
        }

        private static int GetRVAOffset(ImageSectionHeader sectionHeader)
            => (int)(sectionHeader.VirtualAddress - sectionHeader.PointerToRawData);

        /// <summary>
        /// Dumps all C++ defined flags found within the RobloxStudioBeta executable
        /// </summary>
        /// <param name="studioExePath"></param>
        /// <returns></returns>
        /// <exception cref="CppDumpException">
        /// Thrown when an issue with C++ dumping occurs.
        /// </exception>
        public static List<string> DumpCppFlags(string studioExePath)
        {
            byte[] binary = File.ReadAllBytes(studioExePath);

            var sectionHeaders = new PeFile(binary).ImageSectionHeaders;
            var textHeader = sectionHeaders.First(x => x.Name == ".text");
            var rdataHeader = sectionHeaders.First(x => x.Name == ".rdata");
            int rvaOffset = GetRVAOffset(textHeader) - GetRVAOffset(rdataHeader);

            // this snippet is present for each registered fflag in RobloxStudioBeta.exe
            // 00:  41 B8 ?? ?? ?? ??    | mov r8d, <val>     ; byte, determines if dynamic
            // 06:  48 8D 15 ?? ?? ?? ?? | lea rdx, <addr>    ; 
            // 13:  48 8D 0D ?? ?? ?? ?? | lea rcx, <addr>    ; fflag name
            // 20:  E9 ?? ?? ?? ??       | jmp <addr>         ; registration routine, determines data type (FFlag, SFFlag, FString, etc)

            string pattern = "41 B8 ?? 00 00 00 48 8D 15 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E9";
            var scanner = new PatternScanner(binary, pattern, (int)textHeader.PointerToRawData, (int)textHeader.SizeOfRawData);

            var dataTypeTable = new Dictionary<int, string>();
            var rawFFlagData = new List<RawFFlagData>();

            while (!scanner.Finished())
            {
                int pos = scanner.FindNext();

                if (pos == -1)
                    break;

                int param = binary[pos + 2];

                // resolving the pointer with a constant offset since we can just assume it will always point to .rdata
                int namePtr = pos + 20 + BitConverter.ToInt32(binary, pos + 16) + rvaOffset;
                int targetAddr = pos + 25 + BitConverter.ToInt32(binary, pos + 21);

                string name = "";

                for (int i = namePtr; binary[i] != 0; i++)
                {
                    if (binary[i] < 0x20 || binary[i] > 0x7F)
                        throw new CppDumpException("Encountered invalid data");

                    name += Convert.ToChar(binary[i]);
                }

                rawFFlagData.Add(new RawFFlagData(targetAddr, param, name));

                if (!dataTypeTable.ContainsKey(targetAddr))
                    dataTypeTable[targetAddr] = null;
            }

            // currently only two SFFlags exist - would there ever be zero?
            if (dataTypeTable.Count != 5)
                throw new CppDumpException("Expected 5 different flag types");

            // the registration routines for each flag type are stored in memory consecutively in this order
            dataTypeTable = dataTypeTable.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            dataTypeTable[dataTypeTable.Keys.ElementAt(0)] = "FFlag";
            dataTypeTable[dataTypeTable.Keys.ElementAt(1)] = "SFFlag";
            dataTypeTable[dataTypeTable.Keys.ElementAt(2)] = "FInt";
            dataTypeTable[dataTypeTable.Keys.ElementAt(3)] = "FLog";
            dataTypeTable[dataTypeTable.Keys.ElementAt(4)] = "FString";

            var finalList = new List<string>();

            foreach (var entry in rawFFlagData)
            {
                string dataType = dataTypeTable[entry.DataTypeId];
                string name = "";

                if (entry.ByteParam == 2)
                    name += "D";

                name += dataType;
                name += entry.Name;

                finalList.Add(name);
            }

            finalList.Sort();
            return finalList;
        }
    }
}
