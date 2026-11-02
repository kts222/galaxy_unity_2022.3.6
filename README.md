# Galaxy Unity Data Importer

Unity 2022.3.6 project tooling for importing the exported `galaxy-data.json` dataset into a ScriptableObject asset.

## Usage

1. Open this project in Unity 2022.3.6.
2. In the Project window, select `Assets/GalaxyData/galaxy-data.json` (or another JSON TextAsset).
3. Open **Tools → Galaxy → JSON to ScriptableObject**.
4. Click **Create / Update Galaxy Data Asset**.
5. The resulting asset is saved under `Assets/GalaxyData/Generated/GalaxyData.asset`.

The generated asset stores the original JSON payload without dropping fields, including nested arrays, the icon SVG strings, and keys that Unity `JsonUtility` cannot directly map. Runtime consumers can access the preserved JSON via `GalaxyDataAsset.Json` and parse only the data they need.

## Scope

Only the data contained in the source JSON is imported. No gameplay systems, UI, networking, or unrelated data are included.

## Project layout

- `Assets/GalaxyData/galaxy-data.json` — source dataset
- `Assets/GalaxyData/Scripts/GalaxyDataAsset.cs` — ScriptableObject container
- `Assets/GalaxyData/Editor/GalaxyDataImporterWindow.cs` — Editor import window

