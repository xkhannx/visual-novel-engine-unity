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

public enum Stats_ENUM
{
    Success,
    Businessman,
    Creator
}

public enum Lovers
{
    Kim,
    Sonmi,
    Minsu,
    Kyurin,
    Jiho
}

public enum Relations
{
    Love,
    Friendship
}

[Serializable]
public struct Choice
{
    public string text;
    public int cost;
}