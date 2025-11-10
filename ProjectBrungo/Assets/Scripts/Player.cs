using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public float speed;
    public float KBForce;
    public float KBCounter;
    public float KBTotalTime;
    public int RoomNumber;

    public GameObject swordPrefab;
    public DeathScript deathScreen;

    public Vector2 KBFromDirection;

    private Animator animator;
    private AudioSource source;
    private const string flashRedAnim = "FlashRed";
    private bool idle = true;
    private float untilIdleTime = 0;
    private float defaultIdleTime = 3;
    private bool isMoving = false;
    private Rigidbody2D rb;
    private int swordRotation;
    private int swordRotationFin;
    private Quaternion blankQuart;
    private Vector2 finPosition;
    private Vector2 movementDirection;
    private Vector2 lastMovementDirection;
    private Vector2 playerPosition;
    private Vector2 startPosition;
    private Vector2 swordSpawnPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        source = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        movementDirection.Set(0, 0);
        blankQuart.Set(0, 0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = rb.position;
        lastMovementDirection = movementDirection;
        movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        updateAnimation(movementDirection, lastMovementDirection);
        if (GameObject.Find("MindSword(Clone)") == null)
        {
            animator.SetBool("Attacking", false);
        }
        else
        {
            animator.SetBool("Attacking", true);
        }
        if (movementDirection.x == 0 && movementDirection.y == 0 && (GameObject.Find("MindSword(Clone)") == null))
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
        if (Input.GetKeyDown(KeyCode.Space) && (GameObject.Find("MindSword(Clone)") == null))
        {
            swordAttack();

        }
    }

    private void swordAttack()
    {

        if (animator.GetInteger("Direction") == 1)
        {
            swordSpawnPosition.Set(playerPosition.x + 0.6f, playerPosition.y + 0.8f);
            swordRotation = 315;
            finPosition = new Vector2(playerPosition.x + 0.6f, playerPosition.y - 0.8f);
            swordRotationFin = 225;
        }
        else if (animator.GetInteger("Direction") == -1)
        {
            swordSpawnPosition.Set(playerPosition.x - 0.6f, playerPosition.y - 0.8f);
            swordRotation = 135;
            finPosition = new Vector2(playerPosition.x - 0.6f, playerPosition.y + 0.8f);
            swordRotationFin = 45;
        }
        else if (animator.GetInteger("Direction") == 2)
        {
            swordSpawnPosition.Set(playerPosition.x - 0.6f, playerPosition.y + 0.8f);
            swordRotation = 45;
            finPosition = new Vector2(playerPosition.x + 0.6f, playerPosition.y + 0.8f);
            swordRotationFin = 315;
        }
        else if (animator.GetInteger("Direction") == -2)
        {
            swordSpawnPosition.Set(playerPosition.x + 0.6f, playerPosition.y - 0.8f);
            swordRotation = 225;
            finPosition = new Vector2(playerPosition.x - 0.6f, playerPosition.y - 0.8f);
            swordRotationFin = 135;
        }
        GameObject sword = Instantiate(swordPrefab, swordSpawnPosition, blankQuart);
        sword.GetComponent<Sword>().swing(finPosition, swordRotation, animator.GetInteger("Direction"), swordRotationFin);
    }

    private void FixedUpdate()
    {
        if ((Input.GetButton("Horizontal") || Input.GetButton("Vertical")) && GameObject.Find("MindSword(Clone)") == null)
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
        if (idle != true && (GameObject.Find("MindSword(Clone)") == null))
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
        else if (GameObject.Find("MindSword(Clone)") == null)
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
        source.Play();
        //flashRed();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("WinBox"))
        {
            deathScreen.isAlive();
        }
    }
}
