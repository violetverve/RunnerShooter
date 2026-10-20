using System.Reflection;
using RunnerShooter.Core.Rendering;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace RunnerShooter.Editor
{
    /// <summary>
    /// Records the shader variants the editor renders during play and registers them for the boot shader warm-up.
    /// </summary>
    public static class ShaderVariantRecorder
    {
        private const string MenuRoot = "Tools/Shader Warmup/";
        private const string CollectionPath = "Assets/Settings/GameplayShaders.shadervariants";
        private const string AddressableGroupName = "Boot";

        private const BindingFlags InternalStatic = BindingFlags.NonPublic | BindingFlags.Static;

        [MenuItem(MenuRoot + "1. Start Recording")]
        private static void StartRecording()
        {
            InvokeShaderUtil("ClearCurrentShaderVariantCollection");
            Debug.Log("[ShaderWarmup] Recording started. Press Play, use every effect (shoot, hit, explode, finish), "
                + "stop Play, then run Tools/Shader Warmup/2. Save Recording.");
        }

        [MenuItem(MenuRoot + "2. Save Recording")]
        private static void SaveRecording()
        {
            if (EditorApplication.isPlaying)
            {
                Debug.LogWarning("[ShaderWarmup] Stop Play mode before saving the recording.");
                return;
            }

            int shaderCount = (int)InvokeShaderUtil("GetCurrentShaderVariantCollectionShaderCount");
            int variantCount = (int)InvokeShaderUtil("GetCurrentShaderVariantCollectionVariantCount");

            if (variantCount == 0)
            {
                Debug.LogWarning("[ShaderWarmup] Nothing recorded yet. Press Play and use the game first.");
                return;
            }

            InvokeShaderUtil("SaveCurrentShaderVariantCollection", CollectionPath);
            AssetDatabase.ImportAsset(CollectionPath, ImportAssetOptions.ForceUpdate);
            MakeAddressable();

            Debug.Log($"[ShaderWarmup] Saved {shaderCount} shaders / {variantCount} variants to {CollectionPath} "
                + $"as Addressable '{RenderingConstants.ShaderVariantsAddress}'.",
                AssetDatabase.LoadAssetAtPath<ShaderVariantCollection>(CollectionPath));
        }

        private static void MakeAddressable()
        {
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            AddressableAssetGroup group = settings.FindGroup(AddressableGroupName);
            string guid = AssetDatabase.AssetPathToGUID(CollectionPath);

            AddressableAssetEntry entry = settings.CreateOrMoveEntry(guid, group);
            entry.SetAddress(RenderingConstants.ShaderVariantsAddress);

            AssetDatabase.SaveAssets();
        }

        private static object InvokeShaderUtil(string methodName, params object[] arguments)
        {
            MethodInfo method = typeof(ShaderUtil).GetMethod(methodName, InternalStatic);
            return method.Invoke(null, arguments);
        }
    }
}
