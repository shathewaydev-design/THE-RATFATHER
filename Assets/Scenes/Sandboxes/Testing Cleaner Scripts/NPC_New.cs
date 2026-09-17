using UnityEngine;

public class NPC_New : MonoBehaviour, IInteractable
{
    private bool playerInRange = false;
    public ConversationData testingIntro; // WILL BE STORED IN NPC PROFILE, JUST A TEST!!

    public string introConversationNode; // WILL BE STORED IN NPC PROFILE, JUST A TEST!!
    // private npc profile

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
        
    }

    public void Interact()
    {
        // pull up proper dialogue
        Debug.Log("Hello World!");
        DialogueManager_New.Instance.StartConversation(introConversationNode); // old: StartConversation(testingIntro)
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



