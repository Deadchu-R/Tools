using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quests/defaultQuest")]
public class Quest : ScriptableObject
{

   public string questName;
   public string QuestDescription;
   public int QuestID;
   public Quest NextQuest;
   public bool IsCompleted = false;

   public void FinishedQuest()
   {
      IsCompleted = true;
   }

   // each quest should have an end quest condition
   // MissionSheet is a sequence of quests
   // levelObj will have multiple missionSheets
   
}
