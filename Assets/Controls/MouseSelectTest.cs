using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSelectTest : MonoBehaviour
{
    public new Camera camera;

    private MouseSelect<GravitationalBody> ms;

    private void Start()
    {
        ms = new MouseSelect<GravitationalBody>(camera)
        {
            OnLeftClick = x => Debug.Log("LeftClick " + x.name), 
            OnMouseOverBegin = x => Debug.Log("MouseOver " + x.name), 
            OnMouseOverEnd = x => Debug.Log("MouseOver " + x.name), 
            OnLeftDoubleClick = x => Debug.Log("Double " + x.name),
        };
    }

    private void Update()
    {
        ms.Update();
    }
}
