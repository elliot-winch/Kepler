using UnityEngine;

public class CelestialBody : GravitationalBody
{
    [Header("Rotation")]
    public double rotationInclination;
    public Quantity rotationalPeriod;

    [SerializeField] private Transform axisTransform;
    [SerializeField] private Transform bodyTransform;

    private double currentRotation = 0; //in degrees

    protected override void Start()
    {
        base.Start();

        axisTransform.rotation = Quaternion.Euler((float)(orbitalElements.Inclination + rotationInclination), 0f, 0f);
    }

    public override void UpdateOrbit(double time)
    {
        base.UpdateOrbit(time);

        //TODO: J2000 epoch
        currentRotation += (time / rotationalPeriod.DoubleValue) * 360;
    }

    public override void UpdateRepresentation()
    {
        base.UpdateRepresentation();

        if(this.currentLevel != ViewLevel.None)
        {
            bodyTransform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, (float)currentRotation));
        }
    }
}
