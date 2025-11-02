using Unity.Cinemachine;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    private CinemachineConfiner2D camera; 
    private Player player;
    private string toSearch;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camera = (CinemachineConfiner2D)GameObject.Find("CinemachineCamera").GetComponent<CinemachineConfiner2D>();
        player = (Player)GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        toSearch = "Room" + player.RoomNumber + "Boundary";
        camera.BoundingShape2D = GameObject.Find(toSearch).GetComponent<BoxCollider2D>();
    }
}
