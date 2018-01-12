using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class userNameScript : MonoBehaviour {

    public InputField InputName;
    public Text userNameText;
    public string userName;

	// Use this for initialization
	void Start () {
		
	}
	
    public void GetUsername()
    {
        userName = userNameText.text;

        Debug.Log("<color=green>YEA SURE:</color>" + userName);
    }

}
