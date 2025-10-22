using UnityEngine;

public class Sniffer : MonoBehaviour
{
    
    Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        if (transform.position != lastPosition)
        {
            Debug.Log($"[Sniffer] Position changed from {lastPosition} to {transform.position}", this);
            lastPosition = transform.position;
        }
    }
}

