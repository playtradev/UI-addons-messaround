using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManagerScript : MonoBehaviour {

    //Seed position
    [Header("Seed Array List")]
    [SerializeField]
    private Button[] seedList1;
    [SerializeField]
    private Button[] seedList2;

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
    [SerializeField]
    private GameObject player1Face;
    [SerializeField]
    private GameObject player2Face;
    [SerializeField]
    private Sprite[] faceAnimations;

    //Timer
    [Header("Timer")]
    public int timeRemaining;
    public int timerDuration = 15;
    [SerializeField]
    private Text timerText;
    [SerializeField]
    private bool isCountingDown = false;

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
    private int damageCount1;
    private int defenceCount1;
    private int damageCount2;
    private int defenceCount2;
    private bool magicSupport1;
    private bool magicSupport2;

    private bool player1ActionComplete;
    private bool player2ActionComplete;





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
        //TODO add maths in here to auto generate HP from Seed lvl
        damageCount1 = 0;
        defenceCount1 = 0;

        damageCount2 = 0;
        defenceCount2 = 0;

        player1ActionComplete = true;
        player2ActionComplete = true;


    StartCoroutine(BattleQueue());
    }

    //Timer functions
    public void BeginTimer()
    {
        if (!isCountingDown)
        {
            CancelInvoke("_tick");
            isCountingDown = true;
            timeRemaining = timerDuration;
            Invoke("_tick", 1f);
        }
        else if (isCountingDown)
        {
            CancelInvoke("_tick");
            timeRemaining = timerDuration;
            Invoke("_tick", 1f);
        }
        timerText.text = timeRemaining.ToString();
    }

    public void StopTimer()
    {
        isCountingDown = false;
        CancelInvoke("_tick");
    }

    private void _tick()
    {
        timeRemaining--;
        if (timeRemaining > 0)
        {
            Invoke("_tick", 1f);
        }
        else
        {
            isCountingDown = false;
        }
        timerText.text = timeRemaining.ToString();
        //(float)timeRemaining / timerDuration;
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

    //Setting Seeds Interactable or Not
    private void SetInteractableSeeds()
    {
        if (player1ActionComplete == false)
        {
            for (int t = 0; t < seedList1.Length; t++)
            {
                seedList1[t].GetComponent<Button>().interactable = true;
                player1Face.GetComponent<Image>().sprite = faceAnimations[1];
            }
        }
        else if (player1ActionComplete == true)
        {
            for (int t = 0; t < seedList1.Length; t++)
            {
                seedList1[t].GetComponent<Button>().interactable = false;
                player1Face.GetComponent<Image>().sprite = faceAnimations[0];
            }
        }

        if (player2ActionComplete == false)
        {
            for (int t = 0; t < seedList2.Length; t++)
            {
                seedList2[t].GetComponent<Button>().interactable = true;
                player2Face.GetComponent<Image>().sprite = faceAnimations[1];
            }
        }
        else if (player2ActionComplete == true)
        {
            for (int t = 0; t < seedList2.Length; t++)
            {
                seedList2[t].GetComponent<Button>().interactable = false;
                player2Face.GetComponent<Image>().sprite = faceAnimations[0];
            }
        }
    }

    //Action + Reaction Phases
    public void ActionPhase()
    {
        damageCount1 = 0;
        defenceCount1 = 0;
        magicSupport1 = false;

        //Iterate through Player 1's seeds and add up damage/defence
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

        Player1ATK.text = "" + damageCount1;
        Player1DEF.text = "" + defenceCount1;
    }

    private void ActionPhaseFailed()
    {
        damageCount1 = 0;
        defenceCount1 = 0;
        magicSupport1 = false;

        //If timer runs out, iterate through all seeds to set full defence
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

    public void ReactionPhase()
    {
        damageCount2 = 0;
        defenceCount2 = 0;
        magicSupport2 = false;

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
        Player2ATK.text = "" + damageCount2;
        Player2DEF.text = "" + defenceCount2;
    }

    private void ReactionPhaseFailed()
    {
        damageCount2 = 0;
        defenceCount2 = 0;
        magicSupport2 = false;

        //If timer runs out, set all seeds to defend and set full defence
        for (int t = 0; t < seedList2.Length; t++)
        {
            seedList2[t].GetComponent<SeedScript>().attackMode = "Defend";
            defenceCount2 = defenceCount2 + seedList2[t].GetComponent<SeedScript>().defenceVal;
        }
        Debug.Log("<color=red> OUT OF TIME, GREEN BOY. FULL DEFENCE </color>");

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
                damageCount1 = Mathf.CeilToInt(damageCount1 + (seedList1[1].GetComponent<SeedScript>().attackVal / 2));
            }
            else if (seedList1[t].transform.Find("Supp_Text").GetComponentInChildren<Text>().text == "Magic")
            {
                damageCount1 = Mathf.CeilToInt(damageCount1 + (seedList1[2].GetComponent<SeedScript>().attackVal / 2));
            }
        }

        //Mind: Increases Target Seed DEF by the Mind Seed's Mathf.Max(Att, Def)
        else if (t == 1)
        {
            if (seedList1[t].transform.Find("Supp_Text").GetComponentInChildren<Text>().text == "Might")
            {
                defenceCount1 = defenceCount1 + (seedList1[0].GetComponent<SeedScript>().defenceVal) + (Mathf.Max(seedList1[t].GetComponent<SeedScript>().attackVal, Mathf.Max(seedList1[t].GetComponent<SeedScript>().defenceVal)));
            }
            else if (seedList1[t].transform.Find("Supp_Text").GetComponentInChildren<Text>().text == "Magic")
            {
                defenceCount1 = defenceCount1 + (seedList1[2].GetComponent<SeedScript>().defenceVal) + (Mathf.Max(seedList1[t].GetComponent<SeedScript>().attackVal, Mathf.Max(seedList1[t].GetComponent<SeedScript>().defenceVal)));
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
                damageCount2 = Mathf.CeilToInt(damageCount2 + (seedList2[1].GetComponent<SeedScript>().attackVal / 2));
            }
            else if (seedList2[t].transform.Find("Supp_Text").GetComponentInChildren<Text>().text == "Magic")
            {
                damageCount2 = Mathf.CeilToInt(damageCount2 + (seedList2[2].GetComponent<SeedScript>().attackVal / 2));
            }
        }

        //Mind: Increases Target Seed DEF by the Mind Seed's Mathf.Max(Att, Def)
        else if (t == 1)
        {
            if (seedList2[t].transform.Find("Supp_Text").GetComponentInChildren<Text>().text == "Might")
            {
                defenceCount2 = defenceCount2 + (seedList2[0].GetComponent<SeedScript>().defenceVal) + (Mathf.Max(seedList2[t].GetComponent<SeedScript>().attackVal, Mathf.Max(seedList2[t].GetComponent<SeedScript>().defenceVal)));
            }
            else if (seedList2[t].transform.Find("Supp_Text").GetComponentInChildren<Text>().text == "Magic")
            {
                defenceCount2 = defenceCount2 + (seedList2[2].GetComponent<SeedScript>().defenceVal) + (Mathf.Max(seedList2[t].GetComponent<SeedScript>().attackVal, Mathf.Max(seedList2[t].GetComponent<SeedScript>().defenceVal)));
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
                //support things
            }
            if (seedList1[t].GetComponent<SeedScript>().attackMode == "Defend")
            {
                //Add Def to Shield
                player1Defence.CurrentVal = player1Defence.CurrentVal + seedList1[t].GetComponent<SeedScript>().defenceVal;
                //Display Info
                infoPane.text = "Player 1 gains " + (seedList1[t].GetComponent<SeedScript>().defenceVal) + " defence points";
                //Play Animation
                //***ANIMANIMANIM***
                yield return new WaitForSeconds(2f);
            }
        }

        // Iterate through P2 Seeds: Supp and Def
        for (int t = 0; t < seedList2.Length; t++)
        {
            if (seedList2[t].GetComponent<SeedScript>().attackMode == "Support")
            {
                //support things
            }
            if (seedList2[t].GetComponent<SeedScript>().attackMode == "Defend")
            {
                //Add Def to Shield
                player2Defence.CurrentVal = player2Defence.CurrentVal + seedList2[t].GetComponent<SeedScript>().defenceVal;
                //Display Info
                infoPane.text = "Player 2 gains " + (seedList2[t].GetComponent<SeedScript>().defenceVal) + " defence points";
                //Play Animation
                //***ANIMANIMANIM***
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
                    seedList1[t].GetComponent<Animator>().Play("Might_1_ATK", -1, 0.0f);

                    //Deal Shield damage
                    if (player2Defence.CurrentVal >= seedList1[t].GetComponent<SeedScript>().attackVal)
                    {
                        player2Defence.CurrentVal = player2Defence.CurrentVal - seedList1[t].GetComponent<SeedScript>().attackVal;
                    }

                    // Or, Deal Shield damage + HP damage 
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
                    //***ANIMANIMANIM***

                    //Deal damage
                    player2Health.CurrentVal = player2Health.CurrentVal - seedList1[t].GetComponent<SeedScript>().attackVal;
                    yield return new WaitForSeconds(2f);
                }
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
                    //***ANIMANIMANIM***

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
                    //***ANIMANIMANIM***

                    //Deal damage
                    player1Health.CurrentVal = player1Health.CurrentVal - seedList2[t].GetComponent<SeedScript>().attackVal;

                    yield return new WaitForSeconds(2f);
                }
            }
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
                //support things
            }
            if (seedList2[t].GetComponent<SeedScript>().attackMode == "Defend")
            {
                //Add Def to Shield
                player2Defence.CurrentVal = player2Defence.CurrentVal + seedList2[t].GetComponent<SeedScript>().defenceVal;
                //Display Info
                infoPane.text = "Player 2 gains " + (seedList2[t].GetComponent<SeedScript>().defenceVal) + " defence points";
                //Play Animation
                //***ANIMANIMANIM***
                yield return new WaitForSeconds(2f);
            }
        }

        // Iterate through P1 Seeds: Supp and Def
        for (int t = 0; t < seedList1.Length; t++)
        {
            if (seedList1[t].GetComponent<SeedScript>().attackMode == "Support")
            {
                //support things
            }
            if (seedList1[t].GetComponent<SeedScript>().attackMode == "Defend")
            {
                //Add Def to Shield
                player1Defence.CurrentVal = player1Defence.CurrentVal + seedList1[t].GetComponent<SeedScript>().defenceVal;
                //Display Info
                infoPane.text = "Player 1 gains " + (seedList1[t].GetComponent<SeedScript>().defenceVal) + " defence points";
                //Play Animation
                //***ANIMANIMANIM***
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
                    //seedList2[t].GetComponent<Animator>().Play();

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
                    //***ANIMANIMANIM***

                    //Deal damage
                    player1Health.CurrentVal = player1Health.CurrentVal - seedList2[t].GetComponent<SeedScript>().attackVal;
                    yield return new WaitForSeconds(2f);
                }
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
                    //***ANIMANIMANIM***

                    //Deal Shield damage
                    if (player2Defence.CurrentVal >= seedList1[t].GetComponent<SeedScript>().attackVal)
                    {
                        player2Defence.CurrentVal = player2Defence.CurrentVal - seedList1[t].GetComponent<SeedScript>().attackVal;
                    }

                    // Or, Deal Shield damage + HP damage 
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
                    //***ANIMANIMANIM***

                    //Deal damage
                    player2Health.CurrentVal = player2Health.CurrentVal - seedList1[t].GetComponent<SeedScript>().attackVal;

                    yield return new WaitForSeconds(2f);
                }
            }

            ResetDamageCounters();
        }
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

    //Combat Manager
    public IEnumerator BattleQueue()
    {
        //  ***Phase One***
        infoPane.text = "Player 1's Action";
        player1ActionComplete = false;
        SetInteractableSeeds();
        BeginTimer();
        
        yield return new WaitUntil(() => { return timeRemaining <= 0 || player1ActionComplete;  });
        if (player1ActionComplete == true)
        {
            ActionPhase();
        }
        else
        {
            ActionPhaseFailed();
        }
        StopTimer();


        //  ***Phase Two***
        infoPane.text = "Player 2's Reaction";
        player2ActionComplete = false;
        SetInteractableSeeds();
        BeginTimer();

        yield return new WaitUntil(() => { return timeRemaining <= 0 || player2ActionComplete; });
        if (player2ActionComplete == true)
        {
            ReactionPhase();
        }
        else
        {
            ReactionPhaseFailed();
        }
        StopTimer();


        //  ***Resolution Phase***
        infoPane.text = "RESOLVING COMBAT!";
        SetInteractableSeeds();
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
        player2ActionComplete = false;
        SetInteractableSeeds();
        BeginTimer();
        yield return new WaitUntil(() => { return timeRemaining <= 0 || player2ActionComplete; });
        if (player2ActionComplete == true)
        {
            ReactionPhase();
        }
        else
        {
            ReactionPhaseFailed();
        }
        StopTimer();


        //  ***Phase Four***
        infoPane.text = "Player 1's Reaction";
        player1ActionComplete = false;
        SetInteractableSeeds();
        BeginTimer();
        yield return new WaitUntil(() => { return timeRemaining <= 0 || player1ActionComplete; });
        if (player1ActionComplete == true)
        {
            ActionPhase();
        }
        else
        {
            ActionPhaseFailed();
        }
        StopTimer();


        //  ***Resolution Phase***
        SetInteractableSeeds();
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