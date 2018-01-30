using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour {

    private float fillAmount;

    [SerializeField]
    private float lerpSpeed;

    [SerializeField]
    private Image healthBar;

    [SerializeField]
    private Text valueText;

    public float MaxVal { get; set; }

    private void Update()
    {
        HandleBar();
    }

    public float Value
    {
        set
        {
            string[] tmp = valueText.text.Split(':');
            valueText.text = tmp[0] + ": " + value;
            fillAmount = HpConvert(value, 0, MaxVal, 0, 1);
        }
    }

    private void HandleBar()
    {
        if (fillAmount != healthBar.fillAmount)
        {
            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, fillAmount, Time.deltaTime * lerpSpeed);
        }
    }

    private float HpConvert(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

}
