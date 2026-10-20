using UnityEditor;
using UnityEditor.SceneManagement;

namespace RunnerShooter.Editor
{
    [InitializeOnLoad]
    public static class BootPlayModeStarter
    {
        private const string BootScenePath = "Assets/Scenes/Boot.unity";

        static BootPlayModeStarter()
        {
            EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(BootScenePath);
        }
    }
}
