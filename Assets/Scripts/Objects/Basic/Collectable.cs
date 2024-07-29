using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public bool rotationEnabled = true;

    public GameObject collectEffect;
    void Update()
    {
        Rotate();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Collect();
        }
    }
    void Rotate()
    {
        if (rotationEnabled)
        {
            float rotation = rotationSpeed * Time.deltaTime;

            transform.Rotate(0, rotation, 0);
        }
    }
    public abstract void Collect();
    protected void ActivateCollectedEffect()
    {
        if(collectEffect != null)
        {
            GameObject effect = Instantiate(collectEffect, transform.position, Quaternion.identity);
            Destroy(effect, 1.5f);
        }
    }
}
