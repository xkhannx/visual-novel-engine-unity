using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Data", menuName = "Game/New Character Data")]
public class CharacterSO : ScriptableObject
{
    public string characterName;
    public Sprite portrait;
    public string[] stats;
    public int money;
}
