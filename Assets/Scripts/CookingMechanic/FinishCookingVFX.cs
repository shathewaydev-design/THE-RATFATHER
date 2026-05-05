using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FinishCookingVFX : MonoBehaviour
{
    [SerializeField] private GameObject cookingPotVFX;    
    private void FinishCooking()
    {
        CookingManager.Instance.StartAdditive();
        Debug.Log("Finish cooking, start additive!");
    }    



    

}