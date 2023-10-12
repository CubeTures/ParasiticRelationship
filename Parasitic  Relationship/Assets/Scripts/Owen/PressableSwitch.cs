using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressableSwitch : MonoBehaviour
{
    [SerializeField] UnityEvent onSwitchPressSingleAction;
    [SerializeField] UnityEvent<bool> onSwitchPress;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] bool pressableByPlayer;
    [SerializeField] bool heldSwitch;
    bool pressedOnce = false;

    List<Collision2D> currentlyColliding;

    private void Start()
    {
        currentlyColliding = new List<Collision2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentlyColliding.Add(collision);
        if (ValidCollision(collision))
        {
            SwitchPressed(collision);
        }

        TryDisableSwitch();
    }
    void SwitchPressed(Collision2D collision)
    {
        if(!pressedOnce) { onSwitchPressSingleAction.Invoke(); }
        pressedOnce = true;

        //animation
        onSwitchPress.Invoke(true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        currentlyColliding.Remove(collision);
        if(currentlyColliding.Count == 0) 
        {
            SwitchReleased(collision);
        }
    }
    void SwitchReleased(Collision2D collision)
    {
        //animation
        onSwitchPress.Invoke(false);
    }

    bool ValidCollision(Collision2D collision)
    {
        if(collision.gameObject.layer == groundLayer)
        {
            return false;
        }
        else if (PressedByPlayer(collision) && !pressableByPlayer)
        {
            return false;
        }

        return true;
    }
    bool PressedByPlayer(Collision2D collision)
    {
        return collision.gameObject.CompareTag("Player");
    }

    void TryDisableSwitch()
    {
        if (!heldSwitch)
        {
            //wait for animation to finish
            gameObject.SetActive(false);
        }
    }
}


