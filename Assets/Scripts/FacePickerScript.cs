using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FacePickerScript : MonoBehaviour {

    public Image currentFace;
    public Sprite[] spriteList;
    int i = 0;

    // Use this for initialization
    void Start ()
    {
        currentFace.sprite = spriteList[i];
    }
	
    public void NextFace()
    {
        currentFace.sprite = spriteList[i++];

        Debug.Log("<color=red>int i number:</color>" + i);
    }
}
