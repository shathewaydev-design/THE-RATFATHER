using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;

public class BossLvl1Cutscene : MonoBehaviour
{
    [SerializeField] private GameObject cutsceneCamera;//player follow camera    
    [SerializeField] private GameObject[] playerObjects;  
    [SerializeField] private SoldatoScript soldatoScript;  

    //public ThirdPersonController thirdPersonController;
    public bool bossCutsceneFinished = false;

    void Start()
    {
        cutsceneCamera.SetActive(true);
        playerObjects[0].SetActive(false);
        playerObjects[1].SetActive(false);
        //playerObjects[2].SetActive(false);
        soldatoScript.enabled = false;
    }
    private void CloseBossCutscene()
    {
        cutsceneCamera.SetActive(false);

        playerObjects[0].SetActive(true);
        playerObjects[1].SetActive(true);
        //playerObjects[2].SetActive(true);
        bossCutsceneFinished = true;
        soldatoScript.enabled = true;
    }    



    

}