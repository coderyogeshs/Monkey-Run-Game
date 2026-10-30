using UnityEngine;

// moves the monkey picture with the joystick and keeps it on screen
public class MonkeyMover : MonoBehaviour
{
    [SerializeField] private VirtualJoystick _joystick;
    [SerializeField] private RectTransform _monkey;
    [SerializeField] private float _speed = 600f;   // canvas units per second

    // every new game starts with the monkey in the middle
    private void OnEnable() => _monkey.anchoredPosition = Vector2.zero;

    private void Update()
    {
        RectTransform bounds = (RectTransform)_monkey.parent;
        Vector2 limit = (bounds.rect.size - _monkey.rect.size) * 0.5f;

        // Time.deltaTime is 0 while paused, so the monkey stops by itself
        Vector2 pos = _monkey.anchoredPosition + _joystick.Direction * (_speed * Time.deltaTime);
        pos.x = Mathf.Clamp(pos.x, -limit.x, limit.x);
        pos.y = Mathf.Clamp(pos.y, -limit.y, limit.y);

        _monkey.anchoredPosition = pos;
    }
}