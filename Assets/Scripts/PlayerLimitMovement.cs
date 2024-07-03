using UnityEngine;

public class PlayerLimitMovement : MonoBehaviour
{
    public TileManager tileManager; // Referencia al TileManager

    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        bool isFrontView = !playerMovement.isLateralView;
        Vector3 position = transform.position;

        // Obtener los l�mites de la l�nea de tiles en la vista actual
        Vector2 lineBounds = tileManager.GetLineBounds(position, isFrontView);

        // Ajustar l�mites en funci�n de la vista
        if (isFrontView)
        {
            // Eje X es el principal, ajustar l�mites en X
            float xMin = lineBounds.x;
            float xMax = lineBounds.y;

            // Ajustar posici�n en X
            position.x = Mathf.Clamp(position.x, xMin, xMax);
        }
        else
        {
            // Eje Z es el principal, ajustar l�mites en Z
            float zMin = lineBounds.x;
            float zMax = lineBounds.y;

            // Ajustar posici�n en Z
            position.z = Mathf.Clamp(position.z, zMin, zMax);
        }

        // Solo aplicar cambios en X/Z, mantener Y intacto para permitir el salto
        transform.position = new Vector3(position.x, transform.position.y, position.z);
    }
}
