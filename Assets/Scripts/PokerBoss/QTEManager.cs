using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum QTEType
{
    QuickDraw,
    Read,
    AllIn
}

public class QTEManager : MonoBehaviour
{

    //This is the second most important gameplay script.
    //It handles choosing which QTE happens, keeping track of the player's input,
    //and determining whether they succeed or fail.
    //We should keep all three QTE implementations inside this one manager for now.
    //We don't need three separate MonoBehaviours just to handle three relatively simple mechanics.


    [SerializeField] private QTEData currentQTE;
    [SerializeField] private QTEType currentQTEType;
    [SerializeField] private float qteTimer;//Remaining Reaction time 
    [SerializeField] private bool isQTEActive;//Whether QTE input is currently accepted 
    [SerializeField] private bool isQTECompleted;//Whether the player completed the QTE 
    [SerializeField] private int readSequenceIndex;//Tracks progress through The Read sequence 
    [SerializeField] private float allInIndicatorPosition;
    //Current position of the All-In indicator 
    //(where the zone the player wants to hit the skill check)
    //prolly image
    [Header("QTE Types")]

    [SerializeField] private QuickDraw quickDraw;

    public void StartQuickDraw()
    {
        quickDraw.StartQuickDraw(OnQuickDrawFinished);
        //receiving bool from StarQuickDraw function
    }

    private void OnQuickDrawFinished(bool success)
    {
        if (success)
        {
            Debug.Log("Player won the quick draw!");

            // TODO: Boss loses confidence
            // TODO: Play success animation
        }
        else
        {
            Debug.Log("Player lost the quick draw!");

            // TODO: player loses confidence
        }
    }
    private void SelectRandomQTE()
    {
        //Consumes one dose if available 
    }
    private void StartQTE() 
    {
        //Initializes the selected QTE 


    }
    
    private void CheckQTEInput() 
    {
        //Determines which QTE input was pressed
    }
    private void CheckQuickDraw() 
    {
        //Checks whether F was pressed
    }
    private void CheckRead() 
    {
        //Checks Q → E → F sequence 
    }
   
    private void CheckAllIn() 
    {
        //Checks whether Space was pressed in the target zone
    }
    private void SucceedQTE() 
    {
        //Marks QTE as successful 
    }
   
    private void FailQTE() 
    {
        //Fail QTE
    }
   
    private void EndQTE() 
    {
        //Stops accepting input and ends QTE calculations | *Input handling

    }
   
    private void GetQTEResult() 
    {
        //Returns whether the player succeeded 
    }
   








}