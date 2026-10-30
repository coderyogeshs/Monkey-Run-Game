using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class ScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private Image _loadingFill;
    [SerializeField] private TMP_Text _loadingPercentage;

    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _playMenu;
    [SerializeField] private GameObject _shopMenu;
    [SerializeField] private GameObject _leaderboardMenu;
    [SerializeField] private GameObject _creditsMenu;
    [SerializeField] private GameObject _settingsMenu;

    [SerializeField] private float _loadingDuration = 3f;

    private void Start()
    {
        StartCoroutine(LoadGame());
    }

    private IEnumerator LoadGame()
    {
        // Show loading screen and hide other screens
        _loadingScreen.SetActive(true);
        _mainMenu.SetActive(false);
        _playMenu.SetActive(false);
        _shopMenu.SetActive(false);
        _leaderboardMenu.SetActive(false);
        _creditsMenu.SetActive(false);
        _settingsMenu.SetActive(false);

        float elapsed = 0f;

        _loadingFill.fillAmount = 0f;

        while (elapsed < _loadingDuration)
        {
            elapsed += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsed / _loadingDuration);

            _loadingFill.fillAmount = progress;

            _loadingPercentage.text =
                Mathf.RoundToInt(progress * 100f) + "%";

            yield return null;
        }

        _loadingFill.fillAmount = 1f;
        _loadingPercentage.text = "100%";

        yield return new WaitForSeconds(0.5f);

        // Hide loading screen and open Main Menu
        _loadingScreen.SetActive(false);
        ShowMainMenu();
    }

    public void ShowMainMenu() => Show(_mainMenu);
    public void ShowPlay() => Show(_playMenu);
    public void ShowShop() => Show(_shopMenu);
    public void ShowLeaderboard() => Show(_leaderboardMenu);
    public void ShowCredits() => Show(_creditsMenu);

    // Settings opens ON TOP of whatever screen is open
    public void OpenSettings() => _settingsMenu.SetActive(true);
    public void CloseSettings() => _settingsMenu.SetActive(false);

    // shows one screen and hides the others
    private void Show(GameObject screen)
    {
        Time.timeScale = 1f;
        _mainMenu.SetActive(screen == _mainMenu);
        _playMenu.SetActive(screen == _playMenu);
        _shopMenu.SetActive(screen == _shopMenu);
        _leaderboardMenu.SetActive(screen == _leaderboardMenu);
        _creditsMenu.SetActive(screen == _creditsMenu);
        _settingsMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}