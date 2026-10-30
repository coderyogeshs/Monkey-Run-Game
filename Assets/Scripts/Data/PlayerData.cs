using UnityEngine;

public static class PlayerData
{
    private const string CoinsKey = "coins";

    public static int Coins => PlayerPrefs.GetInt(CoinsKey, 0);

    // the username saved in Settings > Account, or "Monkey" if none
    public static string PlayerName
    {
        get
        {
            string name = PlayerPrefs.GetString("acc_username", "");
            return string.IsNullOrEmpty(name) ? "Monkey" : name;
        }
    }

    public static void AddCoins(int amount)
    {
        PlayerPrefs.SetInt(CoinsKey, Coins + amount);
        PlayerPrefs.Save();
    }

    // false if the player can't afford it
    public static bool SpendCoins(int amount)
    {
        if (Coins < amount) return false;

        PlayerPrefs.SetInt(CoinsKey, Coins - amount);
        PlayerPrefs.Save();
        return true;
    }

    private static string ItemKey(string itemId) => "item_" + itemId;

    // how many of this boost the player holds for upcoming matches
    public static int GetCount(string itemId) => PlayerPrefs.GetInt(ItemKey(itemId), 0);

    public static void AddItem(string itemId)
    {
        PlayerPrefs.SetInt(ItemKey(itemId), GetCount(itemId) + 1);
        PlayerPrefs.Save();
    }

    // uses one up; false if the player had none
    public static bool UseItem(string itemId)
    {
        int count = GetCount(itemId);
        if (count <= 0) return false;

        PlayerPrefs.SetInt(ItemKey(itemId), count - 1);
        PlayerPrefs.Save();
        return true;
    }
}