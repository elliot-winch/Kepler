using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class MouseSelectFocus : MonoBehaviour
{
    public new Camera camera;

    private MouseSelect<CelestialSelectable> ms;

    private void Start()
    {
        ms = new MouseSelect<CelestialSelectable>(camera)
        {
            OnLeftClick = x => Debug.Log("LeftClick " + x.name),
            OnMouseOverBegin = x => Debug.Log("MouseOver " + x.name),
            OnMouseOverEnd = x => Debug.Log("MouseOver " + x.name),
            OnLeftDoubleClick = x =>
            {
                SimulationManager.Instance.Focus(x.gravitationalBody, x.level);
                SimulationManager.Instance.SetUnitScale(SIUnitType.Meter, x.distanceScale);

                Debug.Log("Focus " + x.gravitationalBody.name);
            }
        };
    }

    private void Update()
    {
        ms.Update();
    }
}
*/
