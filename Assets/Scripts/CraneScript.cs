using UnityEngine;
using UnityEngine.InputSystem;

public class CraneScript : MonoBehaviour
{

    public Animator craneAnimator;
    //private Animation tester;
    //public GameObject Boss;

    private bool inRange = false;

    // remember to set animation bools false when another is set true

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //craneAnimator = GameObject.Find("Cube.153").GetComponent<Animator>();

        //if (craneAnimator == null)
        //{
        //    Debug.LogError("Animator NOT found!");
        //}
        //else
        //{
        //    Debug.Log("Animator found on: " + craneAnimator.gameObject.name);
        //}

    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && inRange)
        {
    
           
            // trigger anim
            craneAnimator.SetBool("ButtonPressed", true);

            bool test = craneAnimator.GetBool("ButtonPressed");
            Debug.Log("ButtonPressed value: " + test);

            //Debug.Log("E pressed, bool set!");
            
        }

        
    }

    void CheckAnimationState() 
    {
        // if e is pressed, AND the other bools are false,
        // trigger crane swiing

        // if crane has swung already but missed, trigger crane reset

        // after reset, needs t go back to no movement (all bool false?)




    }

    //public void Interact()
    //{
    //    //throw new System.NotImplementedException();
    //    //bool test = craneAnimator.GetBool(ButtonPressed);
    //    craneAnimator.SetBool("ButtonPressed", true);

    //}


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Press E to Swing Crane!");
            inRange = true;

        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
        }
    }
}
