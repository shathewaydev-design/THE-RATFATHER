using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    // ReputationPrefab refs
    [SerializeField] private Scrollbar repBar;
    [SerializeField] private TextMeshProUGUI repTitle;
    [SerializeField] private TextMeshProUGUI repMultiplier;

    // ObjectivePrefab refs
    [SerializeField] private TextMeshProUGUI objType;
    [SerializeField] private TextMeshProUGUI objTitle;
    [SerializeField] private TextMeshProUGUI objStatus;
    [SerializeField] private TextMeshProUGUI objDesc;

    // RequestPrefab refs
    [SerializeField] private TextMeshProUGUI reqName;
    [SerializeField] private TextMeshProUGUI reqType;
    [SerializeField] private TextMeshProUGUI reqQuantity;
    [SerializeField] private TextMeshProUGUI reqQuality;
    [SerializeField] private TextMeshProUGUI estPayNum;

    // Customer refs
    [SerializeField] private TextMeshProUGUI custName;
    [SerializeField] private TextMeshProUGUI custStatus;
    [SerializeField] private TextMeshProUGUI custPref;
    [SerializeField] private TextMeshProUGUI failedReqsNum;

    // PREFABS
    [SerializeField] private GameObject repPrefab;
    [SerializeField] private GameObject objPrefab;
    [SerializeField] private GameObject reqPrefab;
    [SerializeField] private GameObject custPrefab;


    // METHODS ----------------------------------------------------->

    // methods will most likely be triggered via events to help with decoupling

    // methods SPECIFICALLY for UI buttons
    public void OnObjectivesButton() { }
    public void OnRequestsButton() { }
    public void OnCustomersButton() { }

    // methods to populate each section
    private void PopulateObjectives() { } // NEEDS TO ADD REP TO THE BEGINING OF LIST
    private void PopulateRequests() { }
    private void PopulateCustomers() { }

    // methods to update prefabs
    //private void FillReputationPrefab(ReputationState repState) { }
    private void FillObjectivePrefab(ObjectiveData obj) { }
    private void FillRequestPrefab(RequestData req) { }
    private void FillCustomerPrefab(CustomerData cust) { }

    // methods to update ui lists >>> MAY NOT BE NEEDED ANYMORE!!!
    //public void AddObjective() { } // NEEDS OBJ DATA!
    //public void AddRequest() { } // NEEDS REQ DATA!
    //public void AddCustomer() { } // NEEDS CUSTOMER DATA!
    //public void RemoveObjective() { } // NEEDS OBJ DATA!
    //public void RemoveRequest() { } // NEEDS REQ DATA!
    //public void RemoveCustomer() { } // NEEDS CUSTOMER DATA!


    // methods to clear ui
    private void ClearObjectives() { }
    private void ClearRequests() { }    
    private void ClearCustomers() { }





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
        
    }

    private void OnDisable()
    {
        
    }



}
