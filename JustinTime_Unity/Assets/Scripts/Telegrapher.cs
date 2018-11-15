using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Telegrapher : MonoBehaviour
{
    #region Singleton
    public static Telegrapher instance;

    private void Awake() {
        if(instance == null) {
            instance = this;
        }    
    }
    #endregion

    public GameObject telegraphPrefab;
    public TelegraphSettings[] telegraphSettings;

    List<ObjectToTelegraph> objectsToTelegraph;

    private void Start() {
        objectsToTelegraph = new List<ObjectToTelegraph>();    
    }

    private void Update() {
        foreach(ObjectToTelegraph telegrapher in objectsToTelegraph) {
            telegrapher.UpdateTelegrapher();
        }
    }

    public void AddNewObjectToFollow(GameObject newObject) {
        foreach(TelegraphSettings telegraphSettings in telegraphSettings) {
            foreach(string tag in telegraphSettings.Tags) {
                if(newObject.tag == tag) {
                    objectsToTelegraph.Add(new ObjectToTelegraph(newObject, Instantiate(telegraphPrefab, transform), telegraphSettings));
                    return;
                }
            }
        }
    }
}

// Sets the settings for which objects to telegraph
[System.Serializable]
public class TelegraphSettings {
    [TagSelector]
    public string[] Tags;

    public Sprite alertSprite;
    public Color alertColor = new Color(1,1,1,1);
}

// Makes a telegrapher follow a gameObject
public class ObjectToTelegraph {
    //Setted variables
    GameObject objectToTelegraph;
    GameObject telegraphPrefab;
    TelegraphSettings settings;

    //Found variables
    Image indicator;
    Image icon;

    Camera cam = Camera.main;

    RectTransform rect;

    bool update = true;
    
    public ObjectToTelegraph(GameObject objectToTelegraph,GameObject telegraphPrefab, TelegraphSettings settings) {
        this.objectToTelegraph = objectToTelegraph;
        this.telegraphPrefab = telegraphPrefab;
        this.settings = settings;

        Image[] imagesInChildren = this.telegraphPrefab.GetComponentsInChildren<Image>();
        indicator = imagesInChildren[0];
        icon = imagesInChildren[1];

        indicator.color = this.settings.alertColor;
        icon.sprite = this.settings.alertSprite;
        
        rect = this.telegraphPrefab.GetComponent<RectTransform>();

        UpdateTelegrapher();
    }

    public void UpdateTelegrapher() {
        if(!update) {
            return;
        }
        float objectHeight = cam.WorldToViewportPoint(objectToTelegraph.transform.position).y;
        rect.position = new Vector3(objectToTelegraph.transform.position.x,rect.position.y,rect.position.z);


        Color color = settings.alertColor;
        color.a = Mathf.Clamp01(1f-(objectHeight-1f));
        indicator.color = color;
        indicator.transform.localScale = new Vector3(Mathf.Clamp01((objectHeight-1f)*0.5f + 0.5f ),1,1);

        if(objectHeight <= 1) {
            update = false;
            Hide();
        }
    }

    void Hide() {
        rect.DOMoveY(rect.position.y + 0.5f, 0.3f).SetEase(Ease.InOutCubic);
    }
}
