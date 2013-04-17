using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Inedo.BuildMasterExtensions.DotNet2
{
    internal static class NativeMethods
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern uint GetShortPathName(string lpszLongPath, StringBuilder lpszShortPath, uint cchBuffer);
    }
}
