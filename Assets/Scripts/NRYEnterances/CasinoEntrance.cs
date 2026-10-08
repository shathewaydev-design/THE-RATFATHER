using System;
using UnityEngine;

public class CasinoEntrance : MonoBehaviour
{
    public static event Action<Transform> OnEnteredCasino;
    public static event Action<Transform> OnExitedCasino;

    [SerializeField] Transform casinoEntrance;
    [SerializeField] Transform casinoExit;

    [SerializeField] private bool isEnterance;
    [SerializeField] private bool isExit;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isEnterance)
            {
                //Debug.Log("Detected player at enterance!");
                OnEnteredCasino?.Invoke(casinoExit);
            }
            else if (isExit) 
            {
                OnExitedCasino?.Invoke(casinoEntrance);
            }
            

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
        }
    }

}
