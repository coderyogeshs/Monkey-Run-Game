using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _playMenu;
    [SerializeField] private GameObject _shopMenu;
    [SerializeField] private GameObject _leaderboardMenu;
    [SerializeField] private GameObject _creditsMenu;
    [SerializeField] private GameObject _settingsMenu;

    private void Start() => ShowMainMenu();

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