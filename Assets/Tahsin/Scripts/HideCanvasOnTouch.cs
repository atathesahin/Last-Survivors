using UnityEngine;

public class HideCanvasOnTouch : MonoBehaviour
{
    private Canvas canvas;

    void Start()
    {
        // Canvas bileþenini al
        canvas = GetComponent<Canvas>();
    }

    void Update()
    {
        // Ekrana dokunulduðunda Canvas'ý gizle
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (canvas != null)
            {
                canvas.enabled = false; // Canvas'ý devre dýþý býrak
            }
        }
    }
}
