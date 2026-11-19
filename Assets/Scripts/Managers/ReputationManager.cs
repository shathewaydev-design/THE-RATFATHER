using System;
using UnityEngine;



public enum ReputationTier
{
    Unknown,
    Recognized,
    Established,
    WellKnown,
    Infamous
}


public class ReputationManager : MonoBehaviour
{
    public static ReputationManager Instance;

    // EVENTS  ---------------------------------------------------------->>
    public event Action<int> OnReputationChanged;


    // VARIABLES ---------------------------------------------------------->>
    private int currentReputation;
    private int minimumReputation = 0;
    private int maxReputation = 100;


    // METHODS ---------------------------------------------------------->>

    public void AddReputation(int amount) 
    {
        if (currentReputation + amount > 100)
        {
            if (currentReputation < 100)
            {
                OnReputationChanged?.Invoke(currentReputation);
            }

            currentReputation = 100;
            return;
        }

        OnReputationChanged?.Invoke(currentReputation);
        currentReputation += amount;
    
    }
    public void RemoveReputation(int amount) 
    {

        if (currentReputation - amount < 0)
        {
            if (currentReputation > 0)
            {
                OnReputationChanged?.Invoke(currentReputation);
            }


            currentReputation = 0;
            return;
        }

        OnReputationChanged?.Invoke(currentReputation);
        currentReputation -= amount;

    }
    public void SetReputation(int amount) 
    {
        if (amount > 100 || amount is < 0)
        {
            return;
        }

        OnReputationChanged?.Invoke(currentReputation);
        currentReputation = amount;
    
    }
    public float GetReputationMultiplier() 
    { 
        if (GetReputationTier() == ReputationTier.Unknown)
        {
            return 1.0f;
        } 
        else if (GetReputationTier() == ReputationTier.Recognized)
        {
            return 1.1f;
        } 
        else if (GetReputationTier() == ReputationTier.Established)
        {
            return 1.2f;
        } 
        else if (GetReputationTier() == ReputationTier.WellKnown)
        {
            return 1.3f;
        } 
        else if (GetReputationTier() == ReputationTier.Infamous)
        {
            return 1.5f;
        }

        return 1.0f; 

    }
    public ReputationTier GetReputationTier() 
    { 
        if (currentReputation >= 0 && currentReputation <= 19)
        {
            return ReputationTier.Unknown;
        } 
        else if (currentReputation >= 20 && currentReputation <= 39)
        {
            return ReputationTier.Recognized;
        }
        else if (currentReputation >= 40 && currentReputation <= 59)
        {
            return ReputationTier.Established;
        }
        else if (currentReputation >= 60 && currentReputation <= 79)
        {
            return ReputationTier.WellKnown;
        }
        else if (currentReputation >= 80 && currentReputation <= 100)
        {
            return ReputationTier.Infamous;
        }


        return ReputationTier.Unknown; 
    }




    // Unity Methods --->
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

}
