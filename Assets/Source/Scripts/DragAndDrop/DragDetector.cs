using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDetector : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public event Action BeginDrag;
    public event Action Drag;
    public event Action EndDrag;

    private ScrollRect _parentScroll;
    private Vector2 _pointerStartPos;
    private bool _isCubeDragging;
    private bool _isDecisionMade;

    private void Awake()
    {
        _parentScroll = GetComponentInParent<ScrollRect>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _pointerStartPos = eventData.position;
        _isDecisionMade = false;
        _isCubeDragging = false;
    }

    public void OnBeginDrag(PointerEventData eventData) 
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isDecisionMade)
        {
            Vector2 delta = eventData.position - _pointerStartPos;
            
            if (delta.magnitude < 10f) 
                return; 

            _isDecisionMade = true;

            bool isHorizontalSwipe = Mathf.Abs(delta.x) > Mathf.Abs(delta.y);
            
            bool isScrollDirection = (_parentScroll != null) && 
                                    ((_parentScroll.horizontal && isHorizontalSwipe) || 
                                     (_parentScroll.vertical && !isHorizontalSwipe));

            if (isScrollDirection)
            {
                _isCubeDragging = false;
                _parentScroll.OnBeginDrag(eventData);
            }
            else
            {
                _isCubeDragging = true;
                if (_parentScroll != null)
                {
                    _parentScroll.StopMovement();
                    _parentScroll.enabled = false;
                }
                BeginDrag?.Invoke();
            }
        }

        if (_isCubeDragging)
        {
            Drag?.Invoke();
        }
        else if (_parentScroll != null)
        {
            _parentScroll.OnDrag(eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_isCubeDragging)
        {
            EndDrag?.Invoke();
        }
        else if (_parentScroll != null)
        {
            _parentScroll.OnEndDrag(eventData);
        }

        if (_parentScroll != null)
        {
            _parentScroll.enabled = true;
        }

        _isDecisionMade = false;
        _isCubeDragging = false;
    }
}