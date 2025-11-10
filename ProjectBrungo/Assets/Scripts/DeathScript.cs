using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.VisualScripting.Member;

public class DeathScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Animator an;
    public AudioClip clip;
    public void Dying()
    {
        SceneManager.LoadSceneAsync("DeathScreen");
        an = (Animator)GameObject.Find("DeathAnimation").GetComponent<Animator>();
    }

    public void Pantsing()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void isDead()
    {
        gameObject.SetActive(true);
    }

    public void isAlive()
    {
        gameObject.SetActive(true);
        an.SetBool("Fail", false);
    }

    public void playSound()
    {
        AudioSource.PlayClipAtPoint(clip, GameObject.Find("Player").transform.position);
    }

    public void SpinSound()
    {
        GetComponent<AudioSource>().Play();
        GetComponent<AudioSource>().pitch -= 0.08f; 
    }
}
