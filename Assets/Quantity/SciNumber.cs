using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SciNumber
{
    public static SciNumber Zero => new SciNumber(0, 0);
    public static SciNumber One => new SciNumber(1, 0);

    private const int SqrtSteps = 10;

    [SerializeField] public double mantissa = 0;
    [SerializeField] public int exponent = 0;

    public double Value => mantissa * Math.Pow(10, exponent);
    public float FloatValue => (float)Value;

    public SciNumber(double value) : this(value, exponent: 0) { }

    public SciNumber(double mantissa, int exponent)
    {
        if(mantissa == 0)
        {
            this.exponent = 0;
            this.mantissa = 0;
        }
        else
        {
            int mantissaFactor = Log10(mantissa);

            this.exponent = exponent + mantissaFactor;
            this.mantissa = mantissa / Math.Pow(10, mantissaFactor);
        }
    }

    private int Log10(double val)
    {
        double absVal = Math.Abs(val);

        if(absVal >= 10)
        {
            return 1 + Log10(val / 10);
        }
        else if(absVal > 0 && absVal < 1)
        {
            return -1 + Log10(val * 10);
        }
        else
        {
            return 0;
        }
    }

    public override bool Equals(object obj)
    {
        if(obj is SciNumber other)
        {           
            return mantissa.RoughlyEquals(other.mantissa) && other.exponent == this.exponent;
        }

        return false;
    }

    public static SciNumber operator -(SciNumber a)
    {
        return new SciNumber(-a.mantissa, a.exponent);
    }

    public static SciNumber operator +(SciNumber a, SciNumber b)
    {
        return Combine(a, b, (u, v) => u + v);
    }

    public static SciNumber operator -(SciNumber a, SciNumber b)
    {
        return Combine(a, b, (u, v) =>
        {
            bool uSign = u > 0;
            bool vSign = v > 0;

            if(uSign ^ vSign)
            {
                return u + v;
            }
            else
            {
                return u - v;
            }
        });
    }

    public static SciNumber operator +(SciNumber a, double b)
    {
        return a + new SciNumber(b);
    }

    public static SciNumber operator +(double b, SciNumber a)
    {
        return a + b;
    }

    public static SciNumber operator -(SciNumber a, double b)
    {
        return a - new SciNumber(b);
    }

    public static SciNumber operator -(double b, SciNumber a)
    {
        return new SciNumber(b) - a;
    }

    private static SciNumber Combine(SciNumber a, SciNumber b, Func<double, double, double> combine)
    {
        int expDif = a.exponent - b.exponent;

        double bMan = b.mantissa * Math.Pow(10, -expDif);

        return new SciNumber(combine(a.mantissa, bMan), a.exponent);
    }

    public static SciNumber operator *(double s, SciNumber v)
    {
        return v * s;
    }

    public static SciNumber operator *(SciNumber v, double s)
    {
        return new SciNumber(v.mantissa * s, v.exponent);
    }

    public static SciNumber operator /(SciNumber v, double s)
    {
        return new SciNumber(v.mantissa / s, v.exponent);
    }

    public static SciNumber operator *(SciNumber a, SciNumber b)
    {
        return new SciNumber(a.mantissa * b.mantissa, a.exponent + b.exponent);
    }

    public static SciNumber operator /(SciNumber a, SciNumber b)
    {
        return new SciNumber(a.mantissa / b.mantissa, a.exponent - b.exponent);
    }

    public static SciNumber operator ^(SciNumber a, int e)
    {
        return new SciNumber(a.mantissa, a.exponent + e);
    }

    /*
    public static bool operator <(SciNumber a, SciNumber b)
    {
        if(a.exponent != b.exponent)
        {
            return a.exponent < b.exponent;
        }

        return a.mantissa < b.mantissa;
    }

    public static bool operator >(SciNumber a, SciNumber b)
    {
        return b < a;
    }
    */

    public SciNumber Sqrt
    {
        get
        {
            return SqrtStep(this, 2 * new SciNumber(1, this.exponent / 2), 0);
        }
    }

    private SciNumber SqrtStep(SciNumber s, SciNumber x, int stepCount)
    {
        SciNumber nextX = 0.5 * (x + (s / x));

        if(stepCount >= SqrtSteps)
        {
            return nextX;
        } 
        else
        {
            return SqrtStep(s, nextX, ++stepCount);
        }
    }

    public SciNumber Clone()
    {
        return new SciNumber(mantissa, exponent);
    }

    public override string ToString()
    {
        return mantissa + "^" + exponent;
    }

    public override int GetHashCode()
    {
        var hashCode = -1387274962;
        hashCode = hashCode * -1521134295 + mantissa.GetHashCode();
        hashCode = hashCode * -1521134295 + exponent.GetHashCode();
        return hashCode;
    }
}
