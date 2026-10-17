using UnityEngine;

public class NRYLevelManager : MonoBehaviour
{

    public static NRYLevelManager Instance;
    // handle checks:

    // how much money player has

    // what day it is

    // how many sold to this level

    // current part of main story

    // update main objective

    // boss fight? (rigged poker?)

    public bool hasEnteredClubOnce = false;

    private void Awake()
    {
        Instance = this;
    }



    private void Start()
    {
        ObjectiveManager.Instance.pages[0].objectives.Clear();
        ObjectiveManager.Instance.pages[0].objectives.Add("Ask around for Tommy.");
        ObjectiveManager.Instance.pages[0].objectives.Add("Continue your 'business'.");

    }



    private void OnEnable()
    {
        StripClubEntrance.OnSCCutsceneTriggered += PrepareForCasinoFight;
    }

    private void OnDisable()
    {
        StripClubEntrance.OnSCCutsceneTriggered -= PrepareForCasinoFight;
    }


    public void PrepareForCasinoFight()
    {
        ObjectiveManager.Instance.pages[0].objectives.Add("Make at least 500 chedz to confront the cassino boss.");



    }






}
