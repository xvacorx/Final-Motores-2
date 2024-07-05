using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotationSpeed = 100f;

    void Update()
    {
        Rotate();
    }
    private void OnTriggerEnter(Collider other)
    {
        
    }
    void Rotate()
    {
        float rotation = rotationSpeed * Time.deltaTime;

        transform.Rotate(0, rotation, 0);
    }
}