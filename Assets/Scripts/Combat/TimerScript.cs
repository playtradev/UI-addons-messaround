using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour {

    public int timeRemaining;
    public int timerDuration = 15;
    [SerializeField]
    private Text timerText;
    [SerializeField]
    private bool isCountingDown = false;

    //Timer functions
    public void BeginTimer()
    {
        if (!isCountingDown)
        {
            CancelInvoke("_tick");
            isCountingDown = true;
            timeRemaining = timerDuration;
            Invoke("_tick", 1f);
        }
        else if (isCountingDown)
        {
            CancelInvoke("_tick");
            timeRemaining = timerDuration;
            Invoke("_tick", 1f);
        }
        timerText.text = timeRemaining.ToString();
    }

    public void StopTimer()
    {
        isCountingDown = false;
        CancelInvoke("_tick");
    }

    private void _tick()
    {
        timeRemaining--;
        if (timeRemaining > 0)
        {
            Invoke("_tick", 1f);
        }
        else
        {
            isCountingDown = false;
        }
        timerText.text = timeRemaining.ToString();
        //(float)timeRemaining / timerDuration;
    }

}
