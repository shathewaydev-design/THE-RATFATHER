using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public NPCProfile profile;
    public NPCState state;

    public ConversationData currentConvo;
    public DialogueManager dialogueManager;
    public QuestManager questManager;

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
        if (profile.quests.Count == 0) return;

        Quest relevantQuest = questManager.GetRelevantQuest(profile.quests);

        if (relevantQuest == null)
            return;

        if (!state.hasMetPlayer)
        {
            currentConvo = relevantQuest.questIntro;
        }
        else
        {
            currentConvo = questManager.UpdateDialogue(relevantQuest);
        }

        dialogueManager.StartConversation(currentConvo);
        state.hasMetPlayer = true;

        Debug.Log("Relevant quest: " + relevantQuest.questName);

    }

    // Update is called once per frame
    void Update()
    {
        // mabe add this logic to interact method?
        // currentConvo = questManager.UpdateDialogue(profile.quests[0]); // change to loop through all quests to find the active one


        //  if (!playerInRange) return;
        if (playerTransform == null) return;

        float dist = Vector3.Distance(playerTransform.position, transform.position);

        if (dist <= interactionRadius && !playerInRange) // might need to make ui also match this...
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
