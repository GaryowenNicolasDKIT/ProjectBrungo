using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Dying()
    {
        SceneManager.LoadSceneAsync("DeathScreen");
    }

    public void isDead()
    {
        gameObject.SetActive(true);
    }
}
