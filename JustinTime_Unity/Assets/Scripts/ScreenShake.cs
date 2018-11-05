using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScreenShake : MonoBehaviour
{
    Camera cam;
    float orthographicSize;

    private void Start() {
        cam = Camera.main; 
        orthographicSize = cam.orthographicSize;   
    }

    public void ShakeToDirection(Vector2 dir, float intensity, float speed) {
        StartCoroutine(ShakeToDirectionAnimation(dir,intensity,speed));
    }

    public void ShakeZoom(float zoomAmount, float speed) {
        this.cam.DOOrthoSize(orthographicSize * zoomAmount, speed * 0.6f).OnComplete(()=>{
            this.cam.DOOrthoSize(orthographicSize, speed * 0.4f);
        });
    }

    public void ShakeRandom(float intensity, float duration){
        cam.DOShakePosition(duration, intensity, 10, 30, false);
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
}
