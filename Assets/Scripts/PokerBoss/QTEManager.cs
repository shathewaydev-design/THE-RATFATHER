using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum QTEType
{
    QuickDraw,
    ReadSequence,
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
    [SerializeField] private AllIn allIn;
    [SerializeField] private ReadSequence readSequence;
    public event Action<bool> CompleteQTE;

    public void StartQuickDraw(float duration)
    {
        quickDraw.StartQuickDraw(duration, OnQuickDrawFinished);
        //receiving bool from StarQuickDraw function
    }

    private void OnQuickDrawFinished(bool success)
    {
        if (success)
        {
            Debug.Log("Player won the quick draw!");
            SucceedQTE();
            // TODO: Boss loses confidence
            // TODO: Play success animation
        }
        else
        {
            Debug.Log("Player lost the quick draw!");
            FailQTE();
            // TODO: player loses confidence
        }
    }
    public void StartAllIn(float duration)
    {
        allIn.StartAllIn(duration, OnAllInFinished);
    }
    private void OnAllInFinished(bool success)
    {
        if (success)
        {
            Debug.Log("Player won the all in!");
            SucceedQTE();
            // TODO: Boss loses confidence
            // TODO: Play success animation
        }
        else
        {
            Debug.Log("Player lost the all in!");
            FailQTE();
            // TODO: player loses confidence
        }
    }

    public void StartReadSequence(float duration)
    {
        readSequence.StartReadSequence(duration, OnReadSequenceFinished);
    }
    private void OnReadSequenceFinished(bool success)
    {
        if (success)
        {
            Debug.Log("Player won the all in!");
            SucceedQTE();
            // TODO: Boss loses confidence
            // TODO: Play success animation
        }
        else
        {
            Debug.Log("Player lost the all in!");
            FailQTE();
            // TODO: player loses confidence
        }
    }
    private void SelectRandomQTE(float duration)
    {
        //Consumes one dose if available 
        QTEType[] qteTypes = (QTEType[])Enum.GetValues(typeof(QTEType));
        //Enum.GetValues(typeof(QTEType))
        //->Give all the possible values inside the QTEType enum
        //->[QuickDraw, ReadSequence, AllIn]
        //(QTEType[]) type casing, converting enum to int

        int randomIndex = UnityEngine.Random.Range(0, qteTypes.Length);

        QTEType selectedQTE = qteTypes[randomIndex];

        switch(selectedQTE)
        {
            case QTEType.QuickDraw:
                StartQuickDraw(duration);
                break;
            case QTEType.ReadSequence:
                StartReadSequence(duration);
                break;
            case QTEType.AllIn:
                StartAllIn(duration);
                break;
        }
    }
    public void StartQTE(float duration) 
    {
        //TODO: Initializes the selected QTE 

        SelectRandomQTE(duration);

    }
    
    private void SucceedQTE() 
    {
        //Marks QTE as successful 
        CompleteQTE?.Invoke(true);
    }
   
    private void FailQTE() 
    {
        //Fail QTE
        CompleteQTE?.Invoke(false);
    }
   
}