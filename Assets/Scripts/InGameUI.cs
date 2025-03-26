using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    public static InGameUI Instance;

    public GameObject PauseMenuPanel;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
    private void Update()
    {
        Debug.Log(Time.timeScale);
    }
    public void OpenPauseMenu(bool isPaused)
    {
        PauseMenuPanel.SetActive(isPaused);
        SetTimeScale(isPaused == true ? 0 : 1);
    }

    public void ReturnToMainMenu()
    {
        LoadScene(0);
    }

    public void Restart()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
        SetTimeScale(1);
    }

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }
}
