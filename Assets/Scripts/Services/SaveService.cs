using System.Collections.Generic;
using UnityEngine;
using SO;

public class SaveService
{
    private readonly SaveData _saveData;

    public SaveService(SaveData saveData)
    {
        _saveData = saveData;
    }

    public void Save(PlayerModel player, List<BuildingPresenter> buildings)
    {
        _saveData.BuildingResources.Clear();
        _saveData.PlayerResources.Clear();

        // --- Player ---
        foreach (var kvp in player.Resources)
        {
            _saveData.PlayerResources.Add(new SaveData.PlayerSave
            {
                resourceName = kvp.Key,
                amount = kvp.Value
            });
        }

        // --- Buildings ---
        foreach (var building in buildings)
        {
            _saveData.BuildingResources.Add(new SaveData.BuildingSave
            {
                buildingName = building.Name,
                resourceAmount = building.ResourceAmount
            });
        }

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(_saveData);
        UnityEditor.AssetDatabase.SaveAssets();
#endif

        Debug.Log("✅ Game saved to ScriptableObject!");
    }

    public void Load(PlayerModel player, List<BuildingPresenter> buildings)
    {
        player.Resources.Clear();

        // --- Player ---
        foreach (var p in _saveData.PlayerResources)
        {
            player.Resources[p.resourceName] = p.amount;
            Debug.Log($"Loaded player resource: {p.resourceName} = {p.amount}");
        }

        // --- Buildings ---
        foreach (var b in _saveData.BuildingResources)
        {
            var building = buildings.Find(x => x.Name == b.buildingName);
            if (building != null)
            {
                building.SetResourceAmount(b.resourceAmount);
                building.RefreshView();
            }
        }

        Debug.Log("✅ Game loaded from ScriptableObject!");
    }

    public void ResetAll(PlayerModel player, List<BuildingPresenter> buildings)
    {
        player.Resources.Clear();
        _saveData.PlayerResources.Clear();
        _saveData.BuildingResources.Clear();

        foreach (var b in buildings)
        {
            b.SetResourceAmount(0);
            b.RefreshView();
        }

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(_saveData);
        UnityEditor.AssetDatabase.SaveAssets();
#endif

        Debug.Log("🔄 All data reset.");
    }
}
