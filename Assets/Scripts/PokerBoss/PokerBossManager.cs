using UnityEngine;
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
    //[SerializeField] private PokerBossData currentBoss;//whether the first boss or second boss
    [SerializeField] private PokerBossState currentBossState;
    [SerializeField] private QTEManager qteManager;
    [SerializeField] private PokerHypervigilanceManager pokerHypervigilanceManager;
    [SerializeField] private Animator bossAnimationController;
    //[SerializeField] private PokerBossCutsceneManager pokerBossCutsceneManager;

    [SerializeField] private float tellDuration;//tell phase
    [SerializeField] private float reactionDuration;//reaction phase
    [SerializeField] private float showdownDuration;//showdown phase
    [SerializeField] private bool currentQTESuccess;//Whether the current QTE was completed successfully
    [SerializeField] private bool isEncounterActive;//Prevents gameplay after the encounter ends.

    public void StartEncounter()
    {
        //Initializes a boss encounter 
        // qteManager.StartQuickDraw();
        qteManager.StartAllIn();

    }    
    private void StartDealPhase()
    {
        //Starts the untimed Deal phase
    }

    private void Bet()
    {
        //Ends Deal and begins Tell

        //Important: should only be available during Deal. 
        // It should also work whether or not the player has taken a dose.


    }

    private void StartTellPhase()
    {
        //Selects an attack and displays its sprite tell 
    }
    private void StartReactionPhase()
    {
        //Begins the QTE and its timer 
    }
    private void StartShowdownhase()
    {
        //Ends QTE input and displays the result 


    }
    private void EndShowdownPhase()
    {
        //Applies confidence changes and determines what happens next
    }
    private void UpdateBossDifficulty()
    {
        //Determines which difficulty bracket the boss occupies
    }
    private void CalculatePhaseTimers()
    {
        //Determines Tell and Reaction durations 
    }
    private void EndEncounter()
    {
        //Stops the gameplay loop 
    }
    private void RestartDealLoop()
    {
        //Starts the next Deal phase 
    }







}