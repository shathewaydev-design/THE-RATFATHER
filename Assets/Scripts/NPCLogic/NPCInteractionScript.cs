using System;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
public class NPCInteractionScript : MonoBehaviour, IInteractable
{

    private bool playerInRange = false;

    [SerializeField] private NPCProfile profile;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI npcName;
    public Animator promptAnimator;
    [SerializeField] private GameObject promptCanvas;
    [SerializeField] private GameObject TrustPanel;
    [SerializeField] private GameObject TrustIndicator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created   
    void Start()
    {
        NPCState npcState = new NPCState();
        npcState.hasMetPlayer = false;
        npcState.isCustomer = false;
        npcState.currDialogueNode = profile.introNode;

        profile.npcState = npcState;

    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnEnable()
    {

    }

    void OnDisable()
    {

    }

    public void Interact()
    {
        if (!profile.npcState.hasMetPlayer)
        {
            // ALSO NEED TO CHECK IF THE FREE SAMPLE WAS SUCCESFUL
            // IF NOT, HAS MET PLAYER NEEDS TO STAY TRUE
            // FOR NOW, ALL FREE SAMPLE CHECKS CAN BE SUCCESFUL FOR TESTING PURPOSES


            profile.npcState.hasMetPlayer = true;
            // trigger new dialoguemanager (yarnspinner) with profile.introNode
            return;
        }

        // trigger new dialoguemanager (yarnspinner) with profile.customerNode


    }








    // UI PROMPT POPUP ---------------------------------------->

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("playerInRange is " + playerInRange);


            if (promptAnimator != null)
            {
                //promptCanvas.transform.position += new Vector3(0, 2f, 0); // making "E" appear obce npc heads
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
                //promptCanvas.transform.position += new Vector3(0, -2f, 0); // go back to below npc heads
            }
        }
    }
}



