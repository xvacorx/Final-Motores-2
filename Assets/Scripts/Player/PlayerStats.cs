using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    Animator animator;
    PlayerMovement movement;

    [SerializeField] float health;
    [SerializeField] float damage;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
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
            Debug.Log("Player Death");
            Destroy(gameObject);
        }
    }
}
