using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public void ShakeToDirection(Vector2 dir, float intensity, float speed) {
        StartCoroutine(ShakeToDirectionAnimation(dir,intensity,speed));
    }

    public void ShakeZoom(float zoomAmount, float speed) {
        StartCoroutine(ShakeZoomAnimation(zoomAmount, speed));
    }

    public void ShakeRandom(float intensity, float duration){

    }

    IEnumerator ShakeToDirectionAnimation(Vector2 dir, float intensity, float speed) {
        bool done = false;
        bool start = true;

        intensity = Mathf.Clamp(intensity, 0.13f, float.MaxValue);
        
        while(done == false) {
            if(start) {
                transform.position = Vector2.Lerp(transform.position, dir * intensity, speed * Time.deltaTime);
                if(Vector2.Distance(transform.position, dir * intensity) < 0.1f) {
                    start = false;
                }
            } else {
                transform.position = Vector2.Lerp(transform.position, Vector2.zero, speed * Time.deltaTime);
                if(Vector2.Distance(transform.position, Vector2.zero)<0.01f) {
                    transform.position = Vector2.zero;
                    done = true;
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator ShakeZoomAnimation(float zoomAmount, float speed) {
        Debug.Log("start shake");
        bool done = false;
        bool start = true;
        
        while(done == false) {
            if(start) {
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one*zoomAmount, speed * Time.deltaTime);
                if(Mathf.Abs(transform.localScale.x - 1f*zoomAmount) < 0.03f) {
                    start = false;
                }
            } else {
                transform.localScale = Vector3.Lerp(transform.localScale, Vector2.one, speed * Time.deltaTime);
                if(Mathf.Abs(transform.localScale.x - 1f) < 0.03f) {
                    transform.localScale = Vector3.one;
                    done = false;
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
