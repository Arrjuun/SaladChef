using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HighScore
{
    public int Score;
    public string PlayerName;
}

[Serializable]
public class HighScores
{
    [SerializeField]
    public List<HighScore> highScores;
}