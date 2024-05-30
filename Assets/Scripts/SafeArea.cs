using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
    RectTransform rectTransform;
    Rect safeArea;
    Vector2 minAnchors;
    Vector2 maxAnchors;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        safeArea = Screen.safeArea;
        minAnchors = safeArea.position;
        maxAnchors = minAnchors + safeArea.size;

        minAnchors.x /= Screen.width;
        minAnchors.y /= Screen.height;
        maxAnchors.x /= Screen.width;
        maxAnchors.y /= Screen.height;

        rectTransform.anchorMin = minAnchors;
        rectTransform.anchorMax = maxAnchors;
    }
}
