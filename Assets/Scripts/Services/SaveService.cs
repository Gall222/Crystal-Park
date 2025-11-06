using System;
using System.Collections.Generic;
using Presenters;
using SO;
using UnityEngine;

namespace Services
{
    [Serializable]
    public class SaveEntry
    {
        public string name;
        public int amount;
    }

    [Serializable]
    public class SaveData
    {
        public List<SaveEntry> playerResources = new();
        public List<SaveEntry> buildingResources = new(); // <-- добавлено
        public float masterVolume;
    }

    public class SaveService
    {
        private const string SAVE_KEY = "game_save_v1";

        public void Save(ResourceService resourceService, SettingsSo settings, List<BuildingPresenter> buildings)
        {
            var data = new SaveData();

            // Игрок
            foreach (var res in resourceService.Resources)
                data.playerResources.Add(new SaveEntry { name = res.resourceName, amount = res.amount });

            // Здания
            foreach (var building in buildings)
                data.buildingResources.Add(new SaveEntry { name = building.Model.ResourceName, amount = building.Model.ResourceAmount });

            data.masterVolume = settings.volume;

            PlayerPrefs.SetString(SAVE_KEY, JsonUtility.ToJson(data));
            PlayerPrefs.Save();
            Debug.Log("Game saved");
        }


        public void Load(ResourceService resourceService, SettingsSo settings, List<BuildingPresenter> buildings)
        {
            if (!PlayerPrefs.HasKey(SAVE_KEY)) return;

            var json = PlayerPrefs.GetString(SAVE_KEY);
            var data = JsonUtility.FromJson<SaveData>(json);

            // Игрок
            resourceService.ResetAll();
            foreach (var e in data.playerResources)
                resourceService.AddResource(e.name, e.amount);

            // Здания
            foreach (var e in data.buildingResources)
            {
                var building = buildings.Find(b => b.Model.ResourceName == e.name);
                if (building != null)
                    building.Model.Produce(e.amount, settings.maxCapacity); // ставим текущее количество
            }

            settings.volume = data.masterVolume;
            Debug.Log("Game loaded");
        }

    }
}