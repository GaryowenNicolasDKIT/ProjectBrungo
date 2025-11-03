using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float KBForce;
    public float KBCounter;
    public float KBTotalTime;
    public int RoomNumber;

    public Vector2 KBFromDirection;
    
    private const string flashRedAnim = "FlashRed";
    private bool idle = true;
    private float untilIdleTime = 0;
    private float defaultIdleTime = 3;
    private bool isMoving = false;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movementDirection;
    private Vector2 lastMovementDirection;
    private Vector2 startPosition;
    private SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        movementDirection.Set(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        lastMovementDirection = movementDirection;
        movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        updateAnimation(movementDirection,lastMovementDirection);

        if (movementDirection.x == 0 && movementDirection.y == 0)
        {
            untilIdleTime += Time.deltaTime;
            if (untilIdleTime > defaultIdleTime)
            {
                idle = true;
                animator.SetBool("Idle", true);
            }
        }
        else
        {
            untilIdleTime = 0;
            idle = false;
            animator.SetBool("Idle", false);
        }

    }

    private void FixedUpdate()
    {
        if(Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        if (KBCounter <= 0)
        {
            if (isMoving == true)
            {
                rb.linearVelocity = movementDirection * speed;
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    rb.linearVelocity = movementDirection / 2;
                }
                rb.linearVelocity = movementDirection * 0;
            }
        }
        else
        {
            rb.linearVelocity = new Vector2((-1 * KBFromDirection.x) * KBForce, (-1 * KBFromDirection.y) * KBForce);
            KBCounter -= Time.deltaTime;
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
                animator.SetInteger("Direction", 1); // Animation Right
            }
            else if (move.x < 0 && move.y != 1)
            {
                animator.SetInteger("Direction", -1); // Animation Left
            }
            else if (move.y < 0 && move.x != 1)
            {
                animator.SetInteger("Direction", -2); // Animation Down
            }
            else if (move.y > 0 && move.x != 1)
            {
                animator.SetInteger("Direction", 2); // Animation Up
            }
        }
        else
        {
            if (lastMove.x > 0 && lastMove.y != 1)
            {
                animator.SetInteger("Direction", 1); // Animation Right

            }
            else if (lastMove.x < 0 && lastMove.y != 1)
            {
                animator.SetInteger("Direction", -1); // Animation Left
            }
            else if (lastMove.y < 0 && lastMove.x != 1)
            {
                animator.SetInteger("Direction", -2); // Animation Down
            }
            else if (lastMove.y > 0 && lastMove.x != 1)
            {
                animator.SetInteger("Direction", 2); // Animation Up
            }
        }
    }
       public void knockback()
    {
        rb.linearVelocity = (-1 * lastMovementDirection) * speed;
        animator.SetTrigger(flashRedAnim);
        //flashRed();
    }  
    
}
