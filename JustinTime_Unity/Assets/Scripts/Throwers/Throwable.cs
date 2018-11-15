using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{  
    SpriteRenderer sprite;
    PlayerMotor motor;

    Vector2 velocity;

    [Header("Graphics")]
    public ParticleSystem blood;

    [Header("Throwing")]
    public Vector2 minThrowVelocity;
    public Vector2 maxThrowVelocity;

    [Header("Scores")]
    public int score = 1;
    public int penalty = 1;

    [Header("Physics")]
    float gravity;
    
    public float maxFallingSpeed = 5;
    float horizontalDrag = 2;

    [HideInInspector]
    public bool isCatched = false;
    [HideInInspector]
    public bool isDead = false;

    bool isLanded = false;
    bool isThrown = false;
    bool applyPhysics = true;

    void Start() {
        motor = GetComponent<PlayerMotor>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        gravity = GameManager.instance.gravity;
    }

    public void Throw(Vector2 velocity) {
        isThrown = true;
        this.velocity = velocity;

        Telegrapher.instance.AddNewObjectToFollow(gameObject);
    }

    void Update() {
        if(isThrown && applyPhysics) {
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
            float velocityXDrag = -Mathf.Sign(velocity.x) * Mathf.Clamp01(Mathf.Abs(velocity.x)) * ((motor.collisions.below == true)?horizontalDrag*50f:horizontalDrag);
            velocity.x += velocityXDrag * Time.deltaTime;
        }

        if(motor.collisions.below || motor.collisions.above) {
            velocity.y = 0;

            if(motor.collisions.below && !isLanded) {
                isLanded = true;

                if(isCatched) {
                    //Landed safely
                    GameManager.instance.Catched(this);
                    GetComponent<AI>().Landed = true;
                } else {
                    //Died
                    GameManager.instance.Died(this);
                    Die();
                }
            }
        }
    }

    public void SetVelocity(Vector2 velocity) {
        this.velocity = velocity;
    }

    public void Catch() {
        isCatched = true;
        TogglePhysics(false);
    }

    public void Die() {
        TogglePhysics(false);
        isDead = true;
        sprite.enabled = false;

        //Die animation
        blood.Play();

        GameObject.Destroy(gameObject, 10);
    }

    public void TogglePhysics(bool tf) {
        applyPhysics = tf;
        GetComponent<BoxCollider2D>().enabled = tf;
    }
}
