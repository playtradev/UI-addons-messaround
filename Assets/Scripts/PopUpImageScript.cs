using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopUpImageScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
    public void destroySelf()
    {
        Destroy(gameObject);
    }

    public void loadBattleScene()
    {
        SceneManager.LoadScene("Combat", LoadSceneMode.Single);
    }
}
