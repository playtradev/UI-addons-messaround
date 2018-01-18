    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class EnableMapScroll : MonoBehaviour
    {
        public GameObject moveScriptRef;
        public GameObject scrollSnapRef;

        public void EnableScroll()
        {
            if (scrollSnapRef.GetComponent<UnityEngine.UI.Extensions.VerticalScrollSnap>().CurrentPage == 1)
            {
            moveScriptRef.GetComponent<Mapbox.Examples.QuadTreeCameraMovement>().enabled = true;
            }
            else
            {
            moveScriptRef.GetComponent<Mapbox.Examples.QuadTreeCameraMovement>().enabled = false;
            }
        }

    }

