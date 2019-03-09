using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Dialogue : MonoBehaviour
{
    public static bool messageComplete = true;

    const char SPLIT_SYMBOL = '|';
    const float NORMAL_SCROLL_RATE = 0.04f;
    const float FAST_SCROLL_RATE = 0.02f;

    public Text text;

    public string[] phrases;
    private float lastUpdateTime;
    private float currentScrollRate;

    private int phraseIndex = 0;
    private int charIndex = -1;
    private bool awaitingUser = false;
    private float beginPress;
    private bool skipToEndOfPhrase;
    private float awaitingTime;

    PlayerController player;
    IDialogueEncounter dialogueEncounter;
    public GameObject dialogueBox;

    public void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    public void Setup(IDialogueEncounter de)
    {
        player.canMove = false;
        dialogueEncounter = de;
        lastUpdateTime = Time.time;
        currentScrollRate = NORMAL_SCROLL_RATE;
        phrases = new string[1];
    }

    void Update()
    {
        if (text != null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (awaitingUser)
                    awaitingTime = Time.time;
                currentScrollRate = FAST_SCROLL_RATE;
                awaitingUser = false;
                beginPress = Time.time;

            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                currentScrollRate = NORMAL_SCROLL_RATE;
                if (Time.time - beginPress < 0.3 && Time.time - awaitingTime > 0.6)
                    skipToEndOfPhrase = true;
            }

            //Add a new letter after each interval defined by SCROLL_RATE
            while ((Time.time - lastUpdateTime > currentScrollRate && phraseIndex < phrases.Length && awaitingUser == false))
            {
                string phrase = phrases[phraseIndex];
                lastUpdateTime = Time.time;

                if (skipToEndOfPhrase)
                {
                    text.text = phrase;
                    charIndex = -1;
                    phraseIndex++;
                    awaitingUser = true;
                    awaitingTime = Time.time;
                    skipToEndOfPhrase = false;
                }
                else
                {
                    text.text = phrase.Substring(0, 1 + charIndex);
                    charIndex++;
                }

                if (charIndex == phrase.Length)
                {
                    charIndex = -1;
                    phraseIndex++;
                    awaitingUser = true;
                    awaitingTime = Time.time;
                }
            }

            if (phraseIndex >= phrases.Length && Input.GetKeyUp(KeyCode.Space))
            {
                dialogueEncounter.DialogueFinished();
                Destroy(transform.parent.gameObject);
                player.canMove = true;
            }
        }
    }

    public void ParseMessage(string message)
    {
        phrases = message.Split(SPLIT_SYMBOL);
    }

    public Dialogue ConstructDialogueBox()
    {
        dialogueBox.SetActive(true);
        GameObject textBox = dialogueBox.transform.GetChild(1).gameObject;
        Dialogue dialogue = textBox.GetComponent<Dialogue>();
        return dialogue;
    }
}
