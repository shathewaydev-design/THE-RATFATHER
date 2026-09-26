using UnityEngine;

public class Poker : MonoBehaviour
{
    private bool inRange;

    [Header("UI")]
    public Animator promptAnimator;
    [SerializeField] private GameObject promptCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Interact()
    {
        //throw new System.NotImplementedException();
        if (inRange == true)
        {
            //blackjackPanel.SetActive(true);
            Debug.Log("Playing poker...");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Play Poker?");
            inRange = true;



            if (promptAnimator != null)
            {
                promptAnimator.SetBool("UIappeared", true);
                promptAnimator.SetTrigger("UIappearing");
            }

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Play BlackJack?");
            inRange = false;

            if (promptAnimator != null)
            {
                promptAnimator.SetBool("UIappeared", false);
                promptAnimator.SetTrigger("UIdisappearing");

            }

        }

    }
}
