﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchHandler : MonoBehaviour {

    public GameObject selectedEvent;

    // Update is called once per frame
    void Update ()
    {
        //for (int i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetMouseButtonUp(0))
            {
                RaycastHit hit;
                Debug.DrawLine(Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector3(0, -100, 0), Color.magenta, 15f);

                if (Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector3(0, -100, 0), out hit) && hit.transform.tag == "MapEvent")
                {
                    //Deselect Old Event
                    //if (selectedEvent != null)
                    {
                        //selectedEvent.GetComponent<BattleSelected>().StartCoroutine("NewIsNotSelected");
                        //selectedEvent = null;
                    }

                    selectedEvent = hit.collider.gameObject;

                    selectedEvent.GetComponent<BattleSelected>().CallIsSelected();
                }

            }

        }
    }

}
