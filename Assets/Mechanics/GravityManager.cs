using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GravityManager : MonoBehaviour
{
    public static GravityManager Instance { get; private set; }

    public GravitationalBody sun;

    //TEMP
    public RotatingBody[] rots;

    public float timeScale = 1;

    private void Awake()
    {
        Instance = this;
    }

    private void FixedUpdate()
    {
        double deltaTime = Time.fixedDeltaTime * timeScale;

        sun.UpdateOrbit(deltaTime);

        foreach(var r in rots)
        {
            r.UpdateRotation(deltaTime);
        }
    }
}
