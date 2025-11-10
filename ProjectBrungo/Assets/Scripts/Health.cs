using System;
using UnityEngine;
using UnityEngine.InputSystem.Processors;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float health;
    public float keyAmount;
    public float maxHealth;
    public int numHearts;

    public Image key;
    public Image keyBig;
    public Image keyKing;
    public Image[] hearts;
    public Sprite emptyHeart;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Player player;
    public Text keyText;
    public Text keyBigText;
    public Text keyKingText;
    public DeathScript deathScreen;

    private bool foundHalf = false;
    private bool isDead = false;
    private float invulnerable;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
        UpdateHealthDisplay();
        key.enabled = false;
        keyBig.enabled = false;
        keyKing.enabled = false;
        keyText.enabled = false;
        keyBigText.enabled = false;
        keyKingText.enabled = false;
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
        if (health <= 0)
        {
            isDead = true;
            deathScreen.isDead();
            Destroy(GameObject.Find("Player").GetComponent<BoxCollider2D>());
        }
        UpdateHealthDisplay();
    }

    public void Heal(float heal)
    {
        if ((health + heal) <= maxHealth)
        {
            health += heal;
        }
        else
        {
            health = maxHealth;
        }
        UpdateHealthDisplay();
    }

    public void UpdateHealthDisplay()
    {
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
            // hearts[i].enabled = i < numHearts;
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

    public void keyCollected(String keyType)
    {
        if (keyType.ToLower() == "key")
        {
            key.enabled = true;
            keyText.enabled = true;
            int collected = int.Parse(keyText.text);
            keyText.text = (collected + 1).ToString();
        }
        else if (keyType.ToLower() == "big key")
        {
            keyBig.enabled = true;
            keyBigText.enabled = true;
            int collected = int.Parse(keyBigText.text);
            keyBigText.text = (collected + 1).ToString();
        }
        else if (keyType.ToLower() == "king key")
        {
            keyKing.enabled = true;
            keyKingText.enabled = true;
            int collected = int.Parse(keyKingText.text);
            keyKingText.text = (collected + 1).ToString();
        }
    }

    public void keyUsed(String keyType)
    {
        if (keyType.ToLower() == "key")
        {
            int collected = int.Parse(keyText.text);
            keyText.text = (collected - 1).ToString();
            if(collected <= 0)
            {
                key.enabled = false;
                keyText.enabled = false;
            }
        }
        else if (keyType.ToLower() == "big key")
        {
            keyBig.enabled = true;
            keyBigText.enabled = true;
            int collected = int.Parse(keyBigText.text);
            keyBigText.text = (collected - 1).ToString();
            if (collected <= 0)
            {
                key.enabled = false;
                keyText.enabled = false;
            }
        }
        else if (keyType.ToLower() == "king key")
        {
            keyKing.enabled = true;
            keyKingText.enabled = true;
            int collected = int.Parse(keyKingText.text);
            keyKingText.text = (collected - 1).ToString();
            if (collected <= 0)
            {
                key.enabled = false;
                keyText.enabled = false;
            }
        }
    }
}
