---
title: "ModMeta API"
linkTitle: "ModMeta API"
weight: 91
description: >
  An overview of what the ModMeta API is for
---

For plugin authors, the most important things to know about the ModMeta API are the request types and version matching.

### Request Types

#### Key (`by_key`)

This is the "default" request used by Vortex, and the only request supported by Nexus Mods. It's also the simplest and requests metadata using a single key: the MD5 hash of the mod archive file. This is super useful for mod sources that store a hash as it's fast, unique and simple.

#### Logical File Name (`by_name`)

This is the next-best request, not as specific as file key, but not as vague or intensive as expression. Technically speaking, the logical file name is just a string, but it's usually a string that's semantically important to the mod, such as a unique name for a specific variant of a file.

> Logical file name lookups also use version matching

#### File Expression (`by_expression`)

File expressions are, essentially, glob patterns. That makes them very flexible and able to work with practically any file/mod format, but also fragile and intensive to look up, so they are considered a last-ditch option for when the other two are not possible.

> File expression lookups also use version matching

### Version Matching

Lookups using logical file name or file expression also use a version string to match specific versions of a file. Version matches are not (necessarily) an exact match and are best specified as a version range.

If you've used Node/`npm` and it's version semantics before, that's how `modmeta-db` does its version matching. ModMeta Relay tries to match that as closely as possible, but for those curious, existing plugins are using [semver.net (Sematnic Versioning for .NET)](https://github.com/adamreeve/semver.net) for version matching.

The actual implementation of matching version strings with mod package/file versions is up to plugins, but should always be as specific as practical since returning too many results for a mod can result in conflicting metadata.