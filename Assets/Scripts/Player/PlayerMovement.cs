using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public bool doubleJumpEnabled = false;
    bool doubleJump = true;

    private Rigidbody rb;
    private bool isGrounded;
    public bool isLateralView = false;
    private bool isRotating = false;
    private float rotationDuration = 1f;
    private TilePositioning tilePositioning;

    [SerializeField] GameObject jumpEffect;

    float move;
    Animator animator;

    private float lastRotationY;
    private Vector3 storedVelocity;

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
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            GameObject jumpingEffect = Instantiate(jumpEffect, new Vector3(transform.position.x, transform.position.y - 0.25f, transform.position.z), Quaternion.identity);
            Destroy(jumpingEffect, 1f);
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetTrigger("damage");
        }
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            //GameObject jumpingEffect = Instantiate(jumpEffect, new Vector3(transform.position.x, transform.position.y - 0.25f, transform.position.z), Quaternion.identity);
            //Destroy(jumpingEffect, 1f);
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
        if (Input.GetButtonDown("Jump") && doubleJump && !isGrounded && doubleJumpEnabled)
        {
            GameObject jumpingEffect = Instantiate(jumpEffect, new Vector3(transform.position.x, transform.position.y - 0.25f, transform.position.z), Quaternion.identity);
            Destroy(jumpingEffect, 1f);
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            doubleJump = false;
        }
        if (isGrounded) { doubleJump = true; }
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
        Time.timeScale = 0f;

        storedVelocity = rb.velocity;

        Coroutine moveCoroutine = StartCoroutine(tilePositioning.PositionToNearestTileCenter(rotationDuration));
        Coroutine rotateCoroutine = StartCoroutine(RotatePlayerSmoothUnscaled());

        yield return moveCoroutine;
        yield return rotateCoroutine;

        if (isLateralView)
        {
            rb.velocity = new Vector3(0f, rb.velocity.y, storedVelocity.x);
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX;
        }
        else
        {
            rb.velocity = new Vector3(storedVelocity.z, rb.velocity.y, 0f);
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
        }

        Time.timeScale = 1f;
    }

    IEnumerator RotatePlayerSmoothUnscaled()
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
            elapsedTime += Time.unscaledDeltaTime; // Usar Time.unscaledDeltaTime para que la rotación no se vea afectada por la pausa
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