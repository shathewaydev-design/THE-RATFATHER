using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheeseSatiationAmountScript : MonoBehaviour
{

    [Header("Satiation by Rarity")]
    [SerializeField] private float commonAmount = 400f;
    [SerializeField] private float rareAmount = 600f;
    [SerializeField] private float epicAmount = 850f;
    [SerializeField] private float legendaryAmount = 1200f;

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