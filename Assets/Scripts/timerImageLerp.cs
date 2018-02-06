using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timerImageLerp : MonoBehaviour {

    [SerializeField]
    private Image timerImage;

    [SerializeField]
    private GameObject battleManagerRef;

    private float time;

    private void Start()
    {
        time = 15;
    }

	// Update is called once per frame
	void Update ()
    {
        //TODO this is hacky
        timerImage.fillAmount = Mathf.Lerp(timerImage.fillAmount, (float)((battleManagerRef.GetComponent<BattleManagerScript>().timeRemaining) - 1) / battleManagerRef.GetComponent<BattleManagerScript>().timerDuration, Time.deltaTime * 2);
    }
}
