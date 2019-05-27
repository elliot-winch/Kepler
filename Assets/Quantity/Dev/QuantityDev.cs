using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuantityDev : MonoBehaviour
{
    public Quantity sunMass;
    public Quantity earthMass;
    public Quantity bigG;
    public Quantity distance;

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            Quantity gravitationalForce = bigG * earthMass * sunMass / (distance * distance);
            Debug.Log("Force: " + gravitationalForce);
            Debug.Log("Real value: double " + gravitationalForce.TrueValue);
        }
    }
}
