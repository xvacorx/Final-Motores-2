using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private bool isGrounded;
    public bool isLateralView = false;
    private bool isRotating = false;
    private float rotationDuration = 1f;
    private TilePositioning tilePositioning;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tilePositioning = GetComponent<TilePositioning>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);
        HandleMovement();
        HandleJump();
        SwitchingBehaviour();
    }

    private void OnEnable()
    {
        EventManager.OnSwitch += SwitchView;
    }

    private void OnDisable()
    {
        EventManager.OnSwitch -= SwitchView;
    }

    void HandleMovement()
    {
        float move = Input.GetAxis("Horizontal") * moveSpeed;

        if (isLateralView)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, move);
        }
        else
        {
            rb.velocity = new Vector3(move, rb.velocity.y, rb.velocity.z);
        }
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }
    void SwitchingBehaviour()
    {
        if (isRotating) { rb.velocity = Vector3.zero; }
    }
    void SwitchView()
    {
        if (!isRotating)
        {
            StartCoroutine(SwitchViewCoroutine());
        }
    }

    IEnumerator SwitchViewCoroutine()
    {
        isLateralView = !isLateralView;
        Coroutine moveCoroutine = StartCoroutine(tilePositioning.PositionToNearestTileCenter(rotationDuration));
        Coroutine rotateCoroutine = StartCoroutine(RotatePlayer(isLateralView ? -90f : 0f));

        yield return moveCoroutine;
        yield return rotateCoroutine;
    }

    IEnumerator RotatePlayer(float targetYRotation)
    {
        isRotating = true;
        float elapsedTime = 0f;
        Quaternion initialRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, targetYRotation, 0);

        while (elapsedTime < rotationDuration)
        {
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
        isRotating = false;
    }
}