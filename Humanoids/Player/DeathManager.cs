using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathManager : MonoBehaviour
{
    public GameObject sceneFadeOut;
    public GameObject text;
    public float fadeWait;


    public void PlayerDiedScreen() {
        text.SetActive(true);
        StartCoroutine(DeathScreenCoroutine());
    }


    public IEnumerator DeathScreenCoroutine() {
        if (sceneFadeOut) {
            Instantiate(sceneFadeOut, Vector3.zero, Quaternion.identity);
        }

        yield return new WaitForSeconds(fadeWait);
        AsyncOperation async = SceneManager.LoadSceneAsync("Dungeon");
        while (!async.isDone) {
            yield return null;
        }
    }

}
