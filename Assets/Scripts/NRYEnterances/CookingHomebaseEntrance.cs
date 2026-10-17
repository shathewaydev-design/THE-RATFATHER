using StarterAssets;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CookingHomebaseEntrance : MonoBehaviour
{
    public static event Action<Transform> OnEnteredCookingHomeBase;
    public static event Action<Transform> OnExitedCookingHomeBase;

    [SerializeField] Transform cookingHomebaseEntrance;
    [SerializeField] Transform cookingHomebaseExit;

    [SerializeField] private bool isEnterance;
    [SerializeField] private bool isExit;

    [SerializeField] private GameObject entranceCheck;
    [SerializeField] private TextMeshProUGUI enterPrompt;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    public ThirdPersonController thirdPersonController;


    public void OnYesButtonEntrance()
    {
        OnEnteredCookingHomeBase?.Invoke(cookingHomebaseExit);

        Debug.Log("Attempting teleport....");

        entranceCheck.SetActive(false);

        thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        yesButton.onClick.RemoveAllListeners();

    }

    public void OnYesButtonExit()
    {
        OnExitedCookingHomeBase?.Invoke(cookingHomebaseEntrance);

        entranceCheck.SetActive(false);

        thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        yesButton.onClick.RemoveAllListeners();
    }

    public void OnNoButtonEntrance()
    {

        OnExitedCookingHomeBase?.Invoke(cookingHomebaseEntrance);

        entranceCheck.SetActive(false);

        thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        noButton.onClick.RemoveAllListeners();

    }

    public void OnNoButtonExit()
    {

        OnEnteredCookingHomeBase?.Invoke(cookingHomebaseExit);

        entranceCheck.SetActive(false);

        thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        noButton.onClick.RemoveAllListeners();

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            entranceCheck.SetActive(true);

            thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Mouse");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;


            if (isEnterance)
            {

                //Debug.Log("Detected player at enterance!");
                enterPrompt.text = "Enter the cooking homebase?";
                yesButton.onClick.AddListener(OnYesButtonEntrance);
                noButton.onClick.AddListener(OnNoButtonEntrance);
                //OnEnteredCookingHomebase?.Invoke(cookingHomebaseExit);
            }
            else if (isExit) 
            {
                enterPrompt.text = "Exit the cooking homebase?";
                //OnExitedCookingHomebase?.Invoke(cookingHomebaseEntrance);
                yesButton.onClick.AddListener(OnYesButtonExit);
                noButton.onClick.AddListener(OnNoButtonExit);
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
