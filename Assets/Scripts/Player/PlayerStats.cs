using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    Animator animator;
    PlayerMovement movement;

    public TextMeshProUGUI healthTMP;

    public int health;
    public int damage;

    private int currentHealth;
    private int healthLevel;
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
        UpdateHealthLevel();

    }
    public void JumpOnKill()
    {
        movement.JumpAboveEnemy();
    }
    public void LoseLife(int amount)
    {
        animator.SetTrigger("damage");
        health -= amount;
        UpdateHealthLevel();
        if (health <= 0)
        {
            movement.enabled = false;
            Debug.Log("Player Death");
            animator.SetTrigger("death");
            Destroy(gameObject, 0.5f);
        }
    }
    private void OnDestroy()
    {
        EventManager.TriggerPlayerDeath();
    }
    void UpdateHealthLevel()
    {
        healthLevel = Mathf.Max(0, (health - currentHealth) / 25);
        healthTMP.text = "lives: " + (healthLevel.ToString());
    }
}