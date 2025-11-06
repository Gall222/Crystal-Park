using System;
using System.Collections.Generic;
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
        public List<SaveEntry> resources = new List<SaveEntry>();
        public float masterVolume;
    }

    public class SaveService
    {
        private const string SAVE_KEY = "game_save_v1";

        public void Save(ResourceService resourceService, SettingsSo settings)
        {
            var data = new SaveData();
            foreach (var res in resourceService.Resources)
            {
                data.resources.Add(new SaveEntry { name = res.resourceName, amount = res.amount });
            }
            data.masterVolume = settings.volume;

            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(SAVE_KEY, json);
            PlayerPrefs.Save();
            Debug.Log("Game saved: " + json);
        }

        public void Load(ResourceService resourceService, SettingsSo settings)
        {
            if (!PlayerPrefs.HasKey(SAVE_KEY)) return;

            string json = PlayerPrefs.GetString(SAVE_KEY);
            try
            {
                var data = JsonUtility.FromJson<SaveData>(json);
                resourceService.ResetAll();
                foreach (var e in data.resources)
                {
                    resourceService.AddResource(e.name, e.amount);
                }

                settings.volume = data.masterVolume;
                Debug.Log("Game loaded: " + json);
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Failed to load save: " + ex);
            }
        }
    }
}