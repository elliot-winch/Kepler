using UnityEngine;

public class CameraScroll : CameraControlScheme
{
    public float sensitivity;
    public float minScroll;
    public float maxScroll;
    
    void Update()
    {
        if(Input.mouseScrollDelta.y != 0)
        {
            ScrollBy(Input.mouseScrollDelta.y * sensitivity);
        }
    }

    public void ScrollBy(float distance)
    {
        Vector3 newPos = transform.position + (transform.forward * distance);
        
        //Using z value as camera is rotated 90deg so y is z and z is y
        if (MathsUtility.Between(newPos.z, minScroll, maxScroll))
        {
            transform.position = newPos;
        }
    }
}
