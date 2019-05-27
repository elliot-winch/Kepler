using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UnitExponent
{
    public SIUnitType unit;
    public int exponent;

    public UnitExponent(SIUnitType unit, int exponent = 1)
    {
        this.unit = unit;
        this.exponent = exponent;
    }

    public override string ToString()
    {
        return unit + "^" + exponent;
    }
}

[Serializable]
public class Units
{
    public List<UnitExponent> unitExponents;

    public Units(SIUnitType unit) : this(new UnitExponent(unit)) { }

    public Units(UnitExponent unit) : this(new List<UnitExponent> { unit }) { }

    public Units(List<UnitExponent> units = null)
    {
        this.unitExponents = units ?? new List<UnitExponent>();
    }

    public Units Clone()
    {
        return this + new Units();
    }

    public int ExponentFor(SIUnitType unit)
    {
        return unitExponents.FirstOrDefault(x => x.unit == unit)?.exponent ?? 0;
    }

    public override string ToString()
    {
        string s = "";
        unitExponents.ForEach(u => s += u);
        return s;
    }

    public override bool Equals(object obj)
    {
        if(obj is Units other)
        {
            foreach (SIUnitType u in Enum.GetValues(typeof(SIUnitType)))
            {
                if (this.ExponentFor(u) != other.ExponentFor(u))
                {
                    return false;
                }
            }

            //if all exponents are equal
            return true;
        }

        //if not units
        return false;
    }

    public override int GetHashCode()
    {
        return -1754912516 + EqualityComparer<List<UnitExponent>>.Default.GetHashCode(unitExponents);
    }

    public static Units operator +(Units a, Units b)
    {
        return CombineUnits(a, b, (u, v) => u + v);
    }

    public static Units operator -(Units a, Units b)
    {
        return CombineUnits(a, b, (u, v) => u - v);
    }

    public static Units operator /(Units v, int s)
    {
        return new Units(v.unitExponents.Select(x => new UnitExponent(x.unit, x.exponent/ s)).ToList());
    }

    private static Units CombineUnits(Units a, Units b, Func<int, int, int> combine)
    {
        List<UnitExponent> data = new List<UnitExponent>();

        foreach (SIUnitType u in Enum.GetValues(typeof(SIUnitType)))
        {
            int exp = combine(a.ExponentFor(u), b.ExponentFor(u));

            if (exp != 0)
            {
                data.Add(new UnitExponent(u, exp));
            }
        }

        return new Units(data);
    }
}
