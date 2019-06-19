using UnityEngine;

public class CameraPan : CameraControlScheme
{
    public float sensitivity;
    public float sensitivityToCameraHeight = 1f;
    public KeyCode forward;
    public KeyCode back;
    public KeyCode left;
    public KeyCode right;

    void Update()
    {
        float dx = 0f;
        float dy = 0f;

        if (Input.GetKey(forward))
        {
            dy += sensitivity;
        }
        if (Input.GetKey(back))
        {
            dy -= sensitivity;
        }
        if (Input.GetKey(left))
        {
            dx -= sensitivity;
        }
        if (Input.GetKey(right))
        {
            dx += sensitivity;
        }

        if(dx != 0 || dy != 0)
        {
            PanBy(dx, dy);
        }
    }

    public void PanBy(float dx, float dy)
    {
        var distanceModifier = Mathf.Abs(transform.position.z * sensitivityToCameraHeight);
        dx *= distanceModifier;
        dy *= distanceModifier;

        transform.position += dx * new Vector3(transform.right.x, transform.right.y).normalized + dy * new Vector3(transform.up.x, transform.up.y).normalized;
    }
}
