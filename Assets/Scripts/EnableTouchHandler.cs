    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class EnableTouchHandler : MonoBehaviour
    {
        public GameObject eventSystemRef;
        public GameObject scrollSnapRef;

        //This is triggered by VerticalScrollSnap 'On Selection Page Changed Event'
        public void EnableMapTouch()
        {
            if (scrollSnapRef.GetComponent<UnityEngine.UI.Extensions.VerticalScrollSnap>().CurrentPage == 1)
            {
                //Enable Raycasting Touch Handler script
                eventSystemRef.GetComponent<TouchHandler>().enabled = true;
            }
            else
            {
                eventSystemRef.GetComponent<TouchHandler>().enabled = false;
            }
        }

    }
