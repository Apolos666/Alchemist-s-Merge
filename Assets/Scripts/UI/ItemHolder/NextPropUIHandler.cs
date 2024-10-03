using System;
using UnityEngine;
using UnityEngine.UI;

public class NextPropUIHandler : MonoBehaviour
{
    [SerializeField] private Image _propImage;

    private void Start()
    {
        EventBus.Subscribe<NextPropReadyEvent>(UpdatePropUI);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<NextPropReadyEvent>(UpdatePropUI);
    }

    private void UpdatePropUI(NextPropReadyEvent message)
    {
        _propImage.sprite = message.Prop.Icon;
    }
}