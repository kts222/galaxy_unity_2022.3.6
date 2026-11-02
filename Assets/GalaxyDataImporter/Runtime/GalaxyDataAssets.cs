using System.Collections.Generic;
using UnityEngine;

namespace GalaxyDataImporter
{
    public abstract class GalaxyDataAsset<T> : ScriptableObject
    {
        public List<T> entries = new List<T>();
    }

    [CreateAssetMenu(menuName = "Galaxy Data/Settings")]
    public class GalaxySettingsAsset : ScriptableObject { public GalaxySettingsData data; public int version; public string exportedAt; }

    [CreateAssetMenu(menuName = "Galaxy Data/Icons")]
    public class GalaxyIconsAsset : GalaxyDataAsset<GalaxyIconEntry> { }

    [CreateAssetMenu(menuName = "Galaxy Data/Base Names")]
    public class GalaxyBaseNamesAsset : ScriptableObject { public List<string> entries = new List<string>(); }

    [CreateAssetMenu(menuName = "Galaxy Data/Properties")]
    public class GalaxyPropertiesAsset : GalaxyDataAsset<GalaxyPropertyData> { }

    [CreateAssetMenu(menuName = "Galaxy Data/Slots")]
    public class GalaxySlotsAsset : GalaxyDataAsset<GalaxySlotData> { }

    [CreateAssetMenu(menuName = "Galaxy Data/Resource Categories")]
    public class GalaxyResourceCategoriesAsset : GalaxyDataAsset<GalaxyResourceCategoryData> { }

    [CreateAssetMenu(menuName = "Galaxy Data/Equipment Categories")]
    public class GalaxyEquipmentCategoriesAsset : GalaxyDataAsset<GalaxyEquipmentCategoryData> { }

    [CreateAssetMenu(menuName = "Galaxy Data/Ship Categories")]
    public class GalaxyShipCategoriesAsset : GalaxyDataAsset<GalaxyShipCategoryData> { }

    [CreateAssetMenu(menuName = "Galaxy Data/Ships")]
    public class GalaxyShipsAsset : GalaxyDataAsset<GalaxyItemData> { }

    [CreateAssetMenu(menuName = "Galaxy Data/Resources")]
    public class GalaxyResourcesAsset : GalaxyDataAsset<GalaxyItemData> { }

    [CreateAssetMenu(menuName = "Galaxy Data/Equipment")]
    public class GalaxyEquipmentsAsset : GalaxyDataAsset<GalaxyItemData> { }

    [CreateAssetMenu(menuName = "Galaxy Data/Database")]
    public class GalaxyDatabaseAsset : ScriptableObject
    {
        public GalaxySettingsAsset settings;
        public GalaxyIconsAsset icons;
        public GalaxyBaseNamesAsset baseNames;
        public GalaxyPropertiesAsset properties;
        public GalaxySlotsAsset slots;
        public GalaxyResourceCategoriesAsset resourceCategories;
        public GalaxyEquipmentCategoriesAsset equipmentCategories;
        public GalaxyShipCategoriesAsset shipCategories;
        public GalaxyShipsAsset ships;
        public GalaxyResourcesAsset resources;
        public GalaxyEquipmentsAsset equipments;
    }
}
