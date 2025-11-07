using UnityEngine;
using UI;
using UniRx;
using Unity.Cinemachine;
using Zenject;

namespace Presenters
{
    public class CameraPresenter
    {
        private readonly CinemachineCamera _vcam;
        private readonly Transform _playerTransform;
        private readonly ScreenMoveButtonView _screenMoveButton;

        private bool _isDragging;
        private Vector2 _lastPointerPos;
        private Transform _originalFollow;

        [Inject]
        public CameraPresenter(CinemachineCamera vcam,
                               Transform playerTransform,
                               ScreenMoveButtonView screenMoveButton)
        {
            _vcam = vcam;
            _playerTransform = playerTransform;
            _screenMoveButton = screenMoveButton;

            if (_vcam.Follow != null)
                _originalFollow = _vcam.Follow;
            
            //Screen move button subscribe
            _screenMoveButton.OnPointerDownObservable.Subscribe(OnPointerDown).AddTo(_vcam.gameObject);
            _screenMoveButton.OnPointerUpObservable.Subscribe(_ => OnPointerUp()).AddTo(_vcam.gameObject);

            Observable.EveryUpdate().Subscribe(_ => Tick()).AddTo(_vcam.gameObject);
        }

        private void OnPointerDown(Vector2 pointerPos)
        {
            _isDragging = true;
            _lastPointerPos = pointerPos;
            _vcam.Follow = null;
        }

        private void OnPointerUp()
        {
            _isDragging = false;
            _vcam.Follow = _originalFollow;
        }

        private void Tick()
        {
            if (_isDragging)
            {
                Vector2 currentPos = Vector2.zero;
                if (UnityEngine.Input.mousePresent)
                    currentPos = UnityEngine.Input.mousePosition;
                else if (UnityEngine.Input.touchCount > 0)
                    currentPos = UnityEngine.Input.GetTouch(0).position;

                Vector2 delta = currentPos - _lastPointerPos;
                _lastPointerPos = currentPos;

                Vector3 deltaMove = new Vector3(-delta.x, 0, -delta.y) * 0.1f;
                _vcam.transform.position += deltaMove;
            }
            else if (_vcam.Follow != null)
            {
                var pos = _vcam.transform.position;
                _vcam.transform.position = Vector3.Lerp(pos,
                    new Vector3(_playerTransform.position.x, pos.y, _playerTransform.position.z),
                    Time.deltaTime * 5f);
            }
        }
    }
}
