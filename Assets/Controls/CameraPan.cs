using UnityEngine;

public class CameraPan : CameraControlScheme
{
    public float sensitivity;
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
        transform.position += dx * new Vector3(transform.right.x, transform.right.y).normalized + dy * new Vector3(transform.up.x, transform.up.y).normalized;
    }
}
