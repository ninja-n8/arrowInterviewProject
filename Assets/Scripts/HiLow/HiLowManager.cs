using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiLowManager : MonoBehaviour {
    public Button dealBtn;
    public Button lowerBtn;
    public Button higherBtn;
    public Button betBtn;

    private int standClicks = 0;

    public PlayerScript playerScript;
    public PlayerScript dealerScript;

    public Text betsText;
    public Text cashText;
    public Text mainText;
    public Text higherBtnText;

    public GameObject hideCard;
    int pot = 0;

    bool playerGuessHigh;

    // Start is called before the first frame update
    void Start () {
        dealBtn.onClick.AddListener(() => DealClicked());
        lowerBtn.onClick.AddListener(() => LowerClicked());
        higherBtn.onClick.AddListener(() => HigherClicked());
        betBtn.onClick.AddListener(() => BetClicked());
    }

    private void DealClicked () {
        dealerScript.ResetHand();

        mainText.gameObject.SetActive(false);
        GameObject.Find("Deck").GetComponent<DeckScript>().Shuffle();
        dealerScript.StartHand();

        hideCard.GetComponent<Renderer>().enabled = true;
        Debug.Log("Dealer's Cards: " + dealerScript.hand[0].GetComponent<CardScript>().GetValueOfCard().ToString() + " " + dealerScript.hand[1].GetComponent<CardScript>().GetValueOfCard().ToString());

        dealBtn.gameObject.SetActive(false);
        lowerBtn.gameObject.SetActive(true);
        higherBtn.gameObject.SetActive(true);

        pot = 40;
        betsText.text = "Bets: $" + pot.ToString();
        playerScript.AdjustMoney(-20);
        cashText.text = "$" + playerScript.GetMoney().ToString();
    }
    private void LowerClicked () {
        playerGuessHigh = false;
        RoundOver();
    }

    private void HigherClicked () {
        playerGuessHigh = true;
        RoundOver();
    }

    private void HitDealer () {
        while (dealerScript.handValue < 16 && dealerScript.cardIndex < 10) {
            dealerScript.GetCard();
            if (dealerScript.handValue > 20)
                RoundOver();
        }
    }

    void RoundOver () {
        int hiddenCardValue = dealerScript.hand[0].GetComponent<CardScript>().GetValueOfCard();
        int shownCardValue = dealerScript.hand[1].GetComponent<CardScript>().GetValueOfCard();

        for (int i = 0; i < dealerScript.hand.Length; i++) {
            if (dealerScript.hand[i].GetComponent<CardScript>().GetValueOfCard() == 0)
                dealerScript.hand[i].GetComponent<CardScript>().SetValue(13);
        }

        bool roundOver = true;

        if (playerGuessHigh) {
            if (hiddenCardValue > shownCardValue) {
                mainText.text = "You win!";
                playerScript.AdjustMoney(pot);
            } else if (hiddenCardValue < shownCardValue) {
                mainText.text = "Dealer wins!";
            } else if (hiddenCardValue == shownCardValue) {
                mainText.text = "Bust: Bets returned";
                playerScript.AdjustMoney(pot / 2);
            } else {
                roundOver = false;
            }
        } else {
            if (hiddenCardValue < shownCardValue) {
                mainText.text = "You win!";
                playerScript.AdjustMoney(pot);
            } else if (hiddenCardValue > shownCardValue) {
                mainText.text = "Dealer wins!";
            } else if (hiddenCardValue == shownCardValue) {
                mainText.text = "Bust: Bets returned";
                playerScript.AdjustMoney(pot / 2);
            } else {
                roundOver = false;
            }
        }

        if (roundOver) {
            lowerBtn.gameObject.SetActive(false);
            higherBtn.gameObject.SetActive(false);
            dealBtn.gameObject.SetActive(true);
            mainText.gameObject.SetActive(true);
            hideCard.GetComponent<Renderer>().enabled = false;
            cashText.text = "$" + playerScript.GetMoney().ToString();
            standClicks = 0;
        }
    }

    void BetClicked () {
        Text newBet = betBtn.GetComponentInChildren(typeof(Text)) as Text;
        int intBet = int.Parse(newBet.text.ToString());
        Debug.Log("Pot: " + pot + " intBet: " + intBet);
        playerScript.AdjustMoney(-intBet);
        cashText.text = "$" + playerScript.GetMoney().ToString();
        pot += (intBet * 2);
        betsText.text = "Bets: $" + pot.ToString();
        Debug.Log("Pot: " + pot + " intBet: " + intBet);
    }
}
