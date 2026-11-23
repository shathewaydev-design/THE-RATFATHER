using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PokerHypervigilanceManager : MonoBehaviour
{

    //This script handles the entire casino boss upper mechanic.
    //It should NOT control the phases itself. 
    //It simply communicates whether a dose was taken
    //and what timer values should be used.

    //[SerializeField] private PokerBossData currentBossData;//Reads dose settings 
    [SerializeField] private PokerBossState currentBossState;//Reads and updates remaining doses
    [SerializeField] private bool doseUsedThisRound;//Tracks whether a dose was taken during Deal 


    private void TakeDose()
    {
        //Consumes one dose if available 
    }
    private void HasDosesRemaining()
    {
        //Checks remaining doses 
    }
    
    private void GetTellDuration()
    {
        //Returns the Tell duration with or without Hypervigilance
    }
    private void GetReactionDuration()
    {
        //Returns the Reaction duration with or without Hypervigilance
    }
    private void ResetDoseForNextRound()
    {
        //Clears the current-round dose flag 
    }
   








}