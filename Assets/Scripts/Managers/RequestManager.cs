using System.Collections.Generic;
using UnityEngine;

public class RequestManager : MonoBehaviour
{
    // EVENTS ---------------------------------------------------->>


    // VARIABLES ---------------------------------------------------->>
    public static RequestManager Instance;
    private List<CustomerData> customers;
    private List<RequestData> activeRequests;
    [SerializeField] private float requestInterval;
    [SerializeField] private float requestTimer;

    // read only lists for protection
    public IReadOnlyList<CustomerData> Customers => customers;
    public IReadOnlyList<RequestData> ActiveRequests => activeRequests;




    // METHODS ---------------------------------------------------->>

    public void RegisterCustomer(CustomerData customer) 
    {
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

    private void CheckForNewRequests() // checks whether or not customer should generate a request
    {
        //foreach (CustomerData customer in customers)
        //{


        //}

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

    private void Update()
    {
        foreach (CustomerData customer in customers)
        {
            GenerateRequest(customer);
        }
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
