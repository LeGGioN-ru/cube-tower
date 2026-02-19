using System;
using System.Collections.Generic;
using System.Linq;
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

    private List<CubePresenter> _presenters = new List<CubePresenter>();

    public IReadOnlyList<CubePresenter> Presenters => _presenters;

    public override void PlaceElement(PhantomCube phantomCube)
    {
        if (_presenters.Contains(phantomCube.Presenter))
            return;

        if (_presenters.Count == 0)
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
            CubeView lastCubeView = _presenters.Last().CubeView;
            Bounds lastBounds = lastCubeView.GetComponent<Collider2D>().bounds;
            Vector2 checkPos = (Vector2)lastCubeView.transform.position + Vector2.up * lastBounds.size.y;

            Vector3 targetPosition = GetNextStackPosition(lastCubeView, lastBounds);

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
        _presenters = presenters;
    }

    private void SpawnAndJump(PhantomCube sourcePhantom, Vector3 startPos, Vector3 endPos, Action<CubePresenter> onComplete)
    {
        CubePresenter presenter = _cubeFactory.Create(
            _template,
            sourcePhantom.Presenter.CubeModel,
            transform
        );

        presenter.CubeView.transform.position = startPos;

        presenter.CubeView.transform
            .DOJump(endPos, 4, 1, 0.5f)
            .OnComplete(() => onComplete?.Invoke(presenter));
    }

    private Vector3 GetNextStackPosition(CubeView lastView, Bounds lastBounds)
    {
        float firstCubeWidth = _presenters[0].CubeView.GetComponent<Collider2D>().bounds.size.x;
        float maxOffset = firstCubeWidth * 0.5f;
        float randomOffset = Random.Range(-maxOffset, maxOffset);

        float targetX = lastView.transform.position.x + randomOffset;
        float targetY = lastView.transform.position.y + lastBounds.size.y;

        return new Vector3(targetX, targetY, lastView.transform.position.z);
    }

    private void HandleFailedPlacement(CubePresenter presenter)
    {
        SpriteRenderer spriteRenderer = presenter.CubeView.GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.DOFade(0f, 0.5f).OnComplete(() => { Destroy(presenter.CubeView.gameObject); });
        }
        else
        {
            Destroy(presenter.CubeView.gameObject);
        }
    }

    private void OnSuccessJump(CubePresenter presenter)
    {
        _presenters.Add(presenter);
        _gameSaveManager.SaveProgress();
    }

    public void RemoveElement(CubePresenter presenter)
    {
        int indexToRemove = _presenters.IndexOf(presenter);

        GameCubeView gameCubeView = presenter.CubeView as GameCubeView;

        if (gameCubeView == null || indexToRemove == -1)
        {
            return;
        }

        float heightGap = 0f;

        heightGap = gameCubeView.Collider2D.bounds.size.y;

        _presenters.RemoveAt(indexToRemove);

        gameCubeView.transform.DOKill();
        Destroy(gameCubeView.gameObject);

        if (indexToRemove >= _presenters.Count)
            return;

        ShiftStackDown(indexToRemove, heightGap);
    }

    private void ShiftStackDown(int startIndex, float dropAmount)
    {
        if (startIndex >= _presenters.Count) return;

        Sequence shiftSequence = DOTween.Sequence();

        for (int i = startIndex; i < _presenters.Count; i++)
        {
            var upperCube = _presenters[i].CubeView;

            if (upperCube != null)
            {
                upperCube.transform.DOKill();

                float targetY = upperCube.transform.position.y - dropAmount;

                shiftSequence.Join(upperCube.transform
                    .DOMoveY(targetY, 0.4f)
                    .SetEase(Ease.OutBounce));
            }
        }

        shiftSequence.OnComplete(() => _gameSaveManager.SaveProgress());
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_presenters == null || _presenters.Count == 0) return;

        var lastView = _presenters.Last().CubeView;
        if (lastView == null) return;

        Bounds lastBounds = lastView.GetComponent<Collider2D>().bounds;
        Vector3 centerPos = lastView.transform.position + Vector3.up * lastBounds.size.y;

        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawCube(centerPos, _detectionSize);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(centerPos, _detectionSize);

        var firstView = _presenters[0].CubeView;
        if (firstView != null)
        {
            float firstWidth = firstView.GetComponent<Collider2D>().bounds.size.x;
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