using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapePicker : MonoBehaviour
{
    private DrawRectangle drawRectangle;
    public bool RectangleSelected { get; set; }
    private bool SquareSelected { get; set; }
    private bool CircleSelected { get; set; }
    private bool TriangleSelected { get; set; }
    private bool CubeSelected { get; set; }
    private bool CylinderSelected { get; set; }
    private static int shapeNumber = 0;

    void Start()
    {
        drawRectangle = GetComponent<DrawRectangle>();
    }

    void Update()
    {
        // #if (!UNITY_EDITOR)
        // SelectOnTouch();
        // #else
        // SelectOnMouse();
        // #endif

        SelectOnTouchOrMouse();
    }

    void SelectOnTouch()
    {
        switch (shapeNumber)
        {
            case (0):
                Debug.Log("selected shape is: " + shapeNumber);
                break;

            case (1):
                Debug.Log("selected shape is: " + shapeNumber);
                break;

            case (2):
                Debug.Log("selected shape is: " + shapeNumber);
                break;

            case (3):
                Debug.Log("selected shape is: " + shapeNumber);
                break;
                
            case (4):
                Debug.Log("selected shape is: " + shapeNumber);
                break;
            
            case (5):
                Debug.Log("selected shape is: " + shapeNumber);
                break;
                
            case (6):
                Debug.Log("selected shape is: " + shapeNumber);
                break;
            
            default:
                Debug.Log("selected shape is: " + shapeNumber);
                break;
        }
    }

    void SelectOnMouse()
    {
        switch (shapeNumber)
        {
            case (0):
                GrowAndShrink();
                Rotate();
                Debug.Log("selected shape is: " + shapeNumber);
                break;

            case (1):
                Debug.Log("selected shape is: " + shapeNumber);
                break;

            case (2):
                Debug.Log("selected shape is: " + shapeNumber);
                break;

            case (3):
                Debug.Log("selected shape is: " + shapeNumber);
                break;
                
            case (4):
                Debug.Log("selected shape is: " + shapeNumber);
                break;
            
            case (5):
                Debug.Log("selected shape is: " + shapeNumber);
                break;
                
            case (6):
                Debug.Log("selected shape is: " + shapeNumber);
                break;
            
            default:
                Debug.Log("selected shape is: " + shapeNumber);
                break;
        }
    }
    
        void SelectOnTouchOrMouse()
    {
        switch (shapeNumber)
        {
            case (0):
                Debug.Log("selected shape is: " + shapeNumber);
                break;

            case (1):
                drawRectangle.DrawingRectangle();
                Debug.Log("selected shape is: " + shapeNumber);
                break;

            case (2):
                Debug.Log("selected shape is: " + shapeNumber);
                break;

            case (3):
                Debug.Log("selected shape is: " + shapeNumber);
                break;
                
            case (4):
                Debug.Log("selected shape is: " + shapeNumber);
                break;
            
            case (5):
                Debug.Log("selected shape is: " + shapeNumber);
                break;
                
            case (6):
                Debug.Log("selected shape is: " + shapeNumber);
                break;
            
            default:
                Debug.Log("selected shape is: " + shapeNumber);
                break;
        }
    }

    public void SelectRectangle(bool isRectangleSelected)
    {
        RectangleSelected = isRectangleSelected;
        shapeNumber = 1;
        Debug.Log("selected shape is: " + shapeNumber);
    }

    public void SelectSquare(bool isSquareSelected)
    {
        SquareSelected = isSquareSelected;
        shapeNumber = 2;
        Debug.Log("selected shape is: " + shapeNumber);
    }

    public void SelectCircle(bool isCircleSelected)
    {
        CircleSelected = isCircleSelected;
        shapeNumber = 3;
        Debug.Log("selected shape is: " + shapeNumber);
    }

    public void SelectTriangle(bool isTriangleSelected)
    {
        TriangleSelected = isTriangleSelected;
        shapeNumber = 4;
        Debug.Log("selected shape is: " + shapeNumber);
    }

    public void SelectCube(bool isCubeSelected)
    {
        CubeSelected = isCubeSelected;
        shapeNumber = 5;
        Debug.Log("selected shape is: " + shapeNumber);
    }

    public void SelectCylinder(bool isCylinderSelected)
    {
        CylinderSelected = isCylinderSelected;
        shapeNumber = 6;
        Debug.Log("selected shape is: " + shapeNumber);
    }

    private void DrawSquare()
    {

    }

    private void GrowAndShrink()
    {

    }

    private void Rotate()
    {

    }
}
