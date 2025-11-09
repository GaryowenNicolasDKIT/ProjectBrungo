using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    public string type;
    public int keysRequired;
    public AudioClip doorMove;
    public AudioClip doorOpened;
    private AudioSource source;


    private Animator animator;
    private Health player;
    private LayerMask layer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        //layer = GetComponent<LayerMask>();
        layer = LayerMask.NameToLayer("Default");
        player = (Health)GameObject.Find("Player").GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(type.ToLower() == "regdoor")
            {
                if(int.Parse(player.keyText.text) > 0)
                {
                    player.keyUsed("key");
                    keysRequired -= 1;
                    source.Play();
                    if(keysRequired <= 0)
                    {
                        
                        //layer = LayerMask.NameToLayer("Floor");
                        layer = LayerMask.NameToLayer("Floor");
                        animator.SetBool("IsOpen", true);
                    }
                    else
                    {
                        animator.SetTrigger("NextStage");
                    }
                }
            }
            else if (type.ToLower() == "bigdoor")
            {
                if (int.Parse(player.keyBigText.text) > 0)
                {
                    player.keyUsed("big key");
                    keysRequired -= 1;
                    source.Play();
                    if (keysRequired <= 0)
                    {
                        
                        //layer = LayerMask.NameToLayer("Floor");
                        layer = LayerMask.NameToLayer("Floor");
                        animator.SetBool("IsOpen", true);
                    }
                    else
                    {
                        animator.SetTrigger("NextStage");
                    }
                }
            }
            else if (type.ToLower() == "kingdoor")
            {
                if (int.Parse(player.keyKingText.text) > 0)
                {
                    player.keyUsed("king key");
                    keysRequired -= 1;
                    source.Play();
                    if (keysRequired <= 0)
                    {
                        
                        //layer = LayerMask.NameToLayer("Floor");
                        layer = LayerMask.NameToLayer("Floor");
                        animator.SetBool("IsOpen", true);
                    }
                    else
                    {
                        animator.SetTrigger("NextStage");
                    }
                }
            }
        }
    }

    public void DisableCollision()
    {
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public void doorMoveSound()
    {
        AudioSource.PlayClipAtPoint(doorMove, transform.position);
    }
    public void doorOpenSound()
    {
        AudioSource.PlayClipAtPoint(doorOpened, transform.position);
    }
}
