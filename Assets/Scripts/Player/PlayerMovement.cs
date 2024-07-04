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

    float move;
    Animator animator;

    private float lastRotationY;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        tilePositioning = GetComponent<TilePositioning>();
        lastRotationY = transform.rotation.eulerAngles.y;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);
        HandleMovement();
        HandleJump();
        SwitchingBehaviour();
        Animations();
    }

    private void OnEnable()
    {
        EventManager.OnSwitch += SwitchView;
    }

    private void OnDisable()
    {
        EventManager.OnSwitch -= SwitchView;
    }

    void Animations()
    {
        if (isGrounded && move != 0 && !isRotating)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("YVelocity", rb.velocity.y);
    }

    void HandleMovement()
    {
        move = Input.GetAxis("Horizontal") * moveSpeed;

        if (isLateralView)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, move);

            if (move > 0 && Mathf.Abs(transform.rotation.eulerAngles.y) != 270)
            {
                RotatePlayerInstant(-90);
            }
            else if (move < 0 && Mathf.Abs(transform.rotation.eulerAngles.y) != 90)
            {
                RotatePlayerInstant(90);
            }
        }
        else
        {
            rb.velocity = new Vector3(move, rb.velocity.y, rb.velocity.z);

            if (move > 0 && Mathf.Abs(transform.rotation.eulerAngles.y) != 0)
            {
                RotatePlayerInstant(0);
            }
            else if (move < 0 && Mathf.Abs(transform.rotation.eulerAngles.y) != 180)
            {
                RotatePlayerInstant(180);
            }
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
        Coroutine rotateCoroutine = StartCoroutine(RotatePlayerSmooth());

        yield return moveCoroutine;
        yield return rotateCoroutine;
    }

    IEnumerator RotatePlayerSmooth()
    {
        isRotating = true;
        float elapsedTime = 0f;
        Quaternion initialRotation = transform.rotation;

        float targetYRotation = isLateralView ? -90f : 0f;
        if (lastRotationY == 180 || lastRotationY == 90)
        {
            targetYRotation = isLateralView ? 90f : 180f;
        }

        Quaternion targetRotation = Quaternion.Euler(0, targetYRotation, 0);

        while (elapsedTime < rotationDuration)
        {
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
        lastRotationY = transform.rotation.eulerAngles.y;
        isRotating = false;
    }

    void RotatePlayerInstant(float targetYRotation)
    {
        transform.rotation = Quaternion.Euler(0, targetYRotation, 0);
        lastRotationY = targetYRotation;
    }
}