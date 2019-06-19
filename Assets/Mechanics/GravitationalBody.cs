using System;
using System.Collections.Generic;
using UnityEngine;

//Maths from https://space.stackexchange.com/questions/1904/how-to-programmatically-calculate-orbital-elements-using-position-velocity-vecto
public partial class GravitationalBody : MonoBehaviour
{
    //Definitions
    public static Quantity GravitationalConstant = new Quantity
    (
        value: new SciNumber(6.67408, -11),
        units: new Units(new List<UnitExponent>()
        {
            new UnitExponent(SIUnitType.Meter,     3),
            new UnitExponent(SIUnitType.Kilogram, -1),
            new UnitExponent(SIUnitType.Second,   -2),
        })
    );

    public static double lowInclination = 0.01;

    //Parameters
    public Transform WorldTransform;
    
    public GravitationalBody Orbiting;
    public List<GravitationalBody> OrbitedBy;

    public Quantity mass;
    public MechanicalElements mechanicalElements;
    public OrbitalElements orbitalElements;
    
    /*
    //Calcualted properties
    public Quantity3 Barycenter
    {
        get
        {
            var semiMajorAxisDir = (mechanicalElements.position - Orbiting.mechanicalElements.position).Normalised<Quantity3>();

            var r = (mass / Orbiting.mass);
            var r2 = 1 + r;
            var dis = orbitalElements.SemiMajorAxis * r2;
            var pos = semiMajorAxisDir * dis;
            var bary = mechanicalElements.position - pos;

            return bary;
        }
    }
    */

    protected virtual void Start()
    {
        orbitalElements?.SetToRadians();
    }

    public virtual void UpdateOrbit(double time)
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
        }

        OrbitedBy?.ForEach(b =>
        {
            b.Orbiting = this;
            b.UpdateOrbit(time);
        });

        UpdateRepresentation();
    }

    public MechanicalElements SampleOrbit(double trueAnomoly)
    {
        return orbitalElements.ToMechanicalElements(GravitationalConstant * Orbiting.mass, trueAnomoly);
    }

    private Quantity3 PositionAt(double timestep)
    {
        var e = orbitalElements.Eccentricity;
        var M = MeanAnomoly(orbitalElements.TrueAnomoly, e);

        var E = EccentricAnomoly(M, e);

        var P = orbitalElements.SemiMajorAxis * (Math.Cos(E) - e);
        var Q = orbitalElements.SemiMajorAxis * Math.Sin(E) * Math.Sqrt(1 - (e * e));

        var perifocal = new Quantity3(P.TrueValue, Q.TrueValue, SciNumber.Zero, orbitalElements.SemiMajorAxis.units);

        return perifocal.Mul(orbitalElements.PerifocalToEquitorial());
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
