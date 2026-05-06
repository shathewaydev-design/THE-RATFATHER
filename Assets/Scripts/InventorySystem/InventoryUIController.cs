using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using StarterAssets;

public class InventoryUIController : MonoBehaviour
{
    public static InventoryUIController Instance;
    [SerializeField] private GameObject inventoryBigPanel;
    [SerializeField] private GameObject ingredientInventoryPanel;
    [SerializeField] private GameObject cheeseInventoryPanel;
    [SerializeField] private GameObject logPanel;
    public GameObject useButton;//ref to use button to pass to cheese button
    
    public ThirdPersonController thirdPersonController;
    private bool isOpen = false;
    [Header("Input")]
    public List<CheeseButton> selectedCheeses = new List<CheeseButton>();//store selected cheeses that are selected when player click on CheeseButton.
    [SerializeField] private UI_ItemInfoPanel itemInfoPanel;//data for more details textBox 

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        thirdPersonController = ThirdPersonController.Instance;
        inventoryBigPanel.SetActive(isOpen);
        ingredientInventoryPanel.SetActive(isOpen);
        cheeseInventoryPanel.SetActive(isOpen);
        logPanel.SetActive(isOpen);
        thirdPersonController.OnOpenInventory += ToggleInventory;
    }
    void Update()
    {
        
  
    }

    private void ToggleInventory()
    {
        isOpen = !isOpen;
        itemInfoPanel.Hide();

        inventoryBigPanel.SetActive(isOpen);
        //ingredientInventoryPanel.SetActive(isOpen);
        if(isOpen)//close everything
        {
            OpenIngredientInventory();
        }
        else
        {   
            ingredientInventoryPanel.SetActive(false);
            cheeseInventoryPanel.SetActive(false);
            logPanel.SetActive(false);
            
            ClearCheeseSelection();
        }

        // if (!isOpen)//hides use button
        // {
        //     ClearCheeseSelection();
        // }

        Cursor.visible = isOpen;

        Cursor.lockState = isOpen
            ? CursorLockMode.None//use this if isOpen is trur
            : CursorLockMode.Locked;//use this if isOpen is false
    }
    public void OpenIngredientInventory()
    {
        ClearCheeseSelection();

        ingredientInventoryPanel.SetActive(true);
        cheeseInventoryPanel.SetActive(false);
        logPanel.SetActive(false);

        itemInfoPanel.Hide();//hide info panel
    }
    public void OpenCheeseInventory()
    {
        ClearCheeseSelection();

        ingredientInventoryPanel.SetActive(false);
        cheeseInventoryPanel.SetActive(true);
        logPanel.SetActive(false);

        itemInfoPanel.Hide();
    }
    public void OpenLogPanel()
    {
        ClearCheeseSelection();

        ingredientInventoryPanel.SetActive(false);
        cheeseInventoryPanel.SetActive(false);
        logPanel.SetActive(true);
        
        itemInfoPanel.Hide();
    }
    public void UpdateUseButton()
    //initialize the use button with the selected cheese; 
    // this is called in CheeseButton when player selects a cheese
    {
        useButton.SetActive(selectedCheeses.Count > 0);
        //only show use button when there is a selected cheese
        //or there is a cheese to use

        if (selectedCheeses.Count > 0)
        {
            UseButton useButtonScript = useButton.GetComponent<UseButton>();
            useButtonScript.cheeseButtonRef = selectedCheeses[0];//pass the reference of the selected cheese button to the use button
            //note about this: if we want to support multiple cheese selection in the future, 
            // we can modify the use button to handle multiple cheeses instead of just one;
        }
        
    }
    void ApplyEffect()//this will be redone later when we have an effect system, then this pass the data to that script later
    {
        // Double jump
        if (cheeseButtonRef.finalResultCheese.grantDoubleJump)
        {
            ThirdPersonController.Instance.EnableDoubleJump();
        }
    }
    private void ClearCheeseSelection()
    {
        selectedCheeses.Clear();

        useButton.SetActive(false);
    }
    public void ShowIngredientInfo(CheeseIngredientData data)
    //for more details TextBox
    {
        itemInfoPanel.ShowIngredient(data);
    }
    public void HideIngredientInfo()
    //for more details TextBox
    {
        itemInfoPanel.Hide();
    }
    public void ShowCheeseInfo(FinalResultCheese data)
    //for more details TextBox
    {
        itemInfoPanel.ShowCheese(data);
    }
    public void HideCheeseInfo()
    //for more details TextBox
    {
        itemInfoPanel.Hide();
    }
    private void OnDestroy()
    {
        if (thirdPersonController == null) return;

        thirdPersonController.OnOpenInventory -= ToggleInventory;
    }
}