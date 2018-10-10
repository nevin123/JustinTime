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

    [Header("Game States")]
    public GamePhase[] gamePhases;

    //Hidden
    ObjectThrower[] objectThrowers;

    void Start() {
        objectThrowers = Object.FindObjectsOfType<ObjectThrower>();

        StartThrowing();
    }

    public void StartThrowing() {
        StartCoroutine("Throw");
    }

    public void StopThrowing() {
        StopCoroutine("Throw");
    }

    IEnumerator Throw() {
        while(true) {
            yield return new WaitForSeconds(gamePhases[0].throwInterval);

            int throwerIndex = Random.Range(0,objectThrowers.Length);
            objectThrowers[throwerIndex].Throw();
        }
    }
}

[System.Serializable]
public class GamePhase {
    public float throwInterval;
}
