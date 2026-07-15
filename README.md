<div align="center">
  <a href="https://github.com/4anti/Roblox-Fastflag-Manager">
    <img src="https://raw.githubusercontent.com/4anti/Roblox-Fastflag-Manager/main/logo.svg" alt="Roblox FastFlag Manager" width="600">
  </a>
</div>

<div align="center">

Companion repository for [Roblox FastFlag Manager](https://github.com/4anti/Roblox-Fastflag-Manager).

</div>

## Overview

This repository does two things:

1. It mirrors public Roblox FFlag offset endpoints and cross checks them against imtheo so FFM has a working fallback when a source is unreachable due to regional blocks, downtime, or TLS interception.
2. It archives the source code of every public offset dumper we could find, so builds remain reproducible if an upstream repository disappears.

Endpoint mirrors under `endpoints/<name>/` are refreshed every six hours. Dumper source snapshots under `dumpers/<owner>__<repo>/snapshot/` refresh once a week. Everything runs on GitHub Actions.

## Contribute

If you host a dumper, know one that is not listed, or want to share offsets you generated yourself, please open an issue on the main FFM repository:

- [New endpoint URL](https://github.com/4anti/Roblox-Fastflag-Manager/issues/new?labels=offset-source&template=new-offset-source.yml)
- [New dumper repo](https://github.com/4anti/Roblox-Fastflag-Manager/issues/new?labels=dumper-repo&template=new-dumper-repo.yml)
- Private or sensitive URLs: [Discord](https://discord.gg/ECekjAkQu7)

Submissions are fetched, sanity checked against a minimum size and a valid `namespace FFlagOffsets` block, and then cross checked against imtheo. Anything that passes is added under `endpoints/` here and picked up by FFM's fallback chain on the next release. Contributors are credited in the folder README on request.

## Endpoints

Each folder contains the mirrored HPP (plus `.json` if the upstream ships one) and a `meta.json` describing the last fetch and the imtheo cross check.

| Source | URL | Format | Folder | Consumed by FFM |
| --- | --- | --- | --- | --- |
| imtheo (stable) | https://offsets.imtheo.lol/FFlags.hpp | A | `endpoints/imtheo/` | Tier 2 |
| imtheo (dev) | https://dev.imtheo.lol/Offsets/FFlags.hpp | A | `endpoints/imtheo-dev/` | reserve |
| workers.dev | https://offsets.ntgetwritewatch.workers.dev/FFlags.hpp | B | `endpoints/workers-dev/` | Tier 4 |
| NtReadVirtualMemory | https://raw.githubusercontent.com/NtReadVirtualMemory/Roblox-Offsets-Website/main/FFlags.hpp | B | `endpoints/ntreadvirtualmemory/` | Tier 4 |
| souloveryall | https://raw.githubusercontent.com/souloveryall/offsets.hpp/main/Offsets.hpp | B | `endpoints/souloveryall/` | Tier 4 |

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

Endpoints are fetched on a `17 */6 * * *` cron. Dumper source snapshots run weekly on `0 5 * * 0`. Both can be triggered manually with `gh workflow run mirror-all.yml -R 4anti/Roblox-Offsets-ALL`. Every commit is made by `github-actions[bot]`, which is why the Verified badge is signed server side.

### Sanity gate

For every fetch:

- the response must contain `namespace FFlagOffsets`
- the response must contain at least 500 `uintptr_t` declarations
- for sources that also expose a `.json`, the JSON must parse and its `Total Offsets` value must be at least 500

If any check fails the previous good copy is retained, so a folder never regresses to a broken snapshot.

### Cross check against imtheo

After each fetch cycle the workflow parses every folder's HPP and compares it against `endpoints/imtheo/FFlags.hpp` using the same struct-stripping logic FFM uses in `offset_loader._extract_struct_offsets`. Every folder's `meta.json` gets:

- `verified`: `true`, `false`, or `null`
- `verification_reason`: one of `source_of_truth`, `matches_imtheo`, `version_skew`, `struct_offset_mismatch`, `insufficient_overlap`, `imtheo_baseline_missing`, `quorum_disagreement`, `rva_mismatch_rate_<N>pct`
- `verification_report`: supporting evidence such as intersection size, version strings, and a few sample mismatches

FFM's `_mirror_meta_verified` gate only consumes a body when the sibling `meta.json` reports `verified: true`. Anything else cascades silently to the next tier.

### Quorum override

If three or more non-imtheo folders come back with `verified: false` in the same run, imtheo itself is likely the outlier. The workflow sets `endpoints/imtheo/meta.json.verified` to `null` with `verification_reason: quorum_disagreement` and appends `verification_alert: imtheo_may_be_wrong` to the commit body. Commits continue to land so the situation stays visible for review.

### Manual override

Setting `FFM_TRUST_MIRROR=1` in the FFM process environment bypasses `_mirror_meta_verified` for the current session. This is useful when the mirror is known to be safe but the baseline is temporarily missing. The value is not persisted.

### `meta.json`

```
{
  "source_url":       str,
  "fetched_at":       str,        # ISO-8601 UTC
  "roblox_version":   str|null,   # first `version-<hex>` match in the HPP
  "flag_count":       int,        # count of `uintptr_t` declarations
  "sanity_passed":    bool,       # >=500 flags AND `namespace FFlagOffsets`
  "verified":         bool|null,
  "verification_reason": str,
  "verification_report": {
    "intersection_size":         int|null,
    "imtheo_version":            str|null,
    "folder_version":            str|null,
    "mismatched_struct_offsets": object|null,
    "rva_mismatch_rate":         float|null,
    "samples":                   list|null
  }
}
```

### Licensing

Files that belong to this repository (README, workflows, folder layout) are licensed under [PolyForm Noncommercial 1.0.0](LICENSE). Snapshotted dumper trees under `dumpers/<owner>__<repo>/snapshot/` remain governed by their upstream licenses. The SPDX identifier for each snapshot is listed in the folder's README. Snapshots are skipped when the upstream license prohibits redistribution, and the folder README says so.
