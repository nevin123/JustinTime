using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalThrower : ObjectThrower
{
    int faceDir;
    
    public override void Start()
    {
        faceDir = (int)Mathf.Sign(-transform.position.x);
    }

    public override void Throw() {
        GameObject newThrowableObject = Instantiate(throwables[0].gameObject, transform.position, Quaternion.identity);
        Throwable throwable = newThrowableObject.GetComponent<Throwable>();

        Vector2 throwVelocity = Vector2.Lerp(throwable.minThrowVelocity, throwable.maxThrowVelocity, Random.Range(0f,1f));
        throwVelocity.x *= faceDir;

        throwable.Throw(throwVelocity);
    }
}
