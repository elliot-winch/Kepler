using System;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class GravitationBodyPath : GravitationalBodyUIElement
{
    public int resolution = 100;
    public bool autoRefresh;

    private LineRenderer lr;

    protected override void Awake()
    {
        base.Awake();

        lr = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        Refresh();
    }

    private void Update()
    {
        //TODO barycenter
        if(body.Orbiting != null)
        {
            //transform.position = body.Orbiting.WorldTransform.position;
        }
    }

    public void Refresh()
    {
        if(resolution <= 0)
        {
            lr.positionCount = 0;
            return;
        }

        var points = new Vector3[resolution + 1];

        for (int i = 0; i <= resolution; i++)
        {
            double t = ((i / (double)resolution) * 2.0 * Math.PI);
            points[i] = body.SampleOrbit(t).position.Scaled.Vector;
        }

        lr.positionCount = points.Length;
        lr.SetPositions(points);
    }

    public override void Display(ViewLevel level)
    {
        lr.enabled = level == ViewLevel.Orbiting;
    }
}

