using System;
using UI;
using UnityEngine;
using UniRx;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Views;

namespace Services
{
    public class InputService : IDisposable
    {
        public readonly Subject<ClickData> OnClick = new();
        public readonly Subject<Vector2> OnDrag = new();
        public readonly Subject<Unit> OnDragStart = new();
        public readonly Subject<Unit> OnDragEnd = new();

        private PlayerInputSystem _input;
        private bool _isDragging = false;
        private Vector2 _lastPosition;

        public InputService(PlayerInputSystem input)
        {
            _input = input ?? throw new ArgumentNullException(nameof(input));
            _input.Enable();

            // Click
            _input.UI.Click.performed += ctx => TryClick(ctx);

            // Drag
            _input.UI.Drag.started += ctx => StartDrag(ctx);
            _input.UI.Drag.performed += ctx => DoDrag(ctx);
            _input.UI.Drag.canceled += ctx => EndDrag(ctx);
        }

        private void TryClick(InputAction.CallbackContext ctx)
        {
            Vector2 screenPos = Vector2.zero;

            if (Mouse.current != null)
                screenPos = Mouse.current.position.ReadValue();
            else if (Touchscreen.current != null && Touchscreen.current.touches.Count > 0)
                screenPos = Touchscreen.current.touches[0].position.ReadValue();
            else
                return;

            // Check UI by RaycastAll
            if (EventSystem.current != null)
            {
                var pointer = new PointerEventData(EventSystem.current)
                {
                    position = screenPos
                };

                var results = new System.Collections.Generic.List<RaycastResult>();
                EventSystem.current.RaycastAll(pointer, results);

                if (results.Count > 0)
                {
                    // UI clicked, ignore plauer move
                    return;
                }
            }

            // Player move click
            Ray ray = Camera.main.ScreenPointToRay(screenPos);
            if (!Physics.Raycast(ray, out RaycastHit hit)) return;

            var building = hit.collider.GetComponent<BuildingView>();
            var data = new ClickData
            {
                Building = building,
                TargetPoint = building != null ? building.CollectPoint.position : hit.point
            };

            OnClick.OnNext(data);
        }

        private void StartDrag(InputAction.CallbackContext ctx)
        {
            _isDragging = true;
            _lastPosition = ctx.ReadValue<Vector2>();
            OnDragStart.OnNext(Unit.Default);
        }

        private void DoDrag(InputAction.CallbackContext ctx)
        {
            Vector2 current = ctx.ReadValue<Vector2>();
            Vector2 delta = current - _lastPosition;
            _lastPosition = current;
            OnDrag.OnNext(delta);
        }

        private void EndDrag(InputAction.CallbackContext ctx)
        {
            _isDragging = false;
            OnDragEnd.OnNext(Unit.Default);
        }

        public void Dispose()
        {
            OnClick.Dispose();
            OnDrag.Dispose();
            OnDragStart.Dispose();
            OnDragEnd.Dispose();
            _input.Disable();
        }
    }
}