using UnityEngine;
using UnityEngine.Events;
[System.Serializable]
public class PageEvent : UnityEvent<Page> { }

public class Page : ScriptableObject
{
 public NPC_ID NPCInfo;
 public string Text;
 public string QuestionText;
 [SerializeField] private PageEvent onFinishedPage = new PageEvent();
 public void FinishedPage()
 {
  onFinishedPage.RemoveAllListeners();
  onFinishedPage.AddListener(GameObject.Find("GameManager").GetComponent<GameManager>().PageAction);
  onFinishedPage.Invoke(this);
 }
 
 public virtual void PageAction(SpeechBubbleUI speechBubble)
 {
 } 
 
 
 public bool PageEquals(Page other)
 {
  return Text == other.Text && QuestionText == other.QuestionText && NPCInfo == other.NPCInfo;
 }
 
}





