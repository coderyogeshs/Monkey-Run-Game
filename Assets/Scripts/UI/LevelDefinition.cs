using UnityEngine;

// the fixed data of ONE level (shown inside LevelList in the Inspector)
[System.Serializable]
public class LevelDefinition
{
    public string DisplayName = "Level";
    [Min(1)] public int TargetScore = 150;   // reach this score to win
    [Min(5f)] public float TimeLimit = 30f;  // seconds
}
