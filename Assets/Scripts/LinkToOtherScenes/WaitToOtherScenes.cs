using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitToOtherScenes : MonoBehaviour
{
    public string sceneName;
    public float delay;
    public bool isTimerStarted = false;

    void Update()
    {
        if (isTimerStarted == true)
        {
            StartCoroutine("WaitToChangeScene");
        }
    }

    IEnumerator WaitToChangeScene()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
    
    public void SetIsTimerStarted(bool boolean)
    {
        isTimerStarted = boolean;
    }
}
