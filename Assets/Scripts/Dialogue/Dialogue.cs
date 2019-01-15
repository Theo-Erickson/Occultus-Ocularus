using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public static string response;

    const char SPLIT_SYMBOL = '|';
    const float NORMAL_SCROLL_RATE = 0.06f;
    const float FAST_SCROLL_RATE = 0.03f;

    public Text text;

    private string[] phrases;
    private float lastUpdateTime;
    private float currentScrollRate;

    private int phraseIndex = 0;
    private int charIndex = 0;
    private bool awaitingUser = false;

    void Setup()
    {
        Debug.Log("hi");
        lastUpdateTime = Time.time;
        currentScrollRate = NORMAL_SCROLL_RATE;
        phrases = new string[1];
    }

    void Update()
    {
        //Debug.Log(currentScrollRate);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentScrollRate = FAST_SCROLL_RATE;
            awaitingUser = false;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            currentScrollRate = NORMAL_SCROLL_RATE;
        }

        //Add a new letter after each interval defined by SCROLL_RATE
        while (Time.time - lastUpdateTime > currentScrollRate && phraseIndex < phrases.Length && awaitingUser == false)
        {
            string phrase = phrases[phraseIndex];
            lastUpdateTime = Time.time;
            text.text = phrase.Substring(0, 1 + charIndex);

            charIndex++;

            if (charIndex == phrase.Length)
            {
                charIndex = 0;
                phraseIndex++;
                awaitingUser = true;
            }

            if (phraseIndex == phrases.Length && Input.GetKeyUp(KeyCode.Space))
            {
                Destroy(gameObject);
            }
        }
    }

    void parseText(string message)
    {
        phrases = message.Split(SPLIT_SYMBOL);
    }

    public static void constructDialogueBox(string message)
    {
        Vector3 position = GameObject.Find("DialogueBoxPos").transform.position;
        GameObject dialogueBox = Instantiate(Resources.Load("DialogueBox") as GameObject, position, Quaternion.identity);
        dialogueBox.transform.parent = GameObject.Find("Canvas").transform;
        dialogueBox.BroadcastMessage("Setup");
        dialogueBox.BroadcastMessage("parseText", message);
    }
}
