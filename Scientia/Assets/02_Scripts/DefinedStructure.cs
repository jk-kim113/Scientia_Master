using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class DefinedStructure
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PacketInfo                                        // 전체 사이즈 1032byte
    {
        [MarshalAs(UnmanagedType.I4)]
        public int _id;
        [MarshalAs(UnmanagedType.I4)]
        public int _totalSize;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
        public byte[] _data;
    }

    #region FromClient
    [StructLayout(LayoutKind.Sequential)]
    public struct P_LogInTry
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string _id;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string _pw;
    }
    #endregion
}
