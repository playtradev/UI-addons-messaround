using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaceLerp : MonoBehaviour {

    [Header("References")]
    [SerializeField]
    private GameObject FaceBackgroundRef;
    [SerializeField]
    private RectTransform faceRT;
    [SerializeField]
    private GameObject faceImageRef;
    [SerializeField]
    private Sprite[] faceSpriteList;

    [Header("Lerp Controls")]
    public float lerpSmoothness = 0.01f;
    public float lerpDuration = 1.0f;


    private void Start()
    {
        faceRT = FaceBackgroundRef.GetComponent<RectTransform>();
    }

    public void CallShrinkFace()
    {
        StopAllCoroutines();
        StartCoroutine(ShrinkFace());
    }

    public void CallShowFace()
    {
        StopAllCoroutines();
        StartCoroutine(ShowFace());
    }

    private IEnumerator ShrinkFace()
    {
        //Set sleepy Face
        faceImageRef.GetComponent<Image>().sprite = faceSpriteList[0];
        faceImageRef.GetComponent<Image>().transform.localScale = new Vector3(4.896619f, 4.896623f, 4.896623f);

        Vector2 smallFace = new Vector2(0f, 105f);

        float progress = 0;
        float increment = lerpSmoothness / lerpDuration;

        while (progress < 1)
        {
            //Position Lerp
            faceRT.sizeDelta = Vector3.Lerp(faceRT.sizeDelta, smallFace, progress);

            progress += increment;
            yield return new WaitForSeconds(lerpSmoothness);
        }
    }

    private IEnumerator ShowFace()
    {
        //Set angry Face
        faceImageRef.GetComponent<Image>().sprite = faceSpriteList[1];
        faceImageRef.GetComponent<Image>().transform.localScale = new Vector3(3.937256f, 3.937256f, 3.937256f);

        Vector2 bigFace = new Vector2(0f, 300f);

        float progress = 0;
        float increment = lerpSmoothness / lerpDuration;

        while (progress < 1)
        {
            //Position Lerp
            faceRT.sizeDelta = Vector3.Lerp(faceRT.sizeDelta, bigFace, progress);

            progress += increment;
            yield return new WaitForSeconds(lerpSmoothness);
        }

    }
}
