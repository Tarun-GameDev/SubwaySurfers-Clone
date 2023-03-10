using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class powerUpSlider : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Gradient gradient;
    [SerializeField] Image fill;
    [SerializeField] Image handle;

    public void MaxTime(float _time)
    {
        slider.maxValue = _time;
        slider.value = _time;

        fill.color = gradient.Evaluate(1f);

    }

    public void SetTime(float _time)
    {
        slider.value = _time;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void SetHandleImage(Sprite _icon)
    {
        handle.sprite = _icon;
    }
}
