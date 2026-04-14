using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using StarterAssets;

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
    public GameObject inventoryPanel;
    public GameObject stepPourMilk;
    public GameObject stepFlavor;
    public GameObject stepHeat;
    public GameObject stepAdditive;

    [Header("Input")]
    // [SerializeField] private float holdTime = 1.0f;
    // [SerializeField] private float currentHoldTime = 0f;
    public ThirdPersonController thirdPersonController;
    private bool playerInRange = false;

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
        thirdPersonController = ThirdPersonController.Instance;
        thirdPersonController.OnStopInteract += ExitCookingMode;
    }

    // Update is called once per frame
    void Update()
    {
        

        
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
        inventoryPanel.SetActive(false);
        stepPourMilk.SetActive(true);
    }

    public void StartFlavor()
    {
        stepPourMilk.SetActive(false);
        stepFlavor.SetActive(true);
        thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Mouse");
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
        if (thirdPersonController == null) return;

        thirdPersonController.OnStopInteract -= ExitCookingMode;
    }
}
