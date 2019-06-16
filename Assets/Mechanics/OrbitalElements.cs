using System;
using UnityEngine;

[Serializable]
public class OrbitalElements
{
    public Quantity SemiMajorAxis;
    public double Eccentricity;

    public bool angleInRadians = true;
    public double Inclination;
    public double ArgumentOfPeriapsis;
    public double LongitudeOfAscendingNode;
    public double TrueAnomoly;

    public void SetToRadians()
    {
        if (angleInRadians == false)
        {
            angleInRadians = true;
            double factor = Math.PI / 180;

            Inclination *= factor;
            ArgumentOfPeriapsis *= factor;
            LongitudeOfAscendingNode *= factor;
            TrueAnomoly *= factor;
        }
    }

    public Quantity Period(Quantity mu) => 2 * Math.PI * (SemiMajorAxis * SemiMajorAxis * SemiMajorAxis / mu).Sqrt<Quantity>();


    /// <param name="trueAnomoly">Provide a true anomoly to sample the orbit</param>
    public MechanicalElements ToMechanicalElements(Quantity mu, double? trueAnomoly = null)
    {
        var ta = trueAnomoly ?? this.TrueAnomoly;

        var rA = SemiMajorAxis / (1 + Eccentricity * Math.Cos(ta));

        var perifocalR = new Quantity3(rA.TrueValue * Math.Cos(ta), rA.TrueValue * Math.Sin(ta), SciNumber.Zero, rA.units);

        var vP = new SciNumber(-Math.Sin(ta));
        var vQ = new SciNumber(Eccentricity + Math.Cos(ta));

        var perifocalV = (mu / SemiMajorAxis).Sqrt<Quantity>() * new Quantity3(vP, vQ, SciNumber.Zero);

        //TODO: use doubles instead
        var pToE = PerifocalToEquitorial();

        return new MechanicalElements()
        {
            position = perifocalR.Mul(pToE),
            velocity = perifocalV.Mul(pToE),
        };
    }

    //using floats so we can use unity matrix mul
    //I is inclination, a is ascending (big omega), and p is argument of periapsis (little omega)
    public Matrix4x4 PerifocalToEquitorial()
    {
        float i = (float)Inclination;
        float bigOmega = (float)LongitudeOfAscendingNode;
        float littleOmega = (float)ArgumentOfPeriapsis;

        float r00 = Mathf.Cos(bigOmega) * Mathf.Cos(littleOmega) - Mathf.Sin(bigOmega) * Mathf.Sin(littleOmega) * Mathf.Cos(i);
        float r01 = -Mathf.Cos(bigOmega) * Mathf.Sin(littleOmega) - Mathf.Sin(bigOmega) * Mathf.Cos(littleOmega) * Mathf.Cos(i);
        float r02 = Mathf.Sin(bigOmega) * Mathf.Sin(i);

        float r10 = Mathf.Sin(bigOmega) * Mathf.Cos(littleOmega) + Mathf.Cos(bigOmega) * Mathf.Sin(littleOmega) * Mathf.Cos(i);
        float r11 = -Mathf.Sin(bigOmega) * Mathf.Sin(littleOmega) + Mathf.Cos(bigOmega) * Mathf.Cos(littleOmega) * Mathf.Cos(i);
        float r12 = -Mathf.Cos(bigOmega) * Mathf.Sin(i);

        float r20 = Mathf.Sin(littleOmega) * Mathf.Sin(i);
        float r21 = Mathf.Cos(littleOmega) * Mathf.Sin(i);
        float r22 = Mathf.Cos(i);

        //Column major!
        return new Matrix4x4(
            new Vector4(r00, r10, r20, 0),
            new Vector4(r01, r11, r21, 0),
            new Vector4(r02, r12, r22, 0),
            new Vector4(0, 0, 0, 1f)
        );
    }

    public void UpdateTrueAnomoly(Quantity mu, double timePeriod)
    {
        TrueAnomoly += 2 * Math.PI * timePeriod / Period(mu).DoubleValue;

        if (TrueAnomoly > (2 * Math.PI))
        {
            TrueAnomoly -= 2 * Math.PI;
        }
    }
}