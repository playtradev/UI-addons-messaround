using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStartupAnim : MonoBehaviour {

    public float animWaitTime;

    [SerializeField]
    private BattleManagerScript battleManagerRef; 
	
	// Update is called once per frame
	public void StartupAnimations ()
    {
        StartCoroutine(StartupAnimLoop());
    }

    private IEnumerator StartupAnimLoop()
    {
        for (int t = 0; t < battleManagerRef.seedList1.Length; t++)
        {
            battleManagerRef.seedList1[t].GetComponent<SeedScript>().StartupAnim();

            yield return new WaitForSeconds(animWaitTime);
        }

        for (int t = 0; t < battleManagerRef.seedList2.Length; t++)
        {
            battleManagerRef.seedList2[t].GetComponent<SeedScript>().StartupAnim();

            yield return new WaitForSeconds(animWaitTime);
        }

        yield break;
    }


}
