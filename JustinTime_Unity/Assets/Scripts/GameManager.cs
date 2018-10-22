using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager instance;

    void Awake() {
        if(instance == null) {
            instance = this;
        }    
    }
    #endregion
    
    [Header("World Rules")]
    public float time = 1;
    public float gravity;

    [Header("Score")]
    public int currentStage;
    public int currentScore;

    [Header("Game States")]
    public GameStage[] gamePhases;

    //Hidden
    ObjectThrower[] objectThrowers;

    void Start() {
        objectThrowers = Object.FindObjectsOfType<ObjectThrower>();

        StartThrowing();

        UIManager.instance.SetStageName(gamePhases[0].name);
        UIManager.instance.SetScoreUI(0,true, true);
    }

    #region Score

    public void Catched(Throwable throwable) {
        currentScore += throwable.score;

        UIManager.instance.SetScoreUI(Mathf.Clamp01((float)Mathf.Abs(currentScore)/gamePhases[0].scoreThreshold), Mathf.Sign(currentScore) == 1);

        if(currentScore >= gamePhases[currentStage].scoreThreshold) {
            currentScore = 0;
            currentStage++;
            currentStage = Mathf.Clamp(currentStage, 0, gamePhases.Length);
        }
    } 

    public void Died(Throwable throwable) {
        currentScore -= throwable.penalty;

        UIManager.instance.SetScoreUI(Mathf.Clamp01((float)Mathf.Abs(currentScore)/gamePhases[0].scoreThreshold), Mathf.Sign(currentScore) == 1);

        if(currentScore <= -gamePhases[currentStage].scoreThreshold) {
            GameOver();
        }
    }

    public void GameOver() {
        Debug.Log("GAME OVER !");
    }

    #endregion

    #region Throwing
    public void StartThrowing() {
        StartCoroutine("Throw");
    }

    public void StopThrowing() {
        StopCoroutine("Throw");
    }

    IEnumerator Throw() {
        while(true) {
            yield return new WaitForSeconds(gamePhases[currentStage].throwInterval);

            int throwerIndex = Random.Range(0,objectThrowers.Length);
            objectThrowers[throwerIndex].Throw();
        }
    }
    #endregion
}


[System.Serializable]
public class GameStage {
    public string name;
    public int scoreThreshold;
    public float throwInterval;
}
