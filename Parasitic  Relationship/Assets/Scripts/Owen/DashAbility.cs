using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAbility : MonoBehaviour
{
    PlayerController player;
    [SerializeField] LayerMask groundLayer;

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        player = playerObj.GetComponent<PlayerController>();
    }

    private void Update()
    {
        DetectInput();
    }

    void DetectInput()
    {
        if (IsPressingShift() && player.IsGrounded())
        {
            Dash();
        }
    }
    void Dash()
    {
        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, player.GetFacingDirection(), 100, groundLayer);
        if(hit)
        {
            float bodyOffset = player.GetFacingDirection() == Vector2.left ? player.transform.localScale.x / 2 : -player.transform.localScale.x / 2;
            player.transform.position = new Vector2(hit.point.x + bodyOffset, player.transform.position.y);
        }
    }

    bool IsPressingShift()
    {
        return Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift);
    }
}
