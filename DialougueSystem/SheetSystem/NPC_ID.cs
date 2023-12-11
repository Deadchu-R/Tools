using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NPCInfo", menuName = "NPCInfo")]
public class NPC_ID : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public Sprite TalkingIcon;
    public bool ShowIcon;
}
