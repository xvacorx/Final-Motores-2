using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearVision : MonoBehaviour
{
    public float checkInterval = 0.1f;
    public float triggerRadius = 5f;
    public Color gizmoColor = Color.red;

    private List<MeshRenderer> hiddenRenderers = new List<MeshRenderer>();

    private void Start()
    {
        StartCoroutine(CheckColliders());
    }

    private IEnumerator CheckColliders()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(checkInterval); // Usa WaitForSecondsRealtime para ignorar el timescale

            Collider[] colliders = Physics.OverlapSphere(transform.position, triggerRadius);
            HashSet<MeshRenderer> currentRenderers = new HashSet<MeshRenderer>();

            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.TryGetComponent<MeshRenderer>(out MeshRenderer render))
                {
                    currentRenderers.Add(render);
                    if (!hiddenRenderers.Contains(render))
                    {
                        render.enabled = false;
                        hiddenRenderers.Add(render);
                    }
                }
            }

            // Reactivar renderers que ya no están en el área
            hiddenRenderers.RemoveAll(renderer =>
            {
                if (!currentRenderers.Contains(renderer))
                {
                    renderer.enabled = true;
                    return true;
                }
                return false;
            });
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
    }
}
