using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class GameManager : MonoBehaviour
{
    //public int currCurrency;

    public static GameManager Instance;
    public PlayerState playerState;

    public bool correctCheese = true; // determined by npc preference (desc = clue). for now always assume true
    public ConversationData sellConvo;

    public bool firstBossDefeated;

    private void Awake()
    {

        // Singleton pattern (simple version); avoid duplicates
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        playerState = new PlayerState
        {
            currency = 0,
            reputation = 0,
            currLevel = 1,
            recruitedRats = new List<NPCProfile>(),
            soldTo = new List<NPCProfile>()

        };
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        UIManager.Instance.ToggleLog();
        RecruitManager();

        
    }

    public void increaseCurrency(int increaseBy)
    {
        playerState.currency += increaseBy;
    }

    // called from button press
    //public void SellCheese(NPCProfile npc, CheeseIngredientData cheese) // add cheese perameter
    //{
    //    // check if correct cheese
    //    // sell cheese here, increase reputation
    //    // remove specific cheese from inventory
    //    if (correctCheese) 
    //    {
    //        InventorySystem.Instance.RemoveItem(cheese);
    //        playerState.soldTo.Add(npc);
    //        return;

    //    }

    //    Debug.Log("This rat doesn't want that.");

        

    //}

    public void RecruitRat(NPCProfile npc)
    {
        playerState.recruitedRats.Add(npc);
        UIManager.Instance.AddToRecruitLog(npc);

    }

    public void RecruitManager()
    {
        // loop through the recruited rats, 
        // check the type of things they bring back and at what rate,
        // add each ingrident they bring back to inventory.
    }

    //public void SellingManager()
    //{

    //}

    public void SellItem(CheeseIngredientData cheese)
    {
        // logic will become more complicated later on
        // can check if rat wants the cheese here later

        // check if correct cheese
        // sell cheese here, increase reputation
        // remove specific cheese from inventory

        InventorySystem.Instance.RemoveItem(cheese);
        increaseCurrency(int.Parse(cheese.price));
        UIManager.Instance.ToggleSellScreen();
        Debug.Log("Cheese sold!");
        //DialogueManager.Instance.StartConversation(sellConvo);

    }

}
