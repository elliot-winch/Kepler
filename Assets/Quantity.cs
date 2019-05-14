using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Quantity
{
    [SerializeField] SciNumber trueValue;
    [SerializeField] List<UnitExponent> units;

    public SciNumber Value => CalculateValue();
    public List<UnitExponent> Units => units;

    private SciNumber CalculateValue()
    {
        SciNumber val = trueValue;

        units.ForEach(x =>
        {
            val *= x.unit.scale ^ x.exponent;
        });

        return val;
    }

    public override string ToString()
    {
        string s = Value + "  ";

        foreach(var u in units)
        {
            s += u.unit.abbreviation + "^" + u.exponent + " ";
        }

        return s;
    }
}
