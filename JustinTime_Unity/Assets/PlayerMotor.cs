﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : RaycastController
{
    public CollisionInfo collisions;

    public void Move(Vector2 moveAmount, float input) {
        UpdateRaycastOrigins();
        collisions.Reset();

        HorizontalCollisions(ref moveAmount);

        if(moveAmount.y != 0) {
            VerticalCollisions(ref moveAmount);
        }

        transform.Translate(moveAmount);
    }

    void VerticalCollisions(ref Vector2 moveAmount) {
        float directionY = Mathf.Sign(moveAmount.y);
        float rayLength = Mathf.Abs(moveAmount.y) + skinWidth;

        for(int i = 0; i < verticalRayCount; i++) {
            Vector2 rayOrigin = (directionY == -1)?raycastOrigins.bottomLeft:raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + moveAmount.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

            if(hit) {
                moveAmount.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            } 
        }
    }

    void HorizontalCollisions(ref Vector2 moveAmount) {
        float directionX = Mathf.Sign(moveAmount.x);
        float rayLength = Mathf.Abs(moveAmount.x) + skinWidth;

        if(Mathf.Abs(moveAmount.x) < skinWidth) {
            rayLength = skinWidth * 2;
        }

        Debug.Log("horizontalRays: " + directionX);

        for(int i = 0; i < horizontalRayCount; i++) {
            Vector2 rayOrigin = (directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

            if(hit) {
                moveAmount.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;

                collisions.left = directionX == -1;
                collisions.right = directionX == 1;
            }
        }
    }

    public struct CollisionInfo {
        public bool above, below;
        public bool left, right;

        public int faceDir;

        public void Reset() {
            above = below = false;
            left = right = false;
        }
    }
}