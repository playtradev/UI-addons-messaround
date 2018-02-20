using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManagerScript : MonoBehaviour {

    //Seed position
    [Header("Seed Array List")]
    public Button[] seedList1;   
    public Button[] seedList2;

    //UI
    [Header("UI")]
    [SerializeField]
    private Text infoPane;
    [SerializeField]
    private Text Player1ATK;
    [SerializeField]
    private Text Player1DEF;
    [SerializeField]
    private Text Player2ATK;
    [SerializeField]
    private Text Player2DEF;

    //Timer
    [Header("Timer")]
    [SerializeField]
    private TimerScript timerScript;

    //Battle info
    [Header("Health and Defence Bars")]
    [SerializeField]
    private Stat player1Health;
    [SerializeField]
    private Stat player1Defence;
    [SerializeField]
    private Stat player2Health;
    [SerializeField]
    private Stat player2Defence;

    [Header("End of Game PopUp")]
    [SerializeField]
    private GameObject GameOverPopUpRef;

    //Combat counters
    public int damageCount1;
    public int defenceCount1;
    public int damageCount2;
    public int defenceCount2;
    public bool magicSupport1;
    public bool magicSupport2;

    [Header("Set Interactable Ref")]
    public bool player1ActionComplete;
    public bool player2ActionComplete;
    [SerializeField]
    private SetInteractableScript SetInteractableScriptRef;

    [Header("AI")]
    [SerializeField]
    private AI_Script AIScriptRef;



    void Awake()
    {
        //TODO make all these auto calculate from seed stats
        player1Health.MaxVal = 28;
        player2Health.MaxVal = 33;
        player1Health.CurrentVal = 28;
        player2Health.CurrentVal = 33;

        player1Defence.MaxVal = 11;
        player2Defence.MaxVal = 17;
        player1Defence.CurrentVal = 0;
        player2Defence.CurrentVal = 0;
    }

    // Use this for initialization
    void Start ()
    {
        damageCount1 = 0;
        defenceCount1 = 0;

        damageCount2 = 0;
        defenceCount2 = 0;

        StartCoroutine(BattleQueue());
    }

    //Player turns, simply for end turn button
    public void PlayerOneEndTurn()
    {
        player1ActionComplete = true;
    }

    public void PlayerTwoEndTurn()
    {
        player2ActionComplete = true;
    }

    private void ResetDamageCounters()
    {
        //Reset Defence
        player1Defence.CurrentVal = 0;
        player2Defence.CurrentVal = 0;

        //Reset Damage Counters
        damageCount1 = 0;
        defenceCount1 = 0;
        damageCount2 = 0;
        defenceCount2 = 0;
        magicSupport1 = false;
        magicSupport2 = false;

        //Reset ATT/DEF counters
        Player1ATK.text = "0";
        Player1DEF.text = "0";
        Player2ATK.text = "0";
        Player2DEF.text = "0";

    }

    //Action + Reaction Phases
    public void P1ActionPhase()
    {
        //First, reset all stats so it re-calculates on each press
        damageCount1 = 0;
        defenceCount1 = 0;
        magicSupport1 = false;
        //Including resetting each seed!
        for (int t = 0; t < seedList1.Length; t++)
        {
            seedList1[t].GetComponent<SeedScript>().ResetSeedStats();
            seedList1[t].GetComponent<SeedScript>().SetStatsText();
        }

        //Then Iterate through Player 1's seeds and add up damage/defence
        for (int t = 0; t < seedList1.Length; t++)
        {
            if (seedList1[t].GetComponent<SeedScript>().attackMode == "Attack")
            {
                damageCount1 = damageCount1 + seedList1[t].GetComponent<SeedScript>().attackVal;
            }

            else if (seedList1[t].GetComponent<SeedScript>().attackMode == "Defend")
            {
                defenceCount1 = defenceCount1 + seedList1[t].GetComponent<SeedScript>().defenceVal;
            }
            else if (seedList1[t].GetComponent<SeedScript>().attackMode == "Support")
            {
                SupportActions1(t);
            }
        }

        //Update UI
        //seedList1[t].GetComponent<SeedScript>().SetStatsText();
        Player1ATK.text = "" + damageCount1;
        Player1DEF.text = "" + defenceCount1;
    }

    private void P1ActionPhaseFailed()
    {
        damageCount1 = 0;
        defenceCount1 = 0;
        magicSupport1 = false;

        //Reset each Seed + Seed Text
        for (int t = 0; t < seedList1.Length; t++)
        {
            seedList1[t].GetComponent<SeedScript>().ResetSeedStats();
            seedList1[t].GetComponent<SeedScript>().SetStatsText();
        }

        //Iterate through all seeds to set full defence
        for (int t = 0; t < seedList1.Length; t++)
        {
            seedList1[t].GetComponent<SeedScript>().attackMode = "Defend";
            //Also set text to "Defend"
            seedList1[t].transform.Find("AttackType").GetComponentInChildren<Text>().text = "Defend";
            //Add to defence Val
            defenceCount1 = defenceCount1 + seedList1[t].GetComponent<SeedScript>().defenceVal;
        }
        Debug.Log("<color=red>OUT OF TIME </color>");

        Player1ATK.text = "" + damageCount1;
        Player1DEF.text = "" + defenceCount1;

        player1ActionComplete = true;
    }

    public void P2ActionPhase()
    {
        damageCount2 = 0;
        defenceCount2 = 0;
        magicSupport2 = false;
        //Including resetting each seed!
        for (int t = 0; t < seedList2.Length; t++)
        {
            seedList2[t].GetComponent<SeedScript>().ResetSeedStats();
            seedList2[t].GetComponent<SeedScript>().SetStatsText();
        }

        //Iterate through Player 2's seeds and add up damage/defence
        for (int t = 0; t < seedList2.Length; t++)
        {
            if (seedList2[t].GetComponent<SeedScript>().attackMode == "Attack")
            {
                damageCount2 = damageCount2 + seedList2[t].GetComponent<SeedScript>().attackVal;
            }
            else if (seedList2[t].GetComponent<SeedScript>().attackMode == "Defend")
            {
                defenceCount2 = defenceCount2 + seedList2[t].GetComponent<SeedScript>().defenceVal;
            }
            else if (seedList2[t].GetComponent<SeedScript>().attackMode == "Support")
            {
                SupportActions2(t);
            }
        }

        //Update UI
        Player2ATK.text = "" + damageCount2;
        Player2DEF.text = "" + defenceCount2;
    }

    private void P2ActionPhaseFailed()
    {
        damageCount2 = 0;
        defenceCount2 = 0;
        magicSupport2 = false;

        //Reset each Seed + Seed Text
        for (int t = 0; t < seedList2.Length; t++)
        {
            seedList2[t].GetComponent<SeedScript>().ResetSeedStats();
            seedList2[t].GetComponent<SeedScript>().SetStatsText();
        }

        //Iterate through all seeds to set full defence
        for (int t = 0; t < seedList2.Length; t++)
        {
            seedList2[t].GetComponent<SeedScript>().attackMode = "Defend";
            //Also set text to "Defend"
            seedList2[t].transform.Find("AttackType").GetComponentInChildren<Text>().text = "Defend";
            //Add to defence Val
            defenceCount2 = defenceCount2 + seedList2[t].GetComponent<SeedScript>().defenceVal;
        }
        Debug.Log("<color=red>OUT OF TIME </color>");

        Player2ATK.text = "" + damageCount2;
        Player2DEF.text = "" + defenceCount2;

        player2ActionComplete = true;
    }

    //Support seed functions
    private void SupportActions1(int t)
    {
        //Might: Increases Target Seed ATT by 50%
        if (t == 0)
        {
            if (seedList1[t].transform.Find("Supp_Text").GetComponentInChildren<Text>().text == "Mind")
            {
                seedList1[1].GetComponent<SeedScript>().attackVal = Mathf.CeilToInt(seedList1[1].GetComponent<SeedScript>().attackVal + (seedList1[1].GetComponent<SeedScript>().attackVal / 2));
                seedList1[1].GetComponent<SeedScript>().SetStatsText();
            }
            else if (seedList1[t].transform.Find("Supp_Text").GetComponentInChildren<Text>().text == "Magic")
            {
                seedList1[2].GetComponent<SeedScript>().attackVal = Mathf.CeilToInt(seedList1[2].GetComponent<SeedScript>().attackVal + (seedList1[2].GetComponent<SeedScript>().attackVal / 2));
                seedList1[2].GetComponent<SeedScript>().SetStatsText();
            }
        }

        //Mind: Increases Target Seed DEF by the Mind Seed's Mathf.Max(Att, Def)
        else if (t == 1)
        {
            if (seedList1[t].transform.Find("Supp_Text").GetComponentInChildren<Text>().text == "Might")
            {
                seedList1[0].GetComponent<SeedScript>().defenceVal = seedList1[0].GetComponent<SeedScript>().defenceVal + (Mathf.Max(seedList1[t].GetComponent<SeedScript>().attackVal, (seedList1[t].GetComponent<SeedScript>().defenceVal)));
                seedList1[0].GetComponent<SeedScript>().SetStatsText();
            }
            else if (seedList1[t].transform.Find("Supp_Text").GetComponentInChildren<Text>().text == "Magic")
            {
                seedList1[2].GetComponent<SeedScript>().defenceVal = seedList1[2].GetComponent<SeedScript>().defenceVal + (Mathf.Max(seedList1[t].GetComponent<SeedScript>().attackVal, (seedList1[t].GetComponent<SeedScript>().defenceVal)));
                seedList1[2].GetComponent<SeedScript>().SetStatsText();
            }
        }

        //Magic: Deals 20% of damage recieved back to opponent.
        else if (t == 2)
        {
            magicSupport1 = true;
        }
    }

    private void SupportActions2(int t)
    {
        //Might: Increases Target Seed ATT by 50%
        if (t == 0)
        {
            if (seedList2[t].transform.Find("Supp_Text").GetComponentInChildren<Text>().text == "Mind")
            {
                seedList2[1].GetComponent<SeedScript>().attackVal = Mathf.CeilToInt(seedList2[1].GetComponent<SeedScript>().attackVal + (seedList2[1].GetComponent<SeedScript>().attackVal / 2));
                seedList2[1].GetComponent<SeedScript>().SetStatsText();
            }
            else if (seedList2[t].transform.Find("Supp_Text").GetComponentInChildren<Text>().text == "Magic")
            {
                seedList2[2].GetComponent<SeedScript>().attackVal = Mathf.CeilToInt(seedList1[2].GetComponent<SeedScript>().attackVal + (seedList2[2].GetComponent<SeedScript>().attackVal / 2));
                seedList2[2].GetComponent<SeedScript>().SetStatsText();
            }
        }

        //Mind: Increases Target Seed DEF by the Mind Seed's Mathf.Max(Att, Def)
        else if (t == 1)
        {
            if (seedList2[t].transform.Find("Supp_Text").GetComponentInChildren<Text>().text == "Might")
            {
                seedList2[0].GetComponent<SeedScript>().defenceVal = seedList2[0].GetComponent<SeedScript>().defenceVal + (Mathf.Max(seedList2[t].GetComponent<SeedScript>().attackVal, (seedList2[t].GetComponent<SeedScript>().defenceVal)));
                seedList2[0].GetComponent<SeedScript>().SetStatsText();
            }
            else if (seedList2[t].transform.Find("Supp_Text").GetComponentInChildren<Text>().text == "Magic")
            {
                seedList2[2].GetComponent<SeedScript>().defenceVal = seedList2[2].GetComponent<SeedScript>().defenceVal + (Mathf.Max(seedList2[t].GetComponent<SeedScript>().attackVal, (seedList2[t].GetComponent<SeedScript>().defenceVal)));
                seedList2[2].GetComponent<SeedScript>().SetStatsText();
            }
        }

        //Magic: Deals 20% of damage recieved back to opponent.
        else if (t == 2)
        {
            magicSupport2 = true;
        }
    }

    //Resolution phase
    private IEnumerator ResolutionPhase1()
    {

        // Iterate through P1 Seeds: Supp and Def
        for (int t = 0; t < seedList1.Length; t++)
        {
            if (seedList1[t].GetComponent<SeedScript>().attackMode == "Support")
            {
                infoPane.text = "Player 1's Seed Supports";

                seedList1[t].GetComponent<SeedScript>().SupportAnim(t);

                yield return new WaitForSeconds(2f);
            }
            if (seedList1[t].GetComponent<SeedScript>().attackMode == "Defend")
            {
                //Add Def to Shield
                player1Defence.CurrentVal = player1Defence.CurrentVal + seedList1[t].GetComponent<SeedScript>().defenceVal;

                //Display Info
                infoPane.text = "Player 1 gains " + (seedList1[t].GetComponent<SeedScript>().defenceVal) + " defence points";

                //Play Animation
                seedList1[t].GetComponent<SeedScript>().DefenceAnim(t);

                yield return new WaitForSeconds(2f);
            }
        }

        // Iterate through P2 Seeds: Supp and Def
        for (int t = 0; t < seedList2.Length; t++)
        {
            if (seedList2[t].GetComponent<SeedScript>().attackMode == "Support")
            {
                infoPane.text = "Player 2's Seed Supports";

                seedList2[t].GetComponent<SeedScript>().SupportAnim(t);

                yield return new WaitForSeconds(2f);
            }
            if (seedList2[t].GetComponent<SeedScript>().attackMode == "Defend")
            {
                //Add Def to Shield
                player2Defence.CurrentVal = player2Defence.CurrentVal + seedList2[t].GetComponent<SeedScript>().defenceVal;

                //Display Info
                infoPane.text = "Player 2 gains " + (seedList2[t].GetComponent<SeedScript>().defenceVal) + " defence points";

                //Play Animation
                seedList2[t].GetComponent<SeedScript>().DefenceAnim(t);

                yield return new WaitForSeconds(2f);
            }
        }

        // Iterate through P1 Attack Seeds
        for (int t = 0; t < seedList1.Length; t++)
        {
            if (seedList1[t].GetComponent<SeedScript>().attackMode == "Attack")
            {
                //Deal damage to Shield first
                if (player2Defence.CurrentVal > 0)
                {
                    //Display Info
                    infoPane.text = "Player 1 deals " + (seedList1[t].GetComponent<SeedScript>().attackVal) + " damage!";
                    //Play Animation
                    seedList1[t].GetComponent<SeedScript>().AttackAnim(t);


                    //Deal Shield damage...
                    if (player2Defence.CurrentVal >= seedList1[t].GetComponent<SeedScript>().attackVal)
                    {
                        player2Defence.CurrentVal = player2Defence.CurrentVal - seedList1[t].GetComponent<SeedScript>().attackVal;
                    }

                    //...Or, Deal Shield damage + HP damage 
                    else if (player2Defence.CurrentVal < seedList1[t].GetComponent<SeedScript>().attackVal)
                    {
                        //store the remainder
                        int remainder = (int)(seedList1[t].GetComponent<SeedScript>().attackVal - player2Defence.CurrentVal);
                        //Deal Shield damage
                        player2Defence.CurrentVal = player2Defence.CurrentVal - seedList1[t].GetComponent<SeedScript>().attackVal;
                        yield return new WaitForSeconds(1f);
                        //Deal remaining HP damage
                        player2Health.CurrentVal = player2Health.CurrentVal - (remainder);
                    }

                    yield return new WaitForSeconds(2f);

                }

                //Deal straight to HP pool
                else if (player2Defence.CurrentVal <= 0)
                {
                    //Display Info
                    infoPane.text = "Player 1 deals " + (seedList1[t].GetComponent<SeedScript>().attackVal) + " damage!";

                    //Play Animation
                    seedList1[t].GetComponent<SeedScript>().AttackAnim(t);

                    //Deal damage
                    player2Health.CurrentVal = player2Health.CurrentVal - seedList1[t].GetComponent<SeedScript>().attackVal;
                    yield return new WaitForSeconds(2f);
                }
            }
        }

        if (magicSupport2 == true)
        {
            //Info
            infoPane.text = "Player 2 Magic Support returns " + (Mathf.CeilToInt((float)damageCount1 / 5)) + " true damage!";
            //Make Animation!
            //TODO magic shield animation
            //Deal damage
            player1Health.CurrentVal = player1Health.CurrentVal - (Mathf.CeilToInt((float)damageCount1 / 5));

            yield return new WaitForSeconds(2f);
        }


        // Iterate through P2 Attack Seeds
        for (int t = 0; t < seedList2.Length; t++)
        {
            if (seedList2[t].GetComponent<SeedScript>().attackMode == "Attack")
            {
                //Deal damage to Shield first
                if (player1Defence.CurrentVal > 0)
                {
                    //Display Info
                    infoPane.text = "Player 2 deals " + (seedList2[t].GetComponent<SeedScript>().attackVal) + " damage!";

                    //Play Animation
                    seedList2[t].GetComponent<SeedScript>().AttackAnim(t);

                    //Deal Shield damage
                    if (player1Defence.CurrentVal >= seedList2[t].GetComponent<SeedScript>().attackVal)
                    {
                        player1Defence.CurrentVal = player1Defence.CurrentVal - seedList2[t].GetComponent<SeedScript>().attackVal;
                    }

                    // Or, Deal Shield damage + HP damage 
                    else if (player1Defence.CurrentVal < seedList2[t].GetComponent<SeedScript>().attackVal)
                    {
                        //store the remainder
                        int remainder = (int)(seedList2[t].GetComponent<SeedScript>().attackVal - player1Defence.CurrentVal);
                        //Deal Shield damage
                        player1Defence.CurrentVal = player1Defence.CurrentVal - seedList2[t].GetComponent<SeedScript>().attackVal;
                        yield return new WaitForSeconds(1f);

                        //Deal remaining HP damage
                        player1Health.CurrentVal = player1Health.CurrentVal - (remainder);
                    }

                    yield return new WaitForSeconds(2f);
                }

                //Deal straight to HP pool
                else if (player1Defence.CurrentVal <= 0)
                {
                    //Display Info
                    infoPane.text = "Player 2 deals " + (seedList2[t].GetComponent<SeedScript>().attackVal) + " damage!";

                    //Play Animation
                    seedList2[t].GetComponent<SeedScript>().AttackAnim(t);

                    //Deal damage
                    player1Health.CurrentVal = player1Health.CurrentVal - seedList2[t].GetComponent<SeedScript>().attackVal;

                    yield return new WaitForSeconds(2f);
                }
            }
        }

        if (magicSupport1 == true)
        {
            //Info
            infoPane.text = "Player 1 Magic Support returns " + (Mathf.CeilToInt((float)damageCount2 / 5)) + " true damage!";
            //Make Animation!
            //TODO magic shield animation
            //Deal damage
            player2Health.CurrentVal = player2Health.CurrentVal - (Mathf.CeilToInt((float)damageCount2 / 5));

            yield return new WaitForSeconds(2f);
        }

        ResetDamageCounters();
    }

    private IEnumerator ResolutionPhase2()
    {
        // Iterate through P2 Seeds: Supp and Def
        for (int t = 0; t < seedList2.Length; t++)
        {
            if (seedList2[t].GetComponent<SeedScript>().attackMode == "Support")
            {
                infoPane.text = "Player 2's Seed Supports";

                seedList1[t].GetComponent<SeedScript>().SupportAnim(t);

                yield return new WaitForSeconds(2f);
            }
            if (seedList2[t].GetComponent<SeedScript>().attackMode == "Defend")
            {
                //Add Def to Shield
                player2Defence.CurrentVal = player2Defence.CurrentVal + seedList2[t].GetComponent<SeedScript>().defenceVal;

                //Display Info
                infoPane.text = "Player 2 gains " + (seedList2[t].GetComponent<SeedScript>().defenceVal) + " defence points";

                //Play Animation
                seedList1[t].GetComponent<SeedScript>().DefenceAnim(t);

                yield return new WaitForSeconds(2f);

            }
        }

        // Iterate through P1 Seeds: Supp and Def
        for (int t = 0; t < seedList1.Length; t++)
        {
            if (seedList1[t].GetComponent<SeedScript>().attackMode == "Support")
            {
                infoPane.text = "Player 1's Seed Supports";

                seedList1[t].GetComponent<SeedScript>().SupportAnim(t);

                yield return new WaitForSeconds(2f);
            }
            if (seedList1[t].GetComponent<SeedScript>().attackMode == "Defend")
            {
                //Add Def to Shield
                player1Defence.CurrentVal = player1Defence.CurrentVal + seedList1[t].GetComponent<SeedScript>().defenceVal;

                //Display Info
                infoPane.text = "Player 1 gains " + (seedList1[t].GetComponent<SeedScript>().defenceVal) + " defence points";

                //Play Animation
                seedList1[t].GetComponent<SeedScript>().DefenceAnim(t);

                yield return new WaitForSeconds(2f);
            }
        }

        // Iterate through P2 Attack Seeds
        for (int t = 0; t < seedList2.Length; t++)
        {
            if (seedList2[t].GetComponent<SeedScript>().attackMode == "Attack")
            {
                //Deal damage to Shield first
                if (player1Defence.CurrentVal > 0)
                {
                    //Display Info
                    infoPane.text = "Player 2 deals " + (seedList2[t].GetComponent<SeedScript>().attackVal) + " damage!";
                    //Play Animation
                    seedList2[t].GetComponent<SeedScript>().AttackAnim(t);

                    //Deal Shield damage
                    if (player1Defence.CurrentVal >= seedList2[t].GetComponent<SeedScript>().attackVal)
                    {
                        player1Defence.CurrentVal = player1Defence.CurrentVal - seedList2[t].GetComponent<SeedScript>().attackVal;
                    }

                    // Or, Deal Shield damage + HP damage 
                    else if (player1Defence.CurrentVal < seedList2[t].GetComponent<SeedScript>().attackVal)
                    {
                        //store the remainder
                        int remainder = (int)(seedList2[t].GetComponent<SeedScript>().attackVal - player1Defence.CurrentVal);
                        //Deal Shield damage
                        player1Defence.CurrentVal = player1Defence.CurrentVal - seedList2[t].GetComponent<SeedScript>().attackVal;
                        yield return new WaitForSeconds(1f);

                        //Deal remaining HP damage
                        player1Health.CurrentVal = player1Health.CurrentVal - (remainder);
                    }

                    yield return new WaitForSeconds(2f);

                }

                //Deal straight to HP pool
                else if (player1Defence.CurrentVal <= 0)
                {
                    //Display Info
                    infoPane.text = "Player 2 deals " + (seedList2[t].GetComponent<SeedScript>().attackVal) + " damage!";

                    //Play Animation
                    seedList2[t].GetComponent<SeedScript>().AttackAnim(t);

                    //Deal damage
                    player1Health.CurrentVal = player1Health.CurrentVal - seedList2[t].GetComponent<SeedScript>().attackVal;
                    yield return new WaitForSeconds(2f);
                }
            }
        }


        if (magicSupport1 == true)
        {
            //Info
            infoPane.text = "Player 1 Magic Support returns " + (Mathf.CeilToInt((float)damageCount2 / 5)) + " true damage!";
            //Make Animation!
            //TODO magic shield animation
            //Deal damage
            player2Health.CurrentVal = player2Health.CurrentVal - (Mathf.CeilToInt((float)damageCount2 / 5));

            yield return new WaitForSeconds(2f);
        }

            // Iterate through P1 Attack Seeds
            for (int t = 0; t < seedList1.Length; t++)
        {
            if (seedList1[t].GetComponent<SeedScript>().attackMode == "Attack")
            {
                //Deal damage to Shield first
                if (player2Defence.CurrentVal > 0)
                {
                    //Display Info
                    infoPane.text = "Player 1 deals " + (seedList1[t].GetComponent<SeedScript>().attackVal) + " damage!";
                    //Play Animation
                    seedList1[t].GetComponent<SeedScript>().AttackAnim(t);

                    //Deal Shield damage...
                    if (player2Defence.CurrentVal >= seedList1[t].GetComponent<SeedScript>().attackVal)
                    {
                        player2Defence.CurrentVal = player2Defence.CurrentVal - seedList1[t].GetComponent<SeedScript>().attackVal;
                    }

                    //...Or, Deal Shield damage + HP damage 
                    else if (player2Defence.CurrentVal < seedList1[t].GetComponent<SeedScript>().attackVal)
                    {
                        //store the remainder
                        int remainder = (int)(seedList1[t].GetComponent<SeedScript>().attackVal - player2Defence.CurrentVal);
                        //Deal Shield damage
                        player2Defence.CurrentVal = player2Defence.CurrentVal - seedList1[t].GetComponent<SeedScript>().attackVal;
                        yield return new WaitForSeconds(1f);

                        //Deal remaining HP damage
                        player2Health.CurrentVal = player2Health.CurrentVal - (remainder);
                    }

                    yield return new WaitForSeconds(2f);
                }

                //Deal straight to HP pool
                else if (player2Defence.CurrentVal <= 0)
                {
                    //Display Info
                    infoPane.text = "Player 1 deals " + (seedList1[t].GetComponent<SeedScript>().attackVal) + " damage!";

                    //Play Animation
                    seedList1[t].GetComponent<SeedScript>().AttackAnim(t);

                    //Deal damage
                    player2Health.CurrentVal = player2Health.CurrentVal - seedList1[t].GetComponent<SeedScript>().attackVal;

                    yield return new WaitForSeconds(2f);
                }
            }

            if (magicSupport2 == true)
            {
                //Info
                infoPane.text = "Player 2 Magic Support returns " + (Mathf.CeilToInt((float)damageCount1 / 5)) + " true damage!";
                //Make Animation!
                //TODO magic shield animation
                //Deal damage
                player1Health.CurrentVal = player1Health.CurrentVal - (Mathf.CeilToInt((float)damageCount1 / 5));

                yield return new WaitForSeconds(2f);
            }

            ResetDamageCounters();
        }
    }

    //Combat Manager
    public IEnumerator BattleQueue()
    {
        //  ***Phase One***
        infoPane.text = "Player 1's Action";
        player2ActionComplete = true;
        SetInteractableScriptRef.SetInteractableObjects();
        timerScript.BeginTimer();
        
        yield return new WaitUntil(() => { return timerScript.timeRemaining <= 0 || player1ActionComplete;  });
        if (player1ActionComplete == true)
        {
            P1ActionPhase();
        }
        else
        {
            P1ActionPhaseFailed();
        }
        timerScript.StopTimer();


        //  ***Phase Two***
        infoPane.text = "Player 2's Reaction";
        player2ActionComplete = false;
        SetInteractableScriptRef.SetInteractableObjects();
        timerScript.BeginTimer();

        AIScriptRef.StartAIReactionPhase();

        yield return new WaitUntil(() => { return timerScript.timeRemaining <= 0 || player2ActionComplete; });
        if (player2ActionComplete == true)
        {
            P2ActionPhase();
        }
        else
        {
            P2ActionPhaseFailed();
        }
        timerScript.StopTimer();


        //  ***Resolution Phase***
        player1ActionComplete = false;
        player2ActionComplete = false;
        SetInteractableScriptRef.ResolutionPhaseInteractables();

        AIScriptRef.ResetSeedSprites();
        yield return StartCoroutine( ResolutionPhase1() );
        

        //**End here if somebody is dead!***
        if (player1Health.CurrentVal <= 0  || player2Health.CurrentVal <= 0)
        {
            GameOver();
            infoPane.text = "Game Over, Man! Game Over";
            yield break;
        }


        //  ***Phase Three***
        infoPane.text = "Player 2's Action";
        player1ActionComplete = true;
        SetInteractableScriptRef.SetInteractableObjects();
        timerScript.BeginTimer();

        AIScriptRef.StartAIActionPhase();

        yield return new WaitUntil(() => { return timerScript.timeRemaining <= 0 || player2ActionComplete; });
        if (player2ActionComplete == true)
        {
            P2ActionPhase();
        }
        else
        {
            P2ActionPhaseFailed();
        }
        timerScript.StopTimer();


        //  ***Phase Four***
        infoPane.text = "Player 1's Reaction";
        player1ActionComplete = false;
        SetInteractableScriptRef.SetInteractableObjects();
        timerScript.BeginTimer();
        yield return new WaitUntil(() => { return timerScript.timeRemaining <= 0 || player1ActionComplete; });
        if (player1ActionComplete == true)
        {
            P1ActionPhase();
        }
        else
        {
            P1ActionPhaseFailed();
        }
        timerScript.StopTimer();


        //  ***Resolution Phase***
        player1ActionComplete = false;
        player2ActionComplete = false;
        SetInteractableScriptRef.ResolutionPhaseInteractables();

        AIScriptRef.ResetSeedSprites();
        yield return StartCoroutine( ResolutionPhase1() );


        //***Restart if both alive***
        if (player1Health.CurrentVal > 0 && player2Health.CurrentVal > 0)
        { StartCoroutine(BattleQueue()); }
        else
        {
            GameOver();
            infoPane.text = "Game Over, Man! Game Over";
            yield break;
        }
    }

    //Game Over PopUp
    private void GameOver()
    {
        GameOverPopUpRef.gameObject.SetActive(true);

        if (player1Health.CurrentVal < 1 && player2Health.CurrentVal < 1)
        {
            GameOverPopUpRef.transform.Find("EndGameText").GetComponent<Text>().text = "DRAW";
        }
        else if (player1Health.CurrentVal > 0 && player2Health.CurrentVal < 1)
        {
            GameOverPopUpRef.transform.Find("EndGameText").GetComponent<Text>().text = "Player 1 Wins";
        }
        else if (player1Health.CurrentVal < 1 && player2Health.CurrentVal > 0)
        {
            GameOverPopUpRef.transform.Find("EndGameText").GetComponent<Text>().text = "Player 2 Wins";
        }

    }

}