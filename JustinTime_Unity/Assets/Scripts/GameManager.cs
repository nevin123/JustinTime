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

    public GameState gameState;

    UIManager UI;

    //Hidden
    ObjectThrower[] objectThrowers;

    void Start() {
        UI = UIManager.instance;
        objectThrowers = Object.FindObjectsOfType<ObjectThrower>();

        UI.SetStageName(gamePhases[0].name);
        UI.SetScoreUI(0,true, true);
    }

    #region Score

    public void Catched(Throwable throwable) {
        currentScore += throwable.score;

        UI.SetScoreUI(Mathf.Clamp01((float)Mathf.Abs(currentScore)/gamePhases[currentStage].scoreThreshold), Mathf.Sign(currentScore) == 1);

        if(currentScore >= gamePhases[currentStage].scoreThreshold) {
            //Next stage
            currentScore = 0;
            currentStage++;
            currentStage = Mathf.Clamp(currentStage, 0, gamePhases.Length-1);
            UI.SetStageName(gamePhases[currentStage].name);
        }
    } 

    public void Died(Throwable throwable) {
        currentScore -= throwable.penalty;

        UI.SetScoreUI(Mathf.Clamp01((float)Mathf.Abs(currentScore)/gamePhases[currentStage].scoreThreshold), Mathf.Sign(currentScore) == 1);

        if(currentScore <= -gamePhases[currentStage].scoreThreshold) {
            GameOver();
        }
    }

    public void GameOver() {
        Debug.Log("GAME OVER !");

        StopCoroutine("Throw");
    }

    #endregion

    #region Throwing
    public void StartGame() {
        //Reset Score
        UI.SetStageName(gamePhases[0].name);
        UI.SetScoreUI(0,true, true);

        //Hide Main Menu
        UI.ShowHideMainMenu(false);

        StartCoroutine("Throw");
    }

    public void PauseGame() {

    }

    public void StopGame() {
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

public enum GameState {
    InMainMenu,
    Playing,
    Paused,
    GameOver
}
