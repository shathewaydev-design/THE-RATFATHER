using System.Collections;
using UnityEngine;

public class AddFlavor : MonoBehaviour
{
    [SerializeField] private GameObject flavorObject; 
    [SerializeField] private GameObject spawnPoint; 
    void OnEnable()
    {
        flavorObject.transform.position = spawnPoint.transform.position;
        flavorObject.transform.rotation = spawnPoint.transform.rotation;
    }

}