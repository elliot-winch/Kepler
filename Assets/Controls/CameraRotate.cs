using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : CameraControlScheme
{
    public float sensitivity;
    public KeyCode rotateLeft;
    public KeyCode rotateRight;
    public float minAngle;

    private Plane zeroPlane = new Plane(Vector3.forward, Vector3.zero);

    void Update()
    {
        if (Input.GetKey(rotateLeft))
        {
            RotateBy(-sensitivity);
        }

        if (Input.GetKey(rotateRight))
        {
            RotateBy(sensitivity);
        }
    }

    public void RotateBy(float dr)
    {
        if(transform.localEulerAngles.x < minAngle)
        {
            return;
        }

        Ray r = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));

        zeroPlane.Raycast(r, out float distance);

        Vector3 point = r.GetPoint(distance);

        transform.RotateAround(point, Vector3.forward, dr);
    }
}
