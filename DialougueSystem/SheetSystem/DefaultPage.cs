using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [CreateAssetMenu(fileName = "New Page", menuName = "Page/defualtPage")]
public class DefaultPage : Page
{
        public override void PageAction(SpeechBubbleUI speechBubbleUI)
        {
            speechBubbleUI.SetButtonInteractable(speechBubbleUI.NextButton,speechBubbleUI.CurrentPageIndex + 1 <= speechBubbleUI.CurrentSheet.pages.Length);
            speechBubbleUI.CloseNonDefaultPageActions();
        }
}
