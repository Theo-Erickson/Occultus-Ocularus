using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTextHandler : MonoBehaviour
{
    //const int PHRASE_SIZE = 85; //This will change depending on the size and font of the text
    const char SPLIT_SYMBOL = '|';
    const float SCROLL_RATE = 0.2f;

    public Text text;

    private string[] phrases;
    private int phraseIndex = 0;
    private int charIndex = 0;
    private float lastUpdateTime;

    void Setup()
    {
        lastUpdateTime = Time.time;
    }

    void Update()
    {
        //Add a new letter after each interval defined by SCROLL_RATE
        while (Time.time - lastUpdateTime > SCROLL_RATE && phraseIndex < phrases.Length)
        {
            string phrase = phrases[phraseIndex];
            lastUpdateTime = Time.time;
            text.text = phrase.Substring(0, 1 + charIndex);

            charIndex++;

            if (charIndex == phrase.Length)
            {
                charIndex = 0;
                phraseIndex++;
            }

            if (phraseIndex == phrases.Length && Input.GetKeyUp(KeyCode.Space))
            {
                Destroy(gameObject);
            }
        }
    }

    void parseText(string s)
    {
        phrases = s.Split(SPLIT_SYMBOL);
        //Option to further split each phrase if its too long may be added
    }
}
