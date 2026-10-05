using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheeseSatiationAmountScript : MonoBehaviour
{

    [Header("Satiation by Rarity")]
    public float commonAmount = 400f;
    public float rareAmount = 600f;
    public float epicAmount = 850f;
    public float legendaryAmount = 1200f;

    public float GetSatiationAmount(CheeseRarity cheeseRarity)
    {
        switch (cheeseRarity)
        {
            case CheeseRarity.Common:
                return commonAmount;

            case CheeseRarity.Rare:
                return rareAmount;

            case CheeseRarity.Epic:
                return epicAmount;

            case CheeseRarity.Legendary:
                return legendaryAmount;

            default:
                return 0f;
        }
    }



}