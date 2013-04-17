using System;
using System.Configuration.Assemblies;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Inedo.BuildMasterExtensions.DotNet2.MetadataServices
{
    /// <summary>
    /// Contains methods for reading assembly metadata without using the managed reflection API.
    /// </summary>
    internal static class NativeMetadataInspector
    {
        private static readonly Guid CLSID_CorMetaDataDispenser = new Guid("E5CB7A31-7512-11d2-89CE-0080C792E5D8");

        /// <summary>
        /// Returns the full name of an assembly.
        /// </summary>
        /// <param name="assemblyFileName">File name of the assembly.</param>
        /// <returns>Full name of the assembly.</returns>
        public static AssemblyName GetAssemblyName(string assemblyFileName)
        {
            if (string.IsNullOrEmpty(assemblyFileName))
                throw new ArgumentNullException("assemblyFileName");

            var dispenser = (IMetaDataDispenser)Activator.CreateInstance(Type.GetTypeFromCLSID(CLSID_CorMetaDataDispenser, true));
            try
            {
                object unknownAsmImport;
                dispenser.OpenScope(assemblyFileName, CorOpenFlags.ofRead, typeof(IMetaDataAssemblyImport).GUID, out unknownAsmImport);
                try
                {
                    var assemblyImport = (IMetaDataAssemblyImport)unknownAsmImport;

                    uint assemblyToken;
                    assemblyImport.GetAssemblyFromScope(out assemblyToken);

                    ASSEMBLYMETADATA assemblyMetadata;
                    var assemblyName = new StringBuilder(500);
                    IntPtr publicKey;
                    uint publicKeySize;
                    uint publicKeyAlgorithm;
                    uint nameLength = (uint)assemblyName.Capacity;
                    CorAssemblyFlags assemblyFlags;
                    assemblyImport.GetAssemblyProps(assemblyToken, out publicKey, out publicKeySize, out publicKeyAlgorithm, assemblyName, nameLength, out nameLength, out assemblyMetadata, out assemblyFlags);

                    var assemblyNameInst = new AssemblyName
                    {
                        Name = assemblyName.ToString(),
                        Version = new Version(assemblyMetadata.usMajorVersion, assemblyMetadata.usMinorVersion, assemblyMetadata.usBuildNumber, assemblyMetadata.usRevisionNumber)
                    };

                    if (publicKey != IntPtr.Zero && publicKeySize > 0)
                    {
                        var publicKeyBytes = new byte[publicKeySize];
                        Marshal.Copy(publicKey, publicKeyBytes, 0, publicKeyBytes.Length);

                        assemblyNameInst.HashAlgorithm = (AssemblyHashAlgorithm)publicKeyAlgorithm;
                        assemblyNameInst.SetPublicKey(publicKeyBytes);
                    }

                    return assemblyNameInst;
                }
                finally
                {
                    Marshal.FinalReleaseComObject(unknownAsmImport);
                }
            }
            finally
            {
                Marshal.FinalReleaseComObject(dispenser);
            }
        }
    }
}
