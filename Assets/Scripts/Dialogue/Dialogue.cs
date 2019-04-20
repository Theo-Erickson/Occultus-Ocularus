using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public static bool messageComplete = true;

    const char SPLIT_SYMBOL = '|';
    const float NORMAL_SCROLL_RATE = 0.04f;
    const float FAST_SCROLL_RATE = 0.015f;
    const float SKIP_TO_END_TIME_RANGE = 1f;

    public Text text;
    public PlayerController player;
    public GameObject dialogueBox;

    private string[] phrases;
    private string[] actions;
    private bool actionPerformed;

    private IDialogueEncounter dialogueEncounter;

    private float lastUpdateTime;
    private float lastScrollSpeedUpTime;
    private float currentScrollRate;

    private int phraseIndex = 0;
    private int charIndex = -1;
    private bool awaitingUser = false;
    private bool skipToEndOfPhrase = false;

    public void Setup(IDialogueEncounter de)
    {
        player.canMove = false;
        player.body.velocity = Vector2.zero;
        dialogueEncounter = de;
        lastUpdateTime = Time.time;
        currentScrollRate = NORMAL_SCROLL_RATE;
    }

    void Update()
    {
        if (text != null)
        {

            // Do dialogue action for this phrase if there is one
            if (phraseIndex < phrases.Length &&
                actions[phraseIndex] != null &&
                !actionPerformed &&
                !awaitingUser)
            {
                dialogueEncounter.DialogueAction(actions[phraseIndex]);
                actionPerformed = true;
            }

            if (Input.GetKeyDown(KeyCode.Space)){
                // Resume normal speed text scrolling
                if (awaitingUser)
                    awaitingUser = false;
                // Skip to end of line if button is pressed immediately after increasing the scroll speed
                else if (Time.time - lastScrollSpeedUpTime < SKIP_TO_END_TIME_RANGE)
                {
                    skipToEndOfPhrase = true;
                }
                // Increase speed of text scrolling
                else
                {
                    currentScrollRate = FAST_SCROLL_RATE;
                    lastScrollSpeedUpTime = Time.time;

                    // End dialogue if it all has already appeared
                    if (phraseIndex >= phrases.Length && charIndex == -1)
                    {
                        dialogueEncounter.DialogueFinished();
                        phraseIndex = 0;
                        awaitingUser = false;
                        skipToEndOfPhrase = false;
                        dialogueBox.SetActive(false);
                        player.canMove = true;
                    }
                }
            }

            // Make scroll rate normal again if space key is released
            if (Input.GetKeyUp(KeyCode.Space))
                currentScrollRate = NORMAL_SCROLL_RATE;

            // Make a new letter appear after each interval defined by SCROLL_RATE
            while (Time.time - lastUpdateTime > currentScrollRate &&
                   phraseIndex < phrases.Length &&
                   !awaitingUser)
            {
                string phrase = phrases[phraseIndex];
                lastUpdateTime = Time.time;

                // Fill in the rest of the current phrase
                if (skipToEndOfPhrase)
                {
                    text.text = phrase;
                    charIndex = -1;
                    phraseIndex++;
                    awaitingUser = true;
                    skipToEndOfPhrase = false;
                    actionPerformed = false;
                }
                else
                {
                    text.text = phrase.Substring(0, 1 + charIndex);
                    charIndex++;
                }

                // Prep for next phrase when the end of current phrase is reached
                if (charIndex == phrase.Length)
                {
                    charIndex = -1;
                    phraseIndex++;
                    awaitingUser = true;
                    actionPerformed = false;
                }
            }
        }
    }

    public void ParseMessage(string message)
    {
        string[] phrasesAndActions = message.Split(SPLIT_SYMBOL);

        int numPhrases = phrasesAndActions.Length;

        for (int i = 0; i < phrasesAndActions.Length; i++)
        {
            string phrase = phrasesAndActions[i].Trim();
            if (phrase.StartsWith("{") && phrase.EndsWith("}"))
                numPhrases--;
        }
        actions = new string[numPhrases + 1];
        phrases = new string[numPhrases];

        int diff = 0;
        for (int i = 0; i < phrasesAndActions.Length; i++)
        {
            string phrase = phrasesAndActions[i].Trim();
            if (phrase.StartsWith("{") && phrase.EndsWith("}"))
            {
                actions.SetValue(phrase.Trim(new char[2] { '{', '}' }), i - diff);
                diff++;
            }
            else
                phrases.SetValue(phrase, i - diff);
        }
    }

    public Dialogue ActivateDialogueBox()
    {
        dialogueBox.SetActive(true);
        Dialogue dialogue = dialogueBox.transform.GetChild(1).gameObject.GetComponent<Dialogue>();
        return dialogue;
    }
}
