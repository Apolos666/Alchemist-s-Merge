using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveGameAndLoadScene : MonoBehaviour
{
    public Image fadeOverlay;
    public float fadeDuration = 1f;

    private void Start()
    {
        if (fadeOverlay != null)
        {
            fadeOverlay.gameObject.SetActive(true);
            fadeOverlay.color = Color.clear;
        }
    }

    public void LoadSceneWithTransition(string sceneName)
    {
        StartCoroutine(FadeAndLoadScene(sceneName));
    }

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        SaveSystem.Instance.SaveGame();
        
        // Fade out
        yield return fadeOverlay.DOFade(1f, fadeDuration).SetEase(Ease.InOutQuad).WaitForCompletion();

        // Load the new scene
        SceneManager.LoadScene(sceneName);
    }
}