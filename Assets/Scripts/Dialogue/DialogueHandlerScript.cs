using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueHandlerScript : MonoBehaviour {

    [Header("Dialogue")]
    public GameObject ScrollSnapRef;
    public Button nextButton;
    public TextAsset textFile;
    public Text textBox;
    private string[] mineDialogue;
    public int currentLine;
    private int endAtLine;

    [Header("GPS")]
    public Button recentreMapButton;

    [Header("Username Input")]
    public GameObject inputName; //to show + hide
    public GameObject submitNameButton;
    //public InputField InputName;
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
    private int currentFace;

    //private string[] locationStringsX = new string[3];

    // Use this for initialization
    void Start()
    {
        currentLine = 0;
        currentFace = 0;

        ScrollSnapRef.GetComponent<ScrollRect>().vertical = false;

        if (textFile != null)
        {
        mineDialogue = (textFile.text.Split('\n'));
        }

        if (endAtLine == 0)
        {
        endAtLine = mineDialogue.Length - 1;
        }

        CheckLine();

        if (GameObject.Find("PersistentInfo").GetComponent<PersistentScript>().Initialised == true)
        {
            userName = GameObject.Find("PersistentInfo").GetComponent<PersistentScript>().PlayerUsername;
            currentLine = 11;
            currentFace = 11;
            CheckLine();
        }
    }

    //Updates Dialogue +1
    private IEnumerator NextLine()
    {
        string textBoxText = mineDialogue[currentLine++].Replace("PLAYER_USERNAME", userName).Replace("_NEW_LINE_", "\n");
        string currentText = "";

        for (int i = 0; i < textBoxText.Length; i++)
        {
            currentText = textBoxText.Substring(0, i);
            textBox.text = currentText;
            yield return new WaitForSeconds(0.05f);
        }      
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
            StopAllCoroutines();
            StartCoroutine (NextLine());
            NextFace();

            if (currentLine == 8)
            {
                inputName.SetActive(true);
                submitNameButton.SetActive(true);
                nextButton.gameObject.SetActive(false);
            }
            if ((currentLine == 9) && (userName == ""))
            {
                GenerateRandomUsername();
                submitNameButton.SetActive(false);
               }
            if ((currentLine == 9) && (userName != ""))
            {
                inputName.SetActive(false);
                submitNameButton.SetActive(false);
                nextButton.gameObject.SetActive(true);
            }
            if (currentLine == 10)
            {
            // TODO this if is hacky AF.
                if (userName != null)
                {
                GameObject.Find("PersistentInfo").GetComponent<PersistentScript>().PushUserName();
                }

                nextButton.gameObject.SetActive(false);
                ScrollSnapRef.GetComponent<ScrollRect>().vertical = true;
            }

            // Jumps to here when already initialised
            if (currentLine == 12)
            {
            nextButton.gameObject.SetActive(false);
            ScrollSnapRef.GetComponent<ScrollRect>().vertical = true;
            }
    }

    //Gets Username from Box
    public void GetUsername()
    {
        userName = userNameText.text;

        Debug.Log("<color=green>USERNAME IS:</color>" + userName);
    }

    //Simply calls Checkline(), but from submit button
    public void SubmitName()
    {
        CheckLine();
    }

    //Generates a Random Username
    private void RandomUsername()
    {
        string[] randomUserNameList = { "Ducky", "Bucko", "Bebe", "Geezer", "Matey", "Buddy", "Muffin", "Puddin'", "My Precious", "BAE", "Bumpkin", "Dashing", "Young Skywalker", "Hopper", "The Chosen One", "Kid Righteous" };

        randomUserName = randomUserNameList[Random.Range(0, randomUserNameList.Length - 1)];
    }

    public void GenerateRandomUsername()
    {
        string lastrandomName = randomUserName;

        RandomUsername();
        if (lastrandomName == randomUserName)
        {
            //Reroll because name is the same
            GenerateRandomUsername();
        }
        else
        {
            textBox.text = mineDialogue[13].Replace("PLAYER_USERNAME", userName).Replace("_NEW_LINE_", "\n").Replace("_RANDOM_USERNAME_", randomUserName);
            currentFaceImage.sprite = spriteList[1];

            //Hide and show correct buttons
            inputName.SetActive(false);
            nextButton.gameObject.SetActive(false);
            newRandomButton.gameObject.SetActive(true);
            acceptRandomButton.gameObject.SetActive(true);
            inputOwnNameButton.gameObject.SetActive(true);
        }


    }

    public void AcceptRandomName()
    {
        currentLine = 9;
        currentFace = 9;

        userName = randomUserName;
        CheckLine();

        //Hide and show correct buttons
        //nextButton.gameObject.SetActive(true);
        newRandomButton.gameObject.SetActive(false);
        acceptRandomButton.gameObject.SetActive(false);
        inputOwnNameButton.gameObject.SetActive(false);
    }

    public void PickOwnName()
    {
        currentLine = 7;
        currentFace = 7;

        CheckLine();

        //Hide buttons
        newRandomButton.gameObject.SetActive(false);
        acceptRandomButton.gameObject.SetActive(false);
        inputOwnNameButton.gameObject.SetActive(false);
    }

}