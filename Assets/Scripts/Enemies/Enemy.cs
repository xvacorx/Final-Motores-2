using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] GameObject deathEffect;
    public float health;
    [SerializeField] bool hurting;
    [SerializeField] float damage;
    public virtual void LoseLife(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }
    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (hurting)
            {
                PlayerStats.Instance.LoseLife(damage);
            }
            else
            {
                PlayerStats.Instance.JumpOnKill();
                LoseLife(1f);
            }
        }
    }
    public virtual void Die()
    {
        if (deathEffect != null)
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, 1f);
        }
        Destroy(gameObject);
    }
}
