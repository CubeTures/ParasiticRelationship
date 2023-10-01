using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SwitchActiveCameraTrigger : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera vCamera;

    private void Start()
    {
        vCamera.Follow = GameObject.FindGameObjectWithTag("Player").transform;
        vCamera.LookAt = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SwitchActiveCamera.Instance.SetNewActiveCamera(vCamera);
    }
}
