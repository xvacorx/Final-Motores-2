using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform portalDestination;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player")){
            
        }
    }
}
