using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectThrower : MonoBehaviour
{
    public Throwable throwables;
    int faceDir;

    void Start() {
        faceDir = (int)Mathf.Sign(-transform.position.x);
    }
    
    public void Throw() {
        GameObject newThrowableObject = Instantiate(throwables.gameObject, transform.position, Quaternion.identity);
        Throwable throwable = newThrowableObject.GetComponent<Throwable>();

        Vector2 throwVelocity = Vector2.Lerp(throwable.minThrowVelocity, throwable.maxThrowVelocity, Random.Range(0f,1f));
        throwVelocity.x *= faceDir;

        throwable.Throw(throwVelocity);
    }
}
