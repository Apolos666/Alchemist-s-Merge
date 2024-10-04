using DG.Tweening;
using UnityEngine;

public class RotatingButtonAnimator : ISettingsButtonAnimator
{
    private readonly Transform buttonTransform;
    private readonly float rotationDuration;
    private readonly float rotationAngle;
    private readonly Ease rotationEase;

    public RotatingButtonAnimator(Transform buttonTransform, float rotationDuration, float rotationAngle, Ease rotationEase)
    {
        this.buttonTransform = buttonTransform;
        this.rotationDuration = rotationDuration;
        this.rotationAngle = rotationAngle;
        this.rotationEase = rotationEase;
    }

    public void AnimateButtonClick()
    {
        buttonTransform.DOKill();
        buttonTransform.DORotate(new Vector3(0, 0, rotationAngle), rotationDuration, RotateMode.FastBeyond360)
            .SetEase(rotationEase)
            .SetRelative()
            .OnComplete(() => buttonTransform.DORotate(Vector3.zero, rotationDuration, RotateMode.FastBeyond360));
    }
}