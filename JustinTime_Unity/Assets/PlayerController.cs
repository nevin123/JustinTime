using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {
    
    PlayerMotor motor;
    
    Vector2 velocity;

    public float gravity = -9.81f;
    public float horizontalDrag = 5;

    void Start() {
        motor = GetComponent<PlayerMotor>();
    }

    void Update() {
        velocity.y += gravity * Time.deltaTime;
        velocity.x = Input.GetAxisRaw("Horizontal") * Time.deltaTime;

        if(motor.collisions.below || motor.collisions.above) {
            velocity.y = 0;
        }

        motor.Move(velocity, 0f);
    }
}