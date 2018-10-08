using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {
    
    PlayerMotor motor;
    
    Vector2 velocity;

    public float gravity = -9.81f;
    public float horizontalDrag = -5;
    public float groundDragMultiplier = 5;

    [Header("charging")]
    public Vector2 minChargeVelocity;
    public Vector2 maxChargeVelocity;
    public float maxChargeTime;

    bool isCharging = false;
    int chargeDirection = 0;
    int currentInputDirection = 0;
    float chargeStrength = 0;
    float chargeTimer = 0;

    void Start() {
        motor = GetComponent<PlayerMotor>();
    }

    void Update() {
        //Charge Input
        if(Input.GetKeyDown(KeyCode.A)) {
            InputDown(-1);
        }

        if(Input.GetKeyDown(KeyCode.D)) {
            InputDown(1);
        }

        if(Input.GetKeyUp(KeyCode.A)) {
            InputUp(-1);
        }

        if(Input.GetKeyUp(KeyCode.D)) {
            InputUp(1);
        }
        
        //Do Charge
        if(isCharging) {
            chargeTimer += Time.deltaTime;
            
            if(chargeTimer >= maxChargeTime) {
                StartCoroutine("Charge");
            }
        }

        //Move
        motor.Move(velocity * Time.deltaTime);

        //Apply Physics
        velocity.y += gravity * Time.deltaTime;
        
        if(motor.collisions.left || motor.collisions.right) {
            velocity.x = 0;
        } else {
            float velocityXDrag = -Mathf.Sign(velocity.x) * Mathf.Clamp01(Mathf.Abs(velocity.x)) * ((motor.collisions.below == true)?horizontalDrag*groundDragMultiplier:horizontalDrag);
            velocity.x += velocityXDrag * Time.deltaTime;
        }

        if(motor.collisions.below || motor.collisions.above) {
            velocity.y = 0;
        }
    }

    public void InputDown(int input) {
        if(isCharging || chargeTimer != 0) {
            return;
        }

        //To the right
        if(input > 0) {
            chargeDirection = 1;
            currentInputDirection = 1;
        }
        //To the left
        if(input < 0) {
            chargeDirection = -1;
            currentInputDirection = -1;
        }

        isCharging = true;
    }

    public void InputUp(int input) {
        if((isCharging || chargeTimer != 0) && currentInputDirection == input) {
            StartCoroutine("Charge");
        }
    }

    IEnumerator Charge() {        
        while(isCharging == true) {
            if(motor.collisions.below) {
                Vector2 newVelocity = Vector2.Lerp(minChargeVelocity, maxChargeVelocity, chargeTimer/maxChargeTime);
                newVelocity.x *= chargeDirection;
                
                velocity = newVelocity;
                
                chargeTimer = 0;
                isCharging = false;
                chargeDirection = 0;
                currentInputDirection = 0;
                chargeStrength = 0;
            }

            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other.gameObject.name);    
    }
}