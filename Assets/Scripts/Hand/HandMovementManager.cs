using DG.Tweening;
using UnityEngine;

public class HandMovementManager : MonoBehaviour
{
    [SerializeField] private GameObject _objectToMove;
    [SerializeField] private Transform _holdPoint;
    [SerializeField] private Collider2D _movementArea;
    [SerializeField] private LayerMask _movementLayer;
    [SerializeField] private float _scaleDuration = 0.5f;
    [SerializeField] private float _movementSpeed = 10f;

    private Camera _mainCamera;
    private GameObject _heldPropObject; // The currently held prop object
    private Prop _heldProp; // The currently held prop
    private bool _isHolding;
    private bool _canDrop = true;
    private bool _isInteractionAllowed = true;
    
    private void Start()
    {
        _mainCamera = Camera.main;
        PickUpProp();
        EventBus.Subscribe<UIStateChangedEvent>(OnUIStateChanged);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<UIStateChangedEvent>(OnUIStateChanged);
    }

    private void OnUIStateChanged(UIStateChangedEvent message)
    {
        _isInteractionAllowed = !message.IsOverlayActive;
    }

    private void Update()
    {
        if (!_isInteractionAllowed) return;

        if (TryGetInput(out var inputPosition, out var isInputEnded))
        {
            HandleMovement(inputPosition, isInputEnded);
        }
    }

    private static bool TryGetInput(out Vector2 inputPosition, out bool isInputEnded)
    {
        inputPosition = Vector2.zero;
        isInputEnded = false;

        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            inputPosition = touch.position;
            isInputEnded = touch.phase == TouchPhase.Ended;
            return true;
        }
        else if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
        {
            inputPosition = Input.mousePosition;
            isInputEnded = Input.GetMouseButtonUp(0);
            return true;
        }
        return false;
    }

    private void HandleMovement(Vector2 inputPosition, bool isInputEnded)
    {
        var worldPosition = _mainCamera.ScreenToWorldPoint(new Vector3(inputPosition.x, inputPosition.y, _mainCamera.nearClipPlane));;
        if (IsInMovementArea(worldPosition))
        {
            MoveObjectTo(worldPosition);
            if (isInputEnded && _isHolding && _canDrop)
            {
                DropProp();
            }
        }
    }

    private bool IsInMovementArea(Vector3 worldPosition)
    {
        var hit = Physics2D.Raycast(worldPosition, Vector2.zero, Mathf.Infinity, _movementLayer);
        return hit.collider != null && hit.collider == _movementArea;
    }

    private void MoveObjectTo(Vector3 worldPosition)
    {
        var targetPosition = new Vector3(worldPosition.x, _objectToMove.transform.position.y, _objectToMove.transform.position.z);
        var distance = Vector3.Distance(_objectToMove.transform.position, targetPosition);
        var duration = distance / _movementSpeed;

        _objectToMove.transform.DOMove(targetPosition, duration).SetEase(Ease.OutQuad);
    }

    private void PickUpProp()
    {
        (_heldPropObject, _heldProp) = PropSelector.Instance.GetNextProp(_holdPoint.position);
        var rb2d = _heldPropObject.GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 0;
        _heldPropObject.GetComponent<Collider2D>().enabled = false;
        _heldPropObject.transform.SetParent(_holdPoint);
        _isHolding = true;
        _canDrop = false;
        
        // Animate the prop scaling up
        var objectScale = _heldProp.Prefab.transform.localScale;
        _heldPropObject.transform.localScale = Vector3.zero;
        _heldPropObject.transform.DOScale(objectScale, _scaleDuration).SetEase(Ease.OutBack).OnComplete(() => _canDrop = true);
    }

    private void DropProp()
    {
        var rb2d = _heldPropObject.GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 1;
        _heldPropObject.GetComponent<Collider2D>().enabled = true;
        _heldPropObject.transform.SetParent(null);
        _heldPropObject = null;
        _isHolding = false;

        PickUpProp();
    }
}