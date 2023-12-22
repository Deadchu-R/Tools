using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questText;
    [SerializeField] private TextMeshProUGUI questText2;
    [SerializeField] private FontStyles fontStyle;
    private FontStyles _defaultFontStyle;
    private Quest _currentQuest;
    private Quest _previousQuest;

    private void Awake()
    {
        _defaultFontStyle = questText.fontStyle;
    }
    public void SetQuest(Quest quest)
    {
        if (gameObject.activeSelf == false) gameObject.SetActive(true);
        _currentQuest = quest;
        SetQuestText(_currentQuest.QuestDescription);
    }

    
    private void NextQuest()
    {
        questText.text = _previousQuest.QuestDescription;
        questText2.text = _currentQuest.QuestDescription; 
        StrikeThoughQuestText(questText);
        questText.gameObject.SetActive(true);
        questText2.gameObject.SetActive(true);
    }

    private void StrikeThoughQuestText(TextMeshProUGUI text) 
    {
        text.fontStyle = FontStyles.Strikethrough;
    }
    

    private void SetQuestText(string questDescription)
    {
        questText.text = questDescription;
        questText.gameObject.SetActive(true);
    }


    private void ResetQuestSequence()
    {
        questText.fontStyle = fontStyle;
        questText.gameObject.SetActive(false);
        questText2.gameObject.SetActive(false);
    }

    public void FinishedQuest(Quest quest)
    {
        if (quest.NextQuest != null)
        {
            _currentQuest = quest.NextQuest;
            _previousQuest = quest;
            NextQuest();
        }
        else
        {
            ResetQuestSequence();
        }
    }
}