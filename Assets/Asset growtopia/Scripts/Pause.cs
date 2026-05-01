using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{   
    public GameObject pausePanel;
    private bool isPaused = false;

    void Start()
    {
        pausePanel.SetActive(false);
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
    public void ExitGame()
    {
        SceneManager.LoadScene("awalan");

    }
    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
}
