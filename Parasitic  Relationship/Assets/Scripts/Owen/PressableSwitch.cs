using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressableSwitch : MonoBehaviour
{
    [SerializeField] GameObject[] toToggle;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach(GameObject obj in toToggle)
        {
            obj.SetActive(false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        foreach (GameObject obj in toToggle)
        {
            obj.SetActive(true);
        }
    }
}
