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
    private int SeedsSet;

    [Header("Sub-Buttons")]
    [SerializeField]
    private Button Might_Supp_1;
    [SerializeField]
    private Button Might_Supp_2;
    [SerializeField]
    private Button Mind_Supp_1;
    [SerializeField]
    private Button Mind_Supp_2;


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

        for (int t = 0; t < BattleManagerRef.seedList2.Length; t++)
        {
            //Deploy Might support Seed
            if ((t == 0)
                && ((BattleManagerRef.seedList2[0].GetComponent<SeedScript>().attackVal < (Mathf.CeilToInt(BattleManagerRef.seedList2[1].GetComponent<SeedScript>().attackVal) * 1.5))
                || (BattleManagerRef.seedList2[0].GetComponent<SeedScript>().attackVal < (Mathf.CeilToInt(BattleManagerRef.seedList2[2].GetComponent<SeedScript>().attackVal) * 1.5))))
            {

                if (BattleManagerRef.seedList2[1].GetComponent<SeedScript>().attackVal >= BattleManagerRef.seedList2[2].GetComponent<SeedScript>().attackVal)
                {
                StartCoroutine(ClickUntil(BattleManagerRef.seedList2[1], "Support"));
                yield return new WaitUntil(() => BattleManagerRef.seedList2[1].transform.Find("AttackType").GetComponentInChildren<Text>().text == "Support");
                yield return new WaitForSeconds(1f);
                Might_Supp_1.onClick.Invoke();
                }
                else
                {
                StartCoroutine(ClickUntil(BattleManagerRef.seedList2[t], "Support"));
                yield return new WaitUntil(() => BattleManagerRef.seedList2[t].transform.Find("AttackType").GetComponentInChildren<Text>().text == "Support");
                yield return new WaitForSeconds(1f);
                Might_Supp_2.onClick.Invoke();
                }
            }

            else
            {
                StartCoroutine(ClickUntil(BattleManagerRef.seedList2[t], "Attack"));
                yield return new WaitUntil(() => BattleManagerRef.seedList2[t].transform.Find("AttackType").GetComponentInChildren<Text>().text == "Attack");
            }
        }

        yield return new WaitForSeconds(2f);

        //Press End Turn Button
        p2EndTurnButtonRef.onClick.Invoke();
        Debug.Log("<color=yellow>END TURN CLICK </color>");
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
            SeedsSet++;
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

        //If defence still not high enough, more defence
        if (BattleManagerRef.damageCount1 > tempDefenceCount && (SeedsSet < 3))
        {
            Debug.Log("<color=red>damage > def, looping </color>");
            StartCoroutine("AIReactionDefenceLoop");
            yield break;
        }

        //Else if all damage is blocked, set remaining Seeds to attack
        else if (BattleManagerRef.damageCount1 <= tempDefenceCount && (SeedsSet < 3))
        {
            for (int t = 0; t < BattleManagerRef.seedList2.Length; t++)
            {
                if (p2Defence[t] > 0)
                {
                    StartCoroutine(ClickUntil(BattleManagerRef.seedList2[t], "Attack"));
                    yield return new WaitUntil(() => BattleManagerRef.seedList2[t].transform.Find("AttackType").GetComponentInChildren<Text>().text == "Attack");
                }

                if ((BattleManagerRef.damageCount1 / 5) > BattleManagerRef.seedList2[2].GetComponent<SeedScript>().attackVal)
                {
                    StartCoroutine(ClickUntil(BattleManagerRef.seedList2[2], "Support"));
                    yield return new WaitUntil(() => BattleManagerRef.seedList2[2].transform.Find("AttackType").GetComponentInChildren<Text>().text == "Support");
                }
            }

        }

        yield return new WaitForSeconds(2f);

        //Press End Turn Button
        p2EndTurnButtonRef.onClick.Invoke();
        Debug.Log("<color=yellow>END TURN CLICK </color>");
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


	

