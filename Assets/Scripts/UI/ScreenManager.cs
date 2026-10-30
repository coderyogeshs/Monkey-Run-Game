using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ScreenManager : MonoBehaviour
{
    [Header("Loading Screen")]
    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private Image _loadingFill;
    [SerializeField] private TMP_Text _loadingPercentage;
    [SerializeField] private float _loadingDuration = 1.5f;

    [Header("Menu Screens")]
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _playMenu;
    [SerializeField] private GameObject _shopMenu;
    [SerializeField] private GameObject _leaderboardMenu;
    [SerializeField] private GameObject _creditsMenu;
    [SerializeField] private GameObject _settingsMenu;

    private GameObject _targetScreen;
    private Coroutine _loadingCoroutine;

    private GameObject _previousScreen;

    private void Start()
    {
        StartTransition(_mainMenu);
    }

    // Main menu navigation
    public void ShowMainMenu()
    {
        StartTransition(_mainMenu);
    }

    public void ShowPlay()
    {
        StartTransition(_playMenu);
    }

    public void ShowShop()
    {
        StartTransition(_shopMenu);
    }

    public void ShowLeaderboard()
    {
        StartTransition(_leaderboardMenu);
    }

    public void ShowCredits()
    {
        StartTransition(_creditsMenu);
    }

    // Settings navigation
    public void OpenSettings()
    {
        if (_loadingCoroutine != null)
            return;

        _previousScreen = GetCurrentScreen();

        StartTransition(_settingsMenu);
    }

    public void CloseSettings()
    {
        if (_previousScreen != null)
        {
            StartTransition(_previousScreen);
        }
        else
        {
            StartTransition(_mainMenu);
        }
    }

    // Close buttons
    public void CloseShop()
    {
        StartTransition(_mainMenu);
    }

    public void CloseLeaderboard()
    {
        StartTransition(_mainMenu);
    }

    public void CloseCredits()
    {
        StartTransition(_mainMenu);
    }

    // Identify currently active screen
    private GameObject GetCurrentScreen()
    {
        if (_mainMenu.activeSelf) return _mainMenu;
        if (_playMenu.activeSelf) return _playMenu;
        if (_shopMenu.activeSelf) return _shopMenu;
        if (_leaderboardMenu.activeSelf) return _leaderboardMenu;
        if (_creditsMenu.activeSelf) return _creditsMenu;

        return _mainMenu;
    }

    // Start loading transition
    private void StartTransition(GameObject screen)
    {
        if (_loadingCoroutine != null)
            return;

        if (screen == null)
        {
            Debug.LogError("ScreenManager: Target screen is not assigned!");
            return;
        }

        _targetScreen = screen;

        _loadingCoroutine = StartCoroutine(LoadAndShowScreen());
    }

    // Loading animation
    private IEnumerator LoadAndShowScreen()
    {
        _loadingScreen.SetActive(true);

        _loadingFill.fillAmount = 0f;

        float elapsed = 0f;

        while (elapsed < _loadingDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float progress = Mathf.Clamp01(
                elapsed / _loadingDuration
            );

            _loadingFill.fillAmount = progress;

            _loadingPercentage.text =
                Mathf.RoundToInt(progress * 100f) + "%";

            yield return null;
        }

        _loadingFill.fillAmount = 1f;
        _loadingPercentage.text = "100%";

        yield return new WaitForSecondsRealtime(0.3f);

        Show(_targetScreen);

        _loadingScreen.SetActive(false);

        _loadingCoroutine = null;
    }

    // Show selected screen and hide others
    private void Show(GameObject screen)
    {
        Time.timeScale = 1f;

        _mainMenu.SetActive(screen == _mainMenu);
        _playMenu.SetActive(screen == _playMenu);
        _shopMenu.SetActive(screen == _shopMenu);
        _leaderboardMenu.SetActive(screen == _leaderboardMenu);
        _creditsMenu.SetActive(screen == _creditsMenu);
        _settingsMenu.SetActive(screen == _settingsMenu);
    }

    // Quit game
    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}