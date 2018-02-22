    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class EnableMapScroll : MonoBehaviour
    {
        public GameObject moveScriptRef;
        public GameObject scrollSnapRef;

        //This is triggered automatically by VerticalScrollSnap 'On Selection Page Changed Event'
        public void EnableScroll()
        {
            if (scrollSnapRef.GetComponent<UnityEngine.UI.Extensions.VerticalScrollSnap>().CurrentPage == 1)
            {
                //Enable the Map Scroll (camera movement) Script
                moveScriptRef.GetComponent<Mapbox.Examples.QuadTreeCameraMovement>().enabled = true;
            }
            else
            {
                moveScriptRef.GetComponent<Mapbox.Examples.QuadTreeCameraMovement>().enabled = false;
            }
        }

        //To be triggered by IsSelected() from BattleSelected.cs
        public void DisableScroll()
        {
            moveScriptRef.GetComponent<Mapbox.Examples.QuadTreeCameraMovement>().enabled = false;
        }

    }

