using System.Collections;
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

    [Header("IsSelected Animation Lerp")]
    public float lerpDuration = 1f;
    public float lerpSmoothness = 0.01f;
    private Vector3 originalPosition;
    [SerializeField]
    private GameObject MapRef;
    [SerializeField]
    private GameObject EventSystemRef;

    private void Start()
    {
        //TODO this is hacky, and bad practice
        MapRef = GameObject.Find("Map");
        EventSystemRef = GameObject.Find("EventSystem");
    }

    private void Update()
    {
        if (popUp != null)
        {
            //popUp.transform.position = GameObject.Find("Main Camera").GetComponent<Camera>().WorldToScreenPoint(refToOwnSprite.transform.position);
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

    public IEnumerator NewIsSelected()
    {
        //Disable Map Scroll + Raycast Touchhandler
        MapRef.GetComponent<EnableMapScroll>().DisableScroll();
        EventSystemRef.GetComponent<EnableTouchHandler>().DisableMapTouch();

        //Set Red eyes
        refToOwnSprite.GetComponent<SpriteRenderer>().color = Color.red;

        //Save location
        originalPosition = refToOwnSprite.transform.position;

        //Instantiate PopUp
        popUp = Instantiate(popUpImageRef, GameObject.Find("Canvas").transform) as GameObject;

        //Set popUp parent Reference
        popUp.GetComponent<PopUpImageScript>().parentEventRef = refToSelfPOI;

        //Lerp variables
        float progress = 0;
        float increment = lerpSmoothness / lerpDuration;

        while (progress < 1)
        {
            //Position Lerp
            refToOwnSprite.transform.position = Vector3.Lerp(originalPosition, new Vector3(0, 5, 0), progress);
            //Scale Lerp
            refToOwnSprite.transform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(4, 4, 4), progress);

            progress += increment;
            yield return new WaitForSeconds(lerpSmoothness);
        }


    }

    public IEnumerator NewIsNotSelected()
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

        //Get Current Position
        Vector3 currentPosition = refToOwnSprite.transform.position;

        //Lerp variables
        float progress = 0;
        float increment = lerpSmoothness / lerpDuration;

        while (progress < 1)
        {
            //Position Lerp
            refToOwnSprite.transform.position = Vector3.Lerp(currentPosition, originalPosition, progress);
            //Scale Lerp
            refToOwnSprite.transform.localScale = Vector3.Lerp(new Vector3(4, 4, 4), new Vector3(1, 1, 1), progress);

            progress += increment;
            yield return new WaitForSeconds(lerpSmoothness);
        }

    }
}


