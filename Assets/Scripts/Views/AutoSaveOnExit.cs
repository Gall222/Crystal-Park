using UnityEngine;
using Zenject;
using System.Collections.Generic;

namespace Views
{
    public class AutoSaveOnExit : MonoBehaviour
    {
        private SaveService _saveService;
        private PlayerModel _playerModel;
        private List<BuildingPresenter> _buildings;

        private bool _isQuitting;

        [Inject]
        public void Construct(SaveService saveService, PlayerModel playerModel, List<BuildingPresenter> buildings)
        {
            _saveService = saveService;
            _playerModel = playerModel;
            _buildings = buildings;
        }

        private void OnApplicationQuit()
        {
            _isQuitting = true;
            TrySave("Quit");
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause && !_isQuitting)
                TrySave("Pause");
        }

        private void TrySave(string reason)
        {
            if (_saveService == null || _playerModel == null || _buildings == null)
            {
                Debug.LogWarning($"AutoSave failed on {reason}: one of dependencies is null");
                return;
            }

            Debug.Log($"Auto-saving on {reason}...");
            _saveService.Save(_playerModel, _buildings);
        }
    }
}