using UnityEngine;
using UnityEngine.UI;

public class SlideMenu : MonoBehaviour
{
    [SerializeField] private RectTransform _panel;        // the panel that slides in from the right
    [SerializeField] private RectTransform[] _buttons;    // the buttons inside it, they slide in one by one
    [SerializeField] private RectTransform _menuIcon;     // the menu icon, it spins as the menu opens
    [SerializeField] private Image _dimmer;               // full-screen dark layer behind the panel
    [SerializeField] private float _duration = 0.4f;
    [SerializeField] private float _stagger = 0.2f;       // gap between buttons (0 to 1 of the animation)
    [SerializeField] private float _shownX = -20f;        // where the panel rests when it is open

    private float _progress;   // 0 = closed, 1 = fully open
    private bool _isOpen;

    // extra 200 so the panel stays hidden even behind a phone notch
    private float HiddenX => _panel.rect.width + 200f;

    // every time the Main Menu is shown, the slide menu starts closed
    private void OnEnable()
    {
        _isOpen = false;
        _progress = 0f;
        Apply();
    }

    public void Toggle() => _isOpen = !_isOpen;
    public void Close() => _isOpen = false;

    private void Update()
    {
        float target = _isOpen ? 1f : 0f;
        if (_progress == target) return;   // nothing to animate

        // unscaled time, so it also works while the game is paused
        _progress = Mathf.MoveTowards(_progress, target, Time.unscaledDeltaTime / _duration);
        Apply();
    }

    private void Apply()
    {
        float eased = EaseOut(_progress);

        _panel.anchoredPosition = new Vector2(Mathf.Lerp(HiddenX, _shownX, eased), _panel.anchoredPosition.y);
        _menuIcon.localRotation = Quaternion.Euler(0f, 0f, -90f * eased);

        _dimmer.gameObject.SetActive(_progress > 0f);
        _dimmer.color = new Color(0f, 0f, 0f, 0.55f * eased);

        // each button starts a little later than the one before it
        float window = Mathf.Max(0.2f, 1f - _stagger * (_buttons.Length - 1));

        for (int i = 0; i < _buttons.Length; i++)
        {
            float local = Mathf.Clamp01((_progress - i * _stagger) / window);
            float x = Mathf.Lerp(350f, 0f, EaseOut(local));
            _buttons[i].anchoredPosition = new Vector2(x, _buttons[i].anchoredPosition.y);
        }
    }

    // fast start, soft landing
    private static float EaseOut(float t) => 1f - (1f - t) * (1f - t) * (1f - t);
}