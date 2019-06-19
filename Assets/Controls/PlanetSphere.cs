using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSphere : GravitationalBodyUIElement, IClickable
{
    public override void Display(ViewLevel level)
    {
        GetComponent<Renderer>().enabled = level == ViewLevel.Planetary;
    }

    public void OnLeftClick()
    {

    }

    public void OnLeftDoubleClick()
    {
        //SimulationManager.Instance.SystemFocus(this.body, ViewLevel.Planetary);
        //SimulationManager.Instance.SetUnitScale(SIUnitType.Meter, new SciNumber(1, -6));
    }

    public void OnMouseOverBegin()
    {

    }

    public void OnMouseOverEnd()
    {

    }
}
