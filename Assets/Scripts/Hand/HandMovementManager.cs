using UnityEngine;

public class HandMovementManager : MonoBehaviour
{
    [SerializeField] private GameObject _objectToMove;
    [SerializeField] private Transform _holdPoint;
    private Camera _mainCamera;
    private GameObject _heldProp;
    private bool _isHolding;

    [SerializeField] private Transform _leftBarrier;
    [SerializeField] private Transform _rightBarrier;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.touchCount <= 0) return;

        var touch = Input.GetTouch(0);

        var touchPosition = _mainCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, _mainCamera.nearClipPlane));
        
        var clampX = Mathf.Clamp(touchPosition.x, _leftBarrier.position.x, _rightBarrier.position.x);
        
        var newPosition = new Vector3(clampX, _objectToMove.transform.position.y, _objectToMove.transform.position.z);
        _objectToMove.transform.position = newPosition;

        if (touch.phase == TouchPhase.Began && !_isHolding)
        {
            PickUpProp();
        } else if (touch.phase == TouchPhase.Ended && _isHolding)
        {
            DropProp();
        }
    }
    
    private void PickUpProp()
    {
        _heldProp = PropSelector.Instance.GetNextProp(_holdPoint.position);
        _heldProp.transform.SetParent(_holdPoint);
        _isHolding = true;
    }

    private void DropProp()
    {
        _heldProp.GetComponent<Rigidbody2D>().gravityScale = 1;
        _heldProp.transform.SetParent(null);
        _heldProp = null;
        _isHolding = false;
    }
}