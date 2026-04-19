using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //public int currCurrency;

    public PlayerState playerState;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void increaseCurrency(int increaseBy)
    {
        playerState.currency += increaseBy;
    }

    public void SellCheese() // add cheese perameter
    {
        // check if correct cheese
        // sell cheese here, increase reputation
        // remove specific cheese from inventory

    }

    public void RecruitRat(NPCProfile npc)
    {

    }

}
