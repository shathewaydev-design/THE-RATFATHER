using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class CookingManager : MonoBehaviour, IInteractable
{
    public static CookingManager Instance;

    [Header("Camera")]
    public CinemachineCamera playerCamera;
    public GameObject mainCamera;
    public Transform cookingPot;//view cooking pot
    private Vector3 savedMainCameraPosition;
    private Quaternion savedMainCameraRotation;

    [Header("Player")]
    public GameObject player;//playerCameraRoot
    //public GameObject playerGeo;//player Geo
    public GameObject playerFollowCamera;//cinemachine

    [Header("UI Steps")]
    public GameObject recipeMenu;
    public GameObject stepPourMilk;
    public GameObject stepFlavor;
    public GameObject stepHeat;
    public GameObject stepAdditive;

    [Header("Input")]
    // [SerializeField] private float holdTime = 1.0f;
    // [SerializeField] private float currentHoldTime = 0f;
    public ThirdPersonController thirdPersonController;
    private bool playerInRange = false;
    //public InputActionAsset InputActions;//import all function from the input system
    // public PlayerInput playerInput;
    // private InputActionMap playerMap;
    // private InputActionMap cookingMap;

    // private InputAction stopInteract;//press Q
    // public InputAction tiltLeft;//press A
    // public InputAction tiltRight;//press D

    [Header("UI")]
    public Animator promptAnimator;

    public Image holdProgressBar;
    //[Header("Movement")]
    

    // private void OnEnable()//this is necessary to avoid bugs
    // {
    //     InputActions.FindActionMap("Player").Enable();
    //     InputActions.FindActionMap("Cooking").Enable();

    // }
    // private void OnDisable()//this is necessary to avoid bugs
    // {
    //     InputActions.FindActionMap("Player").Disable();
    //     InputActions.FindActionMap("Cooking").Disable();

    // }
    private void Awake()//this is necessary to avoid bugs
    {
        Instance = this;
        
    //     if (playerInput == null)
    //     playerInput = FindFirstObjectByType<PlayerInput>();
    //     // Grab actions from PlayerInput's asset
    // var cookingMap = playerInput.actions.FindActionMap("Cooking");

    //     tiltLeft = cookingMap.FindAction("TiltLeft");
    //     tiltRight = cookingMap.FindAction("TiltRight");
    //     stopInteract = cookingMap.FindAction("StopInteract");
        
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        thirdPersonController = ThirdPersonController.Instance;

        thirdPersonController.OnStopInteract += ExitCookingMode;
        thirdPersonController.OnTiltLeft += HandleTiltLeft;
        thirdPersonController.OnTiltRight += HandleTiltRight;
    }

    // Update is called once per frame
    void Update()
    {
        // if (playerInput.currentActionMap.name == "Cooking")
        // {
        //     if(ThirdPersonController.Instance.stopInteract.WasPressedThisFrame())
        //     {
        //         ExitCookingMode();
        //     }
        // }

        
    }

    public void Interact()//runs when player press E to interact
    {
        EnterCookingMode();
        ShowRecipeMenu();
    }
    public void EnterCookingMode()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Cooking");
        //ThirdPersonController.Instance.playerInput.SwitchCurrentActionMap("Cooking");
        
        //Save current camera data
        savedMainCameraPosition = mainCamera.transform.position;
        savedMainCameraRotation = mainCamera.transform.rotation;
        // Camera zoom to pot
        playerCamera.Follow = cookingPot;
        mainCamera.transform.position = new Vector3(3.747427f, 2.5f, -5.494404f);
        mainCamera.transform.rotation = Quaternion.Euler(14.08f, -0.086f, -0.025f);


        // Disable player control
        //playerGeo.SetActive(false);
        playerFollowCamera.SetActive(false);
    } 

    public void ExitCookingMode()
    {
        
        
        //ThirdPersonController.Instance.playerInput.SwitchCurrentActionMap("Player");
        thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");

        //playerGeo.SetActive(true);
        playerFollowCamera.SetActive(true);
        // Restore camera
        playerCamera.Follow = player.transform;
        mainCamera.transform.position = savedMainCameraPosition;
        mainCamera.transform.rotation = savedMainCameraRotation;


        recipeMenu.SetActive(false);
        stepPourMilk.SetActive(false);
        stepFlavor.SetActive(false);
        stepHeat.SetActive(false);
        stepAdditive.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // ---------- FLOW ----------

    public void ShowRecipeMenu()
    {
        recipeMenu.SetActive(true);
    }

    public void StartPourMilk()
    {
        recipeMenu.SetActive(false);
        stepPourMilk.SetActive(true);
    }

    public void StartFlavor()
    {
        stepPourMilk.SetActive(false);
        stepFlavor.SetActive(true);
    }

    public void StartHeat()
    {
        stepFlavor.SetActive(false);
        stepHeat.SetActive(true);
    }

    public void StartAdditive()
    {
        stepHeat.SetActive(false);
        stepAdditive.SetActive(true);
    }

    public void FinishCooking()
    {
        stepAdditive.SetActive(false);
        ExitCookingMode();
    }

/////UI ONLY////
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("playerInRange is "+playerInRange);
            if(promptAnimator != null)
            {
                promptAnimator.SetBool("UIappeared", true);
                promptAnimator.SetTrigger("UIappearing");
            }


            Debug.Log("Hold E to extract sample"); // Replace with UI later
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("playerInRange is "+playerInRange);
            //currentHoldTime = 0f;
            if(promptAnimator != null)
            {
                promptAnimator.SetBool("UIappeared", false);
                promptAnimator.SetTrigger("UIdisappearing");
            }
        }
    }

    private void OnDestroy()
    {
        if (controller == null) return;

        controller.OnStopInteract -= ExitCookingMode;
        controller.OnTiltLeft -= HandleTiltLeft;
        controller.OnTiltRight -= HandleTiltRight;
    }
}
