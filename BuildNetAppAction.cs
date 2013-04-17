using System;
using System.IO;
using Inedo.BuildMaster;
using Inedo.BuildMaster.Extensibility.Actions;
using Inedo.BuildMaster.Web;

namespace Inedo.BuildMasterExtensions.DotNet2
{
    /// <summary>
    /// Represents an action that builds a .NET application.
    /// </summary>
    [ActionProperties(
        "Build .NET Project",
        "Builds a .NET project or solution using MSBuild.",
        ".NET")]
    [CustomEditor(typeof(BuildNetAppActionEditor))]
    public sealed class BuildNetAppAction : BuildNetActionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuildNetAppAction"/> class.
        /// </summary>
        public BuildNetAppAction()
        {
        }

        /// <summary>
        /// Gets or sets the project's build configuration (generally, Debug or Release)
        /// </summary>
        [Persistent]
        public string ProjectBuildConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the project's target platform (generally AnyCPU or x86).
        /// </summary>
        [Persistent]
        public string ProjectTargetPlatform { get; set; }

        /// <summary>
        /// Gets or sets the absolute path the project file
        /// </summary>
        /// <example>
        /// c:\Build\myproj\myproj.cs
        /// </example>
        [Persistent]
        public string ProjectPath { get; set; }

        /// <summary>
        /// Gets or sets the project's properties for the msbuild script
        /// </summary>
        [Persistent]
        public string MSBuildProperties { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a web project.
        /// </summary>
        /// <value>
        /// <c>true</c> if this is a web project; otherwise, <c>false</c>.
        /// </value>
        [Persistent]
        public bool IsWebProject { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [build to project config subdirectories].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [build to project config subdirectories]; otherwise, <c>false</c>.
        /// </value>
        [Persistent]
        public bool BuildToProjectConfigSubdirectories { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var sourceDir = Util.CoalesceStr(this.OverriddenSourceDirectory, "default directory");
            var targetDir = this.BuildToProjectConfigSubdirectories
                ? string.Format("the \\bin\\{{config}} subdirectory of {0}", this.ProjectPath.EndsWith(".sln") ? "each project in the solution" : "the project")
                : Util.CoalesceStr(this.OverriddenTargetDirectory, "default directory");
            
            var fileName = Path.GetFileName(this.ProjectPath);

            var config = this.ProjectBuildConfiguration;
            if(!string.IsNullOrEmpty(this.ProjectTargetPlatform))
                config += "; " + this.ProjectTargetPlatform;

            if (string.IsNullOrEmpty(this.DotNetVersion))
                return string.Format("Build ({0}) {1} from {2} to {3}", config, fileName, sourceDir, targetDir);
            else
                return string.Format("Build ({0}) {1} using .NET {2} from {3} to {4}", config, fileName, this.DotNetVersion, sourceDir, targetDir);
        }

        protected override void Execute()
        {
            string retVal = string.Empty;

            LogDebug("Building Application");
            retVal = ExecuteRemoteCommand("Build");
            if (retVal != "0")
            {
                throw new Exception(string.Format(
                    "Step Failed (msbuild returned code {0})",
                    retVal));
            }

            if (this.IsWebProject)
            {
                LogDebug("Copying Web Files");
                retVal = ExecuteRemoteCommand("CopyWeb");
                if (retVal != "0")
                {
                    throw new Exception(string.Format(
                        "Step Failed (msbuild returned code {0})",
                        retVal));
                }

                LogDebug("Copying References");
                retVal = ExecuteRemoteCommand("CopyRef");
                if (retVal != "0")
                {
                    throw new Exception(string.Format(
                        "Step Failed (msbuild returned code {0})",
                        retVal));
                }
            }

        }

        protected override string ProcessRemoteCommand(string name, string[] args)
        {
            string projectFullPath = Path.Combine(RemoteConfiguration.SourceDirectory, ProjectPath);

            var buildProperties =
                string.Join(
                    ";",
                    (this.MSBuildProperties ?? "").Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
            );

            LogDebug(string.Format("  Action: {0}; Path: {1}", name, projectFullPath));

            var config = "Configuration=" + this.ProjectBuildConfiguration;
            if (!string.IsNullOrEmpty(this.ProjectTargetPlatform))
                config += ";Platform=" + this.ProjectTargetPlatform;

            if (!string.IsNullOrEmpty(buildProperties))
                config += ";" + buildProperties;

            switch (name)
            {
                case "Build":
                    try
                    {
                        DotNet2Helper.EnsureMsBuildWebTargets();
                    }
                    catch
                    {
                        //gulp
                    }
                    return msbuild(" \"{0}\" \"/p:{1}\""
                        + (this.IsWebProject || this.BuildToProjectConfigSubdirectories ? "" : "  \"/p:OutDir={2}\\\""),
                        projectFullPath,
                        config,
                        this.RemoteConfiguration.TargetDirectory.EndsWith(Path.DirectorySeparatorChar.ToString())
                            ? this.RemoteConfiguration.TargetDirectory
                            : this.RemoteConfiguration.TargetDirectory + Path.DirectorySeparatorChar
                        );

                case "CopyWeb":
                    return msbuild(" \"{0}\" /target:_CopyWebApplication \"/p:OutDir={1}\\\" \"/p:WebProjectOutputDir={1}\\\" \"/p:{2}\"",
                        projectFullPath,
                        this.RemoteConfiguration.TargetDirectory.EndsWith(Path.DirectorySeparatorChar.ToString())
                            ? this.RemoteConfiguration.TargetDirectory
                            : this.RemoteConfiguration.TargetDirectory + Path.DirectorySeparatorChar,
                        config);
                    
                case "CopyRef":
                    return msbuild(" \"{0}\" /target:ResolveReferences \"/property:OutDir={1}\\\" \"/p:{2}\"",
                        projectFullPath,
                        Path.Combine(this.RemoteConfiguration.TargetDirectory, "bin" + Path.DirectorySeparatorChar),
                        config);
                    
                default:
                    throw new ArgumentOutOfRangeException("name");
            }
        }

        private string msbuild(string argsFormat, params string[] args)
        {
            var allArgs = string.Format(argsFormat, args);

            var workingDir = Path.Combine(
                this.RemoteConfiguration.SourceDirectory,
                Path.GetDirectoryName(this.ProjectPath)
            );

            if (!Directory.Exists(workingDir))
                throw new DirectoryNotFoundException(string.Format("Directory {0} does not exist.", workingDir));

            return this.InvokeMSBuild(
                allArgs,
                workingDir
            )
            .ToString();
        }
    }
}
