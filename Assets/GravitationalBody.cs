using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitationalBody : MonoBehaviour
{
    [Tooltip("In kg * 10^24")]
    public float mass;
    public Vector3 velocity;
    public Vector3 VernalEquinox => transform.right;

    //Classical Orbital Variables
    public float SemiMinorAxis
    {
        get 
        {
            return SemiMajorAxis * Mathf.Sqrt(1 - (Eccentricity.magnitude * Eccentricity.magnitude));
        }
    }

    //World co-ordiinate
    public Vector3 Periapsis
    {
        get
        {
            return Orbiting.transform.position + PeriapsisDistance * PeriapsisDirection;
        }
    }

    public float PeriapsisDistance
    {
        get
        {
            return SemiMajorAxis * (1 - Eccentricity.magnitude);
        }
    }

    public Vector3 Apoapsis
    {
        get
        {
            return Orbiting.transform.position + OrbitalMathUtililty.RotateAboutYAxis(ApoapsisDistance * Orbiting.VernalEquinox, Mathf.PI + ArgumentOfPeriapsis);
        }
    }

    public float ApoapsisDistance
    {
        get
        {
            return SemiMajorAxis * (1 + Eccentricity.magnitude);
        }
    }

    public Vector3 EllipseCenter
    {
        get
        {
            return Orbiting.transform.position + (SemiMajorAxis - ApoapsisDistance) * PeriapsisDirection;
        }
    }

    public Vector3 PeriapsisDirection
    {
        get
        {
            return OrbitalMathUtililty.RotateAboutYAxis(Orbiting.VernalEquinox, ArgumentOfPeriapsis);
        }
    }

    public float LongitudeOfPeriapsis
    {
        get
        {
            return LongitudeOfAscendingNode + ArgumentOfPeriapsis;
        }
    }

    public GravitationalBody Orbiting { get; private set; }
    public float SemiMajorAxis { get; private set; }
    public Vector3 Eccentricity { get; private set; }
    public float Inclination { get; private set; }
    public float ArgumentOfPeriapsis { get; private set; }
    public float LongitudeOfAscendingNode { get; private set; }
    public float TrueAnomoly { get; private set; }

    public void ApplyForce(Vector3 force)
    {
        velocity += (force / mass) * Time.fixedDeltaTime;
    }

    public void RevolveAround(GravitationalBody body)
    {
        this.Orbiting = body;

        transform.position += (velocity * Time.fixedDeltaTime);

        Vector3 r = transform.position - body.transform.position;
        Vector3 angularMomentum = Vector3.Cross(r, velocity);

        Vector3 nodeVector = Vector3.Cross(new Vector3(0f, 0f, 1f), angularMomentum);
        float mu = GravityManager.Instance.BigG * body.mass;

        Eccentricity = CalculateEccentricity(r, mu);

        if(Eccentricity.magnitude == 1)
        {
        }
        else
        {
            float mechanicalEnergy = ((velocity.magnitude * velocity.magnitude) / 2) - (mu / r.magnitude);

            SemiMajorAxis = -mu / (2 * mechanicalEnergy);
        }

        Inclination = Mathf.Acos(angularMomentum.y / angularMomentum.magnitude);

        LongitudeOfAscendingNode = Mathf.Acos(nodeVector.x / nodeVector.magnitude);
        if (nodeVector.y < 0)
        {
            LongitudeOfAscendingNode = Mathf.PI * 2 - LongitudeOfAscendingNode;
        }

        ArgumentOfPeriapsis = Mathf.PI - Mathf.Acos(Vector3.Dot(nodeVector, Eccentricity) / (nodeVector.magnitude * Eccentricity.magnitude));
        if (Eccentricity.y < 0)
        {
            ArgumentOfPeriapsis = Mathf.PI * 2 - ArgumentOfPeriapsis;
        }

        TrueAnomoly = Mathf.Acos(Vector3.Dot(Eccentricity, r) / (r.magnitude * Eccentricity.magnitude));

        Debug.LogFormat(" {0} {1} {2}", Eccentricity.magnitude, SemiMajorAxis, ArgumentOfPeriapsis);
    }

    private Vector3 CalculateEccentricity(Vector3 r, float mu)
    {
        var eccA = ((velocity.magnitude * velocity.magnitude) - mu / r.magnitude) * r;
        return (eccA - Vector3.Dot(r, velocity) * velocity) / mu;
    }
}

public static class OrbitalMathUtililty
{
    public static Vector3 RotateAboutYAxis(Vector3 v, float rads)
    {
        return new Vector3()
        {
            x =   v.x * Mathf.Cos(rads) + v.z * Mathf.Sin(rads),
            y =   v.y,
            z = - (v.x * Mathf.Sin(rads)) + v.z * Mathf.Cos(rads),
        };
    }
}
