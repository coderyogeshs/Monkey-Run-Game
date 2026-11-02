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
    [SerializeField] private GameObject _levelSelectMenu;
    [SerializeField] private GameObject _playMenu;
    [SerializeField] private GameObject _shopMenu;
    [SerializeField] private GameObject _leaderboardMenu;
    [SerializeField] private GameObject _creditsMenu;
    [SerializeField] private GameObject _settingsMenu;

    private GameObject _targetScreen;
    private Coroutine _loadingCoroutine;

    private void Awake()
    {
        // The loading page must draw ABOVE every other screen (Settings uses Sort Order 10).
        // Without this, Shop / Leaderboard / Credits / Level Select hide the loading bar while they close.
        Canvas canvas = _loadingScreen.GetComponent<Canvas>();
        if (canvas == null) canvas = _loadingScreen.AddComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.sortingOrder = 100;

        // also blocks taps on the screen below while loading
        if (_loadingScreen.GetComponent<GraphicRaycaster>() == null)
            _loadingScreen.AddComponent<GraphicRaycaster>();
    }

    private void Start() => StartTransition(_mainMenu);

    // Menu navigation (each one plays the loading bar)
    public void ShowMainMenu() => StartTransition(_mainMenu);
    public void ShowLevelSelect() => StartTransition(_levelSelectMenu);
    public void ShowPlay() => StartTransition(_playMenu);
    public void ShowShop() => StartTransition(_shopMenu);
    public void ShowLeaderboard() => StartTransition(_leaderboardMenu);
    public void ShowCredits() => StartTransition(_creditsMenu);

    // Close buttons
    public void CloseShop() => StartTransition(_mainMenu);
    public void CloseLeaderboard() => StartTransition(_mainMenu);
    public void CloseCredits() => StartTransition(_mainMenu);

    // Settings opens ON TOP of the current screen, so the screen below is never switched off.
    // (If it were, closing Settings during a match would switch Play Menu back on and restart the match.)
    public void OpenSettings()
    {
        if (_loadingCoroutine != null) return;
        _settingsMenu.SetActive(true);
    }

    public void CloseSettings() => _settingsMenu.SetActive(false);

    // Start loading transition
    private void StartTransition(GameObject screen)
    {
        if (_loadingCoroutine != null) return;

        if (screen == null)
        {
            Debug.LogError("ScreenManager: Target screen is not assigned!");
            return;
        }

        _targetScreen = screen;
        _loadingCoroutine = StartCoroutine(LoadAndShowScreen());
    }

    // Loading animation (smooth ease-in-out, always ends exactly at 100%)
    private IEnumerator LoadAndShowScreen()
    {
        _loadingScreen.SetActive(true);
        SetProgress(0f);
        yield return null;   // let the loading page draw one frame before we start counting

        float elapsed = 0f;

        while (elapsed < _loadingDuration)
        {
            elapsed += Mathf.Min(Time.unscaledDeltaTime, 0.05f);   // one slow frame can't make the bar jump

            float t = Mathf.Clamp01(elapsed / _loadingDuration);
            SetProgress(Mathf.SmoothStep(0f, 1f, t));

            yield return null;
        }

        SetProgress(1f);   // always completely full
        yield return new WaitForSecondsRealtime(0.25f);

        Show(_targetScreen);

        _loadingScreen.SetActive(false);
        _loadingCoroutine = null;
    }

    private void SetProgress(float progress)
    {
        _loadingFill.fillAmount = progress;
        _loadingPercentage.text = Mathf.RoundToInt(progress * 100f) + "%";
    }

    // Show selected screen and hide the others (Settings is closed too)
    private void Show(GameObject screen)
    {
        Time.timeScale = 1f;

        _mainMenu.SetActive(screen == _mainMenu);
        _levelSelectMenu.SetActive(screen == _levelSelectMenu);
        _playMenu.SetActive(screen == _playMenu);
        _shopMenu.SetActive(screen == _shopMenu);
        _leaderboardMenu.SetActive(screen == _leaderboardMenu);
        _creditsMenu.SetActive(screen == _creditsMenu);
        _settingsMenu.SetActive(false);
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