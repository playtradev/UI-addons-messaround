using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopUpImageScript : MonoBehaviour {

    public GameObject parentEventRef;
	
    public void CancelButton()
    {
        if (parentEventRef != null)
        {
            parentEventRef.GetComponent<BattleSelected>().StartCoroutine("NewIsNotSelected");
        }

    }

    public void FightButton()
    {
        SceneManager.LoadScene("P10_Combat", LoadSceneMode.Single);
    }
}
