using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DownerScript : MonoBehaviour
{
    //this script is for the downer
    // add time to game, make NPCs go to sleep
    

    //[SerializeField] private TMP_Text useButtonText; // Reference to the Text component of the button
    [Header("Downer Settings")]
    [SerializeField] private float downerDuration = 90f;




    // void Start()
    // {
    //     // Set the initial text of the button based on the current state
    
    // }
    public void AddTime()
    {
        Debug.Log("Downer ability activated!");
        DayManager.Instance.AddTime(downerDuration);
    }
    
    
    
}