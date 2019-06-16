using UnityEngine;

[RequireComponent(typeof(Camera))]
public abstract class CameraControlScheme : MonoBehaviour
{
    protected new Camera camera;

    protected virtual void Awake()
    {
        camera = GetComponent<Camera>();
    }
}
