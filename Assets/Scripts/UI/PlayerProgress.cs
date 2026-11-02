using UnityEngine;

// Saves which levels are unlocked and the best stars per level (PlayerPrefs).
// Same idea as PlayerData: static, no UI inside.
public static class PlayerProgress
{
    private const string HighestUnlockedKey = "Progress.HighestUnlocked";
    private const string StarsKeyPrefix = "Progress.Stars.";   // + level index, e.g. "Progress.Stars.3"

    // the level the player tapped in Level Select (not saved, only for this run)
    public static int SelectedLevel { get; set; }

    // level 0 is always unlocked
    public static bool IsUnlocked(int levelIndex) => levelIndex <= PlayerPrefs.GetInt(HighestUnlockedKey, 0);

    public static int GetStars(int levelIndex) => PlayerPrefs.GetInt(StarsKeyPrefix + levelIndex, 0);

    // keeps the BEST stars and unlocks the next level
    public static void CompleteLevel(int levelIndex, int stars)
    {
        if (stars > GetStars(levelIndex))
            PlayerPrefs.SetInt(StarsKeyPrefix + levelIndex, stars);

        int next = levelIndex + 1;
        if (next > PlayerPrefs.GetInt(HighestUnlockedKey, 0))
            PlayerPrefs.SetInt(HighestUnlockedKey, next);

        PlayerPrefs.Save();
    }
}
