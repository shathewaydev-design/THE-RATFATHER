using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class HypervigilanceScript : MonoBehaviour
{
    //this script is for the hypervigilance
    // see interactable objects nearby and highlight them
    //location player
    

    [Header("Hypervigilance Settings")]
    [SerializeField] private float hypervigilanceDuration = 60f;
    [SerializeField] private float detectionRadius = 20f;
    [SerializeField] private float scanInterval = 0.25f;

    [Header("Visual Effects")]
    [SerializeField] private GameObject hypervigilanceOverlay;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip activationSound;

    private int highlightLayer;

    private float remainingTime;
    private float scanTimer;

    private bool isActive = false;

    // Store original layers so we can restore them later.
    private Dictionary<GameObject, int> originalLayers = new Dictionary<GameObject, int>();

    // Keep track of currently highlighted objects.
    private HashSet<GameObject> highlightedObjects = new HashSet<GameObject>();
    //Every time we scan nearby items, we collect the objects currently within the 20m radius.
    //We don't want the same Transform appearing multiple times.
    //For example, an item has multiple child objects. 
    //We might encounter the same child through different detection paths. 
    //A HashSet prevents duplicate entries.

    private void Awake()
    {
        highlightLayer = LayerMask.NameToLayer("ItemHighlight");

        if (highlightLayer == -1)
        {
            Debug.LogError("Layer 'ItemHighlight' does not exist!");
        }

        if (hypervigilanceOverlay != null)
        {
            hypervigilanceOverlay.SetActive(false);
        }
    }

    private void Update()
    {
        if (!isActive)
            return;

        // Countdown
        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0f)
        {
            DeactivateHypervigilance();
            return;
        }

        // Periodically update nearby highlighted objects every 0.25s.
        scanTimer -= Time.deltaTime;

        if (scanTimer <= 0f)
        {
            ScanNearbyItems();
            scanTimer = scanInterval;
        }
    }

    public void ActivateHypervigilance()
    {
        // Already active: reset duration only.
        if (isActive)
        {
            //This runs when the player takes an upper when hypervigilance is active
            remainingTime = hypervigilanceDuration;
            Debug.Log("Hypervigilance duration refreshed!");
            return;
        }

        isActive = true;
        remainingTime = hypervigilanceDuration;
        scanTimer = 0f;

        // Enable overlay.
        if (hypervigilanceOverlay != null)
        {
            hypervigilanceOverlay.SetActive(true);
        }

        // Play activation sound.
        if (audioSource != null && activationSound != null)
        {
            audioSource.PlayOneShot(activationSound);
        }

        // Initial scan.
        ScanNearbyItems();

        Debug.Log("Hypervigilance activated!");
    }

    private void ScanNearbyItems()
    {
        if (highlightLayer == -1)
            return;

        GameObject[] allInteractableItems = GameObject.FindGameObjectsWithTag("InteractableObj");

        HashSet<GameObject> nearbyGameObjects = new HashSet<GameObject>();

        foreach (GameObject interactableItem in allInteractableItems)
        {
            float distance = Vector3.Distance(transform.position, interactableItem.transform.position);
            //distance from this script to the interactable objects
            //script is placed on player
            if (distance <= detectionRadius)
            {
                // Include all children, since renderers will be attached to child objects.
                Transform[] childrenOfInteractableItem = interactableItem.GetComponentsInChildren<Transform>(true);

                foreach (Transform children in childrenOfInteractableItem)
                {
                    nearbyGameObjects.Add(children.gameObject);
                }
            }
        }

        // Apply highlight layer to nearby objects.
        foreach (GameObject obj in nearbyGameObjects)
        {
            if (obj == null)
                continue;

            if (!originalLayers.ContainsKey(obj))
            {
                originalLayers.Add(obj, obj.gameObject.layer);
            }

            obj.gameObject.layer = highlightLayer;
            highlightedObjects.Add(obj);
        }

        // Restore objects that are no longer within range.
        List<GameObject> objectsToRemove = new List<GameObject>();

        foreach (GameObject obj in highlightedObjects)
        {
            if (obj == null)
            {
                objectsToRemove.Add(obj);
                continue;
            }

            if (!nearbyGameObjects.Contains(obj))
            {
                RestoreLayer(obj);
                objectsToRemove.Add(obj);
            }
        }

        foreach (GameObject obj in objectsToRemove)
        {
            highlightedObjects.Remove(obj);
        }
    }

    private void RestoreLayer(GameObject obj)
    {
        if (obj != null && originalLayers.TryGetValue(obj, out int oldLayer))
        {
            obj.gameObject.layer = oldLayer;
            originalLayers.Remove(obj);
        }
    }

    public void DeactivateHypervigilance()
    {
        isActive = false;
        remainingTime = 0f;

        // Restore all original layers.
        foreach (var layer in originalLayers)
        {
            if (layer.Key != null)
            {
                layer.Key.gameObject.layer = layer.Value;
            }
        }

        originalLayers.Clear();
        highlightedObjects.Clear();

        // Disable overlay.
        if (hypervigilanceOverlay != null)
        {
            hypervigilanceOverlay.SetActive(false);
        }

        Debug.Log("Hypervigilance ended!");
    }
    
    

    
    
    
    
}