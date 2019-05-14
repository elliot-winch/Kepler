using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField] List<Unit> units;

    public List<Unit> AllUnits => units;
}
