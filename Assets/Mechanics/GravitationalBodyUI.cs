using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GravitationalBody : MonoBehaviour
{
    protected ViewLevel currentLevel;

    [HideInInspector]
    public List<GravitationalBodyUIElement> uiElements = new List<GravitationalBodyUIElement>();

    public void SetViewLevel(ViewLevel level)
    {
        if(currentLevel != level)
        {
            currentLevel = level;

            uiElements.ForEach(x => x.Display(currentLevel));
        }
    }

    public virtual void UpdateRepresentation()
    {
        if (Orbiting != null && this.currentLevel == ViewLevel.Orbiting)
        {
            WorldTransform.position = mechanicalElements.position.Scaled.Vector;
        }
    }

    public static Vector3 WorldPosition(MechanicalElements me, GravitationalBody orbiting)
    {
        return (orbiting.mechanicalElements.position + me.position).Scaled.Vector;
    }
}
