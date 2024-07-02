using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private bool isSideView = false;  // Determina si la cámara está en vista lateral o frontal

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;
    }

    private void OnEnable()
    {
        EventManager.OnSwitch += ToggleView;
    }

    private void OnDisable()
    {
        EventManager.OnSwitch -= ToggleView;
    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 moveDirection;

        if (isSideView)
        {
            moveDirection = new Vector3(horizontalInput, 0, 0);
        }
        else
        {
            moveDirection = new Vector3(0, 0, horizontalInput);
        }

        rb.AddForce(moveDirection * moveSpeed);
    }

    private void ToggleView()
    {
        isSideView = !isSideView;
    }
}