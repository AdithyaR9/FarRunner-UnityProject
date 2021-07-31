using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        //Setting Gradient to Highest Value-Color
        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        slider.value = health;

        //Setting Gradient to Color Equivalent to Health 
        fill.color = gradient.Evaluate(slider.normalizedValue);         //normalizedValue is the regular value but between 0 and 1
    }

}
