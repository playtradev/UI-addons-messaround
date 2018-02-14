using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AI_Script : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private BattleManagerScript BattleManagerRef;
    [SerializeField]
    private Button p2EndTurnButtonRef;


    public int[] p2Attack;
    public int[] p2Defence;

    private int tempDefenceCount;
    private int tempAttackCount;



    private void Start()
    {
        p2Attack = new int[] { 0, 0, 0 };
        p2Defence = new int[] { 0, 0, 0 };

        //Get current values
        for (int t = 0; t < BattleManagerRef.seedList2.Length; t++)
        {
            p2Attack[t] = BattleManagerRef.seedList2[t].GetComponent<SeedScript>().attackVal;
            p2Defence[t] = BattleManagerRef.seedList2[t].GetComponent<SeedScript>().defenceVal;
        }
    }

    public void StartAIActionPhase()
    {
        StartCoroutine ("AIActionPhase");
    }

    public void StartAIReactionPhase()
    {
        StartCoroutine("AIReactionPhase");
    }

    public IEnumerator AIActionPhase()
    {
        Debug.Log("<color=blue>Begin </color>");

        for (int t = 0; t < BattleManagerRef.seedList2.Length; t++)
        {
            //If attack higher, set attack
            if (BattleManagerRef.seedList2[t].GetComponent<SeedScript>().attackVal > BattleManagerRef.seedList2[t].GetComponent<SeedScript>().defenceVal)
            {
                if (BattleManagerRef.seedList2[t].transform.Find("AttackType").GetComponentInChildren<Text>().text == "Attack")
                {
                    Debug.Log("<color=blue>Attack </color>");
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    StartCoroutine(ClickUntil(BattleManagerRef.seedList2[t], "Attack"));
                    yield return new WaitUntil(() => BattleManagerRef.seedList2[t].transform.Find("AttackType").GetComponentInChildren<Text>().text == "Attack");
                }
            }

            //If defence higher, set defence
            else
            {
                if (BattleManagerRef.seedList2[t].transform.Find("AttackType").GetComponentInChildren<Text>().text == "Defend")
                {
                    Debug.Log("<color=blue>Def </color>");
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    StartCoroutine(ClickUntil(BattleManagerRef.seedList2[t], "Defend"));
                    yield return new WaitUntil(() => BattleManagerRef.seedList2[t].transform.Find("AttackType").GetComponentInChildren<Text>().text == "Defend");
                }
            }
        }

        yield return new WaitForSeconds(2f);

        //Press End Turn Button
        p2EndTurnButtonRef.onClick.Invoke();
        Debug.Log("<color=yellow>END TURN CLICK </color>");
    }

    private IEnumerator AIReactionPhase()
    {

        //If enemy attack is low, go full attack
        if ((BattleManagerRef.damageCount1 < p2Defence[0]) && (BattleManagerRef.damageCount1 < p2Defence[1]) && (BattleManagerRef.damageCount1 < p2Defence[2]))
        {
            StartCoroutine("AIReactionMassiveAttack");
        }

        else
        {
            StartCoroutine("AIReactionDefenceLoop");

        }

            yield return 0;
    }

    private IEnumerator AIReactionMassiveAttack()
    {
        Debug.Log("<color=blue>MASSIVE ATTACK </color>");
        yield return 0;
    }


    private IEnumerator AIReactionDefenceLoop()
    {
        int maxDefVal = p2Defence[0];
        int maxDefValIndex = 0;

        //Find max Defence stat
        for (int t = 0; t < p2Defence.Length; t++)
        {
            if (p2Defence[t] > maxDefVal)
            {
                maxDefVal = p2Defence[t];
                maxDefValIndex = t;
            }
        }

        //Set the maxDef Seed to defend + remove values from BOTH arrays

        //If already correct, Set and remove seed values from array.
        if (BattleManagerRef.seedList2[maxDefValIndex].transform.Find("AttackType").GetComponentInChildren<Text>().text == "Defend")
        {
            Debug.Log("<color=blue>MAX DEFENCE SET ON SEED </color>" + BattleManagerRef.seedList2[maxDefValIndex]);
            tempDefenceCount += p2Defence[maxDefValIndex];
            p2Defence[maxDefValIndex] = 0;
            p2Attack[maxDefValIndex] = 0;
            yield return new WaitForSeconds(1f);
        }
        //Or, Invoke click and restart
        else
        {
            Debug.Log("<color=red> MAX DEF *click* + rerun </color>");
            BattleManagerRef.seedList2[maxDefValIndex].onClick.Invoke();
            yield return new WaitForSeconds(1f);
            StartCoroutine("AIReactionDefenceLoop");
            yield break;
        }

        if (BattleManagerRef.damageCount1 > tempDefenceCount)
        {
            Debug.Log("<color=red>damage > def, looping </color>");
            StartCoroutine("AIReactionDefenceLoop");
            yield break;
        }
        else if (BattleManagerRef.damageCount1 <= tempDefenceCount)
        {

        }
    }




    private IEnumerator ClickUntil(Button b, string s)
    {
        yield return new WaitForSeconds(1f);
        b.onClick.Invoke();
        yield return new WaitForSeconds(1f);

        if (b.transform.Find("AttackType").GetComponentInChildren<Text>().text != s)
        {
            StartCoroutine(ClickUntil(b, s));
            yield break;
        }

        else if (b.transform.Find("AttackType").GetComponentInChildren<Text>().text == s)
        {
            Debug.Log("<color=blue>Clicked </color>"+ b + " until it was " + s);
            yield break;
        }

    }
}


	

