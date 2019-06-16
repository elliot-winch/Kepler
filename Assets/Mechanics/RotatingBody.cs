using UnityEngine;

[RequireComponent(typeof(GravitationalBody))]
public class RotatingBody : MonoBehaviour
{
    public double rotationInclination;
    public Quantity rotationalPeriod;

    [SerializeField] private Transform axisTransform;
    [SerializeField] private Transform bodyTransform;

    private double currentRotation = 0; //in degrees
    private GravitationalBody body;

    private void Start()
    {
        body = GetComponent<GravitationalBody>();

        axisTransform.rotation = Quaternion.Euler((float)(body.orbitalElements.Inclination + rotationInclination), 0f, 0f);
    }

    public void UpdateRotation(double time)
    {
        if(enabled)
        {
            //TODO: J2000 epoch
            currentRotation += (time / rotationalPeriod.DoubleValue) * 360;
            bodyTransform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, (float)currentRotation));
        }
    }
}
