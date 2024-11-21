using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public void OnRetryGame()
    {
        // Restart the game by reloading the current scene
        Time.timeScale = 1f;  // Unfreeze the game
        SceneManager.LoadScene("SampleScene");
    }

    public void OnExitGame()
    {
        // Quit the application
        Application.Quit();
    }
}
