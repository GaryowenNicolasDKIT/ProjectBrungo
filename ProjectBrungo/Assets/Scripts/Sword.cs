using UnityEngine;
using UnityEngine.UIElements;

public class Sword : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Rigidbody2D rbody;
    public int damage;
    public float maxSwingTime;

    private float swingTime = 9999;
    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    public void swing(int rotation, int direction)
    {
        gameObject.transform.Rotate(0, 0, rotation, 0);
        swingTime = maxSwingTime;
        
    }
    void Update()
    {
        if (swingTime != 9999)
        {
            swingTime -= Time.deltaTime;
        }

        if (swingTime <= 0)
        {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.CompareTag("Enemy")) { 
            gameObject.GetComponent<EnemyNew>().takeDamage(damage);
        }
    }
}
// Update is called once per frame
