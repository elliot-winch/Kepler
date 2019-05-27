using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GravitationBodyUI : MonoBehaviour
{
    public LineRenderer periapsisLine;
    public LineRenderer apoapsisLine;
    public Ellipse ellipse;

    public void Draw(GravitationalBody body)
    {
        periapsisLine.SetPositions(new Vector3[]
        {
            //body.Orbiting.position.ScaledValue,
            //body.Periapsis.PositionScaledVector
        });

        apoapsisLine.SetPositions(new Vector3[]
        {
            //body.Orbiting.position.ScaledValue,
            //body.Apoapsis.PositionScaledVector
        });

        ellipse.DrawEllipse(body);
    }
}

