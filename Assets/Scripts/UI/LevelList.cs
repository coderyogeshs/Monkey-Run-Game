using System.Collections.Generic;
using UnityEngine;

// ONE asset that holds every level in order.
// Create it with: right-click in Project > Create > Game > Level List
[CreateAssetMenu(fileName = "LevelList", menuName = "Game/Level List")]
public class LevelList : ScriptableObject
{
    [SerializeField] private List<LevelDefinition> _levels = new List<LevelDefinition>();

    public int Count => _levels.Count;

    public LevelDefinition GetLevel(int index) => _levels[index];
}
