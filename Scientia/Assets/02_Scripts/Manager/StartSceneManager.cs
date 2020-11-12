using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneManager : MonoBehaviour
{
    void Start()
    {
        ClientManager._instance.ConnectServer();
        SceneControlManager._instance.StartLoadLogInScene();
        ResourcePoolManager._instance.ResourceAllLoad();
    }
}
