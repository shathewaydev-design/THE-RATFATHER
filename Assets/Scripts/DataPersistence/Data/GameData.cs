using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Diagnostics.Tracing;

[System.Serializable]
public class GameData
{
    public int currentDay;
    public float timeLeft;//dont need initializing because DayManager will do it instead




    // Positions of all ingredients and which ingredients in that level, Transform of GameObject
    // make sure the ingredient spawner doesn’t spawn ingredients when player loads the game, only spawn when a new day starts

    public List<IngredientSaveData> playerIngredients = new List<IngredientSaveData>();
    //id of ingredient, quantity
    //What ingredients the player has/ ScriptableObject
    public List<CheeseSaveData> playerCheese = new List<CheeseSaveData>();
    //id of cheese, quantity
    // What cheese the player has/ ScriptableObject
    public float currencyInInventory;
    // Chedz
    // [NPCs]
    // What Quests are completed
    // Trust level for each NPCs (is there a limited number of NPCs in the level?)

    //[Cutscenes]
    //check what cutscenes (slumps, NRY) has played so wont play that cutscene again

    public Level currentGameLevel;
     public enum Level
    {
        Slumps,
        Soldato,
        NRY,
        CasinoBoss
    }
    // What level the player is in. (If soldato boss, starts fresh)
    
    //the values defined in this constuctor will be the default values
    //the game starts with when there's no data to load

    public GameData()
    {
        this.currentDay = 1;
        this.currencyInInventory = 0f;
        this.currentGameLevel = Level.Slumps;
        this.playerIngredients = new List<IngredientSaveData>();
        this.playerCheese = new List<CheeseSaveData>();
    }

}
