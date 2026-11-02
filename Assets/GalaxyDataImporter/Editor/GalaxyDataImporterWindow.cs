#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;

namespace GalaxyDataImporter.Editor
{
    public class GalaxyDataImporterWindow : EditorWindow
    {
        private TextAsset jsonFile;
        private const string OutputFolder = "Assets/GalaxyData";

        [MenuItem("Tools/Galaxy Data/JSON Importer")]
        public static void Open() => GetWindow<GalaxyDataImporterWindow>("Galaxy Data Importer");

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Galaxy Data JSON Importer", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Выберите JSON-файл внутри проекта (Assets), затем нажмите Import. Будут созданы отдельные ScriptableObject для каждой группы данных и общий GalaxyDatabase.", MessageType.Info);
            jsonFile = (TextAsset)EditorGUILayout.ObjectField("JSON file", jsonFile, typeof(TextAsset), false);
            using (new EditorGUI.DisabledScope(jsonFile == null))
            {
                if (GUILayout.Button("Import JSON → ScriptableObjects", GUILayout.Height(34)))
                    Import();
            }
        }

        private void Import()
        {
            try
            {
                var root = JsonConvert.DeserializeObject<GalaxyJsonRoot>(jsonFile.text);
                if (root == null) throw new Exception("JSON не удалось прочитать.");
                EnsureFolder(OutputFolder);

                var settings = Create<GalaxySettingsAsset>("Settings");
                settings.data = root.settings;
                settings.version = root.version;
                settings.exportedAt = root.exportedAt;
                Save(settings);

                var icons = Create<GalaxyIconsAsset>("Icons");
                icons.entries = new List<GalaxyIconEntry>();
                if (root.icons != null)
                    foreach (var pair in root.icons) icons.entries.Add(new GalaxyIconEntry { key = pair.Key, svgPath = pair.Value });
                Save(icons);
                var names = Create<GalaxyBaseNamesAsset>("BaseNames"); names.entries = root.baseNames ?? new List<string>(); Save(names);
                var properties = Create<GalaxyPropertiesAsset>("Properties"); properties.entries = Safe(root.properties); Save(properties);
                var slots = Create<GalaxySlotsAsset>("Slots"); slots.entries = Safe(root.slots); Save(slots);
                var resourceCats = Create<GalaxyResourceCategoriesAsset>("ResourceCategories"); resourceCats.entries = Safe(root.categories_resources); Save(resourceCats);
                var equipmentCats = Create<GalaxyEquipmentCategoriesAsset>("EquipmentCategories"); equipmentCats.entries = Safe(root.categories_equipments); Save(equipmentCats);
                var shipCats = Create<GalaxyShipCategoriesAsset>("ShipCategories"); shipCats.entries = Safe(root.categories_ships); Save(shipCats);
                var ships = Create<GalaxyShipsAsset>("Ships"); ships.entries = Safe(root.ships); Save(ships);
                var resources = Create<GalaxyResourcesAsset>("Resources"); resources.entries = Safe(root.resources); Save(resources);
                var equipment = Create<GalaxyEquipmentsAsset>("Equipments"); equipment.entries = Safe(root.equipments); Save(equipment);

                var db = Create<GalaxyDatabaseAsset>("GalaxyDatabase");
                db.settings = settings; db.icons = icons; db.baseNames = names; db.properties = properties;
                db.slots = slots; db.resourceCategories = resourceCats; db.equipmentCategories = equipmentCats;
                db.shipCategories = shipCats; db.ships = ships; db.resources = resources; db.equipments = equipment;
                Save(db);

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Selection.activeObject = db;
                EditorGUIUtility.PingObject(db);
                EditorUtility.DisplayDialog("Galaxy Data", "Импорт завершён. Создано 11 ScriptableObject assets в Assets/GalaxyData.", "OK");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                EditorUtility.DisplayDialog("Galaxy Data Import Error", e.Message, "OK");
            }
        }

        private static T Create<T>(string name) where T : ScriptableObject
        {
            var path = OutputFolder + "/" + name + ".asset";
            var existing = AssetDatabase.LoadAssetAtPath<T>(path);
            if (existing != null) return existing;
            var asset = CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, path);
            return asset;
        }
        private static void Save(UnityEngine.Object asset) { EditorUtility.SetDirty(asset); }
        private static List<T> Safe<T>(List<T> list) => list ?? new List<T>();
        private static void EnsureFolder(string path)
        {
            var parts = path.Split('/');
            var current = parts[0];
            for (int i = 1; i < parts.Length; i++)
            {
                var next = current + "/" + parts[i];
                if (!AssetDatabase.IsValidFolder(next)) AssetDatabase.CreateFolder(current, parts[i]);
                current = next;
            }
        }
    }
}
#endif
