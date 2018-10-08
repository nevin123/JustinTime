using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{  
    PlayerMotor motor;

    Vector2 velocity;

    float gravity;
    
    public float maxFallingSpeed = 5;
    float horizontalDrag = 2;

    bool isThrown = false;

    void Start() {
        motor = GetComponent<PlayerMotor>();
        gravity = GameManager.instance.gravity;
    }

    public void Throw(Vector2 velocity) {
        isThrown = true;
        this.velocity = velocity;
    }

    void Update() {
        if(isThrown) {
            //Move
            motor.Move(velocity * Time.deltaTime);

            //Apply Physics
            velocity.y += gravity * Time.deltaTime;
            velocity.y = Mathf.Clamp(velocity.y, -maxFallingSpeed, int.MaxValue);
            
            if(motor.collisions.left || motor.collisions.right) {
                velocity.x = 0;
            } else {
                float velocityXDrag = -Mathf.Sign(velocity.x) * Mathf.Clamp01(Mathf.Abs(velocity.x)) * horizontalDrag;
                velocity.x += velocityXDrag * Time.deltaTime;
            }

            if(motor.collisions.below || motor.collisions.above) {
                velocity.y = 0;
            }
        }
    }
}
