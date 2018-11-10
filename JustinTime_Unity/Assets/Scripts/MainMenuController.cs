using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    Canvas canvas;

    void Start() {
        canvas = GetComponent<Canvas>();
    }

    public void Show() {
        canvas.enabled = true;
    }

    public void Hide() {
        canvas.enabled = false;
    }

    public void StartGame() {
        GameManager.instance.StartGame();
    }
}
