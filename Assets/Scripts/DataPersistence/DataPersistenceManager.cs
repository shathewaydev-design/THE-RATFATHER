using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;


public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool initializeDataIfNull = false;
    //turn this on if we need to test stuff related to data persistence
    //so dont have to go through main menu

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
            Debug.Log("Found more than one Data Persistence Manager in the scene. Destroying the newest one.");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        //keep data persistence manager alive when loading new scenes

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        //Application.persistentDataPath will give the operating system standard directory
        //this needs to be here so FileDataHandler is initialized before any Load or Save calls
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
       Debug.Log("OnSceneLoaded Called ");
        //Debug.Log("LoadSceneMode: " + mode);

        //find all data persistance objects in the scene
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }
    public void OnSceneUnloaded(Scene scene)
    {
        SaveGame();
        Debug.Log("OnSceneUnloaded Called ");
    }
    public void NewGame()
    {
        this.gameData = new GameData();
    }
    public void LoadGame()
    {
        // Load any daved ddata from a file using the data handler
        this.gameData = dataHandler.Load();
        //start a new game if the data is null and we're configured to initialize data for debugging purposes
        if(this.gameData == null && initializeDataIfNull)
        {
            NewGame();
        }
        //if no data can be loaded, dont continue
        if(this.gameData == null)
        {
            Debug.Log("No data was found. A new Game needs to be started.");
            return;
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
        //if we dont have any data to save, log a warning here
        if(this.gameData == null)
        {
            Debug.LogWarning("No data was found. A new Game needs to be started before data can be saved.");
            return;
        }
        // pass the data to other scripts so they can update it
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
    public bool HasGameData()
    {
        return this.gameData != null;
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
