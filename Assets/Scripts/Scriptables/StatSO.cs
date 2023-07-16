using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stat", menuName = "Game/New Stat")]
public class StatSO : ScriptableObject
{
    public string statName = "";
    public int value;
}
