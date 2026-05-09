using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossLvl1Cutscene : MonoBehaviour
{
    [SerializeField] private GameObject cutsceneCamera;    
    [SerializeField] private GameObject[] playerObjects;    

    void Start()
    {
        cutsceneCamera.SetActive(true);
        playerObjects[0].SetActive(false);
        playerObjects[1].SetActive(false);
        playerObjects[2].SetActive(false);
    }
    private void CloseBossCutscene()
    {
        cutsceneCamera.SetActive(false);

        playerObjects[0].SetActive(true);
        playerObjects[1].SetActive(true);
        playerObjects[2].SetActive(true);
    }    



    

}