using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text _coinsText;
    [SerializeField] private Image _healthFill;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private TMP_Text _resultText;
    [SerializeField] private float _gameTime = 30f;
    [SerializeField] private float _healthDrainPerSecond = 0.05f;

    private int _score, _coins, _bonusPoints, _extraCoins, _bonusMultiplier;
    private float _health, _timeLeft, _drain, _healAmount;
    private bool _isPlaying;

    private void OnEnable() => StartGame();

    public void StartGame()
    {
        _score = 0;
        _coins = 0;
        _health = 1f;

        // each boost bought in the shop is used up by this match
        _timeLeft = _gameTime + (PlayerData.UseItem("time") ? 10f : 0f);
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

        if (_timeLeft <= 0f || _health <= 0f) EndGame();

        Refresh();
    }

    // called by the skill buttons
    public void Collect() { if (_isPlaying) AddScore(10, 1); }
    public void Bonus() { if (_isPlaying) AddScore(25 * _bonusMultiplier, 3); }

    public void Heal()
    {
        if (_isPlaying) _health = Mathf.Min(1f, _health + _healAmount);
    }

    private void AddScore(int points, int coins)
    {
        _score += points + _bonusPoints;
        _coins += coins + _extraCoins;
    }

    private void EndGame()
    {
        _isPlaying = false;
        PlayerData.AddCoins(_coins);

        int best = Mathf.Max(PlayerPrefs.GetInt("bestScore", 0), _score);
        PlayerPrefs.SetInt("bestScore", best);
        PlayerPrefs.Save();

        // rank is 0 when the score didn't make the top 10
        int rank = Leaderboard.Submit(PlayerData.PlayerName, _score);

        _resultText.text = $"Score: {_score}\nBest: {best}\nCoins earned: {_coins}"
                           + (rank > 0 ? $"\nLeaderboard rank: #{rank}" : "");
        _gameOverPanel.SetActive(true);
    }

    private void Refresh()
    {
        _scoreText.text = $"Score {_score}";
        _timerText.text = Mathf.CeilToInt(Mathf.Max(0f, _timeLeft)).ToString();
        _coinsText.text = $"Coins {_coins}";
        _healthFill.fillAmount = Mathf.Clamp01(_health);
    }
}