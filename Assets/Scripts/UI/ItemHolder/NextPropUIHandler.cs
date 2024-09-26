using UnityEngine;
using UnityEngine.UI;

public class NextPropUIHandler : MonoBehaviour
{
    [SerializeField] private Image _propImage;

    private void Start()
    {
        UpdatePropUI();
    }

    private void Update()
    {
        UpdatePropUI();
    }

    private void UpdatePropUI()
    {
        if (PropSelector.Instance.CurrentProp is null) return;
        
        _propImage.sprite = PropSelector.Instance.CurrentProp.Icon;
    }
}
