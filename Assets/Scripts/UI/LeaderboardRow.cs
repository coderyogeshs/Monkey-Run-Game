using TMPro;
using UnityEngine;

public class LeaderboardRow : MonoBehaviour
{
    [SerializeField] private TMP_Text _rankText;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _scoreText;

    public void Set(int rank, string playerName, int score, Color color)
    {
        _rankText.text = $"#{rank}";
        _nameText.text = playerName;
        _scoreText.text = score.ToString();

        _rankText.color = _nameText.color = _scoreText.color = color;
    }
}