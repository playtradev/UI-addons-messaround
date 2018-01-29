using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSelected : MonoBehaviour {

    [Header("References")]
    public GameObject refToSelfPOI;
    public GameObject refToOwnSprite;
    public GameObject popUpImageRef;
    public GameObject popUp;

    private void Update()
    {
        if (popUp != null)
        {
            popUp.transform.position = GameObject.Find("Main Camera").GetComponent<Camera>().WorldToScreenPoint(refToOwnSprite.transform.position);
        }
    }

    public void IsSelected()
    {
        refToOwnSprite.GetComponent<SpriteRenderer>().color = Color.red;

        //Get on-screen position.
        Vector3 screenPos = GameObject.Find("Main Camera").GetComponent<Camera>().WorldToScreenPoint(refToOwnSprite.transform.position);

        //Instantiate PopUp
        popUp = Instantiate(popUpImageRef, screenPos, Quaternion.identity, GameObject.Find("Canvas").transform) as GameObject;

        //Set Scale, seems to be 100 for some reason.
        popUp.transform.localScale = new Vector3(100, 100, 0);

        //Set popUp parent Reference
        popUp.GetComponent<PopUpImageScript>().parentEventRef = refToSelfPOI;
    }

    public void IsNotSelected()
    {
 
        refToOwnSprite.GetComponent<SpriteRenderer>().color = Color.white;

        if (popUp != null)
        {
            Destroy(popUp);
        }
    }
}
