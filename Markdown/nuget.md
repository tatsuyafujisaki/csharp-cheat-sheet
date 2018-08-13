# Best practices
* Don't bother to prepare `.nuspec` to create `.nupkg` because `.nuspec` is optional.
  * But then, you need to familiarize yourself with `.nuspec` because `.nuspec` is automatically created inside `.nupkg`.

# Create `.nupkg` from `.csproj`

## If using Visual Studio 2017 Update 3 or higher
```batch
cd folder_that_contains_csproj
msbuild /t:pack /p:Configuration=Release
```

## If not using Visual Studio 2017 Update 3 or higher
```batch
nuget pack Foo.csproj -Properties Configuration=Release
```

# Required elements in `.nuspec`
* `<version>` is sourced from `AssemblyVersion`. Not `AssemblyFileVersion`.
* `<description>` is sourced from `AssemblyDescription`.
* `<authors>` is sourced from `AssemblyCompany`.

# References
* [.nuspec reference](https://docs.microsoft.com/en-us/nuget/schema/nuspec)
* [Create .NET Standard 2.0 packages with Visual Studio 2017](https://docs.microsoft.com/en-us/nuget/guides/create-net-standard-packages-vs2017)