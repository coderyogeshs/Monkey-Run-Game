using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// one thing you can buy (filled in the Inspector)
[System.Serializable]
public class ShopItem
{
    public string Id;
    public string Name;
    [TextArea] public string Description;
    public int Price;
    public Sprite Icon;
}

public class ShopManager : MonoBehaviour
{
    [SerializeField] private List<ShopItem> _items = new List<ShopItem>();
    [SerializeField] private ShopItemCard _cardPrefab;
    [SerializeField] private Transform _container;
    [SerializeField] private TMP_Text _coinsText;
    [SerializeField] private TMP_Text _statusText;
    [SerializeField] private float _statusDuration = 2f;
    [SerializeField] private int _maxPerItem = 1;   // how many of one boost you can hold

    public int MaxPerItem => _maxPerItem;

    private readonly List<ShopItemCard> _cards = new List<ShopItemCard>();
    private Coroutine _statusRoutine;

    // build one card per item, once
    private void Awake()
    {
        foreach (ShopItem item in _items)
        {
            ShopItemCard card = Instantiate(_cardPrefab, _container);
            card.Setup(item, this);
            _cards.Add(card);
        }
    }

    // runs every time the shop is opened
    private void OnEnable()
    {
        RefreshAll();
        if (_statusText != null) _statusText.text = "";
    }

    public void TryBuy(ShopItem item)
    {
        if (PlayerData.GetCount(item.Id) >= _maxPerItem)
        {
            ShowStatus("Already ready for your next match", true);
            return;
        }

        if (!PlayerData.SpendCoins(item.Price))
        {
            ShowStatus("Not enough coins", true);
            return;
        }

        PlayerData.AddItem(item.Id);
        ShowStatus($"{item.Name} ready for your next match!");
        RefreshAll();
    }

    private void RefreshAll()
    {
        _coinsText.text = PlayerData.Coins.ToString();
        foreach (ShopItemCard card in _cards) card.Refresh();
    }

    private void ShowStatus(string message, bool isError = false)
    {
        if (_statusText == null) return;

        _statusText.gameObject.SetActive(true);   // nothing is drawn if the text object is off

        if (_statusRoutine != null) StopCoroutine(_statusRoutine);
        _statusRoutine = StartCoroutine(StatusRoutine(message, isError));
    }

    private IEnumerator StatusRoutine(string message, bool isError)
    {
        _statusText.color = isError ? Color.red : Color.green;
        _statusText.text = message;

        // realtime, so it also works while the game is paused
        yield return new WaitForSecondsRealtime(_statusDuration);

        _statusText.text = "";
    }
}