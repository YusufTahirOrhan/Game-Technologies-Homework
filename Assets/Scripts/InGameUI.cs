using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    public static InGameUI Instance;

    public GameObject PauseMenuPanel;
    public GameObject WinMenuPanel;
    public GameObject LoseMenuPanel;
    public Image Skull;
    public GameObject LoseMenu;


    [SerializeField]
    private Image _healthBarFillImage;
    [SerializeField]
    private Image _healthBarTrailingFillImage;

    [SerializeField]
    private float _trailDelay = 0.4f;



    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void OpenPauseMenu(bool isPaused)
    {
        PauseMenuPanel.SetActive(isPaused);
        SetTimeScale(isPaused == true ? 0 : 1);
    }

    public void OpenWinMenu()
    {
        WinMenuPanel.SetActive(true);
        SetTimeScale(0);
    }

    public void OpenLoseMenu()
    {
        StartCoroutine(StartLoseSequance());
    }

    private IEnumerator StartLoseSequance(float fadeInStartDelay = 3, float fadeInTime = 2f)
    {
        yield return new WaitForSecondsRealtime(fadeInStartDelay);

        LoseMenuPanel.SetActive(true);
        Image loseImage = LoseMenuPanel.GetComponent<Image>();

        // Baþlangýçta alpha'yý 0 yap
        Color tempColor = loseImage.color;
        tempColor.a = 0f;
        loseImage.color = tempColor;

        // DOTween ile alpha'yý fade in yaparak 1'e çýkar
        loseImage.DOFade(1f, fadeInTime);

        // Baþlangýçta alpha'yý 0 yap
        Color tempColorSkull = Skull.color;
        tempColorSkull.a = 0f;
        Skull.color = tempColorSkull;

        // DOTween ile alpha'yý fade in yaparak 1'e çýkar
        Skull.DOFade(1f, fadeInTime);


        // Fade süresi kadar bekle
        yield return new WaitForSecondsRealtime(fadeInTime);
        Skull.transform.SetAsLastSibling();
        LoseMenu.SetActive(true);
        SetTimeScale(0);
    }

    public void ReturnToMainMenu()
    {
        LoadScene(0);
    }

    public void Restart()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
        SetTimeScale(1);
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
