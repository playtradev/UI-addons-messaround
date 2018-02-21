using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstantiateCirclePopAnim : MonoBehaviour {

    [SerializeField]
    private GameObject CirclePopPrefab;

    [SerializeField]
    private GameObject refToAnimCentre;


    // Use this for initialization
    public void AnimateOnClick()
    {
        StartCoroutine(InstantiateAnimAtLocation());
	}
	
    private IEnumerator InstantiateAnimAtLocation()
    {
        GameObject animInstance = (GameObject)Instantiate(CirclePopPrefab, refToAnimCentre.transform);

        //animInstance.transform.localPosition = new Vector3 (0, 50, -10);

        animInstance.GetComponent<Animator>().speed = 2f;

        yield return new WaitForSeconds(100);

        Destroy(animInstance);

        yield break;
    }

}
