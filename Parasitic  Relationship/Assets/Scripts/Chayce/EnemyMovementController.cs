using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    [SerializeField] float detectionRange = 0.0F;
    [SerializeField] float movementSpeed = 0.0F;
    [SerializeField] bool isFacingLeft = true;
    bool isTurning = false;
    [SerializeField] float turnDelay = 1.0f;
    [SerializeField] LayerMask groundLayer;

    private void Start()
    {
        SetDetectionRange();
    }
    void SetDetectionRange()
    {
        if (detectionRange == 0)
        {
            detectionRange = transform.localScale.x * 3 / 4;
        }
    }

    void FixedUpdate()
    {
        if (isTurning) { return; }
        CheckForTurn();
        Move();
    }
    void CheckForTurn()
    {

        Vector2 vector = isFacingLeft ? Vector2.left : Vector2.right;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, vector, detectionRange, groundLayer);
        if (hit)
        {
            FlipDirection();
        }
    }
    void Move()
    {
        transform.Translate(movementSpeed * Time.deltaTime * Vector2.left);
    }

    private void FlipDirection()
    {
        isFacingLeft = !isFacingLeft;
        StartCoroutine(ResetTurningFlag());
    }

    private IEnumerator ResetTurningFlag()
    {
        isTurning = true;
        yield return new WaitForSeconds(turnDelay);

        transform.Rotate(0, 180, 0);
        isTurning = false;
    }
}
