using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UniRx;
using UI;

namespace Views
{
    public class ResourcesPanelView : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TMP_Text[] resourceTexts; // 5 штук, один для каждого ресурса
        [SerializeField] private Animator animator; // для анимации показа/скрытия панели

        private Dictionary<string, TMP_Text> _labels = new();
        private CompositeDisposable _disposables = new();

        public void Initialize(UIEventBus eventBus)
        {
            // Подписываемся на события
            eventBus.OnPopupShow.Subscribe(_ => Show()).AddTo(_disposables);
            eventBus.OnPopupHide.Subscribe(_ => Hide()).AddTo(_disposables);
            eventBus.OnResourceChanged.Subscribe(data => UpdateUI(data.resources)).AddTo(_disposables);
        }

        private void Awake()
        {
            foreach (var txt in resourceTexts)
            {
                if (!_labels.ContainsKey(txt.name))
                    _labels[txt.name] = txt;
            }
        }

        private void UpdateUI(Dictionary<string, int> resources)
        {
            foreach (var kvp in resources)
            {
                if (_labels.TryGetValue(kvp.Key, out var txt))
                    txt.text = kvp.Value.ToString();
            }
        }

        public void Show() => animator.SetTrigger("Show");
        public void Hide() => animator.SetTrigger("Hide");

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}