using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;
using System.Linq;


public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;
    private GameData gameData;
    public static DataPersistenceManager Instance { get; private set; }
    private FileDataHandler dataHandler;
    private List<IDataPersistence> dataPersistenceObjects;

    //the vlues defined in this constuctor will be the default values
    //the game starts with when there's no data to load

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more than one Data Persistence Manager in the scene.");
        }
        Instance = this;
    }
    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        //Application.persistentDataPath will give the operating system standard directory
        //for persisting data in a unity project
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }
    public void NewGame()
    {
        this.gameData = new GameData();
    }
    public void LoadGame()
    {
        // Load any daved ddata from a file using the data handler
        this.gameData = dataHandler.Load();

        //if no data can be loaded, initialize a new game
        if(this.gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }

        //Push the loaded data to all other scripts that need it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
        Debug.Log(Application.persistentDataPath);

    }
    public void SaveGame()
    {
        //TODO pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }

        
        //save that data to a file using the data handler
        dataHandler.Save(gameData);
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }
    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>();
        //the scripts also need to be MonoBehaviors with IDataPersistence interface
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
    [ContextMenu("Open Save Folder")]
    public void OpenSaveFolder()
    {
        string saveFolder = Application.persistentDataPath;
        string saveFile = Path.Combine(saveFolder, fileName);

        Debug.Log("Save Folder: " + saveFolder);
        Debug.Log("Save File: " + saveFile);

    #if UNITY_EDITOR
        UnityEditor.EditorUtility.RevealInFinder(saveFolder);
    #else
        Application.OpenURL(new System.Uri(saveFolder).AbsoluteUri);
    #endif
    }
}
