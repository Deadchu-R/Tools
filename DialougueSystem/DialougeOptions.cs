using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialougeOptions : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    [SerializeField] private SpeechBubbleUI speechBubbleScript; 
    private TextMeshProUGUI[] buttonTexts;
    private Page[] pages;

    private void Awake()
    {
    
    }
    

    /// <summary>
    ///  will set the dialogue options according to the page: (pages) array that is passed
    /// </summary>
    /// <param name="page"></param>
    public void SetDialogueOptions(DialoguePage page)
    {
        pages = page.Pages;
        SetButtons(page);
        SetActive(true);
        speechBubbleScript.SetButtonInteractable(speechBubbleScript.NextButton,false);
    }

    /// <summary>
    ///  will set the buttons text according to the page options
    /// </summary>
    /// <param name="page"></param>
    private void SetButtons(DialoguePage page)
    {
    
        //disabling all the buttons
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }
     
        // adding the buttons texts acording to how much buttons added in the inspector
        buttonTexts = new TextMeshProUGUI[buttons.Length];
        for (int i = 0; i < buttons.Length; i++) 
        {
            buttonTexts[i] = buttons[i].GetComponentInChildren<TextMeshProUGUI>();
        }
        // setting the buttons texts according to the page options (which are texts)
        for (int i = 0; i < page.Options.Length ; i++)
        {
            buttonTexts[i].text = page.Options[i];
            buttons[i].gameObject.SetActive(true); // enabling the buttons that are needed
        }
  
    }


    /// <summary>
    ///  will set the options Buttons interactable according to the state
    /// </summary>
    /// <param name="activeState"></param>
    public void SetActive(bool activeState)
    {
        gameObject.SetActive(activeState);
    }

    /// <summary>
    /// a method to use the option that is clicked
    /// </summary>
    /// <param name="optionIndex"></param>
    public void UseOption(int optionIndex)
    {
        if (optionIndex < 0 || optionIndex >= pages.Length)
        {
            Debug.LogError("Option index is out of bounds, seems you forgot to set the option index in the inspector");
            return;
        }
        
        Sheet sheet = new Sheet(1);
        sheet.pages = new Page[1];
        sheet.pages[0] = pages[optionIndex];
        speechBubbleScript.SetSheetUI(sheet,0);
        SetActive(false);
    }
}