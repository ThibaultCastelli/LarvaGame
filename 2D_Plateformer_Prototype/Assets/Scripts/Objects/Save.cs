using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    public int currentLevel;
    public int totalLarva;

    public bool[] collectedLarva;
    public bool isNewLevel;

    public float xStartPos;
    public float yStartPos;

    public float xCamPos;
    public float yCamPos;
    public float xOffset;
    public float yOffset;
    public float orthographicSize;
    public float leftBound;
    public float rightBound;
    public float topBound;
    public float bottomBound;

    public Save(int currentLevel, int numberOfScenes)
    {
        this.currentLevel = currentLevel;

        totalLarva = 0;
        isNewLevel = true;

        // Initialize all the larva collected to false
        collectedLarva = new bool[numberOfScenes];
        for (int i = 0; i < collectedLarva.Length; i++)
            collectedLarva[i] = false;
    }
}
