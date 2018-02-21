using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstantiateCirclePopAnim : MonoBehaviour {

    [SerializeField]
    private GameObject CirclePopPrefab;

    [SerializeField]
    private GameObject refToSelfButton;


    // Use this for initialization
    public void AnimateOnClick()
    {
        StartCoroutine(InstantiateAnimAtLocation());
	}
	
    private IEnumerator InstantiateAnimAtLocation()
    {
        //GameObject animInstance = (GameObject)Instantiate(CirclePopPrefab, refToSelfButton.GetComponent<RectTransform>().rect.center, refToSelfButton.transform.rotation);

        GameObject animInstance = (GameObject)Instantiate(CirclePopPrefab, refToSelfButton.transform);

        animInstance.GetComponent<Animator>().speed = 2f;

        yield return new WaitForSeconds(100);

        Destroy(animInstance);

        yield break;
    }

}
