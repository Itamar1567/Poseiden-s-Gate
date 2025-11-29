using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{

    [SerializeField] protected AudioClip audioClip;
    private AudioSource audioSource;

    [SerializeField] protected GameObject controlsScreen;
    [SerializeField] protected GameObject mainScreen;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    protected void PlayButtonClickSound()
    {

        audioSource.PlayOneShot(audioClip);
    }

    public void PlayButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Round_Based");
    }

    public virtual void ControlsButton()
    {

        PlayButtonClickSound();
        controlsScreen.SetActive(true);
        mainScreen.SetActive(false);
    }

    public virtual void BackToMain()
    {
        PlayButtonClickSound();
        controlsScreen.SetActive(false);
        mainScreen.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}