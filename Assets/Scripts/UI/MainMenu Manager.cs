using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] GameObject storyScreen;
    [SerializeField] GameObject creditsScreen;
    [SerializeField] GameObject mainMenuScreen;

    public void OnPlay()
    {
        SceneManager.LoadScene("Level Design");
    }

    public void OnOpenStory()
    {
        mainMenuScreen.SetActive(false);
        storyScreen.SetActive(true);
    }

    public void OnOpenCredits()
    {
        mainMenuScreen.SetActive(false);
        creditsScreen.SetActive(true);
    }

    public void OnBackToMenu()
    {
        storyScreen.SetActive(false);
        creditsScreen.SetActive(false);
        mainMenuScreen.SetActive(true);
    }
}
