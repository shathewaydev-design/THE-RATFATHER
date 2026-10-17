using StarterAssets;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class StripClubEntrance : MonoBehaviour
{
    public static event Action<Transform> OnEnteredStripClub;
    public static event Action<Transform> OnExitedStripClub;

    [SerializeField] Transform stripClubEntrance;
    [SerializeField] Transform stripClubExit;

    [SerializeField] private bool isEnterance;
    [SerializeField] private bool isExit;

    [SerializeField] private GameObject entranceCheck;
    [SerializeField] private TextMeshProUGUI enterPrompt;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    public ThirdPersonController thirdPersonController;

    public bool hasEnteredBefore = false;


    public static event Action OnSCCutsceneTriggered;


    public void OnYesButtonEntrance()
    {

        if (!hasEnteredBefore)
        {

            StubbedCutsceneManager.Instance.OpenCutscene(0);

            OnSCCutsceneTriggered?.Invoke();

            entranceCheck.SetActive(false);

            hasEnteredBefore = true;
            NRYLevelManager.Instance.hasEnteredClubOnce = true;

            return;


        }

        OnEnteredStripClub?.Invoke(stripClubExit);

        entranceCheck.SetActive(false);

        thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        yesButton.onClick.RemoveAllListeners();

    }

    public void OnYesButtonExit()
    {
        OnExitedStripClub?.Invoke(stripClubEntrance);

        entranceCheck.SetActive(false);

        thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        yesButton.onClick.RemoveAllListeners();
    }

    public void OnNoButtonEntrance()
    {

        OnEnteredStripClub?.Invoke(stripClubEntrance);

        entranceCheck.SetActive(false);

        thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        noButton.onClick.RemoveAllListeners();


    }

    public void OnNoButtonExit()
    {

        OnExitedStripClub?.Invoke(stripClubExit);

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
                enterPrompt.text = "Enter the strip club?";
                yesButton.onClick.AddListener(OnYesButtonEntrance);
                noButton.onClick.AddListener(OnNoButtonEntrance);
                //OnEnteredCasino?.Invoke(casinoExit);
            }
            else if (isExit)
            {
                enterPrompt.text = "Exit the strip club?";
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
