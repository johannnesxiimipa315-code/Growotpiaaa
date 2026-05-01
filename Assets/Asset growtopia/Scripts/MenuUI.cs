using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public AudioSource bgMusic; // drag AudioSource dari Inspector

    void Start()
    {
        bgMusic.loop = true;
        bgMusic.Play(); // mulai musik pas menu dibuka
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
