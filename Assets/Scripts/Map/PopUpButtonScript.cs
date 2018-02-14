using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpButtonScript : MonoBehaviour {

    public GameObject popUpImagePrefab;
    public GameObject refToMap;
    public GameObject refToButton;

    //Transform placePointTransform = refToButton.transform;

    // Use this for initialization
    void Start ()
    {

	}
	
    public void SpawnPopUp ()
    {      
        GameObject PopUp = Instantiate(popUpImagePrefab, refToMap.transform) as GameObject;

        Debug.Log("<color=red>Fatal error:</color> refButton location=" + refToButton.transform.position);

        Vector3 offset = new Vector3(0, 1, 0);

        PopUp.transform.position = refToButton.transform.position + offset;

        //PopUp.transform.Translate(Vector3.up * 8, Space.World);
        //PopUp.transform.localPosition = new Vector3(0, 12, 0);
    }
}
