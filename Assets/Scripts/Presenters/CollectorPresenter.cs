using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UI;
using Views;
using SO;

public class CollectorPresenter
{
    private readonly PlayerPresenter _player;
    private readonly List<BuildingPresenter> _buildings;
    private readonly UIEventBus _ui;
    private readonly SettingsSo _settings;
    private readonly PlayerView _playerView;

    private Coroutine _collectRoutine;
    private BuildingPresenter _currentBuilding;

    public CollectorPresenter(PlayerPresenter player,
                              List<BuildingPresenter> buildings,
                              UIEventBus ui,
                              SettingsSo settings,
                              PlayerView playerView)
    {
        _player = player;
        _buildings = buildings;
        _ui = ui;
        _settings = settings;
        _playerView = playerView;

        _player.OnBuildingReached
               .Subscribe(OnBuildingReached)
               .AddTo(_playerView.gameObject);
    }

    private void OnBuildingReached(BuildingView buildingView)
    {
        _currentBuilding = _buildings.Find(b => b.View == buildingView);
        if (_currentBuilding == null) return;

        StartCollecting(_currentBuilding);
    }

    private void StartCollecting(BuildingPresenter building)
    {
        StopCollecting();
        _collectRoutine = _playerView.StartCoroutine(CollectRoutine(building));
    }

    private IEnumerator CollectRoutine(BuildingPresenter building)
    {
        // Show res panel
        _ui.OnPopupShow.OnNext(Unit.Default);

        string resourceName = building.Data.buildingName;

        while (_playerView.IsCollecting)
        {
            if (building.Data.resourceAmount > 0 &&
                _player.Model.TryAddResource(resourceName, 1, _settings.maxResourcePerType))
            {
                building.Collect();
                _ui.OnResourceChanged.OnNext((_player.Model.Resources, resourceName));
            }

            yield return new WaitForSeconds(_settings.resourceAddDelay);
        }

        // Hide panel
        _ui.OnPopupHide.OnNext(Unit.Default);
        _collectRoutine = null;
    }

    private void StopCollecting()
    {
        if (_collectRoutine != null)
        {
            _playerView.StopCoroutine(_collectRoutine);
            _collectRoutine = null;
        }
    }
}
