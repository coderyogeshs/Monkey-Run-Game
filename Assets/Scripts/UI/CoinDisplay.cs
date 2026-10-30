using TMPro;
using UnityEngine;

public class CoinDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _coinsText;

    private void OnEnable() => _coinsText.text = $"Coins: {PlayerData.Coins}";
}