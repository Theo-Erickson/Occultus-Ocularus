using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamsonDialogueEncounter2 : MonoBehaviour, IDialogueEncounter
{
    public Dialogue dialogueSetup;
    public PlayerController player;
    public LevelTransition levelTransition;

    private bool talkedToSamson;

    private void Update()
    {
        if (!talkedToSamson)
            player.canMove = false;
    }

    public void Talk()
    {
        talkedToSamson = true;
        Dialogue dialogueInstance = dialogueSetup.ConstructDialogueBox();
        dialogueInstance.Setup(this);
        dialogueInstance.ParseMessage("Welcome to —!|" +
            "Awww, crap. Something messed up the power\ngrid. How am I supposed " +
            "to give you a dramatic\npresentation now?|" +
            "Hey, since you’re fresh out of the fountain….no Halo or anything….|" +
            "How about this: you go figure out how to get the power back on. " +
            "It’ll probably take some thinking, and it’ll be a big help to " +
            "everyone in the city.|" +
            "It’ll make more sense once you start. Go ahead, try lining up the " +
            "lasers over there. Anyway...I’ll\nmeet you once you’re done! See ya " +
            "later!");
    }

    public void DialogueFinished()
    {
        levelTransition.FadeAway(GetComponent<SpriteRenderer>());
        foreach(BoxCollider2D bc in GetComponents<BoxCollider2D>())
            bc.enabled = false;
    }
}
