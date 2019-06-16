using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineDispatch : MonoBehaviour
{
    public static CoroutineDispatch Instance;

    private void Awake()
    {
        Instance = this;
    }
}
