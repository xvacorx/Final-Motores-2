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

    private AudioSource damageAudio;
    private AudioSource deathAudio;
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
        damageAudio = GetComponent<AudioSource>();
        deathAudio = GetComponent<AudioSource>();
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
        damageAudio.Play();
        animator.SetTrigger("damage");
        health -= amount;
        UpdateHealthLevel();
        if (health <= 0)
        {
            deathAudio.Play();
            movement.enabled = false;
            Debug.Log("Player Death");
            animator.SetTrigger("death");
            Destroy(gameObject, 0.5f);
            EventManager.TriggerPlayerDeath();
        }
    }
    void UpdateHealthLevel()
    {
        healthLevel = Mathf.Max(0, (health - currentHealth) / 25);
        healthTMP.text = "lives: " + (healthLevel.ToString());
    }
}