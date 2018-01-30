using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManagerScript : MonoBehaviour {

    //Battle info
    [Header("Health and Defence bars")]
    [SerializeField]
    private Stat player1Health;
    [SerializeField]
    private Stat player1Defence;
    [SerializeField]
    private Stat player2Health;
    [SerializeField]
    private Stat player2Defence;

    //Combat counters
    private int damageCount1;
    private int defenceCount1;
    private int damageCount2;
    private int defenceCount2;
    private bool magicSupport1;
    private bool magicSupport2;

    private bool player1ActionComplete;
    private bool player2ActionComplete;

    //seed position
    [Header("Seed Array List")]
    public Button[] seedList1;
    public Button[] seedList2;

    [Header("End of Game PopUp")]
    public GameObject GameOverPopUpRef;

    //UI
    [Header("UI and Timer")]
    public Text infoPane;
    public Text Player1ATK;
    public Text Player1DEF;
    public Text Player2ATK;
    public Text Player2DEF;

    //Timer
    public Text timerText;
    public int timeRemaining;
    public int timerDuration = 15;
    public bool isCountingDown = false;

    void Awake()
    {
        //TODO make these auto calculate from seed stats
        player1Health.MaxVal = 28;
        player2Health.MaxVal = 33;
        player1Health.CurrentVal = 28;
        player2Health.CurrentVal = 33;

        player1Defence.MaxVal = 11;
        player2Defence.MaxVal = 17;
        player1Defence.CurrentVal = 0;
        player2Defence.CurrentVal = 0;

        //player1Health.Initialize();
        //player2Health.Initialize();
    }

    // Use this for initialization
    void Start ()
    {
        //TODO add maths in here to auto generate HP from Seed lvl

        damageCount1 = 0;
        defenceCount1 = 0;

        damageCount2 = 0;
        defenceCount2 = 0;

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
    }

    //Player turns
    public void PlayerOneEndTurn()
    {
        player1ActionComplete = true;
    }

    public void PlayerTwoEndTurn()
    {
        player2ActionComplete = true;
    }

    //Action/Reaction Phases
    private void ActionPhase()
    {
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

        Player1ATK.text = "Overall ATK:" + damageCount1;
        Player1DEF.text = "Overall DEF:" + defenceCount1;
    }

    private void ActionPhaseFailed()
    {
        //If timer runs out, iterate through all seeds to set full defence
        for (int t = 0; t < seedList1.Length; t++)
        {
            seedList1[t].GetComponent<SeedScript>().attackMode = "Defend";
            //Also set text to "Defend"
            seedList1[t].transform.Find("AttackType").GetComponentInChildren<Text>().text = "Defend";
            //Add to defence Val
            defenceCount1 = defenceCount1 + seedList1[t].GetComponent<SeedScript>().defenceVal;
        }
        Debug.Log("<color=red>OUT OF TIME, BUCKO. FULL DEFENSE </color>");
        Player1ATK.text = "Overall ATK:" + damageCount1;
        Player1DEF.text = "Overall DEF:" + defenceCount1;
    }

    private void ReactionPhase()
    {
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
        Player2ATK.text = "Overall ATK:" + damageCount2;
        Player2DEF.text = "Overall DEF:" + defenceCount2;
    }

    private void ReactionPhaseFailed()
    {
        //If timer runs out, set all seeds to defend and set full defence
        for (int t = 0; t < seedList2.Length; t++)
        {
            seedList2[t].GetComponent<SeedScript>().attackMode = "Defend";
            defenceCount2 = defenceCount2 + seedList2[t].GetComponent<SeedScript>().defenceVal;
        }
        Debug.Log("<color=red> OUT OF TIME, GREEN BOY. FULL DEFENCE </color>");
        Player2ATK.text = "Overall ATK:" + damageCount2;
        Player2DEF.text = "Overall DEF:" + defenceCount2;
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
                seedList1[0].GetComponent<SeedScript>().defenceVal += (Mathf.Max(seedList1[t].GetComponent<SeedScript>().attackVal, Mathf.Max(seedList1[t].GetComponent<SeedScript>().defenceVal)));
            }
            else if (seedList1[t].transform.Find("Supp_Text").GetComponentInChildren<Text>().text == "Magic")
            {
                seedList1[2].GetComponent<SeedScript>().defenceVal += (Mathf.Max(seedList1[t].GetComponent<SeedScript>().attackVal, Mathf.Max(seedList1[t].GetComponent<SeedScript>().defenceVal)));
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
                seedList2[0].GetComponent<SeedScript>().defenceVal += (Mathf.Max(seedList2[t].GetComponent<SeedScript>().attackVal, Mathf.Max(seedList2[t].GetComponent<SeedScript>().defenceVal)));
            }
            else if (seedList2[t].transform.Find("Supp_Text").GetComponentInChildren<Text>().text == "Magic")
            {
                seedList2[2].GetComponent<SeedScript>().defenceVal += (Mathf.Max(seedList2[t].GetComponent<SeedScript>().attackVal, Mathf.Max(seedList2[t].GetComponent<SeedScript>().defenceVal)));
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
        //Generate P1 Shield
        player1Defence.CurrentVal = defenceCount1;
        infoPane.text = "Player 1 gains " + (defenceCount1) + " defence points";
        yield return new WaitForSeconds(2f);

        //Generate P2 Shield
        player2Defence.CurrentVal = defenceCount2;
        infoPane.text = "Player 2 gains " + (defenceCount2) + " defence points";
        yield return new WaitForSeconds(2f);

        //Deal P1 damage
        infoPane.text = "Player 1 deals " + (damageCount1) + " damage!";
        if ((player2Defence.CurrentVal > 0) && (damageCount1 > 0))
        {
            player2Defence.CurrentVal = player2Defence.CurrentVal - damageCount1;
            yield return new WaitForSeconds(1f);
        }
        player2Health.CurrentVal = player2Health.CurrentVal - (damageCount1 - defenceCount2);
        yield return new WaitForSeconds(2f);
        //Check for magic support seed
        if (magicSupport2 == true)
        {
            infoPane.text = "Player 2's MAGIC support Seed returns " + (Mathf.CeilToInt((float)damageCount1 / 5)) + " TRUE damage!";
            player1Health.CurrentVal = player1Health.CurrentVal - (Mathf.CeilToInt((float)damageCount1 / 5));
            yield return new WaitForSeconds(2f);
        }

        //Deal P2 damage
        infoPane.text = "Player 2 deals " + (damageCount2) + " damage!";
        if ((player1Defence.CurrentVal > 0) && (damageCount2 > 0))
        {
            player1Defence.CurrentVal = player1Defence.CurrentVal - damageCount2;
            yield return new WaitForSeconds(1f);
        }
        player1Health.CurrentVal = player1Health.CurrentVal - (damageCount2 - defenceCount1);
        yield return new WaitForSeconds(2f);
        //Check for magic support seed
        if (magicSupport1 == true)
        {
            player2Health.CurrentVal = player2Health.CurrentVal - (Mathf.CeilToInt((float)damageCount2 / 5));
            infoPane.text = "Player 1's MAGIC support Seed returns " + (Mathf.CeilToInt((float)damageCount2 / 5)) + " TRUE damage!";
            yield return new WaitForSeconds(2f);
        }

        ResetDamageCounters();
}

    private IEnumerator ResolutionPhase2()
    {
        //Generate P2 Shield
        player2Defence.CurrentVal = defenceCount2;
        infoPane.text = "Player 2 gains " + (defenceCount2) + " defence points";
        yield return new WaitForSeconds(2f);

        //Generate P1 Shield
        player1Defence.CurrentVal = defenceCount1;
        infoPane.text = "Player 1 gains " + (defenceCount1) + " defence points";
        yield return new WaitForSeconds(2f);

        //Deal P2 damage
        infoPane.text = "Player 2 deals " + (damageCount2) + " damage!";
        if ((player1Defence.CurrentVal > 0) && (damageCount2 > 0))
        {
            player1Defence.CurrentVal = player1Defence.CurrentVal - damageCount2;
            yield return new WaitForSeconds(1f);
        }
        player1Health.CurrentVal = player1Health.CurrentVal - (damageCount2 - defenceCount1);
        yield return new WaitForSeconds(2f);
        //Check for magic support seed
        if (magicSupport1 == true)
        {
            infoPane.text = "Player 1's MAGIC support Seed returns " + (Mathf.CeilToInt((float)damageCount2 / 5)) + " TRUE damage!";
            player2Health.CurrentVal = player2Health.CurrentVal - (Mathf.CeilToInt((float)damageCount2 / 5));
            yield return new WaitForSeconds(2f);
        }

        //Deal P1 damage
        infoPane.text = "Player 1 deals " + (damageCount1) + " damage!";
        if ((player2Defence.CurrentVal > 0) && (damageCount1 > 0))
        {
            player2Defence.CurrentVal = player2Defence.CurrentVal - damageCount1;
            yield return new WaitForSeconds(1f);
        }
        player2Health.CurrentVal = player2Health.CurrentVal - (damageCount1 - defenceCount2);
        yield return new WaitForSeconds(2f);
        //Check for magic support seed
        if (magicSupport2 == true)
        {
            player1Health.CurrentVal = player1Health.CurrentVal - (Mathf.CeilToInt((float)damageCount1 / 5));
            infoPane.text = "Player 2's MAGIC support Seed returns " + (Mathf.CeilToInt((float)damageCount1 / 5)) + " TRUE damage!";
            yield return new WaitForSeconds(2f);
        }

        ResetDamageCounters();
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
        Player1ATK.text = "Overall ATK:";
        Player1DEF.text = "Overall DEF:";
        Player2ATK.text = "Overall ATK:";
        Player2DEF.text = "Overall DEF:";
    }

    //Combat Manager
    public IEnumerator BattleQueue()
    {
        //  ***Phase One***
        infoPane.text = "<color=red>Red Player's Action </color>";
           player1ActionComplete = false;
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
        infoPane.text = "<color=green> Green Player's Reaction </color>";
        player2ActionComplete = false;
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
        yield return StartCoroutine( ResolutionPhase1() );
        

        //**End here if somebody is dead!***
        if (player1Health.CurrentVal <= 0  || player2Health.CurrentVal <= 0)
        {
            GameOver();
            infoPane.text = "Game Over, Man! Game Over";
            yield break;
        }

        //  ***Phase Three***
        infoPane.text = "<color=green> Green Player's Action </color>";
        player2ActionComplete = false;
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
        infoPane.text = "<color=red>Red Player's Reaction </color>";
        player1ActionComplete = false;
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