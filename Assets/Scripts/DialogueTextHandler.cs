using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTextHandler : MonoBehaviour
{
    const int PHRASE_SIZE = 85; //This will change depending on the size and font of the text
    const char SPLIT_SYMBOL = '|'; 

    public Text text;

    private string[] phrases;
    private int phraseIndex = 0;
    private float lastUpdateTime;

    void Setup()
    {
        lastUpdateTime = Time.time;
    }

    void Update()
    {


    }

    void parseText(string s)
    {
        phrases = s.Split(SPLIT_SYMBOL);
        //Option to further split each phrase if its too long may be added
    }
}
