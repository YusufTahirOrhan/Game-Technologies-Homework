using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class SceneReference
{
#if UNITY_EDITOR
    [SerializeField] private SceneAsset _sceneAsset;
#endif
    [SerializeField] private string _sceneName;

#if UNITY_EDITOR
    public void SetSceneAsset(SceneAsset asset)
    {
        _sceneAsset = asset;
        _sceneName = asset != null ? asset.name : string.Empty;
    }
#endif

    public string SceneName => _sceneName;
}
