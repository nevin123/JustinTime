using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public bool Landed {internal get; set;}

    public float moveSpeed = 5;

    float velocityX;

    void Update() {
        if(Landed) {
            float newWalkDirection = Random.Range(0f,2f) - 1f;

            transform.Translate(new Vector2(newWalkDirection * moveSpeed * Time.deltaTime, 0));
        }
    }
}
