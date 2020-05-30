Task("Publish-Docker-Image")
.IsDependentOn("Build-Docker-Image")
.WithCriteria(() => !string.IsNullOrWhiteSpace(EnvironmentVariable("QUAY_TOKEN")))
.WithCriteria(() => EnvironmentVariable("GITHUB_REF").StartsWith("refs/tags/v"))
.Does(() => {
    var token = EnvironmentVariable("QUAY_TOKEN");
    DockerLogin(new DockerRegistryLoginSettings{
        Password = token,
        Username = EnvironmentVariable("QUAY_USER") ?? "modmeta_build"
    }, "quay.io");
    DockerPush($"quay.io/modmeta-relay/server:{packageVersion}");
});

Task("Release")
.IsDependentOn("Publish")
.IsDependentOn("Publish-Docker-Image");