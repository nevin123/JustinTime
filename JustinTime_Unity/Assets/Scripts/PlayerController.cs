using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {
    
    PlayerMotor motor;
    
    Vector2 velocity;

    ParticleSystem[] dustParticles;
    public ScreenShake screenShakeController;
    public Transform cape;

    [Header("Physics")]
    public float gravity = -9.81f;
    public float horizontalDrag = -5;
    public float groundDragMultiplier = 5;

    [Header("Catching")]
    public Transform handTransform;
    [Range(0.2f, 3)]
    public float catchRadius = 1;
    public float flyCatchMultimlier = 1.5f;
    public bool releaseOnGrounded = true;
    public float dropAfterTime;
    float dropTimer = 0;
    float armRadius;
    ObjectStacker stacker;

    [Header("charging")]
    public Vector2 minChargeVelocity;
    public Vector2 maxChargeVelocity;
    public float maxChargeTime;

    public AnimationCurve chargePower;

    bool isCharging = false;
    int chargeDirection = 0;
    int currentInputDirection = 0;
    float chargeStrength = 0;
    float chargeTimer = 0;
    
    bool notgrounded = true;
    float lastRandomShake = 0;
    bool doRandomShake = true;

    void Start() {
        stacker = GetComponent<ObjectStacker>();
        motor = GetComponent<PlayerMotor>();
        dustParticles = GetComponentsInChildren<ParticleSystem>();
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
            
            //Charge shake
            if(doRandomShake == false && (chargeTimer - lastRandomShake)>0) {
                doRandomShake = true;
                lastRandomShake += 0.2f;
            } 
            if(doRandomShake) {
                screenShakeController.ShakeRandom(chargeTimer/maxChargeTime * 0.03f, 0.2f);
                doRandomShake = false;
            }

            //Do Charge after the max charge time is complete
            if(chargeTimer >= maxChargeTime) {
                StartCoroutine("Charge");
            }
        }

        //Move
        motor.Move(velocity * Time.deltaTime);

        //Catch
        armRadius = Mathf.Lerp(catchRadius, catchRadius * flyCatchMultimlier, Mathf.Clamp01(velocity.magnitude/5));
        CheckCatches();

        //Apply Physics
        velocity.y += gravity * Time.deltaTime;
        
        if(motor.collisions.left || motor.collisions.right) {
            velocity.x = 0;
        } else {
            float velocityXDrag = -Mathf.Sign(velocity.x) * Mathf.Clamp01(Mathf.Abs(velocity.x)) * ((motor.collisions.below == true)?horizontalDrag*groundDragMultiplier:horizontalDrag);
            velocity.x += velocityXDrag * Time.deltaTime;
        }

        //Colliding with ground
        if(motor.collisions.below || motor.collisions.above) {
            Debug.Log("grounded");

            if(notgrounded) {
                notgrounded = false;
                Debug.Log("grounded shake : " + velocity.magnitude);
                screenShakeController.ShakeZoom(0.99f, 0.1f);
            }
            
            cape.rotation = Quaternion.Lerp(cape.rotation, Quaternion.identity, Time.deltaTime * 5f);
            
            velocity.y = 0;

            //particles
            foreach(ParticleSystem dust in dustParticles) {
                var em = dust.emission;
                em.enabled = true;
            }

        } else {
            Debug.Log("not grounded");

            Debug.Log(velocity);
            

            cape.rotation = Quaternion.Lerp(cape.rotation, Quaternion.Euler(0,0,-Mathf.Sign(velocity.x) * Vector2.Angle(Vector2.down,velocity)), Time.deltaTime * velocity.magnitude*0.3f);

            foreach(ParticleSystem dust in dustParticles) {
                var em = dust.emission;
                em.enabled = false;
            }

            notgrounded = true;
        }

        //Grounded
        if(velocity.y < 0.01f) {
            if(releaseOnGrounded) {
                dropTimer += Time.deltaTime;
                
                if(dropTimer > dropAfterTime) {
                    stacker.ReleaseAllObjects(velocity);
                }
            }
        } else {
            dropTimer = 0;
            
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
                
                //calculate charge velocity
                Vector2 newVelocity = Vector2.Lerp(minChargeVelocity, maxChargeVelocity, chargePower.Evaluate(chargeTimer/maxChargeTime));
                newVelocity.x *= chargeDirection;
                
                //apply velocity and do screenshake
                velocity = newVelocity;
                screenShakeController.ShakeToDirection(newVelocity.normalized * -1f, newVelocity.magnitude * 0.02f, 1f*newVelocity.magnitude);

                //Charge info reset
                chargeTimer = 0;
                isCharging = false;
                chargeDirection = 0;
                currentInputDirection = 0;
                chargeStrength = 0;

                //Charge shake reset
                doRandomShake = true;
                lastRandomShake = 0;
            }

            yield return null;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(handTransform.position, armRadius);
    }

    void CheckCatches() {
        Throwable[] allThrowables;
        allThrowables = Object.FindObjectsOfType<Throwable>();

        foreach(Throwable throwable in allThrowables) {
            if(Vector2.Distance(transform.position, throwable.transform.position) < armRadius && !throwable.isCatched && !throwable.isDead) {
                throwable.Catch();
                stacker.AddObject(throwable.gameObject);
            }
        }
    }
}