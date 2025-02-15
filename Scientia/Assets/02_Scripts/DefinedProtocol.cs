﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefinedProtocol
{
    public enum eFromClient
    {
        LogInTry,
        OverlapCheck_ID,
        OverlapCheck_NickName,
        EnrollTry,
        GetMyCharacterInfo,
        CreateCharacter,
        GetMyInfoData,
        AddReleaseCard,
        CreateRoom,
        GetRoomList,
        TryEnterRoom,
        InformReady,
        InformGameStart,
        FinishReadCard,
        PickCard,
        SelectAction,
        PickCardInProgress,
        RotateInfo,
        FinishRotate,
        ChooseCompleteCard,
        SelectFieldResult,
        RequestShopInfo,
        BuyItem,
        RequestFriendList,
        QuickEnter,
        SelectCardResult,
        FinishGameOver,

        ConnectionTerminate,

        max
    }

    public enum eToClient
    {
        LogInResult,
        ResultOverlap_ID,
        ResultOverlap_NickName,
        EnrollResult,
        CharacterInfo,
        EndCharacterInfo,
        EndCreateCharacter,
        ShowMyInfo,
        CompleteAddReleaseCard,
        EnterRoom,
        ShowRoomList,
        FinishShowRoom,
        ShowReady,
        ShowMaster,
        GameStart,
        ShowPickedCard,
        PickCard,
        ShowProjectBoard,
        ShowPickCard,
        DeletePickCard,
        ChooseAction,
        GetCard,
        RotateCard,
        ShowRotateInfo,
        SelectCompleteCard,
        ShowTotalFlask,
        ShowUserFlask,
        ShowTotalSkill,
        ShowUserSkill,
        ShowUserSlot,
        SelectField,
        ShowUserShopInfo,
        EndUserShopInfo,
        ShowCoinInfo,
        ShowFriendList,
        SelectCard,
        SelectMyCard,
        SelectOtherCard,

        GameOver,
        ShowGameResult,

        SystemMessage,
        ConfirmTerminate,

        max
    }
}
