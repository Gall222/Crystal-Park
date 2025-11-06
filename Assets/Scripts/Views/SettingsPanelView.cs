// Views/SettingsPanelView.cs

using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Views
{
    public class SettingsPanelView : MonoBehaviour
    {
        [Header("UI Elements")]
        public GameObject panel;
        public Button closeButton;
        public Slider volumeSlider;

        private readonly CompositeDisposable _disposables = new();

        public void Initialize()
        {
            closeButton.onClick.AsObservable()
                .Subscribe(_ => Hide())
                .AddTo(_disposables);
        }

        public void Show() => panel.SetActive(true);
        public void Hide() => panel.SetActive(false);

        public void SetSliderValue(float value) => volumeSlider.SetValueWithoutNotify(value);
        public IObservable<float> OnSliderChanged() => volumeSlider.OnValueChangedAsObservable();

        private void OnDestroy() => _disposables.Dispose();
    }
}