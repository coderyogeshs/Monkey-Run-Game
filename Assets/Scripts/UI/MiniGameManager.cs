using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text _coinsText;
    [SerializeField] private Image _healthFill;
    [SerializeField] private float _healthDrainPerSecond = 0.05f;

    [Header("Levels")]
    [SerializeField] private LevelList _levels;
    [SerializeField] private int _coinsPerStar = 50;

    [Header("Result panel (Victory / Game Over)")]
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _resultText;
    [SerializeField] private GameObject _starsGroup;
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private Color _starEarnedColor = new Color(1f, 0.82f, 0.2f);
    [SerializeField] private Color _starEmptyColor = new Color(1f, 1f, 1f, 0.25f);

    private int _score, _coins, _bonusPoints, _extraCoins, _bonusMultiplier;
    private int _levelIndex, _target;
    private float _health, _timeLeft, _totalTime, _drain, _healAmount;
    private bool _isPlaying;

    private void OnEnable() => StartGame();

    public void StartGame()
    {
        // the level the player tapped in Level Select (kept inside the valid range)
        _levelIndex = Mathf.Clamp(PlayerProgress.SelectedLevel, 0, _levels.Count - 1);
        PlayerProgress.SelectedLevel = _levelIndex;
        LevelDefinition level = _levels.GetLevel(_levelIndex);
        _target = level.TargetScore;

        _score = 0;
        _coins = 0;
        _health = 1f;

        // each boost bought in the shop is used up by this match
        _timeLeft = level.TimeLimit + (PlayerData.UseItem("time") ? 10f : 0f);
        _totalTime = _timeLeft;   // stars are judged against this
        _drain = _healthDrainPerSecond * (PlayerData.UseItem("shield") ? 0.6f : 1f);
        _bonusPoints = PlayerData.UseItem("lucky") ? 5 : 0;
        _healAmount = PlayerData.UseItem("heal") ? 0.30f : 0.15f;
        _extraCoins = PlayerData.UseItem("magnet") ? 1 : 0;
        _bonusMultiplier = PlayerData.UseItem("golden") ? 2 : 1;

        _isPlaying = true;
        Time.timeScale = 1f;
        _gameOverPanel.SetActive(false);
        Refresh();
    }

    private void Update()
    {
        if (!_isPlaying) return;

        _timeLeft -= Time.deltaTime;
        _health -= _drain * Time.deltaTime;

        if (_timeLeft <= 0f || _health <= 0f) EndGame(false);

        Refresh();
    }

    // called by the skill buttons
    public void Collect() { if (_isPlaying) AddScore(10, 1); }
    public void Bonus() { if (_isPlaying) AddScore(25 * _bonusMultiplier, 3); }

    public void Heal()
    {
        if (_isPlaying) _health = Mathf.Min(1f, _health + _healAmount);
    }

    // buttons on the result panel
    public void Retry() => StartGame();

    public void NextLevel()
    {
        PlayerProgress.SelectedLevel++;   // StartGame keeps it in range
        StartGame();
    }

    private void AddScore(int points, int coins)
    {
        _score += points + _bonusPoints;
        _coins += coins + _extraCoins;

        if (_score >= _target) EndGame(true);   // reached the target = win
    }

    // more time left = more stars
    private int CalculateStars()
    {
        float timeRatio = _timeLeft / _totalTime;
        if (timeRatio >= 0.5f) return 3;
        if (timeRatio >= 0.25f) return 2;
        return 1;
    }

    private void EndGame(bool won)
    {
        if (!_isPlaying) return;
        _isPlaying = false;

        int stars = won ? CalculateStars() : 0;
        if (won)
        {
            _coins += stars * _coinsPerStar;
            PlayerProgress.CompleteLevel(_levelIndex, stars);   // saves stars + unlocks the next level
        }

        PlayerData.AddCoins(_coins);

        int best = Mathf.Max(PlayerPrefs.GetInt("bestScore", 0), _score);
        PlayerPrefs.SetInt("bestScore", best);
        PlayerPrefs.Save();

        // rank is 0 when the score didn't make the top 10
        int rank = Leaderboard.Submit(PlayerData.PlayerName, _score);

        _titleText.text = won ? "VICTORY!" : "GAME OVER";
        _titleText.color = won ? new Color(0.45f, 0.9f, 0.4f) : new Color(1f, 0.4f, 0.35f);

        string reason = won ? "" : (_health <= 0f ? "\nYou ran out of health" : "\nTime's up!");
        _resultText.text = $"Score: {_score} / {_target}\nBest: {best}\nCoins earned: {_coins}"
                           + reason
                           + (rank > 0 ? $"\nLeaderboard rank: #{rank}" : "");

       _starsGroup.SetActive(won);
Image[] starImages = _starsGroup.GetComponentsInChildren<Image>(true);   // finds Star1, Star2, Star3 itself
for (int i = 0; i < starImages.Length; i++)
    starImages[i].color = i < stars ? _starEarnedColor : _starEmptyColor;

        // Next Level only after a win, and only if another level exists
        _nextLevelButton.gameObject.SetActive(won && _levelIndex + 1 < _levels.Count);

        _gameOverPanel.SetActive(true);
        Refresh();
    }

    private void Refresh()
    {
        _scoreText.text = $"Score {_score}/{_target}";
        _timerText.text = Mathf.CeilToInt(Mathf.Max(0f, _timeLeft)).ToString();
        _coinsText.text = $"Coins {_coins}";
        _healthFill.fillAmount = Mathf.Clamp01(_health);
    }
}