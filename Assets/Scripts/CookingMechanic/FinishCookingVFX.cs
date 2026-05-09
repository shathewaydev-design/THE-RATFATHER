using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FinishCookingVFX : MonoBehaviour
{
    [SerializeField] private GameObject cookingPotVFX;    
    private void FinishCooking()
    {
        //CookingManager.Instance.StartAddictive();
        //CookingManager.Instance.FinishCooking();
        CookingManager.Instance.CraftCheese();

        Debug.Log("Finish cooking!");
    }    



    

}