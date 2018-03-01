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
    public int SeedsSet;

    [Header("Sub-Buttons")]
    [SerializeField]
    private Button Might_Supp_1;
    [SerializeField]
    private Button Might_Supp_2;
    [SerializeField]
    private Button Mind_Supp_1;
    [SerializeField]
    private Button Mind_Supp_2;

    [Header("Seed Sprites")]
    [SerializeField]
    private Sprite[] SeedSprites;



    private void Start()
    {
        GetAIPlayerSeedStats();
    }

    private void GetAIPlayerSeedStats()
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

    public void ResetSeedSprites()
    {
        for (int t = 0; t < BattleManagerRef.seedList2.Length; t++)
        {
            BattleManagerRef.seedList2[t].GetComponent<Image>().sprite = SeedSprites[t];
        }   
    }

    private void SetSeedLocked(int t)
    {
        BattleManagerRef.seedList2[t].GetComponent<Image>().sprite = SeedSprites[3];

        BattleManagerRef.seedList2[t].GetComponent<InstantiateCirclePopAnim>().AnimateOnClick();
    }

    private IEnumerator ClickUntil(Button b, string s)
    {
        yield return new WaitForSeconds(1f);
        b.onClick.Invoke();

        if (b.transform.Find("AttackType").GetComponentInChildren<Text>().text != s)
        {
            StartCoroutine(ClickUntil(b, s));
            yield return 0;
        }

        else if (b.transform.Find("AttackType").GetComponentInChildren<Text>().text == s)
        {
            Debug.Log("<color=blue>Clicked </color>" + b + " until it was " + s);
            yield return 0;
        }

    }

    private void RestartReactionLoop()
    {
        StopAllCoroutines();
        StartCoroutine(AIReactionDefenceLoop());
    }



    public void StartAIActionPhase()
    {
        StartCoroutine ("AIActionPhase");
    }

    public void StartAIReactionPhase()
    {
        StartCoroutine("AIReactionPhase");
    }

    private IEnumerator AIActionPhase()
    {
        Debug.Log("<color=blue>Action Phase Begin</color>");

        for (int t = 0; t < BattleManagerRef.seedList2.Length; t++)
        {
            //If attack higher, set attack
            if (BattleManagerRef.seedList2[t].GetComponent<SeedScript>().attackVal > BattleManagerRef.seedList2[t].GetComponent<SeedScript>().defenceVal)
            {
                if (BattleManagerRef.seedList2[t].transform.Find("AttackType").GetComponentInChildren<Text>().text == "Attack")
                {
                    Debug.Log("<color=blue>Seed </color>" + t + " already set to attack");
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    Debug.Log("<color=blue>Seed </color>" + t + " *click* to attack");
                    StartCoroutine(ClickUntil(BattleManagerRef.seedList2[t], "Attack"));
                    yield return new WaitUntil(() => BattleManagerRef.seedList2[t].transform.Find("AttackType").GetComponentInChildren<Text>().text == "Attack");
                    yield return new WaitForSeconds(1f);
                }

                SetSeedLocked(t);
            }

            //If defence higher, set defence
            else
            {
                if (BattleManagerRef.seedList2[t].transform.Find("AttackType").GetComponentInChildren<Text>().text == "Defend")
                {
                    Debug.Log("<color=blue>Seed </color>" + t + " already set to defend");
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    Debug.Log("<color=blue>Seed </color>" + t + " *click* to defend");
                    StartCoroutine(ClickUntil(BattleManagerRef.seedList2[t], "Defend"));
                    yield return new WaitUntil(() => BattleManagerRef.seedList2[t].transform.Find("AttackType").GetComponentInChildren<Text>().text == "Defend");
                    yield return new WaitForSeconds(1f);
                }

                SetSeedLocked(t);
            }
        }

        yield return new WaitForSeconds(2f);

        //Press End Turn Button
        p2EndTurnButtonRef.onClick.Invoke();
        Debug.Log("<color=yellow>END TURN CLICK </color>");
    }

    private IEnumerator AIReactionPhase()
    {
        Debug.Log("<color=blue>Reaction Phase Begin</color>");        

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
                Debug.Log("<color=blue>Deploying Might Support </color>");
                if (BattleManagerRef.seedList2[1].GetComponent<SeedScript>().attackVal >= BattleManagerRef.seedList2[2].GetComponent<SeedScript>().attackVal)
                {
                StartCoroutine(ClickUntil(BattleManagerRef.seedList2[0], "Support"));
                yield return new WaitUntil(() => BattleManagerRef.seedList2[0].transform.Find("AttackType").GetComponentInChildren<Text>().text == "Support");
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

                yield return new WaitForSeconds(1f);
                SetSeedLocked(t);
            }

            else
            {
                Debug.Log("<color=blue>Seed </color>" + t + " *click* to attack");
                StartCoroutine(ClickUntil(BattleManagerRef.seedList2[t], "Attack"));
                yield return new WaitUntil(() => BattleManagerRef.seedList2[t].transform.Find("AttackType").GetComponentInChildren<Text>().text == "Attack");

                yield return new WaitForSeconds(1f);
                SetSeedLocked(t);
            }
        }

        yield return new WaitForSeconds(2f);

        //Press End Turn Button
        p2EndTurnButtonRef.onClick.Invoke();
        Debug.Log("<color=yellow>END TURN CLICK </color>");
    }

    private IEnumerator AIReactionDefenceLoop()
    {

        Debug.Log("<color=blue>Reaction Defence Loop preferred</color>");

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

        //Set the Seed with highest defence to Defend
        if (BattleManagerRef.seedList2[maxDefValIndex].transform.Find("AttackType").GetComponentInChildren<Text>().text != "Defend")
        {
            Debug.Log("<color=blue>Max Def set on Seed : </color>" + BattleManagerRef.seedList2[maxDefValIndex]);
            StartCoroutine(ClickUntil(BattleManagerRef.seedList2[maxDefValIndex], "Defend"));
            yield return new WaitUntil(() => BattleManagerRef.seedList2[maxDefValIndex].transform.Find("AttackType").GetComponentInChildren<Text>().text == "Defend");
        }

        tempDefenceCount += p2Defence[maxDefValIndex];
        p2Defence[maxDefValIndex] = 0;
        p2Attack[maxDefValIndex] = 0;

        yield return new WaitForSeconds(1f);
        SetSeedLocked(maxDefValIndex);
        yield return new WaitForSeconds(1f);

        SeedsSet++;


        if (SeedsSet < 3)
        {
            //If defence still not high enough, Restart to add more defence
            if (BattleManagerRef.damageCount1 > tempDefenceCount)
            {
                Debug.Log("<color=red>damage > def, looping </color>");
                RestartReactionLoop();
            }

            //Else if all damage is blocked, set remaining Seeds to max attack
            else if (BattleManagerRef.damageCount1 <= tempDefenceCount)
            {

                Debug.Log("<color=red>damage < def, setting rest to attack or support </color>");
                for (int t = 0; t < BattleManagerRef.seedList2.Length; t++)
                {
                    if (p2Defence[t] > 0)
                    {
                        if (BattleManagerRef.seedList2[t].transform.Find("AttackType").GetComponentInChildren<Text>().text == "Attack")
                        {
                            Debug.Log("<color=blue>Seed </color>" + t + " already set to attack");
                            yield return new WaitForSeconds(1f);
                            SetSeedLocked(t);
                        }
                        else
                        {
                            StartCoroutine(ClickUntil(BattleManagerRef.seedList2[t], "Attack"));
                            yield return new WaitUntil(() => BattleManagerRef.seedList2[t].transform.Find("AttackType").GetComponentInChildren<Text>().text == "Attack");
                            yield return new WaitForSeconds(1f);
                            SetSeedLocked(t);
                        }
                    }
                    //TODO this needs to be an else if   
                    /*
                    if ((BattleManagerRef.damageCount1 / 5) > BattleManagerRef.seedList2[2].GetComponent<SeedScript>().attackVal)
                    {
                        StartCoroutine(ClickUntil(BattleManagerRef.seedList2[2], "Support"));
                        yield return new WaitUntil(() => BattleManagerRef.seedList2[2].transform.Find("AttackType").GetComponentInChildren<Text>().text == "Support");

                        yield return new WaitForSeconds(1f);
                
                    }                 
                    */
                }
            }
        }

        yield return new WaitForSeconds(2f);

        //Press End Turn Button    
        p2EndTurnButtonRef.onClick.Invoke();
        Debug.Log("<color=yellow>END TURN CLICK </color>");

        //Reset Calculators
        SeedsSet = 0;
        GetAIPlayerSeedStats();

        yield return 0;

    }
}


	

