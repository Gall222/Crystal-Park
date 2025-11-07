using System.Collections.Generic;

[System.Serializable]
public class ResourceData
{
    public string resourceName;
    public int amount;
}

public class ResourceService
{
    private readonly Dictionary<string, int> _resources = new();
    public IReadOnlyDictionary<string, int> Resources => _resources;

    public void Add(string name, int amount)
    {
        if (!_resources.ContainsKey(name))
            _resources[name] = 0;

        _resources[name] += amount;
    }

    public int Get(string name) => _resources.ContainsKey(name) ? _resources[name] : 0;
    public void Set(string name, int amount) => _resources[name] = amount;
    public void Clear() => _resources.Clear();
}
