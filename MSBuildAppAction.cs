using System;
using System.IO;
using Inedo.BuildMaster;
using Inedo.BuildMaster.Extensibility.Actions;
using Inedo.BuildMaster.Web;

namespace Inedo.BuildMasterExtensions.DotNet2
{
    /// <summary>
    /// Represents an action that builds an MSBuild project.
    /// </summary>
    [ActionProperties(
        "Build .NET MSBuild Project",
        "Builds a .NET project using a .msbuild file.",
        ".NET")]
    [CustomEditor(typeof(MSBuildAppActionEditor))]
    public sealed class MSBuildAppAction : BuildNetActionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MSBuildAppAction"/> class.
        /// </summary>
        public MSBuildAppAction()
        {
            this.MSBuildProperties = string.Empty;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(
                "Build {0} Target:{1} Properties:{2}",
                Path.GetFileName(this.MSBuildPath),
                this.ProjectBuildTarget,
                Util.CoalesceStr(string.Join(
                    ";",
                    this.MSBuildProperties.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                ), "(none)")
            );
        }

        /// <summary>
        /// Gets or sets the project's target for the msbuild script
        /// </summary>
        [Persistent]
        public string ProjectBuildTarget { get; set; }

        /// <summary>
        /// Gets or sets the project's path for the msbuild script
        /// </summary>
        [Persistent]
        public string MSBuildPath { get; set; }

        /// <summary>
        /// Gets or sets the project's properties for the msbuild script
        /// </summary>
        [Persistent]
        public string MSBuildProperties { get; set; }

        protected override void Execute()
        {
            var retval = ExecuteRemoteCommand(null);
            if (retval != "0")
                this.LogError("MSBuild action failed; msbuild.exe returned code " + retval);
        }

        protected override string ProcessRemoteCommand(string name, string[] args)
        {
            var projectFileName = Path.Combine(this.RemoteConfiguration.SourceDirectory, this.MSBuildPath);

            // Parse build properties from:
            //      prop1=val1
            //      prop2=val2
            // to:
            //      prop1=val1;prop2=val2
            var buildProperties = string.Join(
                ";",
                this.MSBuildProperties.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
            );

            //Execute msbuild script
            //Format: MSBuild {projectFileName} /t:{ProjectBuildTarget}
            return this.InvokeMSBuild(
                string.Format(
                    " \"{0}\" \"/t:{1}\" \"/p:outDir={2}{3}\"",
                    projectFileName,
                    this.ProjectBuildTarget,
                    this.RemoteConfiguration.TargetDirectory.EndsWith("\\") ?
                        this.RemoteConfiguration.TargetDirectory :
                        this.RemoteConfiguration.TargetDirectory + "\\",
                    Util.ConcatNE(";", buildProperties)
                ),
                this.RemoteConfiguration.SourceDirectory
            )
            .ToString();
        }
    }
}
