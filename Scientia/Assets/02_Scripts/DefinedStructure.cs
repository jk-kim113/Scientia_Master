﻿using System.Collections;
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

    //[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)] // 한글

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
    public struct P_GetMyInfoData
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string _nickName;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct P_ReleaseCard
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string _nickName;
        [MarshalAs(UnmanagedType.I4)]
        public int _cardIndex;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct P_CreateRoom
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string _nickNaame;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string _name;
        [MarshalAs(UnmanagedType.I4)]
        public int _isLock;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string _pw;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string _mode;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string _rule;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct P_TryEnterRoom
    {
        [MarshalAs(UnmanagedType.I4)]
        public int _roomNumber;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string _nickName;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct P_InformRoomInfo
    {
        [MarshalAs(UnmanagedType.I4)]
        public int _roomNumber;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct P_PickCard
    {
        [MarshalAs(UnmanagedType.I4)]
        public int _roomNumber;
        [MarshalAs(UnmanagedType.I4)]
        public int _cardIndex;
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
    public struct P_MyInfoData
    {
        [MarshalAs(UnmanagedType.I4)]
        public int _characIndex;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public int[] _levelArr;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public int[] _expArr;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
        public int[] _cardReleaseArr;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
        public int[] _cardRentalArr;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
        public float[] _rentalTimeArr;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public int[] _myDeckArr;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct P_RoomInfo
    {
        [MarshalAs(UnmanagedType.I4)]
        public int _roomNumber;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string _name;
        [MarshalAs(UnmanagedType.I4)]
        public int _isLock;
        [MarshalAs(UnmanagedType.I4)]
        public int _currentMemberCnt;
        [MarshalAs(UnmanagedType.I4)]
        public int _maxMemberCnt;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string _mode;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string _rule;
        [MarshalAs(UnmanagedType.I4)]
        public int _isPlay;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct P_UserInfo
    {
        [MarshalAs(UnmanagedType.I4)]
        public int _roomNumber;
        [MarshalAs(UnmanagedType.I4)]
        public int _index;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string _nickName;
        [MarshalAs(UnmanagedType.I4)]
        public int _accountLevel;
        [MarshalAs(UnmanagedType.I4)]
        public int _isReady;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct P_MasterInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string _masterName;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct P_ShowReady
    {
        [MarshalAs(UnmanagedType.I4)]
        public int _index;
        [MarshalAs(UnmanagedType.I4)]
        public int _isReady;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct P_PickedCard
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public int[] _pickedCardArr;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct P_ThisTurn
    {
        [MarshalAs(UnmanagedType.I4)]
        public int _index;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct P_ShowPickCard
    {
        [MarshalAs(UnmanagedType.I4)]
        public int _index;
        [MarshalAs(UnmanagedType.I4)]
        public int _cardIndex;
        [MarshalAs(UnmanagedType.I4)]
        public int _slotIndex;
    }
    #endregion
}
