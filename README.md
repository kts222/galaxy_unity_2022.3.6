# Galaxy Data Unity Importer

Мини-проект/пакет для Unity Editor. Импортирует **только данные** из `galaxy-data.json` в ScriptableObject assets.

## Использование
1. Откройте проект в Unity 2022.3 LTS или новее.
2. Поместите исходный JSON-файл в папку `Assets` проекта (например, `Assets/Import/galaxy-data.json`).
3. Откройте `Tools → Galaxy Data → JSON Importer`.
4. Выберите JSON в поле `JSON file`.
5. Нажмите `Import JSON → ScriptableObjects`.
6. Результат появится в `Assets/GalaxyData/`, главный asset — `GalaxyDatabase.asset`.

Повторный импорт обновляет уже созданные assets с теми же именами, а не создаёт новые копии.

## Создаваемые assets
- Settings
- Icons
- BaseNames
- Properties
- Slots
- ResourceCategories
- EquipmentCategories
- ShipCategories
- Ships
- Resources
- Equipments
- GalaxyDatabase (ссылки на все перечисленные assets)

## Использование из игрового кода
```csharp
using GalaxyDataImporter;
using UnityEngine;

public class Example : MonoBehaviour
{
    [SerializeField] private GalaxyDatabaseAsset database;

    private void Start()
    {
        foreach (var ship in database.ships.entries)
            Debug.Log($"{ship.name}: buy={ship.price_buy}");
    }
}
```

## Важно
- Импортёр использует пакет Unity `com.unity.nuget.newtonsoft-json` для корректного чтения словаря `icons`, nullable `slot` и поля `prop2`, которое в исходном JSON бывает числом или массивом.
- `prop2` сохранён как `JToken`-совместимое значение? Для строгого использования рекомендуется смотреть исходное поле `prop2` как `object`; JSON.NET сохранит фактический тип значения при десериализации.
- Импортёр не создаёт UI, сцены, префабы, игровые механики или инвентарь.
