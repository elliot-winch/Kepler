using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public enum SIUnitType
{
    Second,
    Meter,
    Kilogram,
    Kelvin,
    //Ampere,
    //Mole,
    //Candela
}

[Serializable]
public class UnitScale
{
    public SIUnitType unit;
    public SciNumber scale;
}

public class SIUnitManager : MonoBehaviour
{
    public static SIUnitManager Instance { get; private set; }

    public List<UnitScale> unitScales;

    private void Awake()
    {
        Instance = this;
    }

    public T Scale<T>(T quantity) where T : QuantityN
    {
        SciNumber scaler = SciNumber.One;

        quantity.units.unitExponents.ForEach(x => scaler *= (unitScales.FirstOrDefault(y => y.unit == x.unit)?.scale ?? SciNumber.One));

        return QuantityN.CreateQuantityInstance<T>(quantity.values.Select(x => x * scaler).ToList(), quantity.units);

    }

}
