using System.Collections.Generic;
using UniRx;
using System.Linq;

namespace Services
{
    [System.Serializable]
    public class ResourceData
    {
        public string resourceName;
        public int amount;
    }

    public class ResourceService
    {
        // ReactiveCollection позволяет подписываться на изменения
        public ReactiveCollection<ResourceData> Resources { get; } = new();

        public void AddResource(string name, int amount)
        {
            var existing = Resources.FirstOrDefault(r => r.resourceName == name);
            if (existing != null)
            {
                existing.amount += amount;
                Resources.ObserveReplace(); // вручную обновить реактивную подписку, если нужно
            }
            else
            {
                Resources.Add(new ResourceData { resourceName = name, amount = amount });
            }
        }

        public int GetResourceAmount(string name)
        {
            return Resources.FirstOrDefault(r => r.resourceName == name)?.amount ?? 0;
        }

        public void ResetAll()
        {
            Resources.Clear();
        }
    }
}