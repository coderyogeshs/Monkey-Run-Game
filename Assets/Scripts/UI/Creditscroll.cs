using UnityEngine;

// movie-style credits: the text rolls up from the bottom and then starts again
public class CreditsScroll : MonoBehaviour
{
    [SerializeField] private RectTransform _content;    // all the credit lines (pivot top)
    [SerializeField] private RectTransform _viewport;   // the clipped window (has a RectMask2D)
    [SerializeField] private float _speed = 70f;        // canvas units per second

    private void OnEnable() => Restart();

    private void Update()
    {
        _content.anchoredPosition += Vector2.up * (_speed * Time.unscaledDeltaTime);

        // the last line has left the top, so start again
        if (_content.anchoredPosition.y > _content.rect.height) Restart();
    }

    // puts the content just below the visible window
    private void Restart()
    {
        Canvas.ForceUpdateCanvases();
        _content.anchoredPosition = new Vector2(_content.anchoredPosition.x, -_viewport.rect.height);
    }
}