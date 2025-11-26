using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{

    [SerializeField] protected GameObject controlsScreen;
    [SerializeField] protected GameObject mainScreen;

    public void PlayButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Round_Based");
    }

    public virtual void ControlsButton()
    {
        controlsScreen.SetActive(true);
        mainScreen.SetActive(false);
    }

    public virtual void BackToMain()
    {
        controlsScreen.SetActive(false);
        mainScreen.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}