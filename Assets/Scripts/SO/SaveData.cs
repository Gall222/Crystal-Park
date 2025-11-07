using System;
using System.Collections.Generic;
using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "SaveData", menuName = "Game/Save Data")]
    public class SaveData : ScriptableObject
    {
        [Serializable]
        public class BuildingSave
        {
            public string buildingName;
            public int resourceAmount;
        }

        [Serializable]
        public class PlayerSave
        {
            public string resourceName;
            public int amount;
        }

        public List<BuildingSave> BuildingResources = new();
        public List<PlayerSave> PlayerResources = new();
    }
}