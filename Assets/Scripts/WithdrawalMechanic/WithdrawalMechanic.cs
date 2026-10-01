using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class WithdrawalMechanic : MonoBehaviour
{
    public static WithdrawalMechanic Instance;

    // =========================================================
    // WITHDRAWAL STAGES
    // =========================================================
    public enum WithdrawalStage
    {
        Stable,     // 100% - 75%
        Craving,    // <75% - 50%
        Withdrawal, // <50% - 25%
        Critical,   // <25% - 5%
        Emergency,  // <5% - 0%
        PassedOut
    }

    [Header("Current State")]
    [SerializeField] private WithdrawalStage currentStage = WithdrawalStage.Stable;

    // =========================================================
    // WITHDRAWAL TIMER
    // =========================================================

    [Header("Withdrawal Timer")]
    [SerializeField] private float baseWithdrawalTime = 1200.0f;
    //this number increases when player eats cheese 
    // and is reset to baseWithdrawalTime at the start of each day
    [SerializeField] private float timeUntilWithdrawal = 1200.0f;
    //current time left until player enters withdrawal state
    //this number is being drained every second
    //[SerializeField] private float maximumWithdrawalTime = 1200.0f;
    [SerializeField] private float maximumWithdrawalDrainRate = 1.0f;
    //doesnt change
    [SerializeField] private float withdrawalDrainRate = 1.0f;//per second
    [SerializeField] private float withdrawalDrainRateAfterEatingCheese = 0.0825f;
    //partial reset after each night
    //satiation effectiveness does decrease every time player eats cheese
    //but does regain abit after each night

    //this will be changed by the cheese rarity
    [SerializeField] private float resetAmountEndOfDay = 0.15f;
    //15% of maximumWithdrawalTime will be reset at the end of each day

    // =========================================================
    // WITHDRAWAL THRESHOLDS
    // =========================================================

    [Header("Stage Thresholds")]

    [Range(0f, 1f)]
    [SerializeField] private float cravingThreshold = 0.75f;

    [Range(0f, 1f)]
    [SerializeField] private float withdrawalThreshold = 0.50f;

    [Range(0f, 1f)]
    [SerializeField] private float criticalThreshold = 0.25f;
     //this state is opened from 15%-0% to increase tension
    //player can choose to enter Emergency Satiation Mode
    //player in withdrawal state and can eat as much cheese as they want 
    //without increasing cheeseBuffCapacity 
    //but doesnt count the buff either.
    //20s duration
    //unlock this state again when timeUntilWithdrawal is at 15%
    //camera and UIs shake --->when exit state, stabilizes

    [Range(0f, 1f)]
    [SerializeField] private float emergencyThreshold = 0.05f;
    // =========================================================
    // CHEESE / TOLERANCE
    // =========================================================

    [Header("Cheese / Tolerance")]

    [SerializeField] private float cheeseSatiation = 100f;
    [SerializeField] private CheeseSatiationAmountScript cheeseSatiationAmountScript;
    //this script holds the satiation amount for each cheese rarity

    [SerializeField] private float toleranceIncrease = 1f;

    [SerializeField] private float maximumTolerance = 5f;
    //max cheese can eat to apply buff
    //if eat more than 3 cheese, the buff will not be applied
    //Player feedback: "You can't handle another dose."
    private float cheeseBuffCapacity = 3f;//how many buffs player can have at once.
    [SerializeField] private float currentCheeseBuffs = 0f;
    [SerializeField] private string warningText = "You can't handle another dose.";
    // =========================================================
    // DESPERATE FIX MODE
    // =========================================================

    [Header("Desperate Fix Mode")]

    [SerializeField] private float desperateFixMultiplier = 1.15f;
    //withdrawal recovery rate is increased by 15% when player is in desperate fix mode
    [SerializeField] private float desperateFixDuration = 20f;
    [SerializeField] private bool canEnterDesperateFix = false;
    [SerializeField] private bool isInDesperateFix = false;

    [Header("UI")]
    [SerializeField] private GameObject[] withdrawalIcons;
    //show player how much left until withdrawal
    //can be hovered to see %
    //UI effect, white opaque background
    [Header("Variables")]
    
    
    [SerializeField] private bool isDesperateFix = false;
   
    

 

    // =========================================================
    // EVENTS
    // =========================================================

    public event Action<WithdrawalStage> OnWithdrawalStageChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    void Start()
    {
        timeUntilWithdrawal = baseWithdrawalTime;
        foreach (GameObject icon in withdrawalIcons)
        {
            icon.SetActive(false);
        }
        UpdateWithdrawalStage();
    }
    
    void Update()
    {
        WithdrawalDrain();
    }
    // =========================================================
    // WITHDRAWAL TIMER
    // =========================================================

    private void WithdrawalDrain()
    {
        // Don't drain after passing out.
        if (currentStage == WithdrawalStage.PassedOut)
            return;
        //dont drain when player is in cooking mode
        timeUntilWithdrawal -= withdrawalDrainRate * Time.deltaTime;

        timeUntilWithdrawal = Mathf.Max(timeUntilWithdrawal, 0f);

        UpdateWithdrawalStage();

        if (timeUntilWithdrawal <= 0f)
        {
            PassOut();
        }
    }
    // =========================================================
    // DETERMINE STAGE
    // =========================================================

    private void UpdateWithdrawalStage()
    {
        float percentage = GetWithdrawalPercentage();

        WithdrawalStage newStage;

        if (percentage > cravingThreshold)
        {
            newStage = WithdrawalStage.Stable;
        }
        else if (percentage > withdrawalThreshold)
        {
            newStage = WithdrawalStage.Craving;
        }
        else if (percentage > criticalThreshold)
        {
            newStage = WithdrawalStage.Withdrawal;
        }
        else if (percentage > emergencyThreshold)
        {
            newStage = WithdrawalStage.Critical;
        }
        else
        {
            newStage = WithdrawalStage.Emergency;
        }

        // Only do something when the stage actually changes.
        if (newStage != currentStage)
        {
            WithdrawalStage previousStage = currentStage;

            currentStage = newStage;

            OnStageChanged(previousStage, newStage);

            OnWithdrawalStageChanged?.Invoke(newStage);
        }
    }
    // =========================================================
    // STAGE CHANGE
    // =========================================================

    private void OnStageChanged(WithdrawalStage previousStage, WithdrawalStage newStage)
    {
        Debug.Log($"Withdrawal changed: {previousStage} -> {newStage}");

        switch (newStage)
        {
            case WithdrawalStage.Stable:
                EnterStable();
                break;

            case WithdrawalStage.Craving:
                EnterCraving();
                break;

            case WithdrawalStage.Withdrawal:
                EnterWithdrawal();
                break;

            case WithdrawalStage.Critical:
                EnterCritical();
                break;

            case WithdrawalStage.Emergency:
                EnterEmergency();
                break;
        }
    }
    // =========================================================
    // STAGE FUNCTIONS
    // =========================================================

    private void EnterStable()
    {
        Debug.Log("Player is stable.");

        // UI:
        // Hide all withdrawal icons
        foreach (GameObject icon in withdrawalIcons)
        {
            icon.SetActive(false);
        }

        // Animation:
        // Normal idle

        // VFX:
        // None
    }

    private void EnterCraving()
    {
        Debug.Log("Player is craving cheese.");

        // UI:
        // Show small cheese icon
        withdrawalIcons[0].SetActive(true);//green outline
        withdrawalIcons[1].SetActive(false);
        withdrawalIcons[2].SetActive(false);
        // Green outline
        // Slight shaking animation
        //vingrette effect on screen

        // NPC:
        // Normal dialogue
    }

    private void EnterWithdrawal()
    {
        Debug.Log("Player is beginning withdrawal.");

        // UI:
        // Cheese icon partly eaten
        // Orange outline
        withdrawalIcons[0].SetActive(false);
        withdrawalIcons[1].SetActive(true);//orange outline
        withdrawalIcons[2].SetActive(false);
        // VFX:
        // Occasional screen shake
        // Other symptoms

        // NPC placeholder
        NPCWithdrawalDialogue();
    }

    private void EnterCritical()
    {
        Debug.Log("Player is critically withdrawn.");

        // UI:
        // More eaten cheese icon
        // Red outline
        withdrawalIcons[0].SetActive(false);
        withdrawalIcons[1].SetActive(false);
        withdrawalIcons[2].SetActive(true);//red outline

        canEnterDesperateFix = true;
        // Animation:
        // Player looks around / appears nervous

        // NPC:
        // NPCs may notice player looks withdrawn

        NPCWithdrawalDialogue();
    }

    private void EnterEmergency()
    {
        Debug.Log("Player is in emergency withdrawal.");

        // UI:
        // Strong warning

        // VFX:
        // Stronger symptoms
        //stronger screen shake every 1min 

        // Player can now use Emergency Satiation.
    }

    // =========================================================
    // CHEESE CONSUMPTION
    // =========================================================

    public void EatCheese(CheeseRarity cheeseRarity)
    {
        if (currentStage == WithdrawalStage.PassedOut)
            return;

        
        if(isInDesperateFix)
        {
            float cheeseSatiationAmount = cheeseSatiationAmountScript.GetSatiationAmount(cheeseRarity);
            timeUntilWithdrawal += cheeseSatiationAmount * desperateFixMultiplier;

            Debug.Log("Player has entered Desperate Fix mode.");
            // Start a timer for the duration of Desperate Fix mode.
            StartCoroutine(DesperateFixTimer());
        }
        else
        {
            // Normal cheese consumption
            timeUntilWithdrawal += cheeseSatiationAmount;
        }
        timeUntilWithdrawal = Mathf.Clamp(timeUntilWithdrawal, 0f, baseWithdrawalTime);
        
        IncreaseTolerance();// Increase tolerance after eating.
        UpdateWithdrawalStage();
        ApplyCheeseBuff();
        Debug.Log($"Ate cheese. Restored {cheeseSatiationAmount:F1} seconds.");
    }

    public void ApplyCheeseBuff() 
    {
        if (currentCheeseBuffs < cheeseBuffCapacity)
        {
            // /Max buff player can have at once. 
            // If player eats more than the max, the buff will not be applied.
            currentCheeseBuffs++;
            Debug.Log($"Cheese buff applied. Current buffs: {currentCheeseBuffs}");
            // Apply the actual buff effect here 
        }
        else
        {
            Debug.Log(warningText);
            // "You can't handle another dose." 
        }
    }

    // =========================================================
    // EMERGENCY CHEESE
    // =========================================================

    public bool CanEnterDesperateFix()
    {
        return currentStage == WithdrawalStage.Emergency;
    }

    /*public void EmergencyEatCheese(float cheeseSatiationAmount)
    {
        if (!CanEnterDesperateFix())
        {
            Debug.Log("Emergency satiation is not available yet.");
            return;
        }

        // Emergency cheese restores withdrawal
        // without applying a cheese buff.
        //not sure if we want a multiplier to gain more cheese satiation.
        float actualSatiation = cheeseSatiationAmount * desperateFixMultiplier;

        timeUntilWithdrawal += actualSatiation;

        timeUntilWithdrawal = Mathf.Clamp(timeUntilWithdrawal, 0f, baseWithdrawalTime);

        // IMPORTANT:
        // Emergency eating does NOT increase tolerance.
        // It also does NOT give the cheese's normal buff.

        UpdateWithdrawalStage();

        Debug.Log(
            $"Emergency cheese consumed. Restored {actualSatiation:F1} seconds."
        );
    }*/

    // =========================================================
    // TOLERANCE
    // decrease effectiveness of withdrawal recovery after each cheese consumption
    // =========================================================

    private void IncreaseTolerance()
    {
        withdrawalDrainRate += withdrawalDrainRateAfterEatingCheese;

    }

    // =========================================================
    // NEW DAY
    // =========================================================

    public void NewDay()
    {
        // Partially reset withdrawal.
        /*baseWithdrawalTime -= resetAmountEndOfDay * maximumWithdrawalTime;
        if(baseWithdrawalTime <= maximumWithdrawalTime)
        {
            baseWithdrawalTime = maximumWithdrawalTime;
        }
        timeUntilWithdrawal = baseWithdrawalTime;*/

        withdrawalDrainRate -= resetAmountEndOfDay; 
        if(withdrawalDrainRate <= maximumWithdrawalDrainRate)
        {
            withdrawalDrainRate = maximumWithdrawalDrainRate;
        }

        // currentStage = WithdrawalStage.Stable;

        // OnWithdrawalStageChanged?.Invoke(
        //     WithdrawalStage.Stable
        // );

        Debug.Log("New day. Withdrawal and tolerance partial reset.");
    }

    // =========================================================
    // PASS OUT
    // =========================================================

    private void PassOut()
    {
        if (currentStage == WithdrawalStage.PassedOut)
            return;

        currentStage = WithdrawalStage.PassedOut;

        Debug.Log(
            "Player has passed out due to withdrawal."
        );

        // TODO:
        // Play pass-out animation
        // Disable player movement
        // Tell TimeManager to advance one day
        // Reset player position
        // Reset withdrawal timer
    }
 // =========================================================
    // NPC PLACEHOLDER
    // =========================================================

    private void NPCWithdrawalDialogue()
    {
        Debug.Log(
            "NPC noticed the player is experiencing withdrawal."
        );

        // Later:
        // Send information to NPC dialogue system.
        //
        // Example:
        // NPCDialogueManager.Instance.SetPlayerWithdrawalStage(
        //     currentStage
        // );
    }

    // =========================================================
    // FUNCTIONS THAT GET CURRENT DATA FOR VARIABLESs
    // =========================================================

    public float GetWithdrawalTime()
    {
        return timeUntilWithdrawal;
    }

    public float GetMaximumWithdrawalTime()
    {
        return baseWithdrawalTime;
    }

    public float GetWithdrawalPercentage()
    {
        if (baseWithdrawalTime <= 0f)
            return 0f;

        return timeUntilWithdrawal / baseWithdrawalTime;
    }

    public WithdrawalStage GetCurrentStage()
    {
        return currentStage;
    }

//     public float GetCheeseTolerance()
//     {
//         return cheeseTolerance;
//     }
    public IEnumerator DesperateFixTimer()
    {
        yield return new WaitForSeconds(desperateFixDuration);
        isInDesperateFix = false;
        canEnterDesperateFix = false;
        Debug.Log("Desperate Fix mode has ended.");
    }
}
