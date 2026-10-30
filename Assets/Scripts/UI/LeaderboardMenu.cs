using System.Collections.Generic;
using UnityEngine;

public class LeaderboardMenu : MonoBehaviour
{
    [SerializeField] private LeaderboardRow _rowPrefab;
    [SerializeField] private Transform _container;     // the object with the Vertical Layout Group
    [SerializeField] private GameObject _emptyText;    // "No scores yet" message

    [SerializeField] private Color _gold = new Color(1f, 0.84f, 0.2f);
    [SerializeField] private Color _silver = new Color(0.8f, 0.85f, 0.9f);
    [SerializeField] private Color _bronze = new Color(0.85f, 0.55f, 0.3f);
    [SerializeField] private Color _normal = Color.white;

    // rebuilt every time the page opens, so it always shows the latest scores
    private void OnEnable()
    {
        foreach (Transform child in _container)
        {
            child.gameObject.SetActive(false);   // hide now, so the layout ignores it
            Destroy(child.gameObject);
        }

        List<ScoreEntry> scores = Leaderboard.Load();
        _emptyText.SetActive(scores.Count == 0);

        for (int i = 0; i < scores.Count; i++)
        {
            LeaderboardRow row = Instantiate(_rowPrefab, _container);
            row.Set(i + 1, scores[i].Name, scores[i].Score, ColorFor(i + 1));
        }
    }

    private Color ColorFor(int rank) => rank == 1 ? _gold : rank == 2 ? _silver : rank == 3 ? _bronze : _normal;
}