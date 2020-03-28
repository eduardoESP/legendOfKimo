using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class HighestScore 
{
    public int highestScore;
    public float longestTime;

    public HighestScore(GameController game)
    {
        highestScore = game.GetHighestScore();
    }
}

public static class SaveGame
{

    public static void SaveScore()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/highest_score.dat";
        FileStream stream = new FileStream(path, FileMode.Create);
        HighestScore data = new HighestScore(GameController.controler);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static HighestScore LoadScore()
    {
        string path = Application.persistentDataPath + "/highest_score.dat";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            HighestScore score =  (HighestScore)formatter.Deserialize(stream);
            stream.Close();
            return score;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

}
