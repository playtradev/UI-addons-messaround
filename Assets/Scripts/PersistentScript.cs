using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentScript : MonoBehaviour {

    public string PlayerUsername;
    public bool Initialised;
    public GameObject DialogueHandlerRef;

    [Header("AI stats")]
    public int[] enemyStats = new int[6];



    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    //TODO Push UserName currently driven from Dialogue Handler Script (if currentLine == 13 in Checkline()) - seems like bad practice...

    public void PushUserName()
    {
        PlayerUsername = DialogueHandlerRef.GetComponent<DialogueHandlerScript>().userName;

        Initialised = true;
    }
}
