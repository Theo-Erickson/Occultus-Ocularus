using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamsonDialogueEncounter : MonoBehaviour, IDialogueEncounter
{
    public Dialogue dialogueSetup;

    public void Talk()
    {
        Dialogue dialogueInstance = dialogueSetup.ConstructDialogueBox();
        dialogueInstance.Setup(this);
        dialogueInstance.ParseMessage("Hey, kid. Come here.|" +
            "You’re new, aren’t you?|" +
            "Neat. Mind fetching me a cup of water from the\nfountain " +
            "you just crawled out of?|Thanks.|You know, you look like you’ve " +
            "got potential.\nHow about I show you around?|" +
            "Don’t talk much, do you? That’s fine, most\nNewbies don’t.|" +
            "Well said.. Okay, meet me outside — I’ll help\nyou out.|" +
            "Oh, by the way, I’m Samson. Nice to meet you.");
    }

    public void DialogueFinished()
    {
        GetComponent<MovingPlatform>().Extend();
    }
}
