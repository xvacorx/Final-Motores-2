using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleSwitch : MonoBehaviour
{
    bool isRotating = false;
    bool isLateralView = false;
    Vector3 storedVelocity;
    float lastRotationY;
    Rigidbody rb;
    TilePositioning tilePositioning;
    TileManager tileManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        tilePositioning = GetComponent<TilePositioning>();
        tileManager = FindObjectOfType<TileManager>();
    }

    private void OnEnable()
    {
        EventManager.OnSwitch += SwitchView;
    }

    private void OnDisable()
    {
        EventManager.OnSwitch -= SwitchView;
    }

    private void Update()
    {
        LimitMovement();
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

        storedVelocity = rb.velocity;

        Coroutine moveCoroutine = StartCoroutine(tilePositioning.PositionToNearestTileCenter(1f));
        Coroutine rotateCoroutine = StartCoroutine(RotatePlayerSmoothUnscaled());

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

        yield return moveCoroutine;
        yield return rotateCoroutine;
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

        while (elapsedTime < 1f)
        {
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / 1f);
            elapsedTime += Time.unscaledDeltaTime; // Usar Time.unscaledDeltaTime para que la rotación no se vea afectada por la pausa
            yield return null;
        }

        transform.rotation = targetRotation;
        lastRotationY = transform.rotation.eulerAngles.y;
        isRotating = false;
    }

    void LimitMovement()
    {
        bool isFrontView = !isLateralView;
        Vector3 position = transform.position;

        Vector2 lineBounds = tileManager.GetLineBounds(position, isFrontView);

        if (isFrontView)
        {
            float xMin = lineBounds.x;
            float xMax = lineBounds.y;

            position.x = Mathf.Clamp(position.x, xMin, xMax);
        }
        else
        {
            float zMin = lineBounds.x;
            float zMax = lineBounds.y;

            position.z = Mathf.Clamp(position.z, zMin, zMax);
        }

        transform.position = new Vector3(position.x, transform.position.y, position.z);
    }
}