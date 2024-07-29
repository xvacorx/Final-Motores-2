using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Billboard : MonoBehaviour
{
    bool isRotating = false;
    public bool isLateralView = false;
    float lastRotationY;
    private void OnEnable()
    {
        EventManager.OnSwitch += SwitchView;
    }

    private void OnDisable()
    {
        EventManager.OnSwitch -= SwitchView;
    }
    void SwitchView()
    {
        if (!isRotating)
        {
            StartCoroutine(RotatePlayerSmoothUnscaled());
        }
    }
    IEnumerator RotatePlayerSmoothUnscaled()
    {
        isLateralView = !isLateralView;

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
}
