using StarterAssets;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectiveManager : MonoBehaviour
{
    [SerializeField] private InputActionReference openObjectives;
    public bool canOpenObjectives = true;

    [Header("UI")]
    [SerializeField] private GameObject objectivePanel;

    [Header("Input")]//player inputs
    public ThirdPersonController thirdPersonController;


    void Update()
    {
        if (canOpenObjectives && openObjectives.action.WasPressedThisFrame())
        {
            ToggleObjectivePanel();
        }
    }

    public void ToggleObjectivePanel()
    {
        if (objectivePanel != null)
        {
            if (!objectivePanel.activeSelf)
            {
                objectivePanel.SetActive(true);

                //change to Mouse Map
                thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Mouse");
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                return;
            }

            objectivePanel.SetActive(false);

            //change to Player Map
            thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            return;
        }

        Debug.Log("No objective panel set!!!");

    }
}
