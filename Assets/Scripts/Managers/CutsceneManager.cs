using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CutsceneManager : MonoBehaviour
{
    // game manager checks and calls this script << GOAL (for simplicity)



    [SerializeField] private GameObject cutscenePanel;
    [SerializeField] private RawImage currPanel;
    [SerializeField] private string currDialogue;


    // NOTES FOR THIS OBJECT TYPE

    //public GameObject CutsceneData currCutscene;

    // cutscenes as types? Start with list of images, properly labeled << dialogue must also be
    // properly linked to each panel, and may have more than one dialogue line per image

    public bool isCutsceneActive; // game manager sets this true? maybe purely for this class



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (isCutsceneActive)
        //{
        //    if () // if mouse button clicked
        //    {
        //        AdvanceCutscene();


        //    }
        //}
        
    }


    // 1. pull up entire cutscene panel
    // 2. grab first comic panel of proper cutscene and display it
    // 3. grab first dialogue line of custene and display it
    // 4. allow for a click to continue the cutscene
    public void StartCutscene()
    {
        cutscenePanel.SetActive(true);

        // CURSOR NEEDS TO BE LOCKED

        // FIND FIRST PANEL

    }

    // if there is an existing comic panel after, when clicking, 
    // it displays this next panel
    // if not, calls for the cutscene to end
    private void AdvanceCutscene()
    {




    }

    // when cutscene has reached its end, removes cutscene panel 
    // and gives player back character controls -- PERCHANCE TELEPORTS 
    private void EndCutscene()
    {

    }
}
