using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class FloatingTextController : MonoBehaviour, IPoolable
{
    [SerializeField] private TextMeshPro _text;
    [SerializeField] private float _floatDuration = 1f;
    [SerializeField] private float _floatDistance = 1f;
    [SerializeField] private float _fadeDuration = 0.5f;
    
    private Sequence floatSequence;
    
    public void OnSpawn()
    {
        _text.alpha = 1;
        floatSequence = DOTween.Sequence();
        floatSequence.Append(transform.DOMove(transform.position + Vector3.up * _floatDistance, _floatDuration).SetEase(Ease.OutQuad));
        floatSequence.Join(_text.DOFade(0, _fadeDuration).SetEase(Ease.InQuad)
            .SetDelay(_floatDuration - _fadeDuration));
        floatSequence.OnComplete(() => ObjectPoolManager.Instance.Despawn(this));
    }

    public void OnDespawn()
    {
        floatSequence?.Kill();
    }
    
    public void SetText(string prefix, string value)
    {
        _text.text = $"{prefix} {value}";
    }
}
