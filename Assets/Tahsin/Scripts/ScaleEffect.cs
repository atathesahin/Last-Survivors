using UnityEngine;

public class ScaleEffect : MonoBehaviour
{
    public float scaleSpeed = 2f;  // B�y�me/k���lme h�z�
    public float maxScale = 1.2f; // Maksimum boyut
    public float minScale = 0.8f; // Minimum boyut

    private RectTransform rectTransform;
    private bool scalingUp = true;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (scalingUp)
        {
            rectTransform.localScale += Vector3.one * scaleSpeed * Time.deltaTime;
            if (rectTransform.localScale.x >= maxScale)
            {
                scalingUp = false;
            }
        }
        else
        {
            rectTransform.localScale -= Vector3.one * scaleSpeed * Time.deltaTime;
            if (rectTransform.localScale.x <= minScale)
            {
                scalingUp = true;
            }
        }
    }
}
