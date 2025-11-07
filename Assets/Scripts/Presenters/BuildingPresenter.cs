using System.Collections;
using TMPro;
using UnityEngine;
using Views;

public class BuildingPresenter
{
    public BuildingData Data { get; private set; }
    public BuildingView View { get; private set; }

    private TMP_Text _floatingText;
    private Coroutine _produceRoutine;
    private MonoBehaviour _coroutineOwner;

    private float _productionInterval = 3f;
    private int _productionPerCycle = 1;

    public BuildingPresenter(BuildingData data, BuildingView view, MonoBehaviour coroutineOwner)
    {
        Data = data;
        View = view;
        _coroutineOwner = coroutineOwner;

        _floatingText = view.GetComponentInChildren<TMP_Text>();
        UpdateFloatingText();
    }

    public void StartProduction()
    {
        if (_coroutineOwner != null)
        {
            if (_produceRoutine != null)
                _coroutineOwner.StopCoroutine(_produceRoutine);

            _produceRoutine = _coroutineOwner.StartCoroutine(ProduceCoroutine());
        }
    }

    private IEnumerator ProduceCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_productionInterval);
            Data.resourceAmount = Mathf.Min(Data.resourceAmount + _productionPerCycle, Data.maxCapacity);
            UpdateFloatingText();
        }
    }

    public int Collect(int amount = 1)
    {
        if (Data.resourceAmount <= 0) return 0;
        int collected = Mathf.Min(amount, Data.resourceAmount);
        Data.resourceAmount -= collected;
        UpdateFloatingText();
        return collected;
    }

    private void UpdateFloatingText()
    {
        if (_floatingText != null)
            _floatingText.text = Data.resourceAmount.ToString();
    }
    
    public void SetResourceAmount(int amount)
    {
        Data.resourceAmount = Mathf.Clamp(amount, 0, Data.maxCapacity);
    }

    public void RefreshView()
    {
        if (_floatingText != null)
            _floatingText.text = ResourceAmount.ToString();
    }

    public string Name => Data.buildingName;
    public int ResourceAmount => Data.resourceAmount;
}