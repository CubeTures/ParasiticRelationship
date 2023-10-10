using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SwitchActiveCamera : MonoBehaviour
{
    public static SwitchActiveCamera Instance { get; private set; }
    CinemachineVirtualCamera activeCamera;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void SetNewActiveCamera(CinemachineVirtualCamera newActiveCamera)
    {
        if(newActiveCamera != activeCamera)
        {
            if(activeCamera != null)
            {
                activeCamera.enabled = false;
            }
            
            newActiveCamera.enabled = true;
            activeCamera = newActiveCamera;

            SetActiveScene(newActiveCamera.gameObject);
        }
    }
    void SetActiveScene(GameObject obj)
    {
        SceneLoader.SetScene(obj);
    }
}
