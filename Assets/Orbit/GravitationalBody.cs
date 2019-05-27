﻿using System;
using System.Collections.Generic;
using UnityEngine;

//Maths from https://space.stackexchange.com/questions/1904/how-to-programmatically-calculate-orbital-elements-using-position-velocity-vecto
public class GravitationalBody : MonoBehaviour
{
    public static Quantity GravitationalConstant = new Quantity
    (
        value: new SciNumber(6.67408, -11),
        units: new Units(new List<UnitExponent>()
        {
            new UnitExponent(SIUnitType.Meter,     3),
            new UnitExponent(SIUnitType.Kilogram, -2),
            new UnitExponent(SIUnitType.Second,   -1),
        })
    );


    //Definitions
    public static double lowInclination = 0.01;

    //Parameters
    
    public GravitationalBody Orbiting;

    public Quantity mass;
    public MechanicalElements mechanicalElements;
    public OrbitalElements orbitalElements;

    private void Start()
    {
        orbitalElements?.SetToRadians();
    }

    public void UpdateOrbit(double time)
    {
        if(Orbiting != null)
        {
            var mu = Orbiting.mass * GravitationalConstant;

            if (orbitalElements == null)
            {
                orbitalElements = mechanicalElements.ToOrbitalElements(mu);
            }

            orbitalElements.UpdateTrueAnomoly(mu, time);

            mechanicalElements = orbitalElements.ToMechanicalElements(mu);

            //Update front end
            transform.position = mechanicalElements.position.Scaled.Vector;  
        }
    }

    private Quantity3 PositionAt(double timestep)
    {
        var e = orbitalElements.Eccentricity;
        var M = MeanAnomoly(orbitalElements.TrueAnomoly, e);

        var E = EccentricAnomoly(M, e);

        var P = orbitalElements.SemiMajorAxis * (Math.Cos(E) - e);
        var Q = orbitalElements.SemiMajorAxis * Math.Sin(E) * Math.Sqrt(1 - (e * e));

        var perifocal = new Quantity3(SciNumber.Zero, P.TrueValue, Q.TrueValue, orbitalElements.SemiMajorAxis.units);

        return perifocal.Mul(OrbitalUtility.PerifocalToEquitorial(orbitalElements));
    }

    private double MeanAnomoly(double trueAnomoly, double e)
    {
        var d = (e + Math.Cos(trueAnomoly)) / (1 + e * Math.Cos(trueAnomoly));
        Debug.Log(d);
        double E = Math.Acos(d);

        int factor = trueAnomoly > Math.PI ? -1 : 1;

        return factor * (E - e * Math.Sin(E));
    }

    //Using Newtons methods
    private double EccentricAnomoly(double M, double e, int steps = 3)
    {
        var E = M;

        for(int i = 0; i < steps; i++)
        {
            var dE = (E - e * Math.Sin(E) - M) / (1 - e * Math.Cos(E));
            E -= dE;
        }

        return E;
    }

    /* TODO
public static Quantity3 VelocityAt(OrbitalElements oe, double timestep)
{

}
*/

    /* Gravitational Force - Correct but unused
    /// <param name="body"></param>
    /// <param name="upon"></param>
    /// <returns>The gravitational force of A upon B</returns>
    public Quantity3 GravitationalForce(GravitationalBody body, GravitationalBody upon)
    {
        var r = (upon.mechanicalElements.position - body.mechanicalElements.position);
        var sqrR = (upon.mechanicalElements.position - body.mechanicalElements.position).SqrMagitude;

        Quantity forceMag = (GravityManager.Instance.BigG * body.mass * upon.mass) / sqrR;

        Quantity3 forceDir = r.Normalised<Quantity3>();

        var force = -1 * forceDir * forceMag.Magnitude;

        Debug.DrawLine(transform.position, force.Scaled.Vector + transform.position, Color.green, 1f);

        return force;
    }
    */

    /* Other paramters - unconfirmed calculations
public Quantity3 Apoapsis
{
    get
    {
        return OrbitalMathUtililty.RotateAboutYAxis(ApoapsisDistance * Orbiting.VernalEquinox, Math.PI + ArgumentOfPeriapsis);
    }
}

public Quantity ApoapsisDistance
{
    get
    {
        return SemiMajorAxis * (1 + Eccentricity.Magnitude);
    }
}

//Relative to orbited body
public Quantity3 EllipseCenter
{
    get
    {
        return (SemiMajorAxis - ApoapsisDistance) * PeriapsisDirection;
    }
}

public Quantity3 PeriapsisDirection
{
    get
    {
        return OrbitalMathUtililty.RotateAboutYAxis(Orbiting.VernalEquinox, (float)ArgumentOfPeriapsis);
    }
}

public double LongitudeOfPeriapsis
{
    get
    {
        return LongitudeOfAscendingNode + ArgumentOfPeriapsis;
    }
}
*/

}
