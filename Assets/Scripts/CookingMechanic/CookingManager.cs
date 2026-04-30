using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using StarterAssets;
public enum CookingStepType
{
    Milk,
    Flavor,
    Heat,
    Additive
}

[System.Serializable]
public class CookingStepUI
{
    public CookingStepType stepType;
    public GameObject root;     // container object
    public Animator animator;   // for punch/glow/etc
}
public class CookingManager : MonoBehaviour, IInteractable
{
    public static CookingManager Instance;

    [Header("Camera")]
    public CinemachineCamera playerCamera;
    public GameObject mainCamera;
    public Transform cookingPot;//view cooking pot
    [SerializeField] private GameObject cookingPotGeo;// cooking pot
    private Vector3 savedMainCameraPosition;
    private Quaternion savedMainCameraRotation;

    [Header("Player")]
    public GameObject player;//playerCameraRoot
    public GameObject playerGeo;//player Geo
    public GameObject playerFollowCamera;//cinemachine

    public GameObject recipeMenu;
    public GameObject inventoryPanel;
    [Header("UI Steps")]
    public List<CookingStepUI> steps;
    private int currentStepIndex = -1;

    [Header("Input")]
    // [SerializeField] private float holdTime = 1.0f;
    // [SerializeField] private float currentHoldTime = 0f;
    public ThirdPersonController thirdPersonController;
    private bool playerInRange = false;
    bool hasInteracted = false;
    public List<IngredientButton> selectedIngredients = new List<IngredientButton>();//store selected ingredients that are selected when player click on IngredientButton.
    public List<CheeseButton> selectedCheeses = new List<CheeseButton>();//store selected cheeses that are selected when player click on CheeseButton.
    public List<CheeseRecipeTemplate> allRecipes;//all cheese recipes

    [Header("UI")]
    public Animator promptAnimator;

    public Image holdProgressBar;
    
    private void Awake()//this is necessary to avoid bugs
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cookingPotGeo.SetActive(true);
        thirdPersonController = ThirdPersonController.Instance;
        thirdPersonController.OnStopInteract += ExitCookingMode;
    }

    // Update is called once per frame
    void Update()
    {

        if(thirdPersonController.stopInteract.WasPressedThisFrame())
        {
            ExitCookingMode();
        }

        
    }

    public void Interact()//runs when player press E to interact
    {
        hasInteracted = true;
        EnterCookingMode();
        ShowRecipeMenu();
    }
    public void EnterCookingMode()
    {
        UpdatePrompt();
        // HidePrompt();

        playerGeo.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Cooking");
        
        //Save current camera data
        savedMainCameraPosition = mainCamera.transform.position;
        savedMainCameraRotation = mainCamera.transform.rotation;
        // Camera zoom to pot
        playerCamera.Follow = cookingPot;
        mainCamera.transform.position = new Vector3(3.747427f, 2.5f, -5.494404f);
        mainCamera.transform.rotation = Quaternion.Euler(14.08f, -0.086f, -0.025f);
        
    
    } 

    public void ExitCookingMode()
    {
        selectedIngredients.Clear();//clear selected ingredients in case player exit cooking mode in the middle of cooking process
        //selectedSlots.Clear();

        playerGeo.SetActive(true);
        
        thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");

        playerCamera.Follow = player.transform;
        mainCamera.transform.position = savedMainCameraPosition;
        mainCamera.transform.rotation = savedMainCameraRotation;
        
        ResetSteps();
        recipeMenu.SetActive(false);
        inventoryPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // ---------- FLOW ----------
    private void SetStep(int newIndex)
    {
        if (newIndex < 0 || newIndex >= steps.Count) return;

        int previousIndex = currentStepIndex;
        currentStepIndex = newIndex;

        for (int i = 0; i < steps.Count; i++)
        {
            var step = steps[i];

            if (i < currentStepIndex)
            {
                // Completed
                step.root.SetActive(false);
                step.animator.SetTrigger("Completed");
            }
            else if (i == currentStepIndex)
            {
                // Active
                step.root.SetActive(true);
                step.animator.SetTrigger("Activate");
            }
            else
            {
                // Upcoming
                step.root.SetActive(false);
                step.animator.SetTrigger("Idle");
            }
        }
        UpdatePrompt();
        
        // Optional: animate previous step losing focus
        // if (previousIndex >= 0 && previousIndex < steps.Count)
        // {
        //     steps[previousIndex].animator.SetTrigger("Idle");
        // }
    }
    public void ShowRecipeMenu()
    {
        recipeMenu.SetActive(true);
        inventoryPanel.SetActive(true);
    }

    public void StartPourMilk()
    {
        inventoryPanel.SetActive(false);
        
        SetStep(0);
    }

    public void StartFlavor()
    {
        
        SetStep(1);
        thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Mouse");
    }

    public void StartHeat()
    {
        
        SetStep(2);
    }

    public void StartAdditive()
    {
        
        SetStep(3);
        thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Cooking");
    }

    public void FinishCooking()
    {
        CraftCheese();
        foreach (IngredientButton ingredient in selectedIngredients)
        {
            ingredient.RemoveIngredient();
        }
        


        //selectedIngredients.Clear();
        //selectedSlots.Clear();
        thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        ExitCookingMode();
    }
/////CHEESE RECIPES AND CRAFTING CHEESE////
    
    public CheeseRecipeTemplate GetMatchingRecipe(List<IngredientButton> selectedIngredients)
    //This method checks the selected ingredients against all recipes to find a match. 
    //It returns the matching recipe .
    {
        List<CheeseIngredientData> selectedData = new List<CheeseIngredientData>();

        foreach (IngredientButton ingredient in selectedIngredients)
        {
            selectedData.Add(ingredient.inventorySlot.itemData);
            //inside IngredientButton, there is a reference to the InventorySlot, 
            // which contains the CheeseIngredientData. we need to extract the CheeseIngredientData 
            // from each selected ingredient to compare with the recipes.
        }

        foreach (CheeseRecipeTemplate recipe in allRecipes)
        {
            if (RecipeMatches(recipe.requiredIngredients, selectedData))
            {
                return recipe;
            }
        }

        return null;
    }
    private bool RecipeMatches(List<CheeseIngredientData> recipeIngredients,
    List<CheeseIngredientData> selectedIngredients)
    {
        if (recipeIngredients.Count != selectedIngredients.Count)
            return false;

        foreach (CheeseIngredientData ingredient in recipeIngredients)
        {
            if (!selectedIngredients.Contains(ingredient))
                return false;
        }

        return true;
    }
    public void CraftCheese()//CookingManager checks if the selected ingredients match any recipe, 
    // if it does, it gives the player the resulting cheese.
    {
        CheeseRecipeTemplate recipe = GetMatchingRecipe(selectedIngredients);
        
        if (recipe == null)
        {
            Debug.Log("Invalid recipe");
            return;
        }

        //ConsumeSelectedIngredients();

        GivePlayerCheese(recipe.resultCheese);

        Debug.Log("Crafted: " + recipe.resultCheese.cheeseName);
    }
    
    public void GivePlayerCheese(FinalResultCheese cheese)
    {
        InventorySystem.Instance.AddFinalCheese(cheese);
    }


/////UI ONLY////
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            UpdatePrompt();
            // if(promptAnimator != null)
            // {
            //     promptAnimator.SetBool("UIappeared", true);
                //promptAnimator.SetTrigger("UIappearing");
                
            //}


            //Debug.Log("Hold E to extract sample"); // Replace with UI later
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            hasInteracted = false;
            //currentHoldTime = 0f;
            UpdatePrompt();
            //HidePrompt();
        }
    }
    void UpdatePrompt()
    {
        bool shouldShow = playerInRange && !hasInteracted;

        promptAnimator.SetBool("UIappeared", shouldShow);
        if(!shouldShow)
        {
            promptAnimator.SetTrigger("UIdisappearing");
        }
        
    }
    
    private void ResetSteps()
    {
        currentStepIndex = -1;

        foreach (var step in steps)
        {
            step.root.SetActive(false);
        }
    }
    private void OnDestroy()
    {
        if (thirdPersonController == null) return;

        thirdPersonController.OnStopInteract -= ExitCookingMode;
    }
}
