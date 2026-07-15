# Roblox Offsets (ALL)

## Overview

Read-only archive of publicly available Roblox FFlag offset dumpers. No user
data, no telemetry, no runtime — this repo just mirrors public sources so
consumers can pick whichever endpoint or dumper best serves their region.
Noncommercial mirror; upstream authors retain all rights.

## Dumper endpoints

| Source | URL | Format | Version comment | Folder |
| --- | --- | --- | --- | --- |
| imtheo (stable) | https://offsets.imtheo.lol/FFlags.hpp | A (nested FFlagList) | yes | `endpoints/imtheo/` |
| imtheo (dev) | https://dev.imtheo.lol/Offsets/FFlags.hpp | A (nested FFlagList) | yes | `endpoints/imtheo-dev/` |
| workers.dev | https://offsets.ntgetwritewatch.workers.dev/FFlags.hpp | B (flat FFlagOffsets) | no | `endpoints/workers-dev/` |
| NtReadVirtualMemory | https://raw.githubusercontent.com/NtReadVirtualMemory/Roblox-Offsets-Website/main/FFlags.hpp | B | rarely | `endpoints/ntreadvirtualmemory/` |
| souloveryall | https://raw.githubusercontent.com/souloveryall/offsets.hpp/main/Offsets.hpp | B | rarely | `endpoints/souloveryall/` |
| upio (macOS) | https://offsets.upio.dev/FFlags.hpp | B | no | `endpoints/upio-macos/` |

## Dumper source code

| Repo | Upstream URL | Language | Folder |
| --- | --- | --- | --- |
| pizzaboxer/rbxfflagdumper | https://github.com/pizzaboxer/rbxfflagdumper | C++ | `dumpers/pizzaboxer__rbxfflagdumper/` |
| Srungot/FFlags-dumper-roblox | https://github.com/Srungot/FFlags-dumper-roblox | Python | `dumpers/Srungot__FFlags-dumper-roblox/` |
| nopjo/roblox-dumper | https://github.com/nopjo/roblox-dumper | Python | `dumpers/nopjo__roblox-dumper/` |
| henrick001/External-Dumper | https://github.com/henrick001/External-Dumper | C++ | `dumpers/henrick001__External-Dumper/` |
| nhisoka/Roblox-Offset-Dumper | https://github.com/nhisoka/Roblox-Offset-Dumper | C++ | `dumpers/nhisoka__Roblox-Offset-Dumper/` |
| Lonegwadiwaitor/roblox-offset-dumper | https://github.com/Lonegwadiwaitor/roblox-offset-dumper | C++ | `dumpers/Lonegwadiwaitor__roblox-offset-dumper/` |
| NtReadVirtualMemory/Roblox-Offsets-Website | https://github.com/NtReadVirtualMemory/Roblox-Offsets-Website | Web | `dumpers/NtReadVirtualMemory__Roblox-Offsets-Website/` |

## Update cadence

- `.github/workflows/mirror-all.yml` refreshes the `endpoints/*/` snapshots
  every 6 hours (`cron: 37 */6 * * *`) and takes a weekly source-code snapshot
  of every dumper repo at `dumpers/<owner>__<repo>/snapshot/` (Sunday 05:00 UTC).
- Manual refresh: `gh workflow run mirror-all.yml -R 4anti/Roblox-Offsets-ALL`.
- The workflow uses the `GITHUB_TOKEN` bot identity and produces backend-signed
  (Verified) commits.

## License notice

The mirror-repo code (README, CI, layout) is licensed under
[PolyForm Noncommercial 1.0.0](LICENSE). Snapshotted upstream dumper trees
under `dumpers/<owner>__<repo>/snapshot/` remain governed by their own
upstream licenses; see each folder's `README.md` for the upstream link and
SPDX identifier. If any upstream forbids redistribution, the snapshot leg is
suppressed and the folder README says so explicitly.
