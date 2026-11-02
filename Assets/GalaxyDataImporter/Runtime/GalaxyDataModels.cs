using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GalaxyDataImporter
{
    [Serializable] public class GalaxySettingsData
    {
        public string initialBase;
        public string initialShip;
        public int startingCredits;
    }
    [Serializable] public class GalaxyPropertyData { public int id; public string title; public string unit; }
    [Serializable] public class GalaxySlotData { public int id; public string title; }
    [Serializable] public class GalaxyResourceCategoryData { public int id; public string name; public string icon; }
    [Serializable] public class GalaxyEquipmentCategoryData { public int id; public string name; public List<int> propIds = new List<int>(); public string icon; }
    [Serializable] public class GalaxyShipCategoryData { public int id; public string name; public List<int> propIds = new List<int>(); public string icon; }
    [Serializable] public class GalaxyValueData { public int id; public string value; }
    [Serializable] public class GalaxyHoldData { public string name; public int capacity; }

    [Serializable]
    public class GalaxyItemData
    {
        public string id;
        public string name;
        public int category;
        public List<GalaxyValueData> values = new List<GalaxyValueData>();
        public int price_buy;
        public int price_sale;
        public string icon;
        [JsonIgnore] public int slot;
        [JsonIgnore] public bool hasSlot;
        [JsonProperty("slot")] public int? SlotValue { set { hasSlot = value.HasValue; slot = value ?? 0; } }
        public int prop2Number;
        public bool prop2IsArray;
        public List<string> prop2Strings = new List<string>();
        [JsonProperty("prop2")] private JToken Prop2Token
        {
            set
            {
                prop2Strings.Clear();
                prop2IsArray = value != null && value.Type == JTokenType.Array;
                if (prop2IsArray)
                    foreach (var item in (JArray)value) prop2Strings.Add(item.ToString());
                else if (value != null && value.Type == JTokenType.Integer)
                    prop2Number = value.Value<int>();
            }
        }
        public List<int> slots = new List<int>();
        public List<GalaxyHoldData> holds = new List<GalaxyHoldData>();
    }

    [Serializable]
    public class GalaxyJsonRoot
    {
        public int version;
        public string exportedAt;
        public Dictionary<string, string> icons;
        public List<string> baseNames = new List<string>();
        public GalaxySettingsData settings;
        public List<GalaxyPropertyData> properties = new List<GalaxyPropertyData>();
        public List<GalaxySlotData> slots = new List<GalaxySlotData>();
        [JsonProperty("categories-resources")] public List<GalaxyResourceCategoryData> categories_resources = new List<GalaxyResourceCategoryData>();
        [JsonProperty("categories-equipments")] public List<GalaxyEquipmentCategoryData> categories_equipments = new List<GalaxyEquipmentCategoryData>();
        [JsonProperty("categories-ships")] public List<GalaxyShipCategoryData> categories_ships = new List<GalaxyShipCategoryData>();
        public List<GalaxyItemData> ships = new List<GalaxyItemData>();
        public List<GalaxyItemData> resources = new List<GalaxyItemData>();
        public List<GalaxyItemData> equipments = new List<GalaxyItemData>();
    }
    [Serializable] public class GalaxyIconEntry { public string key; [TextArea] public string svgPath; }
}
