using System;
using UnityEngine;
using Yarn.Unity;

public class NPC_New : MonoBehaviour, IInteractable
{
    public static event Action<NPCProfile_New> OnStartInteraction;

    private bool playerInRange = false;

    [SerializeField] private NPCProfile_New profile; // make sure to change when completely finished to NPCProfile!!!

    [Header("UI")]
    public Animator promptAnimator;
    [SerializeField] private GameObject promptCanvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //CheckTrustLevel();
        CheckCurrentDialogueNode(); // perhaps make an event???


    }


    void OnEnable()
    {
        // laptop.PriceChanged += OnPriceChanged;
        NPCProfile_New.OnTrustLevelChange += CheckTrustLevel;
        FavorManager.OnFavorComplete += CompletedFavor;

    }

    void OnDisable()
    {
        NPCProfile_New.OnTrustLevelChange -= CheckTrustLevel;
        FavorManager.OnFavorComplete -= CompletedFavor;
    }

    public void Interact()
    {
        // pull up proper dialogue
        //Debug.Log("Hello World!");
        if (!profile.GetHasMetPlayer())
            profile.MetPlayer();

        //Debug.Log("node: " + profile.GetCurrDialogueNode());
        DialogueManager_New.Instance.StartConversation(profile.GetCurrDialogueNode()); // old: StartConversation(testingIntro)
    }

    private void CheckTrustLevel(float lvl)
    {
        if (profile.GetTrustLevel() < 0)
        {
            profile.SetTrustLevel(0);
        }

        if (profile.GetTrustLevel() > profile.GetMaxTrust())
        {
            profile.SetTrustLevel(profile.GetMaxTrust());
        }

        
    }

    [YarnCommand("Activate_Favor")]
    public void StartFavor()
    {

        FavorManager.Instance.StartFavor(profile.GetState().activeFavor.favor);
 
    }

    private string CheckCurrentDialogueNode()
    {
        return ""; // stub!!
    }

    private void CompletedFavor(FavorState favor)
    {
        profile.IncreaseCompFavors();
    }

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
            Debug.Log("playerInRange is "+playerInRange);

            if (promptAnimator != null)
            {
                promptAnimator.SetBool("UIappeared", false);
                promptAnimator.SetTrigger("UIdisappearing");
                //promptCanvas.transform.position += new Vector3(0, -2f, 0); // go back to below npc heads
            }
        }
    }
}



