using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamsonDialogueEncounter2 : MonoBehaviour, IDialogueEncounter
{
    public TextAsset dialogueText;
    public Dialogue dialogueSetup;
    public LevelTransition levelTransition;
    public Light cityLight;

    private bool talkedToSamson;
    private bool lightsOut;

    private void Update()
    {
        if (lightsOut && cityLight.intensity >= 8)
            cityLight.intensity -= 1f;
    }

    public void Talk()
    {
        talkedToSamson = true;
        Dialogue dialogueInstance = dialogueSetup.ActivateDialogueBox();
        dialogueInstance.Setup(this);
        dialogueInstance.ParseMessage(dialogueText.ToString());
    }

    public void DialogueFinished()
    {
        levelTransition.FadeAway(GetComponent<SpriteRenderer>());
        foreach (BoxCollider2D bc in GetComponents<BoxCollider2D>())
            bc.enabled = false;
    }

    public void DialogueAction(string action)
    {
        if (action.Equals("Lights go out"))
            lightsOut = true;
        else
            Debug.Log("DialogAction: " + action);
    }
}
