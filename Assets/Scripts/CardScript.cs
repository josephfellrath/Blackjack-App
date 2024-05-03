using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    // Value of card, 2 of clubs = 2, etc
    public int value = 0;

    //returns the value of the card dealt
    public int GetValueOfCard()
    {
        return value;
    }

    //changes the value of the card (for aces)
    public void SetValue(int newValue)
    {
        value = newValue;
    }

    //gets the value of the card
    public string GetSpriteName()
    {
        return GetComponent<SpriteRenderer>().sprite.name;
    }

    //changes the value of the card face
    public void SetSprite(Sprite newSprite)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
    }

    //resets the value of the card to 0
    public void ResetCard()
    {
        Sprite back = GameObject.Find("Deck").GetComponent<DeckScript>().GetCardBack();
        gameObject.GetComponent<SpriteRenderer>().sprite = back;
        value = 0;
    }
}
