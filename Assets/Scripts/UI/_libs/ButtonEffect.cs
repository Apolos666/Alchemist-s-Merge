using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEffect : MonoBehaviour
{
    public Button targetButton;
    public float scaleDuration = 0.2f;
    public float scaleMultiplier = 1.2f;

    private void Start()
    {
        if (targetButton == null)
        {
            targetButton = GetComponent<Button>();
        }

        targetButton.onClick.AddListener(PlayButtonEffect);
    }

    private void PlayButtonEffect()
    {
        transform.DOScale(Vector3.one * scaleMultiplier, scaleDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => 
            {
                transform.DOScale(Vector3.one, scaleDuration)
                    .SetEase(Ease.InQuad);
            });
    }
}