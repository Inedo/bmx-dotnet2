using System;
using System.Runtime.InteropServices;

namespace Inedo.BuildMasterExtensions.DotNet2.MetadataServices
{
    /// <summary>
    /// Provides methods to create a new metadata scope, or open an existing one.
    /// </summary>
    [ComImport]
    [Guid("809C652E-7396-11D2-9771-00A0C9B4D50C")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IMetaDataDispenser
    {
        /// <summary>
        /// Creates a new area in memory in which you can create new metadata.
        /// </summary>
        /// <param name="rclsid">[in] The CLSID of the version of metadata structures to be created. This value must be CLSID_CorMetaDataRuntime.</param>
        /// <param name="dwCreateFlags">[in] Flags that specify options. This value must be zero.</param>
        /// <param name="riid">
        /// [in] The IID of the desired metadata interface to be returned; the caller will use the interface to create the new metadata.
        /// The value of riid must specify one of the "emit" interfaces. Valid values are IID_IMetaDataEmit, IID_IMetaDataAssemblyEmit, or IID_IMetaDataEmit2. 
        /// </param>
        /// <param name="ppIUnk">[out] The pointer to the returned interface.</param>
        /// <remarks>
        /// STDMETHOD(DefineScope)(         // Return code.
        ///     REFCLSID    rclsid,         // [in] What version to create.
        ///     DWORD       dwCreateFlags,      // [in] Flags on the create.
        ///     REFIID      riid,           // [in] The interface desired.
        ///     IUnknown    **ppIUnk) PURE;     // [out] Return interface on success.
        /// </remarks>
        [PreserveSig]
        void DefineScope(
            [In] ref Guid rclsid,
            [In] uint dwCreateFlags,
            [In] ref Guid riid,
            [Out, MarshalAs(UnmanagedType.Interface)] out object ppIUnk);

        /// <summary>
        /// Opens an existing, on-disk file and maps its metadata into memory.
        /// </summary>
        /// <param name="szScope">[in] The name of the file to be opened. The file must contain common language runtime (CLR) metadata.</param>
        /// <param name="dwOpenFlags">[in] A value of the <c>CorOpenFlags</c> enumeration to specify the mode (read, write, and so on) for opening. </param>
        /// <param name="riid">
        /// [in] The IID of the desired metadata interface to be returned; the caller will use the interface to import (read) or emit (write) metadata. 
        /// The value of riid must specify one of the "import" or "emit" interfaces. Valid values are IID_IMetaDataEmit, IID_IMetaDataImport, IID_IMetaDataAssemblyEmit, IID_IMetaDataAssemblyImport, IID_IMetaDataEmit2, or IID_IMetaDataImport2. 
        /// </param>
        /// <param name="ppIUnk">[out] The pointer to the returned interface.</param>
        /// <remarks>
        /// STDMETHOD(OpenScope)(           // Return code.
        ///     LPCWSTR     szScope,        // [in] The scope to open.
        ///     DWORD       dwOpenFlags,        // [in] Open mode flags.
        ///     REFIID      riid,           // [in] The interface desired.
        ///     IUnknown    **ppIUnk) PURE;     // [out] Return interface on success.
        /// </remarks>
        [PreserveSig]
        void OpenScope(
            [In, MarshalAs(UnmanagedType.LPWStr)] string szScope,
            [In] CorOpenFlags dwOpenFlags,
            [In] [MarshalAs(UnmanagedType.LPStruct)] Guid riid,
            [Out, MarshalAs(UnmanagedType.Interface)] out object ppIUnk);

        /// <summary>
        /// Opens an area of memory that contains existing metadata. That is, this method opens a specified area of memory in which the existing data is treated as metadata.
        /// </summary>
        /// <param name="pData">[in] A pointer that specifies the starting address of the memory area.</param>
        /// <param name="cbData">[in] The size of the memory area, in bytes.</param>
        /// <param name="dwOpenFlags">[in] A value of the <c>CorOpenFlags</c> enumeration to specify the mode (read, write, and so on) for opening.</param>
        /// <param name="riid">
        /// [in] The IID of the desired metadata interface to be returned; the caller will use the interface to import (read) or emit (write) metadata. 
        /// The value of riid must specify one of the "import" or "emit" interfaces. Valid values are IID_IMetaDataEmit, IID_IMetaDataImport, IID_IMetaDataAssemblyEmit, IID_IMetaDataAssemblyImport, IID_IMetaDataEmit2, or IID_IMetaDataImport2. 
        /// </param>
        /// <param name="ppIUnk">[out] The pointer to the returned interface.</param>
        /// <remarks>
        /// STDMETHOD(OpenScopeOnMemory)(       // Return code.
        ///     LPCVOID     pData,          // [in] Location of scope data.
        ///     ULONG       cbData,         // [in] Size of the data pointed to by pData.
        ///     DWORD       dwOpenFlags,        // [in] Open mode flags.
        ///     REFIID      riid,           // [in] The interface desired.
        ///     IUnknown    **ppIUnk) PURE;     // [out] Return interface on success.
        /// </remarks>
        [PreserveSig]
        void OpenScopeOnMemory(
            [In] IntPtr pData,
            [In] uint cbData,
            [In] CorOpenFlags dwOpenFlags,
            [In] ref Guid riid,
            [Out, MarshalAs(UnmanagedType.IUnknown)] out object ppIUnk);
    }

    [Flags]
    internal enum CorOpenFlags : uint
    {
        ofRead = 0x00000000,     // Open scope for read
        ofWrite = 0x00000001,     // Open scope for write.
        ofReadWriteMask = 0x00000001,     // Mask for read/write bit.

        ofCopyMemory = 0x00000002,     // Open scope with memory. Ask metadata to maintain its own copy of memory.

        ofReadOnly = 0x00000010,     // Open scope for read. Will be unable to QI for a IMetadataEmit* interface
        ofTakeOwnership = 0x00000020,     // The memory was allocated with CoTaskMemAlloc and will be freed by the metadata

        // These are obsolete and are ignored.
        // ofCacheImage     =   0x00000004,     // EE maps but does not do relocations or verify image
        // ofManifestMetadata = 0x00000008,     // Open scope on ngen image, return the manifest metadata instead of the IL metadata
        ofNoTypeLib = 0x00000080,     // Don't OpenScope on a typelib.

        // Internal bits
        ofReserved1 = 0x00000100,     // Reserved for internal use.
        ofReserved2 = 0x00000200,     // Reserved for internal use.
        ofReserved3 = 0x00000400,     // Reserved for internal use.
        ofReserved = 0xffffff40      // All the reserved bits.
    }
}
