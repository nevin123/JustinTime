using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager instance = null;

    private void Awake() {
        if(instance == null) {
            instance = this;
        }    
    }
    #endregion

    public GameplayUIController gameplayMenuController;

    public void SetScoreUI (float percentage, bool positive, bool instant = false) {
        gameplayMenuController.SetScorePercentage(percentage, positive, instant);
    }

    public void SetStageName(string name) {
        gameplayMenuController.SetStageName(name);
    }
}
