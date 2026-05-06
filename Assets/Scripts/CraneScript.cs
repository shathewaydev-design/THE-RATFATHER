using UnityEngine;
using UnityEngine.InputSystem;

public class CraneScript : MonoBehaviour
{

    public Animator craneAnimator;
    //private Animation tester;
    //public GameObject Boss;

    private bool inRange = false;

    private float resetTimer = 0f;
    private float resetReq = 3f;

    private bool buttonPressed;
    private bool hasSwungAndMissed;
    private bool hasReset;

    public bool hitBoss = false;



    // remember to set animation bools false when another is set true

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {




    }

    // Update is called once per frame
    void Update()
    {

        CheckAnimationState();

        if (hasSwungAndMissed)
        {
            ResetTimer();
        }


    }

    void CheckAnimationState() 
    {


        // if e is pressed, AND the other bools are false,
        // trigger crane swing
        if (hitBoss)
        {
            buttonPressed = false;
            hasSwungAndMissed = false;
            hasReset = false;

            craneAnimator.SetBool("ButtonPressed", false);
            craneAnimator.SetBool("HasSwung&Missed", false);
            craneAnimator.SetBool("HasReset", false);

            return;
        }

        if (Keyboard.current.eKey.wasPressedThisFrame && inRange && !buttonPressed)
        {
            // trigger anim
            hasReset = false;
            craneAnimator.SetBool("HasReset", false);

            craneAnimator.SetBool("ButtonPressed", true);
            buttonPressed = true;
            StartHitWindow();

        }

        // if crane has swung already but missed, trigger crane reset
        if (!SoldatoScript.Instance.IsCraneWindowActive() && buttonPressed && !hitBoss)
        {
            //Debug.Log(name + " Missed so will reset");

            hasSwungAndMissed = true;

            craneAnimator.SetBool("HasSwung&Missed", true);
            craneAnimator.SetBool("ButtonPressed", false);

            buttonPressed = false; 

            resetTimer = 0f; // reset timer ONCE
        }

        

        // after reset, needs to go back to no movement (all bool false?)

    }

    public float hitWindowDuration = 2f;

    public void StartHitWindow()
    {
        Debug.Log(name + " started hit window");

        SoldatoScript.Instance.StartCraneWindow(this, hitWindowDuration);
    }



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


    void ResetTimer()
    {
        resetTimer += Time.deltaTime;

        if (resetTimer >= resetReq)
        {
            craneAnimator.SetBool("HasReset", true);
            hasReset = true;
            craneAnimator.SetBool("HasSwung&Missed", false);

            hasSwungAndMissed = false; // stop timer loop
            resetTimer = 0f;
        }


    }


}


