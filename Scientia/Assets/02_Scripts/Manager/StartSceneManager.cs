using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneManager : MonoBehaviour
{
    void Start()
    {
        ClientManager._instance.ConnectServer();
        TableManager._instance.LoadAll();
        ResourcePoolManager._instance.ResourceAllLoad();
        SceneControlManager._instance.StartLoadLogInScene();
    }
}
