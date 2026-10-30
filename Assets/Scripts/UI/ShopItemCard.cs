using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemCard : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private Button _buyButton;
    [SerializeField] private TMP_Text _buyButtonText;

    private ShopItem _item;
    private ShopManager _shop;

    // called once by ShopManager when it creates the card
    public void Setup(ShopItem item, ShopManager shop)
    {
        _item = item;
        _shop = shop;

        _icon.sprite = item.Icon;
        _nameText.text = item.Name;
        _descriptionText.text = item.Description;
        _priceText.text = item.Price.ToString();

        _buyButton.onClick.AddListener(() => _shop.TryBuy(_item));

        Refresh();
    }

    // button shows BUY, BUY (x1) or READY
    public void Refresh()
    {
        int count = PlayerData.GetCount(_item.Id);
        bool full = count >= _shop.MaxPerItem;

        _buyButton.interactable = !full;
        _buyButtonText.text = full ? "READY" : (count > 0 ? $"BUY (x{count})" : "BUY");
    }
}