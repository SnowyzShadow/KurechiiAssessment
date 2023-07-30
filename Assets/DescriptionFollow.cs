using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptionFollow : MonoBehaviour
{
    private float width;
    private float height;

    private Vector3 mousePosition;
    public float moveSpeed = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        width = GetComponent<RectTransform>().rect.width;
        height = GetComponent<RectTransform>().rect.height;
    }

    // Update is called once per frame
    void Update()
    {
        // Get the mouse position in screen coordinates
        Vector2 mousePositionScreen = Input.mousePosition;

        // Convert the mouse position from screen coordinates to canvas coordinates
        Vector2 mousePositionCanvas;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), mousePositionScreen, null, out mousePositionCanvas);

        // Set the image's anchored position to the mouse position within the canvas
        GetComponent<RectTransform>().anchoredPosition = mousePositionCanvas;
    }
}

