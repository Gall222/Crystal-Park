using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    [System.Serializable]
    public class PlayerModel
    {
        public Dictionary<string, int> Resources { get; private set; } = new();

        public bool TryAddResource(string name, int amount, int maxPerType)
        {
            if (!Resources.ContainsKey(name))
                Resources[name] = 0;

            if (Resources[name] >= maxPerType)
                return false;

            Resources[name] = Mathf.Min(Resources[name] + amount, maxPerType);
            return true;
        }
    }
}