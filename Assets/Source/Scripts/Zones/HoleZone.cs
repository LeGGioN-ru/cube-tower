using UnityEngine;
using DG.Tweening;
using Zenject;

public class HoleZone : PlacableZone<PhantomCube>
{
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private TowerZone _towerZone;

    [Header("Animation Settings")]
    [SerializeField] private float _duration = 0.5f;
    [SerializeField] private float _dropDistance = 1.5f;
    [SerializeField] private Vector3 _rotationAmount = new Vector3(0, 0, 360);

    [Inject] private NotificationManager _notificationManager;

    private Vector3 _initialPos;
    private Quaternion _initialRot;

    private void Awake()
    {
        if (_renderer != null)
        {
            _initialPos = _renderer.transform.localPosition;
            _initialRot = _renderer.transform.localRotation;
            _renderer.gameObject.SetActive(false);
        }
    }

    public override void PlaceElement(PhantomCube element)
    {
        _notificationManager.Notify(LocalizationKeys.Localization.CUBE_DELETED);

        _towerZone.RemoveElement(element.Presenter);
        _renderer.transform.DOKill();

        _renderer.transform.localPosition = _initialPos;
        _renderer.transform.localRotation = _initialRot;
        _renderer.sprite = element.Presenter.CubeView.CurrentSprite;
        _renderer.gameObject.SetActive(true);

        Sequence dropSequence = DOTween.Sequence();

        dropSequence.Join(_renderer.transform
            .DOLocalRotate(_rotationAmount, _duration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear));

        dropSequence.Join(_renderer.transform
            .DOLocalMove(_initialPos - new Vector3(0, _dropDistance, 0), _duration)
            .SetEase(Ease.InQuad));

        dropSequence.OnComplete(() =>
        {
            _renderer.gameObject.SetActive(false);
            _renderer.transform.localPosition = _initialPos;
            _renderer.transform.localRotation = _initialRot;
        });
    }

    private void OnDestroy()
    {
        if (_renderer != null)
            _renderer.transform.DOKill();
    }
}