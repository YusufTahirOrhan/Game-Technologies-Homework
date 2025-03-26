using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class SceneReference
{
#if UNITY_EDITOR
    [SerializeField] private SceneAsset sceneAsset;
#endif
    [SerializeField] private string sceneName;

#if UNITY_EDITOR
    public void SetSceneAsset(SceneAsset asset)
    {
        sceneAsset = asset;
        sceneName = asset != null ? asset.name : string.Empty;
    }
#endif

    public string SceneName => sceneName;
}
