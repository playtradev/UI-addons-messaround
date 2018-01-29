namespace Mapbox.Unity.Map
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Mapbox.Unity.Utilities;
    using Mapbox.Unity.Location;

    public class gpsScript : MonoBehaviour
    {
        public GameObject mapObjectReference;
        public GameObject PlayerRef;

        public AbstractMap map;
        public AbstractLocationProvider editorLocationProvider;
        public AbstractLocationProvider deviceLocationProvider;

        float duration = 1;
        float smoothness = 0.01f;

        Mapbox.Utils.Vector2d newLocation = Conversions.StringToLatLon("37.120196, -80.223586");

        public void StartRecentre()
        {
            StartCoroutine(RecentreMap());
        }

        public IEnumerator RecentreMap()
        {
            //Mapbox.Utils.Vector2d currentRealLocation = PlayerRef.transform.position.GetGeoPosition (map.CenterMercator, map.WorldRelativeScale);
          
            //TODO make conditional for editor / phone UNTIL THEN, just pick n choose

            //Mapbox.Utils.Vector2d currentRealLocation = editorLocationProvider.CurrentLocation.LatitudeLongitude;
            Mapbox.Utils.Vector2d currentRealLocation = deviceLocationProvider.CurrentLocation.LatitudeLongitude;

            Mapbox.Utils.Vector2d cameraLocation = mapObjectReference.GetComponent<QuadTreeBasicMap>().CenterLatitudeLongitude;

            Vector3 currentScale = mapObjectReference.transform.localScale;
            Vector3 defaultScale = new Vector3 (1, 1, 1);

            float currentZoom = mapObjectReference.GetComponent<QuadTreeBasicMap>().Zoom;
            float defaultZoom = 12f;

            Debug.Log("<color=red>cam location:</color>" + cameraLocation);
            Debug.Log("<color=red>location provider CL :</color>" + currentRealLocation);
        
            float progress = 0;
            float increment = smoothness / duration;

            while (progress < 1)
            {
                //Lerp Zoom
                mapObjectReference.GetComponent<QuadTreeBasicMap>().SetZoom(Mathf.Lerp(currentZoom, defaultZoom, progress));
                //Lerp Position
                mapObjectReference.GetComponent<QuadTreeBasicMap>().SetCenterLatitudeLongitude(Mapbox.Utils.Vector2d.Lerp(cameraLocation, currentRealLocation, progress));
                //Lerp Scale
                mapObjectReference.transform.localScale = (Vector3.Lerp(currentScale, defaultScale, progress));

                progress += increment;
                yield return new WaitForSeconds(smoothness);
            }
            
            yield return null;
            
        }

    }
}
