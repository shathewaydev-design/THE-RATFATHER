using System.Collections.Generic;
using UnityEngine;

public class RequestManager : MonoBehaviour
{
    // EVENTS ---------------------------------------------------->>


    // VARIABLES ---------------------------------------------------->>
    public static RequestManager Instance;
    private List<CustomerData> customers = new List<CustomerData>();
    private List<RequestData> activeRequests = new List<RequestData>();
    [SerializeField] private float requestInterval;
    [SerializeField] private float requestTimer;

    // read only lists for protection
    public IReadOnlyList<CustomerData> Customers => customers;
    public IReadOnlyList<RequestData> ActiveRequests => activeRequests;




    // METHODS ---------------------------------------------------->>

    public void RegisterCustomer(NPCProfile npc) // called after a succesful free sample
    {
        CustomerData customer = new CustomerData();

        customer.npcProfile = npc;
        customer.strikeCount = 0;
        customer.isActiveCustomer = true;

        customers.Add(customer);
    
    }

    private void GenerateRequest(CustomerData customer) 
    { 
        // check if request can be genrated here?
        if (HasActiveRequest(customer))
            return;

        // also check timer, but for now test previous perameter
        
        // otherwise, add a new request!
        RequestData newRequest = new RequestData();
        newRequest.customer = customer;
        newRequest.requestQuantity = Random.Range(1, 6); // random number between 1 and 5
        newRequest.requestType = customer.npcProfile.typePreference;
        newRequest.requestQuality = customer.npcProfile.qualityPreference;

        // calculate the estimated pay!
        float finalPriceEstimate = customer.npcProfile.basePrice * ReputationManager.Instance.GetReputationMultiplier();
        newRequest.estimatedPay = (int)finalPriceEstimate;

        // add it to the active requests list!
        activeRequests.Add(newRequest);


    }

    private void CheckForNewRequests() // checks whether or not customer should generate a request MAY NOT BE NEEDED
    {
        //foreach (CustomerData customer in customers)
        //{


        //}

    } 


    public bool HasActiveRequest(NPCProfile npc)
    {
        foreach(CustomerData customer in customers)
        {

            if (customer.npcProfile == npc)
            {
                return HasActiveRequest(customer);
            }

        }

        return false;
    }

    private bool HasActiveRequest(CustomerData customer) // determines whether a customer already has an active request
    { 
        foreach (RequestData req in activeRequests)
        {
            if (req.customer == customer)
            {
                return true;
            }

        }

        return false;

    }

    public void CompleteRequest(RequestData request) // evaluates an attempted request completion >> CAN BE TRIGGERED VIA AN EVENT
    {
        int points = 3;

        // via loops, check what's listed below. if even one cheese is wrong, dock a point. 
        // the amount of points left will determine the result, which will also effect the  
        // possible change in price and reputation

        // first check type 

        // second check quantity

        // last check quality


        // then based on score, give out proper consequence!
    } 


    public void RemoveRequest(RequestData request) 
    {
        activeRequests.Remove(request);
    }
    public void RemoveCustomer(CustomerData customer) 
    { 
        customers.Remove(customer);
    }
    public int GetActiveRequestCount() 
    { 
        return ActiveRequests.Count; 
    }





    // Unity Methods --->
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start() // for testing for now!!!
    {
        RequestData req1 = new RequestData();
        CustomerData customer1 = new CustomerData();

        customer1.npcProfile = new NPCProfile();
        customer1.npcProfile.basePrice = 1;
        customer1.npcProfile.npcName = "Judy";
        customer1.npcProfile.typePreference = CheeseType.Upper;
        customer1.npcProfile.qualityPreference = Quality.Low;

        req1.customer = customer1;
        req1.requestType = customer1.npcProfile.typePreference;
        req1.requestQuality = customer1.npcProfile.qualityPreference;
        req1.requestQuantity = 3;
        req1.estimatedPay = 100;

        customer1.strikeCount = 1;
        customer1.isActiveCustomer = true;

        customers.Add(customer1);
        activeRequests.Add(req1);

    }

    private void Update()
    {
        // GENERATE REQUESTS HERE!!!

        //foreach (CustomerData customer in customers)
        //{
        //    GenerateRequest(customer);
        //}
    }




}

public enum RequestOutcome
{
    Perfect,
    Partial,
    Failed
}

public class RequestResult
{
    public RequestOutcome outcome;
    public int correctRequirements;
    public int paymentEarned;
    public int reputationChange;
    public bool strikeAdded;

}
