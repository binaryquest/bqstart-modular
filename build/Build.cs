using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;

class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    [Parameter] string NugetApiUrl = "https://api.nuget.org/v3/index.json"; //default
    [Parameter] string NugetApiKey;

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;
    [GitVersion] readonly GitVersion GitVersion;

    AbsolutePath SourceDirectory => RootDirectory / "backend" / "bqstart-modular";
    AbsolutePath TestsDirectory => RootDirectory / "backend" / "bqstart-modular";
    AbsolutePath OutputDirectory => RootDirectory / "backend" / "output";
    AbsolutePath PackagesDirectory => RootDirectory / "backend" / "output" / "packages";


    public static int Main () => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    Target Info => _ => _        
        .Executes(() =>
        {
            Serilog.Log.Information($"Root Folder {RootDirectory} {GitVersion.NuGetVersionV2} {Configuration}");
        });

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {            
            SourceDirectory.GlobFiles("**/bin/**/*.nupkg").DeleteFiles();
            OutputDirectory.CreateOrCleanDirectory();
            PackagesDirectory.CreateOrCleanDirectory();
        });

    Target Restore => _ => _        
        .Executes(() =>
        {
            DotNetTasks.DotNetRestore(s => s
               .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            Serilog.Log.Information($"Root Folder {RootDirectory} {GitVersion} {Configuration}");
            DotNetTasks.DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration.Release)
                .SetAssemblyVersion(GitVersion.AssemblySemVer)
                .SetFileVersion(GitVersion.AssemblySemFileVer)
                .SetVersion(GitVersion.NuGetVersionV2)
                .SetInformationalVersion(GitVersion.InformationalVersion)
                .EnableNoRestore());
        });

    Target Pack => _ => _
        .DependsOn(Compile)
        .Produces(PackagesDirectory / "*.nupkg")
        .Executes(() =>
        {
            PackagesDirectory.CreateOrCleanDirectory();
            SourceDirectory.GlobFiles("**/bin/release/*.nupkg").ForEach(x => CopyFileToDirectory(x, PackagesDirectory));
        });

    Target Push => _ => _
       .DependsOn(Pack)
       .Requires(() => NugetApiUrl)
       .Requires(() => NugetApiKey)
       .Requires(() => Configuration.Equals(Configuration.Release))
       .Executes(() =>
       {
           PackagesDirectory.GlobFiles(PackagesDirectory, "BinaryQuest.Framework.ModularCore.*.nupkg")
               .ForEach(x =>
               {
                   DotNetTasks.DotNetNuGetPush(s => s
                       .SetTargetPath(x)
                       .SetSource(NugetApiUrl)
                       .SetApiKey(NugetApiKey)
                   );
               });
       });
}
