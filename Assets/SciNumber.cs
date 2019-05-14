using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SciNumber
{
    public double mantissa;
    public double exponent;

    public double DoubleValue => Math.Pow(mantissa, exponent);
    public float Value => (float)DoubleValue;

    public static SciNumber operator*(SciNumber a, SciNumber b)
    {
        return new SciNumber()
        {
            mantissa = a.mantissa * b.mantissa,
            exponent = a.exponent + b.exponent,
        };
    }

    public static SciNumber operator /(SciNumber a, SciNumber b)
    {
        return new SciNumber()
        {
            mantissa = a.mantissa / b.mantissa,
            exponent = a.exponent - b.exponent,
        };
    }

    public static SciNumber operator ^(SciNumber a, float e)
    {
        return new SciNumber()
        {
            exponent = a.exponent + e,
        };
    }
}
