using System;

[Serializable]
public class MechanicalElements
{
    public Quantity3 position;
    public Quantity3 velocity;

    public OrbitalElements ToOrbitalElements(Quantity mu)
    {
        var eccA = (velocity.SqrMagitude - (mu / position.Magnitude).Magnitude) * position;
        var eccB = Quantity3.Dot(position, velocity) * velocity;
        var eccVec = (eccA - eccB) / mu;

        var oe = new OrbitalElements();

        oe.Eccentricity = eccVec.Magnitude.DoubleValue;

        var specMechEnergy = (velocity.SqrMagitude / 2) - (mu / position.Magnitude);

        oe.SemiMajorAxis = -1 * (mu / (2 * specMechEnergy.Magnitude));

        Quantity3 angularMomentum = Quantity3.Cross(position, velocity);

        oe.Inclination = Math.Acos((angularMomentum.Y / angularMomentum.Magnitude).DoubleValue);

        Quantity3 nodeVector = Quantity3.Cross(new Quantity3(SciNumber.One, SciNumber.Zero, SciNumber.Zero), angularMomentum);

        oe.LongitudeOfAscendingNode = Math.Acos((nodeVector.X / nodeVector.Magnitude).DoubleValue);

        oe.ArgumentOfPeriapsis = Math.Acos((Quantity3.Dot(nodeVector, eccVec) / (nodeVector.Magnitude * eccVec.Magnitude)).DoubleValue);

        oe.TrueAnomoly = Math.Acos((Quantity3.Dot(eccVec, position) / (eccVec.Magnitude * position.Magnitude)).DoubleValue);

        return oe;
    }
}