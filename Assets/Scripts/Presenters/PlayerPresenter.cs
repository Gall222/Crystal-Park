using System;
using System.Collections;
using UniRx;
using Views;
using Services;
using Zenject;

public class PlayerPresenter : ITickable
{
    private readonly PlayerView _view;
    private readonly InputService _input;
    public PlayerModel Model { get; }

    private readonly Subject<BuildingView> _onBuildingReached = new();
    public IObservable<BuildingView> OnBuildingReached => _onBuildingReached;

    public PlayerPresenter(PlayerView view, InputService input, PlayerModel model)
    {
        _view = view;
        _input = input;
        Model = model;
        
        _input.OnClick.Subscribe(OnClick).AddTo(_view);
        
        _view.StartCoroutine(InitializeModelNextFrame());
    }

    private IEnumerator InitializeModelNextFrame()
    {
        yield return null;
        Model.Initialize(_view.Agent, _view.transform.position);
    }

    private void OnClick(ClickData data)
    {
        if (Model.IsCollecting)
            Model.StopMoving();

        Model.MoveTo(data.TargetPoint);

        if (data.Building != null)
            _view.StartCoroutine(WaitAndNotifyBuildingReached(data.Building));
    }

    private IEnumerator WaitAndNotifyBuildingReached(BuildingView building)
    {
        while (!Model.HasReached(building.CollectPoint.position))
            yield return null;

        Model.StopAndCollect();
        _view.PlayCollectingAnimation(true);
        _onBuildingReached.OnNext(building);
    }

    public void Tick()
    {
        Model.Update();
        _view.PlayCollectingAnimation(Model.IsCollecting);
    }
}