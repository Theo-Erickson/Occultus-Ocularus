using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayStats : MonoBehaviour
{
    public List<float> checkpointTimes = new List<float>();
    public List<string> checkpointNames = new List<string>();
    public bool enable;

    private PlayStats original;

    private void Start()
    {
        if (enable)
        {
            Object.DontDestroyOnLoad(this);

            switch (SceneManager.GetActiveScene().name)
            {
                case "OutsideMall;Rooftops":
                    original = GameObject.Find("PlaytestStatsMall").GetComponent<PlayStats>();
                    break;
                case "EndMenu":
                    original = GameObject.Find("PlaytestStatsRooftops").GetComponent<PlayStats>();
                    break;
                case "MainMenu":
                    original = GameObject.Find("PlaytestStatsEnd").GetComponent<PlayStats>();
                    break;
                case "MallIntro":
                    original = GameObject.Find("PlaytestStatsMain").GetComponent<PlayStats>();
                    break;
            }

            if (original != null)
            {
                checkpointTimes = original.checkpointTimes;
                checkpointNames = original.checkpointNames;
                Destroy(original.gameObject);
            }
        }
        else
        {
            Destroy(this);
        }
    }

    public void CheckpointReached(string name)
    {
        checkpointNames.Add(name);
        if (name.Equals("Puzzle1") || name.Equals("Puzzle2") || name.Equals("ExitRoof"))
            checkpointTimes.Add(Time.timeSinceLevelLoad - checkpointTimes[checkpointTimes.Count - 1]);
        else
            checkpointTimes.Add(Time.timeSinceLevelLoad);
    }

    public void RestartGame()
    {
        string gameOutput = "";
        for (int i = 0; i < checkpointNames.Count; i++)
        {
            gameOutput += checkpointNames[i];
            if (i != checkpointNames.Count - 1)
                gameOutput += ", ";
        }
        gameOutput += System.Environment.NewLine;
        for (int i = 0; i < checkpointTimes.Count; i++)
        {
            gameOutput += Mathf.Round(checkpointTimes[i] * 100) / 100;
            if (i != checkpointTimes.Count - 1)
                gameOutput += ", ";
        }
        if (checkpointTimes.Count != 0)
        {
            System.IO.File.AppendAllText(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/OccOcPlaytestStats.txt",
                                     gameOutput + System.Environment.NewLine);
        }
        checkpointNames = new List<string>();
        checkpointTimes = new List<float>();
    }

    public void OnApplicationQuit()
    {
        string gameOutput = "";
        for (int i = 0; i < checkpointNames.Count; i++)
        {
            gameOutput += checkpointNames[i];
            if (i != checkpointNames.Count - 1)
                gameOutput += ", ";
        }
        gameOutput += System.Environment.NewLine;
        for (int i = 0; i < checkpointTimes.Count; i++)
        {
            gameOutput += Mathf.Round(checkpointTimes[i] * 100) / 100;
            if (i != checkpointTimes.Count - 1)
                gameOutput += ", ";
        }
        if (checkpointTimes.Count != 0)
        {
            System.IO.File.AppendAllText(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/OccOcPlaytestStats.txt",
                                         gameOutput + System.Environment.NewLine);
        }
    }
}
