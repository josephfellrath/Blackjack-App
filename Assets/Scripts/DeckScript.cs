using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class DeckScript : MonoBehaviour
{

    //creating 2 arrays, one for the cards, and the other for the card icons
    public Sprite[] cardSprites;
    int[] cardValues = new int[53];
    int currentIndex = 0;

    void Start()
    {
        //gets current card values in use
        GetCardValues();
    }

    void GetCardValues()
    {
        int num = 0;
        // This loop assigns a value of 1-10 to each card
        for (int i = 0; i < cardSprites.Length; i++)
        {
            num = i;
            // Count up to the amout of cards per suite
            num %= 13;

            // gives 13 cards per hand
            // THis gives all values in between 10-13 the value of 10 (since K, Q, and J are all equal to 10)
            if(num > 10 || num == 0)
            {
                num = 10;
            }
            cardValues[i] = num++;
        }
    }

    public void Shuffle()
    {
        // Swaps cards around within the deck
        for(int i = cardSprites.Length -1; i > 0; --i)
        {
            int j = Mathf.FloorToInt(Random.Range(0.0f, 1.0f) * cardSprites.Length - 1) + 1;
            Sprite face = cardSprites[i];
            cardSprites[i] = cardSprites[j];
            cardSprites[j] = face;

            int value = cardValues[i];
            cardValues[i] = cardValues[j];
            cardValues[j] = value;
        }
        currentIndex = 1;
    }

    //gives cards to players
    public int DealCard(CardScript cardScript)
    {
        cardScript.SetSprite(cardSprites[currentIndex]);
        cardScript.SetValue(cardValues[currentIndex]);
        currentIndex++;
        return cardScript.GetValueOfCard();
    }

    public Sprite GetCardBack()
    {
        return cardSprites[0];
    }
}
