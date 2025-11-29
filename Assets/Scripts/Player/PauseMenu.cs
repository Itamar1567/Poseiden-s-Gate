using UnityEngine;

public class PauseMenu : StartMenu
{

    public void ResumeGame()
    {
        PlayButtonClickSound();
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);
    }

    public void QuitToMainButton()
    {
        PlayButtonClickSound();
        Time.timeScale = 1.0f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartScene");
    }

}
