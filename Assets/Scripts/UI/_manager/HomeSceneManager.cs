using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeSceneManager : MonoBehaviour
{
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private string gameplayScene = "Gameplay";
    
    [SerializeField] private Image fadeOverlay;
    [SerializeField] private float fadeDuration = 1f;

    private void Start()
    {
        newGameButton.onClick.AddListener(StartNewGame);
        continueButton.onClick.AddListener(ContinueGame);
        
        continueButton.interactable = SaveSystem.Instance.HasSaveData();
        
        if (fadeOverlay != null)
        {
            fadeOverlay.gameObject.SetActive(true);
            fadeOverlay.color = Color.clear;
        }
    }

    private void StartNewGame()
    {
        PlayerPrefs.SetInt("IsNewGame", 1);
        PlayerPrefs.Save();
        StartCoroutine(FadeAndLoadScene(gameplayScene));
    }

    private void ContinueGame()
    {
        PlayerPrefs.SetInt("IsNewGame", 0);
        PlayerPrefs.Save();
        StartCoroutine(FadeAndLoadScene(gameplayScene));
    }
    
    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        yield return fadeOverlay.DOFade(1f, fadeDuration).SetEase(Ease.InOutQuad).WaitForCompletion();

        SceneManager.LoadScene(sceneName);
    }
}