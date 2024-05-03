using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Game Buttons
    public Button dealBtn;
    public Button hitBtn;
    public Button standBtn;
    public Button betBtn;
    public Button runningCntBtn;
    public Button exit;
    public Button settings;
    public Button tutorialBtn;
    public Button perfectBtnEnter;
    public Button perfectBtnExit;
 

    private int standClicks = 0;

    // Access the player and dealer's script
    public PlayerScript playerScript;
    public PlayerScript dealerScript;

    // public Text to access and update - hud
    public Text scoreText;
    public Text dealerScoreText;
    public Text betsText;
    public Text cashText;
    public Text mainText;
    public Text standBtnText;
    public Text runningCount;
    public Text placeHolder;
    public Text inputField;
    public Image image;
    public Text tutorialInput;
    public Text tutorialPlace;
    public Image perfectStrategy;

    public int currentCount = 0;
    public int tempCount = 0;

    // Card hiding dealer's 2nd card
    public GameObject hideCard;
    // How much is bet
    int pot = 0;

    void Start()
    {
        // Add on click listeners to the buttons
        dealBtn.onClick.AddListener(() => DealClicked());
        hitBtn.onClick.AddListener(() => HitClicked());
        standBtn.onClick.AddListener(() => StandClicked());
        betBtn.onClick.AddListener(() => BetClicked());
        runningCntBtn.onClick.AddListener(() => RunClicked());
        exit.onClick.AddListener(() => ExitClicked());
        settings.onClick.AddListener(() => SettingsClicked());
        tutorialBtn.onClick.AddListener(() => TutorialClicked());
        perfectBtnExit.onClick.AddListener(() => PerfectBtnClicked());
        perfectBtnEnter.onClick.AddListener(() => PerfectEnterBtnClicked());
    }

    //this is used to turn off the perfect strategy guide in the corner
    public void PerfectBtnClicked()
    {
        perfectStrategy.gameObject.SetActive(false);
        perfectBtnEnter.gameObject.SetActive(true);
    }

    //this is used to open the perfect strategy guide in the corner
    public void PerfectEnterBtnClicked()
    {
        perfectBtnEnter.gameObject.SetActive(false);
        perfectStrategy.gameObject.SetActive(true);
    }

    //checks whether or not the user has inputed the correct value into the tutorial guide and tells them if they are correct
    public void TutorialClicked()
    {

        //runs if it is correct
        if(int.Parse(tutorialInput.text) == -1)
        {
            tutorialInput.text = "";
            tutorialPlace.text = "Correct";
        }
        //runs if it is not correct
        else
        {
            tutorialInput.text = "";
            tutorialPlace.text = "Incorrect";
        }
    }

    //opens setting screen
    private void SettingsClicked()
    {
        image.gameObject.SetActive(true);
        settings.gameObject.SetActive(false);
    }

    //closes setting screen
    private void ExitClicked()
    {
        image.gameObject.SetActive(false);
        settings.gameObject.SetActive(true);
    }

    //this is used for the card counting checker. Checks if the user inputted the right count, and then updates the count and tells them if they are right or wrong
    private void RunClicked()

    {

        //adds player and dealer count to get the current count and then updates it
        currentCount = playerScript.GetCurrentCount();
        currentCount += dealerScript.GetCurrentCount();
        runningCount.text = currentCount.ToString();

        //converts string into int
        tempCount = int.Parse(inputField.text);

        //checks if user input is correct and then changes placeholder text
        if (tempCount == currentCount)
        {
            placeHolder.text = "\n Correct";
        }
        else
        {
            placeHolder.text = "\n Incorrect";
        }

        //clears input field (not working for some reason)
        inputField.text = "";
    }

    private void DealClicked()
    {
        //this resets the hand at the beginning of each round
        playerScript.ResetHand();
        dealerScript.ResetHand();

        // this hides the dealers card and resets the text popup
        dealerScoreText.gameObject.SetActive(false);
        mainText.gameObject.SetActive(false);
        dealerScoreText.gameObject.SetActive(false);

        //shuffles deck and gives out cards to both player and dealer
        GameObject.Find("Deck").GetComponent<DeckScript>().Shuffle();
        playerScript.StartHand();
        dealerScript.StartHand();

        // Update the scores displayed
        scoreText.text = "Hand: " + playerScript.handValue.ToString();
        dealerScoreText.text = "Hand: " + dealerScript.handValue.ToString();

        // Place card back on dealer card, hide card
        hideCard.GetComponent<Renderer>().enabled = true;

        // allows use to see the hit and stand button and disables to the deal button
        dealBtn.gameObject.SetActive(false);
        hitBtn.gameObject.SetActive(true);
        standBtn.gameObject.SetActive(true);
        standBtnText.text = "Stand";

        // Resets the bet for the player to 40 and takes it from account
        pot = 40;
        betsText.text = "Bets: $" + pot.ToString();
        playerScript.AdjustMoney(-20);
        cashText.text = "$" + playerScript.GetMoney().ToString();

    }

    //this method gives the player another card
    private void HitClicked()
    {
        // Checks that there are less than 10 cards on the table and then gives the player another card
        if (playerScript.cardIndex <= 10)
        {
            //givers player the card and updates the text
            playerScript.GetCard();
            scoreText.text = "Hand: " + playerScript.handValue.ToString();

            //checks if player busts and the finishes the round
            if (playerScript.handValue > 20)
            {
                RoundOver();
            }
        }
    }

    //this method is run when the player clicks the stand button
    private void StandClicked()
    {
        //adds 1 to the count of how many times the stand button has been clicked
        standClicks++;

        //if the player has clicked the button more than once, it will finish the round
        if (standClicks > 1)
        {
            RoundOver();
        }

        //gives the dealer more cards and then changes the button from stand to call
        HitDealer();
        standBtnText.text = "Call";
    }


    //This method is run when the stand button has been clicked
    //It gives the dealer their cards
    private void HitDealer()
    {
        //runs a while loop that sees if the dealers hand is under 16
        while (dealerScript.handValue <= 16 && dealerScript.cardIndex < 10)
        {
            //gives the dealer a card and updates their total
            dealerScript.GetCard();
            dealerScoreText.text = "Hand: " + dealerScript.handValue.ToString();

            //checks if the dealer busts
            if (dealerScript.handValue > 20)
            {
                RoundOver();
            }
        }
    }

    //This method checks for a winner, and if there is one, it will reset the board for the next round
    void RoundOver()
    {
        // Booleans (true/false) for bust and blackjack/21
        bool playerBust = playerScript.handValue > 21;
        bool dealerBust = dealerScript.handValue > 21;
        bool player21 = playerScript.handValue == 21;
        bool dealer21 = dealerScript.handValue == 21;

        // If stand has been clicked less than twice, no 21s or busts, quit function
        if (standClicks < 2 && !playerBust && !dealerBust && !player21 && !dealer21) return;
        bool roundOver = true;

        // All bust, bets returned
        if (playerBust && dealerBust)
        {
            mainText.text = "All Bust: Bets returned";
            playerScript.AdjustMoney(pot / 2);
        }
        // if player busts, dealer didnt, or if dealer has more points, dealer wins
        else if (playerBust || (!dealerBust && dealerScript.handValue > playerScript.handValue))
        {
            mainText.text = "Dealer wins!";
        }
        // if dealer busts, player didnt, or player has more points, player wins
        else if (dealerBust || playerScript.handValue > dealerScript.handValue)
        {
            mainText.text = "You win!";
            playerScript.AdjustMoney(pot);
        }
        //Check for tie, return bets
        else if (playerScript.handValue == dealerScript.handValue)
        {
            mainText.text = "Push: Bets returned";
            playerScript.AdjustMoney(pot / 2);
        }
        else
        {
            roundOver = false;
        }

        if (roundOver)
        {
            //activates/deactivates buttons and text. Resets the amount of times the stand button has been clicked as well
            hitBtn.gameObject.SetActive(false);
            standBtn.gameObject.SetActive(false);
            dealBtn.gameObject.SetActive(true);
            mainText.gameObject.SetActive(true);
            dealerScoreText.gameObject.SetActive(true);
            hideCard.GetComponent<Renderer>().enabled = false;
            cashText.text = "$" + playerScript.GetMoney().ToString();
            standClicks = 0;
        }
    }

    //When the bet button is clicked, this method adds money into the pot
    void BetClicked()
    { 
        Text newBet = betBtn.GetComponentInChildren(typeof(Text)) as Text;
        int intBet = int.Parse(newBet.text.ToString().Remove(0, 1));
        playerScript.AdjustMoney(-intBet);
        cashText.text = "$" + playerScript.GetMoney().ToString();
        pot += (intBet * 2);
        betsText.text = "Bets: $" + pot.ToString();
    }
}
