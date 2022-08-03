using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitToOtherScenes : MonoBehaviour
{
    public string sceneName;

    void Update()
    {
        if (gameObject.activeInHierarchy == true)
        {
            StartCoroutine("WaitToChangeScene");
        }
    }

    IEnumerator WaitToChangeScene()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(sceneName);
    }
}
