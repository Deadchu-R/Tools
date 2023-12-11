using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Diagnostics;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

public class SpeechBubbleUI : MonoBehaviour
{
    [Header("Events")] [SerializeField] private UnityEvent onEndSpeech;
    [SerializeField] private UnityEvent onStartSpeech;

    #region Buttons

    [Header("Buttons")] 
    public Button BackButton;
    public Button NextButton;

    #endregion

    #region Speech Bubble UI Components

    [Header("Speech Bubble UI Components")] [SerializeField]
    private Image NPCIcon;

    [SerializeField] private TextMeshProUGUI NPCName;

    #endregion

    [Header("SpeechBubble Settings")] [SerializeField]
    private Sprite NPCIconSprite;

    private Sprite NPCTalkingIconSprite;

    #region Side Scripts

    [Header("Side Scripts")] [SerializeField]
    private SimpleTypeWriterEffect typeWriterEffect;

    [SerializeField] private DialougeOptions dialogueOptionsScript;

    #endregion




    #region Data Variables

    [Header("for testing only")]
    [SerializeField]
    private int currentPageIndex = 0;

    [SerializeField] private int LastPageIndex = 0;
    [SerializeField] private Page currentPage;
    [SerializeField] private Page lastPage;
    [SerializeField] private Sheet firstSheet;
    [SerializeField] private Sheet currentSheet;
    [SerializeField] private Sheet lastSheet;

    public int CurrentPageIndex
    {
        get => currentPageIndex;
       private set
        {
            currentPageIndex = value;
            SetButtonInteractable(BackButton,currentPageIndex > 0);
            SetButtonInteractable(NextButton,currentPageIndex < _pages.Length - 1);
        }
    }

    private int lastPageIndex
    {
        get => LastPageIndex;
        set => LastPageIndex = value;
    }


    private Page CurrentPage { get => currentPage;
        set => currentPage = value; }

    private Page LastPage{ get => lastPage;
         set => lastPage = value; }
    public Sheet FirstSheet{ get => firstSheet; private set => firstSheet = value; }
     public Sheet CurrentSheet{ get => currentSheet; private set => currentSheet = value; }
     public Sheet LastSheet{ get => lastSheet; private set => lastSheet = value; }
     private Page[] _pages;
    [SerializeField] private NPC_DATA _currentNpcData;
    private bool _isDialogueActive = false;

    #endregion



    /// <summary>
    /// will set the sheet to the current sheet 
    /// </summary>
    /// <param name="sheet">scriptable object made from Sheet.cs</param>
    /// <param name="npcD">is NPC_DATA which is the NPC itself</param>
    /// <param name="pageIndex">the Index of the page it will start at</param> 
    public void SetSheetUI(Sheet sheet, int pageIndex, NPC_DATA npcData = null)
    {
        if (firstSheet == null) firstSheet = sheet;
        var caller = new StackTrace().GetFrame(1).GetMethod().Name;
        HandleCallerActions(sheet, caller, pageIndex);
        onStartSpeech.Invoke(); 
        if (npcData != null) _currentNpcData = npcData;
        UpdateCurrentAndLastSheet(sheet);

        CurrentSheet = sheet;
        _pages = sheet.pages;
        SetPageSequence(pageIndex);
    }

    private void UpdateCurrentAndLastSheet(Sheet sheet)
    {
        if (CurrentSheet == null) return;
        // Only update lastSheet if it's a new sheet
        if (CurrentSheet != sheet) LastSheet = CurrentSheet;
        currentPageIndex = Array.IndexOf(CurrentSheet.pages, LastPage); // Use the last page index for the old sheet
        ResetSheet();
    }

    private void HandleCallerActions(Sheet sheet, string caller, int pageIndex)
    {
        if (caller != "NextPage" && caller != "PreviousPage")
        {
            lastPageIndex = currentPageIndex;
            SetLastPage();
        }

        if (caller == "PreviousPage") 
        {
            lastPageIndex = pageIndex - 1;
        }
    }

    /// <summary>
    /// will set the text sequence to the current page
    /// </summary>
    /// <param name="pageIndex"> Index to set the currentPage</param>
    private void SetPageSequence(int pageIndex = 0)
    {
        if (_isDialogueActive) return;
        _isDialogueActive = true;
        CurrentPage = _pages[pageIndex];
        SetPage(CurrentPage);
        if (_pages.Length > 1) ResetNextButtonListeners();
        else NextButton.onClick.AddListener(ClosePanel);
    }

    /// <summary>
    /// will set the page to the current page and will set the NPC info
    /// </summary>
    /// <param name="pageToSet">the Page to bet set to</param>
    private void SetPage(Page pageToSet)
    {
        ResetPageState();
        SetCurrentPageIndex(PageIndexSetter.CurrentPageAtSheet, pageToSet);
        SetButtonInteractable(BackButton, FirstSheet.pages[0] != pageToSet);
        CurrentPage = pageToSet;
        PageActions(CurrentPage);
        SetNPCInfo();
        typeWriterEffect.SetText(CurrentPage);
    }
/// <summary>
/// will reset the Page to default state
/// </summary>
    private void ResetPageState()
    {
        ResetNextButtonListeners();
        ResetBackButtonListeners();
        typeWriterEffect.StopText();
    }

    /// <summary>
    /// will set the NPC name and icon
    /// </summary>
    private void SetNPCInfo()
    {
        NPCName.text = CurrentPage.NPCInfo.Name;
        if (!CurrentPage.NPCInfo.ShowIcon) NPCIcon.gameObject.SetActive(false);
        else
        {
            NPCIcon.gameObject.SetActive(true);
            NPCIconSprite = CurrentPage.NPCInfo.Icon;
            NPCTalkingIconSprite = CurrentPage.NPCInfo.TalkingIcon;
            NPCIcon.sprite = CurrentPage.NPCInfo.Icon;
        }
    }

    /// <summary>
    /// will set the dialogue options to the buttons
    /// </summary>
    /// <param name="page">Dialogue Page for DialogueOptions SetDialogueOptions</param>
    public void SetDialogueOptions(DialoguePage page)
    {
        dialogueOptionsScript.SetDialogueOptions(page);
    }

    /// <summary>
    /// will run only after TypeWriterEffect is finished typing (through the event)
    /// </summary>
    public void FinishedTyping()
    {
        PageActions(CurrentPage);
    }

    /// <summary>
    /// Will check if the page has any actions to do and will do them
    /// </summary>
    /// <param name="page"></param>
    private void PageActions(Page page)
    {
        page.PageAction(this);
    }
public bool isCurrentPageIndexPlusOneSmallerThanPagesLength()
    {
        return currentPageIndex + 1 <= CurrentSheet.pages.Length;
    }
    /// <summary>
    /// will close all the non default page actions
    /// </summary>
    public void CloseNonDefaultPageActions()
    {
        dialogueOptionsScript.SetActive(false);
    }


    /// <summary>
    /// Will change the NPC icon to the talking icon if the NPC is talking
    /// </summary>
    public void TalkAnimation()
    {
        if (!CurrentPage.NPCInfo.ShowIcon) return;
        NPCIcon.sprite = NPCIcon.sprite == NPCIconSprite ? NPCTalkingIconSprite : NPCIconSprite;
    }

    /// <summary>
    /// will set the Button interactable according to the bool
    /// </summary>
    /// <param name="interactable">the bool</param>
    public void SetButtonInteractable(Button button, bool interactable)
    {
        button.interactable = interactable;
    }


    private void SetLastPage()
    {
        LastPage = CurrentPage; // Update lastPage here
    }

    /// <summary>
    /// checks if it is the last page of the sheet and will change the next button Listener accordingly 
    /// </summary>
    private void IsLastPage()
    {
        if (currentPageIndex + 1 < _pages.Length) return;
        NextButton.onClick.RemoveListener(NextPage);
        NextButton.onClick.AddListener(ClosePanel);
    }

    /// <summary>
    /// will Move To the next page
    /// </summary>
    private void NextPage()
    {
        SetLastPage();
        SetCurrentPageIndex(PageIndexSetter.Next);
        Page nextPage = _pages[currentPageIndex];

        if (!CurrentSheet.ContainsPage(nextPage)) return; // normal set page
        IsLastPage();
        SetPage(nextPage);
    }

    /// <summary>
    ///  will Move To the previous page
    /// </summary>
    /// <summary>
    ///  will Move To the previous page
    /// </summary>
    private void PreviousPage()
    {
        if (CurrentSheet.ContainsPage(LastPage)) // Normal Set Page
        {
            SetCurrentPageIndex(PageIndexSetter.Previous);
            SetPage(LastPage);
            NextButton.onClick.RemoveListener(ClosePanel);
        }
        else if (LastSheet != null && LastSheet.ContainsPage(LastPage)) // new sheet Set Page
        {
            LastPage = LastSheet.pages[currentPageIndex];
            SetSheetUI(LastSheet, lastPageIndex );
        }
    }

    private void ResetBackButtonListeners()
    {
        BackButton.onClick.RemoveAllListeners();
        BackButton.onClick.AddListener(PreviousPage);
    }

    private void ResetNextButtonListeners()
    {
        NextButton.onClick.RemoveAllListeners();
        NextButton.onClick.AddListener(NextPage);
    }

    private void SetCurrentPageIndex(PageIndexSetter setter, Page page = null)
    {
        switch (setter)
        {
            case PageIndexSetter.Next:
                if (currentPageIndex < _pages.Length - 1) currentPageIndex++;
                break;
            case PageIndexSetter.Previous:
                if (currentPageIndex > 0) currentPageIndex--;
                break;
            case PageIndexSetter.CurrentPageAtSheet:
                currentPageIndex = Array.IndexOf(CurrentSheet.pages, page);
                break;
            case PageIndexSetter.Reset:
                currentPageIndex = 0;
                break;
        }
    }


    /// <summary>
    ///  will close the panel and invoke the onEndSpeech event
    /// </summary>
    private void ClosePanel()
    {
        CurrentPage.FinishedPage();
        gameObject.SetActive(false);
        onEndSpeech.Invoke();
        ResetBubble();
    }

    /// <summary>
    ///  will reset the bubble to the default state
    /// </summary>
    private void ResetBubble()
    {
        ResetNPCData();
        ResetSheet();
        ResetVariables();
        BackButton.onClick.RemoveAllListeners();
    }
    private void ResetNPCData()
    {
        if (_currentNpcData != null) _currentNpcData.SetIsNpcInteractionStarted(false);
        currentSheet = null;
    }
private void ResetVariables()
    {
        SetCurrentPageIndex(PageIndexSetter.Reset);
        lastPageIndex = 0;
        CurrentPage = null;
        LastPage = null;
        LastSheet = null;
        firstSheet = null;
    }
    private void ResetSheet()
    {
        CurrentSheet = null;
        NextButton.onClick.RemoveAllListeners();
        _isDialogueActive = false;
    }
}
public enum PageIndexSetter
{
    Next,
    Previous,
    CurrentPageAtSheet,
    Reset
    
}