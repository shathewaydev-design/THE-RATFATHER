using System;
using UnityEngine;

public class LocationCheck : MonoBehaviour
{
    [SerializeField] private string locationID;

    public static event Action<String> OnLocationEnter;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        //Debug.Log("Entered: " + locationID);
        OnLocationEnter?.Invoke(locationID);
    }
}
