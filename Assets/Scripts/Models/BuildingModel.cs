using UnityEngine;

namespace Models
{
    [System.Serializable]
    public class BuildingModel
    {
        public string ResourceName { get; private set; }
        public int ResourceAmount { get; private set; }

        public BuildingModel(string resourceName)
        {
            ResourceName = resourceName;
            ResourceAmount = 0;
        }

        public void Produce(int amount, int max)
        {
            ResourceAmount = Mathf.Min(ResourceAmount + amount, max);
        }

        public int Collect(int requested)
        {
            int collected = Mathf.Min(requested, ResourceAmount);
            ResourceAmount -= collected;
            return collected;
        }

        public bool HasResources => ResourceAmount > 0;
    }
}