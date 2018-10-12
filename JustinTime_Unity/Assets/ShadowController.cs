using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowController : MonoBehaviour
{
    public float minShadowWidth;
    public float maxShadowWidth;

    public float defaultDifference;
    public float maxDifference;

    public Color closeColor;
    public Color farColor;

    public GameObject target;

    SpriteRenderer spriteRenderer;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update() {
        transform.position = new Vector2(target.transform.position.x, -0.55f);

        float difference = Mathf.Abs(transform.position.y + defaultDifference - target.transform.position.y);
        float differencePercentage = Mathf.Clamp01(difference/maxDifference);
        spriteRenderer.color = Color.Lerp(closeColor, farColor, differencePercentage);
        transform.localScale = new Vector2(Mathf.Lerp(minShadowWidth, maxShadowWidth, differencePercentage), 10);
    }
}
