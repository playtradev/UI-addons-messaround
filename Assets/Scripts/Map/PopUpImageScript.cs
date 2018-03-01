using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PopUpImageScript : MonoBehaviour {

    [Header("GameObject References")]
    public GameObject parentEventRef;
    public GameObject persistentInfoRef;

    [Header("Text References")]
    [SerializeField]
    private Text eventTextRef;
    [SerializeField]
    private Text mightTextRef;
    [SerializeField]
    private Text mindTextRef;
    [SerializeField]
    private Text magicTextRef;

    [Header("Seed List Array")]
    [SerializeField]
    private GameObject[] seedList;


    private void Start()
    {
        persistentInfoRef = GameObject.Find("PersistentInfo");

        mightTextRef.text = "ATT: " + parentEventRef.GetComponent<EnemyStats>().statsList[0] + " | DEF: " + parentEventRef.GetComponent<EnemyStats>().statsList[1];
        mindTextRef.text = "ATT: " + parentEventRef.GetComponent<EnemyStats>().statsList[2] + " | DEF: " + parentEventRef.GetComponent<EnemyStats>().statsList[3];
        magicTextRef.text = "ATT: " + parentEventRef.GetComponent<EnemyStats>().statsList[4] + " | DEF: " + parentEventRef.GetComponent<EnemyStats>().statsList[5];

        SetBaddieLevelText();
        AnimationCoruotineHandler();
    }

    public void CancelButton()
    {
        if (parentEventRef != null)
        {
            parentEventRef.GetComponent<BattleSelected>().CallIsNotSelected();
        }

    }

    public void FightButton()
    {
        persistentInfoRef.GetComponent<PersistentScript>().enemyStats = parentEventRef.GetComponent<EnemyStats>().statsList;

        SceneManager.LoadScene("P10_Combat", LoadSceneMode.Single);
    }

    private void SetBaddieLevelText()
    {
        float statsSum = 0;
        int baddieLevel = 0;

        //Add up sum of values
        for (int i = 0; i < parentEventRef.GetComponent<EnemyStats>().statsList.Length; i++)
        {
            statsSum += parentEventRef.GetComponent<EnemyStats>().statsList[i];
        }

        //Little bit of Maths to work out Level
        baddieLevel = Mathf.FloorToInt(statsSum / 10);

        //Set text
        eventTextRef.text = "It's a Level " + baddieLevel + " Baddie!";
    }

    private void AnimationCoruotineHandler()
    {
        StopAllCoroutines();
        StartCoroutine(FloatyAnimation());
    }

    private IEnumerator FloatyAnimation()
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < seedList.Length; i++)
        {
            seedList[i].GetComponent<Animator>().Play("Idle");
            seedList[i].GetComponent<Animator>().Play("SeedFloat");

            yield return new WaitForSeconds(0.3f);
        }

        AnimationCoruotineHandler();
    }

}
