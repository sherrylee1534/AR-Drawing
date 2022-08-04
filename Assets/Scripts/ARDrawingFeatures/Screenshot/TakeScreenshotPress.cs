using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeScreenshotPress : MonoBehaviour
{
    public ActualScreenshot actualScreenshot;
    public bool isScreenshotButtonPressed;

    public void TakeScreenshotButton()
    {
        actualScreenshot.hasTakenScreenshot = false;
        isScreenshotButtonPressed = true;
    }
}
