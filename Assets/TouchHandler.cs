using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        //for (int i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Construct a ray from the current touch coordinates
                //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                //Vector2 vectorRay = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

                //RaycastHit2D rayHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.down);

                RaycastHit2D rayHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector2(0, -100));

                Debug.DrawRay(Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector2(0, -100), Color.yellow, 20f);

                //Debug.DrawRay(Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector3(0, -100, 1), Color.yellow, 10f);

                Debug.Log("<color=green>Raycast is shooting...</color>" + rayHit.collider);

                // Do something if hit
                if (rayHit.collider != null)
                {
                    Debug.Log("<color=teal>Raycast hit! </color>" + rayHit.collider);
                }

                //&& hit.transform.tag == "Dynamic")

            }

        }
    }
}
