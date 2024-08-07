using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gem : Collectable
{
    public override void Collect()
    {
        StartCoroutine(LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1));
    }
    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

}
