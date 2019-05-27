using System;

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

    public MechanicalElements ToMechanicalElements(Quantity mu)
    {
        var rA = SemiMajorAxis / (1 + Eccentricity * Math.Cos(TrueAnomoly));

        var perifocalR = new Quantity3(SciNumber.Zero, rA.TrueValue * Math.Cos(TrueAnomoly), rA.TrueValue * Math.Sin(TrueAnomoly), rA.units);

        var vP = new SciNumber(-Math.Sin(TrueAnomoly));
        var vQ = new SciNumber(Eccentricity + Math.Cos(TrueAnomoly));

        var perifocalV = (mu / SemiMajorAxis).Sqrt<Quantity>() * new Quantity3(vP, SciNumber.Zero, vQ);

        //TODO: use doubles instead
        var pToE = OrbitalUtility.PerifocalToEquitorial(this);

        return new MechanicalElements()
        {
            position = perifocalR.Mul(pToE),
            velocity = perifocalV.Mul(pToE),
        };
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