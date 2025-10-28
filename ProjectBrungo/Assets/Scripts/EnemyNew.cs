using System;
using UnityEngine;

public class EnemyNew : MonoBehaviour
{
    [Header("Behavior Settings")]
    public bool trackPlayer;
    public float damage;
    public float enemySpeed = 2f;
    public float totalFreezeTime = 1f;
    public float totalTimeDirection = 3f;
    public float totalMovementStop = 1f;

    [Header("References")]
    public GameObject hitbox;
    //public Health playerHealth;
    //public Player playerMovement;

    private Animator animator;
    private Health playerHealth;
    private Player playerMovement;
    private Rigidbody2D rb;
    private PlayerAwarenessController playerAwarenessController;

    private bool directionClear = true;
    private bool idle = true;
    private float freezeTime;
    private float movementStop;
    private float timeDirection;
    private Vector2 targetDirection;
    private Vector2 wallDirection;
    private Vector2 oldPosition;
    private Vector2 olderPosition;

    private System.Random rnd = new System.Random();

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAwarenessController = GetComponent<PlayerAwarenessController>();
        playerMovement = (Player)GameObject.Find("Player").GetComponent<Player>();
        playerHealth = (Health)GameObject.Find("Player").GetComponent<Health>();
        idle = true;
        animator.SetBool("isIdle", idle);
        movementStop = totalMovementStop;
        PickNewDirection();
    }

    void FixedUpdate()
    {
        // Timer updates
        freezeTime -= Time.deltaTime;
        timeDirection += Time.deltaTime;
        movementStop -= Time.deltaTime;

        // Idle Failsafe
        Vector2 currentPosition = rb.position;
        bool positionUnchanged =
            Vector2.Distance(currentPosition, oldPosition) < 0.001f &&
            Vector2.Distance(oldPosition, olderPosition) < 0.001f;

        // If frozen (e.g., just hit player)
        if (freezeTime > 0)
        {
            if (positionUnchanged)
                SetIdle(true);
            rb.linearVelocity = Vector2.zero;
            UpdatePositions(currentPosition);
            return;
        }

        // If chasing player
        if (playerAwarenessController.AwareOfPlayer && trackPlayer)
        {
            ChasePlayer();
            UpdatePositions(currentPosition);
            return;
        }

        // Handle idle vs movement cycle
        if (idle)
        {
            rb.linearVelocity = Vector2.zero;

            // After idle time expires, start moving again
            if (movementStop <= 0)
            {
                SetIdle(false);
                PickNewDirection();
            }
        }
        else
        {
            rb.linearVelocity = targetDirection * enemySpeed;

            // After moving for a set duration, pause again
            if (timeDirection >= totalTimeDirection)
            {
                SetIdle(true);
                rb.linearVelocity = Vector2.zero;

                // Reset timers for next cycle
                timeDirection = 0;
                movementStop = totalMovementStop;
            }
        }

        // Update stored positions at the end
        UpdatePositions(currentPosition);
    }

    private void PickNewDirection()
    {
        timeDirection = 0;
        directionClear = false;
        // Pick random direction
        while (!directionClear)
        {
            int dir = rnd.Next(1, 5);
            switch (dir)
            {
                case 1: targetDirection = Vector2.left; break;
                case 2: targetDirection = Vector2.up; break;
                case 3: targetDirection = Vector2.right; break;
                case 4: targetDirection = Vector2.down; break;
            }
            if (wallDirection != targetDirection)
            {
                directionClear = true;
            }
        }
        wallDirection = Vector2.zero;
        updateAnimation(targetDirection);
    }

    private void SetIdle(bool value)
    {
        idle = value;
        animator.SetBool("isIdle", value);
    }

    private void ChasePlayer()
    {
        SetIdle(false);
        targetDirection = playerAwarenessController.DirectionToPlayer.normalized;
        updateAnimation(targetDirection);
        rb.linearVelocity = targetDirection * enemySpeed;
    }

    private void UpdatePositions(Vector2 current)
    {
        olderPosition = oldPosition;
        oldPosition = current;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerMovement.KBCounter = playerMovement.KBTotalTime;
            if (collision.transform.position.x <= transform.position.x && (collision.transform.position.y + 4 >= transform.position.y && collision.transform.position.y + 4 < transform.position.y))
            {
                playerMovement.KBFromDirection.Set(1, 0);
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
                playerMovement.KBFromDirection.Set(-1, 0);
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

            rb.linearVelocity = Vector2.zero;
            idle = true;
            animator.SetBool("isIdle", idle);
            freezeTime = totalFreezeTime;
            movementStop = totalMovementStop;
            timeDirection = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Wall Collision Detected — stopping movement");
            rb.linearVelocity = Vector2.zero;

            // Store last wall direction
            wallDirection = targetDirection;
            oldPosition = rb.position;

            // Reset for next direction choice
            idle = true;
            animator.SetBool("isIdle", idle);
            movementStop = totalMovementStop * 0.5f; // shorter idle pause after wall hit
            timeDirection = totalTimeDirection + 1;  // forces a new direction next update
        }
    }

    private void updateAnimation(Vector2 move)
    {
        if (move.x > 0 && Mathf.Abs(move.x) > Mathf.Abs(move.y))
            animator.SetInteger("Facing", 1); // Right
        else if (move.x < 0 && Mathf.Abs(move.x) > Mathf.Abs(move.y))
            animator.SetInteger("Facing", -1); // Left
        else if (move.y > 0)
            animator.SetInteger("Facing", 2); // Up
        else if (move.y < 0)
            animator.SetInteger("Facing", -2); // Down
    }
}
