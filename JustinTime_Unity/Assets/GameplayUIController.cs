using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIController : MonoBehaviour
{
    public TextMeshProUGUI stageTitle;
    public Image progressBar;
    public Image gameOverProgressBar;

    public float progressBarSmoothing = 0.5f;

    float scorePercentage = 0;
    float scorePercentageVel;
    float negativeScorePercentage = 0;
    float negativeScorePercentageVel;

    private void Update() {
        progressBar.fillAmount = Mathf.SmoothDamp(progressBar.fillAmount, scorePercentage, ref scorePercentageVel, progressBarSmoothing, 1);
        gameOverProgressBar.fillAmount = Mathf.SmoothDamp(gameOverProgressBar.fillAmount, negativeScorePercentage, ref negativeScorePercentageVel, progressBarSmoothing, 1);

        Debug.Log(progressBar.fillAmount);
        if(progressBar.fillAmount >= 0.98f) {
            //ProgressBar Full
            Debug.Log("Dsadsadas");
            progressBar.fillAmount = 0;
            scorePercentage = 0;
            scorePercentageVel = 0;
        }

        if(gameOverProgressBar.fillAmount >= 0.98f) {
            //GameOverBar Full
            // Do gameover effect
        }
    }

    public void SetScorePercentage(float percentage, bool positive, bool instant = false) {
        if(positive) {
            scorePercentage = Mathf.Clamp01(percentage);
            negativeScorePercentage = 0;
        } else {
            negativeScorePercentage = Mathf.Clamp01(percentage);
            scorePercentage = 0;
        }
        
        if(instant) {
            progressBar.fillAmount = percentage;
            scorePercentage = percentage;
            scorePercentageVel = 0;
            gameOverProgressBar.fillAmount = percentage;
            negativeScorePercentage = percentage;
            negativeScorePercentageVel = 0;
        }
    }

    public void SetStageName(string name) {
        stageTitle.text = name;
    }
}
