﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSelected : MonoBehaviour {

    [Header("References")]
    public GameObject refToSelfPOI;
    [SerializeField]
    private GameObject refToOwnSprite;
    [SerializeField]
    private GameObject popUpImageRef;
    [SerializeField]
    private GameObject popUp;


    private Vector3 originalPosition;

    [Header("IsSelected Animation Lerp")]
    public float lerpDuration = 0.3f;
    public float lerpSmoothness = 0.01f;

    [Header("References")]
    [SerializeField]
    private GameObject MapRef;
    [SerializeField]
    private GameObject EventSystemRef;
    [SerializeField]
    private GameObject AlphaMaskRef;
    [SerializeField]
    private GameObject FaceBackgroundRef;




    private void Start()
    {
        //TODO this is hacky, and bad practice
        MapRef = GameObject.Find("Map");
        EventSystemRef = GameObject.Find("EventSystem");
        AlphaMaskRef = GameObject.Find("AlphaMask");
        FaceBackgroundRef = GameObject.Find("FaceBackground");
    }

    //Update() NO LONGER NEEDED AS MAP MOVEMENT CURRENTLY FROZEN WHEN POI IS SELECTED + other old functions
    /*
    private void Update()
    {
        if (popUp != null)
        {
            //popUp.transform.position = GameObject.Find("Main Camera").GetComponent<Camera>().WorldToScreenPoint(refToOwnSprite.transform.position);
        }
    }
    

    public void OldIsSelected()
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

    public void OldIsNotSelected()
    {
 
        refToOwnSprite.GetComponent<SpriteRenderer>().color = Color.white;

        if (popUp != null)
        {
            Destroy(popUp);
        }
    }
    */

    public void CallIsSelected()
    {
        StopAllCoroutines();
        StartCoroutine(IsSelected());
    }

    public void CallIsNotSelected()
    {
        StopAllCoroutines();
        StartCoroutine(IsNotSelected());
    }

    private IEnumerator IsSelected()
    {
        //Disable Map Scroll + Raycast Touchhandler
        MapRef.GetComponent<EnableMapScroll>().DisableScroll();
        EventSystemRef.GetComponent<EnableTouchHandler>().DisableMapTouch();

        //Lerp Face
        FaceBackgroundRef.GetComponent<FaceLerp>().CallShowFace();

        //Set Red eyes
        refToOwnSprite.GetComponent<SpriteRenderer>().color = Color.red;

        //Instantiate PopUp
        popUp = Instantiate(popUpImageRef, GameObject.Find("Canvas").transform) as GameObject;

        //Save position
        originalPosition = refToOwnSprite.transform.position;

        //Set popUp parent Reference
        popUp.GetComponent<PopUpImageScript>().parentEventRef = refToSelfPOI;

        //Lerp variables
        float progress = 0;
        float increment = lerpSmoothness / lerpDuration;
        

        while (progress < 1)
        {
            //Position Lerp
            refToOwnSprite.transform.position = Vector3.Lerp(refToSelfPOI.transform.position, new Vector3(0, 5, 0), progress);
            //Scale Lerp
            refToOwnSprite.transform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(4, 4, 4), progress);
            //AlphaMask Lerp
            AlphaMaskRef.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.clear, new Color(1, 1, 1, 0.7f), progress);


            progress += increment;
            yield return new WaitForSeconds(lerpSmoothness);
        }


    }

    private IEnumerator IsNotSelected()
    {
        //Delete Pop Up
        if (popUp != null)
        {
            Destroy(popUp);
        }

        //Enable Map Scroll + Raycast Touchhandler
        MapRef.GetComponent<EnableMapScroll>().EnableScroll();
        EventSystemRef.GetComponent<EnableTouchHandler>().EnableMapTouch();

        //Set White eyes
        refToOwnSprite.GetComponent<SpriteRenderer>().color = Color.white;

        //Lerp Face
        FaceBackgroundRef.GetComponent<FaceLerp>().CallShrinkFace();

        //Get Current Position
        Vector3 currentPosition = refToOwnSprite.transform.position;

        //Lerp variables
        float progress = 0;
        float increment = lerpSmoothness / lerpDuration;

        while (progress < 1)
        {
            //Position Lerp
            refToOwnSprite.transform.position = Vector3.Lerp(currentPosition, refToSelfPOI.transform.position, progress);
            //Scale Lerp
            refToOwnSprite.transform.localScale = Vector3.Lerp(refToOwnSprite.transform.localScale, new Vector3(1, 1, 1), progress);
            //AlphaMask Lerp
            AlphaMaskRef.GetComponent<SpriteRenderer>().color = Color.Lerp(AlphaMaskRef.GetComponent<SpriteRenderer>().color, Color.clear, progress);

            progress += increment;
            yield return new WaitForSeconds(lerpSmoothness);
        }


        //Offset slightly so Sprite doesn't clip through map
        refToOwnSprite.transform.position = new Vector3(refToOwnSprite.transform.position.x, 1, refToOwnSprite.transform.position.z);
    }
}


