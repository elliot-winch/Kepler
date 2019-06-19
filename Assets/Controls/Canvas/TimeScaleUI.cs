using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class TimeScaleUI : MonoBehaviour
{
    public Text timeScaleLabel;
    public int maxTimeScaleExponent = 8;

    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();

        slider.minValue = 0;
        slider.maxValue = maxTimeScaleExponent;

        SetTimeScale(1f);

        slider.onValueChanged.AddListener((v) =>
        {
            SetTimeScale(SciNumber.EstimateExponential(v), setSlider: false);
        });
    }

    public void SetTimeScale(float timeScale, bool setSlider = true)
    {
        SimulationManager.Instance.timeScale = timeScale;

        timeScaleLabel.text = "Time Scale:\n" + timeScale + " Seconds Per Second";

        if (setSlider)
        {
            slider.value = SciNumber.EstimateLog(timeScale);
        }
    }
}
