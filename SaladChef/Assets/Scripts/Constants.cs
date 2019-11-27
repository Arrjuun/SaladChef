using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    /// <summary>
    /// Time to chop vegetables
    /// </summary>
    public static int[] TimeToChop = new int[] { 3, 3, 2, 5, 2, 1 };


    /// <summary>
    /// Score for delivering salad containing each of the vegtable in the salad
    /// </summary>
    public static int[] ScoreForVegetables = new int[] { 30, 30, 20, 50, 20, 10 };


    /// <summary>
    /// PowerUp pickup : Speed 0 , Time 1, Score 2
    /// </summary>
    public static int[] Booster = { 0, 1, 2 };

    public static bool GameOver = true;
}
