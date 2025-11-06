using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;
using System;

namespace UI
{
    public class ScreenMoveButtonView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private readonly Subject<Vector2> _onPointerDown = new();
        private readonly Subject<Unit> _onPointerUp = new();

        public IObservable<Vector2> OnPointerDownObservable => _onPointerDown;
        public IObservable<Unit> OnPointerUpObservable => _onPointerUp;

        public void OnPointerDown(PointerEventData eventData)
        {
            _onPointerDown.OnNext(eventData.position);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _onPointerUp.OnNext(Unit.Default);
        }
    }
}