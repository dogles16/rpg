using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New LearningTrack", menuName = "Player/Learning Track")]
public class LearningTrack : ScriptableObject
{
    public List<SpellObject> spells;
    public List<int> spellLevels;
    public LevelsXp levelsExp;
}
