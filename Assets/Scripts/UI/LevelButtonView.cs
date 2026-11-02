using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// ONE level button: number, lock icon or stars. It only shows data and reports the click.
public class LevelButtonView : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _numberText;
    [SerializeField] private GameObject _lockIcon;
    [SerializeField] private GameObject _starsGroup;
private Image[] _starImages;
    [SerializeField] private Color _starEarnedColor = new Color(1f, 0.82f, 0.2f);
    [SerializeField] private Color _starEmptyColor = new Color(1f, 1f, 1f, 0.25f);

    private int _levelIndex;
    private Action<int> _onClicked;

private void Awake()
{
    _button.onClick.AddListener(OnButtonClicked);
    _starImages = _starsGroup.GetComponentsInChildren<Image>(true);   // finds the 3 stars itself
}
    private void OnDestroy() => _button.onClick.RemoveListener(OnButtonClicked);

    // called by LevelSelectScreen every time the screen opens
    public void Setup(int levelIndex, bool isUnlocked, int stars, Action<int> onClicked)
    {
        _levelIndex = levelIndex;
        _onClicked = onClicked;

        _numberText.text = (levelIndex + 1).ToString();   // players count from 1
        _numberText.gameObject.SetActive(isUnlocked);
        _lockIcon.SetActive(!isUnlocked);
        _starsGroup.SetActive(isUnlocked);
        _button.interactable = isUnlocked;                // locked levels can't be clicked

        for (int i = 0; i < _starImages.Length; i++)
            _starImages[i].color = i < stars ? _starEarnedColor : _starEmptyColor;
    }

    private void OnButtonClicked() => _onClicked?.Invoke(_levelIndex);
}
