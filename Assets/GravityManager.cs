using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GravityManager : MonoBehaviour
{
    public static GravityManager Instance { get; private set; }

    public float BigG;
    public float lowInclination = 0.01f;

    //public List<GravitationalBody> Bodies = new List<GravitationalBody>();

    public GravitationalBody sun;
    public GravitationalBody earth;

    private void Awake()
    {
        Instance = this;
    }

    private void FixedUpdate()
    {
        float r = Vector3.Distance(earth.transform.position, sun.transform.position);
        float forceMag = (BigG * earth.mass * sun.mass) / (r * r);
        Vector3 forceDir = (sun.transform.position - earth.transform.position).normalized;

        earth.ApplyForce(forceDir * forceMag);

        earth.RevolveAround(sun);

        earth.GetComponent<GravitationBodyUI>()?.Draw(earth);
    }
}
