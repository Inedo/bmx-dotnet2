using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Inedo.BuildMasterExtensions.DotNet2.MetadataServices
{
    /// <summary>
    /// Provides methods to access and examine the contents of an assembly manifest.
    /// </summary>
    [ComImport]
    [Guid("EE62470B-E94B-424e-9B7C-2F00C9249F93")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IMetaDataAssemblyImport
    {
        /// <summary>
        /// Gets the set of properties for the assembly with the specified metadata signature.
        /// </summary>
        /// <param name="mda">[in]. The <c>mdAssembly</c> metadata token that represents the assembly for which to get the properties. </param>
        /// <param name="ppbPublicKey">[out] A pointer to the public key or the metadata token.</param>
        /// <param name="pcbPublicKey">[out] The number of bytes in the returned public key.</param>
        /// <param name="pulHashAlgId">[out] A pointer to the algorithm used to hash the files in the assembly.</param>
        /// <param name="szName">[out] The simple name of the assembly.</param>
        /// <param name="cchName">[in] The size, in wide chars, of <c>szName</c>.</param>
        /// <param name="pchName">[out] The number of wide chars actually returned in <c>szName</c>.</param>
        /// <param name="pMetaData">[out] A pointer to an <c>ASSEMBLYMETADATA</c> structure that contains the assembly metadata. </param>
        /// <param name="pdwAssemblyFlags">[out] Flags that describe the metadata applied to an assembly. This value is a combination of one or more <c>CorAssemblyFlags</c> values.</param>
        /// <remarks>
        /// STDMETHOD(GetAssemblyProps)(        // S_OK or error.
        ///     mdAssembly  mda,            // [IN] The Assembly for which to get the properties.
        ///     const void  **ppbPublicKey,     // [OUT] Pointer to the public key.
        ///     ULONG       *pcbPublicKey,      // [OUT] Count of bytes in the public key.
        ///     ULONG       *pulHashAlgId,      // [OUT] Hash Algorithm.
        ///     __out_ecount_part_opt(cchName, *pchName) LPWSTR  szName, // [OUT] Buffer to fill with assembly's simply name.
        ///     ULONG       cchName,        // [IN] Size of deployablesBuffer in wide chars.
        ///     ULONG       *pchName,           // [OUT] Actual # of wide chars in name.
        ///     ASSEMBLYMETADATA *pMetaData,    // [OUT] Assembly MetaData.
        ///     DWORD       *pdwAssemblyFlags) PURE;    // [OUT] Flags.
        /// </remarks>
        [PreserveSig]
        void GetAssemblyProps(
            [In] uint mda,
            [Out] out IntPtr ppbPublicKey,
            [Out] out uint pcbPublicKey,
            [Out] out uint pulHashAlgId,
            [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder szName,
            [In] uint cchName,
            [Out] out uint pchName,
            [Out] out ASSEMBLYMETADATA pMetaData,
            [Out] out CorAssemblyFlags pdwAssemblyFlags);

        /// <summary>
        /// Gets the set of properties for the assembly reference with the specified metadata signature.
        /// </summary>
        /// <param name="mdar">[in] The <c>mdAssemblyRef</c> metadata token that represents the assembly reference for which to get the properties. </param>
        /// <param name="ppbPublicKeyOrToken">[out] A pointer to the public key or the metadata token.</param>
        /// <param name="pcbPublicKeyOrToken">[out] The number of bytes in the returned public key or token.</param>
        /// <param name="szName">[out] The simple name of the assembly.</param>
        /// <param name="cchName">[in] The size, in wide chars, of <c>szName</c>.</param>
        /// <param name="pchName">[out] A pointer to the number of wide chars actually returned in <c>szName</c>.</param>
        /// <param name="pMetaData">[out] A pointer to an <c>ASSEMBLYMETADATA</c> structure that contains the assembly metadata.</param>
        /// <param name="ppbHashValue">[out] A pointer to the hash value. This is the hash, using the SHA-1 algorithm, of the <c>PublicKey</c> property of the assembly being referenced, unless the <c>arfFullOriginator</c> flag of the <c>AssemblyRefFlags</c> enumeration is set.</param>
        /// <param name="pcbHashValue">[out] The number of wide chars in the returned hash value.</param>
        /// <param name="pdwAssemblyFlags">[out] A pointer to flags that describe the metadata applied to an assembly. The flags value is a combination of one or more <c>CorAssemblyFlags</c> values.</param>
        /// <remarks>
        /// STDMETHOD(GetAssemblyRefProps)(     // S_OK or error.
        ///     mdAssemblyRef mdar,         // [IN] The AssemblyRef for which to get the properties.
        ///     const void  **ppbPublicKeyOrToken,  // [OUT] Pointer to the public key or token.
        ///     ULONG       *pcbPublicKeyOrToken,   // [OUT] Count of bytes in the public key or token.
        ///     __out_ecount_part_opt(cchName, *pchName)LPWSTR szName, // [OUT] Buffer to fill with name.
        ///     ULONG       cchName,        // [IN] Size of deployablesBuffer in wide chars.
        ///     ULONG       *pchName,           // [OUT] Actual # of wide chars in name.
        ///     ASSEMBLYMETADATA *pMetaData,    // [OUT] Assembly MetaData.
        ///     const void  **ppbHashValue,     // [OUT] Hash blob.
        ///     ULONG       *pcbHashValue,      // [OUT] Count of bytes in the hash blob.
        ///     DWORD       *pdwAssemblyRefFlags) PURE; // [OUT] Flags.
        /// </remarks>
        [PreserveSig]
        void GetAssemblyRefProps(
            [In] uint mdar,
            [Out] IntPtr ppbPublicKeyOrToken,
            [Out] out uint pcbPublicKeyOrToken,
            [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder szName,
            [In] uint cchName,
            [Out] out uint pchName,
            [Out] out ASSEMBLYMETADATA pMetaData,
            [Out] IntPtr ppbHashValue,
            [Out] out uint pcbHashValue,
            [Out] out CorAssemblyFlags pdwAssemblyFlags);

        /// <summary>
        /// Gets the properties of the file with the specified metadata signature.
        /// </summary>
        /// <param name="mdf">[in] The <c>mdFile</c> metadata token that represents the file for which to get the properties.</param>
        /// <param name="szName">[out] The simple name of the file.</param>
        /// <param name="cchName">[in] The size, in wide chars, of <c>szName</c>.</param>
        /// <param name="pchName">[out] A pointer to the number of wide chars actually returned in <c>szName</c>.</param>
        /// <param name="ppbHashValue">[out] A pointer to the hash value. This is the hash, using the SHA-1 algorithm, of the <c>PublicKey</c> property of the assembly being referenced, unless the <c>arfFullOriginator</c> flag of the <c>AssemblyRefFlags</c> enumeration is set.</param>
        /// <param name="pcbHashValue">[out] The number of wide chars in the returned hash value.</param>
        /// <param name="pdwFileFlags">[out] A pointer to the flags that describe the metadata applied to a file. The flags value is a combination of one or more <c>CorFileFlags</c> values.</param>
        /// <remarks>
        /// STDMETHOD(GetFileProps)(        // S_OK or error.
        ///     mdFile      mdf,            // [IN] The File for which to get the properties.
        ///     __out_ecount_part_opt(cchName, *pchName) LPWSTR      szName, // [OUT] Buffer to fill with name.
        ///     ULONG       cchName,        // [IN] Size of deployablesBuffer in wide chars.
        ///     ULONG       *pchName,           // [OUT] Actual # of wide chars in name.
        ///     const void  **ppbHashValue,     // [OUT] Pointer to the Hash Value Blob.
        ///     ULONG       *pcbHashValue,      // [OUT] Count of bytes in the Hash Value Blob.
        ///     DWORD       *pdwFileFlags) PURE;    // [OUT] Flags.
        /// </remarks>
        [PreserveSig]
        void GetFileProps(
            [In] uint mdf,
            [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder szName,
            [In] uint cchName,
            [Out] out uint pchName,
            [Out] IntPtr ppbHashValue,
            [Out] out uint pcbHashValue,
            [Out] out CorFileFlags pdwFileFlags);

        /// <summary>
        /// Gets the set of properties of the exported type with the specified metadata signature.
        /// </summary>
        /// <param name="mdct">[in] An <c>mdExportedType</c> metadata token that represents the exported type. </param>
        /// <param name="szName">[out] The simple name of the file.</param>
        /// <param name="cchName">[in] The size, in wide chars, of <c>szName</c>.</param>
        /// <param name="pchName">[out] A pointer to the number of wide chars actually returned in <c>szName</c>.</param>
        /// <param name="ptkImplementation">[out] An <c>mdFile</c>, <c>mdAssemblyRef</c>, or <c>mdExportedType</c> metadata token that contains or allows access to the properties of the exported type.</param>
        /// <param name="ptkTypeDef">[out] A pointer to an <c>mdTypeDe</c>f token that represents a type in the file.</param>
        /// <param name="pdwExportedTypeFlags">[out] A pointer to the flags that describe the metadata applied to the exported type. The flags value can be one or more <c>CorTypeAttr</c> values.</param>
        /// <remarks>
        /// STDMETHOD(GetExportedTypeProps)(    // S_OK or error.
        ///     mdExportedType   mdct,          // [IN] The ExportedType for which to get the properties.
        ///     __out_ecount_part_opt(cchName, *pchName) LPWSTR      szName, // [OUT] Buffer to fill with name.
        ///     ULONG       cchName,        // [IN] Size of deployablesBuffer in wide chars.
        ///     ULONG       *pchName,           // [OUT] Actual # of wide chars in name.
        ///     mdToken     *ptkImplementation,     // [OUT] mdFile or mdAssemblyRef or mdExportedType.
        ///     mdTypeDef   *ptkTypeDef,        // [OUT] TypeDef token within the file.
        ///     DWORD       *pdwExportedTypeFlags) PURE; // [OUT] Flags.
        /// </remarks>
        [PreserveSig]
        void GetExportedTypeProps(
            [In] uint mdct,
            [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder szName,
            [In] uint cchName,
            [Out] out uint pchName,
            [Out] out uint ptkImplementation,
            [Out] out uint ptkTypeDef,
            [Out] out CorTypeAttr pdwExportedTypeFlags);

        /// <summary>
        /// Gets the set of properties of the manifest resource with the specified metadata signature.
        /// </summary>
        /// <param name="mdmr">[in] An <c>mdManifestResource</c> token that represents the resource for which to get the properties. </param>
        /// <param name="szName">[out] The simple name of the file.</param>
        /// <param name="cchName">[in] The size, in wide chars, of <c>szName</c>.</param>
        /// <param name="pchName">[out] A pointer to the number of wide chars actually returned in <c>szName</c>.</param>
        /// <param name="ptkImplementation">[out] A pointer to an <c>mdFile</c> token or an mdAssemblyRef token that represents the file or assembly, respectively, that contains the resource. </param>
        /// <param name="pdwOffset">[out] A pointer to a value that specifies the offset to the beginning of the resource within the file.</param>
        /// <param name="pdwResourceFlags">[out] A pointer to flags that describe the metadata applied to a resource. The flags value is a combination of one or more <c>CorManifestResourceFlags</c> values.</param>
        /// <remarks>
        /// STDMETHOD(GetManifestResourceProps)(    // S_OK or error.
        ///     mdManifestResource  mdmr,       // [IN] The ManifestResource for which to get the properties.
        ///     __out_ecount_part_opt(cchName, *pchName)LPWSTR      szName,  // [OUT] Buffer to fill with name.
        ///     ULONG       cchName,        // [IN] Size of deployablesBuffer in wide chars.
        ///     ULONG       *pchName,           // [OUT] Actual # of wide chars in name.
        ///     mdToken     *ptkImplementation,     // [OUT] mdFile or mdAssemblyRef that provides the ManifestResource.
        ///     DWORD       *pdwOffset,         // [OUT] Offset to the beginning of the resource within the file.
        ///     DWORD       *pdwResourceFlags) PURE;// [OUT] Flags.
        /// </remarks>
        [PreserveSig]
        void GetManifestResourceProps(
            [In] uint mdmr,
            [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder szName,
            [In] uint cchName,
            [Out] out uint pchName,
            [Out] out uint ptkImplementation,
            [Out] out uint pdwOffset,
            [Out] out CorManifestResourceFlags pdwResourceFlags);

        /// <summary>
        /// Enumerates the <c>mdAssemblyRef</c> instances that are defined in the assembly manifest. 
        /// </summary>
        /// <param name="phEnum">[in, out] A pointer to the enumerator. This must be a null value when the EnumAssemblyRefs method is called for the first time.</param>
        /// <param name="rAssemblyRefs">[out] The enumeration of mdAssemblyRef metadata tokens.</param>
        /// <param name="cMax">[in] The maximum number of tokens that can be placed in the rAssemblyRefs array.</param>
        /// <param name="pcTokens">[out] The number of tokens actually placed in rAssemblyRefs.</param>
        /// <remarks>
        /// STDMETHOD(EnumAssemblyRefs)(        // S_OK or error
        ///     HCORENUM    *phEnum,        // [IN|OUT] Pointer to the enum.
        ///     mdAssemblyRef rAssemblyRefs[],      // [OUT] Put AssemblyRefs here.
        ///     ULONG       cMax,           // [IN] Max AssemblyRefs to put.
        ///     ULONG       *pcTokens) PURE;    // [OUT] Put # put here.
        /// </remarks>
        [PreserveSig]
        void EnumAssemblyRefs(
            [In] ref IntPtr phEnum,
            [Out, MarshalAs(UnmanagedType.LPArray)] uint[] rAssemblyRefs,
            [In] uint cMax,
            [Out] out uint pcTokens);

        /// <summary>
        /// Enumerates the files referenced in the current assembly manifest.
        /// </summary>
        /// <param name="phEnum">[in, out] A pointer to the enumerator. This must be a null value for the first call of this method.</param>
        /// <param name="rFiles">[out] The array used to store the <c>mdFile</c> metadata tokens.</param>
        /// <param name="cMax">[in] The maximum number of mdFile tokens that can be placed in <c>rFiles</c>.</param>
        /// <param name="pcTokens">[out] The number of <c>mdFile</c> tokens actually placed in <c>rFiles</c>.</param>
        /// <remarks>
        /// STDMETHOD(EnumFiles)(           // S_OK or error
        ///     HCORENUM    *phEnum,        // [IN|OUT] Pointer to the enum.
        ///     mdFile      rFiles[],           // [OUT] Put Files here.
        ///     ULONG       cMax,           // [IN] Max Files to put.
        ///     ULONG       *pcTokens) PURE;    // [OUT] Put # put here.
        /// </remarks>
        [PreserveSig]
        void EnumFiles(
            [In] ref IntPtr phEnum,
            [Out, MarshalAs(UnmanagedType.LPArray)] uint[] rFiles,
            [In] uint cMax,
            [Out] out uint pcTokens);

        /// <summary>
        /// Enumerates the exported types referenced in the assembly manifest in the current metadata scope.
        /// </summary>
        /// <param name="phEnum">[in, out] A pointer to the enumerator. This must be a null value when the <c>EnumExportedTypes</c> method is called for the first time.</param>
        /// <param name="rExportedTypes">[out] The enumeration of mdExportedType metadata tokens.</param>
        /// <param name="cMax">[in] The maximum number of mdExportedType tokens that can be placed in the <c>rExportedTypes</c> array.</param>
        /// <param name="pcTokens">[out] The number of <c>mdExportedType</c> tokens actually placed in <c>rExportedTypes</c>.</param>
        /// <remarks>
        /// STDMETHOD(EnumExportedTypes)(       // S_OK or error
        ///     HCORENUM    *phEnum,        // [IN|OUT] Pointer to the enum.
        ///     mdExportedType   rExportedTypes[],  // [OUT] Put ExportedTypes here.
        ///     ULONG       cMax,           // [IN] Max ExportedTypes to put.
        ///     ULONG       *pcTokens) PURE;    // [OUT] Put # put here.
        /// </remarks>
        [PreserveSig]
        void EnumExportedTypes(
            [In] ref IntPtr phEnum,
            [Out, MarshalAs(UnmanagedType.LPArray)] uint[] rExportedTypes,
            [In] uint cMax,
            [Out] out uint pcTokens);

        /// <summary>
        /// Gets a pointer to an enumerator for the resources referenced in the current assembly manifest.
        /// </summary>
        /// <param name="phEnum">[in, out] A pointer to the enumerator. This must be a null value when the <c>EnumManifestResources</c> method is called for the first time.</param>
        /// <param name="rManifestResources">[out] The array used to store the <c>mdManifestResource</c> metadata tokens.</param>
        /// <param name="cMax">[in] The maximum number of <c>mdManifestResource</c> tokens that can be placed in <c>rManifestResources</c>.</param>
        /// <param name="pcTokens">[out] The number of <c>mdManifestResource</c> tokens actually placed in <c>rManifestResources</c>.</param>
        /// <remarks>
        /// STDMETHOD(EnumManifestResources)(       // S_OK or error
        ///     HCORENUM    *phEnum,        // [IN|OUT] Pointer to the enum.
        ///     mdManifestResource  rManifestResources[],   // [OUT] Put ManifestResources here.
        ///     ULONG       cMax,           // [IN] Max Resources to put.
        ///     ULONG       *pcTokens) PURE;    // [OUT] Put # put here.
        /// </remarks>
        [PreserveSig]
        void EnumManifestResources(
            [In] ref IntPtr phEnum,
            [Out, MarshalAs(UnmanagedType.LPArray)] uint[] rManifestResources,
            [In] uint cMax,
            [Out] out uint pcTokens);

        /// <summary>
        /// Gets a pointer to the assembly in the current scope.
        /// </summary>
        /// <param name="ptkAssembly">[out] A pointer to the retrieved <c>mdAssembly</c> token that identifies the assembly.</param>
        /// <remarks>
        /// STDMETHOD(GetAssemblyFromScope)(    // S_OK or error
        ///     mdAssembly  *ptkAssembly) PURE;     // [OUT] Put token here.
        /// </remarks>
        [PreserveSig]
        void GetAssemblyFromScope(
            [Out] out uint ptkAssembly);

        /// <summary>
        /// Gets a pointer to an exported type, given its name and enclosing type.
        /// </summary>
        /// <param name="szName">[in] The name of the exported type.</param>
        /// <param name="mdtExportedType">[in] The metadata token for the enclosing class of the exported type. This value is <c>mdExportedTypeNil</c> if the requested exported type is not a nested type.</param>
        /// <param name="mdExportedType">[out] A pointer to the <c>mdExportedType</c> token that represents the exported type.</param>
        /// <remarks>
        /// STDMETHOD(FindExportedTypeByName)(      // S_OK or error
        ///     LPCWSTR     szName,         // [IN] Name of the ExportedType.
        ///     mdToken     mdtExportedType,    // [IN] ExportedType for the enclosing class.
        ///     mdExportedType   *ptkExportedType) PURE; // [OUT] Put the ExportedType token here.
        /// </remarks>
        [PreserveSig]
        void FindExportedTypeByName(
            [In, MarshalAs(UnmanagedType.LPWStr)] string szName,
            [In] uint mdtExportedType,
            [Out] out uint mdExportedType);

        /// <summary>
        /// Gets a pointer to the manifest resource with the specified name.
        /// </summary>
        /// <param name="szName">[in] The name of the resource.</param>
        /// <param name="ptkManifestResource">[out] The array used to store the <c>mdManifestResource</c> metadata tokens, each of which represents a manifest resource.</param>
        /// <remarks>
        /// STDMETHOD(FindManifestResourceByName)(  // S_OK or error
        ///     LPCWSTR     szName,         // [IN] Name of the ManifestResource.
        ///     mdManifestResource *ptkManifestResource) PURE;  // [OUT] Put the ManifestResource token here.
        /// </remarks>
        [PreserveSig]
        void FindManifestResourceByName(
            [In, MarshalAs(UnmanagedType.LPWStr)] string szName,
            [Out] out uint ptkManifestResource);

        /// <summary>
        /// Releases a reference to the specified enumeration instance.
        /// </summary>
        /// <param name="hEnum">Handle of enumeration to be closed.</param>
        /// <remarks>
        /// STDMETHOD_(void, CloseEnum)(
        ///     HCORENUM hEnum) PURE;           // Enum to be closed.
        /// </remarks>
        [PreserveSig]
        void CloseEnum(
            [In] IntPtr hEnum);

        //STDMETHOD(FindAssembliesByName)(    // S_OK or error
        //    LPCWSTR  szAppBase,         // [IN] optional - can be NULL
        //    LPCWSTR  szPrivateBin,          // [IN] optional - can be NULL
        //    LPCWSTR  szAssemblyName,        // [IN] required - this is the assembly you are requesting
        //    IUnknown *ppIUnk[],         // [OUT] put IMetaDataAssemblyImport pointers here
        //    ULONG    cMax,              // [IN] The max number to put
        //    ULONG    *pcAssemblies) PURE;       // [OUT] The number of assemblies returned.
        /// <summary>
        /// Gets an array of assemblies with the specified <c>szAssemblyName</c> parameter, using the standard rules employed by the common language runtime (CLR) for resolving references.
        /// </summary>
        /// <param name="szAppBase">[in] The root directory in which to search for the given assembly. If this value is set to null, <c>FindAssembliesByName</c> will look only in the global assembly cache for the assembly.</param>
        /// <param name="szPrivateBin">[in] A list of semicolon-delimited subdirectories (for example, "bin;bin2"), under the root directory, in which to search for the assembly. These directories are probed in addition to those specified in the default probing rules.</param>
        /// <param name="szAssemblyName">[in] The name of the assembly to find. The format of this string is defined in the class reference page for <c>AssemblyName</c>.</param>
        /// <param name="ppIUnk">[in] An array of type <c>IUnknown</c> in which to put the <c>IIMetadataAssemblyImport</c> interface pointers. </param>
        /// <param name="cMax">[out] The maximum number of interface pointers that can be placed in <c>IppIUnk</c>.</param>
        /// <param name="pcAssemblies">[out] The number of interface pointers returned. That is, the number of interface pointers actually placed in <c>ppIUnk</c>.</param>
        [PreserveSig]
        void FindAssembliesByName(
            [In, MarshalAs(UnmanagedType.LPWStr)] string szAppBase,
            [In, MarshalAs(UnmanagedType.LPWStr)] string szPrivateBin,
            [In, MarshalAs(UnmanagedType.LPWStr)] string szAssemblyName,
            [Out, MarshalAs(UnmanagedType.Interface)] out object[] ppIUnk,
            [In] uint cMax,
            [Out] out uint pcAssemblies);
    }


    [Flags]
    internal enum CorAssemblyFlags : uint
    {
        afPublicKey = 0x0001,     // The assembly ref holds the full (unhashed) public key.

        afPA_None = 0x0000,     // Processor Architecture unspecified
        afPA_MSIL = 0x0010,     // Processor Architecture: neutral (PE32)
        afPA_x86 = 0x0020,     // Processor Architecture: x86 (PE32)
        afPA_IA64 = 0x0030,     // Processor Architecture: Itanium (PE32+)
        afPA_AMD64 = 0x0040,     // Processor Architecture: AMD X64 (PE32+)
        afPA_NoPlatform = 0x0070,      // applies to any platform but cannot run on any (e.g. reference assembly), should not have "specified" set
        afPA_Specified = 0x0080,     // Propagate PA flags to AssemblyRef record
        afPA_Mask = 0x0070,     // Bits describing the processor architecture
        afPA_FullMask = 0x00F0,     // Bits describing the PA incl. Specified
        afPA_Shift = 0x0004,     // NOT A FLAG, shift count in PA flags <--> index conversion

        afEnableJITcompileTracking = 0x8000, // From "DebuggableAttribute".
        afDisableJITcompileOptimizer = 0x4000, // From "DebuggableAttribute".

        afRetargetable = 0x0100,     // The assembly can be retargeted (at runtime) to an
        //  assembly from a different publisher.    
    }

    [Flags]
    internal enum CorFileFlags : uint
    {
        ffContainsMetaData = 0x0000,     // This is not a resource file
        ffContainsNoMetaData = 0x0001,     // This is a resource file or other non-metadata-containing file
    }

    [Flags]
    internal enum CorTypeAttr : uint
    {
        // Use this mask to retrieve the type visibility information.
        tdVisibilityMask = 0x00000007,
        tdNotPublic = 0x00000000,     // Class is not public scope.
        tdPublic = 0x00000001,     // Class is public scope.
        tdNestedPublic = 0x00000002,     // Class is nested with public visibility.
        tdNestedPrivate = 0x00000003,     // Class is nested with private visibility.
        tdNestedFamily = 0x00000004,     // Class is nested with family visibility.
        tdNestedAssembly = 0x00000005,     // Class is nested with assembly visibility.
        tdNestedFamANDAssem = 0x00000006,     // Class is nested with family and assembly visibility.
        tdNestedFamORAssem = 0x00000007,     // Class is nested with family or assembly visibility.

        // Use this mask to retrieve class layout information
        tdLayoutMask = 0x00000018,
        tdAutoLayout = 0x00000000,     // Class fields are auto-laid out
        tdSequentialLayout = 0x00000008,     // Class fields are laid out sequentially
        tdExplicitLayout = 0x00000010,     // Layout is supplied explicitly
        // end layout mask

        // Use this mask to retrieve class semantics information.
        tdClassSemanticsMask = 0x00000020,
        tdClass = 0x00000000,     // Type is a class.
        tdInterface = 0x00000020,     // Type is an interface.
        // end semantics mask

        // Special semantics in addition to class semantics.
        tdAbstract = 0x00000080,     // Class is abstract
        tdSealed = 0x00000100,     // Class is concrete and may not be extended
        tdSpecialName = 0x00000400,     // Class name is special.  Name describes how.

        // Implementation attributes.
        tdImport = 0x00001000,     // Class / interface is imported
        tdSerializable = 0x00002000,     // The class is Serializable.

        // Use tdStringFormatMask to retrieve string information for native interop
        tdStringFormatMask = 0x00030000,
        tdAnsiClass = 0x00000000,     // LPTSTR is interpreted as ANSI in this class
        tdUnicodeClass = 0x00010000,     // LPTSTR is interpreted as UNICODE
        tdAutoClass = 0x00020000,     // LPTSTR is interpreted automatically
        tdCustomFormatClass = 0x00030000,     // A non-standard encoding specified by CustomFormatMask
        tdCustomFormatMask = 0x00C00000,     // Use this mask to retrieve non-standard encoding information for native interop. The meaning of the values of these 2 bits is unspecified.

        // end string format mask

        tdBeforeFieldInit = 0x00100000,     // Initialize the class any time before first static field access.
        tdForwarder = 0x00200000,     // This ExportedType is a type forwarder.

        // Flags reserved for runtime use.
        tdReservedMask = 0x00040800,
        tdRTSpecialName = 0x00000800,     // Runtime should check name encoding.
        tdHasSecurity = 0x00040000,     // Class has security associate with it.
    }

    [Flags]
    internal enum CorManifestResourceFlags : uint
    {
        mrVisibilityMask = 0x0007,
        mrPublic = 0x0001,     // The Resource is exported from the Assembly.
        mrPrivate = 0x0002,     // The Resource is private to the Assembly.
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct OSINFO
    {
        public uint dwOSPlatformId,     // Operating system platform.
                dwOSMajorVersion,       // OS Major version.
                dwOSMinorVersion;       // OS Minor version.
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct ASSEMBLYMETADATA
    {
        public ushort usMajorVersion,    // Major Version.
                  usMinorVersion,    // Minor Version.
                  usBuildNumber,     // Build Number.
                  usRevisionNumber;      // Revision Number.
        /// <summary>
        /// Actually this is a LPCWSTR Win API type reference, so it could be converted to <c>string</c> if required.
        /// </summary>
        public IntPtr szLocale;          // Locale.
        public uint cbLocale;        // [IN/OUT] Size of the deployablesBuffer in wide chars/Actual size.
        public IntPtr rProcessor;        // Processor ID array.
        public uint ulProcessor;         // [IN/OUT] Size of the Processor ID array/Actual # of entries filled in.
        public IntPtr rOS;           // OSINFO array.
        public uint ulOS;            // [IN/OUT]Size of the OSINFO array/Actual # of entries filled in.
    }
}
