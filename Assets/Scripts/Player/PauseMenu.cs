using UnityEngine;

public class PauseMenu : StartMenu
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);
    }

    public void QuitToMainButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartMenu");
    }

}
