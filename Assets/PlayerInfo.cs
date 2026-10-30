using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] private string playerName = "Hero";
    [SerializeField] private int level = 1;
    [SerializeField] private int coins = 100;

    private void Start()
    {
        Debug.Log(GetSummary());
    }

    public string GetSummary()
    {
        return $"Player: {playerName} | Level: {level} | Coins: {coins}";
    }
}