using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FinishCookingVFX : MonoBehaviour
{
    [SerializeField] private GameObject cookingPotVFX;    
    private void FinishCooking()
    {
        Animator animator = cookingPotVFX.GetComponent<Animator>();
        animator.SetTrigger("FinishVFX");
        CookingManager.Instance.StartAdditive();

    }    



    

}