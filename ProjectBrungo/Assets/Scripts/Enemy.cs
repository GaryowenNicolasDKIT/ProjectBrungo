using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
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
    private Vector2 wallDirection;
    private Animator animator;
    private bool idle = false;
    private Vector2 lastMovementDirection = Vector2.down;
    private bool clearDirection;
    private Vector2 lastPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        idle = true;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAwarenessController = GetComponent<PlayerAwarenessController>();
        
        targetDirection = new Vector2(rnd.Next(-1, 1),rnd.Next(-1,1));
    }



    // Update is called once per frame
    private void FixedUpdate()
    {
        lastPosition = rb.position;
        UpdateTargetDirection();
        SetVelocity();
        movementStopper();
        timeDirection += Time.deltaTime;
        movementStop -= Time.deltaTime;
        //updateAnimation(targetDirection, lastMovementDirection);
        if (rb.position == lastPosition)
        {
            //idle = true;
            //animator.SetBool("isIdle", idle);
        }
        

    }

    private void UpdateTargetDirection()
    {
        
        if (playerAwarenessController.AwareOfPlayer && trackPlayer == true)
        {
            idle = false;
            animator.SetBool("isIdle", idle);
            lastMovementDirection = targetDirection;
            targetDirection = playerAwarenessController.DirectionToPlayer;
            updateAnimation(targetDirection, lastMovementDirection);
        }
        else if (timeDirection > totalTimeDirection)
        {
            //idle = true;
            //animator.SetBool("isIdle", idle);
            timeDirection = 0;
            clearDirection = false;
            while (clearDirection == false)
            {
                decideDirection = rnd.Next(1, 5);
                    if (decideDirection == 1)
                    {
                        Debug.Log("Movement Direction set to Left");
                        targetDirection = Vector2.left;
                        animator.SetInteger("Facing", -1);
                    }
                    else if (decideDirection == 2)
                    {
                        Debug.Log("Movement Direction set to Up");
                        targetDirection = Vector2.up;
                        animator.SetInteger("Facing", -2);
                    }
                    else if (decideDirection == 3)
                    {
                        Debug.Log("Movement Direction set to Left");
                        targetDirection = Vector2.right;
                        animator.SetInteger("Facing", 1);
                    }
                    else
                    {
                        Debug.Log("Movement Direction set to Down");
                        targetDirection = Vector2.down;
                        animator.SetInteger("Facing", 2);
                    }
                if (targetDirection.x != wallDirection.x && targetDirection.y != wallDirection.y)
                {
                    Debug.Log("Momement Direction " + targetDirection + " Seen as clear");
                    clearDirection = true;
                    //animator.SetInteger("Direction", decideDirection);
                }
                else
                {
                    targetDirection *= -1;
                    animator.SetInteger("Facing",(animator.GetInteger("Facing") * -1));
                    clearDirection = true;
                    Debug.Log("Movement Direction Inversion (" + targetDirection + ") Seen as clear");
                    //animator.SetInteger("Direction", decideDirection);
                }

                
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
            idle = true;
            animator.SetBool("isIdle", idle);
            timeDirection = totalTimeDirection + 1;
            wallDirection = targetDirection;
            Debug.Log("Wall Collision");
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
            Debug.Log("Idle set to True");
            animator.SetBool("isIdle", idle);
            rb.linearVelocity = Vector2.zero;
        }
        
    }
    private void updateAnimation(Vector2 move, Vector2 lastMove)
    {
        if (idle != true)
        {
            if (move.x > 0 && move.y != 1)
            {
                animator.SetInteger("Facing", 1); // Animation Right
            }
            else if (move.x < 0 && move.y != 1)
            {
                animator.SetInteger("Facing", -1); // Animation Left
            }
            //else if ((move.y < 0 && move.x != 1) || (move.y < -0.5 && move.x != 1)) < Attempt to Make Down Happen More
            else if (move.y < 0 && move.x < 1)
            {
                animator.SetInteger("Facing", -2); // Animation Down
            }
            //else if (move.y > 0 && move.x != 1 || (move.y > 0.5 && move.x != 1)) < Attemt to Make Up Happen More
            else if (move.y > 0 && move.x != 1)
            {
                animator.SetInteger("Facing", 2); // Animation Up
            }
        }
        //else
        //{
        //    if (lastMove.x > 0 && lastMove.y != 1)
        //    {
        //        Debug.Log("Facing set to Right");
        //        animator.SetInteger("Facing", 1); // Animation Right

        //    }
        //    else if (lastMove.x < 0 && lastMove.y != 1)
        //    {
        //        Debug.Log("Facing set to Left");
        //        animator.SetInteger("Facing", -1); // Animation Left
        //    }
        //    //else if ((lastMove.y < 0 && lastMove.x != 1) || (lastMove.y < -0.5 && lastMove.x != 1))
        //    else if (lastMove.y < 0 && lastMove.x < 1)
        //    {

        //        animator.SetInteger("Facing", -2); // Animation Down
        //    }
        //    //else if ((lastMove.y > 0 && lastMove.x != 1) || (lastMove.y > 0.5 && lastMove.x != 1))
        //    else if (lastMove.y > 0 && lastMove.x != 1)
        //    { 
        //        animator.SetInteger("Facing", 2); // Animation Up
        //    }
        //}
    }
}
