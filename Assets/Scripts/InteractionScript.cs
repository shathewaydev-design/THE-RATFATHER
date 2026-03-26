using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

interface IInteractable
{
    public void Interact();
}
public class InteractionScript : MonoBehaviour
{
    private List<IInteractable> interactablesInRange = new();//closest object gets interacted first

    public InputActionAsset InputActions;//import all function from the input system
    public InputAction interact;//press E

    private void OnEnable()//this is necessary to avoid bugs
    {
        InputActions.FindActionMap("Player").Enable();

    }
    private void OnDisable()//this is necessary to avoid bugs
    {
        InputActions.FindActionMap("Player").Disable();

    }
    private void Awake()//this is necessary to avoid bugs
    {
        interact = InputSystem.actions.FindAction("Interact");
    }

    void Update()
    {
        if(interact.WasPressedThisFrame())
        {
            if (interactablesInRange.Count > 0)
            {
                int closestInteractableIndex = 0;
                float closestDistance = Mathf.Infinity;

                for (int i = 0; i < interactablesInRange.Count; i++)
                {
                    // Cast to MonoBehaviour to access transform
                    MonoBehaviour interactableMonoBehaviourScript = (MonoBehaviour)interactablesInRange[i];

                    float objDistance = Vector3.Distance(
                        transform.position,
                        interactableMonoBehaviourScript.transform.position
                    );

                    if (objDistance < closestDistance)
                    {
                        closestDistance = objDistance;
                        closestInteractableIndex = i;
                    }
                }

                interactablesInRange[closestInteractableIndex].Interact();
                interactablesInRange.RemoveAt(closestInteractableIndex);//remove from list to avoid bug

                // IInteractable closest = null;
                // float closestDistance = Mathf.Infinity;

                // foreach (var interactable in interactablesInRange)
                // {
                //     MonoBehaviour mono = (MonoBehaviour)interactable;

                //     float distance = Vector3.Distance(
                //         transform.position,
                //         mono.transform.position
                //     );

                //     if (distance < closestDistance)
                //     {
                //         closestDistance = distance;
                //         closest = interactable;
                //     }
                // }

                // if (closest != null)
                // {
                //     closest.Interact();
                    
                // }
            }
        }

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InteractableObj"))
        {            
            if(other.TryGetComponent(out IInteractable interactObj))//TryGetComponent is like GetComponent but checked for null
            {
                interactablesInRange.Add(interactObj);//add this to the list so when press E, it runs interact function
            }

        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("InteractableObj"))
        {
            if (other.TryGetComponent(out IInteractable interactObj))
            {
                interactablesInRange.Remove(interactObj);
            }
        }
    }


}