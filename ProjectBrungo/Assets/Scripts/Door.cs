using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    public string type;
    private Animator animator;
    private Health player;
    private LayerMask layer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        //layer = GetComponent<LayerMask>();
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
                    gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
                    //layer = LayerMask.NameToLayer("Floor");
                    layer = LayerMask.NameToLayer("Floor");
                    animator.SetBool("IsOpen",true);
                    player.keyUsed("key");
                }
            }
            else if (type.ToLower() == "bigdoor")
            {
                if (int.Parse(player.keyBigText.text) > 0)
                {
                    gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
                    //layer = LayerMask.NameToLayer("Floor");
                    layer = LayerMask.NameToLayer("Floor");
                    animator.SetBool("IsOpen", true);
                    player.keyUsed("big key");
                }
            }
            else if (type.ToLower() == "kingdoor")
            {
                if (int.Parse(player.keyKingText.text) > 0)
                {
                    gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
                    //layer = LayerMask.NameToLayer("Floor");
                    layer = LayerMask.NameToLayer("Floor");
                    animator.SetBool("IsOpen", true);
                    player.keyUsed("king key");
                }
            }
        }
    }
}
