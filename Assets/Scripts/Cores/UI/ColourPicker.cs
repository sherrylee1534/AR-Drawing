using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;

[Serializable]
public class ColourEvent : UnityEvent<Color> { }

public class ColourPicker : MonoBehaviour
{
    public TextMeshProUGUI debugText;
    public static Color selectedColour; // Default selectedColour is white. Handled in ARLine.cs
    public static bool isColourSelected;
    public ColourEvent OnColourSelect;

    private RectTransform rectTransform;
    private Texture2D colourChartImageTexture;
    private int xRectOffset = -20;
    private int yRectOffset = 20;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        colourChartImageTexture = GetComponent<Image>().mainTexture as Texture2D;
    }

    // Update is called once per frame
    void Update()
    {
        SelectOnTouchOrMouse();
    }

    void SelectOnTouchOrMouse()
    {
        // Check that we are in ColourChartImage
        if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition))
        {
            // Coordinates of screen -> Coordinates of ColourChartImage
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, null, out localPoint); // Pass result into localPoint vector we created
            string debug = "mousePosition = " + Input.mousePosition;
            debug += "<br>localPoint = " + localPoint;

            float width = rectTransform.rect.width;
            float height = rectTransform.rect.height;

            // In inspector, Rect Transform has (top, centre) as pivot, Pos Y of -200, and Width:Height = 1000:750
            // Hence, to make width and height proportionate, and move localPoint (0, 0) to bottom left corner,
            // we use ((width + xRectOffset) * 0.5f) and ((height + 200 + yRectOffset) * 0.75f)
            localPoint += new Vector2(((width + xRectOffset) * 0.5f), ((height + 200 + yRectOffset) * 0.75f));
            debug += "<br>offset localPoint = " + localPoint;

            // Coordinates of screen -> Coordinates of ColourChartImage -> Coordinates from (0, 1)
            // Normalise x and y in the range (0, 1)
            float x = Mathf.Clamp(localPoint.x / width, 0.0f, 1.0f);
            float y = Mathf.Clamp(localPoint.y / height, 0.0f, 1.0f);
            debug += "<br>x = " + x + ", y = " + y;

            // Coordinates of screen -> Coordinates of ColourChartImage -> Coordinates from (0, 1) -> Coordinates within texture
            int xTex = Mathf.RoundToInt(x * colourChartImageTexture.width);
            int yTex = Mathf.RoundToInt(y * colourChartImageTexture.height);
            debug += "<br>xTex = " + xTex + ", yTex = " + yTex;

            Color32 colour = colourChartImageTexture.GetPixel(xTex, yTex);

            // Visualise our debug texts
            debugText.color = colour;
            debugText.text = debug;

            SelectColour(colour);
        }
    }

    void SelectColour(Color colour)
    {
        if (Input.GetMouseButton(0))
        {
            OnColourSelect?.Invoke(colour);
            selectedColour = colour;
            isColourSelected = true;
        }
    }
}
