using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class NPCInteractionScript : MonoBehaviour, IInteractable
{
    [Header("Interaction")]
    // [SerializeField] private float holdTime = 1.0f;
    // [SerializeField] private float currentHoldTime = 0f;
    //private bool playerInRange = false;

    [Header("UI")]
    public Animator promptAnimator;

    //public Image holdProgressBar;


    // void Start()
    // {
        
    // }

    // void Update()
    // {
        
    // }



    public void Interact()
    {
        Debug.Log("Interacting with NPC Template");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //playerInRange = true;
            if(promptAnimator != null)
            {
                promptAnimator.SetBool("UIappeared", true);
                promptAnimator.SetTrigger("UIappearing");
            }

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //playerInRange = false;
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



