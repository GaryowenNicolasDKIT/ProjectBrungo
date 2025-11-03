using System.IO.Pipes;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public string pickupType;
    public float value;

    private Health player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = (Health)GameObject.Find("Player").GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(pickupType.ToLower() == "key")
            {
                player.keyCollected(pickupType.ToLower());
                Destroy(gameObject);
            }
            else if(pickupType.ToLower() == "big key")
            {
                player.keyCollected(pickupType.ToLower());
                Destroy(gameObject);
            }
            else if (pickupType.ToLower() == "king key")
            {
                player.keyCollected(pickupType.ToLower());
                Destroy(gameObject);
            }
            else if (pickupType.ToLower() == "heart")
            {
                player.Heal(value);
                Destroy(gameObject);
            }
        }
    }

}