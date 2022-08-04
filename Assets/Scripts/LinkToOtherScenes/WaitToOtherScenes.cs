using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitToOtherScenes : MonoBehaviour
{
    public string sceneName;
    public float delay;

    void Update()
    {
        if (gameObject.activeInHierarchy == true)
        {
            StartCoroutine("WaitToChangeScene");
        }
    }

    IEnumerator WaitToChangeScene()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
