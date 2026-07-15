<div align="center">
  <a href="https://github.com/4anti/Roblox-Fastflag-Manager">
    <img src="https://raw.githubusercontent.com/4anti/Roblox-Fastflag-Manager/main/logo.svg" alt="Roblox FastFlag Manager" width="600">
  </a>
</div>

<div align="center">

**Public archive of Roblox FFlag offset dumpers.**

Sister repo for [Roblox FastFlag Manager](https://github.com/4anti/Roblox-Fastflag-Manager).

</div>

---

## What this is

One folder per public FFlag endpoint, one folder per public dumper repo. CI refreshes the endpoint snapshots every six hours and takes a full source snapshot of each dumper once a week. No user data, no telemetry, no runtime code. If someone geoblocks imtheo, FFM still has options.

## Contribute a source

If you run a dumper, host a mirror, or just know one that isn't here yet, please open an issue on the main FFM repo. There are ready-made forms:

- [Add a new endpoint URL](https://github.com/4anti/Roblox-Fastflag-Manager/issues/new?labels=offset-source&template=new-offset-source.yml)
- [Add a new dumper repo](https://github.com/4anti/Roblox-Fastflag-Manager/issues/new?labels=dumper-repo&template=new-dumper-repo.yml)
- For private endpoints or anything you'd rather not post publicly, DM us on [Discord](https://discord.gg/ECekjAkQu7).

Every submission is checked: fetch it, sanity gate it (at least 500 offsets, must contain `namespace FFlagOffsets`), and cross-check against imtheo. If it holds up, it goes into the operational mirror at [Roblox-Offsets-Combined](https://github.com/4anti/Roblox-Offsets-Combined) and starts feeding FFM's fallback chain on the next release. Happy to credit you in the folder README if you leave a handle.

---

## Endpoints

| Source | URL | Format | Folder |
| --- | --- | --- | --- |
| imtheo (stable) | https://offsets.imtheo.lol/FFlags.hpp | A | `endpoints/imtheo/` |
| imtheo (dev) | https://dev.imtheo.lol/Offsets/FFlags.hpp | A | `endpoints/imtheo-dev/` |
| workers.dev | https://offsets.ntgetwritewatch.workers.dev/FFlags.hpp | B | `endpoints/workers-dev/` |
| NtReadVirtualMemory | https://raw.githubusercontent.com/NtReadVirtualMemory/Roblox-Offsets-Website/main/FFlags.hpp | B | `endpoints/ntreadvirtualmemory/` |
| souloveryall | https://raw.githubusercontent.com/souloveryall/offsets.hpp/main/Offsets.hpp | B | `endpoints/souloveryall/` |

## Dumper source repos

Weekly source snapshots land in `dumpers/<owner>__<repo>/snapshot/`.

| Repo | Language | Folder |
| --- | --- | --- |
| [pizzaboxer/rbxfflagdumper](https://github.com/pizzaboxer/rbxfflagdumper) | C++ | `dumpers/pizzaboxer__rbxfflagdumper/` |
| [Srungot/FFlags-dumper-roblox](https://github.com/Srungot/FFlags-dumper-roblox) | Python | `dumpers/Srungot__FFlags-dumper-roblox/` |
| [nopjo/roblox-dumper](https://github.com/nopjo/roblox-dumper) | Python | `dumpers/nopjo__roblox-dumper/` |
| [henrick001/External-Dumper](https://github.com/henrick001/External-Dumper) | C++ | `dumpers/henrick001__External-Dumper/` |
| [nhisoka/Roblox-Offset-Dumper](https://github.com/nhisoka/Roblox-Offset-Dumper) | C++ | `dumpers/nhisoka__Roblox-Offset-Dumper/` |
| [Lonegwadiwaitor/roblox-offset-dumper](https://github.com/Lonegwadiwaitor/roblox-offset-dumper) | C++ | `dumpers/Lonegwadiwaitor__roblox-offset-dumper/` |
| [NtReadVirtualMemory/Roblox-Offsets-Website](https://github.com/NtReadVirtualMemory/Roblox-Offsets-Website) | Web | `dumpers/NtReadVirtualMemory__Roblox-Offsets-Website/` |

---

## Details

**Formats.** Format A means a nested `namespace FFlagList { ... }` inside `namespace FFlagOffsets`. Format B keeps flag entries flat inside `namespace FFlagOffsets`. FFM parses both.

**Update cadence.** Endpoints refresh every 6 hours (`cron: 37 */6 * * *`). Dumper source snapshots refresh weekly (`cron: 0 5 * * 0`). Manual trigger: `gh workflow run mirror-all.yml -R 4anti/Roblox-Offsets-ALL`. Commits come from `github-actions[bot]`, which is how the Verified badge is server-signed.

**Licenses.** Files that belong to this repo (README, workflows, layout) are under [PolyForm Noncommercial 1.0.0](LICENSE). Snapshotted dumper trees under `dumpers/<owner>__<repo>/snapshot/` keep their upstream licenses. Each folder's README carries the SPDX. If an upstream forbids redistribution the snapshot leg is skipped and the folder README says so.
