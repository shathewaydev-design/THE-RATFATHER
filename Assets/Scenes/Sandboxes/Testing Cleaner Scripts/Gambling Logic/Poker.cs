using StarterAssets;
using System.Collections.Generic;
using TMPro;
using Unity.Multiplayer.PlayMode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Poker : MonoBehaviour, IInteractable
{
    private bool inRange;

    [Header("UI")]
    public Animator promptAnimator;
    [SerializeField] private GameObject promptCanvas;

    [Header("Input")]//player inputs
    public ThirdPersonController thirdPersonController;


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
            switch (value)
            {
                case "A":
                    return 14;

                case "K":
                    return 13;

                case "Q":
                    return 12;

                case "J":
                    return 11;

                case "10":
                    return 10;

                case "9":
                    return 9;

                case "8":
                    return 8;

                case "7":
                    return 7;

                case "6":
                    return 6;

                case "5":
                    return 5;

                case "4":
                    return 4;

                case "3":
                    return 3;

                case "2":
                    return 2;

                default:
                    return 0;
            }
        }

        public string GetType()
        {
            return type;
        }

        override
         public string ToString()
        {
            return value + "-" + type;
        }

    }

    List<Card> deck;
    System.Random random = new System.Random(); // shuffling deck

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

    // Drawing a card!
    public Card DrawCard()
    {
        Card card = deck[0];
        deck.RemoveAt(0);

        return card;
    }
    public void BurnCard()
    {
        deck.RemoveAt(0);
    }

    private void ResetHands()
    {
        player.hand.Clear();
        ai.hand.Clear();
        communityCards.Clear();
    }

    // --------------------------
    // Poker Managing >>>>
    // >>Deck

    // >>Player
    //  >> >>Hole Cards
    //  >> >>Chips
    //  >> >>Current Bet

    // >>CPU Player

    // >>PokerHandEvaluator

    // >>PokerGameState
    // --------------------------

    // Player >> helps keep track of bets // MAY NOT BE NEEDED
    public class PokerPlayer
    {
        public List<Card> hand = new List<Card>();

        public int chips;
        public int currentBet;
        //public int totalBet;

        public bool folded;

        // public bool isAllIn;
        // public int totalBetThisHand;
    }

    public void AIAction()
    {
        List<PokerAction> legalActions = GetAILegalActions();

        PokerAction action = ChooseAIAction(legalActions);

        switch (action)
        {
            case PokerAction.Fold:

                SetText(dealerLastAction, "Dealer Last Action: Fold");
                ProcessAction(ai, PokerAction.Fold);

                break;


            case PokerAction.Check:

                SetText(dealerLastAction, "Dealer Last Action: Check");
                ProcessAction(ai, PokerAction.Check);

                break;


            case PokerAction.Call:

                SetText(dealerLastAction, "Dealer Last Action: Call");
                ProcessAction(ai, PokerAction.Call);

                break;


            case PokerAction.Raise:

                int raiseAmount = currentBet + 10;

                SetText(dealerLastAction, "Dealer Last Action: Raise");
                ProcessAction(ai, PokerAction.Raise, raiseAmount);

                break;
        }


    }

    private PokerAction ChooseAIAction(List<PokerAction> legalActions)
    {
        // 5% chance to fold
        if (legalActions.Contains(PokerAction.Fold))
        {
            int foldChance = random.Next(100);

            if (foldChance < 5)
            {
                return PokerAction.Fold;
            }
        }

        // remove fold so it doesn't get selected randomly below
        List<PokerAction> nonFoldActions = new List<PokerAction>(legalActions);

        nonFoldActions.Remove(PokerAction.Fold);

        int randomIndex = random.Next(nonFoldActions.Count);

        return nonFoldActions[randomIndex];
    }


    //handle ai dealer
    private List<PokerAction> GetAILegalActions()
    {
        List<PokerAction> legalActions = new List<PokerAction>();

        // AI can always fold unless preflop
        // AI cannot fold during preflop
        if (currentPhase != PokerPhase.PreFlop)
        {
            legalActions.Add(PokerAction.Fold);
        }

        // AI can check if it has matched the current bet
        if (ai.currentBet == currentBet)
        {
            legalActions.Add(PokerAction.Check);
        }

        // AI can call if it is behind the current bet
        if (ai.currentBet < currentBet)
        {
            legalActions.Add(PokerAction.Call);
        }

        // AI can raise if it has enough chips
        if (ai.currentBet + ai.chips > currentBet)
        {
            legalActions.Add(PokerAction.Raise);
        }

        return legalActions;
    }




















    [Header("Game Logic")]
    // total winnings to be dealt
    public int pot;
    public float currFunds;

    // player
    //List<Card> playerHand; // 2
    PokerPlayer player;

    // AI
    //List<Card> dealerrHand; // 2
    PokerPlayer ai;

    PokerPlayer currentPlayer;

    // table
    List<Card> communityCards; // eventually 5
    int currentBet;

    bool playerActed;
    bool aiActed;

    // phases of the game
    public enum PokerPhase
    {
        PreFlop,
        Flop,
        Turn,
        River,
        Showdown,
        HandOver
    }

    PokerPhase currentPhase;

    public void StartGame()
    {
        // set blinds panel false
        currFunds = InventorySystem.Instance.GetCurrency();
        SetText(playerChips, "Player Chips " + player.chips);

        // Build then shuffle the deck
        BuildDeck();
        ShuffleDeck();

        // Set players and table

        playerActed = false;
        aiActed = false;

        // Reset hands
        ResetHands();

        // post blinds
        blindPanel.SetActive(false);

        // Deal hole cards
        DealHoleCards();

        // Change phase to preflop
        currentPhase = PokerPhase.PreFlop;
        StartBettingRound(currentPhase);

        UpdateBettingRoundText();

        DrawCards();


    }

    public void PostBlinds() // called by button
    {
        canCloseGame = false;

        player = new PokerPlayer();
        ai = new PokerPlayer();
        communityCards = new List<Card>();

        ai.chips = 10000;
        player.chips = (int)blindSlider.value;

        player.currentBet = 0;
        ai.currentBet = 0;

        //player.totalBet = 0;
        //ai.totalBet = 0;

        pot = 0;
        currentBet = 0;

        StartGame();


    }

    public void StartBettingRound(PokerPhase phase)
    {
        currentPhase = phase;

        playerActed = false;
        aiActed = false;

        UpdateBettingRoundText();

        if (phase == PokerPhase.PreFlop)
        {
            currentPlayer = ai;
        }
        else
        {
            // New betting round, so reset the bets
            player.currentBet = 0;
            ai.currentBet = 0;
            currentBet = 0;

            playerActed = false;
            aiActed = false;

            currentPlayer = player;

        }

        UpdateAvailableActions();
        UpdateRaiseSlider();

        // If it's the AI's turn, have the AI make a decision
        StartCurrentTurn();

    }

    public void EndBettingRound()
    {

        Debug.Log("Ending betting round: " + currentPhase);
        Debug.Log("Community cards BEFORE dealing: " + communityCards.Count);

        switch (currentPhase)
        {
            case PokerPhase.PreFlop:

                DealFlop();
                StartBettingRound(PokerPhase.Flop);

                break;

            case PokerPhase.Flop:

                DealTurn();
                StartBettingRound(PokerPhase.Turn);

                break;

            case PokerPhase.Turn:

                DealRiver();
                StartBettingRound(PokerPhase.River);

                break;

            case PokerPhase.River:

                ShowDown();

                break;
        }

        Debug.Log("Community cards AFTER dealing: " + communityCards.Count);

    }


    // -------------------------------
    // DEALING CARDS
    public void DealHoleCards()
    {
        player.hand.Add(DrawCard());
        ai.hand.Add(DrawCard());

        player.hand.Add(DrawCard());
        ai.hand.Add(DrawCard());

        // switch to preflop phase
        // start preflop bet


    }

    //DealFlop();
    public void DealFlop()
    {
        if (communityCards.Count != 0)
            return;

        BurnCard();

        communityCards.Add(DrawCard());
        communityCards.Add(DrawCard());
        communityCards.Add(DrawCard());

        //currentPhase = PokerPhase.Flop;
        Debug.Log("Flop dealt. Community cards: " + communityCards.Count);

        RedrawCards();
        // start flop bet
    }

    //DealTurn();
    public void DealTurn()
    {
        if (communityCards.Count != 3)
            return;

        BurnCard();

        communityCards.Add(DrawCard());

        //currentPhase = PokerPhase.Turn;

        Debug.Log("Turn dealt. Community cards: " + communityCards.Count);

        RedrawCards();
        // start turn bet
    }

    //DealRiver();
    public void DealRiver()
    {
        if (communityCards.Count != 4)
            return;

        BurnCard();

        communityCards.Add(DrawCard());

        //currentPhase = PokerPhase.River;

        Debug.Log("River dealt. Community cards: " + communityCards.Count);

        RedrawCards();
        // start river, then showdown!
    }

    // -------------------------------

    public void PlayerAction(PokerAction action, int raiseAmount = 0)
    {
        if (currentPlayer != player)
        {
            return;
        }

        if (!IsActionValid(action, raiseAmount))
        {
            return;
        }

        ProcessAction(player, action, raiseAmount); // moved logic to new method so AI can share

    }

    private bool IsActionValid(PokerAction action, int raiseAmount = 0)
    {
        // Determine whether action is legal

        if (currentPlayer != player)
        {
            return false;
        }

        switch (action)
        {
            case PokerAction.Fold:
                return true;

            case PokerAction.Check:
                return player.currentBet == currentBet;

            case PokerAction.Call:
                return player.currentBet < currentBet;

            case PokerAction.Raise:
                return raiseAmount > currentBet &&
                    raiseAmount <= player.currentBet + player.chips;

            default:
                return false;
        }

    }

    public void ProcessAction(PokerPlayer actingPlayer, PokerAction action, int raiseAmount = 0)
    {
        currAction = action;

        switch (action)
        {
            case PokerAction.Fold:

                actingPlayer.folded = true;

                if (actingPlayer == player)
                {
                    playerWon = false;
                    SetText(winnerAnnouncement, "Winner: Dealer!");
                }
                else
                {
                    playerWon = true;
                    SetText(winnerAnnouncement, "Winner: Player!");
                }

                RevealHiddenCards();

                EndHand();

                return;


            case PokerAction.Check:

                SetPlayerActed(actingPlayer);

                break;


            case PokerAction.Call:

                int amountToCall = currentBet - actingPlayer.currentBet;

                actingPlayer.chips -= amountToCall;
                actingPlayer.currentBet += amountToCall;

                //actingPlayer.totalBet += amountToCall;

                pot += amountToCall;
                SetText(potDisplay, "Pot: " + pot);

                if (actingPlayer == player)
                {
                    SetText(playerCurrBet, "Player Current Bet: " + player.currentBet);
                    //SetText(playerTotalBet, "Player Total Bet: " + player.totalBet);
                    SetText(playerChips, "Player Chips: " + player.chips);

                } 
                else
                {
                    SetText(DealerCurrBet, "Dealer Current Bet: " + ai.currentBet);
                    //SetText(dealerTotalBet, "Dealer Total Bet: " + ai.totalBet);
                }

                    SetPlayerActed(actingPlayer);

                break;


            case PokerAction.Raise:

                int amountToRaise = raiseAmount - actingPlayer.currentBet;

                actingPlayer.chips -= amountToRaise;
                actingPlayer.currentBet += amountToRaise;

                //actingPlayer.totalBet += amountToRaise;

                if (actingPlayer == player)
                {
                    SetText(playerCurrBet, "Player Current Bet: " + player.currentBet);
                   // SetText(playerTotalBet, "Player Total Bet: " + player.totalBet);
                    SetText(playerChips, "Player Chips: " + player.chips);
                }
                else
                {
                    SetText(DealerCurrBet, "Dealer Current Bet: " + ai.currentBet);
                    //SetText(dealerTotalBet, "Dealer Total Bet: " + ai.totalBet);
                }

                pot += amountToRaise;
                SetText(potDisplay, "Pot: " + pot);

                currentBet = raiseAmount;

                SetActionAfterRaise(actingPlayer);

                break;
        }

        if (IsBettingRoundComplete())
        {
            EndBettingRound();
            return;
        }

        MoveToNextPlayer();
        StartCurrentTurn();
    }

    private void StartCurrentTurn()
    {
        if (currentPlayer == ai)
        {
            AIAction();
            return;
            //Debug.Log("AI acted!!");
        }

        UpdateAvailableActions();
        UpdateRaiseSlider();

        // If currentPlayer is the player,
        // we simply wait for the player's input.
    }

    private void SetPlayerActed(PokerPlayer actingPlayer)
    {
        if (actingPlayer == player)
        {
            playerActed = true;
        }
        else
        {
            aiActed = true;
        }
    }

    private void SetActionAfterRaise(PokerPlayer actingPlayer)
    {
        if (actingPlayer == player)
        {
            playerActed = true;
            aiActed = false;
        }
        else
        {
            aiActed = true;
            playerActed = false;
        }
    }

    private void MoveToNextPlayer()
    {
        if (currentPlayer == player)
        {
            currentPlayer = ai;
        }
        else
        {
            currentPlayer = player;
        }
    }

    private bool IsBettingRoundComplete()
    {
        if (player.folded || ai.folded)
        {
            return false;
        }

        if (player.currentBet != ai.currentBet)
        {
            return false;
        }

        if (!playerActed || !aiActed)
        {
            return false;
        }

        return true;
    }

    public bool playerWon;
    public void EndHand()
    {

        if (InventorySystem.Instance.GetCurrency() < currFunds + pot && playerWon)
        {

            InventorySystem.Instance.AddToCurrency(pot);

        }
        else if (InventorySystem.Instance.GetCurrency() > currFunds - pot && !playerWon)
        {
            InventorySystem.Instance.SubtractFromCurrency(pot);

        }
            canCloseGame = true;
        

    }


    // ACTIONS >> Fold (Player gives up hand), Check (Player stays in w out putting additional chips in),
    // Call (Player matches current bet), Raise (Player increases the current bet)
    // buttons only active if the current phase needs it
    PokerAction currAction;
    public enum PokerAction
    {
        Fold,
        Check,
        Call,
        Raise
    }

    public void OnFoldButton()
    {
        if (currentPlayer != player)
            return;

        PlayerAction(PokerAction.Fold);
    }

    public void OnCheckButton()
    {
        if (currentPlayer != player)
            return;
        PlayerAction(PokerAction.Check);
    }

    public void OnCallButton()
    {
        if (currentPlayer != player)
            return;
        PlayerAction(PokerAction.Call);
    }

    public void OnRaiseButton()
    {
        if (currentPlayer != player)
            return;
        int raiseAmount = Mathf.RoundToInt(raiseSlider.value);
        PlayerAction(PokerAction.Raise, raiseAmount);
    }

    [Header("Button UI")]
    public GameObject actionArea;
    public Button raise;
    public Button check;
    public Button fold;
    public Button call;

    // check and display what actions are available at this time
    public void UpdateAvailableActions()
    {
        if (currentPlayer != player)
        {
            actionArea.SetActive(false);
            return;
        }

        actionArea.SetActive(true);

        Debug.Log("Player Current Bet: " + player.currentBet);
        Debug.Log("Current Bet: " + currentBet);
        Debug.Log("Can Call: " + (player.currentBet < currentBet));

        fold.interactable = IsActionValid(PokerAction.Fold);
        check.interactable = IsActionValid(PokerAction.Check);
        call.interactable = IsActionValid(PokerAction.Call);
        raise.interactable = CanPlayerRaise();
    }
    private void UpdateRaiseSlider()
    {
        raiseSlider.minValue = currentBet + 1;
        raiseSlider.maxValue = player.currentBet + player.chips;

        raiseSlider.value = raiseSlider.minValue;
    }

    private bool CanPlayerRaise()
    {
        int raiseAmount = Mathf.RoundToInt(raiseSlider.value);

        return IsActionValid(PokerAction.Raise, raiseAmount);
    }


    // BIG MANAGING BELOW: HAND EVALUATOR!!
    public enum PokerHandRank
    {
        HighCard,
        OnePair,
        TwoPair,
        ThreeOfAKind,
        Straight,
        Flush,
        FullHouse,
        FourOfAKind,
        StraightFlush
    }
    public class PokerHand
    {
        public PokerHandRank rank;
        public List<Card> cards = new List<Card>();
        public List<int> values = new List<int>();

    }

    public PokerHand EvaluateHand(List<Card> cards)
    {
        PokerHand hand = new PokerHand();

        // hand checking here
        List<Card> sortedCards = new List<Card>(cards);

        // Sort cards from highest value to lowest value
        for (int i = 0; i < sortedCards.Count; i++)
        {
            for (int j = i + 1; j < sortedCards.Count; j++)
            {
                if (sortedCards[j].GetValue() > sortedCards[i].GetValue())
                {
                    Card temp = sortedCards[i];
                    sortedCards[i] = sortedCards[j];
                    sortedCards[j] = temp;
                }
            }
        }

        // Check for Striaght Flush
        List<Card> straightFlushCards;

        if (TryGetStraightFlush(sortedCards, out straightFlushCards))
        {
            hand.rank = PokerHandRank.StraightFlush;

            hand.cards.AddRange(straightFlushCards);

            // Highest card determines the Straight Flush value
            if (straightFlushCards[0].GetValue() == 14 &&
                straightFlushCards[1].GetValue() == 5)
            {
                // A-2-3-4-5 is 5-high
                hand.values.Add(5);
            }
            else
            {
                hand.values.Add(straightFlushCards[0].GetValue());
            }

            return hand;
        }

        // Check for 4 of a Kind
        List<Card> fourCards;

        if (TryGetFourOfAKind(sortedCards, out fourCards))
        {
            hand.rank = PokerHandRank.FourOfAKind;

            hand.cards.AddRange(fourCards);

            // First four values will be the four of a kind.
            // Last value is the kicker.
            hand.values.Add(fourCards[0].GetValue());
            hand.values.Add(fourCards[4].GetValue());

            return hand;
        }

        // Check for Full House
        int threeOfAKindIndex = -1;
        int pairIndex = -1;

        for (int i = 0; i < sortedCards.Count - 2; i++)
        {
            if (sortedCards[i].GetValue() == sortedCards[i + 1].GetValue() &&
                sortedCards[i].GetValue() == sortedCards[i + 2].GetValue())
            {
                threeOfAKindIndex = i;
                break;
            }
        }

        if (threeOfAKindIndex != -1)
        {
            for (int i = 0; i < sortedCards.Count - 1; i++)
            {
                // Don't use the cards that belong to the three of a kind
                if (i == threeOfAKindIndex ||
                    i == threeOfAKindIndex + 1 ||
                    i == threeOfAKindIndex + 2)
                {
                    continue;
                }

                if (sortedCards[i].GetValue() == sortedCards[i + 1].GetValue())
                {
                    pairIndex = i;
                    break;
                }
            }
        }

        if (pairIndex != -1)
        {
            hand.rank = PokerHandRank.FullHouse;

            // Add the three of a kind
            hand.cards.Add(sortedCards[threeOfAKindIndex]);
            hand.cards.Add(sortedCards[threeOfAKindIndex + 1]);
            hand.cards.Add(sortedCards[threeOfAKindIndex + 2]);

            hand.values.Add(sortedCards[threeOfAKindIndex].GetValue());

            // Add the pair
            hand.cards.Add(sortedCards[pairIndex]);
            hand.cards.Add(sortedCards[pairIndex + 1]);

            hand.values.Add(sortedCards[pairIndex].GetValue());

            return hand;
        }


        // Check for Flush
        List<Card> flushCards;

        if (TryGetFlush(sortedCards, out flushCards))
        {
            hand.rank = PokerHandRank.Flush;

            hand.cards.AddRange(flushCards);

            for (int i = 0; i < flushCards.Count; i++)
            {
                hand.values.Add(flushCards[i].GetValue());
            }

            return hand;
        }

        // Check for Straight
        List<Card> straightCards;

        if (TryGetStraight(sortedCards, out straightCards))
        {
            hand.rank = PokerHandRank.Straight;
            hand.cards.AddRange(straightCards);

            // Ace-low straight is 5-high
            if (straightCards[0].GetValue() == 14 &&
                straightCards[1].GetValue() == 5)
            {
                hand.values.Add(5);
            }
            else
            {
                hand.values.Add(straightCards[0].GetValue());
            }

            return hand;
        }


        // Check for Three of a Kind
        for (int i = 0; i < sortedCards.Count - 2; i++)
        {
            if (sortedCards[i].GetValue() == sortedCards[i + 1].GetValue() &&
                sortedCards[i].GetValue() == sortedCards[i + 2].GetValue())
            {
                hand.rank = PokerHandRank.ThreeOfAKind;

                // Add the three of a kind
                hand.cards.Add(sortedCards[i]);
                hand.cards.Add(sortedCards[i + 1]);
                hand.cards.Add(sortedCards[i + 2]);

                hand.values.Add(sortedCards[i].GetValue());

                // Add the two highest remaining cards as kickers
                int addedKickers = 0;

                for (int j = 0; j < sortedCards.Count && addedKickers < 2; j++)
                {
                    if (sortedCards[j].GetValue() != sortedCards[i].GetValue())
                    {
                        hand.cards.Add(sortedCards[j]);
                        hand.values.Add(sortedCards[j].GetValue());

                        addedKickers++;
                    }
                }

                return hand;
            }
        }

        // Check for Two Pair
        int firstPairIndex = -1;
        int secondPairIndex = -1;

        for (int i = 0; i < sortedCards.Count - 1; i++)
        {
            if (sortedCards[i].GetValue() == sortedCards[i + 1].GetValue())
            {
                if (firstPairIndex == -1)
                {
                    firstPairIndex = i;
                }
                else
                {
                    secondPairIndex = i;
                    break;
                }
            }
        }

        if (secondPairIndex != -1)
        {
            hand.rank = PokerHandRank.TwoPair;

            // Add the high pair
            hand.cards.Add(sortedCards[firstPairIndex]);
            hand.cards.Add(sortedCards[firstPairIndex + 1]);

            hand.values.Add(sortedCards[firstPairIndex].GetValue());

            // Add the low pair
            hand.cards.Add(sortedCards[secondPairIndex]);
            hand.cards.Add(sortedCards[secondPairIndex + 1]);

            hand.values.Add(sortedCards[secondPairIndex].GetValue());

            // Find the highest remaining card for the kicker
            for (int i = 0; i < sortedCards.Count; i++)
            {
                if (i != firstPairIndex &&
                    i != firstPairIndex + 1 &&
                    i != secondPairIndex &&
                    i != secondPairIndex + 1)
                {
                    hand.cards.Add(sortedCards[i]);
                    hand.values.Add(sortedCards[i].GetValue());
                    break;
                }
            }

            return hand;
        }

        // Check for One Pair
        for (int i = 0; i < sortedCards.Count - 1; i++)
        {
            if (sortedCards[i].GetValue() == sortedCards[i + 1].GetValue())
            {
                hand.rank = PokerHandRank.OnePair;

                // Add the pair
                hand.cards.Add(sortedCards[i]);
                hand.cards.Add(sortedCards[i + 1]);

                hand.values.Add(sortedCards[i].GetValue());

                // Add the three highest remaining cards
                int addedKickers = 0;

                for (int j = 0; j < sortedCards.Count && addedKickers < 3; j++)
                {
                    if (sortedCards[j].GetValue() != sortedCards[i].GetValue())
                    {
                        hand.cards.Add(sortedCards[j]);
                        hand.values.Add(sortedCards[j].GetValue());

                        addedKickers++;
                    }
                }

                return hand;
            }
        }

        // High Card
        hand.rank = PokerHandRank.HighCard;

        for (int i = 0; i < 5; i++)
        {
            hand.cards.Add(sortedCards[i]);
            hand.values.Add(sortedCards[i].GetValue());
        }

        return hand;
    }

    private bool TryGetStraightFlush(List<Card> sortedCards, out List<Card> straightFlushCards)
    {
        straightFlushCards = new List<Card>();

        string[] suits = { "C", "D", "H", "S" };

        for (int i = 0; i < suits.Length; i++)
        {
            List<Card> cardsOfSuit = new List<Card>();

            // Get all cards of this suit
            for (int j = 0; j < sortedCards.Count; j++)
            {
                if (sortedCards[j].GetType() == suits[i])
                {
                    cardsOfSuit.Add(sortedCards[j]);
                }
            }

            // Need at least five cards of the same suit
            if (cardsOfSuit.Count < 5)
            {
                continue;
            }

            // Look for five consecutive cards
            int consecutiveCards = 1;
            List<Card> currentStraight = new List<Card>();

            currentStraight.Add(cardsOfSuit[0]);

            for (int j = 0; j < cardsOfSuit.Count - 1; j++)
            {
                int currentValue = cardsOfSuit[j].GetValue();
                int nextValue = cardsOfSuit[j + 1].GetValue();

                // Duplicate rank
                if (currentValue == nextValue)
                {
                    continue;
                }

                // Continue the straight
                if (nextValue == currentValue - 1)
                {
                    consecutiveCards++;
                    currentStraight.Add(cardsOfSuit[j + 1]);
                }
                else
                {
                    // Sequence was broken
                    consecutiveCards = 1;
                    currentStraight.Clear();
                    currentStraight.Add(cardsOfSuit[j + 1]);
                }

                if (consecutiveCards == 5)
                {
                    straightFlushCards.AddRange(currentStraight);
                    return true;
                }
            }

            // Check A-2-3-4-5
            bool hasAce = false;
            bool hasFive = false;
            bool hasFour = false;
            bool hasThree = false;
            bool hasTwo = false;

            for (int j = 0; j < cardsOfSuit.Count; j++)
            {
                int value = cardsOfSuit[j].GetValue();

                if (value == 14)
                    hasAce = true;
                else if (value == 5)
                    hasFive = true;
                else if (value == 4)
                    hasFour = true;
                else if (value == 3)
                    hasThree = true;
                else if (value == 2)
                    hasTwo = true;
            }

            if (hasAce && hasFive && hasFour && hasThree && hasTwo)
            {
                straightFlushCards.Clear();

                // Add Ace
                for (int j = 0; j < cardsOfSuit.Count; j++)
                {
                    if (cardsOfSuit[j].GetValue() == 14)
                    {
                        straightFlushCards.Add(cardsOfSuit[j]);
                        break;
                    }
                }

                // Add 5, 4, 3, 2
                for (int value = 5; value >= 2; value--)
                {
                    for (int j = 0; j < cardsOfSuit.Count; j++)
                    {
                        if (cardsOfSuit[j].GetValue() == value)
                        {
                            straightFlushCards.Add(cardsOfSuit[j]);
                            break;
                        }
                    }
                }

                return true;
            }
        }

        return false;
    }

    private bool TryGetFourOfAKind(List<Card> sortedCards, out List<Card> fourCards)
    {
        fourCards = new List<Card>();

        for (int i = 0; i < sortedCards.Count - 3; i++)
        {
            int value = sortedCards[i].GetValue();

            if (sortedCards[i + 1].GetValue() == value &&
                sortedCards[i + 2].GetValue() == value &&
                sortedCards[i + 3].GetValue() == value)
            {
                // Add the four matching cards
                fourCards.Add(sortedCards[i]);
                fourCards.Add(sortedCards[i + 1]);
                fourCards.Add(sortedCards[i + 2]);
                fourCards.Add(sortedCards[i + 3]);

                // Find the highest remaining card for the kicker
                for (int j = 0; j < sortedCards.Count; j++)
                {
                    if (sortedCards[j].GetValue() != value)
                    {
                        fourCards.Add(sortedCards[j]);
                        break;
                    }
                }

                return true;
            }
        }

        return false;
    }

    private bool TryGetStraight(List<Card> sortedCards, out List<Card> straightCards)
    {
        straightCards = new List<Card>();

        int consecutiveCards = 1;

        straightCards.Add(sortedCards[0]);

        for (int i = 0; i < sortedCards.Count - 1; i++)
        {
            int currentValue = sortedCards[i].GetValue();
            int nextValue = sortedCards[i + 1].GetValue();

            // Skip duplicate ranks
            if (currentValue == nextValue)
            {
                continue;
            }

            // Continue the straight
            if (nextValue == currentValue - 1)
            {
                consecutiveCards++;
                straightCards.Add(sortedCards[i + 1]);
            }
            else
            {
                // Start a new sequence
                consecutiveCards = 1;
                straightCards.Clear();
                straightCards.Add(sortedCards[i + 1]);
            }

            if (consecutiveCards == 5)
            {
                return true;
            }
        }

        // Check for A-2-3-4-5
        bool hasAce = false;
        bool hasFive = false;
        bool hasFour = false;
        bool hasThree = false;
        bool hasTwo = false;

        for (int i = 0; i < sortedCards.Count; i++)
        {
            int value = sortedCards[i].GetValue();

            if (value == 14)
                hasAce = true;
            else if (value == 5)
                hasFive = true;
            else if (value == 4)
                hasFour = true;
            else if (value == 3)
                hasThree = true;
            else if (value == 2)
                hasTwo = true;
        }

        if (hasAce && hasFive && hasFour && hasThree && hasTwo)
        {
            straightCards.Clear();

            // Add Ace
            for (int i = 0; i < sortedCards.Count; i++)
            {
                if (sortedCards[i].GetValue() == 14)
                {
                    straightCards.Add(sortedCards[i]);
                    break;
                }
            }

            // Add 5, 4, 3, 2
            for (int value = 5; value >= 2; value--)
            {
                for (int i = 0; i < sortedCards.Count; i++)
                {
                    if (sortedCards[i].GetValue() == value)
                    {
                        straightCards.Add(sortedCards[i]);
                        break;
                    }
                }
            }

            return true;
        }

        // No straight found
        straightCards.Clear();
        return false;
    }

    private bool TryGetFlush(List<Card> sortedCards, out List<Card> flushCards)
    {
        flushCards = new List<Card>();

        string[] suits = { "C", "D", "H", "S" };

        for (int i = 0; i < suits.Length; i++)
        {
            List<Card> cardsOfSuit = new List<Card>();

            for (int j = 0; j < sortedCards.Count; j++)
            {
                if (sortedCards[j].GetType() == suits[i])
                {
                    cardsOfSuit.Add(sortedCards[j]);
                }
            }

            if (cardsOfSuit.Count >= 5)
            {
                // Because sortedCards is already highest to lowest,
                // the first five are the best five cards of this suit.
                for (int j = 0; j < 5; j++)
                {
                    flushCards.Add(cardsOfSuit[j]);
                }

                return true;
            }
        }

        return false;
    }

    private int CompareHands(PokerHand playerHand, PokerHand aiHand)
    {
        if (playerHand.rank > aiHand.rank)
        {
            return 1;
        }

        if (playerHand.rank < aiHand.rank)
        {
            return -1;
        }

        // Same hand rank
        // Compare the cards that break the tie
        for (int i = 0; i < playerHand.values.Count; i++)
        {
            if (playerHand.values[i] > aiHand.values[i])
            {
                return 1;
            }

            if (playerHand.values[i] < aiHand.values[i])
            {
                return -1;
            }
        }

        return 0;
    }

    public void ShowDown()
    {
        List<Card> playerCards = new List<Card>();
        playerCards.AddRange(player.hand);
        playerCards.AddRange(communityCards);

        List<Card> aiCards = new List<Card>();
        aiCards.AddRange(ai.hand);
        aiCards.AddRange(communityCards);

        PokerHand playerHand = EvaluateHand(playerCards);
        PokerHand aiHand = EvaluateHand(aiCards);

        int result = CompareHands(playerHand, aiHand);

        if (result > 0)
        {
            // Player wins
            player.chips += pot;
            InventorySystem.Instance.AddToCurrency(pot);
            SetText(winnerAnnouncement, "Winner: You!");
            playerWon = true;
        }
        else if (result < 0)
        {
            // AI wins
            InventorySystem.Instance.SubtractFromCurrency(pot);
            SetText(winnerAnnouncement, "Winner: Dealer!");
            playerWon = false;
        }
        else
        {
            // Tie
            int halfPot = pot / 2;

            player.chips += halfPot;
            ai.chips += halfPot;

            // If the pot is odd, the dealer gets the extra chip
            if (pot % 2 != 0)
            {
                ai.chips += 1;
               
            }

            InventorySystem.Instance.AddToCurrency(halfPot);
            SetText(winnerAnnouncement, "Winner: Tie!");

        }

        // show hidden cards
        RevealHiddenCards();




        EndHand();

    }


    public void RevealHiddenCards()
    {

        foreach (Transform child in dealerArea.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Card c in ai.hand)
        {
            GameObject newCard = Instantiate(cardPrefab, dealerArea.transform);
            CardDisplay cardDisplay = newCard.GetComponent<CardDisplay>();
            cardDisplay.SetPokerCard(c, true, true);
        }

    }































    // Poker UI>>>
    [Header("Panel UI")]
    public Slider raiseSlider;
    public Slider blindSlider;

    public GameObject pokerPanel;
    public GameObject blindPanel;
    public GameObject dealerArea;
    public GameObject tableArea;
    public GameObject playerArea;

    public TextMeshProUGUI dealerLastAction;
    public TextMeshProUGUI DealerCurrBet;
    public TextMeshProUGUI currBetRound;

    public TextMeshProUGUI playerChips;
    public TextMeshProUGUI playerCurrBet;
    //public TextMeshProUGUI playerTotalBet;

    public TextMeshProUGUI potDisplay;
    public TextMeshProUGUI raiseBetTo;
    public TextMeshProUGUI chipSet;

    public TextMeshProUGUI winnerAnnouncement;

    public GameObject cardPrefab;

    public void DrawCards()
    {
        foreach (Card c in player.hand)
        {
            GameObject newCard = Instantiate(cardPrefab, playerArea.transform);
            CardDisplay cardDisplay = newCard.GetComponent<CardDisplay>();
            cardDisplay.SetPokerCard(c, false);

        }

        foreach (Card c in communityCards)
        {
            GameObject newCard = Instantiate(cardPrefab, tableArea.transform);
            CardDisplay cardDisplay = newCard.GetComponent<CardDisplay>();
            cardDisplay.SetPokerCard(c, false);
        }
  
        foreach (Card c in ai.hand)
        {
            GameObject newCard = Instantiate(cardPrefab, dealerArea.transform);
            CardDisplay cardDisplay = newCard.GetComponent<CardDisplay>();
            cardDisplay.SetPokerCard(c, true);
        }

    }

    public void ClearCards()
    {
        foreach (Transform child in playerArea.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in tableArea.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in dealerArea.transform)
        {
            Destroy(child.gameObject);
        }

    }

    public void RedrawCards()
    {
        ClearCards();
        DrawCards();


    }

    public void ClearText()
    {
        potDisplay.text = "Pot: ";

        dealerLastAction.text = "Dealer Last Action: ";
        DealerCurrBet.text = "Dealer Current Bet: ";
        //dealerTotalBet.text = "Dealer Total Bet: ";
        currBetRound.text = "Current Betting Round: ";

        playerChips.text = "Player Chips: ";
        playerCurrBet.text = "Player Current Bet: ";
        //playerTotalBet.text = "Player Total Bet: ";

        winnerAnnouncement.text = "";
        raiseBetTo.text = "Raising Bet To: ";
    }

    public void SetPot()
    {
        potDisplay.text = "Pot: " + pot;
    }

    public void SetText(TextMeshProUGUI text, string message)
    {

        text.text = message;


    }

    public void UpdateRaiseText()
    {
        raiseBetTo.text = "Raise Bet To: " + Mathf.RoundToInt(raiseSlider.value);
    }

    public void UpdateChipSetText()
    {
        chipSet.text = blindSlider.value + " Chips";
     
    }

    public void UpdateBettingRoundText()
    {
        switch (currentPhase)
        {
            case PokerPhase.PreFlop:
                currBetRound.text = "Current Betting Round: Pre-Flop";
                break;

            case PokerPhase.Flop:
                currBetRound.text = "Current Betting Round: Flop";
                break;

            case PokerPhase.Turn:
                currBetRound.text = "Current Betting Round: Turn";
                break;

            case PokerPhase.River:
                currBetRound.text = "Current Betting Round: River";
                break;

            case PokerPhase.Showdown:
                currBetRound.text = "Showdown";
                break;

            case PokerPhase.HandOver:
                currBetRound.text = "Hand Over";
                break;
        }
    }


    public void OnExitButton()
    {
        TryCloseGame();
    }

    public void CloseGame()
    {

        if (canCloseGame)
        {
            pokerPanel.SetActive(false);

            thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            //ResetHands();
            ClearCards();
            ClearText();
        }
        

    }

    bool canCloseGame = true;
    public void TryCloseGame()
    {
        if (!canCloseGame)
            return;

        CloseGame();
    }



















    // Interaction w physical table

    public void Interact()
    {
        if (inRange == true)
        {
            Debug.Log("Playing poker...");
            pokerPanel.SetActive(true);
            blindPanel.SetActive(true);

            blindSlider.maxValue = InventorySystem.Instance.GetCurrency();
            blindSlider.minValue = 50;

            thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Mouse");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;



         
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Play Poker?");
            inRange = true;
            //Debug.Log(inRange);


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
