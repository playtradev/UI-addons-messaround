using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchHandler : MonoBehaviour {

    public GameObject selectedEvent;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        //for (int i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetMouseButtonUp(0))
            {
                //Deselect Old Event
                if (selectedEvent != null)
                {
                    selectedEvent.GetComponent<BattleSelected>().IsNotSelected();
                    selectedEvent = null;
                }

                RaycastHit hit;
                Debug.DrawLine(Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector3(0, -100, 0), Color.magenta, 15f);

                if (Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector3(0, -100, 0), out hit) && hit.transform.tag == "MapEvent")
                {                
                    Debug.Log("<color=teal>Raycast hit! </color>" + hit.collider.gameObject.GetInstanceID());

                    selectedEvent = hit.collider.gameObject;

                    selectedEvent.GetComponent<BattleSelected>().IsSelected();
                }

            }

        }
    }

}
