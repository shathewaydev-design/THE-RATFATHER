using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;
using static UnityEditor.FilePathAttribute;

public class BlackJack : MonoBehaviour, IInteractable
{
    public static BlackJack Instance;

    private bool inRange;

    [SerializeField] GameObject blackjackPanel;
    [SerializeField] GameObject cardPrefab;

    [SerializeField] GameObject playerArea;
    [SerializeField] GameObject dealerArea;

    [SerializeField] TextMeshProUGUI dealerTotal;
    [SerializeField] TextMeshProUGUI playerTotal;


    [Header("UI")]
    public Animator promptAnimator;
    [SerializeField] private GameObject promptCanvas;

    [Header("Input")]//player inputs
    public ThirdPersonController thirdPersonController;


    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //Debug.Log("Code running?");
        //BlackJack blackjack = new BlackJack();

    }

    void Update()
    {
        
    }

    public class Card
    {
        string value;
        string type;

        public Card(string value, string type)
        {
            this.value = value;
            this.type = type;
        }

        public int GetValue()
        {
            if ("AJQK".Contains(value)) // A J Q K
            {
                if (value == "A")
                {
                    return 11;
                }
                return 10;
            }

            return int.Parse(value); // 2-10
        }

        public bool IsAce()
        {
            return value == "A";
        }

        override
         public string ToString()
        {
            return value + "-" + type;
        }
    }

    List<Card> deck;
    System.Random random = new System.Random(); // shuffling deck

    // dealer
    Card hiddenCard;
    List<Card> dealerHand;
    int dealerSum;
    int dealerAceCount;

    public Card GetHiddenCard()
    {
        return hiddenCard;
    }

    // player
    List<Card> playerHand;
    int playerSum;
    int playerAceCount;



    public void StartGame()
    {
        // deck
        BuildDeck();
        ShuffleDeck();

        // dealer
        dealerHand = new List<Card>();
        dealerSum = 0;
        dealerAceCount = 0;

        hiddenCard = deck[deck.Count - 1];
        deck.RemoveAt(deck.Count - 1);
        dealerSum += hiddenCard.GetValue();
        dealerAceCount += hiddenCard.IsAce() ? 1 : 0;

        Card card = deck[deck.Count - 1];
        deck.RemoveAt(deck.Count - 1);
        dealerSum += card.GetValue();
        dealerAceCount += hiddenCard.IsAce() ? 1 : 0;
        dealerHand.Add(card);


        // player
        playerHand = new List<Card>();
        playerSum = 0;
        playerAceCount = 0;
        for (int i = 0; i < 2; i++)
        {
            card = deck[deck.Count - 1];
            deck.RemoveAt(deck.Count - 1);

            playerSum += card.GetValue();
            playerAceCount += card.IsAce() ? 1 : 0;
            playerHand.Add(card);

        }


        //Debug.Log("PLAYER HAND: ");
        //foreach (Card c in playerHand)
        //{
        //    Debug.Log(c);
        //}
        //Debug.Log("player sum: " + playerSum);
        //Debug.Log("player ace count: " + playerAceCount);



        //Debug.Log("DEALER HAND: ");
        //Debug.Log("hidden card: " + hiddenCard);
        //foreach (Card c in dealerHand)
        //{
        //    Debug.Log(c);
        //}
        //Debug.Log("dealer sum: " + dealerSum);
        //Debug.Log(" dealer ace count: " + dealerAceCount);

    }

    public void BuildDeck()
    {
        deck = new List<Card>();
        string[] values = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
        string[] types = { "C", "D", "H", "S" };

        for (int i = 0; i < types.Length; i++)
        {
            for (int j = 0; j < values.Length; j++) 
            {
                Card card = new Card(values[j], types[i]); 
                deck.Add(card);
            }
        }

        //Debug.Log("BUILD DECK: ");
        //foreach (Card card in deck)
        //{
        //    Debug.Log(card);
        //}
        //Debug.Log(deck.ToString());

    }

    public void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++) 
        {
            int j = random.Next(deck.Count);
            Card currCard = deck[i];
            Card randomCard = deck[j];
            deck[i] = randomCard;
            deck[j] = currCard;

        }



    }


    public void DrawCards()
    {
        //Debug.Log("drawing cards!");


        foreach (Card c in playerHand)
        {
            GameObject newCard = Instantiate(cardPrefab, playerArea.transform);
            CardDisplay cardDisplay = newCard.GetComponent<CardDisplay>();
            cardDisplay.SetCard(c);

        }

        DrawHiddenCard();
        foreach (Card c in dealerHand)
        {
            GameObject newCard = Instantiate(cardPrefab, dealerArea.transform);
            CardDisplay cardDisplay = newCard.GetComponent<CardDisplay>();
            cardDisplay.SetCard(c);
        }

        //DrawHiddenCard();
    }

    public void DrawHiddenCard()
    {
        GameObject newCard = Instantiate(cardPrefab, dealerArea.transform);
        CardDisplay cardDisplay = newCard.GetComponent<CardDisplay>();
        cardDisplay.SetCard(hiddenCard);
    }

    public void DrawTotals()
    {
        playerTotal.text = "Player's total: " + playerSum;
        int hiddenTotal = Mathf.Abs(dealerSum - hiddenCard.GetValue());
        dealerTotal.text = "Dealer's total: " + hiddenTotal + " + ???";

        // need to add logic for when winner is revealed, dealer sum fully revealed
    }























    public void Interact()
    {
        //throw new System.NotImplementedException();
        if (inRange == true)
        {
            blackjackPanel.SetActive(true);

            thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Mouse");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;


            //Debug.Log("Playing blackjack...");
            //BlackJack blackjack = new BlackJack();
            StartGame();
            // need to create cards somewhere
            DrawCards();
            DrawTotals();


        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Play BlackJack?");
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
