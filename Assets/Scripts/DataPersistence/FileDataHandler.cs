using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;
using System;


public class FileDataHandler
{
    private string dataDirPath = "";
    //directory path where the data will be saved
    private string dataFileName = "";
    //file name where the data will be saved
    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }
    public GameData Load()
    {
        //use Path.Combine to account for different OS path formats
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                //load the serialized data from the file
                string dataToLoad = "";
                //we want to read the data
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                //deserialize the data from JSON back into C# object
                //specify GameData as the type of object we want to deserialize
                //send the result to dataToLoad
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        //use Path.Combine to account for different OS path formats
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            //create the directory the file will be written to if it doesn't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            //serialize the C# game data object into a JSON string
            string dataToStore = JsonUtility.ToJson(data, true);

            //write the serialized data to the file
            using(FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore); 
                }
            }
            
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }
}