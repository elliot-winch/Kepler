using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathsUtility
{
    public static double forgiveness = 0.00001;

    public static bool RoughlyEquals(this double x, double y)
    {
        double a = x - y;
        return Between(a, - forgiveness, forgiveness, false);
    }

    public static bool Between(double x, double min, double max, bool strictly)
    {
        return strictly ? (x > min && x < max) : (x >= min && x <= max);
    }
}
