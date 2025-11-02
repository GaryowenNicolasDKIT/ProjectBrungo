using Unity.Cinemachine;
using UnityEngine;

public class RoomChange : MonoBehaviour
{
    public int SetRoom;
    public int Direction;
    private Player player;
    private Vector2 move = Vector2.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        player = (Player)GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.RoomNumber = SetRoom;
            if(Direction == 1)
            {
                move.y = player.GetComponent<Rigidbody2D>().position.y + 4;
                move.x = player.GetComponent<Rigidbody2D>().position.x;

            }
            else if (Direction == 2)
            {
                move.x = player.GetComponent<Rigidbody2D>().position.x + 2;
                move.y = player.GetComponent<Rigidbody2D>().position.y;
            }
            else if (Direction == 3)
            {
                move.y = player.GetComponent<Rigidbody2D>().position.y - 4;
                move.x = player.GetComponent<Rigidbody2D>().position.x;
            }
            else
            {
                move.x = player.GetComponent<Rigidbody2D>().position.y + 2;
                move.y = player.GetComponent<Rigidbody2D>().position.y;
            }
            player.GetComponent<Rigidbody2D>().MovePosition(move);
        }
    }
}
