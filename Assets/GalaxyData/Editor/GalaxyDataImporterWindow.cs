using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GalaxyData.Editor
{
    /// <summary>
    /// Editor utility to create/update a ScriptableObject from the selected JSON TextAsset.
    /// Menu: Tools/Galaxy/JSON to ScriptableObject
    /// </summary>
    public sealed class GalaxyDataImporterWindow : EditorWindow
    {
        private TextAsset sourceJson;
        private const string OutputFolder = "Assets/GalaxyData/Generated";
        private const string OutputAssetPath = OutputFolder + "/GalaxyData.asset";

        [MenuItem("Tools/Galaxy/JSON to ScriptableObject")]
        private static void OpenWindow()
        {
            GetWindow<GalaxyDataImporterWindow>("Galaxy Data Importer");
        }

        private void OnEnable()
        {
            if (Selection.activeObject is TextAsset selected)
                sourceJson = selected;
        }

        private void OnSelectionChange()
        {
            if (Selection.activeObject is TextAsset selected)
            {
                sourceJson = selected;
                Repaint();
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Galaxy JSON Importer", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "Select the exported galaxy-data JSON TextAsset. Import creates or updates one ScriptableObject while preserving the complete JSON payload.",
                MessageType.Info);

            sourceJson = (TextAsset)EditorGUILayout.ObjectField(
                "Source JSON", sourceJson, typeof(TextAsset), false);

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Choose JSON File..."))
                    ChooseJsonFile();

                GUI.enabled = sourceJson != null;
                if (GUILayout.Button("Create / Update ScriptableObject"))
                    CreateOrUpdateAsset();
                GUI.enabled = true;
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Output", OutputAssetPath, EditorStyles.wordWrappedLabel);
        }

        private void ChooseJsonFile()
        {
            string selectedPath = EditorUtility.OpenFilePanel(
                "Select Galaxy JSON", Application.dataPath, "json");

            if (string.IsNullOrEmpty(selectedPath))
                return;

            string projectAssetsPath = Path.GetFullPath(Application.dataPath)
                .Replace('\\\\', '/');
            string normalizedPath = Path.GetFullPath(selectedPath).Replace('\\\\', '/');

            if (normalizedPath.StartsWith(projectAssetsPath + "/", StringComparison.OrdinalIgnoreCase))
            {
                string assetPath = "Assets" + normalizedPath.Substring(projectAssetsPath.Length);
                sourceJson = AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath);
            }
            else
            {
                const string importFolder = "Assets/GalaxyData/Imported";
                EnsureFolder(importFolder);
                string destination = importFolder + "/" + Path.GetFileName(selectedPath);
                File.Copy(selectedPath, destination, true);
                AssetDatabase.ImportAsset(destination, ImportAssetOptions.ForceUpdate);
                sourceJson = AssetDatabase.LoadAssetAtPath<TextAsset>(destination);
            }

            Repaint();
        }

        private void CreateOrUpdateAsset()
        {
            if (sourceJson == null)
            {
                EditorUtility.DisplayDialog("Galaxy Data Importer", "Select a JSON file first.", "OK");
                return;
            }

            string json = sourceJson.text;
            if (string.IsNullOrWhiteSpace(json) || !json.TrimStart().StartsWith("{", StringComparison.Ordinal))
            {
                EditorUtility.DisplayDialog("Invalid JSON", "The selected file does not appear to contain a JSON object.", "OK");
                return;
            }

            EnsureFolder(OutputFolder);

            GalaxyDataAsset dataAsset = AssetDatabase.LoadAssetAtPath<GalaxyDataAsset>(OutputAssetPath);
            if (dataAsset == null)
            {
                dataAsset = CreateInstance<GalaxyDataAsset>();
                dataAsset.SetJson(json);
                AssetDatabase.CreateAsset(dataAsset, OutputAssetPath);
            }
            else
            {
                dataAsset.SetJson(json);
                EditorUtility.SetDirty(dataAsset);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Selection.activeObject = dataAsset;
            EditorGUIUtility.PingObject(dataAsset);

            EditorUtility.DisplayDialog(
                "Import Complete",
                "GalaxyData ScriptableObject created/updated at:\\n" + OutputAssetPath,
                "OK");
        }

        private static void EnsureFolder(string folderPath)
        {
            if (AssetDatabase.IsValidFolder(folderPath))
                return;

            string parent = Path.GetDirectoryName(folderPath)?.Replace('\\\\', '/');
            string folderName = Path.GetFileName(folderPath);
            if (!string.IsNullOrEmpty(parent) && !AssetDatabase.IsValidFolder(parent))
                EnsureFolder(parent);

            if (!string.IsNullOrEmpty(parent))
                AssetDatabase.CreateFolder(parent, folderName);
        }
    }
}
