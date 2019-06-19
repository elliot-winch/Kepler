using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemCenter : GravitationalBodyUIElement, IClickable
{
    public override void Display(ViewLevel level)
    {
        GetComponent<Renderer>().enabled = level == ViewLevel.SystemCenter;
    }

    public void OnLeftClick()
    {

    }

    public void OnLeftDoubleClick()
    {
        //SimulationManager.Instance.SystemFocus(this.body, new SciNumber(1, -6));
    }

    public void OnMouseOverBegin()
    {
        //Debug.Log("Mouse over system pip");
    }

    public void OnMouseOverEnd()
    {

    }
}
