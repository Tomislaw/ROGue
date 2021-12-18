using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private Coroutine loadingOperation = null;

    public void LoadScene(string scene)
    {
       
        if(loadingOperation == null)
            loadingOperation = StartCoroutine(LoadAsyncScene(scene));
    }

    IEnumerator LoadAsyncScene(string scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        loadingOperation = null;
    }
}
