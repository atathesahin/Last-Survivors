using UnityEngine;

public class HideCanvasOnTouch : MonoBehaviour
{
    private Canvas canvas;

    void Start()
    {
        // Canvas bile�enini al
        canvas = GetComponent<Canvas>();
    }

    void Update()
    {
        // Ekrana dokunuldu�unda Canvas'� gizle
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (canvas != null)
            {
                canvas.enabled = false; // Canvas'� devre d��� b�rak
            }
        }
    }
}
