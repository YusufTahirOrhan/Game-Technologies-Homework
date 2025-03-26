using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    public static InGameUI Instance;

    public GameObject PauseMenuPanel;

    [SerializeField]
    private Image _healthBarFillImage;
    [SerializeField]
    private Image _healthBarTrailingFillImage;

    [SerializeField]
    private float _trailDelay = 0.4f;



    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(this);
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
    
    public void UpdateHealthBar(float percentage)
    {
        float ratio = percentage / 100;
        Sequence sequance = DOTween.Sequence();
        sequance.Append(_healthBarFillImage.DOFillAmount(ratio, 0.25f)).SetEase(Ease.InOutSine);
        sequance.AppendInterval(_trailDelay);

        sequance.Append(_healthBarTrailingFillImage.DOFillAmount(ratio, 0.3f)).SetEase(Ease.InOutSine);
        sequance.Play();
    }
}
