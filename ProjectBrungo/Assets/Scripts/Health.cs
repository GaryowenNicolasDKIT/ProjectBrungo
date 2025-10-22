using System;
using UnityEngine;
using UnityEngine.InputSystem.Processors;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public int numHearts;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;
    public Player player;

    private bool foundHalf = false;
    private bool isDead = false;
    private float invulnerable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
        for (int i = 0; i < hearts.Length; i++)
        {

            if (i < health && health % 1 != 0.5)
            {
                hearts[i].sprite = fullHeart;
            }
            else if (i < health && health % 1 == 0.5)
            {
                hearts[i].sprite = halfHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }


            if (i < numHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        invulnerable -= Time.deltaTime;

        
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        player.knockback();
        if(health <= 0)
        {
            isDead = true;
        }
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health - 0.5f)
            {
                hearts[i].sprite = fullHeart;
            }
            else if (i < health)
            {
                hearts[i].sprite = halfHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            // Enable or disable hearts based on total heart slots
            hearts[i].enabled = i < numHearts;
        }
    }
}
