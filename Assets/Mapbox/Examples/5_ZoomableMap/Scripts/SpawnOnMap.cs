﻿namespace Mapbox.Examples
{
	using UnityEngine;
	using Mapbox.Utils;
	using Mapbox.Unity.Map;
	using Mapbox.Unity.MeshGeneration.Factories;
	using Mapbox.Unity.Utilities;
    using System.Collections;
    using System.Collections.Generic;
    using Mapbox.Unity.Location;

    public class SpawnOnMap : MonoBehaviour
	{
		[SerializeField]
		AbstractMap _map;

		[SerializeField]
		[Geocode]
		string[] _locationStrings;
		Vector2d[] _locations;

		[SerializeField]
		float _spawnScale = 100f;

		[SerializeField]
		GameObject _markerPrefab;

		List<GameObject> _spawnedObjects;

        public AbstractLocationProvider AbstractLocationProviderRef;

		void Start()
		{
            _locations = new Vector2d[_locationStrings.Length];

            StartCoroutine (RandomLocations());
		}

		private void Update()
		{
			int count = _spawnedObjects.Count;
			for (int i = 0; i < count; i++)
			{
				var spawnedObject = _spawnedObjects[i];
				var location = _locations[i];
				spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location);
			}
        }

        private IEnumerator RandomLocations()
        {
            yield return new WaitForSeconds(1);

            int RandomLocationCount = 3;

            Vector2d currentLocation = AbstractLocationProviderRef.CurrentLocation.LatitudeLongitude;

            float xDistance = (float)currentLocation.x;
            float yDistance = (float)currentLocation.y;
            float distanceConstant = 0.05f;

            for (int i = 0; i < RandomLocationCount; i++)
            {
                //Create random location and remove annoying (brackets) from either side that mess up the StringToLatLon() function
                _locationStrings[i] = new Vector2(Random.Range(xDistance + distanceConstant, xDistance - distanceConstant), Random.Range(yDistance + distanceConstant, yDistance - distanceConstant)).ToString("F5").Replace("(", "").Replace(")", "");

                Debug.Log("<color=blue>Made a new Random Location: </color>" + _locationStrings[i]);
            }

            _spawnedObjects = new List<GameObject>();
            for (int i = 0; i < _locationStrings.Length; i++)
            {
                var locationString = _locationStrings[i];
                _locations[i] = Conversions.StringToLatLon(locationString);
                var instance = Instantiate(_markerPrefab);
                instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i]);
                instance.transform.localScale = Vector3.one * _spawnScale;
                _spawnedObjects.Add(instance);
            }
        }
	}
}