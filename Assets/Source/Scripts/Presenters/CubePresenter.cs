using System;
using UnityEngine;
using Zenject;

public class CubePresenter : IDisposable
{
    private CubeModel _cubeModel;
    private CubeView _cubeView;

    public CubeView CubeView => _cubeView;
    public CubeModel CubeModel => _cubeModel;

    [Inject] private SignalBus _signalBus;

    public CubePresenter(CubeModel cubeModel, CubeView cubeView, Sprite sprite)
    {
        _cubeModel = cubeModel;
        _cubeView = cubeView;
        _cubeView.UpdateSprite(sprite);

        if (_cubeView.DragDetector)
        {
            _cubeView.DragDetector.BeginDrag += OnBeginDrag;
            _cubeView.DragDetector.Drag += OnDrag;
            _cubeView.DragDetector.EndDrag += OnEndDrag;
        }
    }

    public void Dispose()
    {
        if (_cubeView.DragDetector)
        {
            _cubeView.DragDetector.BeginDrag -= OnBeginDrag;
            _cubeView.DragDetector.Drag -= OnDrag;
            _cubeView.DragDetector.EndDrag -= OnEndDrag;
        }
    }

    public void SavePosition()
    {
        CubeModel.PositionX = _cubeView.transform.position.x;
        CubeModel.PositionY = _cubeView.transform.position.y;
        CubeModel.PositionZ = _cubeView.transform.position.z;
    }

    private void OnBeginDrag()
    {
        _signalBus.Fire(new BeginDragSignal<CubePresenter>(this));
    }

    private void OnDrag()
    {
        _signalBus.Fire(new DragSignal<CubePresenter>(this));
    }

    private void OnEndDrag()
    {
        _signalBus.Fire(new EndDragSignal<CubePresenter>(this));
    }
}