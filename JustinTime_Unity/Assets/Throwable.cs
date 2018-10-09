using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{  
    PlayerMotor motor;

    Vector2 velocity;

    [Header("Graphics")]
    public Sprite deadImage;
    public GameObject pixel;
    public ParticleSystem blood;

    [Header("Throwing")]
    public Vector2 minThrowVelocity;
    public Vector2 maxThrowVelocity;

    float gravity;
    
    public float maxFallingSpeed = 5;
    float horizontalDrag = 2;

    bool isThrown = false;
    bool hitTheGround = false;

    void Start() {
        motor = GetComponent<PlayerMotor>();
        gravity = GameManager.instance.gravity;
    }

    public void Throw(Vector2 velocity) {
        isThrown = true;
        this.velocity = velocity;
    }

    void Update() {
        if(isThrown && !hitTheGround) {
            ApplyPhysics();
        }
    }

    void ApplyPhysics() {
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

            if(motor.collisions.below) {
                Die();
            }
        }
    }

    void Die() {
        hitTheGround = true;
        motor.enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponentInChildren<SpriteRenderer>().enabled = false;

       //Die animation
       blood.Play();
    }
}
