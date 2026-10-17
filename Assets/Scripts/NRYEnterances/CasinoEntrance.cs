using StarterAssets;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CasinoEntrance : MonoBehaviour
{
    public static event Action<Transform> OnEnteredCasino;
    public static event Action<Transform> OnExitedCasino;

    [SerializeField] Transform casinoEntrance;
    [SerializeField] Transform casinoExit;

    [SerializeField] private bool isEnterance;
    [SerializeField] private bool isExit;

    [SerializeField] private GameObject entranceCheck;
    [SerializeField] private TextMeshProUGUI enterPrompt;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    public ThirdPersonController thirdPersonController;



    public void OnYesButtonEntrance()
    {
        OnEnteredCasino?.Invoke(casinoExit);

        entranceCheck.SetActive(false);

        thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;


        if (NRYLevelManager.Instance.hasEnteredClubOnce && InventorySystem.Instance.GetCurrency() >= 500)
        {

            StubbedCutsceneManager.Instance.OpenCutscene(1);

        }

        yesButton.onClick.RemoveAllListeners();


    }

    public void OnYesButtonExit()
    {
        OnExitedCasino?.Invoke(casinoEntrance);

        entranceCheck.SetActive(false);

        thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        yesButton.onClick.RemoveAllListeners();
    }

    public void OnNoButtonEntrance()
    {

        OnEnteredCasino?.Invoke(casinoEntrance);

        entranceCheck.SetActive(false);

        thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        noButton.onClick.RemoveAllListeners();
    }

    public void OnNoButtonExit()
    {

        OnEnteredCasino?.Invoke(casinoExit);

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
                enterPrompt.text = "Enter the casino?";
                yesButton.onClick.AddListener(OnYesButtonEntrance);




                noButton.onClick.AddListener(OnNoButtonEntrance);
                //OnEnteredCasino?.Invoke(casinoExit);
            }
            else if (isExit) 
            {
                enterPrompt.text = "Exit the casino?";
                //OnExitedCasino?.Invoke(casinoEntrance);
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
