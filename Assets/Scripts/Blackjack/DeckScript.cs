using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckScript : MonoBehaviour {
    public Sprite[] cardSprites;
    int[] cardValues = new int[53];
    int currentIndex = 0;
    public bool isHilow = false;

    // Start is called before the first frame update
    void Start () {
        GetCardValues();
    }

    void GetCardValues () {
        int num = 0;

        for (int i = 0; i < cardSprites.Length; i++) {
            num = i;
            num %= 13;
            if (!isHilow) {
                if (num > 10 || num == 0) {
                    num = 10;
                }
            } else {
                if (num == 0)
                    num = 13;
            }
            cardValues[i] = num++;
        }
    }

    public void Shuffle() {
        for (int i = cardSprites.Length - 1; i > 0; --i) {
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

    public int DealCard (CardScript cardScript) {
        cardScript.SetSprite(cardSprites[currentIndex]);
        cardScript.SetValue(cardValues[currentIndex++]);
        currentIndex++;
        return cardScript.GetValueOfCard();
    }

    public Sprite GetCardBack() {
        return cardSprites[0];
    }
}
