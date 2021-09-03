using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyMath
{
    public static float Normalized(float value, float previousMin, float previousMax, float newMin, float newMax)
    {
        return newMin + ((newMax - newMin) * ((value - previousMin) / (previousMax - previousMin)));
    }
}
