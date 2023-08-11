using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Emotions
{
    Neutral,
    Happy,
    Sad,
    Angry,
    Surprise,
    Shame
}

[Serializable]
public struct Choice
{
    public string text;
    public int cost;
}