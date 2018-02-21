using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeedScript : MonoBehaviour
{

    [Header("Public buttons and text")]
    public Button thisSeed;
    public Button support1;
    public Button support2;
    public Text supportText;
    public Text statsText;


    [Header("Seed Attack mode")]
    public string attackMode;
    private string[] seedPosition;

    [Header("Stats")]
    public int attackVal;
    public int defenceVal;
    [SerializeField]
    private int defaultAttackVal;
    [SerializeField]
    private int defaultDefenceVal;

    int t = 0;

    // Use this for initialization
    void Start()
    {
        //Set default Values
        defaultAttackVal = attackVal;
        defaultDefenceVal = defenceVal;

        seedPosition = new string[] { "Attack", "Defend", "Support" };

        thisSeed.transform.Find("AttackType").GetComponentInChildren<Text>().text = seedPosition[t];

        SetStatsText();
        AttackType();
    }

    public void ResetSeedStats()
    {
        attackVal = defaultAttackVal;
        defenceVal = defaultDefenceVal;
    }

    public void SetStatsText()
    {
        statsText.text = "ATT: " + attackVal + " | DEF: " + defenceVal;
    }

    public void AttackType()
    {

        t = (t + 1) % 3;

        thisSeed.transform.Find("AttackType").GetComponentInChildren<Text>().text = seedPosition[t];

        attackMode = seedPosition[t];

        if (!thisSeed.name.StartsWith("Magic_"))
        {
            if (seedPosition[t] == "Support")
            {
                support1.gameObject.SetActive(true);
                support2.gameObject.SetActive(true);
                supportText.gameObject.SetActive(true);

                supportText.text = support1.GetComponentInChildren<Text>().text;
            }
            else
            {
                support1.gameObject.SetActive(false);
                support2.gameObject.SetActive(false);
                supportText.gameObject.SetActive(false);
            }
        }

    }

    public void SupportButton1()
    {
        supportText.text = support1.GetComponentInChildren<Text>().text;
    }

    public void SupportButton2()
    {
        supportText.text = support2.GetComponentInChildren<Text>().text;
    }

    public void StartupAnim()
    {
        thisSeed.GetComponent<Animator>().Play("Startup", -1, 0.0f);
    }

    public void AttackAnim(int a)
    {
        if (a == 0)
        {
            thisSeed.GetComponent<Animator>().Play("Might_ATK", -1, 0.0f);
        }
        if (a == 1)
        {
            thisSeed.GetComponent<Animator>().Play("Mind_ATK", -1, 0.0f);
        }
        if (a == 2)
        {
            thisSeed.GetComponent<Animator>().Play("Magic_ATK", -1, 0.0f);
        }
    }

    public void DefenceAnim(int a)
    {
        if (a == 0)
        {
            thisSeed.GetComponent<Animator>().Play("Might_DEF", -1, 0.0f);
        }
        if (a == 1)
        {
            thisSeed.GetComponent<Animator>().Play("Mind_DEF", -1, 0.0f);
        }
        if (a == 2)
        {
            thisSeed.GetComponent<Animator>().Play("Magic_DEF", -1, 0.0f);
        }
    }

    public void SupportAnim(int a)
    {
        if (a == 0)
        {
            thisSeed.GetComponent<Animator>().Play("Might_SUPP", -1, 0.0f);
        }
        if (a == 1)
        {
            thisSeed.GetComponent<Animator>().Play("Mind_SUPP", -1, 0.0f);
        }
        if (a == 2)
        {
            thisSeed.GetComponent<Animator>().Play("Magic_SUPP", -1, 0.0f);
        }
    }
}
