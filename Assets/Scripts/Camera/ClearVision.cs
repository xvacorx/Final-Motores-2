using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearVision : MonoBehaviour
{
    public float checkInterval = 0.1f;
    public float triggerRadius = 5f;
    public Color gizmoColor = Color.red;
    public Material invisibleMaterial;

    private List<RendererData> hiddenRenderers = new List<RendererData>();

    private class RendererData
    {
        public MeshRenderer renderer;
        public Material[] originalMaterials;

        public RendererData(MeshRenderer renderer)
        {
            this.renderer = renderer;
            this.originalMaterials = renderer.sharedMaterials;
        }
    }

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
                    if (!hiddenRenderers.Exists(data => data.renderer == render))
                    {
                        RendererData rendererData = new RendererData(render);
                        hiddenRenderers.Add(rendererData);

                        Material[] invisibleMaterials = new Material[render.sharedMaterials.Length];
                        for (int i = 0; i < invisibleMaterials.Length; i++)
                        {
                            invisibleMaterials[i] = invisibleMaterial;
                        }
                        render.sharedMaterials = invisibleMaterials;
                    }
                }
            }

            // Reactivar renderers que ya no están en el área
            hiddenRenderers.RemoveAll(data =>
            {
                if (!currentRenderers.Contains(data.renderer))
                {
                    data.renderer.sharedMaterials = data.originalMaterials;
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
