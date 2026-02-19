using UnityEngine;
using Zenject;

public abstract class DragAndDropManager<T> : BaseDragAndDropManager
{
    [Inject] private SignalBus _signalBus;

    private void OnEnable()
    {
        _signalBus.Subscribe<BeginDragSignal<T>>(OnBeginDrag);
        _signalBus.Subscribe<DragSignal<T>>(OnDrag);
        _signalBus.Subscribe<EndDragSignal<T>>(OnEndDrag);
    }

    private void OnDisable()
    {
        _signalBus.Unsubscribe<BeginDragSignal<T>>(OnBeginDrag);
        _signalBus.Unsubscribe<DragSignal<T>>(OnDrag);
        _signalBus.Unsubscribe<EndDragSignal<T>>(OnEndDrag);
    }

    protected virtual void OnBeginDrag(BeginDragSignal<T> signal)
    {
       
    }

    protected virtual void OnDrag(DragSignal<T> signal)
    {
    }

    protected virtual void OnEndDrag(EndDragSignal<T> signal)
    {
        
    }

    protected bool IsObjectFullyOnScreen(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer == null)
            return false;

        Camera camera = Camera.main;
        Bounds bounds = renderer.bounds;


        Vector3 minViewport = camera.WorldToViewportPoint(bounds.min);
        Vector3 maxViewport = camera.WorldToViewportPoint(bounds.max);

        bool inX = minViewport.x >= 0 && maxViewport.x <= 1;
        bool inY = minViewport.y >= 0 && maxViewport.y <= 1;

        bool inZ = minViewport.z > 0;

        return inX && inY && inZ;
    }
}

public abstract class BaseDragAndDropManager : MonoBehaviour
{
    
}