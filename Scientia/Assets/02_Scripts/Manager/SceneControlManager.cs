using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControlManager : TSingleton<SceneControlManager>
{
    public enum eTypeScene
    {
        Start = 0,
        LogIn,
        Lobby,
        Battle
    }

    public enum eStateLoadding
    {
        none = 0,
        UnLoad,
        Load,
        Loading,
        LoadingDone,
        EndLoad
    }

    eTypeScene _currentScene;
    public eTypeScene _nowScene { get { return _currentScene; } }
    eTypeScene _prevScene;
    eStateLoadding _loadState;

    protected override void Init()
    {
        base.Init();
    }

    private void Update()
    {
        if (_loadState == eStateLoadding.EndLoad)
        {
            StopCoroutine("LoadingPrecess");
            _loadState = eStateLoadding.none;
        }
    }

    IEnumerator LoadingPrecess(string sceneName)
    {
        _loadState = eStateLoadding.Load;
        LoadingWindow wnd = UIManager._instance.OpenWnd<LoadingWindow>(UIManager.eKindWindow.LoadingUI);
        wnd.OpenLoadingWnd();

        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneName);
        _loadState = eStateLoadding.Loading;
        while (!asyncOp.isDone)
        {
            wnd.SettingLoadRate(asyncOp.progress);
            yield return null;
        }

        wnd.SettingLoadRate(1);
        yield return new WaitForSeconds(1.8f);

        switch(_currentScene)
        {
            case eTypeScene.Lobby:

                while(LobbyManager._instance._nowLoadType != LobbyManager.eLoadType.LoadEnd)
                {
                    yield return null;
                }

                break;
        }

        _loadState = eStateLoadding.LoadingDone;
        UIManager._instance.Close(UIManager.eKindWindow.LoadingUI);
        yield return new WaitForSeconds(1f);
        _loadState = eStateLoadding.EndLoad;
    }

    public void StartLoadLogInScene()
    {
        _prevScene = _currentScene;
        _currentScene = eTypeScene.LogIn;
        StartCoroutine(LoadingPrecess("LogInScene"));
        //SoundManager._instance.PlayBGMSound(SoundManager.eTypeBGM.HOME);
    }

    public void StartLoadLobbyScene()
    {
        _prevScene = _currentScene;
        _currentScene = eTypeScene.Lobby;
        StartCoroutine(LoadingPrecess("LobbyScene"));
        //SoundManager._instance.PlayBGMSound(SoundManager.eTypeBGM.HOME);
    }

    public void StartLoadBattleScene()
    {
        _prevScene = _currentScene;
        _currentScene = eTypeScene.Battle;
        StartCoroutine(LoadingPrecess("BattleScene"));
        //SoundManager._instance.PlayBGMSound(SoundManager.eTypeBGM.INGAME);
    }
}
