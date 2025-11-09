using System.IO.Pipes;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public string pickupType;
    public float value;

    private AudioSource source;
    private Health player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        source = GetComponent<AudioSource>();
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
                AudioSource.PlayClipAtPoint(source.clip, transform.position);
                player.keyCollected(pickupType.ToLower());
                Destroy(gameObject);
            }
            else if(pickupType.ToLower() == "big key")
            {
                AudioSource.PlayClipAtPoint(source.clip, transform.position);
                player.keyCollected(pickupType.ToLower());
                Destroy(gameObject);
            }
            else if (pickupType.ToLower() == "king key")
            {
                AudioSource.PlayClipAtPoint(source.clip, transform.position);
                player.keyCollected(pickupType.ToLower());
                Destroy(gameObject);
            }
            else if (pickupType.ToLower() == "heart")
            {
                AudioSource.PlayClipAtPoint(source.clip, transform.position);
                player.Heal(value);
                Destroy(gameObject);
            }
        }
    }

}