using StarterAssets;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PhoneBoothUIController : MonoBehaviour
{

    //PhoneBoothUIController
    public static PhoneBoothUIController Instance;

    // EVENTS ----------------------------------------------------->


    // VARIABLES ----------------------------------------------------->

    // Lists to populate each section >>> NOW TAKEN FROM OTHER MANAGERS
    //private List<ObjectiveData> objList;
    //private List<RequestData> reqList;
    //private List<CustomerData> custList;

    // Data to populate RepPrefab when Obj button is pressed
    //private ReputationState repstate;

    // References to each UI/Canvas section objects that needs to be altered
    [Header("Main refs")]
    [SerializeField] private GameObject phoneBoothUIPanel;
    [SerializeField] private Transform contentPanel;

    // PREFABS
    [Header("Prefab Templates")]
    [SerializeField] private GameObject repPrefab;
    [SerializeField] private GameObject objPrefab;
    [SerializeField] private GameObject reqPrefab;
    [SerializeField] private GameObject custPrefab;

    // INSTANTIATED OBJECTS
    private List<GameObject> objObjects;
    private List<GameObject> reqObjects;
    private List<GameObject> custObjects;

    // METHODS ----------------------------------------------------->

    // methods will most likely be triggered via events to help with decoupling
    private void EnterPhoneBooth() 
    {  
        phoneBoothUIPanel.SetActive(true);

        // SWITCH INPUT MAP
        ThirdPersonController.Instance.GetComponent<PlayerInput>().SwitchCurrentActionMap("Mouse");
        CursorManager.Instance.LockCursor();

        PopulateObjectives(); // objectives always pops up first

    }


    // methods SPECIFICALLY for UI buttons
    public void OnObjectivesButton() 
    {
        // clear everything, then populate objectives


    
    }
    public void OnRequestsButton() 
    {
        // clear everything, then populate requests
        PopulateRequests();
    }
    public void OnCustomersButton() 
    {
        // clear everything, then populate requests
        PopulateCustomers();

    }
    public void OnExitButton() 
    {
        ExitPhoneBooth();
    }

    // methods to populate each section. REMEBER TO SET THE Z TO 0, FOR SOME REASON IT ISN'T WHEN FIRST INSTANTIATED
    private void PopulateObjectives() // NEEDS TO ADD REP TO THE BEGINING OF LIST >> ALSO NEED NEW OBJ MANAGER 
    {
        // in a loop, instantiate an objective prefab, then call it's
        // methods to fill in the information.
        ClearCustomers();
        ClearObjectives();
        ClearRequests();

        //foreach ()
        //{

        //}



    } 
    private void PopulateRequests() 
    {

        ClearCustomers();
        ClearObjectives();
        ClearRequests();



        foreach (RequestData req in RequestManager.Instance.ActiveRequests)
        {
            GameObject newReq = GameObject.Instantiate(reqPrefab, contentPanel);
            
            // fill in the prefab!
            FillRequestPrefab(req, newReq);

            // add the game object to a list to be able to clear on 
            reqObjects.Add(newReq);


            //newReq.GetComponent<RequestUI>().SetName(req.customer.npcProfile.name);
            //newReq.reqName.text = req.customer.npcProfile.name;
        }


    }
    private void PopulateCustomers() 
    {
        ClearCustomers();
        ClearObjectives();
        ClearRequests();

        foreach (CustomerData cust in RequestManager.Instance.Customers)
        {

            GameObject newCustomer = GameObject.Instantiate(custPrefab, contentPanel);

            FillCustomerPrefab(cust, newCustomer);

            custObjects.Add(newCustomer);


        }


    }

    // methods to update prefabs
    //private void FillReputationPrefab(ReputationState repState) { }
    private void FillObjectivePrefab(ObjectiveData obj) { }
    private void FillRequestPrefab(RequestData req, GameObject newReq) 
    {

        newReq.GetComponent<RequestUI>().SetName(req.customer.npcProfile.name);

        if (req.requestType == CheeseType.Upper)
        {
            newReq.GetComponent<RequestUI>().SetType("Upper");
        } 
        else if (req.requestType == CheeseType.Downer) 
        {
            newReq.GetComponent<RequestUI>().SetType("Downer");
        }

        newReq.GetComponent<RequestUI>().SetQuantity("" + req.requestQuantity);

        newReq.GetComponent<RequestUI>().SetQuality("Low (placeholder)"); // will be an if statement later!!!

        newReq.GetComponent<RequestUI>().SetPayNum("" + req.estimatedPay);

    }

    private void FillCustomerPrefab(CustomerData cust, GameObject newCustomer) 
    { 

        newCustomer.GetComponent<CustomerUI>().SetName(cust.npcProfile.name);

        if (cust.isActiveCustomer)
        {
            newCustomer.GetComponent<CustomerUI>().SetStatus("ACTIVE");
        } 
        else
        {
            newCustomer.GetComponent<CustomerUI>().SetStatus("LOST");
        }

        string custTypePref = "";
        string custQualityPref = "Low+ (PLACEHOLDER)";

        if (cust.npcProfile.typePreference == CheeseType.Upper)
        {
            custTypePref = "Uppers";
        } 
        else if (cust.npcProfile.typePreference == CheeseType.Downer)
        {
            custTypePref = "Downers";
        }

        newCustomer.GetComponent<CustomerUI>().SetPreference(custTypePref, custQualityPref);

        newCustomer.GetComponent<CustomerUI>().SetStrikes("" + cust.strikeCount);


    }

    // methods to update ui lists >>> MAY NOT BE NEEDED ANYMORE!!!
    //public void AddObjective() { } // NEEDS OBJ DATA!
    //public void AddRequest() { } // NEEDS REQ DATA!
    //public void AddCustomer() { } // NEEDS CUSTOMER DATA!
    //public void RemoveObjective() { } // NEEDS OBJ DATA!
    //public void RemoveRequest() { } // NEEDS REQ DATA!
    //public void RemoveCustomer() { } // NEEDS CUSTOMER DATA!


    // methods to clear ui
    private void ClearObjectives() 
    {
        foreach (GameObject obj in objObjects)
        {
            Destroy(obj);
        }

        objObjects.Clear();

    }
    private void ClearRequests() 
    {  
        foreach (GameObject req in reqObjects)
        {
            Destroy(req);
        }

        reqObjects.Clear();

    }    
    private void ClearCustomers() 
    {
        foreach (GameObject cust in custObjects)
        {
            Destroy(cust);
        }

        custObjects.Clear();


    }

    private void ExitPhoneBooth()
    {
        ClearCustomers();
        ClearObjectives();
        ClearRequests();
        phoneBoothUIPanel.SetActive(false);

        // SWITCH INPUT MAP
        ThirdPersonController.Instance.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        CursorManager.Instance.UnlockCursor();
    }





    // Unity Methods ----->
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    private void OnEnable()
    {
        PhoneBoothInteractionScript.OnPhoneBoothInteract += EnterPhoneBooth;
        //PhoneBoothInteractionScript.OnPhoneBoothInteract += PopulateRequests;
        //PhoneBoothInteractionScript.OnPhoneBoothInteract += PopulateCustomers;
    }

    private void OnDisable()
    {
        PhoneBoothInteractionScript.OnPhoneBoothInteract -= EnterPhoneBooth;
        //PhoneBoothInteractionScript.OnPhoneBoothInteract -= PopulateRequests;
        //PhoneBoothInteractionScript.OnPhoneBoothInteract -= PopulateCustomers;
    }



}
