<div align="center">
  <a href="https://github.com/4anti/Roblox-Fastflag-Manager">
    <img src="https://raw.githubusercontent.com/4anti/Roblox-Fastflag-Manager/main/logo.svg" alt="Roblox FastFlag Manager" width="600">
  </a>
</div>

<div align="center">

Companion repository for [Roblox FastFlag Manager](https://github.com/4anti/Roblox-Fastflag-Manager).

</div>

## Overview

This repository archives publicly available Roblox FFlag offset sources. Its purpose is to give FFM users a working fallback when a particular dumper is unreachable due to regional blocks, downtime, or TLS interception. The layout is one folder per mirrored endpoint plus one folder per snapshotted dumper repository.

Endpoint folders are refreshed every six hours. Dumper source snapshots refresh once a week. Everything runs on GitHub Actions.

## Contribute

If you host a dumper, know one that is not listed, or want to share offsets you generated yourself, please open an issue on the main FFM repository:

- [New endpoint URL](https://github.com/4anti/Roblox-Fastflag-Manager/issues/new?labels=offset-source&template=new-offset-source.yml)
- [New dumper repo](https://github.com/4anti/Roblox-Fastflag-Manager/issues/new?labels=dumper-repo&template=new-dumper-repo.yml)
- Private or sensitive URLs: [Discord](https://discord.gg/ECekjAkQu7)

Submissions are fetched, sanity checked against a minimum size and a valid `namespace FFlagOffsets` block, and then cross checked against imtheo. Anything that passes is added to [Roblox-Offsets-Combined](https://github.com/4anti/Roblox-Offsets-Combined) and picked up by FFM's fallback chain on the next release. Contributors are credited in the folder README on request.

## Endpoints

| Source | URL | Format | Folder |
| --- | --- | --- | --- |
| imtheo (stable) | https://offsets.imtheo.lol/FFlags.hpp | A | `endpoints/imtheo/` |
| imtheo (dev) | https://dev.imtheo.lol/Offsets/FFlags.hpp | A | `endpoints/imtheo-dev/` |
| workers.dev | https://offsets.ntgetwritewatch.workers.dev/FFlags.hpp | B | `endpoints/workers-dev/` |
| NtReadVirtualMemory | https://raw.githubusercontent.com/NtReadVirtualMemory/Roblox-Offsets-Website/main/FFlags.hpp | B | `endpoints/ntreadvirtualmemory/` |
| souloveryall | https://raw.githubusercontent.com/souloveryall/offsets.hpp/main/Offsets.hpp | B | `endpoints/souloveryall/` |

## Dumper repositories

Weekly source snapshots land under `dumpers/<owner>__<repo>/snapshot/`.

| Repository | Language | Folder |
| --- | --- | --- |
| [pizzaboxer/rbxfflagdumper](https://github.com/pizzaboxer/rbxfflagdumper) | C++ | `dumpers/pizzaboxer__rbxfflagdumper/` |
| [Srungot/FFlags-dumper-roblox](https://github.com/Srungot/FFlags-dumper-roblox) | Python | `dumpers/Srungot__FFlags-dumper-roblox/` |
| [nopjo/roblox-dumper](https://github.com/nopjo/roblox-dumper) | Python | `dumpers/nopjo__roblox-dumper/` |
| [henrick001/External-Dumper](https://github.com/henrick001/External-Dumper) | C++ | `dumpers/henrick001__External-Dumper/` |
| [nhisoka/Roblox-Offset-Dumper](https://github.com/nhisoka/Roblox-Offset-Dumper) | C++ | `dumpers/nhisoka__Roblox-Offset-Dumper/` |
| [Lonegwadiwaitor/roblox-offset-dumper](https://github.com/Lonegwadiwaitor/roblox-offset-dumper) | C++ | `dumpers/Lonegwadiwaitor__roblox-offset-dumper/` |
| [NtReadVirtualMemory/Roblox-Offsets-Website](https://github.com/NtReadVirtualMemory/Roblox-Offsets-Website) | Web | `dumpers/NtReadVirtualMemory__Roblox-Offsets-Website/` |

## Additional information

### Formats

Format A places the flag table inside a nested `namespace FFlagList { ... }` block within `namespace FFlagOffsets`. Format B keeps every entry directly inside `namespace FFlagOffsets` with no nesting. FFM's parser accepts either.

### Update cadence

Endpoints are fetched on a `37 */6 * * *` cron. Dumper source snapshots run weekly on `0 5 * * 0`. Both can be triggered manually with `gh workflow run mirror-all.yml -R 4anti/Roblox-Offsets-ALL`. Every commit is made by `github-actions[bot]`, which is why the Verified badge is signed server side.

### Licensing

Files that belong to this repository (README, workflows, folder layout) are licensed under [PolyForm Noncommercial 1.0.0](LICENSE). Snapshotted dumper trees under `dumpers/<owner>__<repo>/snapshot/` remain governed by their upstream licenses. The SPDX identifier for each snapshot is listed in the folder's README. Snapshots are skipped when the upstream license prohibits redistribution, and the folder README says so.
