# rbxfflagdumper

.NET suite for statically dumping Roblox engine FastFlags, used in [Roblox Client Tracker](https://github.com/MaximumADHD/Roblox-Client-Tracker/)

## RbxFFlagDumper.Lib

Simple programmatic dumping of either C++ or Lua defined flags

```c#
List<String> flags = StudioFFlagDumper.DumpAllFlags("versions\\version-xxxxxxxxxxxxxxxx\\");
File.WriteAllText("fflags.txt", String.Join('\n', flags));
```

Flags prefixed with `[Com]` indicate those common between both C++ and Lua

## RbxFFlagDumper.Cli

Basic command line tool for dumping from a directory containing Roblox Studio

```
Usage: RbxFFlagDumper.Cli.exe [cpp|lua|all] [path to studio folder]
```

## License

Licensed under the MIT license. Feel free to use however.

Uses [PeNet](https://github.com/secana/PeNet).
