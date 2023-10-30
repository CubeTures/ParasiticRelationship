using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMovementController : MonoBehaviour
{
    public Animator m_animator;
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
        try{
            if (m_animator.GetBool("moving") == false){
                m_animator.SetBool("moving", true);
            }
        }
        catch (Exception e){
            print("Animator Loaded in inactive scene");
        }
        transform.Translate(movementSpeed * Time.deltaTime * Vector2.left);
    }

    private void FlipDirection()
    {
        try{
            m_animator.SetBool("moving", false);    
        }
        catch (Exception e){
            print("animator loaded in inactive scene");
        }
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
