using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class QuantityN
{
    public List<SciNumber> values;
    public Units units;

    public QuantityN()
    {
        this.values = new List<SciNumber>();
        this.units = new Units();
    }

    protected QuantityN(List<SciNumber> values, Units units = null)
    {
        this.values = values;
        this.units = units?.Clone() ?? new Units();
    }

    public override bool Equals(object obj)
    {
        if (obj is QuantityN other)
        {
            if(CheckDimensionCount(this, other, throwException: false)
                && CheckMatchingUnits(this, other, throwException: false))
            {
                for(int i = 0; i < values.Count; i++)
                {
                    if(values[i] != other.values[i])
                    {
                        //if any values are unequal
                        return false;
                    }
                }

                //all values equal
                return true;
            }

            //if non mathcing units or different dimension count
            return false;
        }

        //if not units
        return false;
    }

    public override string ToString()
    {
        string s = "";
        values.ForEach(u => s += u + " ");
        return s + " " + units;
    }

    public static T CreateQuantityInstance<T>(List<SciNumber> values, Units units) where T : QuantityN
    {
        var comb = Activator.CreateInstance(typeof(T)) as T;
        comb.values = values;
        comb.units = units;
        return comb;
    }

    public Quantity Dot<T>(T other) where T : QuantityN
    {
        CheckDimensionCount(this, other);

        SciNumber dot = SciNumber.Zero;

        for(int i = 0; i < values.Count; i++)
        {
            dot += values[i] * other.values[i];
        }

        return new Quantity(dot, units + other.units);
    }

    //Normalised vectors have no units
    public T Normalised<T>() where T : QuantityN
    {
        var mag = Magnitude.TrueValue;

        return CreateQuantityInstance<T>(this.values.Select(x => x / mag).ToList(), new Units());
    }

    public T Sqrt<T>() where T : QuantityN
    {
        return CreateQuantityInstance<T>(this.values.Select(x => x.Sqrt).ToList(), 
            new Units(units.unitExponents.Select(v =>
            {
                if(Mathf.Abs(v.exponent) % 2 != 0)
                {
                    throw new Exception("Quantity: Sqrt does not support creating non-integer unit exponents. Given units: " + units);
                }

                return new UnitExponent(v.unit, v.exponent / 2);
            }).ToList()));
    }

    public Quantity SqrMagitude
    {
        get
        {
            SciNumber factor = SciNumber.Zero;

            for (int i = 0; i < values.Count; i++)
            {
                factor += values[i] * values[i];
            }

            return new Quantity(factor, this.units + this.units);
        }
    }

    public Quantity Magnitude => new Quantity(SqrMagitude.TrueValue.Sqrt, this.units);

    protected static T Scale<T>(T v, double s) where T : QuantityN
    {
        List<SciNumber> vals = new List<SciNumber>();

        for (int i = 0; i < v.values.Count; i++)
        {
            vals.Add(v.values[i] * s);
        }

        return CreateQuantityInstance<T>(vals, v.units);
    }

    protected static T Combine<T>(T a, T b, Func<SciNumber, SciNumber, SciNumber> combine) where T : QuantityN
    {
        CheckDimensionCount(a, b);
        CheckMatchingUnits(a, b);

        List<SciNumber> vals = new List<SciNumber>();

        for (int i = 0; i < a.values.Count; i++)
        {
            vals.Add(combine(a.values[i], b.values[i]));
        }

        return CreateQuantityInstance<T>(vals, b.units);

    }

    protected static bool CheckMatchingUnits(QuantityN a, QuantityN b, bool throwException = true)
    {
        bool x = a.units.Equals(b.units);

        if (x == false && throwException)
        {
            throw new ArgumentException("Operation cannot be performed on values with units \n" + a.units + " and " + b.units);
        }

        return x;
    }

    protected static bool CheckDimensionCount(QuantityN a, QuantityN b, bool throwException = true)
    {
        bool x = a.values.Count == b.values.Count;

        if (x == false && throwException)
        {
            throw new ArgumentException("Cannot perform this operation on Quantities with different number of values");
        }

        return x;
    }

    public override int GetHashCode()
    {
        var hashCode = 728396047;
        hashCode = hashCode * -1521134295 + EqualityComparer<List<SciNumber>>.Default.GetHashCode(values);
        hashCode = hashCode * -1521134295 + EqualityComparer<Units>.Default.GetHashCode(units);
        return hashCode;
    }
}