using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScalePresetButton : MonoBehaviour
{
    public TimeScaleUI ui;

    public void PresetTime(float timescale)
    {
        ui.SetTimeScale(timescale);
    }
}
