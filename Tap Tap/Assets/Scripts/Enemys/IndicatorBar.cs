using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Slider secondSlider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;

    private float targetValue;
    [SerializeField] private float speed;
    [SerializeField] private float secondSpeed;

    public void SetMaxValue(float number)
    {
        slider.maxValue = number;
        slider.value = number;

        secondSlider.maxValue = number;
        secondSlider.value = number;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetValue(float number)
    {
        targetValue = number;
    }

    private void Update()
    {
        if (slider.value > targetValue)
        {
            slider.value -= Time.deltaTime * speed;
            fill.color = gradient.Evaluate(secondSlider.normalizedValue);
        }
        else
        {
            slider.value = targetValue;
            fill.color = gradient.Evaluate(secondSlider.normalizedValue);
        }

        if (secondSlider.value > targetValue)
        {
            secondSlider.value -= Time.deltaTime * secondSpeed;
        }
        else
        {
            secondSlider.value = targetValue;
        }
    }
}
