/**
 * @file   SaveSystem.cs
 * 
 * @authors  Eduardo S Pino, Brackeys
 * 
 * @version 1.0
 * @date 29/03/2020 (DD/MM/YYYY)
 *
 * This component is based on Brackeys Save and Load tutorial.
 * Implements functions to save and load playe score
 * 
 */


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
