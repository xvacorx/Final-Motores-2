using System.Collections;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Transform playerTransform;
    private Transform cameraTransform;

    private Vector3 initialOffset;
    private Quaternion initialRotation;

    private Vector3 targetOffset;
    private Quaternion targetRotation;

    public float transitionDuration = 1.0f;
    private bool isSideView = false;

    private void Start()
    {
        cameraTransform = transform;
        initialOffset = cameraTransform.position - playerTransform.position;
        initialRotation = cameraTransform.rotation;

        // Define la posici�n y rotaci�n objetivo
        targetOffset = Quaternion.Euler(0, -90, 0) * initialOffset;
        targetRotation = Quaternion.Euler(initialRotation.eulerAngles.x, initialRotation.eulerAngles.y - 90, initialRotation.eulerAngles.z);

        // Aseg�rate de que la c�mara siga al jugador al inicio
        UpdateCameraPosition();
    }

    private void Update()
    {
        // Actualiza la posici�n de la c�mara en cada frame
        UpdateCameraPosition();
    }

    private void OnEnable()
    {
        EventManager.OnSwitch += SwitchPosition;
    }

    private void OnDisable()
    {
        EventManager.OnSwitch -= SwitchPosition;
    }

    void SwitchPosition()
    {
        StartCoroutine(TransitionCamera());
    }

    void UpdateCameraPosition()
    {
        Vector3 offset = isSideView ? targetOffset : initialOffset;
        cameraTransform.position = playerTransform.position + offset;
    }

    IEnumerator TransitionCamera()
    {
        Vector3 startOffset = cameraTransform.position - playerTransform.position;
        Quaternion startRotation = cameraTransform.rotation;

        Vector3 endOffset = isSideView ? initialOffset : targetOffset;
        Quaternion endRotation = isSideView ? initialRotation : targetRotation;

        float elapsedTime = 0.0f;
        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;
            t = t * t * (3f - 2f * t); // Suavizado Slerp

            cameraTransform.position = playerTransform.position + Vector3.Lerp(startOffset, endOffset, t);
            cameraTransform.rotation = Quaternion.Slerp(startRotation, endRotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Aseg�rate de que la c�mara llegue exactamente a la posici�n y rotaci�n objetivo
        cameraTransform.position = playerTransform.position + endOffset;
        cameraTransform.rotation = endRotation;

        isSideView = !isSideView;

        // Actualiza la posici�n de la c�mara despu�s de la transici�n
        UpdateCameraPosition();
    }
}
