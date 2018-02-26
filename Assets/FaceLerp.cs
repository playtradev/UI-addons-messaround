using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceLerp : MonoBehaviour {

    [SerializeField]
    private GameObject FaceBackgroundRef;
    [SerializeField]
    private RectTransform faceRT;

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
