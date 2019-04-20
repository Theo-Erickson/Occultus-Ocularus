using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArielDialogueEncounter : MonoBehaviour, IDialogueEncounter
{
    public TextAsset dialogueText;
    public Dialogue dialogueInstance;

    public void Talk()
    {
        dialogueInstance.ParseMessage(dialogueText.ToString());
        dialogueInstance.Setup(this);
        dialogueInstance.ActivateDialogueBox();
    }

    public void DialogueAction(string action)
    {
        Debug.Log("DialogAction: " + action);
    }

    public void DialogueFinished() {}
}
