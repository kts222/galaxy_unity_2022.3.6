using UnityEngine;

namespace GalaxyData
{
    /// <summary>
    /// Stores the exact JSON export payload. The importer intentionally preserves
    /// all fields, nested structures, and dynamic keys from the source dataset.
    /// </summary>
    [CreateAssetMenu(fileName = "GalaxyData", menuName = "Galaxy/Data Asset")]
    public sealed class GalaxyDataAsset : ScriptableObject
    {
        [SerializeField, TextArea(8, 30)]
        private string json;

        /// <summary>Original JSON data, preserved without field loss.</summary>
        public string Json => json;

        /// <summary>
        /// Replace the stored payload with validated source JSON.
        /// Called by the Editor importer.
        /// </summary>
        public void SetJson(string sourceJson)
        {
            json = sourceJson;
        }
    }
}
