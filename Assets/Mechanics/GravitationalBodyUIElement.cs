using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ViewLevel
{
    None,
    Orbiting,
    SystemCenter,
    Planetary
}

public abstract class GravitationalBodyUIElement : MonoBehaviour
{
    protected GravitationalBody body;

    protected virtual void Awake()
    {
        body = GetComponentInParent<GravitationalBody>();
        body.uiElements.Add(this);
    }

    public abstract void Display(ViewLevel level);
}
