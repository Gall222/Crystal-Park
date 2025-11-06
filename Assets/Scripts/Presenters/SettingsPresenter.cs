// Presenters/SettingsPresenter.cs
using Services;
using Views;
using SO;
using UniRx;

namespace Presenters
{
    public class SettingsPresenter
    {
        private readonly SettingsPanelView _view;
        private readonly SettingsSo _settings;

        public SettingsPresenter(SettingsPanelView view, SettingsSo settings)
        {
            _view = view;
            _settings = settings;

            _view.Initialize();

            // Устанавливаем слайдер на текущее значение
            _view.SetSliderValue(_settings.volume);

            // Изменяем значение в SO при движении слайдера
            _view.OnSliderChanged()
                .Subscribe(val => _settings.volume = val)
                .AddTo(_view);
        }

        public void ShowPanel() => _view.Show();
        public void HidePanel() => _view.Hide();
    }
}