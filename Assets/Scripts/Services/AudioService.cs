using UnityEngine;
using SO;
using UniRx;

namespace Services
{
    public class AudioService
    {
        private readonly SettingsSo _settings;

        public AudioService(SettingsSo settings)
        {
            _settings = settings;
            Observable.EveryUpdate()
                .Subscribe(_ => AudioListener.volume = _settings.volume)
                .AddTo(GameObject.FindObjectOfType<AudioListener>()); // автоматически очистит подписку при уничтожении
        }
    }
}