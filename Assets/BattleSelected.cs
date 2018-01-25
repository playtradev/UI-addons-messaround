using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSelected : MonoBehaviour {

    public GameObject refToOwnSprite;
    public GameObject popUpImageRef;
    private GameObject popUp;

    public void IsSelected()
    {
        refToOwnSprite.GetComponent<SpriteRenderer>().color = Color.red;

        Vector3 screenPos = GameObject.Find("Main Camera").GetComponent<Camera>().WorldToScreenPoint(refToOwnSprite.transform.position);

        popUp = Instantiate(popUpImageRef, screenPos, Quaternion.identity, GameObject.Find("Canvas").transform) as GameObject;
            
        popUp.transform.localScale = new Vector3(100, 100, 0);
    }

    public void IsNotSelected()
    {
        refToOwnSprite.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
