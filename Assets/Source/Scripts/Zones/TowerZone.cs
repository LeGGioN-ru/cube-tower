using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class TowerZone : PlacableZone<PhantomCube>
{
    [SerializeField] private GameCubeView _template;
    [SerializeField] private Vector2 _detectionSize;
    [SerializeField] private LayerMask _zoneLayer;

    [Inject] private CubeFactory _cubeFactory;
    [Inject] private NotificationManager _notificationManager;
    [Inject] private IGameSaveManager _gameSaveManager;

    private TowerModel _towerModel = new TowerModel();
    private bool _isPlacing;

    public IReadOnlyList<CubePresenter> Presenters => _towerModel.Presenters;

    public override void PlaceElement(PhantomCube phantomCube)
    {
        if (_isPlacing || _towerModel.Contains(phantomCube.Presenter))
            return;

        _isPlacing = true;

        if (_towerModel.Count == 0)
        {
            _notificationManager.Notify(LocalizationKeys.Localization.FIRST_CUBE_PLACED);

            SpawnAndJump(
                phantomCube,
                phantomCube.StartPosition,
                phantomCube.EndPosition,
                OnSuccessJump
            );
        }
        else
        {
            GameCubeView lastView = _towerModel.Last().CubeView as GameCubeView;
            Bounds lastBounds = lastView.Collider2D.bounds;
            Vector2 checkPos = (Vector2)lastView.transform.position + Vector2.up * lastBounds.size.y;

            Vector3 targetPosition = GetNextStackPosition(lastView, lastBounds);

            if (Utility.TryGetOverlapComponent(checkPos, _detectionSize, _zoneLayer, out PhantomCube foundPhantom))
            {
                _notificationManager.Notify(LocalizationKeys.Localization.CUBE_PLACED);

                SpawnAndJump(
                    foundPhantom,
                    foundPhantom.StartPosition,
                    targetPosition,
                    OnSuccessJump
                );
            }
            else
            {
                _notificationManager.Notify(LocalizationKeys.Localization.CUBE_TOWER_MISS);

                SpawnAndJump(
                    phantomCube,
                    phantomCube.StartPosition,
                    phantomCube.EndPosition,
                    HandleFailedPlacement
                );
            }
        }
    }

    public void LoadPresenters(List<CubePresenter> presenters)
    {
        _towerModel.LoadPresenters(presenters);
    }

    private void SpawnAndJump(PhantomCube sourcePhantom, Vector3 startPos, Vector3 endPos, Action<CubePresenter> onComplete)
    {
        CubePresenter presenter = _cubeFactory.Create(
            _template,
            sourcePhantom.Presenter.CubeModel,
            transform
        );

        presenter.CubeView.transform.position = startPos;

        CubeAnimator.PlayJump(presenter.CubeView.transform, endPos)
            .OnComplete(() => onComplete?.Invoke(presenter));
    }

    private Vector3 GetNextStackPosition(GameCubeView lastView, Bounds lastBounds)
    {
        GameCubeView firstView = _towerModel[0].CubeView as GameCubeView;
        float firstCubeWidth = firstView.Collider2D.bounds.size.x;

        float minX = firstView.transform.position.x - firstCubeWidth * 0.5f;
        float maxX = firstView.transform.position.x + firstCubeWidth * 0.5f;

        float maxOffset = firstCubeWidth * 0.5f;
        float randomOffset = Random.Range(-maxOffset, maxOffset);

        float targetX = lastView.transform.position.x + randomOffset;
        targetX = Mathf.Clamp(targetX, minX, maxX);

        float targetY = lastView.transform.position.y + lastBounds.size.y;

        return new Vector3(targetX, targetY, lastView.transform.position.z);
    }

    private void HandleFailedPlacement(CubePresenter presenter)
    {
        _isPlacing = false;
        CubeAnimator.PlayFail(presenter.CubeView as GameCubeView, presenter.Dispose);
    }

    private void OnSuccessJump(CubePresenter presenter)
    {
        _towerModel.Add(presenter);
        _gameSaveManager.SaveProgress();
        _isPlacing = false;
    }

    public void RemoveElement(CubePresenter presenter)
    {
        int indexToRemove = _towerModel.IndexOf(presenter);
        GameCubeView gameCubeView = presenter.CubeView as GameCubeView;

        if (gameCubeView == null || indexToRemove == -1)
            return;

        float heightGap = gameCubeView.Collider2D.bounds.size.y;

        _towerModel.Remove(presenter);
        presenter.Dispose();

        CubeAnimator.PlayRemove(gameCubeView);

        if (indexToRemove >= _towerModel.Count)
        {
            _gameSaveManager.SaveProgress();
            return;
        }

        ShiftStackDown(indexToRemove, heightGap);
    }

    private void ShiftStackDown(int startIndex, float dropAmount)
    {
        if (startIndex >= _towerModel.Count)
            return;

        Sequence shiftSequence = DOTween.Sequence();

        for (int i = startIndex; i < _towerModel.Count; i++)
        {
            CubeView cubeView = _towerModel[i].CubeView;

            if (cubeView != null)
            {
                float targetY = cubeView.transform.position.y - dropAmount;
                shiftSequence.Join(CubeAnimator.PlayShift(cubeView.transform, targetY));
            }
        }

        shiftSequence.OnComplete(() => _gameSaveManager.SaveProgress());
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_towerModel == null || _towerModel.Count == 0) return;

        GameCubeView lastView = _towerModel.Last().CubeView as GameCubeView;
        if (lastView == null) return;

        Bounds lastBounds = lastView.Collider2D.bounds;
        Vector3 centerPos = lastView.transform.position + Vector3.up * lastBounds.size.y;

        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawCube(centerPos, _detectionSize);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(centerPos, _detectionSize);

        GameCubeView firstView = _towerModel[0].CubeView as GameCubeView;
        if (firstView != null)
        {
            float firstWidth = firstView.Collider2D.bounds.size.x;
            float maxOffset = firstWidth * 0.5f;

            Gizmos.color = Color.red;

            Vector3 leftLimit = new Vector3(lastView.transform.position.x - maxOffset, centerPos.y, centerPos.z);
            Vector3 rightLimit = new Vector3(lastView.transform.position.x + maxOffset, centerPos.y, centerPos.z);

            Gizmos.DrawLine(leftLimit, rightLimit);
            Gizmos.DrawLine(leftLimit + Vector3.up * 0.2f, leftLimit + Vector3.down * 0.2f);
            Gizmos.DrawLine(rightLimit + Vector3.up * 0.2f, rightLimit + Vector3.down * 0.2f);
        }
    }
#endif
}
