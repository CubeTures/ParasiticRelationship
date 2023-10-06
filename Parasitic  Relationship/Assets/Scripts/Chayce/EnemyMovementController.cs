using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    public float detectionRange = 0.0F;
    public float movementSpeed = 0.0F;
    public bool isMovingRight = true;
    public bool isTurning = false;
    public float seconds = 0.0f;
    public Vector2 entityPos;

    // Update is called once per frame
    void Update()
    {
        if (isTurning)
        {
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(entityPos, isMovingRight ? Vector2.right : Vector2.left, detectionRange);
        if(hit.collider != null)
        {
            FlipDirection();
        }
        Vector2 movement = isMovingRight ? Vector2.right : Vector2.left;
        transform.Translate(movement * movementSpeed * Time.deltaTime); 
    }

    private void FlipDirection()
    {
        Debug.Log("Flipping enemy direction");

        isMovingRight = !isMovingRight;
        StartCoroutine(ResetTurningFlag());
    }

    private IEnumerator ResetTurningFlag()
    {
        yield return new WaitForSeconds(seconds);

        isTurning = false;
    }
}
