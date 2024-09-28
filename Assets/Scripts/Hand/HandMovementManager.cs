﻿using DG.Tweening;
using UnityEngine;

public class HandMovementManager : MonoBehaviour
{
    [SerializeField] private GameObject _objectToMove;
    [SerializeField] private Transform _holdPoint;
    private Camera _mainCamera;
    private GameObject _heldPropObject;
    private Prop _heldProp;
    private bool _isHolding;
    private bool _canDrop = true;
    
    [SerializeField] private Collider2D _movementArea;
    [SerializeField] private LayerMask _movementLayer;
    [SerializeField] private float _scaleDuration = 0.5f;
    
    private void Start()
    {
        _mainCamera = Camera.main;
        PickUpProp();
    }

    private void Update()
    {
        if (Input.touchCount <= 0) return;

        var touch = Input.GetTouch(0);

        var touchPosition = _mainCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, _mainCamera.nearClipPlane));
        
        var hit = Physics2D.Raycast(touchPosition, Vector2.zero, Mathf.Infinity, _movementLayer);

        if (hit.collider != null && hit.collider == _movementArea)
        {
            var newPosition = new Vector3(touchPosition.x, _objectToMove.transform.position.y, _objectToMove.transform.position.z);
            _objectToMove.transform.position = newPosition;

            if (touch.phase == TouchPhase.Ended && _isHolding && _canDrop)
            {
                DropProp();
            }
        }
    }
    
    private void PickUpProp()
    {
        (_heldPropObject, _heldProp) = PropSelector.Instance.GetNextProp(_holdPoint.position);
        _heldPropObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        _heldPropObject.GetComponent<Collider2D>().enabled = false;
        var objectScale = _heldProp.Prefab.transform.localScale;
        _heldPropObject.transform.SetParent(_holdPoint);
        _isHolding = true;
        _canDrop = false;
        
        // Scale Effect
        _heldPropObject.transform.localScale = Vector3.zero;
        _heldPropObject.transform
            .DOScale(objectScale, _scaleDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() => _canDrop = true);
    }

    private void DropProp()
    {
        _heldPropObject.GetComponent<Rigidbody2D>().gravityScale = 1;
        _heldPropObject.transform.SetParent(null);
        _heldPropObject = null;
        _isHolding = false;
        
        PickUpProp();
    }
}