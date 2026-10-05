using System;
using UnityEngine;
using UnityEngine.AI;
using Yarn.Unity;

public class Enforcer : MonoBehaviour
{

    // 1. walks along set path
    // 2. when player enters line of sight,
    //    trigger dialogue
    // 3. take player resources based on dialogue option

    // -------------global variables---------------
    [SerializeField] float waitTimeOnWayPoint;
    [SerializeField] EnforcerPath path;

    NavMeshAgent agent;
    [SerializeField] Animator animator;

    float time = 0f;

    [SerializeField] private DialogueRunner dialogueRunner;
    
    [SerializeField] private Transform resetTeleport;
    public static event Action<Transform> OnConsequenceComplete;

    //  -------------main methods---------------
    private void Awake()
    {
        
        agent = GetComponent<NavMeshAgent>();
        //animator = GetComponent<Animator>();
        dialogueRunner.AddCommandHandler("activate_inventory_consequence", ActivateInventoryConsequence);
        dialogueRunner.AddCommandHandler("activate_currency_consequence", ActivateCurrencyConsequence);


    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent.destination = path.GetCurrentWaypoint();
    }

    private void OnEnable()
    {
        FieldOfView.OnPlayerSeen += CatchPlayer;
    }

    private void OnDisable()
    {
        FieldOfView.OnPlayerSeen -= CatchPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (agent.remainingDistance <= 0.1f)
        {
            time += Time.deltaTime;
            if (time >= waitTimeOnWayPoint)
            {
                agent.destination = path.GetNextWaypoint();
                time = 0f;
            }
        }

        float normalizedSpeed = Mathf.InverseLerp(0f, agent.speed, agent.velocity.magnitude);
        animator.SetFloat("speed", normalizedSpeed);

    }

    private void CatchPlayer()
    {
        // fade to black, move camera (p2 or p3)
        // start conversation
        // give player proper consequence

        DialogueManager_New.Instance.StartConversation("Enforcer_Dialogue", null);
        agent.isStopped = true;

        // need to teleport player away after as well
        // then trigger event to reset enforcer

    }

    public void ActivateInventoryConsequence()
    {

        // go through inventory and take cheese
        // just removes one for now, will take more later
        InventorySystem.Instance.RemoveFinalCheese(InventorySystem.Instance.cheeseInventory[0].finalCheeseData);
        OnConsequenceComplete?.Invoke(resetTeleport);
        agent.isStopped = false;
        

    }

    public void ActivateCurrencyConsequence() 
    {
        // take a set number of money from player

        float subNum = InventorySystem.Instance.GetCurrency();
        InventorySystem.Instance.SubtractFromCurrency(subNum);

        // later, check if player has no money. If so, do a different 
        // consequence
        OnConsequenceComplete?.Invoke(resetTeleport);
        agent.isStopped = false;

    }
}
