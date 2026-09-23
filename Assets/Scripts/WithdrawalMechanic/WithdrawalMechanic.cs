using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WithdrawalMechanic : MonoBehaviour
{
    public static WithdrawalMechanic Instance;

    
     
    [Header("UI")]
    [SerializeField] private GameObject withdrawalIcon;//show player how much left
    [Header("Variables")]
    [SerializeField] private float withdrawalTime = 600.0f;
    [SerializeField] private float maximumWithdrawalTime = 600.0f;
    [SerializeField] private float withdrawalDrainRate = 1.0f;
    [SerializeField] private float withdrawalDrainRateAfterEatingCheese = 0.3f;
    [SerializeField] private float cheeseSatiation = 100.0f;
    //this will be changed by the cheese rarity
    [SerializeField] private bool isWithdrawalActive = false;
    [SerializeField] private int cheeseTolerance = 3;//max cheese can eat before passout

    void Start()
    {
        withdrawalIcon.SetActive(false);
    }
    
    void Update()
    {
        WithdrawalDrain();
    }
    void WithdrawalDrain()
    {
        withdrawalTime -= withdrawalDrainRate * Time.deltaTime;
        if (withdrawalTime <= 0)
        {

            withdrawalTime = maximumWithdrawalTime;
            PassOut();
        }
        if(withdrawalTime >= maximumWithdrawalTime)
        {
            withdrawalTime = maximumWithdrawalTime;
        }
        
    }
    void SatiateAddiction()
    {
        withdrawalTime += cheeseSatiation;
        withdrawalDrainRate += withdrawalDrainRateAfterEatingCheese;
        //player becomes more addicted to cheese after eating it
        // so the withdrawal drain rate increases
    }
    void PlayerIsWithdrawn()
    //cheese tolerance doesn't increase when player is withdrawn
    //so player can eat as much cheese as they need.
    {
        isWithdrawalActive = true;
        //withdrawalIcon.SetActive(true);
    }
    void PassOut()
    {
        Debug.Log("Player has passed out due to withdrawal.");

    }
}
