using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // Allows us to access both our card and deck script
    public CardScript cardScript;   
    public DeckScript deckScript;

    // This is the value of the hand
    public int handValue = 0;

    // Gives player 1000 to start of with
    private int money = 1000;

    // This array is used to store cards that are currently in play
    public GameObject[] hand;

    // This is used to store the index of the next card being played
    public int cardIndex = 0;

    // This list is used to track any ace that is player
    List<CardScript> aceList = new List<CardScript>();

    //this is used to get the current running count of the player/dealers hand
    public int currentCount = 0;

    //this is used to start the hand. Starts by dealing out 2 cards
    public void StartHand()
    {
        GetCard();
        GetCard();
    }

    //this method is used for getting the running count
    public int GetCurrentCount()
    {
        return currentCount;
    }

    // Add a hand to the player/dealer's hand
    public int GetCard()
    {
        // Get a card, use deal card to assign sprite and value to card on table
        int cardValue = deckScript.DealCard(hand[cardIndex].GetComponent<CardScript>());

        // Show card on game screen
        hand[cardIndex].GetComponent<Renderer>().enabled = true;

        // Add card value to running total of the hand
        handValue += cardValue;

        //this is used to update the running count
        //checks if the card is supposed to get a value of -1, 0, or 1
        if (cardValue == 1 || cardValue == 10)
        {
            currentCount -= 1;
        }
        else if (cardValue >= 2 && cardValue <= 6)
        {
            currentCount += 1;
        }

        //if the card is a 1, it will add it to the ace index
        if (cardValue == 1)
        {
            aceList.Add(hand[cardIndex].GetComponent<CardScript>());
        }

        //This runs to see if we need to use an 11 or a 1
        AceCheck();
        cardIndex++;
        return handValue;
    }

    // Search for needed ace conversions, 1 to 11 or vice versa
    public void AceCheck()
    {
        // for each ace in the lsit check
        foreach (CardScript ace in aceList)
        {
            if(handValue + 10 < 22 && ace.GetValueOfCard() == 1)
            {
                // if converting, adjust card object value and hand
                ace.SetValue(11);
                handValue += 10;
            } else if (handValue > 21 && ace.GetValueOfCard() == 11)
            {
                // if converting, adjust gameobject value and hand value
                ace.SetValue(1);
                handValue -= 10;
            }
        }
    }

    // Add or subtract from money, for bets
    public void AdjustMoney(int amount)
    {
        money += amount;
    }

    // Output players current money amount
    public int GetMoney()
    {
        return money;
    }

    // Hides all cards, resets the needed variables
    public void ResetHand()
    {
        for(int i = 0; i < hand.Length; i++)
        {
            hand[i].GetComponent<CardScript>().ResetCard();
            hand[i].GetComponent<Renderer>().enabled = false;
        }
        cardIndex = 0;
        handValue = 0;
        aceList = new List<CardScript>();
    }
}
