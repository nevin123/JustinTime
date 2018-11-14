using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultThrower : ObjectThrower
{
    [Header("Settings")]
    public float width = 5f;

    public override void Start() {
        
    }

    public override void Throw() {
        float x = Random.Range(-width/2f, width/2f);

        GameObject newThrowableObject = Instantiate(throwables[0].gameObject, transform.position, Quaternion.identity);
        newThrowableObject.transform.position += new Vector3(x,0,0);

        Throwable throwable = newThrowableObject.GetComponent<Throwable>();
        throwable.Throw(Vector2.zero);
    }

    private void OnDrawGizmos() {
        Gizmos.color = new Color(1,.5f,0,1);
        Gizmos.DrawLine(new Vector3(transform.position.x - width/2f,transform.position.y, transform.position.z),new Vector3(transform.position.x + width/2f,transform.position.y, transform.position.z));
    }
}
