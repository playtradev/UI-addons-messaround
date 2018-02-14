using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetInteractableScript : MonoBehaviour
{

    [Header("Player 1 UI elements")]
    [SerializeField]
    private Button[] playerOneButtons;
    [SerializeField]
    private Image[] playerOneImages;

    [Header("Player 2 UI elements")]
    [SerializeField]
    private Button[] playerTwoButtons;
    [SerializeField]
    private Image[] playerTwoImages;

    [Header("Shared References")]
    [SerializeField]
    private BattleManagerScript BattleManagerScriptRef;
    [SerializeField]
    private Sprite[] faceAnimations;
    [SerializeField]
    private Image TimerRef;
    [SerializeField]
    private Image CountdownRef;


    //Setting Seeds Interactable or Not
    public void SetInteractableObjects()
    {
        TimerRef.GetComponent<Image>().color = new Color(0, 0, 0, 1f);
        CountdownRef.GetComponent<Image>().color = new Color(0, 0, 0, 1f);

        if (BattleManagerScriptRef.player1ActionComplete == false)
        {
            for (int t = 0; t < playerOneButtons.Length; t++)
            {
                playerOneButtons[t].GetComponent<Button>().interactable = true;
            }

            for (int t = 0; t < playerOneImages.Length; t++)
            {
                Color temp = playerOneImages[t].GetComponent<Image>().color;
                temp.a = 1.0f;
                playerOneImages[t].GetComponent<Image>().color = temp;

                if (playerOneImages[t].name == "FaceImage")
                {
                    playerOneImages[t].GetComponent<Image>().sprite = faceAnimations[1];
                }
            }
        }

        else if (BattleManagerScriptRef.player1ActionComplete == true)
        {
            for (int t = 0; t < playerOneButtons.Length; t++)
            {
                playerOneButtons[t].GetComponent<Button>().interactable = false;
            }

            for (int t = 0; t < playerOneImages.Length; t++)
            {
                Color temp = playerOneImages[t].GetComponent<Image>().color;
                temp.a = 0.4f;
                playerOneImages[t].GetComponent<Image>().color = temp;

                if (playerOneImages[t].name == "FaceImage")
                {
                    playerOneImages[t].GetComponent<Image>().sprite = faceAnimations[0];
                }
            }
        }


        if (BattleManagerScriptRef.player2ActionComplete == false)
        {
            for (int t = 0; t < playerTwoButtons.Length; t++)
            {
                playerTwoButtons[t].GetComponent<Button>().interactable = true;
            }

            for (int t = 0; t < playerTwoImages.Length; t++)
            {
                Color temp = playerTwoImages[t].GetComponent<Image>().color;
                temp.a = 1.0f;
                playerTwoImages[t].GetComponent<Image>().color = temp;

                if (playerTwoImages[t].name == "FaceImage")
                {
                    playerTwoImages[t].GetComponent<Image>().sprite = faceAnimations[1];
                }
            }
        }

        else if (BattleManagerScriptRef.player2ActionComplete == true)
        {
            for (int t = 0; t < playerTwoButtons.Length; t++)
            {
                playerTwoButtons[t].GetComponent<Button>().interactable = false;
            }

            for (int t = 0; t < playerTwoImages.Length; t++)
            {                
                Color temp = playerTwoImages[t].GetComponent<Image>().color;
                temp.a = 0.4f;
                playerTwoImages[t].GetComponent<Image>().color = temp;

                if (playerTwoImages[t].name == "FaceImage")
                {
                    playerTwoImages[t].GetComponent<Image>().sprite = faceAnimations[0];
                }                
            }
        }
    }

    public void ResolutionPhaseInteractables()
    {
        TimerRef.GetComponent<Image>().color = new Color(0, 0, 0, 0.2f);
        CountdownRef.GetComponent<Image>().color = new Color(0, 0, 0, 0.2f);

        for (int t = 0; t < playerOneButtons.Length; t++)
        {
            playerOneButtons[t].GetComponent<Button>().interactable = false;
        }

        for (int t = 0; t < playerOneImages.Length; t++)
        {
            Color temp = playerOneImages[t].GetComponent<Image>().color;
            temp.a = 1.0f;
            playerOneImages[t].GetComponent<Image>().color = temp;

            if (playerOneImages[t].name == "FaceImage")
            {
                playerOneImages[t].GetComponent<Image>().sprite = faceAnimations[0];
            }
        }

        for (int t = 0; t < playerTwoButtons.Length; t++)
        {
            playerTwoButtons[t].GetComponent<Button>().interactable = false;
        }

        for (int t = 0; t < playerTwoImages.Length; t++)
        {
            Color temp = playerTwoImages[t].GetComponent<Image>().color;
            temp.a = 1.0f;
            playerTwoImages[t].GetComponent<Image>().color = temp;

            if (playerTwoImages[t].name == "FaceImage")
            {
                playerTwoImages[t].GetComponent<Image>().sprite = faceAnimations[0];
            }
        }
    }
}
