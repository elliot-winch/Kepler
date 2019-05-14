using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Ellipse : MonoBehaviour
{
    public int resolution;

    public void DrawEllipse(GravitationalBody body)
    {
        var positions = CreateEllipse(body);
        LineRenderer lr = GetComponent<LineRenderer>();

        lr.positionCount = resolution + 1;
        for (int i = 0; i <= resolution; i++)
        {
            lr.SetPosition(i, positions[i]);
        }

        transform.position = body.EllipseCenter;
    }

    Vector3[] CreateEllipse(GravitationalBody body)
    {
        var positions = new Vector3[resolution + 1];
        Quaternion q = Quaternion.AngleAxis(body.Inclination * Mathf.Rad2Deg, body.VernalEquinox);

        for (int i = 0; i <= resolution; i++)
        {
            float angle = ((float)i / (float)resolution * 2.0f * Mathf.PI);
            positions[i] = new Vector3(body.SemiMajorAxis * Mathf.Cos(angle), 0f, body.SemiMinorAxis * Mathf.Sin(angle));
            positions[i] = q * positions[i];
        }

        return positions;
    }
}