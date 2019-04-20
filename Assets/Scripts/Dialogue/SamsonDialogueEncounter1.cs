using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamsonDialogueEncounter1 : MonoBehaviour, IDialogueEncounter
{
    public TextAsset dialogueText;
    public Dialogue dialogueInstance;
    public bool moveWhenFinished = true;

    public void Talk()
    {
        dialogueInstance.ActivateDialogueBox();
        dialogueInstance.Setup(this);
        dialogueInstance.ParseMessage(dialogueText.ToString());
    }

    public void DialogueFinished()
    {
        if (moveWhenFinished) {
            foreach (BoxCollider2D bc in GetComponents<BoxCollider2D>())
                bc.enabled = false;
            GetComponent<MovingPlatform>().Extend();
        }

    }

    public void DialogueAction(string action)
    {
        Debug.Log("DialogAction: " + action);
    }
}
