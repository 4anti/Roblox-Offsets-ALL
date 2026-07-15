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

## Contribute

Have your own dumper, a private FFlag endpoint, or a public one that isn't
listed? Anything from a raw `FFlags.hpp` URL to a full dumper repo, from a
Discord-only paste to a self-hosted mirror — we want them all so American /
region-blocked users have real fallbacks.

The **primary contribution channel** while this repo is private is the public
FFM repo's issue tracker. Two ready-made forms:

- **New endpoint URL** — one HTTP(S) URL returning `FFlags.hpp` or
  `Offsets.hpp` in either Format A (nested `FFlagList`) or Format B
  (flat `FFlagOffsets`):
  [File a new-offset-source issue](https://github.com/4anti/Roblox-Fastflag-Manager/issues/new?labels=offset-source&template=new-offset-source.yml)
- **New dumper repo** — a GitHub (or other public git) repository that dumps
  offsets, whether you wrote it or found it:
  [File a new-dumper-repo issue](https://github.com/4anti/Roblox-Fastflag-Manager/issues/new?labels=dumper-repo&template=new-dumper-repo.yml)
- **Private / sensitive endpoint** — please DM the maintainer on Discord
  (`https://discord.gg/ECekjAkQu7`) before opening a public issue so the
  URL never lands in the public tracker.

What we do with a submission:

1. Sanity-check the URL / repo: fetch, run FFM's `>=500 uintptr_t` +
   `namespace FFlagOffsets` regex gate, confirm it parses in both formats.
2. Cross-check the offsets against imtheo (Combined-repo CI does this
   automatically) so we know the source isn't shipping garbage RVAs.
3. If it passes, add it to `endpoints/` here and to the operational mirror
   at [Roblox-Offsets-Combined](https://github.com/4anti/Roblox-Offsets-Combined).
   FFM's fallback chain picks it up on the next release.
4. Credit you in the folder README (`Contributed by @<your-handle>`) unless
   you'd rather stay anonymous — say so in the issue.

Once these mirror repos flip public (post-baseline analysis), issues here
will be enabled directly and you'll be able to open them against
`4anti/Roblox-Offsets-ALL` instead of the main FFM repo.

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
