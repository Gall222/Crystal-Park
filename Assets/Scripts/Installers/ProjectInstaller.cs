using UnityEngine;
using Zenject;
using Services;
using Presenters;
using Views;
using SO;
using Models;
using UI;
using System.Collections.Generic;
using UniRx;
using Unity.Cinemachine;
using UnityEngine.UI;

namespace Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [Header("Scene References")]
        public SettingsSo settings;
        public SettingsPanelView settingsPanelView;
        public Button settingsButton;
        public PlayerView playerView;
        public ResourcesPanelView resourcesPanelView;
        public CinemachineCamera vcam;
        public Transform playerTransform;
        public List<BuildingView> buildingViews;
        public ScreenMoveButtonView screenMoveButton;

        public override void InstallBindings()
        {
            // ScriptableObjects
            Container.BindInstance(settings).AsSingle();

            // Core services
            Container.Bind<ResourceService>().AsSingle().NonLazy();
            Container.Bind<SaveService>().AsSingle().NonLazy();
            Container.Bind<AudioService>().AsSingle().NonLazy();
            Container.Bind<UIEventBus>().AsSingle().NonLazy();

            // Input
            Container.Bind<InputService>().AsSingle().WithArguments(new PlayerInputSystem()).NonLazy();

            // Views
            Container.BindInstance(playerView);
            Container.BindInstance(resourcesPanelView);
            Container.BindInstance(settingsPanelView);
            Container.BindInstance(screenMoveButton);
            Container.BindInstance(playerTransform);
            Container.BindInstance(vcam);

            // Models
            Container.Bind<PlayerModel>().AsSingle();
            var buildingModels = new List<BuildingModel>();
            foreach (var bv in buildingViews)
                buildingModels.Add(new BuildingModel(bv.ResourceName));
            Container.BindInstance(buildingModels).AsSingle();

            // Presenters
            Container.BindInterfacesAndSelfTo<PlayerPresenter>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<CollectorPresenter>().AsSingle().NonLazy();
            Container.BindInstance(CreateBuildingPresenters(buildingModels)).AsSingle().NonLazy();

            // Camera
            Container.Bind<CameraPresenter>().AsSingle().NonLazy();

            // Settings
            Container.Bind<SettingsPresenter>().AsSingle().WithArguments(settingsPanelView, settings).NonLazy();

            // Initialize UI
            var uiBus = Container.Resolve<UIEventBus>();
            resourcesPanelView.Initialize(uiBus);

            // Settings button
            settingsButton.onClick.AsObservable()
                .Subscribe(_ => Container.Resolve<SettingsPresenter>().ShowPanel())
                .AddTo(settingsButton.gameObject);
        }

        private List<BuildingPresenter> CreateBuildingPresenters(List<BuildingModel> models)
        {
            var list = new List<BuildingPresenter>();
            for (int i = 0; i < buildingViews.Count; i++)
            {
                var model = models[i];
                var view = buildingViews[i];
                list.Add(new BuildingPresenter(model, settings, view));
            }
            return list;
        }
    }
}
