using System;
using System.IO;
using System.Text;
using Inedo.BuildMaster;
using Inedo.BuildMaster.Extensibility.Actions;
using Inedo.BuildMaster.Web;

namespace Inedo.BuildMasterExtensions.DotNet2
{
    /// <summary>
    /// Represents an action that precompiles a ASP.NET 2.0 Web Application.
    /// </summary>
    [ActionProperties(
        "Precompile ASP.NET Site",
        "Precompiles an ASP.NET (2.0 or later) site.",
        ".NET")]
    [CustomEditor(typeof(PrecompileAspNet2SiteEditor))]
    public sealed class PrecompileAspNet2Site : BuildNetActionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrecompileAspNet2Site"/> class.
        /// </summary>
        public PrecompileAspNet2Site()
        {
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
                "Precompile ASP.NET site in {0} to {1} with virtual path {2}",
                Util.CoalesceStr(this.OverriddenSourceDirectory, "default source directory"),
                Util.CoalesceStr(this.OverriddenTargetDirectory, "default target directory"),
                Util.CoalesceStr(this.ApplicationVirtualPath, "/")
            );
        }

        #region Properties
        /// <summary>
        /// Gets or sets the virtual path of the application to be compiled (e.g. "/MyApp"). 
        /// </summary>
        [Persistent]
        public string ApplicationVirtualPath { get; set; }

        /// <summary>
        /// Indicates that the precompiled application is updatable.
        /// </summary>
        [Persistent]
        public bool Updatable { get; set; }
        
        /// <summary>
        /// Indicates that the compiled assemblies will be given fixed names.
        /// </summary>
        [Persistent]
        public bool FixedNames { get; set; }
        #endregion

        /// <summary>
        /// This method is called to execute the Action.
        /// </summary>
        protected override void Execute()
        {
            string retVal = string.Empty;

            // Make sure virtual path starts with a /
            if (string.IsNullOrEmpty(this.ApplicationVirtualPath) || !this.ApplicationVirtualPath.StartsWith("/"))
                this.ApplicationVirtualPath = "/" + this.ApplicationVirtualPath;

            LogInformation("Pre-compiling site...");
            retVal = ExecuteRemoteCommand("PreCompile");
            if (retVal != "0")
            {
                throw new Exception(string.Format(
                    "Step Failed (aspnet_compiler returned code {0})",
                    retVal));
            }

        }

        /// <summary>
        /// When implemented in a derived class, processes an arbitrary command
        /// on the appropriate server.
        /// </summary>
        /// <param name="name">Name of command to process.</param>
        /// <param name="args">Optional command arguments.</param>
        /// <returns>
        /// Result of the command.
        /// </returns>
        protected override string ProcessRemoteCommand(string name, string[] args)
        {
            string retVal = string.Empty;

            switch (name)
            {
                case "PreCompile":
                    var cmdargs = new StringBuilder();
                    cmdargs.AppendFormat(" -v \"{0}\"", this.ApplicationVirtualPath);
                    cmdargs.AppendFormat(" -p {0}", GetShortPath(RemoteConfiguration.SourceDirectory));
                    if (this.Updatable) cmdargs.Append(" -u");
                    if (this.FixedNames) cmdargs.Append(" -fixednames");
                    cmdargs.AppendFormat(" {0}", GetShortPath(RemoteConfiguration.TargetDirectory));

                    retVal = ExecuteCommandLine(
                        GetAspNetCompilerPath(),
                        cmdargs.ToString(),
                        RemoteConfiguration.SourceDirectory);
                   break;

                default:
                    throw new ArgumentOutOfRangeException("name");
            }

            return retVal;
        }

        /// <summary>
        /// Returns a short path of a given path.
        /// </summary>
        /// <param name="path">Path to convert to a short path.</param>
        /// <returns>Short path of the specified path.</returns>
        private static string GetShortPath(string path)
        {
            var buffer = new StringBuilder(1000);
            NativeMethods.GetShortPathName(path, buffer, 1000);
            return buffer.ToString();
        }

        /// <summary>
        /// Gets the ASP.NET compiler path.
        /// </summary>
        /// <returns>The ASP.NET compiler path.</returns>
        private string GetAspNetCompilerPath()
        {
            var frameworkPath = GetFrameworkPath();
            return Path.Combine(frameworkPath, "aspnet_compiler.exe");
        }
    }
}
