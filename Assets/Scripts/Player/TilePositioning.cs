using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePositioning : MonoBehaviour
{
    public float tileSize = 1f;

    public IEnumerator PositionToNearestTileCenter(float moveDuration)
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = new Vector3(
            Mathf.Round(startPosition.x / tileSize) * tileSize,
            startPosition.y,
            Mathf.Round(startPosition.z / tileSize) * tileSize
        );

        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }
}