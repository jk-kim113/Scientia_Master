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
        Ingame
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

    public void StartLoadIngameScene()
    {
        _prevScene = _currentScene;
        _currentScene = eTypeScene.Ingame;
        StartCoroutine(LoadingPrecess("IngameScene"));
        //SoundManager._instance.PlayBGMSound(SoundManager.eTypeBGM.INGAME);
    }
}
