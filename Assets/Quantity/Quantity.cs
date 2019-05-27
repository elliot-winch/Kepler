using System;
using System.Collections.Generic;

[Serializable]
public class Quantity : QuantityN
{
    public Quantity Scaled => SIUnitManager.Instance.Scale(this);
    public double DoubleValue => TrueValue.Value;
    public SciNumber TrueValue => values[0];

    public Quantity() : base() { }

    public Quantity(double value, Units units = null) : this(new SciNumber(value), units) { }

    public Quantity(SciNumber value, Units units = null) : base(new List<SciNumber>() { value }, units) { }

    public static Quantity operator -(Quantity a)
    {
        return new Quantity(-a.TrueValue, a.units);
    }

    public static Quantity operator +(Quantity v, double s)
    {
        return new Quantity(v.TrueValue + s);
    }

    public static Quantity operator +(double s, Quantity v)
    {
        return v + s;
    }

    public static Quantity operator -(Quantity v, double s)
    {
        return new Quantity(v.TrueValue - s);
    }

    public static Quantity operator -(double s, Quantity v)
    {
        return new Quantity(s - v.TrueValue);
    }

    public static Quantity operator *(Quantity v, double s)
    {
        return Scale(v, s);
    }

    public static Quantity operator *(double s, Quantity v)
    {
        return v * s;
    }

    public static Quantity operator /(Quantity v, double s)
    {
        return Scale(v, 1 / s);
    }

    public static Quantity operator +(Quantity a, Quantity b)
    {
        return Combine(a, b, (u, v) => u + v);
    }

    public static Quantity operator -(Quantity a, Quantity b)
    {
        return Combine(a, b, (u, v) => u - v);
    }

    public static Quantity operator *(Quantity a, Quantity b)
    {
        return new Quantity
        (
            value: a.TrueValue * b.TrueValue,
            units: a.units + b.units
        );
    }

    public static Quantity operator /(Quantity a, Quantity b)
    {
        return new Quantity
        (
            value: a.TrueValue / b.TrueValue,
            units:  a.units - b.units
        );
    }
}