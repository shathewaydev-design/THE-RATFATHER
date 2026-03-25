using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PickUpScript : MonoBehaviour
{
    public InputActionAsset InputActions;//import all function from the input system
    public InputAction interact;//press E
    public CheeseIngredientData cheeseIngredientData;

    [Header("Interaction")]
    public float holdTime = 1.0f;
    private float currentHoldTime = 0f;
    private bool playerInRange = false;


    private bool isHolding = false;

    
    private void OnEnable()//this is necessary to avoid bugs
    {
        InputActions.FindActionMap("Player").Enable();

    }
    private void OnDisable()//this is necessary to avoid bugs
    {
        InputActions.FindActionMap("Player").Disable();

    }
    private void Awake()//this is necessary to avoid bugs
    {
        interact = InputSystem.actions.FindAction("Interact");
    }
    void Update()
    {
        if (!playerInRange) return;

        if (interact.IsPressed())//detect hold button
        {
            currentHoldTime += Time.deltaTime;

            if (currentHoldTime >= holdTime)
            {
                Collect();
            }
        }
        else
        {
            currentHoldTime = 0f;
        }
    }

    void Collect()
    {
        Debug.Log(cheeseIngredientData);
        InventorySystem.Instance.AddItem(cheeseIngredientData);
        Destroy(gameObject);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Hold E to extract sample"); // Replace with UI later
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            currentHoldTime = 0f;
        }
    }
}