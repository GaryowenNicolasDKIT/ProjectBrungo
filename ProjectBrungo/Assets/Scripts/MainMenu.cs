using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class MainMenu : MonoBehaviour
{
    private GameObject Buttons;

    private void Start()
    {
        Buttons = GameObject.Find("Buttons");
    }
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("MainDungeon");
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }



    public void QuitGame()
    {
        Application.Quit();
    }

}
