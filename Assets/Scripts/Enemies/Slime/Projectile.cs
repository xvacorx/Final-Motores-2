using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime;
    public float damage;
    public float speed;

    public GameObject hitEffect;
    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
    private void Update()
    {

        transform.Translate(Vector3.left * speed * Time.deltaTime);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Enemy"))
        {
            if (other.gameObject.CompareTag("Player"))
            {
                PlayerStats.Instance.LoseLife(damage);
            }
            if (hitEffect != null)
            {
                GameObject hit = Instantiate(hitEffect, transform.position, Quaternion.identity);
                Destroy(hit, 1f);
            }
            Destroy(gameObject);
        }
    }
}