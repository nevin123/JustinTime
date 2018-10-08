using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectThrower : MonoBehaviour
{
    public Throwable throwable;

    void Start() {
        Throw();    
    }
    
    public void Throw() {
        GameObject newThrowable = Instantiate(throwable.gameObject, transform.position, Quaternion.identity);
        newThrowable.GetComponent<Throwable>().Throw(new Vector2(4,3));
    }
}
