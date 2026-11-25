using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public enum BossPhase
{
    Deal,
    Tell,
    Reaction,
    Showdown
}

public class PokerBossManager : MonoBehaviour
{
    //This is the most important script. 
    //It controls the entire four-phase gameplay loop.
    //It should know which phase the encounter is currently in, 
    //when to move to the next phase, and when the encounter ends.
    //It should NOT contain the actual calculations for each QTE. 
    //That's the QTE manager's responsibility.

    public BossPhase currentBossPhase;
    [Header("Bosses")]
    [SerializeField] private PokerBossData[] currentBoss;//whether the first boss or second boss
    [SerializeField] private int currentBossIndex;//whether the first boss or second boss

    [SerializeField] private PokerBossState currentBossState;
    private PokerBossData CurrentBossData => currentBoss[currentBossIndex];
    [SerializeField] private QTEManager qteManager;
    [SerializeField] private PokerHypervigilanceManager pokerHypervigilanceManager;
    [SerializeField] private Animator bossAnimationController;
    //[SerializeField] private PokerBossCutsceneManager pokerBossCutsceneManager;

    [Header("Scene Transition")]
    [SerializeField] private string nextSceneName;
    [SerializeField] private UnityEvent onFinalBossDefeated;
    [Header("Runtime")]
    [SerializeField] private float tellDuration;//tell phase
    [SerializeField] private float reactionDuration;//reaction phase
    [SerializeField] private float showdownDuration;//showdown phase
    [SerializeField] private bool currentQTESuccess;//Whether the current QTE was completed successfully
    [SerializeField] private bool isEncounterActive;//Prevents gameplay after the encounter ends.
    [SerializeField] private bool tookDoseThisRound;
    private Coroutine phaseCoroutine;
    public event Action<BossPhase> OnPhaseChanged;
    
    
    private void OnEnable()
    {
        qteManager.CompleteQTE += StartShowdownPhase;
    }
    private void OnDisable()
    {
        qteManager.CompleteQTE -= StartShowdownPhase;
    }
    public void StartEncounter()
    {
        //Initializes a boss encounter 
        // qteManager.StartQuickDraw();
        //qteManager.StartAllIn();
        //qteManager.StartReadSequence();
        if (currentBoss == null || currentBoss.Length < 2)
        {
            UnityEngine.Debug.LogError("PokerBossManager requires two boss data assets.");
            return;
        }
        isEncounterActive = true;

        currentBossIndex = 0;
        currentBossState.Initialize(currentBoss[0]);

        StartDealPhase();
    }    
    private void StartDealPhase()
    {
        //Starts the untimed Deal phase
        //Player choose take a dose?
        //listen for events from Use Button 
        //show options
        if (isEncounterActive)
        {   
            tookDoseThisRound = false;

            ChangePhase(BossPhase.Deal);
        }
    }
    public void TakeADose()
    {
        //button call
        //open inventory
        //leave the player option to cancel taking a dose
        if (!isEncounterActive || currentBossPhase != BossPhase.Deal)
            return;

        if (!currentBossState.TryUseDose())
            return;

        tookDoseThisRound = true;

        //UpdateDoseUI();

        //UnityEngine.Debug.Log("Player took a dose!");

        StartTellPhase();
    }

    public void Bet()
    {
        //button call
        //Ends Deal and begins Tell

        //Important: should only be available during Deal. 
        // It should also work whether or not the player has taken a dose.

        if (!isEncounterActive || currentBossPhase != BossPhase.Deal)
            return;

        tookDoseThisRound = false;
        //UpdateDoseUI()

        //UnityEngine.Debug.Log("Player chose to bet!");

        StartTellPhase();
    }

    private void StartTellPhase()
    {
        if (isEncounterActive)
        {
            //Selects an attack and displays its sprite tell 
            ChangePhase(BossPhase.Tell);
            float tellDuration;

            if (tookDoseThisRound)
            {
                tellDuration = CurrentBossData.doseTellTime;
            }
            else
            {
                tellDuration = CurrentBossData.GetTellTime(currentBossState.CurrentDifficulty);
            }
            qteManager.StartQTE(tellDuration);
        }
    }
   

    private void StartReactionPhase()
    {
        //Begins the QTE and its timer 
        ChangePhase(BossPhase.Reaction);
        //updateUI
    }
    private void StartShowdownPhase(bool success)
    {
        ChangePhase(BossPhase.Showdown);
        //Ends QTE input and displays the result 
        if (success)
        {
            currentBossState.DecreaseBossConfidence();
        }
        else
        {
            currentBossState.DecreasePlayerConfidence();
        }
        //UpdateShowdownUI();
        phaseCoroutine = StartCoroutine(ShowdownRoutine());
    }
    private IEnumerator ShowdownRoutine()
    {
        yield return new WaitForSeconds(CurrentBossData.showdownTime);

        EndShowdownPhase();
    }
    private void EndShowdownPhase()
    {
        //Applies confidence changes and determines what happens next
        // Player defeat check.
        if (currentBossState.currentPlayerConfidence <= 0f)
        {
            PlayerDefeated();
            return;
        }

        // Boss defeat check.
        if (currentBossState.currentBossConfidence <= 0f)
        {
            BossDefeated();
            return;
        }

        // Neither side is defeated. Start another round.
        RestartDealLoop();
    }
    private void BossDefeated()
    {
        // There is another boss.
        if (currentBossIndex < currentBoss.Length - 1)
        {
            currentBossIndex++;

            currentBossState.LoadBoss(CurrentBossData);

            StartDealPhase();
        }
        else
        {
            // Final boss defeated.
            EndEncounter();

            UnityEngine.Debug.Log("All bosses defeated!");

            if (onFinalBossDefeated != null)onFinalBossDefeated.Invoke();

            if (!string.IsNullOrEmpty(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }
    private void PlayerDefeated()
    {
        UnityEngine.Debug.Log("PLAYER DEFEATED!");

        EndEncounter();


        // if (lostPanel != null)
        //     lostPanel.SetActive(true);
    }
    private void EndEncounter()
    {
        //Stops the gameplay loop 
        isEncounterActive = false;

        if (phaseCoroutine != null)
        {
            StopCoroutine(phaseCoroutine);
            phaseCoroutine = null;
        }
    }
    private void RestartDealLoop()
    {
        //Starts the next Deal phase 
        StartDealPhase();
    }
    private void ChangePhase(BossPhase newPhase)
    {
        currentBossPhase = newPhase;
        OnPhaseChanged?.Invoke(currentBossPhase);
    }

}