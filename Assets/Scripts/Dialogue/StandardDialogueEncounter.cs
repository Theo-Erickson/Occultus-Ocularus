using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardDialogueEncounter : MonoBehaviour, IDialogueEncounter
{
    public TextAsset dialogueText;
    public Dialogue dialogueSetup;

    public void Talk()
    {
        Dialogue dialogueInstance = dialogueSetup.ActivateDialogueBox();
        dialogueInstance.Setup(this);
        dialogueInstance.ParseMessage(dialogueText.ToString());
    }

    public void DialogueFinished() {}

    public void DialogueAction(string action)
    {
        Debug.Log("DialogAction: " + action);
    }

}
