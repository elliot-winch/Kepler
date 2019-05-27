using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GravityManager : MonoBehaviour
{
    public static GravityManager Instance { get; private set; }

    public GravitationalBody sun;
    public List<GravitationalBody> planets;

    public float timeScale = 1;

    private void Awake()
    {
        Instance = this;
    }

    private void FixedUpdate()
    {
        planets.ForEach(p => p.UpdateOrbit(Time.fixedDeltaTime * timeScale));
    }
}
