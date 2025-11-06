using UnityEngine;

public class ExplodeParticle : MonoBehaviour
{
    private Animator animator;
    private float time = 0.7f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger("explode");
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time < 0)
        {
            Destroy(gameObject);
        }
        
    }

}
