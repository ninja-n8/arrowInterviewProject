using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game1Script : MonoBehaviour {
    [SerializeField] private Button buttonHiLow;
    [SerializeField] private Button buttonBlackJack;
    [SerializeField] private Button buttonFiveCard;
    [SerializeField] private Button buttonSevenCard;

    // Start is called before the first frame update
    void Start () {
        buttonHiLow.onClick.AddListener(() => {
            SceneTransitioner.Instance.LoadScene(2);
        });
        buttonBlackJack.onClick.AddListener(() => {
            SceneTransitioner.Instance.LoadScene(3);
        });
        buttonFiveCard.onClick.AddListener(() => {
            SceneTransitioner.Instance.LoadScene(4);
        });
        buttonSevenCard.onClick.AddListener(() => {
            SceneTransitioner.Instance.LoadScene(5);
        });
    }
}
