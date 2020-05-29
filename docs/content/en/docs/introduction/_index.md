---
title: "Introduction"
linkTitle: "Introduction"
weight: 1
description: >
  A basic introduction to ModMeta Relay
---

ModMeta Relay is an **unofficial** implementation of the ModMeta API for interfacing Vortex (or any other ModMeta client) with any non-Nexus mod source. To clarify, this project is not affiliated in any way with Nexus Mods or anyone else, and is an open-source community resource.

## Status and Limitations

The ModMeta Relay project is very new and is largely untested! That being said, I believe it to be functioning at a basic level and both the REST and plugin API are reasonably stable. The following is definitely supported at this point:

- **REST GET API**: The basic set of `GET` endpoints for the ModMeta REST API are supported through plugins (see below)
- **Plugins**: the ModMeta Relay can load any valid plugin at runtime to support other sources

There's a few things that we specifically don't have (yet):

- **`POST`/Describe API**: The Plugin API doesn't currently have any support for *saving* mod metadata, so the Describe endpoints are not available.
- **Shared Services**: Plugins currently have to implement a lot of their own functionality and shared services. This is on the roadmap.
- **Error Handling**: The server's error handling is pretty barebones. You might see more errors and odd behaviours surface.