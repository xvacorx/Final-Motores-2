using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform portalDestination;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (portalDestination != null)
            {
                other.gameObject.transform.position = portalDestination.position;
            }
            else
            {
                Debug.LogWarning("Portal destination is not set!");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (portalDestination != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, portalDestination.position);
            Gizmos.DrawSphere(portalDestination.position, 0.2f);
        }
    }
}