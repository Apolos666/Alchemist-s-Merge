using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ScoreTextEffect
{
    private static readonly AnimationCurve DefaultAnimationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private int _currentValue;
    private CancellationTokenSource _animationCts;

    public async UniTask AnimateTextTo(
        int targetValue,
        TextMeshProUGUI targetText,
        float animationDuration = 0.5f,
        AnimationCurve animationCurve = null,
        CancellationToken cancellationToken = default)
    {
        // Cancel the previous animation if it's still running
        _animationCts?.Cancel();
        // Create a new CancellationTokenSource for the current animation
        _animationCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        try
        {
            if (animationCurve is null)
                animationCurve = DefaultAnimationCurve;

            await AnimateText(targetValue, targetText, animationDuration, animationCurve, _animationCts.Token);
        }
        catch (OperationCanceledException)
        {
            
        }
        finally
        {
            if (_animationCts != null)
            {
                _animationCts.Dispose();
                _animationCts = null;
            }
        }
    }

    private async UniTask AnimateText(
        int targetValue,
        TextMeshProUGUI targetText,
        float animationDuration,
        AnimationCurve animationCurve,
        CancellationToken cancellationToken)
    {
        var elapsedTime = 0f;
        var startValue = _currentValue;
        
        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            // Calculate the t value for the animation curve
            var t = animationCurve.Evaluate(elapsedTime / animationDuration);
            var currentNumber = Mathf.RoundToInt(Mathf.Lerp(startValue, targetValue, t));

            targetText.text = FormatNumber(currentNumber);

            await UniTask.Yield(cancellationToken);
        }
        
        _currentValue = targetValue;
        targetText.text = FormatNumber(targetValue);
    }

    private static string FormatNumber(int number)
    {
        return $"{number:N0}";
    }
}