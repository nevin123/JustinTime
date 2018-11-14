using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telegrapher : MonoBehaviour
{
    public GameObject telegraphPrefab;
    public TelegraphObject[] telegraphedObjects;
}

[System.Serializable]
public class TelegraphObject {
    [TagSelector]
    public string[] Tags;

    public Sprite alertSprite;
    public Color alertColor = new Color(1,1,1,1);
}
