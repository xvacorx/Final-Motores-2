using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public Transform[] tiles; // Array de tiles del mapa

    private void Start()
    {
        // Inicializar tiles con los hijos del GameObject actual
        tiles = GetComponentsInChildren<Transform>()
            .Where(t => t != transform) // Excluir el GameObject padre
            .ToArray();
    }

    public Vector2 GetLineBounds(Vector3 playerPosition, bool isFrontView)
    {
        List<float> lineTiles = new List<float>();

        foreach (var tile in tiles)
        {
            // Agregar a la lista de tiles si está en la misma línea que el jugador
            if (isFrontView)
            {
                if (Mathf.Abs(tile.position.z - playerPosition.z) < 0.1f)
                {
                    lineTiles.Add(tile.position.x);
                }
            }
            else
            {
                if (Mathf.Abs(tile.position.x - playerPosition.x) < 0.1f)
                {
                    lineTiles.Add(tile.position.z);
                }
            }
        }

        if (lineTiles.Count == 0)
        {
            // Retorna un rango inválido si no hay tiles en la misma línea
            return new Vector2(0, 0);
        }

        float min = Mathf.Min(lineTiles.ToArray());
        float max = Mathf.Max(lineTiles.ToArray());

        return new Vector2(min, max);
    }
}
