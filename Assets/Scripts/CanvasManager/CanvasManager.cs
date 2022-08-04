using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//create screens we need
public enum CanvasType
{
    Landing,
    SelectLanguage,
    HomePage,
    ProfilePage,
    TravellingExhibition,
    TravellingExhibitionExpand,
    ScanPage
}

public class CanvasManager : SingletonUI<CanvasManager>
{
    List<CanvasController> canvasControllerList;
    CanvasController lastActiveCanvas;

    protected override void Awake()
    {
        canvasControllerList = GetComponentsInChildren<CanvasController>().ToList();
        canvasControllerList.ForEach(x => x.gameObject.SetActive(false));
        SwitchCanvas(CanvasType.SelectLanguage);

        canvasControllerList.ForEach(x => x.gameObject.SetActive(false));
        CanvasController homeController = canvasControllerList.Find(x => x.canvasType == CanvasType.SelectLanguage);

        if (homeController != null)
        {
            homeController.gameObject.SetActive(true);
        }

        else
        {
            Debug.LogWarning("The main menu canvas was not found!");
        }
    }
    
    public void SwitchCanvas(CanvasType _type)
    {
        // First need to deactivate the last canvas -> find last canvas
        if (lastActiveCanvas != null)
        {
            lastActiveCanvas.gameObject.SetActive(false);
        }

        CanvasController desiredCanvas = canvasControllerList.Find(x => x.canvasType == _type);

        if (desiredCanvas != null)
        {
            desiredCanvas.gameObject.SetActive(true);
        }

        else
        {
            Debug.LogWarning("The desired canvas was not found!");
        }
    }
}
