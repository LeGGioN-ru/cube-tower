using UnityEngine;
using Zenject;

public class CubeDragAndDropManager : DragAndDropManager<CubePresenter>
{
     [SerializeField] private PhantomCube _phantomCube;

    [Inject] private SignalBus _signalBus;
    [Inject] private NotificationManager _notificationManager;

    private void Start()
    {
        _phantomCube.gameObject.SetActive(false);
    }

    protected override void OnBeginDrag(BeginDragSignal<CubePresenter> signal)
    {
        _phantomCube.Initialize(signal.Element, signal.Element.CubeView.transform.position);
        _phantomCube.gameObject.SetActive(true);
    }

    protected override void OnDrag(DragSignal<CubePresenter> signal)
    {
        _phantomCube.transform.position = Utility.GetMousePosition();
    }

    protected override void OnEndDrag(EndDragSignal<CubePresenter> signal)
    {
        _phantomCube.SetEndPosition(Utility.GetMousePosition());

        if (IsObjectFullyOnScreen(_phantomCube.gameObject))
        {
            if (_phantomCube.TryGetPlaceableZone(out PlacableZone<PhantomCube> placeableZone))
            {
                placeableZone.PlaceElement(_phantomCube);
            }
        }
        else
        {
            _notificationManager.Notify(LocalizationKeys.Localization.FRAME_CONSTRAINT);
        }

        _phantomCube.gameObject.SetActive(false);
    }
}
