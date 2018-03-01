using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour {


    public int[] statsList;

    // Use this for initialization
    void Start ()
    {
        statsList = new int[6];

        for (int i = 0; i < statsList.Length; i++)
        {
            statsList[i] = Random.Range(1, 10);
        }
    }
	

}
