using System;
using Unity.Mathematics;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class Enemy : MonoBehaviour
{
    public float damage;
    public Health playerHealth;
    public Player playerMovement;
    public GameObject hitbox;
    public float timeDirection;
    public float totalTimeDirection;
    public float stopTime;
    public bool trackPlayer;
    
    [SerializeField] private float enemySpeed;
    
    System.Random rnd = new System.Random();
    private float movementStop = 0;
    private int decideDirection;
    private Rigidbody2D rb;
    private PlayerAwarenessController playerAwarenessController;
    private Vector2 targetDirection;
    private Animator animator;
    private bool idle = false;
    private Vector2 lastMovementDirection = Vector2.down;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAwarenessController = GetComponent<PlayerAwarenessController>();
        
        targetDirection = new Vector2(rnd.Next(-1, 1),rnd.Next(-1,1));
    }



    // Update is called once per frame
    private void FixedUpdate()
    {

        UpdateTargetDirection();
        SetVelocity();
        movementStopper();
        timeDirection += Time.deltaTime;
        movementStop -= Time.deltaTime;
        updateAnimation(targetDirection, lastMovementDirection);
        

    }

    private void UpdateTargetDirection()
    {
        idle = false;
        if (playerAwarenessController.AwareOfPlayer && trackPlayer == true)
        {
            lastMovementDirection = targetDirection;
            targetDirection = playerAwarenessController.DirectionToPlayer;
        }
        else if (timeDirection > totalTimeDirection)
        {
            idle = true;
            animator.SetBool("isIdle", idle);
            timeDirection = 0;
            decideDirection = rnd.Next(1, 4);
            if (decideDirection == 1)
            {
                targetDirection = Vector2.left;
            }
            else if (decideDirection == 2)
            {
                targetDirection = Vector2.up;
            }
            else if (decideDirection == 3)
            {
                targetDirection = Vector2.right;
            }
            else
            {
                targetDirection = Vector2.down;
            }
        }
    }

    private void SetVelocity()
    {
        rb.linearVelocity = targetDirection * enemySpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //To Ensure Enemy Stops when hitting a wall instead of going through it
        if(collision.gameObject.tag != "Player")
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerMovement.KBCounter = playerMovement.KBTotalTime;
            if (collision.transform.position.x <= transform.position.x && (collision.transform.position.y + 4 >= transform.position.y && collision.transform.position.y + 4 < transform.position.y))
            {
                playerMovement.KBFromDirection.Set(1,0);
            }
            else if (collision.transform.position.x <= transform.position.x && collision.transform.position.y <= transform.position.y)
            {
                playerMovement.KBFromDirection.Set(1, 1);
            }
            else if (collision.transform.position.x <= transform.position.x && collision.transform.position.y > transform.position.y)
            {
                playerMovement.KBFromDirection.Set(1, -1);
            }


            else if (collision.transform.position.x > transform.position.x && (collision.transform.position.y + 4 >= transform.position.y && collision.transform.position.y + 4 < transform.position.y))
            {
                playerMovement.KBFromDirection.Set(-1,0);
            }
            else if (collision.transform.position.x > transform.position.x && collision.transform.position.y <= transform.position.y)
            {
                playerMovement.KBFromDirection.Set(-1, 1);
            }
            else if (collision.transform.position.x > transform.position.x && collision.transform.position.y > transform.position.y)
            {
                playerMovement.KBFromDirection.Set(-1, -1);
            }


            else if (collision.transform.position.y <= transform.position.y && (collision.transform.position.x + 4 >= transform.position.x && collision.transform.position.x + 4 < transform.position.x))
            {
                playerMovement.KBFromDirection.Set(0, 1);
            }
            else if (collision.transform.position.y > transform.position.y && (collision.transform.position.x + 4 >= transform.position.x && collision.transform.position.x + 4 < transform.position.x))
            {
                playerMovement.KBFromDirection.Set(0, -1);
            }
            playerHealth.TakeDamage(damage);
            movementStop = stopTime;
            
        }
    }
    private void movementStopper()
    {
        if(movementStop > 0)
        {
            idle = true;
            animator.SetBool("isIdle", idle);
            rb.linearVelocity = Vector2.zero;
        }
        
    }
    private void updateAnimation(Vector2 move, Vector2 lastMove)
    {
        animator.SetFloat("MoveX", move.x);
        animator.SetFloat("MoveY", move.y);
        if (idle != true)
        {
            if (move.x > 0 && move.y != 1)
            {
                animator.SetInteger("Facing", 1); // Animation Right
            }
            else if (move.x < 0 && move.y != 1)
            {
                Debug.Log("Direction -1");
                animator.SetInteger("Facing", -1); // Animation Left
            }
            else if (move.y < 0 && move.x != 1)
            {
                Debug.Log("Direction -1");
                animator.SetInteger("Facing", -2); // Animation Down
            }
            else if (move.y > 0 && move.x != 1)
            {
                Debug.Log("Direction 2");
                animator.SetInteger("Facing", 2); // Animation Up
            }
        }
        else
        {
            if (lastMove.x > 0 && lastMove.y != 1)
            {
                animator.SetInteger("Facing", 1); // Animation Right

            }
            else if (lastMove.x < 0 && lastMove.y != 1)
            {
                animator.SetInteger("Facing", -1); // Animation Left
            }
            else if (lastMove.y < 0 && lastMove.x != 1)
            {
                animator.SetInteger("Facing", -2); // Animation Down
            }
            else if (lastMove.y > 0 && lastMove.x != 1)
            {
                animator.SetInteger("Facing", 2); // Animation Up
            }
        }
    }
}
