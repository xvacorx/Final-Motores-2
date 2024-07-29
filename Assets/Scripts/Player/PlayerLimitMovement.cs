using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.UIElements;

public class PlayerLimitMovement : MonoBehaviour
{
    private TileManager tileManager;

    private PlayerMovement playerMovement;

    private void Start()
    {
        tileManager = FindObjectOfType<TileManager>();
        playerMovement = GetComponent<PlayerMovement>();
        transform.position = new Vector3(0f, 0.5f, 0f);
    }

    private void Update()
    {
        bool isFrontView = !playerMovement.isLateralView;
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