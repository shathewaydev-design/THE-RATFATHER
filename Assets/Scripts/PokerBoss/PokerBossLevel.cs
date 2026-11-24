using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
public class PokerBossLevel : MonoBehaviour
{
    //This is a level manager
    //swap player camera w boss fight camer
    //->disable player movement
    //->start actual fight
    //when fight is completed
    //save cheese and inventory
    //reenable player movement
    //->send player back to NRY
    [Header("Movement Actions")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private InputActionReference sprintAction;
    [Header("Level Settings")]
    [SerializeField] private bool enterLevel= true;
    [SerializeField] private bool leaveLevel= false;
    [SerializeField] private bool bossFightCompleted = false;
    [SerializeField] private string gameSceneName = "NRY - Update";
    [SerializeField] private PokerBossManager pokerBossManagerScript;
    [Header("Camera")]
    [SerializeField] private CinemachineCamera playerCamera;
    [SerializeField] private CinemachineCamera bossfightCamera;

    private void Start()
    {
        if(enterLevel)
        {
            DisablePlayerMovement();
            SwapCamera();
            pokerBossManagerScript.StartEncounter();
            //TODO: tell PokerBossManager to start
        }
    }

    private void SwapCamera()
    {
        playerCamera.Priority = 0;
        bossfightCamera.Priority = 10;
    }
    private void DisablePlayerMovement()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
        sprintAction.action.Disable();
    }

    private void EnablePlayerMovement()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
        sprintAction.action.Enable();
    }

}