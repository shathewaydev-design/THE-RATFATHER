using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using StarterAssets;
public enum CookingStepType
{
    Milk,
    Flavor,
    Heat,
    Additive
}

[System.Serializable]
public class CookingStepUI
{
    public CookingStepType stepType;
    public GameObject root;     // container object
    public Animator animator;   // for punch/glow/etc
}
public class CookingManager : MonoBehaviour, IInteractable
{
    public static CookingManager Instance;

    [Header("Camera")]
    public CinemachineCamera playerCamera;
    public GameObject mainCamera;
    public Transform cookingPot;//view cooking pot
    [SerializeField] private GameObject cookingPotGeo;// cooking pot
    private Vector3 savedMainCameraPosition;
    private Quaternion savedMainCameraRotation;

    [Header("Player")]
    public GameObject player;//playerCameraRoot
    //public GameObject playerGeo;//player Geo
    public GameObject playerFollowCamera;//cinemachine

    // [Header("UI Steps")]
    public GameObject recipeMenu;
    public GameObject inventoryPanel;
    // public GameObject stepPourMilk;
    // public GameObject stepFlavor;
    // public GameObject stepHeat;
    // public GameObject stepAdditive;
    [Header("UI Steps")]
    public List<CookingStepUI> steps;
    private int currentStepIndex = -1;

    [Header("Input")]
    // [SerializeField] private float holdTime = 1.0f;
    // [SerializeField] private float currentHoldTime = 0f;
    public ThirdPersonController thirdPersonController;
    //private bool playerInRange = false;

    [Header("UI")]
    public Animator promptAnimator;

    public Image holdProgressBar;
    
    private void Awake()//this is necessary to avoid bugs
    {
        Instance = this;
       
       
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cookingPotGeo.SetActive(true);
        thirdPersonController = ThirdPersonController.Instance;
        thirdPersonController.OnStopInteract += ExitCookingMode;
    }

    // Update is called once per frame
    void Update()
    {
        if(thirdPersonController.stopInteract.WasPressedThisFrame())
        {
            ExitCookingMode();
        }

        
    }

    public void Interact()//runs when player press E to interact
    {
        EnterCookingMode();
        ShowRecipeMenu();
    }
    public void EnterCookingMode()
    {
        promptAnimator.SetBool("UIappeared", false);
        promptAnimator.SetTrigger("UIdisappearing");

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Cooking");
        
        //Save current camera data
        savedMainCameraPosition = mainCamera.transform.position;
        savedMainCameraRotation = mainCamera.transform.rotation;
        // Camera zoom to pot
        //playerCamera.Lens.FieldOfView = 19f;
        playerCamera.Follow = cookingPot;
        mainCamera.transform.position = new Vector3(3.747427f, 2.5f, -5.494404f);
        mainCamera.transform.rotation = Quaternion.Euler(14.08f, -0.086f, -0.025f);
        
        // Disable player control
        //playerGeo.SetActive(false);
        playerFollowCamera.SetActive(false);
    } 

    public void ExitCookingMode()
    {
        thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");

        //playerGeo.SetActive(true);
        playerFollowCamera.SetActive(true);
        // Restore camera
        //playerCamera.Lens.FieldOfView = 40f;
        playerCamera.Follow = player.transform;
        mainCamera.transform.position = savedMainCameraPosition;
        mainCamera.transform.rotation = savedMainCameraRotation;
        
        ResetSteps();
        // recipeMenu.SetActive(false);
        // //reset milk step
        // stepPourMilk.SetActive(false);
        // //reset flavor step
        // stepFlavor.SetActive(false);
        // //reset heat step
        // stepHeat.SetActive(false);
        // //reset additive step
        // stepAdditive.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // ---------- FLOW ----------
    private void SetStep(int newIndex)
    {
        if (newIndex < 0 || newIndex >= steps.Count) return;

        int previousIndex = currentStepIndex;
        currentStepIndex = newIndex;

        for (int i = 0; i < steps.Count; i++)
        {
            var step = steps[i];

            if (i < currentStepIndex)
            {
                // Completed
                step.root.SetActive(true);
                step.animator.SetTrigger("Completed");
            }
            else if (i == currentStepIndex)
            {
                // Active
                step.root.SetActive(true);
                step.animator.SetTrigger("Activate");
            }
            else
            {
                // Upcoming
                step.root.SetActive(true);
                step.animator.SetTrigger("Idle");
            }
        }

        // Optional: animate previous step losing focus
        if (previousIndex >= 0 && previousIndex < steps.Count)
        {
            steps[previousIndex].animator.SetTrigger("Deactivate");
        }
    }
    public void ShowRecipeMenu()
    {
        recipeMenu.SetActive(true);
        inventoryPanel.SetActive(true);
    }

    public void StartPourMilk()
    {
        inventoryPanel.SetActive(false);
        //stepPourMilk.SetActive(true);
        SetStep(0);
    }

    public void StartFlavor()
    {
        // stepPourMilk.SetActive(false);
        // stepFlavor.SetActive(true);
        SetStep(1);
        thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Mouse");
    }

    public void StartHeat()
    {
        //stepFlavor.SetActive(false);
        //stepHeat.SetActive(true);
        SetStep(2);
    }

    public void StartAdditive()
    {
        //stepHeat.SetActive(false);
        //stepAdditive.SetActive(true);
        SetStep(3);
        thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Cooking");
    }

    public void FinishCooking()
    {
        //stepAdditive.SetActive(false);
        thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        ExitCookingMode();
    }

/////UI ONLY////
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //playerInRange = true;
            if(promptAnimator != null)
            {
                promptAnimator.SetBool("UIappeared", true);
                promptAnimator.SetTrigger("UIappearing");
                
            }


            //Debug.Log("Hold E to extract sample"); // Replace with UI later
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //playerInRange = false;
            //currentHoldTime = 0f;
            if(promptAnimator != null)
            {
                promptAnimator.SetBool("UIappeared", false);
                promptAnimator.SetTrigger("UIdisappearing");
            }
        }
    }
    private void ResetSteps()
    {
        currentStepIndex = -1;

        foreach (var step in steps)
        {
            step.root.SetActive(false);
        }
    }
    private void OnDestroy()
    {
        if (thirdPersonController == null) return;

        thirdPersonController.OnStopInteract -= ExitCookingMode;
    }
}
