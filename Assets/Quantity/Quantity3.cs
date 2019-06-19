using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Quantity3 : QuantityN
{
    public Quantity X => new Quantity(values[0], units);
    public Quantity Y => new Quantity(values[1], units);
    public Quantity Z => new Quantity(values[2], units);

    public Quantity3 Scaled => SimulationManager.Instance.Scale(this);

    public Quantity3() : base() { }

    public Quantity3(SciNumber x, SciNumber y, SciNumber z, Units units = null)
        : this(new List<SciNumber>() { x, y, z }, units) { }

    public Quantity3(List<SciNumber> values, Units units = null) : base(values, units) { }

    public Vector3 Vector => new Vector3(values[0].FloatValue, values[1].FloatValue, values[2].FloatValue);

    public Quantity4 Homogenous => new Quantity4(this, SciNumber.One);

    public static Quantity3 operator +(Quantity3 a, Quantity3 b)
    {
        return Combine(a, b, (u, v) => u + v);
    }

    public static Quantity3 operator -(Quantity3 a, Quantity3 b)
    {
        return Combine(a, b, (u, v) => u - v);
    }

    public static Quantity3 operator *(Quantity3 v, double s)
    {
        return Scale(v, s);
    }

    public static Quantity3 operator *(double s, Quantity3 v)
    {
        return v * s;
    }

    public static Quantity3 operator /(Quantity3 v, double s)
    {
        return Scale(v, 1 / s);
    }

    public static Quantity3 operator *(Quantity3 v, Quantity s)
    {
        return new Quantity3(v.values.Select(x => x * s.TrueValue).ToList(), v.units + s.units);
    }

    public static Quantity3 operator *(Quantity s, Quantity3 v)
    {
        return new Quantity3(v.values.Select(x => x * s.TrueValue).ToList(), v.units + s.units);
    }

    public static Quantity3 operator /(Quantity3 v, Quantity s)
    {
        return new Quantity3(v.values.Select(x => x / s.TrueValue).ToList(), v.units - s.units);
    }

    public static Quantity3 Cross(Quantity3 a, Quantity3 b)
    {
        return new Quantity3(new List<SciNumber>()
        {
            (a.Y * b.Z - a.Z * b.Y).TrueValue,
            (a.Z * b.X - a.X * b.Z).TrueValue,
            (a.X * b.Y - a.Y * b.X).TrueValue,
        }, a.units + b.units);
    }

    public static Quantity Dot(Quantity3 a, Quantity3 b)
    {
        return (a.X * b.X) + (a.Y * b.Y) + (a.Z * b.Z);
    }

    public Quantity3 Mul(Matrix4x4 mat)
    {
        return Homogenous.Mul(mat).DropDimension;
    }
}
