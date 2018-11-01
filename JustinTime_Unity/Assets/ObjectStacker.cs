using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStacker : MonoBehaviour
{   
    public Transform stackParant;
    public List<Transform> objectsToStack;
    
    void Start() {
        objectsToStack = new List<Transform>();
    }

    void Update() {
        for(int i = 0; i < objectsToStack.Count; i++) {
            objectsToStack[i].transform.localPosition = Vector3.Lerp(objectsToStack[i].transform.localPosition, Vector2.zero + Vector2.up * (objectsToStack.Count - i - 1) * 0.5f, Time.deltaTime * 10);
        }
    }

    public void AddObject(GameObject newObject) {
        objectsToStack.Add(newObject.transform);
        newObject.transform.SetParent(stackParant,true);
    }

    public void ReleaseAllObjects(Vector2 velocity) {
        for(int i = 0; i < objectsToStack.Count; i++) {
            objectsToStack[i].SetParent(null,true);
            
            if(objectsToStack[i].GetComponent<Throwable>() != null) {
                Throwable throwable = objectsToStack[i].GetComponent<Throwable>();
                throwable.TogglePhysics(true);
                throwable.SetVelocity(velocity);
            }

            objectsToStack.Remove(objectsToStack[i]);
        }
    }
}
