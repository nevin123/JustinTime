using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectThrower : MonoBehaviour
{
    public Throwable[] throwables;

    public virtual void Start() {
        
    }
    
    public virtual void Throw() {
        GameObject newThrowableObject = Instantiate(throwables[0].gameObject, transform.position, Quaternion.identity);
        Throwable throwable = newThrowableObject.GetComponent<Throwable>();

        throwable.Throw(Vector2.zero);
    }
}
