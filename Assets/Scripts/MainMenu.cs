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
        // Önce mevcut çocuklarý temizleyelim.
        foreach (Transform child in SceneButtonContainer)
        {
            Destroy(child.gameObject);
        }
        // Her sahne için buton oluþtur.
        foreach (var sceneRef in SceneReferences)
        {
            GameObject buttonObject = Instantiate(SceneButtonPrefab, SceneButtonContainer);
            Button button = buttonObject.GetComponent<Button>();

            TMP_Text btnText = buttonObject.GetComponentInChildren<TMP_Text>();

            if (btnText != null)
            {
                btnText.text = sceneRef.SceneName;
            }

            // Butona týklandýðýnda OnSceneButtonClicked çaðrýlýyor.
            button.onClick.AddListener(() => OnSceneButtonClicked(sceneRef));
        }
    }

    public void OnSceneButtonClicked(SceneReference selectedScene)
    {
        Debug.Log("Selected Scene: " + selectedScene.SceneName);

        SceneManager.LoadScene(selectedScene.SceneName);
    }
}
