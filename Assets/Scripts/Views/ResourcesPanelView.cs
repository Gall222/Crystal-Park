using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UniRx;
using UI;

namespace Views
{
    public class ResourcesPanelView : MonoBehaviour
    {
        private string _show = "Show";
        private string _hide = "Hide";
        
        [Header("UI Elements")]
        [SerializeField] private TMP_Text[] resourceTexts;
        [SerializeField] private Animator animator;

        private Dictionary<string, TMP_Text> _labels = new();
        private CompositeDisposable _disposables = new();
        
        public void Initialize(UIEventBus eventBus)
        {
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

        public void Show() => animator.SetTrigger(_show);
        public void Hide() => animator.SetTrigger(_hide);

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}