using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Inedo.BuildMaster;
using Inedo.BuildMaster.Diagnostics;
using Inedo.BuildMaster.Proxies;

namespace Inedo.BuildMasterExtensions.DotNet2
{
    public class DotNet2Helper : AgentProxyableObject<DotNet2Helper>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNet2Helper"/> class.
        /// </summary>
        public DotNet2Helper()
        {
        }

        public static DotNet2Helper Proxy(int serverId)
        {
            DotNet2Helper helper = new DotNet2Helper();
            helper = (DotNet2Helper)helper.CreateRemoteProxy(serverId);
            return helper;
        }

        public bool IsMageAvailable(string sdkPath)
        {
            if (string.IsNullOrEmpty(sdkPath))
                return false;

            return File.Exists(Path.Combine(sdkPath, @"bin\mage.exe"));
        }
      
        #region MSBuild Targets
        /// <summary>
        /// Ensures that MSBuild web application targets are present.
        /// </summary>
        public static void EnsureMsBuildWebTargets()
        {
            WriteAllTargets(
                @"MSBuild\Microsoft\VisualStudio\v8.0\WebApplications\",
                 "Inedo.BuildMasterExtensions.DotNet2.Targets.Microsoft.WebApplication.targets",
                 "Microsoft.WebApplication.targets"
                );

            WriteAllTargets(
                @"MSBuild\Microsoft\VisualStudio\v8.0\WebApplications\",
                 "Inedo.BuildMasterExtensions.DotNet2.Targets.Microsoft.ReportingServices.targets",
                 "Microsoft.ReportingServices.targets"
                );

            WriteAllTargets(
                @"MSBuild\Microsoft\VisualStudio\v9.0\WebApplications\",
                 "Inedo.BuildMasterExtensions.DotNet2.Targets.Microsoft.WebApplication35.targets",
                 "Microsoft.WebApplication.targets"
                );

            WriteAllTargets(
                @"MSBuild\Microsoft\VisualStudio\v10.0\WebApplications\",
                "Inedo.BuildMasterExtensions.DotNet2.Targets.Microsoft.WebApplication40.targets",
                 "Microsoft.WebApplication.targets"
                );

            WriteAllTargets(
                @"MSBuild\Microsoft\VisualStudio\v10.0\WebApplications\",
                 "Inedo.BuildMasterExtensions.DotNet2.Targets.Microsoft.WebApplication40.Build.Tasks.Dll",
                 "Microsoft.WebApplication.Build.Tasks.Dll"
                );
        }

        /// <summary>
        /// Writes build targets to 32 and 64 bit program files directories if necessary.
        /// </summary>
        /// <param name="path">MSBuild path under program files to write to.</param>
        /// <param name="resource">Name of resource to write.</param>
        /// <param name="fileName">Name of file to write.</param>
        private static void WriteAllTargets(string path, string resource, string fileName)
        {
            var x86Path = GetProgramFilesx86Path();
            var nativePath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

            WriteTargets(Path.Combine(x86Path, path), resource, fileName);
            if (x86Path != nativePath)
                WriteTargets(Path.Combine(nativePath, path), resource, fileName);
        }
        private static void WriteTargets(string targetDirectory, string resource, string fileName)
        {
            var targetPath = Path.Combine(targetDirectory, fileName);

            if (!File.Exists(targetPath))
            {
                try
                {
                    Directory.CreateDirectory(targetDirectory);

                    Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource);
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);

                    File.WriteAllBytes(targetPath, buffer);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.ToString());
                    throw new Exception("Web Application Targets are unavailable.", ex);
                }
            }
        }
        #endregion

        /// <summary>
        /// Returns the x86 Program Files path.
        /// </summary>
        /// <returns>The x86 Program Files path.</returns>
        private static string GetProgramFilesx86Path()
        {
            if (IntPtr.Size == 8)
            {
                var buffer = new StringBuilder(1024);
                NativeMethods.SHGetFolderPath(IntPtr.Zero, NativeMethods.CSIDL_PROGRAM_FILESX86, IntPtr.Zero, 0, buffer);
                return buffer.ToString();
            }

            return Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        }

        private static class NativeMethods
        {
            [DllImport("shell32.dll")]
            public static extern int SHGetFolderPath(IntPtr hwndOwner, int nFolder, IntPtr hToken, uint dwFlags, StringBuilder pszPath);

            public const int CSIDL_PROGRAM_FILES = 0x0026;
            public const int CSIDL_PROGRAM_FILESX86 = 0x002a;
        }
    }
}
