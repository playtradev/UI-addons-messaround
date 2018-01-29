using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOverButtonSceneManager : MonoBehaviour {

    public void ReturnHome()
    {
        SceneManager.LoadScene("Startup(rotated)", LoadSceneMode.Single);
    }
}
