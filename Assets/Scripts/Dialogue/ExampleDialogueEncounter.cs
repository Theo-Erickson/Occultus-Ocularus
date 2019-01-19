using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleDialogueEncounter : MonoBehaviour
{
    void Start()
    {
        Dialogue dialogueInstance = Dialogue.constructDialogueBox();
        dialogueInstance.Setup();
        dialogueInstance.parseMessage("This is a test.|Hello user this is the second phrase.|This is the third phrase");
    }
}
