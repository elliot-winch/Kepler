using System;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class Ellipse : MonoBehaviour
{
    [Serializable]
    public class Definition
    {
        public Quantity semiMajorAxis;
        public double eccentricity;
    }

    public int resolution;

    private LineRenderer lr;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    public void Draw(Definition args)
    {
        Quantity semiMinorAxis = args.semiMajorAxis * Math.Sqrt(1 - (args.eccentricity * args.eccentricity));
        var points = new Vector3[resolution + 1];

        for (int i = 0; i <= resolution; i++)
        {
            double t = ((i / (double)resolution) * 2.0 * Math.PI);
            points[i] = new Vector3()
            {
                x = (float)(args.semiMajorAxis * Math.Cos(t)).Scaled.DoubleValue,
                y = (float)(semiMinorAxis * Math.Sin(t)).Scaled.DoubleValue,
            };
        }


        lr.positionCount = points.Length;
        lr.SetPositions(points);
    }
}
