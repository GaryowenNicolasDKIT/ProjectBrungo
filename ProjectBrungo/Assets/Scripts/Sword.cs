using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Sword : MonoBehaviour
{
    Rigidbody2D rbody;
    public int damage;
    public float maxSwingTime;

    private bool hitEnemy = false;
    private float swingTime = 9999;
    private int startRotation;
    private int finishRotation;
    private Vector2 startPosition;
    private Vector2 finishPosition;





    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();

        startPosition = rbody.position;
    }

    public void swing(Vector2 fPosition, int sRotation, int direction, int fRotation)
    {
        finishPosition = fPosition;
        finishRotation = fRotation;
        startRotation = sRotation; 

        transform.rotation = Quaternion.Euler(0, 0, startRotation);

        swingTime = maxSwingTime;

    }

    void Update()
    {
        swingTime -= Time.deltaTime;

        float swingProgress = 1 - (swingTime / maxSwingTime);
        swingProgress = Mathf.Clamp01(swingProgress); 

        transform.position = Vector2.Lerp(startPosition, finishPosition, swingProgress);

        Quaternion startRot = Quaternion.Euler(0, 0, startRotation);
        Quaternion finishRot = Quaternion.Euler(0, 0, finishRotation);

        transform.rotation = Quaternion.Lerp(startRot, finishRot, swingProgress);

        if (swingTime <= 0)
        {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hitEnemy != true)
        {
            EnemyNew enemy = collision.gameObject.GetComponent<EnemyNew>();
            enemy.KBCounter = enemy.KBTotalTime;
            if (collision.transform.position.x <= transform.position.x && (collision.transform.position.y + 4 >= transform.position.y && collision.transform.position.y + 4 < transform.position.y))
            {
                enemy.KBFromDirection.Set(1, 0);
            }
            else if (collision.transform.position.x <= transform.position.x && collision.transform.position.y <= transform.position.y)
            {
                enemy.KBFromDirection.Set(1, 1);
            }
            else if (collision.transform.position.x <= transform.position.x && collision.transform.position.y > transform.position.y)
            {
                enemy.KBFromDirection.Set(1, -1);
            }


            else if (collision.transform.position.x > transform.position.x && (collision.transform.position.y + 4 >= transform.position.y && collision.transform.position.y + 4 < transform.position.y))
            {
                enemy.KBFromDirection.Set(-1, 0);
            }
            else if (collision.transform.position.x > transform.position.x && collision.transform.position.y <= transform.position.y)
            {
                enemy.KBFromDirection.Set(-1, 1);
            }
            else if (collision.transform.position.x > transform.position.x && collision.transform.position.y > transform.position.y)
            {
                enemy.KBFromDirection.Set(-1, -1);
            }


            else if (collision.transform.position.y <= transform.position.y && (collision.transform.position.x + 4 >= transform.position.x && collision.transform.position.x + 4 < transform.position.x))
            {
                enemy.KBFromDirection.Set(0, 1);
            }
            else if (collision.transform.position.y > transform.position.y && (collision.transform.position.x + 4 >= transform.position.x && collision.transform.position.x + 4 < transform.position.x))
            {
                enemy.KBFromDirection.Set(0, -1);
            }

            collision.gameObject.GetComponent<EnemyNew>().takeDamage(damage);
            hitEnemy = true;

        }
    }
}
// Update is called once per frame
