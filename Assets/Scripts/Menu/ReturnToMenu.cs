using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReturnToMenu : MonoBehaviour {
    [SerializeField] private Button buttonReturn;
    // Start is called before the first frame update
    void Start () {
        buttonReturn.onClick.AddListener(() => {
            SceneTransitioner.Instance.LoadScene(1);
        });
    }
}
