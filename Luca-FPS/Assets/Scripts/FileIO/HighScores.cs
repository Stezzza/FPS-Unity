using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HighScores : MonoBehaviour
{
    public float[] times = new float[10]; // store times

    private string currentDirectory;
    public string scoreFileName = "highscores.txt";

    void Start()
    {
        currentDirectory = Application.dataPath;
        Debug.Log("current dir: " + currentDirectory);
        LoadTimesFromFile();
    }

    /// load scores from file
    public void LoadTimesFromFile()
    {
        string filePath = Path.Combine(currentDirectory, scoreFileName);
        if (File.Exists(filePath))
        {
            Debug.Log("found file: " + scoreFileName);
        }
        else
        {
            Debug.Log("file not found: " + scoreFileName, this);
            return;
        }

        times = new float[times.Length];

        using (StreamReader reader = new StreamReader(filePath))
        {
            int count = 0;
            while (reader.Peek() != -1 && count < times.Length)
            {
                string line = reader.ReadLine();
                if (float.TryParse(line, out float readTime))
                {
                    times[count] = readTime;
                }
                else
                {
                    Debug.Log("invalid line at " + count, this);
                    times[count] = 0f;
                }
                count++;
            }
        }

        Debug.Log("high times read from " + scoreFileName);
    }

    /// save scores to file
    public void SaveTimesToFile()
    {
        string filePath = Path.Combine(currentDirectory, scoreFileName);
        using (StreamWriter writer = new StreamWriter(filePath, false))
        {
            foreach (float time in times)
            {
                writer.WriteLine(time);
            }
        }

        Debug.Log("high times written to " + scoreFileName);
    }

    /// add new time if it's top 10
    public void AddTime(float newTime)
    {
        int index = -1;
        for (int i = 0; i < times.Length; i++)
        {
            if (newTime < times[i] || times[i] == 0f)
            {
                index = i;
                break;
            }
        }

        if (index < 0)
        {
            Debug.Log("time " + newTime + " not high enough", this);
            return;
        }

        for (int i = times.Length - 1; i > index; i--)
        {
            times[i] = times[i - 1];
        }

        times[index] = newTime;
        Debug.Log("time " + newTime + " added at " + index, this);
    }
}
