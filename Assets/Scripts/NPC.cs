using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{

    public DialogueData dialogue;
    public DialogueManager dialogueManager;

    [SerializeField] private float interactionRadius = 1.0f; // checking distancer manualy, not using triggers
    [SerializeField] private Transform playerTransform;

    // private Coroutine exitRoutine; // for some reason prompt ui is acting up, so this is an attempt to fix that

    [Header("Interaction")]
    [SerializeField] private float holdTime = 1.0f;
    [SerializeField] private float currentHoldTime = 0f;
    private bool playerInRange = false;

    [Header("UI")] 
    public Animator promptAnimator;

    public Image holdProgressBar;


    public void Interact()
    {
        dialogueManager.StartDialogue(dialogue);
    }

    // Update is called once per frame
    void Update()
    {
        //  if (!playerInRange) return;
        if (playerTransform == null) return;

        float dist = Vector3.Distance(playerTransform.position, transform.position);

        if (dist <= interactionRadius && !playerInRange)
        {
            playerInRange = true;
            Debug.Log("Press E to advance dialogue");
            ShowPrompt();

        }
        else if (dist > interactionRadius && playerInRange)
        {
            playerInRange = false;
            Debug.Log("");
            HidePrompt();
        }

    }
    void ShowPrompt()
    {
        if (promptAnimator != null)
        {
            promptAnimator.SetBool("UIappeared", true);
            promptAnimator.SetTrigger("UIappearing");
        }
    }

    void HidePrompt()
    {
        if (promptAnimator != null)
        {
            promptAnimator.SetBool("UIappeared", false);
            promptAnimator.SetTrigger("UIdisappearing");
        }
    }

    // OLD CODE THAT HANDLED INTERACTION PROMPTS WITH TRIGGER ZONES.
    // MAY NEED TO CLEAN NPC TEMPLATE OBJ DUE TO UPDATE, BUT WORKS FINE FOR NOW
    //    void OnTriggerEnter(Collider other)
    //    {
    //        if (other.CompareTag("Player"))
    //        {
    //            if (exitRoutine != null)
    //                StopCoroutine(exitRoutine);

    //            playerInRange = true;
    //            //Debug.Log("ENTER at " + Time.time);
    //            //Debug.Log("ENTER: " + other.name);
    //            //Debug.Log("playerInRange is " + playerInRange);
    //            if (promptAnimator != null)
    //            {
    //                promptAnimator.SetBool("UIappeared", true);
    //                promptAnimator.SetTrigger("UIappearing");
    //            }


    //           Debug.Log("Press E to advance dialogue"); // Replace with UI later
    //        }
    //    }

    //    void OnTriggerExit(Collider other)
    //    {

    //        if (other.CompareTag("Player"))
    //        {
    //           // if (exitRoutine != null)
    //             //   StopCoroutine(exitRoutine);

    //            exitRoutine = StartCoroutine(HandleExit());
    //        }

    //    }

    //    IEnumerator HandleExit() // fixes UI prompt reappearing after collider exit
    //    {
    //        yield return new WaitForSeconds(0.4f); // small delay

    //        playerInRange = false;
    //        currentHoldTime = 0f;

    //        if (promptAnimator != null)
    //        {
    //            promptAnimator.SetBool("UIappeared", false);
    //            promptAnimator.SetTrigger("UIdisappearing");
    //        }
    //    }
}
