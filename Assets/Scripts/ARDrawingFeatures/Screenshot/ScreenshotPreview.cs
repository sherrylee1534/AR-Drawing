using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ScreenshotPreview : MonoBehaviour
{
    [SerializeField]
    GameObject panel;
    string[] files = null;
    int whichScreenshotIsShown = 0;

    void Start()
    {
        files = Directory.GetFiles(Application.persistentDataPath + "/", "*.png");

        if (files.Length > 0)
        {
            ShowPicture();
        }
    }

    void ShowPicture()
    {
        string pathToFile = files[whichScreenshotIsShown];
        Texture2D texture = GetScreenshotImage(pathToFile);
        Sprite spr = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        panel.GetComponent<Image>().sprite = spr;
    }

    Texture2D GetScreenshotImage(string filePath)
    {
        Texture2D texture = null;
        byte[] fileBytes;

        if (File.Exists(filePath))
        {
            fileBytes = File.ReadAllBytes(filePath);
            texture = new Texture2D(2, 2, TextureFormat.RGB24, false);
            texture.LoadImage(fileBytes);
        }

        return texture;
    }
}
