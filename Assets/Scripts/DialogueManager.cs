using StarterAssets;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using Yarn.Unity;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;


    // EVENTS ------------------------------------------------------->>>

    // VARIABLES ----------------------------------------------------->>>

    // yarnspinner refs
    [SerializeField] private DialogueRunner dialogueRunner;
    [SerializeField] private CustomLineAdvancer customLineAdvancer;



    // METHODS ----------------------------------------------------->>>


    public void StartConversation(string dialogueNode)
    {
        if (string.IsNullOrEmpty(dialogueNode))
        {
            Debug.LogError("Tried to start dialogue with NULL or empty node!");
            return;
        }

        // SWITCH INPUT MAP
        ThirdPersonController.Instance.GetComponent<PlayerInput>().SwitchCurrentActionMap("Mouse");
        CursorManager.Instance.LockCursor();

        // TELL YARNSPINNER DIALOGUE SYSTEM TO PLAY DIALOGUE
        customLineAdvancer.OnDialogueStarted();
        dialogueRunner.StartDialogue(dialogueNode);

    }

    public void EndConversation() // CALLED VIA DIALOGUE RUNNER
    {
        // SWITCH INPUT MAP
        ThirdPersonController.Instance.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        CursorManager.Instance.UnlockCursor();

        // DISABLE FURTHER INPUT
        customLineAdvancer.OnDialogueComplete();

    }

    public void BeginSelling() // should bring up inventory. Only the cheese inventory should be available (other tabs unclickable)
    {
        // PLEASE FILL THIS OUT. IT SHOULD BE SIMPLE
        // YOU ALSO NEED TO FIX UP THE CODE THAT CHECKS WHETHER 
        // OR NOT IT IS A TIME TO SELL WHEN INVENTORY IS UP.
        // YOU ALSO NEED TO ACOUNT FOR PLAYERS SELECTING A CERTAIN AMOUNT/TYPE OF CHEESE WITH OUR NEW
        // REQUEST SYSTEM
        // PLEASE REVIEW EVERYTHING AND KEEP IT SIMPLE, STUPD, BUT CLEVER!!
        // REMEMBER, YOU ARE A PROGRAMMER! I SHOULDN'Y HAVE TO HOLD YOUR HAND THROGUH THIS.
        // I SHOULD BE ABLE TO TRUST THAT I CAN ASK YOU FOR A SIMPLE SYSTEM LIKE THIS
        // AND YOU'RE ABLE TO REVIEW WHAT YOU NEED AND CREATE IT

    }



    // Unity Methods --->

    private void Awake()
    {

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);



        // ADD EVENTS THAT CAN BE USED IN YARN SCRIPTS!
        dialogueRunner.AddCommandHandler("begin_selling", BeginSelling);











    }


}
