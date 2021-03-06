---
title: "Developer Guide"
linkTitle: "Development"
weight: 90
description: >
  An introduction to ModMeta Relay's design and how to contribute.
---

ModMeta Relay is fully **open-source**! You can see the full code for the extension (including these docs) [on GitHub](https://github.com/agc93/modmeta-relay). Community contributions, fixes and PRs are all welcome! That being said, please read the info below to make all of our lives a bit easier.

## Licensing

ModMeta Relay is made available under an [MIT License](https://opensource.org/licenses/MIT). That means all contributions will also be licensed under the MIT License and all the conditions and limitations that involves.

## Development Environment

To work with the ModMeta Relay code, you should only need the .NET Core 3.1 SDK (or higher) and the `dotnet` CLI configured. Development so far has been done in Visual Studio Code, but any IDE that supports ASP.NET Core should work just fine.

## Building Locally

To get started building locally, you just need the `dotnet` CLI installed and available. Since we have a [Cake](https://cakebuild.net) script already set up, just run the following commands to get the build started:

```bash
dotnet tool restore
dotnet cake --bootstrap
dotnet cake
```

You can also run `dotnet cake --target=Publish` to build all the packages and images locally.

## Feature Requests

ModMeta Relay is a community project, currently built and maintained by a single non-developer. As such, feature requests will be accepted, but I can't provide any level of assurance that any requests will certainly be included.