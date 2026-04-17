using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PickUpObjectScript : MonoBehaviour, IInteractable
{
    
    public CheeseIngredientData cheeseIngredientData;

    [Header("Interaction")]
    // [SerializeField] private float holdTime = 1.0f;
    // [SerializeField] private float currentHoldTime = 0f;
    private bool playerInRange = false;

    [Header("UI")]
    public Animator promptAnimator;

    public Image holdProgressBar;


    
    
    void Update()
    {
        if (!playerInRange) return;
        //Interact();
        
    }

    public void Interact()
    {
        // if (interact.IsPressed())//detect hold button
        // {
        //     Collect();
        //     // currentHoldTime += Time.deltaTime;

        //     // if (currentHoldTime >= holdTime)
        //     // {
        //     //     Collect();
        //     // }
        // }
        // else
        // {
        //     currentHoldTime = 0f;
        // }
        Collect();
    }
    void Collect()
    {
        Debug.Log("Collected");
        Debug.Log(cheeseIngredientData);
        InventorySystem.Instance.AddItem(cheeseIngredientData);
        Destroy(gameObject);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if(promptAnimator != null)
            {
                promptAnimator.SetBool("UIappeared", true);
                promptAnimator.SetTrigger("UIappearing");
            }


            Debug.Log("Hold E to extract sample"); // Replace with UI later
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            //Debug.Log("playerInRange is "+playerInRange);
            //currentHoldTime = 0f;
            if(promptAnimator != null)
            {
                promptAnimator.SetBool("UIappeared", false);
                promptAnimator.SetTrigger("UIdisappearing");
            }
        }
    }
}