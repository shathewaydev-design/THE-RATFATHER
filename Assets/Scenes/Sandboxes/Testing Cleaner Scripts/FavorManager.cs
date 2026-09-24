using System;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class FavorManager : MonoBehaviour
{
    public static FavorManager Instance;

    // action on favor activated
    public static event Action<FavorState> OnFavorActivated;
    // action on objective complete
    public static event Action<FavorState> OnObjectiveComplete;
    // action on favor complete
    public static event Action<FavorState> OnFavorComplete;


    public List<FavorState> activeFavors = new List<FavorState>();


    private void Awake()
    {
        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        InventorySystem.OnInventoryChange += CheckItemObtained;
        LocationCheck.OnLocationEnter += CheckLocation;
    }

    private void OnDisable()
    {
        InventorySystem.OnInventoryChange -= CheckItemObtained;
        LocationCheck.OnLocationEnter -= CheckLocation;
    }

    //[YarnCommand("Activate_Favor")]
    public void StartFavor(Favor favor)
    {
        FavorState newFavor = new FavorState();

        newFavor.favor = favor;
        newFavor.currentObjective = 0;
        newFavor.isCompleted = false;

        activeFavors.Add(newFavor);
        DialogueManager_New.Instance.SetNode(favor.objectives[newFavor.currentObjective].npcDialogue);
        OnFavorActivated?.Invoke(newFavor);
        Debug.Log("curr objective: " + activeFavors[0].favor.objectives[activeFavors[0].currentObjective].description); // TESTING

    }

    public void CompleteCurrentObjective(FavorState favorState)
    {
        favorState.currentObjective++;
        RemoveKeyItems(favorState,
                favorState.favor.objectives[favorState.currentObjective].toBeRemoved);
        OnObjectiveComplete?.Invoke(favorState);


        if (favorState.currentObjective >= favorState.favor.objectives.Count) // entire favor complete?
        {
            favorState.isCompleted = true;
            RemoveKeyItems(favorState, true);
            OnFavorComplete?.Invoke(favorState);
            return; // TESTING
        }

        DialogueManager_New.Instance.SetNode(favorState.favor.objectives[favorState.currentObjective].npcDialogue);
        Debug.Log("curr objective: " + activeFavors[0].favor.objectives[activeFavors[0].currentObjective].description); // TESTING
    }

    public void CheckItemObtained(CheeseIngredientData item, FinalResultCheese cheese)
    {
        foreach (FavorState state in activeFavors)
        {

            // get objective player currently working on
            FavorObjective objective = state.favor.objectives[state.currentObjective];

            // only care about objectives that require an item
            if (objective.type != FavorObjectiveType.ObtainItem)
                continue;

            // check if item is ingredient
            if (item != null && objective.targetItem == item)
            {
                CompleteCurrentObjective(state);
                continue;
            }

            // check if item is cheese
            if (cheese != null && objective.targetCheese == cheese)
            {
                CompleteCurrentObjective(state);
                continue;
            }

            // eventually, check if item is a misc. item

        }
    }


    public void CheckLocation(string locationID)
    {
        Debug.Log("triggered local check");
        foreach (FavorState state in activeFavors)
        {
            // get objective player currently working on
            FavorObjective objective = state.favor.objectives[state.currentObjective];

            // only care about objectives that require an item
            if (objective.type != FavorObjectiveType.ReachLocation)
                continue;

            // check if the location the player reached is required
            if (objective.targetLocationID == locationID)
            {
                CompleteCurrentObjective(state);
                //Debug.Log("You've entered the building, objective complete! :)");
            }


        }
    }

    public void CheckNPC(NPCProfile_New npc)
    {
        foreach (FavorState state in activeFavors)
        {
            // get objective player currently working on
            FavorObjective objective = state.favor.objectives[state.currentObjective];

            // only care about objectives that require an item
            if (objective.type != FavorObjectiveType.TalkToNPC)
                continue;

            // check if the NPC the player talked to is the NPC required
            if (objective.targetNPC == npc)
            {
                CompleteCurrentObjective(state);
            }


        }

    }
    public void CheckTrade(NPCProfile rat, FinalResultCheese item)
    {
        foreach (FavorState state in activeFavors)
        {
            // get objective player currently working on
            FavorObjective objective = state.favor.objectives[state.currentObjective];

            // only care about objectives that require an item
            if (objective.type != FavorObjectiveType.TradeItem)
                continue;

            // check if correct merchant and item were involved in trade
            if (objective.targetNPC == rat &&
                objective.targetItem == item)
            {
                CompleteCurrentObjective(state);
            }

        }

    }

    public void RemoveKeyItems(FavorState favorState, bool toBeRemoved)
    {
        if (toBeRemoved) // going back and removing for all objectives if removal didn't happen beforehand
        {
            FavorObjective objective = favorState.favor.objectives[favorState.currentObjective];

            if (objective.type != FavorObjectiveType.ObtainItem)
                return;

            if (objective.targetItem != null)
            {
                InventorySystem.Instance.RemoveIngredientItem(objective.targetItem);
                return;
            }

            if (objective.targetCheese != null)
            {
                InventorySystem.Instance.RemoveFinalCheese(objective.targetCheese);
                return;
            }
            // eventually, check for misc. item
        }
        else 
        {
            return;
        }

    }


}
