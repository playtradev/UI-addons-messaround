using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timerImageLerp : MonoBehaviour {

    [SerializeField]
    private Image timerImage;

    [SerializeField]
    private TimerScript timerScript;

    private float time;

    private void Start()
    {
        time = 15;
    }

	// Update is called once per frame
	void Update ()
    {
        //TODO fix this, this is hacky
        timerImage.fillAmount = Mathf.Lerp(timerImage.fillAmount, (float)((timerScript.timeRemaining) - 1) / timerScript.timerDuration, Time.deltaTime * 2);
    }
}
