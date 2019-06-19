using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CelestialSelectable : MonoBehaviour
{
    public ViewLevel level;
    public SciNumber distanceScale;
    public GravitationalBody gravitationalBody { get; private set; }

    private void Awake()
    {
        gravitationalBody = GetComponentInParent<GravitationalBody>();
    }

    /*
    //public void DisplayFacts();
    protected GravitationalBody gravBody;

    private void Start()
    {
        gravBody = GetComponentInParent<GravitationalBody>();
    }

    public void EnterDetailView()
    {

    }

    public void ExitDetailView()
    {

    }

    protected abstract void RecenterSimulation();
    protected abstract void PositionCamera();
    protected abstract void SetUI(bool active);
    */

}
