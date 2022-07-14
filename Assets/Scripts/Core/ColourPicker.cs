using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColourPicker : MonoBehaviour
{
    public TextMeshProUGUI debugText;

    private RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        {
            #if !UNITY_EDITOR
            SelectOnTouch();
            #else
            SelectOnMouse();
            #endif
        }

    }

    void SelectOnTouch()
    {
    }

    void SelectOnMouse()
    {
        Vector2 delta;
        // Last parameter of the new Vector 3 is 0.0f because it is touch screen
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, null, out delta); // Pass result into delta vector we created
        string debug = "mousePosition = " + Input.mousePosition;
        debug += "<br>delta = " + delta;

        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;
        // Since delta is (0, 0) in the middle of the ColourChartImage, we half it so the bottom left corner is (0, 0)
        delta += new Vector2(width * 0.5f, height * 0.5f);
        debug += "<br>offset delta = " + delta;

        debugText.text = debug;
    }
}
