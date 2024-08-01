using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogEnemy : Enemy
{
    public float speed;
    public float raycastLength = 1.0f;
    public LayerMask groundLayer;

    public Transform sprite;

    public bool canMoveForw;
    public bool canMoveBack;

    bool isMovingForw;
    bool isMovingBack;

    TileManager tileManager;
    AngleSwitch angleSwitch;
    Animator animator;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        tileManager = FindObjectOfType<TileManager>();
        angleSwitch = GetComponent<AngleSwitch>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        UpdateMovementOptions();
        Movement();
        RotateTowardsMovement();
    }

    public override void Die()
    {
        base.Die();
    }

    void UpdateMovementOptions()
    {
        canMoveForw = tileManager.HasMoreTilesAhead(transform.position, !angleSwitch.isLateralView);
        canMoveBack = tileManager.HasMoreTilesBackwards(transform.position, !angleSwitch.isLateralView);
    }

    void Movement()
    {
        Vector3 movement = Vector3.zero;

        if (canMoveForw && !isMovingBack)
        {
            isMovingForw = true;
            isMovingBack = false;
            movement = transform.right * speed;
        }
        else if (canMoveBack && !isMovingForw)
        {
            isMovingBack = true;
            isMovingForw = false;
            movement = transform.right * -speed;
        }
        else
        {
            rb.velocity = Vector3.zero;
            isMovingForw = false;
            isMovingBack = false;
        }

        if (movement != Vector3.zero)
        {
            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
        }

        animator.SetBool("isMoving", movement != Vector3.zero);

        CheckForGroundCollision();
    }

    void RotateTowardsMovement()
    {
        if (isMovingForw)
        {
            sprite.localEulerAngles = new Vector3(0f, 180f, 0f);
        }
        else if (isMovingBack)
        {
            sprite.localEulerAngles = new Vector3(0f, 0f, 0f);
        }
    }

    void CheckForGroundCollision()
    {
        Vector3 direction = isMovingForw ? transform.right : -transform.right;

        if (Physics.Raycast(transform.position, direction, raycastLength, groundLayer))
        {
            if (isMovingForw)
            {
                isMovingForw = false;
                isMovingBack = true;
            }
            else if (isMovingBack)
            {
                isMovingBack = false;
                isMovingForw = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 direction = isMovingForw ? transform.right : -transform.right;
        Gizmos.DrawRay(transform.position, direction * raycastLength);
    }
}
