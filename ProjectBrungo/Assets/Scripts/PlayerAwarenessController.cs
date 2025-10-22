using Unity.VisualScripting;
using UnityEngine;

public class PlayerAwarenessController : MonoBehaviour
{
    public bool AwareOfPlayer { get; private set; }

    public Vector2 DirectionToPlayer { get; private set; }

    public Vector2 lastPlayerPosition;

    [SerializeField]
    private float playerAwarenessDistance;
    private Transform player;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [System.Obsolete]
    private void Awake()
    {
        player = FindObjectOfType<Player>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 enemyToPlayerVector = player.position - transform.position;
        DirectionToPlayer = enemyToPlayerVector.normalized;

        if (enemyToPlayerVector.magnitude <= playerAwarenessDistance)
        {
            AwareOfPlayer = true;
        }
        else
        {
            //This stupid fucking line lost me 1h30m...god I hate Unity (and myself?) > position = lastPlayerPosition;
            AwareOfPlayer = false;
        }
    }

    private void FixedUpdate()
    {
       
    }
}
