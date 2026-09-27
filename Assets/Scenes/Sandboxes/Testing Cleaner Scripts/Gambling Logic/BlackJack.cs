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
    [SerializeField] TextMeshProUGUI playerBetDisplay;

    [SerializeField] TextMeshProUGUI onRoundEnd;

    [SerializeField] GameObject betPanel;
    [SerializeField] Slider betSlider;
    [SerializeField] TextMeshProUGUI currSliderNum;
    [SerializeField] float playerBet; 


    public Button hitButton;
    public Button stayButton;
    public Button exitButton;


    [Header("UI")]
    public Animator promptAnimator;
    [SerializeField] private GameObject promptCanvas;

    [Header("Input")]//player inputs
    public ThirdPersonController thirdPersonController;


    void Awake()
    {
        Instance = this;
    }

    public void SetBet()
    {
        currSliderNum.text = "Bet: " + betSlider.value;
        playerBet = betSlider.value;

        // if bet is higher than player's current currency, don't allow them to play
    }
    
    public void HandleBet(bool playerWon)
    {
        if (playerWon)
        {
            InventorySystem.Instance.AddToCurrency(playerBet);
        }
        else if (!playerWon && playerSum == dealerSum)
        {
            return;
        } 
        else if (!playerWon) 
        {
            InventorySystem.Instance.SubtractFromCurrency(playerBet);
        }

        exitButton.enabled = true;

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
        // if bet is higher than player's current currency, don't allow them to play
        if (playerBet > InventorySystem.Instance.GetCurrency())
        {
            Debug.Log("Not enough money to play!");
            ExitGame();
            return;
        }

        // exit button no longer available
        exitButton.enabled = false;

        // display bet
        playerBetDisplay.text = "Your bet: " + playerBet;
        betPanel.SetActive(false);

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

        // moved draw cards to here
        DrawCards();
        DrawTotals();

        // make sure button is enabled
        hitButton.enabled = true;
        stayButton.enabled = true;

    }

    public void ExitGame()
    {
        //Debug.Log("game exited");

        // reset everything after game is over and player exits
        ClearCards();
        ClearText();

        blackjackPanel.SetActive(false);
        thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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

    public void Hit()
    {
        Card c = deck[deck.Count - 1];
        deck.RemoveAt(deck.Count - 1);
        playerSum += c.GetValue();
        playerAceCount += c.IsAce() ? 1 : 0;
        playerHand.Add(c);

        if(ReducePlayerAce() > 21)
        {
            hitButton.enabled = false;
        }
        // making it so dealer also draws after player hits << TESTING
        // seems to work
        if (dealerSum < 17)
        {
            c = deck[deck.Count - 1];
            deck.RemoveAt(deck.Count - 1);
            dealerSum += c.GetValue();
            dealerAceCount += c.IsAce() ? 1 : 0;
            dealerHand.Add(c);

        }

        RedrawCards();
        DrawTotals();
    }

    public void Stay()
    {
        hitButton.enabled = false;
        stayButton.enabled = false;

        while (dealerSum < 17)
        {
            Card c = deck[deck.Count - 1];
            deck.RemoveAt(deck.Count - 1);
            dealerSum += c.GetValue();
            dealerAceCount += c.IsAce() ? 1 : 0;
            dealerHand.Add(c);

        }

        RedrawCards();
        DrawTotals();

    }

    public int ReducePlayerAce()
    {
        while (playerSum > 21 && playerAceCount > 0)
        {
            playerSum -= 10;
            playerAceCount -= 1;
        }

        return playerSum;
    }

    public int ReduceDealerAce()
    {
        while (dealerSum > 21 && dealerAceCount > 0)
        {
            dealerSum -= 10;
            dealerAceCount -= 1;
        }

        return dealerSum;
    }

    public void DrawCards()
    {
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


        if (!stayButton.enabled)
        {
            dealerSum = ReduceDealerAce();
            playerSum = ReducePlayerAce();

        }

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
        if (!stayButton.enabled)
        {
            dealerTotal.text = "Dealer's total: " + dealerSum;

            if (playerSum > 21)
            {
                onRoundEnd.text = "Winner: Dealer!";
                // lose bet
                HandleBet(false);
            }
            else if (dealerSum > 21)
            {
                onRoundEnd.text = "Winner: You!";
                // win bet
                HandleBet(true);
            }
            // both player and dealer have < 21
            else if (playerSum == dealerSum)
            {
                onRoundEnd.text = "Winner: Tie!";
                // bet returned
                HandleBet(false);
            }
            else if (playerSum > dealerSum)
            {
                onRoundEnd.text = "Winner: You!";
                // bet won (doubled and returned)
                HandleBet(true);
            }
            else if (playerSum < dealerSum)
            {
                onRoundEnd.text = "Winner: Dealer!";
                // bet lost (money lost)
                HandleBet(false);
            }


        }
    }

    public void RedrawCards()
    {
        ClearCards();
        DrawCards();


    }

    private void ClearCards()
    {
        foreach (Transform child in playerArea.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in dealerArea.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void ClearText()
    {
        dealerSum = 0;
        playerSum = 0;
        playerAceCount = 0;
        dealerAceCount = 0;

        playerTotal.text = "Player's total: " + playerSum;
        dealerTotal.text = "Dealer's total: ???";
        playerBetDisplay.text = "Your bet: " + 0;
        onRoundEnd.text = "";


    }






























    public void Interact()
    {
        //throw new System.NotImplementedException();
        if (inRange == true)
        {
            blackjackPanel.SetActive(true);
            betPanel.SetActive(true);

            // while setting the bet, player can't press the hit or stay buttons 
            hitButton.enabled = true;
            stayButton.enabled = true;
            exitButton.enabled = true;

            thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Mouse");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;


            //Debug.Log("Playing blackjack...");
            //BlackJack blackjack = new BlackJack();
            //StartGame();
            // need to create cards somewhere
            //DrawCards();
            //DrawTotals();


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
