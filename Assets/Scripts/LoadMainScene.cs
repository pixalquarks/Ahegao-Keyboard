using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMainScene : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(LoadNextScene());
    }

    private IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(10);
        SceneManager.LoadScene(1);
    }
}
