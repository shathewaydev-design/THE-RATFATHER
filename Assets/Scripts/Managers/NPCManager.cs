using UnityEngine;

public class NPCManager : MonoBehaviour
{
    // will most likely replace NPC script later -- need to clean things up

    public NPCProfile profile;
    public NPCState state;

    private bool canInteract;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = new NPCState();
    }

    // Update is called once per frame
    void Update()
    {
       // if canInteract == true ... E can be pressed, dialogue panel pops up 

        // if canInteract == false ... E cannot be pressef, dialogue panel turns off
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name + " entered the trigger!");

        
        if (other.CompareTag("Player"))
        {
            // trigger ui--E pop up
            canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // trigger ui--E goes back down
        canInteract = true;

    }



}
