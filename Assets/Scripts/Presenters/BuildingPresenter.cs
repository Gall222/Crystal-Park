using System.Collections;
using UnityEngine;
using Models;
using TMPro;
using SO;
using Views;

namespace Presenters
{
    public class BuildingPresenter
    {
        public BuildingModel Model { get; private set; }
        public BuildingView View { get; private set; }

        private SettingsSo _settings;
        private Coroutine _produceRoutine;
        private TMP_Text _floatingText;

        public BuildingPresenter(BuildingModel model, SettingsSo settings, BuildingView view)
        {
            Model = model;
            _settings = settings;
            View = view;

            // Получаем текст меш над зданием
            _floatingText = view.GetComponentInChildren<TMP_Text>();
            UpdateFloatingText();

            StartProduction();
        }

        private void StartProduction()
        {
            _produceRoutine = View.StartCoroutine(ProduceCoroutine());
        }

        private IEnumerator ProduceCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(_settings.productionInterval);

                Model.Produce(_settings.productionPerCycle, _settings.maxCapacity);
                UpdateFloatingText();
            }
        }

        // Постепенный сбор игроком (1 единица)
        public int Collect(int amount = 1)
        {
            if (Model.ResourceAmount <= 0)
                return 0;

            int collected = Mathf.Min(amount, Model.ResourceAmount);
            Model.Collect(collected);

            UpdateFloatingText();
            return collected;
        }

        private void UpdateFloatingText()
        {
            if (_floatingText != null)
                _floatingText.text = Model.ResourceAmount.ToString();
        }
    }
}