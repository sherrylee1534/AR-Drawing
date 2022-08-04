using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActualScreenshot : MonoBehaviour
{
    public TakeScreenshotPress takeScreenshotPress;
    public bool hasTakenScreenshot = false;
    public GameObject screenshotFlashParent;

    [SerializeField]
    private GameObject screenshotFlash;
    
    [SerializeField]
    private GameObject screenshotPopUp;

    void Update()
    {
        if (takeScreenshotPress.isScreenshotButtonPressed && !hasTakenScreenshot)
        {
            StartCoroutine("Capture");
        }
    }

    IEnumerator Capture()
    {
        yield return null;

        screenshotPopUp.SetActive(false);

        string timeStamp = System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
        string fileName = "Screenshot" + timeStamp + ".png";
        string pathToSave = fileName;

        ScreenCapture.CaptureScreenshot(pathToSave);
        yield return new WaitForEndOfFrame();

        GameObject flash = Instantiate(screenshotFlash, new Vector2(0f, 0f), Quaternion.identity);
        flash.transform.parent = screenshotFlashParent.transform;
        flash.transform.localPosition = new Vector2(0f, 0f);

        Debug.Log("blink bitch");

        screenshotPopUp.SetActive(true);

        hasTakenScreenshot = true;
    }
}
