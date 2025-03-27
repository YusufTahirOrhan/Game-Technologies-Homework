using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject ScenesPanel;
    public Transform SceneButtonContainer;

    public GameObject SceneButtonPrefab;
    public List<SceneReference> SceneReferences;

    

    private void Awake()
    {
        PopulateSceneButtons();
    }

    public void OnScenesPanelButtonClicked(bool isActive)
    {
        ScenesPanel.SetActive(isActive);
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }




    private void PopulateSceneButtons()
    {
        // �nce mevcut �ocuklar� temizleyelim.
        foreach (Transform child in SceneButtonContainer)
        {
            Destroy(child.gameObject);
        }
        // Her sahne i�in buton olu�tur.
        foreach (var sceneRef in SceneReferences)
        {
            GameObject buttonObject = Instantiate(SceneButtonPrefab, SceneButtonContainer);
            Button button = buttonObject.GetComponent<Button>();

            TMP_Text btnText = buttonObject.GetComponentInChildren<TMP_Text>();

            if (btnText != null)
            {
                btnText.text = sceneRef.SceneName;
            }

            // Butona t�kland���nda OnSceneButtonClicked �a�r�l�yor.
            button.onClick.AddListener(() => OnSceneButtonClicked(sceneRef));
        }
    }

    public void OnSceneButtonClicked(SceneReference selectedScene)
    {
        Debug.Log("Selected Scene: " + selectedScene.SceneName);

        SceneManager.LoadScene(selectedScene.SceneName);
    }
}
