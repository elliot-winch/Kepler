using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

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

public class SimulationManager : MonoBehaviour
{
    public static SimulationManager Instance { get; private set; }

    public GravitationalBody systemCenter;

    public List<UnitScale> unitScales;
    public float timeScale = 1;

    private GravitationalBody currentFocus;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SystemFocus(systemCenter);
    }

    private void FixedUpdate()
    {
        double deltaTime = Time.fixedDeltaTime * timeScale;

        //Update orbit data of all celestial bodies
        systemCenter?.OrbitedBy.ForEach(x => x.UpdateOrbit(deltaTime));        
    }

    public T Scale<T>(T quantity) where T : QuantityN
    {
        SciNumber scaler = SciNumber.One;

        quantity.units.unitExponents.ForEach(x => scaler *= (unitScales.FirstOrDefault(y => y.unit == x.unit)?.scale ?? SciNumber.One));

        return QuantityN.CreateQuantityInstance<T>(quantity.values.Select(x => x * scaler).ToList(), quantity.units);
    }

    public void SystemFocus(GravitationalBody systemCenter, SciNumber distanceScale = null)
    {
        if(currentFocus == systemCenter)
        {
            return;
        }

        currentFocus?.SetViewLevel(ViewLevel.None);
        currentFocus?.OrbitedBy.ForEach(x => x.SetViewLevel(ViewLevel.None));

        if(distanceScale != null)
        {
            SetUnitScale(SIUnitType.Meter, distanceScale);
        }

        systemCenter.SetViewLevel(ViewLevel.SystemCenter);
        systemCenter.OrbitedBy.ForEach(x => x.SetViewLevel(ViewLevel.Orbiting));

        currentFocus = systemCenter;
    }

    public void SetUnitScale(SIUnitType unit, SciNumber scale)
    {
        var distanceScaleInfo = unitScales.FirstOrDefault(x => x.unit == unit);

        if (distanceScaleInfo != null)
        {
            distanceScaleInfo.scale = scale;
        }
        else
        {
            unitScales.Add(new UnitScale()
            {
                scale = scale,
                unit = unit
            });
        }
    }
}
