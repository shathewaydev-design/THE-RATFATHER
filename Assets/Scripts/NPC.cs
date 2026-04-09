using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public NPCProfile profile;
    public NPCState state;

    public ConversationData intro;
    public DialogueManager dialogueManager;

    [SerializeField] private float interactionRadius = 1.5f; // checking distancer manualy, not using triggers
    [SerializeField] private Transform playerTransform;

    // private Coroutine exitRoutine; // for some reason prompt ui is acting up, so this is an attempt to fix that

    [Header("Interaction")]
    [SerializeField] private float holdTime = 1.0f;
    [SerializeField] private float currentHoldTime = 0f;
    private bool playerInRange = false;

    [Header("UI")] 
    public Animator promptAnimator;

    public Image holdProgressBar;

    void Start()
    {
        state = new NPCState();
    }
    public void Interact()
    {
        if (!state.hasMetPlayer)
        {
            dialogueManager.StartConversation(intro);
            state.hasMetPlayer = true;
        }

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

}
