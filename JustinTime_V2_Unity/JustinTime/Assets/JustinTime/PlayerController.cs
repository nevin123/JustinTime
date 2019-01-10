using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Vector2 minJumpForce;
    Rigidbody2D rb;

    Vector2 velocity = Vector2.zero;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        //TMP: Input
        if(Input.GetKeyDown(KeyCode.A)) { //To the left
            velocity = new Vector2(minJumpForce.x, minJumpForce.y);    

        }
        if(Input.GetKeyDown(KeyCode.D)) { //To the right
            velocity = new Vector2(minJumpForce.x * -1, minJumpForce.y);    
        }
    }

    void FixedUpdate() {
        if(velocity != Vector2.zero) {
            rb.AddForce(velocity, ForceMode2D.Impulse);
            velocity = Vector2.zero;
        }
        
    }

    void isGrounded() {

    }

    //Check if the player is standing on the ground
    // void CheckIfGrounded() {
    //     isGrounded = false;
    //     Vector2 playerBottom = (Vector2)transform.position;

    //     for(int i = 0; i < rayCount; i++) {
    //         float x = -feetWidth/2f + (float)feetWidth/(rayCount-1)*i;
    //         RaycastHit2D hit;
    //         hit = Physics2D.Raycast(playerBottom + new Vector2(x, -skinWidth), -transform.up, skinWidth * 2, ground);
    //         if(hit) {
    //             isGrounded = true;

    //             if(collidingWith != hit.collider.gameObject) {
    //                 if(hit.collider.gameObject.GetComponent<MovingChild>()) {
    //                     movingParent = hit.collider.gameObject.GetComponent<MovingChild>().GetParent;
    //                     movingParent.AddPlayer(this);
    //                 }
    //             }

    //             collidingWith = hit.collider.gameObject;
    //         }
    //     }

    //     if(isGrounded == false) {
    //         collidingWith = null;

    //         if(movingParent != null) {
    //             movingParent.RemovePlayer(this);
    //             movingParent = null;
    //         }
    //     }
    // }
}
