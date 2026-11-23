using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
public class PhoneBoothInteractionScript : MonoBehaviour, IInteractable
{

    // EVENTS --------------------------------------------->>>
    public static event Action OnPhoneBoothInteract;


    // VARIABLES --------------------------------------------->>>

    private bool playerInRange = false;

    [Header("UI")]
    public Animator promptAnimator;
    [SerializeField] private GameObject promptCanvas;





    // METHODS --------------------------------------------->>>

    public void Interact()
    {
        //Debug.Log("interacting with PhoneBooth");

        // need to open phone booth UI panel via event
        OnPhoneBoothInteract?.Invoke();



    }


    // Unity Methods --->>>

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("playerInRange is " + playerInRange);


            if (promptAnimator != null)
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
            playerInRange = false;
            //Debug.Log("playerInRange is "+playerInRange);

            if (promptAnimator != null)
            {
                promptAnimator.SetBool("UIappeared", false);
                promptAnimator.SetTrigger("UIdisappearing");
            }
        }
    }
}



