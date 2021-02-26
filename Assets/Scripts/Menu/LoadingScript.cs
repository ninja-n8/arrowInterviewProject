using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingScript : MonoBehaviour {

    [SerializeField] private TMP_Text progressText;
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float animSpeed = 0.5f;

    // Start is called before the first frame update
    void Start () {
        canvas.gameObject.SetActive(false);

        SceneTransitioner.OnStartedLoading += () => {
            StartCoroutine(FadeIn());
        };

        SceneTransitioner.OnProgressUpdated += f => {
            progressText.text = $"{(f * 100f)}%";
        };

        SceneTransitioner.OnDone += () => {
            StartCoroutine(FadeOut());
            SceneTransitioner.Finish();
        };
    }

    private IEnumerator FadeIn() {
        canvas.gameObject.SetActive(true);
        var alpha = 0f;
        while (alpha < 1f) {
            canvasGroup.alpha = Mathf.SmoothStep(0f, 1f, alpha);
            alpha += Time.deltaTime * animSpeed;
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    private IEnumerator FadeOut () {
        var alpha = 0f;
        while (alpha < 1f) {
            var a = Mathf.SmoothStep(1f, 0f, alpha);
            canvasGroup.alpha = a;
            alpha += Time.deltaTime * animSpeed;
            yield return null;
        }
        canvasGroup.alpha = 0f;
        canvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {

    }
}
