using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawRectangle : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector2 initialMousePosition, currentMousePosition;

    [SerializeField]
    private Camera arCamera = null;

    [SerializeField]
    private LineSettings lineSettings = null;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
    }

    public void DrawingRectangle()
    {
        Vector3 mousePosition = arCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, lineSettings.distanceFromCamera));

        if (Input.GetMouseButtonDown(0))
        {
            lineRenderer.positionCount = 4;

            for (int i = 0; i < 4; i++)
            {
                lineRenderer.SetPosition(i, new Vector2(mousePosition.x, mousePosition.y));
                lineRenderer.gameObject.tag = "Line";
            }
            // lineRenderer.SetPosition(0, new Vector2(mousePosition.x, mousePosition.y));
            // lineRenderer.SetPosition(1, new Vector2(mousePosition.x, mousePosition.y));
            // lineRenderer.SetPosition(2, new Vector2(mousePosition.x, mousePosition.y));
            // lineRenderer.SetPosition(3, new Vector2(mousePosition.x, mousePosition.y));
            
            //initialMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //     lineRenderer.SetPosition(0, new Vector2(initialMousePosition.x, initialMousePosition.y));
        //     lineRenderer.SetPosition(1, new Vector2(initialMousePosition.x, initialMousePosition.y));
        //     lineRenderer.SetPosition(2, new Vector2(initialMousePosition.x, initialMousePosition.y));
        //     lineRenderer.SetPosition(3, new Vector2(initialMousePosition.x, initialMousePosition.y));
        }

        if (Input.GetMouseButton(0))
        {
            currentMousePosition = arCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, lineSettings.distanceFromCamera));
            // lineRenderer.SetPosition(0, new Vector2(initialMousePosition.x, initialMousePosition.y));
            // lineRenderer.SetPosition(1, new Vector2(initialMousePosition.x, initialMousePosition.y));
            // lineRenderer.SetPosition(2, new Vector2(initialMousePosition.x, initialMousePosition.y));
            // lineRenderer.SetPosition(3, new Vector2(initialMousePosition.x, initialMousePosition.y));
            lineRenderer.SetPosition(0, new Vector2(mousePosition.x, mousePosition.y));
            lineRenderer.SetPosition(1, new Vector2(mousePosition.x, currentMousePosition.y));
            lineRenderer.SetPosition(2, new Vector2(currentMousePosition.x, currentMousePosition.y));
            lineRenderer.SetPosition(3, new Vector2(currentMousePosition.x, mousePosition.y));
        }
    }
}
