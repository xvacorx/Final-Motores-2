using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    Animator animator;
    PlayerMovement movement;

    public float health;
    public float damage;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
        animator = GetComponentInChildren<Animator>();
    }
    public void JumpOnKill()
    {
        movement.JumpAboveEnemy();
    }
    public void LoseLife(float amount)
    {
        animator.SetTrigger("damage");
        health -= amount;
        if (health <= 0)
        {
            movement.enabled = false;
            Debug.Log("Player Death");
            animator.SetTrigger("death");
            Destroy(gameObject, 0.5f);
        }
    }
}
