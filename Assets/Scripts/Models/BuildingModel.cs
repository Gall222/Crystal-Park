using UnityEngine;

namespace Models
{
    [System.Serializable]
    public class BuildingModel
    {
        public string ResourceName { get; private set; }
        public int ResourceAmount { get; set; }
        public int MaxCapacity { get; private set; }

        public BuildingModel(string name, int maxCapacity)
        {
            ResourceName = name;
            MaxCapacity = maxCapacity;
            ResourceAmount = 0;
        }

        public void Produce(int amount)
        {
            ResourceAmount = Mathf.Min(ResourceAmount + amount, MaxCapacity);
        }

        public int Collect(int requested)
        {
            int collected = Mathf.Min(requested, ResourceAmount);
            ResourceAmount -= collected;
            return collected;
        }
    }
}