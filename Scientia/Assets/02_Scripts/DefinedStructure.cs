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
    public struct P_Send_ID_Pw
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string _id;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string _pw;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct P_OverlapCheck
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string _target;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct P_Request
    {
        
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct P_CreateCharacter
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string _nickName;
        [MarshalAs(UnmanagedType.I4)]
        public int _characterIndex;
        [MarshalAs(UnmanagedType.I4)]
        public int _slot;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct P_MyCardReleaseInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string _nickName;
    }
    #endregion

    #region ToClient
    [StructLayout(LayoutKind.Sequential)]
    public struct P_ResultLogIn
    {
        [MarshalAs(UnmanagedType.I4)]
        public int _isSuccess;
        [MarshalAs(UnmanagedType.I8)]
        public long _UUID;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct P_ResultCheck
    {
        [MarshalAs(UnmanagedType.I4)]
        public int _result;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct P_CharacterInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string _nickName;
        [MarshalAs(UnmanagedType.I4)]
        public int _chracIndex;
        [MarshalAs(UnmanagedType.I4)]
        public int _accountLevel;
        [MarshalAs(UnmanagedType.I4)]
        public int _slotIndex;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct P_ShowCardReleaseInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
        public int[] _cardIndexList;
    }
    #endregion
}
