using System.Collections.Generic;
using UnityEngine;

// Level Select canvas: builds one button per level from the LevelList asset.
public class LevelSelectScreen : MonoBehaviour
{
    [SerializeField] private LevelList _levels;
    [SerializeField] private ScreenManager _screenManager;
    [SerializeField] private LevelButtonView _buttonPrefab;
    [SerializeField] private RectTransform _gridContent;

    private readonly List<LevelButtonView> _buttons = new List<LevelButtonView>();

    // runs every time this screen is switched on, so unlocks and stars are always fresh
    private void OnEnable() => RefreshButtons();

    private void RefreshButtons()
    {
        // create only the buttons that are missing (made once, reused after)
        while (_buttons.Count < _levels.Count)
            _buttons.Add(Instantiate(_buttonPrefab, _gridContent));

        for (int i = 0; i < _levels.Count; i++)
        {
            _buttons[i].Setup(i, PlayerProgress.IsUnlocked(i), PlayerProgress.GetStars(i), OnLevelClicked);
        }
    }

    private void OnLevelClicked(int levelIndex)
    {
        PlayerProgress.SelectedLevel = levelIndex;   // MiniGameManager reads this in Step 4
        _screenManager.ShowPlay();
    }
}
