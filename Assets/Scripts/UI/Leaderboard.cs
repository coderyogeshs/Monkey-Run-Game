using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreEntry
{
    public string Name;
    public int Score;
}

// JsonUtility can't save a bare List, so the list is wrapped in a class
[System.Serializable]
public class ScoreList
{
    public List<ScoreEntry> Entries = new List<ScoreEntry>();
}

public static class Leaderboard
{
    private const string Key = "leaderboard";
    private const int MaxEntries = 10;

    public static List<ScoreEntry> Load()
    {
        string json = PlayerPrefs.GetString(Key, "");
        return string.IsNullOrEmpty(json)
            ? new List<ScoreEntry>()
            : JsonUtility.FromJson<ScoreList>(json).Entries;
    }

    // adds a score and returns its rank (1 = best), or 0 if it didn't make the top 10
    public static int Submit(string playerName, int score)
    {
        if (score <= 0) return 0;

        List<ScoreEntry> list = Load();
        ScoreEntry entry = new ScoreEntry { Name = playerName, Score = score };

        list.Add(entry);
        list.Sort((a, b) => b.Score.CompareTo(a.Score));   // highest first
        int rank = list.IndexOf(entry) + 1;

        if (list.Count > MaxEntries) list.RemoveRange(MaxEntries, list.Count - MaxEntries);

        PlayerPrefs.SetString(Key, JsonUtility.ToJson(new ScoreList { Entries = list }));
        PlayerPrefs.Save();

        return rank <= MaxEntries ? rank : 0;
    }
}