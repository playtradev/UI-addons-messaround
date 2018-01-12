using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeedScript : MonoBehaviour {

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

    int t = 0;

    // Use this for initialization
    void Start ()
    {
        statsText.text = "ATT: " + attackVal + " | DEF: " + defenceVal;

        seedPosition = new string[] { "Attack", "Defend", "Support" };

        thisSeed.transform.Find("AttackType").GetComponentInChildren<Text>().text = seedPosition[t];

        AttackType();
    }

    public void AttackType()
    {

        t = (t + 1) % 3;
    
        thisSeed.transform.Find("AttackType").GetComponentInChildren<Text>().text = seedPosition[t];

        attackMode = seedPosition[t];

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

    public void SupportButton1()
    {
        supportText.text = support1.GetComponentInChildren<Text>().text;
    }

    public void SupportButton2()
    {
        supportText.text = support2.GetComponentInChildren<Text>().text;
    }

}
