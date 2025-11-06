using System.Collections;
using UniRx;
using UnityEngine;
using Services;
using Models;
using Views;
using System;

namespace Presenters
{
    public class PlayerPresenter
    {
        private readonly PlayerView _view;
        private readonly InputService _inputService;

        public PlayerModel Model { get; }

        private readonly Subject<BuildingView> _onBuildingReached = new();
        public IObservable<BuildingView> OnBuildingReached => _onBuildingReached;

        public PlayerPresenter(PlayerView view, InputService inputService, PlayerModel model)
        {
            _view = view;
            _inputService = inputService;
            Model = model;

            // Подписка на клики
            _inputService.OnClick.Subscribe(OnClick).AddTo(_view);
        }

        private void OnClick(ClickData data)
        {
            if (_view.IsCollecting)
                _view.StopMoving();

            _view.MoveTo(data.TargetPoint);

            if (data.Building != null)
                _view.StartCoroutine(WaitAndNotifyBuildingReached(data.Building));
        }

        private IEnumerator WaitAndNotifyBuildingReached(BuildingView building)
        {
            while (!_view.HasReached(building.CollectPoint.position))
                yield return null;

            _view.StopAndCollect();
            _onBuildingReached.OnNext(building);
        }
    }
}