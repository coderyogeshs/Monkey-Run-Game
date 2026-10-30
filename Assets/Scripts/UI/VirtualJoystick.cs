using UnityEngine;
using UnityEngine.EventSystems;

// goes on JoystickArea; moves JoystickHandle and tells other scripts the direction
public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform _area;      // JoystickArea (the big ring)
    [SerializeField] private RectTransform _handle;    // JoystickHandle (the ball)
    [SerializeField, Range(0.2f, 1f)] private float _handleRange = 0.5f;   // how far the ball can travel
    [SerializeField, Range(0f, 0.5f)] private float _deadZone = 0.1f;      // tiny movements are ignored

    // other scripts read this: x and y from -1 to 1
    public Vector2 Direction { get; private set; }

    public void OnPointerDown(PointerEventData eventData) => Move(eventData);
    public void OnDrag(PointerEventData eventData) => Move(eventData);
    public void OnPointerUp(PointerEventData eventData) => ResetStick();

    // if the screen is switched off (pause, menu), the stick must not stay stuck
    private void OnDisable() => ResetStick();

    private void Move(PointerEventData eventData)
    {
        // screen position -> position inside the ring
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _area, eventData.position, eventData.pressEventCamera, out Vector2 local);

        float radius = _area.rect.width * 0.5f * _handleRange;
        Vector2 direction = Vector2.ClampMagnitude((local - _area.rect.center) / radius, 1f);

        Direction = direction.magnitude < _deadZone ? Vector2.zero : direction;
        _handle.anchoredPosition = direction * radius;   // the ball follows the finger
    }

    // the ball springs back to the centre
    private void ResetStick()
    {
        Direction = Vector2.zero;
        _handle.anchoredPosition = Vector2.zero;
    }
}