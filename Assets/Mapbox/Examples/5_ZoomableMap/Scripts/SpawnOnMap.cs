namespace Mapbox.Examples
{
	using UnityEngine;
	using Mapbox.Utils;
	using Mapbox.Unity.Map;
	using Mapbox.Unity.MeshGeneration.Factories;
	using Mapbox.Unity.Utilities;
	using System.Collections.Generic;

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

		void Start()
		{
            RandomLocations();

            _locations = new Vector2d[_locationStrings.Length];

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

        private void RandomLocations()
        {

            int RandomLocationCount = 3;

            for (int i = 0; i < RandomLocationCount; i++)
            {
                _locationStrings[i] = new Vector2(Random.Range(1.34074873697503f, 1.34074873697503f), Random.Range(103.812385905052f, 103.812385905052f)).ToString("F6").Replace("(", "").Replace(")", "");

                Debug.Log("<color=blue>Made a new Random Location: </color>" + _locationStrings[i]);
            }
        }
	}
}