using System;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(TrackTarget))]
public class GravitationBodyPath : MonoBehaviour
{
    public GravitationalBody body;
    public int resolution = 100;
    public bool autoRefresh;

    private LineRenderer lr;
    private TrackTarget tt;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        tt = GetComponent<TrackTarget>();
    }

    private void Start()
    {
        //TODO: position at barycenter, not one foci 
        tt.target = body.Orbiting.WorldTransform;

        Refresh();
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
}

