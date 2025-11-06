using UnityEngine;
using UnityEngine.InputSystem;
using Services;
using UI;
using Views;

public class InputHandler : MonoBehaviour
{
    private PlayerInputSystem _input;
    private InputService _inputService;

    public void Construct(InputService inputService, PlayerInputSystem playerInput)
    {
        _inputService = inputService;
        _input = playerInput;
        _input.Enable();

        _input.UI.Click.performed += OnClickPerformed;
    }

    private void OnClickPerformed(InputAction.CallbackContext ctx)
    {
        Vector2 screenPos = Vector2.zero;

        if (Mouse.current != null)
            screenPos = Mouse.current.position.ReadValue();
        else if (Touchscreen.current != null && Touchscreen.current.touches.Count > 0)
            screenPos = Touchscreen.current.touches[0].position.ReadValue();
        else
            return;

        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        if (!Physics.Raycast(ray, out RaycastHit hit)) return;

        var building = hit.collider.GetComponent<BuildingView>();
        var data = new ClickData
        {
            Building = building,
            TargetPoint = building != null ? building.CollectPoint.position : hit.point
        };

        _inputService.OnClick.OnNext(data);
    }
}