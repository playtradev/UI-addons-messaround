using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentScript : MonoBehaviour {

    public string PlayerUsername;
    public bool Initialised;
    public GameObject DialogueHandlerRef;


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
