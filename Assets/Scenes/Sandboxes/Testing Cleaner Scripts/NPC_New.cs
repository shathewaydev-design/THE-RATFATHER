using System;
using Unity.VisualScripting;
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
        profile.SetMetPlayer(false);
        if (!profile.GetHasMetPlayer())
        {
            profile.GetState().compFavors = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("" + profile.GetState().compFavors);
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
        //if (!profile.GetHasMetPlayer())
        //    profile.MetPlayer();


        string dialogueNode = profile.GetCurrDialogueNode();
        //Debug.Log("Starting Yarn node: " + dialogueNode);

        //Debug.Log("node: " + profile.GetCurrDialogueNode());
        DialogueManager_New.Instance.StartConversation(dialogueNode, this); // old: StartConversation(testingIntro)
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

    //public void ActivateFavor()
    //{
    //    //profile.GetState().activeFavor.favor = null;
    //    Debug.Log("Activate favor command called!");
    //    FavorManager.Instance.StartFavor(profile.GetCurrentFavor());
 
    //}

    private void CompletedFavor(FavorState favor)
    {
        profile.IncreaseCompFavors(favor);
    }

    public NPCProfile_New GetProfile()
    {
        return profile;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            //Debug.Log("playerInRange is " + playerInRange);


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



