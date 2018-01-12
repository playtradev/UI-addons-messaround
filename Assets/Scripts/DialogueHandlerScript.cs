using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueHandlerScript : MonoBehaviour {

    [Header("Dialogue")]
    public Button nextButton;
    public TextAsset textFile;
    public Text textBox;
    public string[] mineDialogue;
    public int currentLine;
    public int endAtLine;

    [Header("GPS")]
    public Button gpsButton;

    [Header("Username Input")]
    public GameObject inputName; //to show + hide
    public InputField InputName;
    public Text userNameText;
    public string userName;
    
    [Header("Random Username Branch")]
    public Button newRandomButton;
    public Button acceptRandomButton;
    public Button inputOwnNameButton;
    private string randomUserName;

    [Header("Face Sprites")]
    public Image currentFaceImage;
    public Sprite[] spriteList;
    public int currentFace;


    // Use this for initialization
    void Start()
    {
        currentLine = 0;
        currentFace = 0;

        if (textFile != null)
        {
        mineDialogue = (textFile.text.Split('\n'));
        }

        if (endAtLine == 0)
        {
        endAtLine = mineDialogue.Length - 1;
        }

        CheckLine();
    }

    //Updates Dialogue +1
    private void NextLine()
    {
        textBox.text = mineDialogue[currentLine++].Replace("PLAYER_USERNAME", userName).Replace("_NEW_LINE_", "\n");
    }

    //Updates Face +1
    public void NextFace()
    {
            currentFaceImage.sprite = spriteList[currentFace++];
    }

    //Checks Dialogue       
    public void CheckLine()
    {
            //Calls for Next Line and Face
            NextLine();
            NextFace();

            if (currentLine == 11)
            {
                inputName.SetActive(true);
            }
            if ((currentLine == 12) && (userName == ""))
            {
                GenerateRandomUsername();
            }
            if ((currentLine == 12) && (userName != ""))
            {
                inputName.SetActive(false);
            }
            if (currentLine == 14)
            {
                gpsButton.gameObject.SetActive(true);
            }
            if (currentLine == 15)
            {
                gpsButton.gameObject.SetActive(false);
            }
            if (currentLine == 16)
            {
                nextButton.gameObject.SetActive(false);
            }
    }


    //Gets Username from Box
    public void GetUsername()
    {
        userName = userNameText.text;

        Debug.Log("<color=green>USERNAME IS:</color>" + userName);
    }

    //Generates a Random Username
    private void RandomUsername()
    {
        string[] randomUserNameList = { "Ducky", "Bucko", "Bebe", "Geezer", "Matey", "Buddy", "Muffin", "Puddin'", "My Precious", "BAE", "Bumpkin", "Dashing", "Young Skywalker", "Hopper", "The Chosen One", "Kid Righteous" };

        randomUserName = randomUserNameList[Random.Range(0, randomUserNameList.Length - 1)];
    }

    public void GenerateRandomUsername()
    {
        RandomUsername();
        textBox.text = mineDialogue[19].Replace("PLAYER_USERNAME", userName).Replace("_NEW_LINE_", "\n").Replace("_RANDOM_USERNAME_", randomUserName);
        currentFaceImage.sprite = spriteList[3];

        //Hide and show correct buttons
        inputName.SetActive(false);
        nextButton.gameObject.SetActive(false);
        newRandomButton.gameObject.SetActive(true);
        acceptRandomButton.gameObject.SetActive(true);
        inputOwnNameButton.gameObject.SetActive(true);
    }

    public void AcceptRandomName()
    {
        userName = randomUserName;
        CheckLine();

        //Hide and show correct buttons
        nextButton.gameObject.SetActive(true);
        newRandomButton.gameObject.SetActive(false);
        acceptRandomButton.gameObject.SetActive(false);
        inputOwnNameButton.gameObject.SetActive(false);
    }

    public void PickOwnName()
    {
        currentLine = 10;
        currentFace = 10;

        CheckLine();

        //Hide and show correct buttons
        inputName.SetActive(true);
        nextButton.gameObject.SetActive(true);
        newRandomButton.gameObject.SetActive(false);
        acceptRandomButton.gameObject.SetActive(false);
        inputOwnNameButton.gameObject.SetActive(false);
    }

}