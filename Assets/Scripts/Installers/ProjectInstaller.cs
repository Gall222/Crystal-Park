using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UniRx;
using Views;
using Services;
using Presenters;
using SO;
using UI;
using Unity.Cinemachine;

namespace Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [Header("Scene References")]
        public SettingsPanelView settingsPanelView;
        public Button settingsButton;
        public PlayerView playerView;
        public ResourcesPanelView resourcesPanelView;
        public CinemachineCamera vcam;
        public Transform playerTransform;
        public List<BuildingView> buildingViews;
        public ScreenMoveButtonView screenMoveButton;

        [Header("ScriptableObjects")]
        public SettingsSo settings;
        public SaveData saveData;

        public override void InstallBindings()
        {
            // --- ScriptableObjects ---
            Container.BindInstance(settings).AsSingle();
            Container.BindInstance(saveData).AsSingle();

            // --- Core Services ---
            Container.Bind<ResourceService>().AsSingle().NonLazy();
            Container.Bind<SaveService>().AsSingle().NonLazy();
            Container.Bind<AudioService>().AsSingle().NonLazy();
            Container.Bind<UIEventBus>().AsSingle().NonLazy();

            // --- Input ---
            var playerInput = new PlayerInputSystem();
            Container.Bind<InputService>().AsSingle().WithArguments(playerInput).NonLazy();

            // --- Views ---
            if (playerView == null || resourcesPanelView == null || settingsPanelView == null ||
                screenMoveButton == null || playerTransform == null || vcam == null)
            {
                Debug.LogError("ProjectInstaller: One or more scene references are not assigned!");
            }

            Container.BindInstance(playerView);
            Container.BindInstance(resourcesPanelView);
            Container.BindInstance(settingsPanelView);
            Container.BindInstance(screenMoveButton);
            Container.BindInstance(playerTransform);
            Container.BindInstance(vcam);

            // --- Models ---
            var playerModel = new PlayerModel();
            Container.BindInstance(playerModel).AsSingle();

            // --- Create Buildings ---
            var buildingPresenters = CreateBuildingPresenters();
            Container.BindInstance(buildingPresenters).AsSingle().NonLazy();

            // --- Presenters ---
            Container.BindInterfacesAndSelfTo<PlayerPresenter>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<CollectorPresenter>().AsSingle().NonLazy();
            Container.Bind<CameraPresenter>().AsSingle().NonLazy();
            Container.Bind<SettingsPresenter>().AsSingle().WithArguments(settingsPanelView, settings).NonLazy();

            // --- Initialize UI ---
            var uiBus = Container.Resolve<UIEventBus>();
            resourcesPanelView.Initialize(uiBus);

            // --- Settings button ---
            settingsButton.onClick.AsObservable()
                .Subscribe(_ => Container.Resolve<SettingsPresenter>().ShowPanel())
                .AddTo(settingsButton.gameObject);

            // --- Load / Reset Save ---
            var saveService = Container.Resolve<SaveService>();
            bool hasAnySave = saveData.PlayerResources.Count > 0 || saveData.BuildingResources.Count > 0;

            if (hasAnySave)
            {
                Debug.Log("Loading existing save...");
                saveService.Load(playerModel, buildingPresenters);
            }
            else
            {
                Debug.Log("No save found. Creating new data...");
                saveService.ResetAll(playerModel, buildingPresenters);
            }

            // --- Start production after loading ---
            foreach (var building in buildingPresenters)
                building.StartProduction();
        }

        private List<BuildingPresenter> CreateBuildingPresenters()
        {
            var list = new List<BuildingPresenter>();
            foreach (var view in buildingViews)
            {
                var data = ScriptableObject.CreateInstance<BuildingData>();
                data.buildingName = view.ResourceName;
                data.resourceAmount = 0;
                data.maxCapacity = settings.maxCapacity;

                var presenter = new BuildingPresenter(data, view, this);
                list.Add(presenter);
            }
            return list;
        }
    }
}
