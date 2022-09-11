using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    public Slider m_Slider;

    public void SetSliderVal(float health)
    {
        m_Slider.maxValue = health;
        m_Slider.value = health;
    }

    public void UpdateSlider(float value)
    {
        m_Slider.value = value;
    }
}
